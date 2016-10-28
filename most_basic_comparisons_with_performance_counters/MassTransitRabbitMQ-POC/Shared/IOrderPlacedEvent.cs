using System;

namespace Shared
{
    public interface IOrderPlacedEvent
    {
        Guid OrderId { get; set; }
        bool IsGrouped { get; set; }
    }
}
