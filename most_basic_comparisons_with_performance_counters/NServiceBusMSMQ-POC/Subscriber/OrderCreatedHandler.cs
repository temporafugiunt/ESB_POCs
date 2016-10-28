using System.Diagnostics;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Shared;

namespace Subscriber
{
    public class OrderCreatedHandler : IHandleMessages<OrderPlaced>
    {
        private static ILog _log = LogManager.GetLogger<OrderCreatedHandler>();
        private static int _processingCount;

        private static bool _groupItems;
        private static Stopwatch _sw;

        public Task Handle(OrderPlaced message, IMessageHandlerContext context)
        {
            _processingCount++;

            _log.Info($"RECV {nameof(OrderPlaced)} [{_processingCount}], {nameof(message.OrderId)}: {message.OrderId}");

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

            return Task.CompletedTask;
        }
    }
}
