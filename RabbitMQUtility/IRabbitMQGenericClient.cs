using System;
using System.Threading.Tasks;

namespace RabbitMQUtility
{
    public interface IRabbitMQGenericClient
    {
        T Listen<T>(string queueName, Func<MessageModel, Task> messageHandler) where T : class;

        Task Publish<T>(string queueName, T DataModel) where T : class;

        Task ConsumeErrors(Func<ErrorModel, Task> errorHandler);
        string ExchangeName { get; set; }
    }
}