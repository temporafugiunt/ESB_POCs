﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using MassTransit;
using Shared;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            AsyncMain().GetAwaiter().GetResult();
        }

        static async Task AsyncMain()
        {
            // This makes it easier to tell console windows apart
            Console.Title = "MassTransitRabitMQ-POC.Client";

            var bus = BusConfigurator.ConfigureBus();

            var sendToUri = new Uri(RabbitMqServerConstants.RabbitMqUri + RabbitMqQueueNames.OrderPublicationQueue);
            var endpointInstance = await bus.GetSendEndpoint(sendToUri);

            await SendOrders(endpointInstance);
        }

        private static int _processingCount = 0;

        private static async Task SendOrders(ISendEndpoint endpointInstance)
        {
            Console.WriteLine("Press enter to send 1 message or a number + enter to send X messages.");
            Console.WriteLine("Type 'exit' to exit");
            Console.WriteLine();

            while (true)
            {
                var keys = Console.ReadLine();
                int count;

                if (keys == "")
                {
                    count = 1;
                }
                else if (string.Compare(keys, "exit", StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    return;
                }
                else
                {
                    if (!int.TryParse(keys, out count))
                    {
                        await Console.Out.WriteLineAsync("Not Recognized.");
                    }
                }
                var messageSent = false;
                var sw = StopwatchExtensions.CreateStartSW();
                for (var inc = 0; inc < count; inc++)
                {
                    await SendOrder(endpointInstance, count > 1 && inc != count -1);
                }
                await sw.LogTimeToConsoleAsync($"All {count} message[s] sent.");
            }
        }

        private static async Task SendOrder(ISendEndpoint endpointInstance, bool isGrouped)
        {
            var id = Guid.NewGuid();

            await endpointInstance.Send<IPlaceOrderCommand>(new
            {
                Product = "New shoes",
                Id = id,
                IsGrouped = isGrouped
            });
            _processingCount++;
            await Console.Out.WriteLineAsync($"{_processingCount} - Sent a PlaceOrder message with id: {id:N} isGrouped:{isGrouped}");
        }
    }
}
