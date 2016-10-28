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

        public Task Handle(OrderPlaced message, IMessageHandlerContext context)
        {
            _processingCount++;

            _log.Info($"RECV {nameof(OrderPlaced)} [{_processingCount}], {nameof(message.OrderId)}: {message.OrderId}");
            return Task.CompletedTask;
        }
    }
}
