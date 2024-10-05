using ContactCore;
using Dapper;
using Messaging.Interface;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Data.SqlClient;
using System.Text.Json;

namespace CreateContact
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

                    channel.QueueDeclare(queue: ContactUserHelper.GetSetting("QueueNameCreateContact"),
                                   durable: true,
                                   exclusive: false,
                                   autoDelete: false,
                                   arguments: null);

                    var consumer = new AsyncEventingBasicConsumer(channel);
                    consumer.Received += async (ch, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        string text = System.Text.Encoding.UTF8.GetString(body);
                        //contact = Newtonsoft.Json.JsonConvert.DeserializeObject<Contact>(text);
                        contact = JsonSerializer.Deserialize<Contact>(text);
                        await Task.CompletedTask;
                        channel.BasicAck(ea.DeliveryTag, false);

                        if (text != null)
                        {

                            try
                            {
                                using (var connectionsql = new SqlConnection(ContactUserHelper.GetConnectionString("DefaultConnection")))
                                {
                                    var sql = "INSERT INTO [TechChallenge1]..[Contact] (ContactId, Name) VALUES (@ContactId,@Name)";
                                    var newRow = new { ContactId = contact.ContactId, Name = contact.Name };
                                    var rowsAffected = connectionsql.Execute(sql, newRow);


                                    foreach (var row in contact.EmailAddresses)
                                    {
                                        var sqlAddress = "INSERT INTO [TechChallenge1]..[ContactEmails] (Type,Address, ContactId) VALUES (@EmailType,@Address,@ContactId)";
                                        var newRowAddress = new { EmailType = row.Type, Address = row.Address, ContactId = contact.ContactId, };
                                        var rowsAffectedAddress = connectionsql.Execute(sqlAddress, newRowAddress);
                                    }

                                    foreach (var row in contact.PhoneNumbers)
                                    {
                                        var sqlPhones = "INSERT INTO [TechChallenge1]..[ContactPhoneNumbers] (Type,CountryCode,AreaCode, Number,ContactId) VALUES " +
                                        "(@PhoneNumberType,@CountryCode,@AreaCode,@Number,@ContactId)";
                                        var newRowPhones = new { PhoneNumberType = row.Type,
                                            CountryCode = row.CountryCode,
                                            AreaCode = row.AreaCode,
                                            Number = row.Number,
                                            ContactId = contact.ContactId
                                        };
                                        var rowsAffectedPhones = connectionsql.Execute(sqlPhones, newRowPhones);
                                    }
                                }
                            }
                            catch (Exception ex) 
                            {
                                Console.WriteLine(ex);
                            }

                            
                        }
                    };

                channel.BasicConsume(
                queue: ContactUserHelper.GetSetting("QueueNameCreateContact"),
                autoAck: true,
                consumer: consumer);
                await Task.Delay(2000, stoppingToken);
                }
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
