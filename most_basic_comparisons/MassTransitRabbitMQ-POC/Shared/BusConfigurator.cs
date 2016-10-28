using System;
using MassTransit;
using MassTransit.RabbitMqTransport;

namespace Shared
{
    public static class BusConfigurator
    {
        public static IBusControl ConfigureBus(
            Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost>
                registrationAction = null)
        {
            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri(RabbitMqServerConstants.RabbitMqUri), hst =>
                {
                    hst.Username(RabbitMqServerConstants.UserName);
                    hst.Password(RabbitMqServerConstants.Password);
                });

                registrationAction?.Invoke(cfg, host);
            });
        }
    }

}
