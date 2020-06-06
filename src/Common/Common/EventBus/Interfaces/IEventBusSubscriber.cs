namespace Qtyb.Common.EventBus.Interfaces
{
    public interface IEventBusSubscriber
    {
        void Subscribe<T>(string topic)
            where T : class;
    }
}