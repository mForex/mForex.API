using System;
using System.Threading.Tasks;
using mForex.API;
using mForex.API.Packets;

namespace Console_Trader_0._3
{
    class Program
    {
        private static APIClient client;
        private static Action<Task<TradeTransResponsePacket>> tradeResponseHandler;

        static void Main(string[] args)
        {

            int login = 0;                  // Enter your login
            string password = "password";   // Enter your password

            InitializeHandler();

            client = new APIClient(new APIConnection(ServerType.Demo));

            client.Connect().Wait();
            Console.WriteLine("Connected");

            client.Login(login, password).ContinueWith(r =>
            {
                var resp = r.Result;
                Console.WriteLine("Login response: {0} - {1}", resp.Login, resp.LoginStatus);
            });

            while (true)
            {
                var line = Console.ReadLine();
                var tokens = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (tokens.Length == 0)
                    continue;

                if (!ParseCommand(tokens))
                    break;
            }
        }

        private static void InitializeHandler()
        {
            tradeResponseHandler = (p => { Console.WriteLine("Trade response {0} for order {1}", p.Result.Order, p.Result.ErrorCode); });
        }

        private static bool ParseCommand(string[] tokens)
        {
            try
            {
                if (tokens[0] == "market")
                {
                    TradeCommand command = (TradeCommand)Enum.Parse(typeof(TradeCommand), tokens[1]);
                    string instrument = tokens[2];
                    double volume = double.Parse(tokens[3]);

                    client.Trade.OpenOrder(instrument, command, 0, 0, 0, volume).ContinueWith(tradeResponseHandler);
                }
                else if (tokens[0] == "pending")
                {
                    TradeCommand command = (TradeCommand)Enum.Parse(typeof(TradeCommand), tokens[1]);
                    string instrument = tokens[2];
                    double price = double.Parse(tokens[3]);
                    double volume = double.Parse(tokens[4]);

                    client.Trade.OpenOrder(instrument, command, price, 0, 0, volume).ContinueWith(tradeResponseHandler);
                }
                else if (tokens[0] == "modify")
                {
                    int orderId = int.Parse(tokens[1]);
                    double price = double.Parse(tokens[2]);
                    double sl = double.Parse(tokens[3]);

                    client.Trade.ModifyOrder(orderId, price, sl, 0, 0, new DateTime(1970, 1, 1)).ContinueWith(tradeResponseHandler);
                }
                else if (tokens[0] == "delete")
                {
                    int orderId = int.Parse(tokens[1]);

                    client.Trade.DeleteOrder(orderId).ContinueWith(tradeResponseHandler);
                }
                else if (tokens[0] == "close")
                {
                    int orderId = int.Parse(tokens[1]);
                    double volume = 100;
                    double.TryParse(tokens[2], out volume);

                    client.Trade.CloseOrder(orderId, volume).ContinueWith(tradeResponseHandler);
                }
                return true;
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                return true;
            }
        }
    }
}
