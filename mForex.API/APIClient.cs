using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using mForex.API.Exceptions;
using mForex.API.Packets;
using mForex.API.Utils;

namespace mForex.API
{
    /// <summary>
    /// 
    /// </summary>
    public class APIClient : ITradeProvider
    {
        private int reqId;
        private Dictionary<int, object> tcsByRequest;
        private Dictionary<int, Type> tcsTypes;
        private object tcsMutex;
        private ITimer heartBeatTimer;
        private const int majorProtocolVersion = 0;
        private const int minorProtocolVersion = 97;

        private Action<APINetworkPacket>[] packetHandlers;

        private IApiConnection apiConnection;

        /// <summary>
        /// Provides interface for managing orders.
        /// </summary>
        public ITradeProvider Trade { get { return this; } }

        /// <summary>
        /// Occurs when new ticks are received from the server.
        /// </summary>
        public event Action<Tick[]> Ticks;

        /// <summary>
        /// Occurs when margin level is updated. 
        /// However, this cannot occur more often than once per second.
        /// </summary>
        /// <remarks>This event is fired only when packet with id 514 is received. 
        /// If margin level was requested explicitly, this even will not be fired.
        /// </remarks>
        public event Action<MarginLevel> Margin;

        /// <summary>
        /// Occurs when client is disconnected from server with exception.
        /// </summary>
        public event Action<Exception> Disconnected;

        /// <summary>
        /// Occurs when trade is updated.
        /// <remarks>
        /// Trade is marked as updated if it has been opened, closed or modified.
        /// </remarks>
        /// </summary>
        public event Action<TradeUpdatePacket> TradeUpdate;

        /// <summary>
        /// Initialises a new instance the api network client class. 
        /// </summary>
        /// <param name="apiConnection"></param>
        /// <param name="timer"></param>       
        public APIClient(IApiConnection apiConnection, ITimer timer)
        {
            this.apiConnection = apiConnection;
            this.apiConnection.PacketReceived += HandlePacket;
            this.apiConnection.Disconnected += OnDisconnected;
            this.heartBeatTimer = timer;

            Initialise();
        }
        public APIClient(IApiConnection apiConnection)
            : this(apiConnection, new EventTimer())
        {
        }

        private void Initialise()
        {
            this.reqId = 0;
            this.tcsByRequest = new Dictionary<int, object>();
            this.tcsTypes = new Dictionary<int, Type>();
            this.tcsMutex = new object();
            this.packetHandlers = new Action<APINetworkPacket>[1024];
            InitialiseHandlers();
            StartHeartBeatTimer();
        }

        /// <summary>
        /// Initialises a connection to the server. 
        /// Returned task is signaled when connection is established.
        /// </summary>
        /// <remarks>
        /// Connect method can be called only once. If client has been disconnected, 
        /// new instance must be created to reconnect.
        /// </remarks>
        public async Task Connect()
        {
            await apiConnection.Connect();
        }

        /// <summary>
        /// Closes the client connection and releases any resources. 
        /// </summary>
        public void Disconnect()
        {
            apiConnection.Disconnect();
        }

        /// <summary>
        /// Sends login request to the server.
        /// </summary>
        /// <param name="login">User Login</param>
        /// <param name="password">User password</param>
        /// <remarks>This is the only legal packet which can be sent after connection is established.</remarks>
        /// <returns>A task that represents login process. The value of the result contains returned packet, if
        /// client was successfully authenticated; or an exception if an authentication failed.</returns>
        public Task<LoginResponsePacket> Login(int login, string password)
        {
            return HandleRequestPacket<LoginResponsePacket>(reqId => new LoginRequestPacket(reqId, login, password, majorProtocolVersion, minorProtocolVersion));
        }

        /// <summary>
        /// Registers for tick stream for a given instrument
        /// </summary>
        /// <param name="symbol">Symbol that is being registered for</param>
        /// <param name="action">Register for/Unregister from streaming</param>
        /// <returns>A task that represents tick registration process. The value of the result contains returnet packet.</returns>
        public Task<TickRegistrationResponsePacket> RequestTickRegistration(string symbol, RegistrationAction action)
        {
            return HandleRequestPacket<TickRegistrationResponsePacket>(reqId => new TickRegistrationRequestPacket(reqId, symbol, action));
        }

        /// <summary>
        /// Sends request to the server for candles from a given period. 
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="period"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns>A task that represents candle query process. The value of the result contains returnet packet.</returns>
        public Task<CandleResponsePacket> RequestCandles(string symbol, CandlePeriod period, DateTime from, DateTime to)
        {
            return HandleRequestPacket<CandleResponsePacket>(reqId => new CandleRequestPacket(reqId, symbol, period, from, to));
        }

        /// <summary>
        /// Sends request to the server for trading hours for particular instrument.
        /// </summary>
        /// <returns>A task that represents instrument settings query process. The value of the result contains returnet packet.</returns>
        public Task<SessionScheduleResponsePacket> RequestSessions(string symbol)
        {
            return HandleRequestPacket<SessionScheduleResponsePacket>(reqId => new SessionScheduleRequestPacket(reqId, symbol));
        }

        /// <summary>
        /// Sends request to the server for closed transactions from a given period. 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns>A task that represents trades history query process. The value of the result contains returnet packet.</returns>
        public Task<ClosedTradesResponsePacket> RequestTradesHistory(DateTime from, DateTime to)
        {
            return HandleRequestPacket<ClosedTradesResponsePacket>(reqId => new ClosedTradesRequestPacket(reqId, from, to));
        }

        /// <summary>
        /// Sends request to the server for settings for all available instruments.
        /// </summary>
        /// <returns>A task that represents instrument settings query process. The task's result contains response packet.</returns>
        public Task<InstrumentSettingsResponsePacket> RequestInstrumentSettings()
        {
            return HandleRequestPacket<InstrumentSettingsResponsePacket>(reqId => new InstrumentSettingsRequestPacket(reqId));
        }

        /// <summary>
        /// Sends request to the server for logged in user account settings.
        /// </summary>
        /// <returns>A task that represents account settings query process. The task's result contains response packet </returns>
        public Task<AccountSettingsResponsePacket> RequestAccountSettings()
        {
           return HandleRequestPacket<AccountSettingsResponsePacket>(reqId => new AccountSettingsRequestPacket(reqId));          
        }

        /// <summary>
        /// Sends request to the server for current margin level.
        /// </summary>
        /// <returns>A task that represents margin query process. The value of the result contains returnet packet.</returns>
        public Task<MarginLevelResponsePacket> RequestMarginLevel()
        {
            return HandleRequestPacket<MarginLevelResponsePacket>(reqId => new MarginLevelRequestPacket(reqId));
        }

        /// <summary>
        /// Send request to the server for open transactions.
        /// </summary>
        /// <returns>A task that represents open orders query process. The value of the result contains returnet packet.</returns>
        public Task<TradesInfoResponsePacket> RequestOpenTrades()
        {
            return HandleRequestPacket<TradesInfoResponsePacket>(reqId => new TradesInfoRequestPacket(reqId));
        }

        Task<TradeTransResponsePacket> ITradeProvider.OpenOrder(string symbol, TradeCommand tradeCommand,
            double price, double stopLoss, double takeProfit, double volume)
        {
            return ((ITradeProvider)this).OpenOrder(symbol, tradeCommand, price, stopLoss, takeProfit, volume, string.Empty);
        }

        Task<TradeTransResponsePacket> ITradeProvider.OpenOrder(string symbol, TradeCommand tradeCommand,
                            double price, double stopLoss, double takeProfit, double volume, string comment)
        {
            Func<int, APINetworkPacket> lambda = (reqId =>
                new TradeTransRequestPacket(reqId, tradeCommand, TransactionType.OpenOrder, price, stopLoss, takeProfit, symbol, volume, 0, comment, new DateTime(1970, 1, 1)));

            return HandleRequestPacket<TradeTransResponsePacket>(lambda);
        }

        Task<TradeTransResponsePacket> ITradeProvider.ModifyOrder(int orderId, double newPrice, double newStopLoss,
            double newTakeProfit, double newVolume, DateTime newExpiration)
        {            
            Func<int, APINetworkPacket> lambda = (reqId => 
                new TradeTransRequestPacket(reqId, 0, TransactionType.OrderModify, newPrice, newStopLoss, newTakeProfit, "", newVolume, orderId, string.Empty, newExpiration));

            return HandleRequestPacket<TradeTransResponsePacket>(lambda);
        }

        Task<TradeTransResponsePacket> ITradeProvider.DeleteOrder(int orderId)
        {         
            Func<int, APINetworkPacket> lambda = (reqId =>
                new TradeTransRequestPacket(reqId, 0, TransactionType.OrderDelete,0, 0, 0, "", 0, orderId, string.Empty, new DateTime(1970, 1, 1)));

            return HandleRequestPacket<TradeTransResponsePacket>(lambda);
        }

        Task<TradeTransResponsePacket> ITradeProvider.CloseOrder(int orderId, double volume)
        {
            var reqId = GetReqId();
            var packet = new TradeTransRequestPacket(reqId, 0, TransactionType.OrderClose,
                0, 0, 0, "", volume, orderId, string.Empty, new DateTime(1970, 1, 1));

            var task = EnqueueTcs<TradeTransResponsePacket>(reqId);

            apiConnection.SendPacket(packet);

            return task;
        }

        #region Packet Handling
        private void HandlePacket(APINetworkPacket genericPacket)
        {
            var handler = packetHandlers[(int)genericPacket.Type];
            if (handler != null)
                handler(genericPacket);
        }

        private void InitialiseHandlers()
        {
            packetHandlers[(int)APINetworkPacketType.Ticks] = HandleTicksPacket;
            packetHandlers[(int)APINetworkPacketType.MarginLevel] = HandleMarginLevelPacket;
            packetHandlers[(int)APINetworkPacketType.TradeUpdate] = HandleTradeUpdate;
            packetHandlers[(int)APINetworkPacketType.LoginResponse] = HandleLoginResponsePacket;
            packetHandlers[(int)APINetworkPacketType.CandleResponse] = HandleResponsePacket<CandleResponsePacket>;
            packetHandlers[(int)APINetworkPacketType.InstrumentSettingsResponse] = HandleResponsePacket<InstrumentSettingsResponsePacket>;
            packetHandlers[(int)APINetworkPacketType.SessionScheduleResponse] = HandleResponsePacket<SessionScheduleResponsePacket>;
            packetHandlers[(int)APINetworkPacketType.MarginLevelResponse] = HandleResponsePacket<MarginLevelResponsePacket>;
            packetHandlers[(int)APINetworkPacketType.ClosedTradesResponse] = HandleResponsePacket<ClosedTradesResponsePacket>;
            packetHandlers[(int)APINetworkPacketType.TradesInfoResponse] = HandleResponsePacket<TradesInfoResponsePacket>;
            packetHandlers[(int)APINetworkPacketType.TradeTransResponse] = HandleResponsePacket<TradeTransResponsePacket>;
            packetHandlers[(int)APINetworkPacketType.HeartBeatResponse] = HandleResponsePacket<HeartBeatResponsePacket>;
            packetHandlers[(int)APINetworkPacketType.AccountSettingsResponse] = HandleResponsePacket<AccountSettingsResponsePacket>;
            packetHandlers[(int)APINetworkPacketType.TickRegistrationResponse] = HandleResponsePacket<TickRegistrationResponsePacket>;
        }

        private void HandleMarginLevelPacket(APINetworkPacket genericPacket)
        {
            var packet = (MarginLevelPacket)genericPacket;
            OnMargin(packet.MarginLevel);
        }

        private void HandleTicksPacket(APINetworkPacket genericPacket)
        {
            var packet = (TickPacket)genericPacket;
            OnTicks(packet.Ticks);
        }

        private void HandleTradeUpdate(APINetworkPacket genericPacket)
        {
            var packet = (TradeUpdatePacket)genericPacket;
            OnTradeUpdate(packet);
        }

        private void HandleLoginResponsePacket(APINetworkPacket genericPacket)
        {
            var packet = (LoginResponsePacket)genericPacket;
            var tcs = AcquireTcs<LoginResponsePacket>(packet.RequestId);

            if (packet.LoginStatus == LoginStatus.Successful)
                tcs.SetResult(packet);
            else
                tcs.SetException(new AuthenticationException("Authentication exception"));
        }

        private Task<Y> HandleRequestPacket<Y>(Func<int, APINetworkPacket> ctor) where Y : APINetworkPacket
        {
            var reqId = GetReqId();
            var packet = ctor(reqId);

            var task = EnqueueTcs<Y>(reqId);

            apiConnection.SendPacket(packet);

            return task;
        }

        private void HandleResponsePacket<T>(APINetworkPacket genericPacket) where T : APINetworkPacket, IIdentifiable
        {
            var packet = genericPacket as T;
            var tcs = AcquireTcs<T>(packet.RequestId);

            tcs.TrySetResult(packet);
        }
        #endregion

        #region Events
        private void OnTicks(Tick[] ticks)
        {
            var h = Ticks;
            if (h != null)
                EventHandler.RiseSafely(() => h(ticks));
        }
        private void OnMargin(MarginLevel ml)
        {
            var h = Margin;
            if (h != null)
                EventHandler.RiseSafely(() => h(ml));
        }
        private void OnTradeUpdate(TradeUpdatePacket tup)
        {
            var h = TradeUpdate;
            if (TradeUpdate != null)
                EventHandler.RiseSafely(() => h(tup));
        }
        private void OnDisconnected(Exception exc)
        {
            var h = Disconnected;
            if (h != null)
                EventHandler.RiseSafely(() => h(exc));

            SetErrorToTcsAndClear(new Exception("Client has been disconnected"));
        }
        #endregion

        #region Task Management
        private TaskCompletionSource<Y> AcquireTcs<Y>(int reqId)
        {
            lock (tcsMutex)
            {
                if (!tcsByRequest.ContainsKey(reqId))
                    throw new InvalidOperationException("Invalid requestId " + reqId + " received from server");

                var tcs = tcsByRequest[reqId] as TaskCompletionSource<Y>;

                if (tcs == null)
                    throw new InvalidOperationException("Invalid TCS for request " + reqId);

                tcsByRequest.Remove(reqId);
                tcsTypes.Remove(reqId);

                return tcs;
            }
        }
        private Task<Y> EnqueueTcs<Y>(int reqId)
        {
            var tcs = new TaskCompletionSource<Y>();

            lock (tcsMutex)
            {
                if (tcsByRequest.ContainsKey(reqId))
                    throw new InvalidOperationException("Request Id is not unique");

                tcsByRequest[reqId] = tcs;
                tcsTypes[reqId] = typeof(Y);
            }

            return tcs.Task;
        }
        private void SetErrorToTcsAndClear(Exception exception)
        {
            try
            {
                lock (tcsMutex)
                {
                    foreach (var kv in tcsByRequest)
                    {
                        var type = tcsTypes[kv.Key];
                        var tcs = kv.Value;
                        typeof(TaskCompletionSource<>).MakeGenericType(type)
                            .InvokeMember("SetException",
                                System.Reflection.BindingFlags.Public |
                                System.Reflection.BindingFlags.Instance |
                                System.Reflection.BindingFlags.InvokeMethod,
                            null, tcs, new[] { exception });
                    }

                    tcsByRequest.Clear();
                    tcsTypes.Clear();
                }
            }
            catch (Exception exc)
            {
                //TODO: This is invoked on Threadpool thread - need to be handled
            }
        }
        #endregion

        private void StartHeartBeatTimer()
        {
            heartBeatTimer.Tick += OnHeartBeatRequest;
            heartBeatTimer.Change(5000, 3000);
        }

        private void OnHeartBeatRequest()
        {
            var reqId = GetReqId();

            EnqueueTcs<HeartBeatResponsePacket>(reqId);
            var packet = new HeartBeatRequestPacket(reqId);

            apiConnection.SendPacket(packet);
        }

        public int GetReqId()
        {
            return System.Threading.Interlocked.Increment(ref reqId);
        }
    }
}