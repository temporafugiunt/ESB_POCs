using System;
using System.Threading.Tasks;
using Shared;
using MassTransit;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            AsyncMain().GetAwaiter().GetResult();
        }

        static async Task AsyncMain()
        {
            Console.Title = "MassTransitRabitMQ-POC.Server";
            var bus = BusConfigurator.ConfigureBus((cfg, host) =>
            {
                cfg.ReceiveEndpoint(host,
                    RabbitMqQueueNames.OrderPublicationQueue, e =>
                    {
                        e.Consumer<PlaceOrderCommandConsumer>();
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
