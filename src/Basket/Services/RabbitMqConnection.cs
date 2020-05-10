using BasketApi.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace BasketApi.Services
{
    public class RabbitMqConnection : IRabbitMqConnection
    {
        private readonly IConfigurationSection _rabbitMqConfig;
        private readonly ILogger<RabbitMqConnection> _logger;
        private IConnection _connection;

        public RabbitMqConnection(
            IConfiguration configuration,
            ILogger<RabbitMqConnection> logger)
        {
            _rabbitMqConfig = configuration.GetSection("RabbitMq");
            _logger = logger;
        }

        public IConnection Connect()
        {
            _logger.LogInformation($"{nameof(RabbitMqConnection)}.{nameof(Connect)} invoked");
            if (_connection is null || _connection.IsOpen is false)
            {
                _logger.LogInformation($"{nameof(RabbitMqConnection)} is not established." +
                    $"Trying to establish connection");
                _connection?.Dispose();
                InitializeConnection();
            }

            return _connection;
        }

        private void InitializeConnection()
        {
            _logger.LogInformation($"{nameof(RabbitMqConnection)}.{nameof(InitializeConnection)} invoked");
            //GET FROM APPSETTINGS
            var factory = new ConnectionFactory() { HostName = _rabbitMqConfig["Hostname"] };
            _connection = factory.CreateConnection();

            _logger.LogInformation($"{nameof(RabbitMqConnection)}.{nameof(InitializeConnection)} connection initialized");
        }
    }
}