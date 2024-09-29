using Core.Entities;
using Dapper;
using Messaging.Interface;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Data.SqlClient;
using System.Text.Json;

namespace DeleteUser
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        private readonly IRabbitMqService _rabbitMqService;

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
                    User user = null;

                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                    using var connection = _rabbitMqService.CreateChannel(
                        DeleteUserHelper.GetSetting("UserName")
                        , DeleteUserHelper.GetSetting("Password")
                        , DeleteUserHelper.GetSetting("Hostname")
                        , Int32.Parse(DeleteUserHelper.GetSetting("Port")));

                    using var channel = connection.CreateModel();

                    channel.QueueDeclare(queue: DeleteUserHelper.GetSetting("QueueNameDeleteUser"),
                                   durable: true,
                                   exclusive: false,
                                   autoDelete: false,
                                   arguments: null);

                    var consumer = new AsyncEventingBasicConsumer(channel);
                    consumer.Received += async (ch, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var text = System.Text.Encoding.UTF8.GetString(body);
                        user = JsonSerializer.Deserialize<User>(body);
                        await Task.CompletedTask;
                        channel.BasicAck(ea.DeliveryTag, false);

                        if (user != null)
                        {
                            using (var connectionsql = new SqlConnection(DeleteUserHelper.GetConnectionString("DefaultConnection")))
                            {
                                var sql = "Delete [TechChallenge1]..[User] where Id = @Id";
                                var parameter = new { Id = user.Id };
                                var rowsAffected = connectionsql.Execute(sql, parameter);
                                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                            }
                        }
                    };

                    channel.BasicConsume(
                    queue: DeleteUserHelper.GetSetting("QueueNameDeleteUser"),
                    autoAck: true,
                    consumer: consumer);
                    await Task.Delay(2000, stoppingToken);
                }

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
