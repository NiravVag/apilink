using EasyNetQ;
using EasyNetQ.Topology;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQUtility
{
    public class RabbitMQExtension: IRabbitMQExtension
    {      

        #region Latest Changes with connection string
        /// <summary>
        /// This method is used to listen the rabbitmq queue
        /// </summary>
        /// <returns></returns>
        public T Watch<T>(string connectionString, string queueName, string deadLetterExchange, string deadLetterRouting, int timeout, Func<MessageModel, Task> messageHandler) where T : class
        {
            var advancedBus = RabbitHutch.CreateBus(connectionString,
                  x => x.Register(DefineBusHandler(connectionString)).EnableLegacyTypeNaming()).Advanced;

            T returnObject = null;
           
            //Register Message Handler
            var queue = advancedBus.QueueDeclare(queueName, false, true, false, false, timeout, null, null, deadLetterExchange, deadLetterRouting, null, null);
            advancedBus.Consume<string>(queue, async (msg, msgInfo) =>
            {
                await messageHandler(new MessageModel() { QueueName = queueName, Payload = JsonConvert.DeserializeObject<T>(msg.Body) });
            });
            
            return returnObject;
        }

        public Task Send<T>(string connectionString, string queueName, string exchangeName, T DataModel, string deadLetterExchange, string deadLetterRouting, int timeout, string routingKey = "#") where T : class
        {
            // TODO: need to incorporate the new logic on to the Listen Method
            using (var advancedBus = Connect(connectionString))
            {
                advancedBus.Container.Resolve<IConventions>().ErrorQueueNamingConvention = (info) => Constants.DefaultErrorQueue;
                var exchange = advancedBus.ExchangeDeclare(exchangeName, ExchangeType.Direct);
                var queue = advancedBus.QueueDeclare(queueName, false, true, false, false, timeout, null, null, deadLetterExchange, deadLetterRouting, null, null);
                var binding = advancedBus.Bind(exchange, queue, routingKey);
                var message = new Message<string>(JsonConvert.SerializeObject(DataModel));
                advancedBus.Publish<string>(exchange, routingKey, true, message);
            };
            return Task.CompletedTask;
        }

        #endregion

        private IAdvancedBus Connect(string connectionString)
        {
            return RabbitHutch.CreateBus(connectionString,
                  x => x.Register(DefineBusHandler(connectionString)).EnableLegacyTypeNaming()).Advanced;
        }


        /// <summary>
        /// This method helps to register the bus handler
        /// </summary>
        /// <returns></returns>
        public AdvancedBusEventHandlers DefineBusHandler(string connectionString)
        {
            var handler = new AdvancedBusEventHandlers(
                  disconnected: (s, e) =>
                  {
                      OnConnectionLost(s, e, connectionString);
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

        #region Bus Handlers Section

        /// <summary>
        /// This method will be triggered once the connection is disconnected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="a"></param>
        private void OnConnectionLost(object sender, EventArgs a, string connectionString)
        {
            Connect(connectionString);
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
