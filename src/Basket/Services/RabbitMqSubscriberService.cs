using BasketApi.Domain.Events.Subscribe;
using BasketApi.Services.Interfaces;
using MediatR;
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
        private readonly IMediator _mediator;
        private readonly ILogger<RabbitMqSubscriberService> _logger;
        private readonly string _queueName = nameof(BasketApi); //GET FROM CONFIGURATION
        private readonly string _exchangeName = "distributedTransactions.exchange"; //GET FROM CONFIGURATION

        public RabbitMqSubscriberService(
            IRabbitMqConnection rabbitMqConnection,
            IServiceProvider provider,
            IConfiguration configuration,
            IMediator mediator,
            ILogger<RabbitMqSubscriberService> logger)
        {
            _rabbitMqConnection = rabbitMqConnection;
            _configuration = configuration;
            _mediator = mediator;
            _logger = logger;
        }

        public void Subscribe<T>(string topic)
            where T : class
        {
            var channel = _rabbitMqConnection.Connect();

            CreateExchange(channel, _exchangeName);
            CreateQueue(channel, _queueName);

            _logger.LogInformation($"Subscribing for topic [{topic}]");
            BindQueue(channel, topic);
            InitializeConsumer(channel);
            _logger.LogInformation($"Subscribed for topic [{topic}]");
        }

        private async Task OnEventReceived<T>(object sender, BasicDeliverEventArgs @event)
        {
            var channel = _rabbitMqConnection.Connect();
            try
            {
                var body = Encoding.UTF8.GetString(@event.Body.ToArray());
                //var message = JsonConvert.DeserializeObject<T>(body);
                var message = JsonSerializer.Deserialize<T>(body);

                await _mediator.Send(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving message from queue.");
            }
            finally
            {
                channel.BasicAck(@event.DeliveryTag, false);
            }
        }

        private void InitializeConsumer(IModel channel)
        {
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += OnEventReceived<ProductCreated>;

            channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
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