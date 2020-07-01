using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProductApi.Data.Context;
using Qtyb.Common.EventBus.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProductApi.Services
{
    public class OutboxEventHostedService : BackgroundService
    {
        private readonly ILogger<OutboxEventHostedService> _logger;
        private readonly IEventBusPublisher _eventBusPublisher;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        private const int DelayInMs = 10_000;

        public OutboxEventHostedService(
            ILogger<OutboxEventHostedService> logger,
            IEventBusPublisher eventBusPublisher,
            IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _eventBusPublisher = eventBusPublisher;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Outbox Event Hosted Service is running");

            await BackgroundProcessing(stoppingToken);
        }

        private async Task BackgroundProcessing(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var context = scope.ServiceProvider.GetRequiredService<ProductContext>();
                        var events = await context.OutboxEvents.Where(m => m.ProcessedOn == null).ToListAsync();

                        foreach (var @event in events)
                        {
                            try
                            {
                                _eventBusPublisher.Publish(@event.Data, @event.RoutingKey);
                                @event.ProcessedOn = DateTime.Now;
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, $"Outbox Event Hosted Service occurred error during sending message with id [{@event.Id}]");
                                @event.ProcessedOn = null;
                                @event.Error = ex.ToString();
                            }

                            await context.SaveChangesAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Outbox Event Hosted Service occurred error");
                }
                finally
                {
                    _logger.LogInformation($"Outbox Event Hosted Service is going to sleep for {DelayInMs}ms");
                    await Task.Delay(DelayInMs, stoppingToken);
                }
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Outbox Event Hosted Service is stopping.");

            await base.StopAsync(stoppingToken);
        }
    }
}