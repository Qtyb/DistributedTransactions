using Microsoft.Extensions.DependencyInjection;
using Qtyb.Common.EventBus.Interfaces;
using Qtyb.Common.EventBus.RabbitMq;
using Qtyb.Common.EventBus.RabbitMq.Interfaces;

namespace Qtyb.Common.EventBus.Extensions
{
    public static class EventBusExtensions
    {
        public static void AddEventBus(this IServiceCollection services)
        {
            services.AddSingleton<IRabbitMqConnection, RabbitMqConnection>();
            services.AddSingleton<IEventBusSubscriber, RabbitMqSubscriberService>();
            services.AddSingleton<IEventBusPublisher, RabbitMqPublisherService>();
        }
    }
}