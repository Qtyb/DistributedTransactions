using BasketApi.Domain.Events;
using BasketApi.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BasketApi.Services
{
    public class RabbitMqSubscriberService : IRabbitMqSubscriberService
    {
        private readonly IRabbitMqConnection _rabbitMqConnection;
        private readonly IConfiguration _configuration;
        private readonly ILogger<RabbitMqSubscriberService> _logger;
        private readonly string _queueName = nameof(BasketApi); //GET FROM CONFIGURATION
        private readonly string _exchangeName = "distributedTransactions.exchange"; //GET FROM CONFIGURATION

        public RabbitMqSubscriberService(
            IRabbitMqConnection rabbitMqConnection,
            IServiceProvider provider,
            IConfiguration configuration,
            ILogger<RabbitMqSubscriberService> logger)
        {
            _rabbitMqConnection = rabbitMqConnection;
            _configuration = configuration;
            _logger = logger;
        }

        public void Subscribe(IEnumerable<string> topics)
        {
            var channel = _rabbitMqConnection.Connect();

            CreateExchange(channel, _exchangeName);
            CreateQueue(channel, _queueName);

            foreach (var topic in topics)
            {
                _logger.LogInformation($"Subscribing for topic {topic}");
                BindQueue(channel, topic);
                CreateProductCreatedConsumer(channel);
                _logger.LogInformation($"Subscribed for topic {topic}");
            }
        }

        //public void SubscribeToProductCreated(IModel channel)
        //{
        //    CreateConsumer(channel);
        //}

        private void CreateProductCreatedConsumer(IModel channel)
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                _logger.LogInformation($"Received messgae");

                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());
                _logger.LogInformation($"Consuming messgae {message}");
                var productCreated = JsonSerializer.Deserialize<ProductCreated>(message);

                //TODO Add action argument
                //Action<ProductCreated> action
                //action.Invoke(productCreated);

            };
            channel.BasicConsume(queue: _queueName,
                                 autoAck: true,
                                 consumer: consumer);
        }

        private void CreateExchange(IModel channel, string exchangeName)
        {
            channel.ExchangeDeclare(exchangeName, ExchangeType.Direct, durable: true, autoDelete: false);
        }

        private void CreateQueue(IModel channel, string queueName)
        {
            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
        }

        private void BindQueue(IModel channel, string topic)
        {
            channel.QueueBind(_queueName, _exchangeName, topic);
        }
    }
}