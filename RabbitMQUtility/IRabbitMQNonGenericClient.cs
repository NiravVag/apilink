namespace RabbitMQUtility
{
    using System;
    using System.Threading.Tasks;

    public interface IRabbitMQNonGenericClient
    {        
        event Func<ErrorModel, Task> OnErrorReceieved;

        Task ConsumeErrors();
        object Listen(string queueName);
        Task Publish(string queueName, object DataModel);
    }
}
