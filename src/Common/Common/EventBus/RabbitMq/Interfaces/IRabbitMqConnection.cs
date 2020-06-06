using RabbitMQ.Client;

namespace Qtyb.Common.EventBus.RabbitMq.Interfaces
{
    public interface IRabbitMqConnection
    {
        IConnection GetConnection();
    }
}