using Messaging;
using Messaging.Interface;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace CreateUser
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        private readonly IRabbitMqService _rabbitMqService;

        //private readonly RabbitMqConfiguration _connfig;

        //private readonly IConfiguration _configuration;

        public Worker(ILogger<Worker> logger, IRabbitMqService rabbitMqService)
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


                    _logger.LogInformation(CreateUserHelper.GetSetting("Hostname"));

                    using var connection = _rabbitMqService.CreateChannel();
                    using var channel = connection.CreateModel();

                    channel.QueueDeclare(queue: "UserQueueGetUsersAsync",
                                   durable: true,
                                   exclusive: false,
                                   autoDelete: false,
                                   arguments: null);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (sender, eventArgs) =>
                    {
                        var body = eventArgs.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);

                        //var pedido = JsonSerializer.Deserialize<Pedido>(body);

                        Console.WriteLine(message?.ToString() + " Evento Processado.");
                    };

                    channel.BasicConsume(
                        
                        queue: CreateUserHelper.GetSetting("QueueNameUserGetUsersAsync"),
                        autoAck: true,
                        consumer: consumer);
                    await Task.Delay(2000, stoppingToken);
                }

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
