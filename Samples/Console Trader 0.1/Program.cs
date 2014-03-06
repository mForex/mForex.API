using System;
using System.Threading.Tasks;
using mForex.API;

namespace Console_Trader_0._1
{
    class Program
    {
        static void Main(string[] args)
        {
            int login = 0;                  // Enter your login
            string password = "password";   // Enter your password
            
            var client = new APIClient(new APIConnection(ServerType.Real));

            client.Connect().Wait();
            Console.WriteLine("Connected");

            client.Login(login, password).ContinueWith(r =>
            {
                var resp = r.Result;
                Console.WriteLine("Login response: {0} - {1}", resp.Login, resp.LoggedIn);
            });

            client.Ticks += ticks =>
            {
                foreach (var tick in ticks)
                    if (tick.Symbol == "EURUSD")
                        Console.WriteLine("{0} \t {1}/{2} \t ({3})", tick.Symbol, tick.Bid, tick.Ask, tick.Time);
            };

            Console.ReadKey();
        }
    }
}
