
using Ardalis.Result;
using RiverBooks.OrderProcessing.Integrations;
using Serilog;

namespace RiverBooks.OrderProcessing;

internal interface IOrderAddressCache
{
    Task<Result<OrderAddress>> GetByIdAsync(Guid addressId);
    Task<Result> StoreAsync(OrderAddress orderAddress);
}
