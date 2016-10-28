using System;

namespace Shared
{
    public interface IPlaceOrderCommand
    {
        Guid Id { get; }
        string Product { get; }
        bool IsGrouped { get; }
    }
}
