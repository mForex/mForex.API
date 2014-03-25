using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Extensions;
using Moq;
using mForex.API;
using mForex.API.Packets;
using mForex.API.Tests.Utils;
using mForex.API.Exceptions;
using System.Linq.Expressions;

namespace mForex.API.Tests
{
    public class APIClientTests
    {
        protected Mock<IApiConnection> moq_connection;

        public APIClientTests()
        {
            moq_connection = new Mock<IApiConnection>();
        }

        [Fact]
        public void ApiConnection_Connect_Called()
        {
            var client = new APIClient(moq_connection.Object);

            client.Connect().Wait();

            moq_connection.Verify(con => con.Connect(), Times.Once);
        }

        [Fact]
        public void ApiConnection_Disconnect_Called()
        {
            var client = new APIClient(moq_connection.Object);

            client.Disconnect();

            moq_connection.Verify(con => con.Disconnect(), Times.Once);
        }

        #region Requests Tests
        [Fact]
        public void LoginRequest_Packet_Has_Been_Sent()
        {
            var client = new APIClient(moq_connection.Object);
            int login = 1;
            string password = "foo";

            client.Login(login, password);

            moq_connection.Verify(con => con.SendPacket(It.Is<LoginRequestPacket>(p => p.Login == login && p.Password == password)));
        }

        [Fact]
        public async Task LoginResponse_Has_Been_Handled_Properly()
        {
            var client = new APIClient(moq_connection.Object);
            var login = 1;

            LoginRequestPacket packet = null;
            moq_connection.Setup(con => con.SendPacket(It.IsAny<LoginRequestPacket>()))
                .Callback<APINetworkPacket>(x => packet = (LoginRequestPacket)x);

            var task = client.Login(login, "Foo");

            moq_connection.Raise(con => con.PacketReceived += null,
                new LoginResponsePacket() { RequestId = packet.RequestId, Login = login, LoginStatus = LoginStatus.Successful });

            var r = await task;

            Assert.Equal(login, r.Login);
            Assert.Equal(LoginStatus.Successful, r.LoginStatus);
        }

        [Fact]
        public async Task LoginRequest_With_Invalid_Credentials_Throws_AuthenticationException()
        {
            var client = new APIClient(moq_connection.Object);
            var login = 1;

            LoginRequestPacket packet = null;
            moq_connection.Setup(con => con.SendPacket(It.IsAny<LoginRequestPacket>()))
                .Callback<APINetworkPacket>(x => packet = (LoginRequestPacket)x);

            var task = client.Login(login, "Foo");

            moq_connection.Raise(con => con.PacketReceived += null,
                new LoginResponsePacket() { RequestId = packet.RequestId, Login = login, LoginStatus = LoginStatus.InvalidPassword });

            await TaskHelper.ThrowsAsync<AuthenticationException>(async () => await task);
        }

        [Fact]
        public void Throws_Error_If_ResponsePacket_Is_Not_IIdentifiable()
        {
            var client = new APIClient(moq_connection.Object);

            Assert.Throws<InvalidOperationException>(() => moq_connection
                .Raise(con => con.PacketReceived += null, new CandleResponsePacket() { RequestId = 1 }));
        }

        [Fact]
        public void CandleRequest_Has_Been_Sent()
        {
            var symbol = "EURUSD";
            var period = CandlePeriod.H4;
            var from = DateTime.Now.AddHours(-1);
            var to = DateTime.Now;

            VerifyRequest<CandleRequestPacket>(c => c.RequestCandles(symbol, period, from, to),
                                                p => p.Symbol == symbol && p.Period == period &&
                                                     p.FromTime == from && p.ToTime == to);
        }

        [Fact]
        public void ClosedTradesRequest_Has_Been_Sent()
        {
            var from = DateTime.Now.AddDays(-1);
            var to = DateTime.Now;

            VerifyRequest<ClosedTradesRequestPacket>(c => c.RequestTradesHistory(from, to),
                                                     p => p.DateFrom == from && p.DateTo == to);
        }

        [Fact]
        public void InstrumentSettingsRequest_Has_Been_Sent()
        {
            VerifyRequest<InstrumentSettingsRequestPacket>(c => c.RequestInstrumentSettings());
        }

        [Fact]
        public void MarginLevelRequest_Has_Been_Sent()
        {
            VerifyRequest<MarginLevelRequestPacket>(c => c.RequestMarginLevel());
        }

        [Fact]
        public void Request_For_Open_Trades_Has_Been_Sent()
        {
            VerifyRequest<TradesInfoRequestPacket>(c => c.RequestOpenTrades());
        }

        [Fact]
        public void Request_For_AccountSettings_Has_Been_Sent()
        {
            VerifyRequest<AccountSettingsRequestPacket>(c => c.RequestAccountSettings());
        }

        [Theory]
        [InlineData("EURUSD", TradeCommand.Buy, 1.2, 0, 0, 0.1, "valid trade")]
        [InlineData("EURUSD", TradeCommand.BuyLimit, 1.2, 1.45, 1.90, 1, "valid trade")]
        [InlineData("EURUSD", TradeCommand.BuyStop, 1.2, 0, 0, 0.1, "valid trade")]
        [InlineData("EURUSD", TradeCommand.Sell, 1.2, 0, 0, 0.1, "valid trade")]
        [InlineData("EURUSD", TradeCommand.SellLimit, 1.2, 0, 0, 0.1, "valid trade")]
        [InlineData("EURUSD", TradeCommand.SellStop, 1.2, 0, 0, 0.1, "valid trade")]
        [InlineData("EURUSD", TradeCommand.SellStop, 1.2, 0, 0, 0.1, "")]
        public void OpenOrder_Has_Send_The_Right_Packet(string symbol, TradeCommand tradeCommand,
            double price, double stopLoss, double takeProfit, double volume, string comment)
        {
            
            Action<APIClient> request_with_comment = c => c.Trade.OpenOrder(symbol, tradeCommand, price, stopLoss, takeProfit, volume, comment);
            Action<APIClient> request_without_comment = c => c.Trade.OpenOrder(symbol, tradeCommand, price, stopLoss, takeProfit, volume);
            
            Expression<Func<TradeTransRequestPacket, bool>> match = p => p.TradeCommand == tradeCommand &&
                                                                    p.TransactionType == TransactionType.OpenOrder &&
                                                                    p.Price == price &&
                                                                    p.StopLoss == stopLoss &&
                                                                    p.TakeProfit == takeProfit &&
                                                                    p.Symbol == symbol &&
                                                                    p.Volume == volume &&
                                                                    p.Comment == comment &&
                                                                    p.Expiration == new DateTime(1970, 1, 1);

            if (comment == String.Empty)
                VerifyRequest<TradeTransRequestPacket>(request_without_comment, match);
            else
                VerifyRequest<TradeTransRequestPacket>(request_with_comment, match);
        }

        [Theory]
        [InlineData(1, 0.1, 0.99, 0.11, 0.1, null)]
        public void Modify_Has_Send_The_Right_Packet(int orderId, double newPrice, double newStopLoss,
            double newTakeProfit, double newVolume, DateTime newExpiration)
        {
            VerifyRequest<TradeTransRequestPacket>(c => c.Trade.ModifyOrder(orderId, newPrice, newStopLoss, newTakeProfit, newVolume, newExpiration),
                                                   p => p.TransactionType == TransactionType.OrderModify &&
                                                   p.Price == newPrice &&
                                                   p.StopLoss == newStopLoss &&
                                                   p.TakeProfit == newTakeProfit &&
                                                   p.Volume == newVolume &&
                                                   p.Order == orderId &&
                                                   p.Expiration == newExpiration);
        }

        [Theory]
        [InlineData(1)]
        public void DeleteOrder_Has_Send_The_Right_Packet(int orderId)
        {
            VerifyRequest<TradeTransRequestPacket>(c => c.Trade.DeleteOrder(orderId),
                                                   p => p.TradeCommand == 0 &&
                                                        p.TransactionType == TransactionType.OrderDelete &&
                                                        p.Order == orderId);
        }

        [Theory]
        [InlineData(1, 1)]
        public void CloseOrder_Has_Send_The_Right_Packet(int orderId, double volume)
        {

            VerifyRequest<TradeTransRequestPacket>(c => c.Trade.CloseOrder(orderId, volume),
                                                   p => p.TradeCommand == 0 && p.TransactionType == TransactionType.OrderClose &&
                                                        p.Volume == volume && p.Order == orderId);
        }

        [Theory]
        [InlineData("EURUSD")]
        public void RequestSessions_Has_Send_The_Right_Packet(string symbol)
        {
            VerifyRequest<SessionScheduleRequestPacket>(c => c.RequestSessions(symbol),
                                                        p => p.Symbol == symbol);
        }

        [Theory]
        [InlineData("EURUSD", RegistrationAction.Register)]
        [InlineData("USDJPY", RegistrationAction.Unregister)]
        public void RequestTickRegistration_Has_Send_The_Right_Packet(string symbol, RegistrationAction action)
        {
            VerifyRequest<TickRegistrationRequestPacket>(c => c.RequestTickRegistration(symbol, action),
                                                         p => p.Symbol == symbol  &&
                                                              p.RegistrationAction == action);
        }
        
        private void VerifyRequest<T>(Action<APIClient> request) where T : APINetworkPacket
        {
            var client = new APIClient(moq_connection.Object);

            request(client);

            moq_connection.Verify(con => con.SendPacket(It.IsAny<T>()));
        }
        private void VerifyRequest<T>(Action<APIClient> request, Expression<Func<T, bool>> match) where T : APINetworkPacket
        {
            var client = new APIClient(moq_connection.Object);

            request(client);

            moq_connection.Verify(con => con.SendPacket(It.Is<T>(match)));
        }
        #endregion Requests Tests

        #region Events Tests
        [Fact]
        public void Proper_Ticks_Have_Been_Received_And_Rised()
        {
            List<Tick> receivedTicks = new List<Tick>();
            List<Tick> sentTicks = new List<Tick> { new Tick { Symbol = "EURUSD", Bid = 1, Ask = 2 },
                                                    new Tick { Symbol = "JPYPLN", Bid = 10, Ask = 11 }};

            var client = new APIClient(moq_connection.Object);
            client.Ticks += (ticks => { foreach (var tick in ticks) { receivedTicks.Add(tick); } });

            moq_connection.Raise(con => con.PacketReceived += null, new TickPacket() { Ticks = sentTicks.ToArray() });

            Assert.Equal(sentTicks, receivedTicks);
        }

        [Fact]
        public void Proper_Margin_Have_Been_Received_And_Rised()
        {
            MarginLevel sentML = new MarginLevel();
            MarginLevel receivedML = null;

            var client = new APIClient(moq_connection.Object);
            client.Margin += (ml => receivedML = ml);

            moq_connection.Raise(con => con.PacketReceived += null, new MarginLevelPacket() { MarginLevel = sentML });

            Assert.Equal(sentML, receivedML);
        }

        [Fact]
        public void Proper_TradeUpdate_Have_Been_Received_And_Rised()
        {
            var client = new APIClient(moq_connection.Object);

            TradeUpdatePacket sentPacket = new TradeUpdatePacket(new TradeRecord(), TradeAction.Opened);
            TradeUpdatePacket receivedPacket = null;

            client.TradeUpdate += (tu => receivedPacket = tu);

            moq_connection.Raise(con => con.PacketReceived += null, sentPacket);

            Assert.Equal(sentPacket, receivedPacket);
        }

        [Fact]
        public void Rise_Event_If_Disconnected()
        {
            bool event_rised = false;
            var client = new APIClient(moq_connection.Object);

            client.Disconnected += (x => event_rised = true);

            moq_connection.Raise(con => con.Disconnected += null, new Exception());

            Assert.Equal(true, event_rised);
        }
        #endregion

        #region Heartbeat test
        [Fact]
        public void HeartBeatTimer_Has_Been_Set()
        {
            var moq_timer = new Mock<ITimer>();
            var client = new APIClient(moq_connection.Object, moq_timer.Object);

            moq_timer.Verify(t => t.Change(It.IsAny<int>(), It.IsAny<int>()));
        }

        [Fact]
        public void HeartBeatRequestPacket_Has_Been_Sent()
        {
            var moq_timer = new Mock<ITimer>();
            var client = new APIClient(moq_connection.Object, moq_timer.Object);

            moq_timer.Raise(t => t.Tick += null);

            moq_connection.Verify(con => con.SendPacket(It.IsAny<HeartBeatRequestPacket>()));
        }
        #endregion
    }
}
