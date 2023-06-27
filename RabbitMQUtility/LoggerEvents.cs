namespace RabbitMQUtility
{
    class LoggerEvents
    {
        public const int ConnectionLost = 7000;
        public const int ConnnectionEstablished = 7001;
        public const int MessageReceived = 7002;
        public const int MessageSent = 7003;
        public const int MessageException = 7004;
        public const int UnabletoRegisterHandlerException = 7004;
        public const int UnabletoReadMessage = 7005;
    }
}
