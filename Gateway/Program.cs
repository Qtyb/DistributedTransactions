using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Gateway
{
    internal class Program
    {
        private enum TransactionType
        {
            Default,
            OutboxPattern,
            Choreography,
            Orchestration
        }

        private static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please enter a numeric argument.");
                return;
            }

            Enum.TryParse(args[1], out TransactionType transactionType);
            var seconds = int.Parse(args[0]);
            var finishDate = DateTime.Now.AddSeconds(seconds);
            Console.WriteLine($"Gateway started sending requests until {finishDate}");

            var client = new HttpClient();
            long index = 1;
            while (DateTime.Now < finishDate)
            {
                var product = new { Name = $"{index}" };
                var content = new StringContent(
                    JsonSerializer.Serialize(product),
                    Encoding.UTF8,
                    "application/json");

                if (transactionType == TransactionType.Default)
                    await HandleDefaultTransaction(content, client);
                else if (transactionType == TransactionType.OutboxPattern)
                    await HandleOutboxPattern(content, client);
                else if (transactionType == TransactionType.Choreography)
                    await HandleChoreographySaga(content, client);
                else if (transactionType == TransactionType.Orchestration)
                    await HandleOrchestrationSaga(content, client);
                else
                    throw new Exception($"Undefined transaction type: [{transactionType}]");

                index++;
            }
        }

        private static async Task HandleDefaultTransaction(StringContent content, HttpClient client)
        {
            using var httpResponse = await client.PostAsync("http://localhost:6000/api/products", content);
        }

        private static async Task HandleOutboxPattern(StringContent content, HttpClient client)
        {
            using var httpResponse = await client.PostAsync("http://localhost:6000/api/products/outbox", content);
        }

        private static async Task HandleChoreographySaga(StringContent content, HttpClient client)
        {
            using var httpResponse = await client.PostAsync("http://localhost:6000/api/products/choreography", content);
        }

        private static async Task HandleOrchestrationSaga(StringContent content, HttpClient client)
        {
            throw new NotImplementedException();
        }
    }
}