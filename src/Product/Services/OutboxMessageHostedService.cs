using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProductApi.Data.Context;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProductApi.Services
{
    public class OutboxMessageHostedService : BackgroundService
    {
        private readonly ILogger<OutboxMessageHostedService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        private const int DelayInMs = 10_000;

        public OutboxMessageHostedService(
            ILogger<OutboxMessageHostedService> logger,
            IHttpClientFactory httpClientFactory,
            IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Outbox Message Hosted Service is running");

            await BackgroundProcessing(stoppingToken);
        }

        private async Task BackgroundProcessing(CancellationToken stoppingToken)
        {
            var client = _httpClientFactory.CreateClient();
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var context = scope.ServiceProvider.GetRequiredService<ProductContext>();
                        var messages = await context.OutboxMessages.Where(m => m.ProcessedOn == null).ToListAsync();

                        foreach (var message in messages)
                        {
                            try
                            {
                                var content = new StringContent(message.Data, Encoding.UTF8, "application/json");
                                var response = message.HttpMethod switch
                                {
                                    Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http.HttpMethod.Post => await client.PostAsync(message.Url, content),
                                    _ => throw new ArgumentException($"uknown http type {message.HttpMethod}")
                                };

                                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                                    message.ProcessedOn = DateTime.Now;
                                else
                                    message.Error = $"Status: [{(int)response.StatusCode}], Reason: [{response.ReasonPhrase}]";
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, $"Outbox Message Hosted Service occurred error during sending message with id [{message.Id}]");
                                message.ProcessedOn = null;
                                message.Error = ex.ToString();
                            }

                            await context.SaveChangesAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Outbox Message Hosted Service occurred error");
                }
                finally
                {
                    _logger.LogInformation($"Outbox Message Hosted Service is going to sleep for {DelayInMs}ms");
                    await Task.Delay(DelayInMs, stoppingToken);
                }
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Outbox Message Hosted Service is stopping.");

            await base.StopAsync(stoppingToken);
        }
    }
}