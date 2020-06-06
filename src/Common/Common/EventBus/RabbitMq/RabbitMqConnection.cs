using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Qtyb.Common.EventBus.RabbitMq.Interfaces;
using RabbitMQ.Client;

namespace Qtyb.Common.EventBus.RabbitMq
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


        public IConnection GetConnection()
        {
            _logger.LogInformation($"{nameof(RabbitMqConnection)}.{nameof(GetConnection)} invoked");
            if (_connection is null || _connection.IsOpen is false)
            {
                _logger.LogInformation($"{nameof(RabbitMqConnection)} is not established." +
                    $"Trying to establish connection");
                
                _connection?.Close();
                _connection?.Dispose();
                _connection = null;

                InitializeConnection();
            }

            return _connection;
        }

        private void InitializeConnection()
        {
            _logger.LogInformation($"{nameof(RabbitMqConnection)}.{nameof(InitializeConnection)} invoked");

            var factory = new ConnectionFactory()
            {
                HostName = _rabbitMqConfig["Hostname"],
                DispatchConsumersAsync = true,
                AutomaticRecoveryEnabled = true
            };
            _connection = factory.CreateConnection();

            _logger.LogInformation($"{nameof(RabbitMqConnection)}.{nameof(InitializeConnection)} connection initialized");
        }
    }
}