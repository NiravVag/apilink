namespace RabbitMQUtility
{
    using System;
    using Microsoft.Extensions.Logging;
    using EasyNetQ;
    using Newtonsoft.Json;
    using EasyNetQ.SystemMessages;
    using System.Threading.Tasks;
    using EasyNetQ.Topology;
    using Microsoft.Extensions.Options;

    public class RabbitMQGenericClient : IRabbitMQGenericClient
    {
        //Fields
        private IExchange sourceExchange;
        private readonly IAdvancedBus _advancedBus;        

        public string ExchangeName { get; set; }
        private string ErrorQueueName { get; set; } = Constants.DefaultErrorQueue;
        private string RoutingKey { get; set; } = Constants.DefaultRoutingKey;
        
        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="logger"></param>
        public RabbitMQGenericClient(IAdvancedBus advancedBus)
        {                                
            _advancedBus = advancedBus;
        }

        #region Implementation

        /// <summary>
        /// This method helps to register the bus handler
        /// </summary>
        /// <returns></returns>
        public AdvancedBusEventHandlers DefineBusHandler()
        {
            var handler = new AdvancedBusEventHandlers(
                  disconnected: (s, e) =>
                  {
                      OnConnectionLost(s, e);
                  },
                  connected: (s, e) =>
                  {
                      OnConnectionEstablished(s, e);
                  },
                  messageReturned: (s, e) =>
                  {
                      OnMessageReturned(s, e);
                  },
                  unblocked: (s, e) =>
                  {
                      OnConnectionUnblocked(s, e);
                  },
                  blocked: (s, e) =>
                  {
                      OnConnectionBlocked(s, e);
                  }
                  );
            return handler;
        }

        /// <summary>
        /// This method is used to listen the rabbitmq queue
        /// </summary>
        /// <returns></returns>
        public T Listen<T>(string queueName, Func<MessageModel, Task> messageHandler) where T : class
        {
            T returnObject = null;
          
            //Register Message Handler
            var queue = _advancedBus.QueueDeclare(queueName);
            _advancedBus.Consume<string>(queue, async (msg, msgInfo) =>
            {

                await messageHandler(new MessageModel() { QueueName = queueName, Payload = JsonConvert.DeserializeObject<T>(msg.Body) });
            });
            return returnObject;
        }
        private void OnError(IMessage<Error> error, MessageReceivedInfo info, IAdvancedBus advancedBus, Func<ErrorModel, Task> errorHandler)
        {
            Type originalMsgType = null;

            if (error.Body.BasicProperties.TypePresent)
            {
                var typeNameSerializer = advancedBus.Container.Resolve<ITypeNameSerializer>();
                originalMsgType = typeNameSerializer.DeSerialize(error.Body.BasicProperties.Type);
            }
            else
            {
                originalMsgType = error.GetType();
            }

            dynamic originalMessage = JsonConvert.DeserializeObject(error.Body.Message, originalMsgType);
            errorHandler(new ErrorModel() { OriginalMessage = originalMessage, ExceptionMessage = error.ToString() });
        }

        public Task Publish<T>(string queueName, T DataModel) where T : class
        {          
            //Register Message Handler
            var queue = _advancedBus.QueueDeclare(queueName);
            var message = new Message<string>(JsonConvert.SerializeObject(DataModel));
            sourceExchange = GetExchangeName();
            _advancedBus.Publish(sourceExchange, queueName, false, message);
            return Task.CompletedTask;
        }

        public Task ConsumeErrors(Func<ErrorModel, Task> errorHandler)
        {         
            //Register Error Handler
            var subscriptionErrorQueue = _advancedBus.QueueDeclare(ErrorQueueName);            
            var source = _advancedBus.ExchangeDeclare(ErrorQueueName, Constants.ExchangeType);
            _advancedBus.Bind(source, subscriptionErrorQueue, RoutingKey);
            _advancedBus.Consume<Error>(subscriptionErrorQueue, (error, info) =>
            {
                OnError(error, info, _advancedBus, errorHandler);
            });
            return Task.CompletedTask;
        }

        private IExchange GetExchangeName()
        {
            if (ExchangeName != null)
                return _advancedBus.ExchangeDeclare(ExchangeName, Constants.ExchangeType);
            else
            {
                return Exchange.GetDefault();
            }
        }
        #endregion

        #region Bus Handlers Section

        /// <summary>
        /// This method will be triggered once the connection is disconnected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="a"></param>
        private void OnConnectionLost(object sender, EventArgs a)
        {
            //_logger.LogInformation(LoggerEvents.ConnectionLost, Constants.Interupted);
            //CreateConnectionIfNotExists(_options);
        }

        /// <summary>
        /// This method will be triggered once the connection is established
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="a"></param>
        private void OnConnectionEstablished(object sender, EventArgs a)
        {
            // _logger.LogInformation(LoggerEvents.ConnnectionEstablished, Constants.Established);
        }

        /// <summary>
        /// This method will be triggered once the connection is blocked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="a"></param>
        private void OnConnectionBlocked(object sender, EventArgs a)
        {
            //_logger.LogError(LoggerEvents.MessageException, Constants.Blocked);
        }

        /// <summary>
        /// This method will be triggered once the connection is unblocked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="a"></param>
        private void OnConnectionUnblocked(object sender, EventArgs a)
        {
            // _logger.LogInformation(LoggerEvents.ConnnectionEstablished, Constants.Unblocked);
        }

        /// <summary>
        /// This method will be triggered when the message is returned from queue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="a"></param>
        private void OnMessageReturned(object sender, EventArgs a)
        {
            //_logger.LogError(LoggerEvents.MessageException, Constants.MessageReturned);
        }

        #endregion

    }
}
