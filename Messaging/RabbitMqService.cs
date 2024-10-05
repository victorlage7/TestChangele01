using Messaging.Interface;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Messaging
{
    public class RabbitMqService : IRabbitMqService
    {
        private readonly RabbitMqConfiguration _configuration;
        public RabbitMqService(IOptions<RabbitMqConfiguration> options)
        {
            _configuration = options.Value;
        }
        public IConnection CreateChannel()
        {
            ConnectionFactory connection = new ConnectionFactory()
            {
                UserName = _configuration.Username,
                Password = _configuration.Password,
                HostName = _configuration.HostName,
                Port = _configuration.Port
            };

            connection.DispatchConsumersAsync = true;
            var channel = connection.CreateConnection();
            return channel;
        }

        public IConnection CreateChannel(string username, string password, string hostName, int port)
        {
            ConnectionFactory connection = new ConnectionFactory()
            {
                UserName = username,
                Password = password,
                HostName = hostName,
                Port = port
            };

            connection.DispatchConsumersAsync = true;
            var channel = connection.CreateConnection();
            return channel;
        }
    }
}
