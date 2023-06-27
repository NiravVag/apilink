namespace RabbitMQUtility
{
    using System;
    using Autofac;
    using EasyNetQ;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    public static class RabbitMQServiceCollectionExtensions
    {
        #region Generic Implementation

        public static IServiceCollection AddRabbitMQClientExtensions(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAdd(ServiceDescriptor.Singleton<IRabbitMQExtension, RabbitMQExtension>());
            return services;
        }


        public static IServiceCollection AddRabbitMQGenericClient(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAdd(ServiceDescriptor.Singleton<IRabbitMQGenericClient, RabbitMQGenericClient>());
            return services;
        }

        public static IServiceCollection AddRabbitMQGenericClient(this IServiceCollection services, string configureOptions)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configureOptions == null)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }

            var bus = GetAdvancedBus(configureOptions);
            services.TryAdd(ServiceDescriptor.Singleton(bus));
            services.TryAdd(ServiceDescriptor.Singleton<IRabbitMQGenericClient, RabbitMQGenericClient>());
            return services;
        }

        private static IAdvancedBus GetAdvancedBus(string rabbitMQOptions)
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<IBus>().SingleInstance();
            containerBuilder.RegisterEasyNetQ(rabbitMQOptions); //c => c.EnableLegacyTypeNaming());
            var container = containerBuilder.Build();
            return container.Resolve<IBus>().Advanced;
        }

        #endregion
    }
}
