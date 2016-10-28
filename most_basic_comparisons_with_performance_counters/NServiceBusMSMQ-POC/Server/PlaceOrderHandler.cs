using System.Diagnostics;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Shared;

namespace Server
{
    public class PlaceOrderHandler : IHandleMessages<PlaceOrder>
    {
        private static ILog _log = LogManager.GetLogger<PlaceOrderHandler>();
        private static int _processingCount;

        private static bool _groupItems;
        private static Stopwatch _sw;
        
        public Task Handle(PlaceOrder message, IMessageHandlerContext context)
        {
            _processingCount++;

            _log.Info($"RECV {nameof(PlaceOrder)} [{_processingCount}], {nameof(message.Id)}: {message.Id}");

            var orderPlaced = new OrderPlaced {OrderId = message.Id, IsGrouped = message.IsGrouped };
            var task = context.Publish(orderPlaced);
            _log.Info($"POST {nameof(OrderPlaced)} [{_processingCount}], {nameof(orderPlaced.OrderId)}: {orderPlaced.OrderId}");

            if (message.IsGrouped != _groupItems)
            {
                // Grouping completed.
                if (_groupItems)
                {
                    _groupItems = false;
                    _log.Info(_sw.LogTimeToMessage("GROUP COMPLETE "));
                }
                // Grouping started;
                else
                {
                    _groupItems = true;
                    _sw = StopwatchExtensions.CreateStartSW();
                }
            }

            return task;
        }
    }
}
