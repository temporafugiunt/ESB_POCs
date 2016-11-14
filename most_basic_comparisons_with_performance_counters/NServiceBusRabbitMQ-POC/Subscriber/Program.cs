using System;
using System.Threading.Tasks;
using NServiceBus;

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
            Console.Title = "NServiceBusMSMQ-POC.Subscriber";
            var endpointConfiguration = new EndpointConfiguration("Samples.StepByStep.Subscriber");
            endpointConfiguration.UseSerialization<JsonSerializer>();
            endpointConfiguration.EnableInstallers();
            
            endpointConfiguration.SendFailedMessagesTo("Samples.StepByStep.Errors");

            endpointConfiguration.UsePersistence<InMemoryPersistence>();

            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
            transport.ConnectionString("host=pvcd-api.pvops.com;username=pvadmin;password=pvadmin1$");

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);
            try
            {
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
            finally
            {
                await endpointInstance.Stop()
                    .ConfigureAwait(false);
            }
        }
    }
}
