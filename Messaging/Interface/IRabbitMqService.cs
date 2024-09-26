using RabbitMQ.Client;

namespace Messaging.Interface
{
    public interface IRabbitMqService
    {
        IConnection CreateChannel();
    }
}
