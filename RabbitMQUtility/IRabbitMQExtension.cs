using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQUtility
{
    public interface IRabbitMQExtension
    {
         T Watch<T>(string connectionString, string queueName, string deadLetterExchange, string deadLetterRouting, int timeout, Func<MessageModel, Task> messageHandler) where T : class;

         Task Send<T>(string connectionString, string queueName, string exchangeName, T DataModel, string deadLetterExchange, string deadLetterRouting, int timeout, string routingKey = "#") where T : class;
    }
}
