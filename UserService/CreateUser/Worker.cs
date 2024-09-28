using Core.Entities;
using Messaging.Interface;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;

namespace CreateUser
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        private readonly IRabbitMqService _rabbitMqService;

        public Worker(ILogger<Worker> logger, IRabbitMqService rabbitMqService )
        {
            _logger = logger;
            _rabbitMqService = rabbitMqService;

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                    using var connection = _rabbitMqService.CreateChannel(
                        CreateUserHelper.GetSetting("UserName")
                        , CreateUserHelper.GetSetting("Password")
                        , CreateUserHelper.GetSetting("Hostname")
                        , Int32.Parse(CreateUserHelper.GetSetting("Port")));


                    using var channel = connection.CreateModel();

                    channel.QueueDeclare(queue: CreateUserHelper.GetSetting("QueueNameCreateUser"),
                                   durable: true,
                                   exclusive: false,
                                   autoDelete: false,
                                   arguments: null);

                    var consumer = new AsyncEventingBasicConsumer(channel);
                    consumer.Received += async (ch, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var text = System.Text.Encoding.UTF8.GetString(body);
                        var pedido = JsonSerializer.Deserialize<User>(body);


                        await Task.CompletedTask;
                        channel.BasicAck(ea.DeliveryTag, false);
                    };


                    channel.BasicConsume(
                    queue: CreateUserHelper.GetSetting("QueueNameCreateUser"),
                    autoAck: true,
                    consumer: consumer);
                    await Task.Delay(2000, stoppingToken);
                }



                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
