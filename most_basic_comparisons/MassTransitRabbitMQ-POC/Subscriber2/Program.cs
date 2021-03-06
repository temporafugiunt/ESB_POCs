﻿using System;
using System.Threading.Tasks;
using MassTransit;
using Shared;

namespace Subscriber2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            AsyncMain().GetAwaiter().GetResult();
        }

        private static async Task AsyncMain()
        {
            Console.Title = "MassTransitRabitMQ-POC.Subscriber2";
            var bus = BusConfigurator.ConfigureBus((cfg, host) =>
            {
                cfg.ReceiveEndpoint(host,
                    $"{RabbitMqQueueNames.OrderPublicationQueue}.Subscriber2", e =>
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
