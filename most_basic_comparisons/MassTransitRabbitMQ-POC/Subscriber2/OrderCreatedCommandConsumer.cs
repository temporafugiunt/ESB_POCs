using System;
using System.Threading.Tasks;
using MassTransit;
using Shared;

namespace Subscriber2
{
    public class OrderCreatedCommandConsumer : IConsumer<IOrderPlacedEvent>
    {
        //static ILog _log = LogManager.GetLogger<OrderCreatedCommandConsumer>();
        private static int _processingCount;

        public async Task Consume(ConsumeContext<IOrderPlacedEvent> context)
        {
            _processingCount++;
            // TODO: Need to determine MassTransit logging services and not use console.
            await Console.Out.WriteLineAsync($"RECV {nameof(IOrderPlacedEvent)} [{_processingCount}], {nameof(context.Message.OrderId)}: {context.Message.OrderId}");
        }
    }
}
