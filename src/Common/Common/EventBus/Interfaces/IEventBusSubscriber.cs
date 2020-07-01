namespace Qtyb.Common.EventBus.Interfaces
{
    public interface IEventBusSubscriber
    {
        void Subscribe<T>()
            where T : class;

        void BindQueue<T>()
            where T : class;
    }
}