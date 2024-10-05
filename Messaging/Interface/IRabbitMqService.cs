using RabbitMQ.Client;

namespace Messaging.Interface
{
    public interface IRabbitMqService
    {
        IConnection CreateChannel();

        IConnection CreateChannel(string username, string password, string hostName, int port);
    }
}
