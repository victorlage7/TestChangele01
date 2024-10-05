using ContactCore;
using Dapper;
using Messaging.Interface;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Data.SqlClient;
using System.Text.Json;

namespace DeleteContact
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

                    Contact contact;

                    using var connection = _rabbitMqService.CreateChannel(
                      ContactUserHelper.GetSetting("UserName")
                      , ContactUserHelper.GetSetting("Password")
                      , ContactUserHelper.GetSetting("Hostname")
                      , Int32.Parse(ContactUserHelper.GetSetting("Port")));

                    using var channel = connection.CreateModel();

                    channel.QueueDeclare(queue: ContactUserHelper.GetSetting("QueueNameDeleteContact"),
                                   durable: true,
                                   exclusive: false,
                                   autoDelete: false,
                                   arguments: null); var consumer = new AsyncEventingBasicConsumer(channel);
                    consumer.Received += async (ch, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        string text = System.Text.Encoding.UTF8.GetString(body);
                        contact = JsonSerializer.Deserialize<Contact>(text);
                        await Task.CompletedTask;
                        channel.BasicAck(ea.DeliveryTag, false);

                        if (text != null)
                        {

                            try
                            {
                                using (var connectionsql = new SqlConnection(ContactUserHelper.GetConnectionString("DefaultConnection")))
                                {
                                    var newRow = new { ContactId = contact.ContactId, };                                    
                                    var sql = "delete from [TechChallenge1]..[Contact] where ContactId = @ContactId";
                                    var rowsAffected = connectionsql.Execute(sql, newRow);
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex);
                            }
                        }
                    };

                    channel.BasicConsume(
                    queue: ContactUserHelper.GetSetting("QueueNameDeleteContact"),
                    autoAck: true,
                    consumer: consumer);
                    await Task.Delay(2000, stoppingToken);
                }
                await Task.Delay(1000, stoppingToken);

            }
            await Task.Delay(1000, stoppingToken);
        }
    }
}
