namespace Qtyb.Common.EventBus.Interfaces
{
    public interface IEventBusPublisher
    {
        void Publish<T>(T objectToSend, string routingKey);
    }
}