using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Qtyb.Common.EventBus.Interfaces;
using Qtyb.Common.EventBus.RabbitMq.Interfaces;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Qtyb.Common.EventBus.RabbitMq
{
    public class RabbitMqPublisher : IEventBusPublisher
    {
        private readonly IRabbitMqConnection _rabbitMqConnection;
        private readonly ILogger<RabbitMqPublisher> _logger;
        private readonly IConfigurationSection _rabbitMqConfig;
        private readonly string _exchangeName = "distributedTransactions.exchange"; //GET FROM CONFIGURATION

        public RabbitMqPublisher(
            IRabbitMqConnection rabbitMqConnection,
            IConfiguration configuration,
            ILogger<RabbitMqPublisher> logger)
        {
            _rabbitMqConnection = rabbitMqConnection;
            _logger = logger;
            _rabbitMqConfig = configuration.GetSection("RabbitMq");
        }

        public void Publish<T>(T objectToSend, string routingKey)
        {
            _logger.LogInformation($"{nameof(RabbitMqPublisher)}.{nameof(Publish)} with " +
                $"\n{nameof(routingKey)}: [{routingKey}]\ntype: [{typeof(T).Name}]");

            var message = JsonSerializer.Serialize(objectToSend);

            var channel = _rabbitMqConnection.Connect();
            SendRabbitMqMessage(message, routingKey, channel);
        }

        private void SendRabbitMqMessage(string message, string routingKey, IModel channel)
        {
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(_exchangeName, routingKey, mandatory: false, basicProperties: null, body);
            _logger.LogInformation($"{nameof(RabbitMqPublisher)}.{nameof(Publish)} with " +
                $"\n{nameof(routingKey)}: {routingKey}\n{nameof(message)}: {message} published");
        }
    }
}