A .NET API Client for RabbitMQ

Api Client:

Step 1: Add the Namespace:

    using RabbitMQUtility;

Step 2: Configure the RabbitMQ Service on Startup

    public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddRabbitMQGenericClient(o => o.ConnectionString = "RabbitMQ Connection String");
        }

Step 3:  Inject the RabbitMQ Service on Controller

    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly IRabbitMQGenericClient _rabbitMQClient;
        public ValuesController(IRabbitMQGenericClient rabbitMQClient)
        {
            _rabbitMQClient = rabbitMQClient;
        }

Step 4: Publish the Message
 
     await _rabbitMQClient.Publish<DataModel>("TestQueue", new DataModel()
            {
                Name= "TestName",
                Location= "TestLocation",                
            });

Step 6: Register the Message Handler

     private Task ProcessMessage(MessageModel message)
        {

            return Task.CompletedTask;
        }

    _rabbitMQClient.OnMessageReceieved += ProcessMessage;

Step 7: Listen the Queue 
 
   _rabbitMQClient.Listen<DataModel>("TestQueue");

Step 8: Register the Error Handler

  private Task ProcesError(ErrorModel message)
        {

            return Task.CompletedTask;
        }

  _rabbitMQClient.OnErrorReceieved += ProcesError;

Step 9: Listen the Error Queue 

  _rabbitMQClient.ConsumeErrors();


Console Client:

Step 1: Configure the service

     internal static void ConfigureServices(IServiceCollection services)
        {
            // Services Registration                 
            
            services.AddRabbitMQClient(o=>o.ConnectionString= "host=localhost:5672,localhost:5673,localhost:5674,localhost:5675");
        }


Step 2: Get RabbitMQ Instance
        
      var rabbitMQListener = Services.GetService<IRabbitMQGenericClient>();
 

Step 4: Publish the Message
 
     await _rabbitMQClient.Publish<DataModel>("TestQueue", new DataModel()
            {
                Name= "TestName",
                Location= "TestLocation",                
            });

Step 6: Register the Message Handler

     private Task ProcessMessage(MessageModel message)
        {

            return Task.CompletedTask;
        }

    _rabbitMQClient.OnMessageReceieved += ProcessMessage;

Step 7: Listen the Queue 
 
   _rabbitMQClient.Listen<DataModel>("TestQueue");

Step 8: Register the Error Handler

  private Task ProcesError(ErrorModel message)
        {

            return Task.CompletedTask;
        }

  _rabbitMQClient.OnErrorReceieved += ProcesError;

Step 9: Listen the Error Queue 

  _rabbitMQClient.ConsumeErrors();




