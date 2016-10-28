﻿using System;
using System.Threading.Tasks;
using NServiceBus;
using Shared;

namespace Client
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            AsyncMain().GetAwaiter().GetResult();
        }

        private static async Task AsyncMain()
        {
            // This makes it easier to tell console windows apart
            Console.Title = "NServiceBusMSMQ-POC.Client";

            // The endpoint name will be used to determine queue names and serves
            // as the address, or identity, of the endpoint
            var endpointConfiguration = new EndpointConfiguration(endpointName: "Samples.StepByStep.Client");
            
            endpointConfiguration.SendFailedMessagesTo("error");

            // Use JSON to serialize and deserialize messages (which are just
            // plain classes) to and from message queues
            endpointConfiguration.UseSerialization<JsonSerializer>();

            // Ask NServiceBus to automatically create message queues
            endpointConfiguration.EnableInstallers();

            // Store information in memory for this example, rather than in
            // a database. In this sample, only subscription information is stored
            endpointConfiguration.UsePersistence<InMemoryPersistence>();

            // Initialize the endpoint with the finished configuration
            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);
            try
            {
                await SendOrders(endpointInstance);
            }
            finally
            {
                await endpointInstance.Stop()
                    .ConfigureAwait(false);
            }
        }

        private static int _processingCount = 0;
        private static async Task SendOrders(IMessageSession endpointInstance)
        {
            Console.WriteLine("Press enter to send a message");
            Console.WriteLine("Press any key to exit");

            while (true)
            {
                var key = Console.ReadKey();
                Console.WriteLine();

                if (key.Key != ConsoleKey.Enter)
                {
                    return;
                }
                var id = Guid.NewGuid();

                var placeOrder = new PlaceOrder
                {
                    Product = "New shoes",
                    Id = id
                };
                await endpointInstance.Send("Samples.StepByStep.Server", placeOrder);
                _processingCount++;
                await Console.Out.WriteLineAsync($"POST {nameof(PlaceOrder)} [{_processingCount}], {nameof(placeOrder.Id)}: {placeOrder.Id:N}");
            }
        }
    }
}
