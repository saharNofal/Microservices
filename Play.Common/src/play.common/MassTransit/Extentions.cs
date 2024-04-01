using System.Reflection;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Play.common.Settings;


namespace Play.common.MassTransit
{
    public static class Extentions
    {
        public static IServiceCollection AddMassTransitWithRabbitMQ(this IServiceCollection services)
        {
            services.AddMassTransit(configure =>
                {
                    configure.AddConsumers(Assembly.GetEntryAssembly());
                    configure.UsingRabbitMq((context, configurator) =>
                    {
                        var configuration = context.GetService<IConfiguration>();
                        var serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
                        var rabbitMQSettings = configuration.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>();
                        configurator.Host(rabbitMQSettings.Host);
                        configurator.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter(serviceSettings.ServiceName, false));
                    });
                });


            //services.AddMassTransitHostedService();
            return services;
        }

    }
}
