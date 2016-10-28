using System;
using System.Diagnostics;
using System.Threading.Tasks;
using MassTransit;
using Shared;

namespace Server
{
    public class PlaceOrderCommandConsumer : IConsumer<IPlaceOrderCommand>
    {
        //static ILog _log = LogManager.GetLogger<PlaceOrderCommandConsumer>();
        private static int _processingCount;

        private static bool _groupItems;
        private static Stopwatch _sw;
        
        public async Task Consume(ConsumeContext<IPlaceOrderCommand> context)
        {
            _processingCount++;

            // TODO: Need to determine MassTransit logging services and not use console.
            await Console.Out.WriteLineAsync($"RECV {nameof(IPlaceOrderCommand)} [{_processingCount}], {nameof(context.Message.Id)}: {context.Message.Id}");
            
            //notify subscribers that a order is registered
            await context.Publish<IOrderPlacedEvent>(new { OrderId = context.Message.Id, context.Message.IsGrouped });
            await Console.Out.WriteLineAsync($"POST {nameof(IOrderPlacedEvent)} [{_processingCount}], {nameof(IOrderPlacedEvent.OrderId)}: {context.Message.Id}");

            if (context.Message.IsGrouped != _groupItems)
            {
                // Grouping completed.
                if (_groupItems)
                {
                    _groupItems = false;
                    _sw.LogTimeToConsole("GROUP COMPLETE ");
                }
                // Grouping started;
                else
                {
                    _groupItems = true;
                    _sw = StopwatchExtensions.CreateStartSW();
                }
            }
        }
    }
}
