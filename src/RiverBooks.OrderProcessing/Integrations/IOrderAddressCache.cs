
using Ardalis.Result;
using Microsoft.Extensions.Logging;
using Serilog;
using StackExchange.Redis;
using System.Text.Json;

namespace RiverBooks.OrderProcessing.Integrations;

internal interface IOrderAddressCache
{
    Task<Result<OrderAddress>> GetByIdAsync(Guid addressId);
    Task<Result> StoreAsync(OrderAddress orderAddress);
}

internal class RedisOrderAddressCache : IOrderAddressCache
{
    private readonly IDatabase _db;
    private readonly ILogger<RedisOrderAddressCache> _logger;

    public RedisOrderAddressCache(ILogger<RedisOrderAddressCache> logger)
    {
        //TODO: get this from config
        ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");
        _db = redis.GetDatabase();
        _logger = logger;
    }

    public async Task<Result<OrderAddress>> GetByIdAsync(Guid addressId)
    {
        string? rawJson = await _db.StringGetAsync(addressId.ToString());

        if (rawJson is null)
        {
            _logger.LogWarning("Address {id} not foun in {db}", addressId, "REDIS");
            return Result.NotFound();
        }

        OrderAddress? address = JsonSerializer.Deserialize<OrderAddress>(rawJson);
        if(address is null) return Result.NotFound();

        _logger.LogInformation("Address {id} returned from {db}", addressId, "REDIS");
        return Result.Success(address);
    }

    public async Task<Result> StoreAsync(OrderAddress orderAddress)
    {
        var key = orderAddress.Id.ToString();
        var address = JsonSerializer.Serialize(orderAddress);

        await _db.StringSetAsync(key, address);
        _logger.LogInformation("Address {id} stored in {db}", orderAddress.Id, "REDIS");

        return Result.Success();
    }
}