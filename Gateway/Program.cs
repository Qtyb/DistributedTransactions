using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Gateway
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please enter a numeric argument.");
                return;
            }

            var seconds = int.Parse(args[0]);
            var finishDate = DateTime.Now.AddSeconds(seconds);
            
            var client = new HttpClient();

            long index = 1;
            Console.WriteLine($"Gateway started sending requests until {finishDate}");
            while (DateTime.Now < finishDate)
            {
                var product = new { Name = $"{index}" };
                var content = new StringContent(
                    JsonSerializer.Serialize(product),
                    Encoding.UTF8,
                    "application/json");

                using var httpResponse = await client.PostAsync("http://localhost:6000/api/products", content);
                index++;
            }
        }
    }
}
