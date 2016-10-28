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

        public Task Handle(PlaceOrder message, IMessageHandlerContext context)
        {
            _processingCount++;

            _log.Info($"RECV {nameof(PlaceOrder)} [{_processingCount}], {nameof(message.Id)}: {message.Id}");

            var orderPlaced = new OrderPlaced {OrderId = message.Id};
            var task = context.Publish(orderPlaced);
            _log.Info($"POST {nameof(OrderPlaced)} [{_processingCount}], {nameof(orderPlaced.OrderId)}: {orderPlaced.OrderId}");
            return task;
        }
    }
}
