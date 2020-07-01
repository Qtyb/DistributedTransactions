using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Qtyb.Common.EventBus.Interfaces;
using Qtyb.Common.EventBus.RabbitMq.Interfaces;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Qtyb.Common.EventBus.RabbitMq
{
    public class RabbitMqPublisherService : IEventBusPublisher
    {
        private readonly IRabbitMqConnection _rabbitMqConnection;
        private readonly ILogger<RabbitMqPublisherService> _logger;
        private readonly IConfigurationSection _rabbitMqConfig;
        private readonly string _exchangeName = "distributedTransactions.exchange"; //GET FROM CONFIGURATION
        private IModel _channel;

        public RabbitMqPublisherService(
            IRabbitMqConnection rabbitMqConnection,
            IConfiguration configuration,
            ILogger<RabbitMqPublisherService> logger)
        {
            _rabbitMqConnection = rabbitMqConnection;
            _logger = logger;
            _rabbitMqConfig = configuration.GetSection("RabbitMq");
        }

        public void Publish<T>(T objectToSend, string routingKey)
        {
            _logger.LogInformation($"{nameof(Publish)} with\n{nameof(routingKey)}: [{routingKey}]\ntype: [{typeof(T).Name}]");

            var message = JsonSerializer.Serialize(objectToSend);

            EnsureConnectionToRabbitMq();
            SendRabbitMqMessage(message, routingKey);
        }

        public void Publish(string message, string routingKey)
        {
            _logger.LogInformation($"{nameof(Publish)} with\n{nameof(routingKey)}: [{routingKey}]\n{nameof(message)}: [{message}]");

            EnsureConnectionToRabbitMq();
            SendRabbitMqMessage(message, routingKey);
        }

        private void SendRabbitMqMessage(string message, string routingKey)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(_exchangeName, routingKey, mandatory: false, basicProperties: null, body);
            _logger.LogInformation($"{nameof(Publish)} with\n{nameof(routingKey)}: {routingKey}\n{nameof(message)}: {message} published");
        }

        private void EnsureConnectionToRabbitMq()
        {
            if (_channel is null || _channel.IsClosed)
            {
                _logger.LogInformation($"RabbitMq channel is not established. Trying to establish channel");

                _channel?.Close();
                _channel?.Dispose();
                _channel = null;

                _channel = _rabbitMqConnection.GetConnection().CreateModel();

                //TODO 2: CallbackException handle
            }
        }
    }
}