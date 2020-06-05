namespace Qtyb.Common.EventBus.Interfaces
{
    public interface IEventBusSubcriber
    {
        void Subscribe<T>(string topic)
            where T : class;
    }
}