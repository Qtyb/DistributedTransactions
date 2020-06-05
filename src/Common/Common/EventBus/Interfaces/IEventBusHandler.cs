namespace Qtyb.Common.EventBus.Interfaces
{
    public interface IEventBusHandler<T> where T : class
    {
        void Handle(T @event);
    }
}