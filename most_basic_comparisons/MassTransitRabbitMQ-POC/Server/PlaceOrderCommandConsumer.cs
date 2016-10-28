using System;
using System.Threading.Tasks;
using MassTransit;
using Shared;

namespace Server
{
    public class PlaceOrderCommandConsumer : IConsumer<IPlaceOrderCommand>
    {
        //static ILog _log = LogManager.GetLogger<PlaceOrderCommandConsumer>();
        private static int _processingCount;

        public async Task Consume(ConsumeContext<IPlaceOrderCommand> context)
        {
            _processingCount++;

            // TODO: Need to determine MassTransit logging services and not use console.
            await Console.Out.WriteLineAsync($"RECV {nameof(IPlaceOrderCommand)} [{_processingCount}], {nameof(context.Message.Id)}: {context.Message.Id}");
            
            //notify subscribers that a order is registered
            await context.Publish<IOrderPlacedEvent>(new { OrderId = context.Message.Id });
            await Console.Out.WriteLineAsync($"POST {nameof(IOrderPlacedEvent)} [{_processingCount}], {nameof(IOrderPlacedEvent.OrderId)}: {context.Message.Id}");
        }
    }
}
