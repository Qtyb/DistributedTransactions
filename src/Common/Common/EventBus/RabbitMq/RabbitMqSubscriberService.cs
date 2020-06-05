using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Qtyb.Common.EventBus.Interfaces;
using Qtyb.Common.EventBus.RabbitMq.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Qtyb.Common.EventBus.RabbitMq
{
    public class RabbitMqSubscriberService : IEventBusSubcriber
    {
        private readonly IRabbitMqConnection _rabbitMqConnection;
        private readonly IMediator _mediator;
        private readonly ILogger<RabbitMqSubscriberService> _logger;
        private readonly string _queueName;
        private readonly string _exchangeName;

        public RabbitMqSubscriberService(
            IRabbitMqConnection rabbitMqConnection,
            IConfiguration configuration,
            IMediator mediator,
            ILogger<RabbitMqSubscriberService> logger)
        {
            _rabbitMqConnection = rabbitMqConnection;
            _mediator = mediator;
            _logger = logger;

            var rabbitMqSettings = configuration.GetSection("RabbitMq");
            _queueName = rabbitMqSettings["Queue"];
            _exchangeName = rabbitMqSettings["Exchange"];
        }

        public void Subscribe<T>(string topic)
            where T : class
        {
            var channel = _rabbitMqConnection.Connect();

            CreateExchange(channel, _exchangeName);
            CreateQueue(channel, _queueName);

            _logger.LogInformation($"Subscribing for topic [{topic}]");
            BindQueue(channel, topic);
            InitializeConsumer<T>(channel);
            _logger.LogInformation($"Subscribed for topic [{topic}]");
        }

        private async Task OnEventReceived<T>(object sender, BasicDeliverEventArgs @event)
        {
            var channel = _rabbitMqConnection.Connect();
            try
            {
                var body = Encoding.UTF8.GetString(@event.Body.ToArray());
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

        private void InitializeConsumer<T>(IModel channel)
            where T : class
        {
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += OnEventReceived<T>;

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