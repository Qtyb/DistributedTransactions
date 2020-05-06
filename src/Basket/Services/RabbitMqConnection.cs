using BasketApi.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace BasketApi.Services
{
    public class RabbitMqConnection : IRabbitMqConnection
    {
        private IConnection _connection;

        public RabbitMqConnection(IConfiguration configuration)
        {
        }

        public IConnection Connect()
        {
            if (_connection is null || _connection.IsOpen is false)
            {
                _connection?.Dispose();
                InitializeConnection();
            }

            return _connection;
        }

        private void InitializeConnection()
        {
            //GET FROM APPSETTINGS
            var factory = new ConnectionFactory() { HostName = "192.168.0.30" };
            _connection = factory.CreateConnection();
        }
    }
}