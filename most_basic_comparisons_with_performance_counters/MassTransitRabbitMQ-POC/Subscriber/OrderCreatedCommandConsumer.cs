using System;
using System.Diagnostics;
using System.Threading.Tasks;
using MassTransit;
using Shared;

namespace Subscriber
{
    public class OrderCreatedCommandConsumer : IConsumer<IOrderPlacedEvent>
    {
        //static ILog _log = LogManager.GetLogger<OrderCreatedCommandConsumer>();
        private static int _processingCount;

        private static bool _groupItems;
        private static Stopwatch _sw;

        public async Task Consume(ConsumeContext<IOrderPlacedEvent> context)
        {
            _processingCount++;
            // TODO: Need to determine MassTransit logging services and not use console.
            await Console.Out.WriteLineAsync($"RECV {nameof(IOrderPlacedEvent)} [{_processingCount}], {nameof(context.Message.OrderId)}: {context.Message.OrderId}");

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
