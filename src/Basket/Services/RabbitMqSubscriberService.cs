using BasketApi.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;

namespace BasketApi.Services
{
    public class RabbitMqSubscriberService : IRabbitMqSubscriberService
    {
        private readonly IRabbitMqConnection _rabbitMqConnection;
        private readonly IConfiguration _configuration;
        private readonly string _queueName = nameof(BasketApi); //GET FROM CONFIGURATION
        private readonly string _exchangeName = "distributedTransactions.exchange";

        public RabbitMqSubscriberService(
            IRabbitMqConnection rabbitMqConnection,
            IConfiguration configuration)
        {
            _rabbitMqConnection = rabbitMqConnection;
            _configuration = configuration;
        }

        public void Subscribe(IEnumerable<string> topics)
        {
            var connection = _rabbitMqConnection.Connect();
            using (var channel = connection.CreateModel())
            {
                CreateExchange(channel, _exchangeName);
                CreateQueue(channel, _queueName);

                foreach (var topic in topics)
                {
                    
                    BindQueue(channel, topic);
                }
            }
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