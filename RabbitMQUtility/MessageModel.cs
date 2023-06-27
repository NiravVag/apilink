namespace RabbitMQUtility
{
    public class MessageModel
    {
        public string QueueName { get; set; }
        public object Payload { get; set; }
    }
}