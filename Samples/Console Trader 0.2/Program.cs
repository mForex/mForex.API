using System;
using System.Threading.Tasks;
using mForex.API;
using mForex.API.Packets;

namespace Console_Trader_0._2
{
    class Program
    {

        static void Main(string[] args)
        {
            int login = 0;                  // Enter your login
            string password = "password";   // Enter your password
                       
            var client = new APIClient(new APIConnection(ServerType.Demo));

            client.Connect().Wait();
            Console.WriteLine("Connected");

            client.Login(login, password).ContinueWith(r =>
            {
                var resp = r.Result;
                Console.WriteLine("Login response: {0} - {1}", resp.Login, resp.LoginStatus);

                TradeCommand command = TradeCommand.Buy;
                string instrument = "EURUSD";
                double volume = 0.1;

                client.Trade.OpenOrder(instrument, command, 0, 0, 0, volume).ContinueWith(p =>
                    {
                        Console.WriteLine("Trade response {0} for order {1}", p.Result.Order, p.Result.ErrorCode);
                    }
                );
            });

            Console.ReadKey();
        }
    }
}
