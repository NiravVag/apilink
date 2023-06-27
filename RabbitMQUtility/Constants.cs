namespace RabbitMQUtility
{
    public class Constants
    {
        internal const string DefaultQueue = "ha.DefaultQueue";

        internal const string DefaultErrorQueue = "ha.DefaultErrorQueue";

        internal const string DefaultRoutingKey = "#";

        internal const string Interupted = "Bus Connection Interupted";

        internal const string Established = "Bus Connection Established";

        internal const string Blocked = "Bus Connection Blocked";

        internal const string Unblocked = "Bus Connection Unblocked";

        internal const string MessageReturned = "Message Returned";

        internal const string ExchangeType = "direct";
    }
}
