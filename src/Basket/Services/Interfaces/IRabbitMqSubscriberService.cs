namespace BasketApi.Services.Interfaces
{
    public interface IRabbitMqSubscriberService
    {
        void Subscribe<T>(string topic)
             where T : class;
    }
}