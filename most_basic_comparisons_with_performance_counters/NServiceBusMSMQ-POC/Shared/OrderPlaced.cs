using System;
using NServiceBus;

namespace Shared
{
    public class OrderPlaced : IEvent
    {
        public Guid OrderId { get; set; }
        public bool IsGrouped { get; set; }
    }
}
