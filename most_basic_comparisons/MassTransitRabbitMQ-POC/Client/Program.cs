using System;
using System.Threading.Tasks;
using MassTransit;
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
            Console.Title = "MassTransitRabitMQ-POC.Client";

            var bus = BusConfigurator.ConfigureBus();
            await bus.StartAsync();
            var sendToUri = new Uri(RabbitMqServerConstants.RabbitMqUri + RabbitMqQueueNames.OrderPublicationQueue);
            var endpointInstance = await bus.GetSendEndpoint(sendToUri);

            try
            {
                await SendOrders(endpointInstance);
            }
            finally
            {
                await bus.StopAsync();
            }
        }

        private static int _processingCount = 0;

        private static async Task SendOrders(ISendEndpoint endpointInstance)
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

                await endpointInstance.Send<IPlaceOrderCommand>(new
                {
                    Product = "New shoes",
                    Id = id
                });
                _processingCount++;
                await Console.Out.WriteLineAsync($"POST {nameof(IPlaceOrderCommand)} [{_processingCount}], {nameof(IPlaceOrderCommand.Id)}: {id:N}");
            }
        }
    }
}
