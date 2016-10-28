using System;
using System.Threading.Tasks;
using Shared;
using MassTransit;

namespace Subscriber
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            AsyncMain().GetAwaiter().GetResult();
        }

        private static async Task AsyncMain()
        {
            Console.Title = "MassTransitRabitMQ-POC.Subscriber";
            var bus = BusConfigurator.ConfigureBus((cfg, host) =>
            {
                cfg.ReceiveEndpoint(host,
                    $"{RabbitMqQueueNames.OrderPublicationQueue}.Subscriber", e =>
                    {
                        e.Consumer<OrderCreatedCommandConsumer>();
                        e.Durable = true;
                    });
            });

            await bus.StartAsync();

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

            await bus.StopAsync();
        }
    }
}
