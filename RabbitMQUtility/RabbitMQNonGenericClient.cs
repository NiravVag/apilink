namespace RabbitMQUtility
{
    using System;
    using EasyNetQ;
    using Newtonsoft.Json;
    using EasyNetQ.SystemMessages;
    using System.Threading.Tasks;
    using EasyNetQ.Topology;
    using Microsoft.Extensions.Options;


    public class RabbitMQNonGenericClient : IRabbitMQNonGenericClient, IDisposable
    {
        //Fields
        private IExchange sourceExchange;

        private static IAdvancedBus advancedBus;

        public string ExchangeName { get; set; }

        private string ErrorQueueName { get; set; } = Constants.DefaultErrorQueue;

        private string RoutingKey { get; set; } = Constants.DefaultRoutingKey;

        private readonly RabbitMQOptions _options;

        //Handlers
        public event Func<object, Task> OnMessageReceieved;

        public event Func<ErrorModel, Task> OnErrorReceieved;

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="logger"></param>
        public RabbitMQNonGenericClient(IOptions<RabbitMQOptions> rabbitMQOptions)
        {
            _options = rabbitMQOptions.Value;
            advancedBus = CreateConnectionIfNotExists(_options);
        }

        #region Bus Handlers Section

        /// <summary>
        /// This method will be triggered once the connection is disconnected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="a"></param>
        public void OnConnectionLost(object sender, EventArgs a)
        {
            CreateConnectionIfNotExists(_options);
        }

        /// <summary>
        /// This method will be triggered once the connection is established
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="a"></param>
        public void OnConnectionEstablished(object sender, EventArgs a)
        {
        }

        /// <summary>
        /// This method will be triggered once the connection is blocked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="a"></param>
        public void OnConnectionBlocked(object sender, EventArgs a)
        {

        }

        /// <summary>
        /// This method will be triggered once the connection is unblocked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="a"></param>
        public void OnConnectionUnblocked(object sender, EventArgs a)
        {

        }

        /// <summary>
        /// This method will be triggered when the message is returned from queue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="a"></param>
        public void OnMessageReturned(object sender, EventArgs a)
        {

        }

        #endregion

        #region Connectivity Section

        /// <summary>
        /// Establishes a connection to RabbitMQ
        /// </summary>
        private IAdvancedBus CreateConnectionIfNotExists(RabbitMQOptions rabbitMQOptions)
        {
            advancedBus = RabbitHutch.CreateBus(rabbitMQOptions.ConnectionString,
                   x => x.Register(DefineBusHandler()).EnableLegacyTypeNaming()).Advanced;
            advancedBus.Container.Resolve<IConventions>().ErrorQueueNamingConvention = (info) => ErrorQueueName;
            return advancedBus;
        }

        /// <summary>
        /// Disposes the RabbitMQ Connection
        /// </summary>
        public void Dispose()
        {
            advancedBus.Dispose();
        }

        #endregion

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
        public object Listen(string queueName)
        {
            object returnObject = null;
            if (!advancedBus.IsConnected)
            {
                CreateConnectionIfNotExists(_options);
            }
            //Register Message Handler
            var queue = advancedBus.QueueDeclare(queueName);
            advancedBus.Consume<object>(queue, (msg, msgInfo) =>
            {
                returnObject =  msg.Body;
            });


            return returnObject;
        }

        private void OnError(IMessage<Error> error, MessageReceivedInfo info, IAdvancedBus advancedBus)
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
            OnErrorReceieved(new ErrorModel() { OriginalMessage = originalMessage, ExceptionMessage = error.ToString() });
        }

        public Task Publish(string queueName, object DataModel)
        {
            if (!advancedBus.IsConnected)
            {
                CreateConnectionIfNotExists(_options);
            }
            //Register Message Handler
            var queue = advancedBus.QueueDeclare(queueName);
            var message = new Message<object>(DataModel);
            sourceExchange = GetExchangeName();
            advancedBus.Publish(sourceExchange, queueName, false, message);
            return Task.CompletedTask;
        }

        public Task ConsumeErrors()
        {
            if (!advancedBus.IsConnected)
            {
                CreateConnectionIfNotExists(_options);
            }
            //Register Error Handler
            var subscriptionErrorQueue = advancedBus.QueueDeclare(ErrorQueueName);
            sourceExchange = GetExchangeName();
            advancedBus.Bind(sourceExchange, subscriptionErrorQueue, RoutingKey);
            advancedBus.Consume<Error>(subscriptionErrorQueue, (error, info) =>
            {
                OnError(error, info, advancedBus);
            });
            return Task.CompletedTask;
        }


        private IExchange GetExchangeName()
        {
            if (ExchangeName != null)
                return advancedBus.ExchangeDeclare(ExchangeName, Constants.ExchangeType);
            else
            {
                return Exchange.GetDefault();
            }
        }
        #endregion

    }
}
