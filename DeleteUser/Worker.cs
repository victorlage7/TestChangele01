using Dapper;
using Messaging.Interface;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Data.SqlClient;

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
                        string text = System.Text.Encoding.UTF8.GetString(body);
                        await Task.CompletedTask;
                        channel.BasicAck(ea.DeliveryTag, false);

                        if(text != null)

                        using (var connectionsql = new SqlConnection(DeleteUserHelper.GetConnectionString("DefaultConnection")))
                        {
                            var parameters = new { UserName = text };
                            var sql = "DELETE FROM [TechChallenge1]..[User WHERE Username =@UserName";
                            var result = connectionsql.Query(sql, parameters);
                            Console.WriteLine();
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
