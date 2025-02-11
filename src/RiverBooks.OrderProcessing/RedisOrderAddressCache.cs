
using Ardalis.Result;
using MediatR;
using Microsoft.Extensions.Logging;
using RiverBooks.OrderProcessing.Integrations;
using StackExchange.Redis;
using System.Text.Json;

namespace RiverBooks.OrderProcessing;

internal class ReadThroughOrderAddressCache : IOrderAddressCache
{
    private readonly IMediator _mediator;
    private readonly RedisOrderAddressCache _redisCache;
    private readonly ILogger<ReadThroughOrderAddressCache> _logger;

    public ReadThroughOrderAddressCache(IMediator mediator, RedisOrderAddressCache redisCache, ILogger<ReadThroughOrderAddressCache> logger)
    {
        _mediator = mediator;
        _redisCache = redisCache;
        _logger = logger;
    }

    public async Task<Result<OrderAddress>> GetByIdAsync(Guid addressId)
    {
        var result = await _redisCache.GetByIdAsync(addressId);
        if(result.IsSuccess) return result;

        //TODO: handle where result is not success but the error is something other then not found
        if (result.Status != ResultStatus.NotFound) return Result.NotFound();
        _logger.LogInformation("Address {id} not found in cache fetching from source", addressId);
        var query = new Users.Contracts.UserAddressDetailsByIdQuery(addressId);
        var queryResult = await _mediator.Send(query);
        if(queryResult.IsSuccess)
        {
            //return and cache the result
            var dto = queryResult.Value;
            var address = new Address(
                dto.Street1,
                dto.Street2,
                dto.City,
                dto.State,
                dto.PostalCode,
                dto.Country
            );
            var orderAddress = new OrderAddress(dto.AddressId, address);
            await StoreAsync(orderAddress);
            return orderAddress;
        }
        return Result.NotFound();
    }

    public Task<Result> StoreAsync(OrderAddress orderAddress)
    {
        return _redisCache.StoreAsync(orderAddress);
    }
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
            _logger.LogWarning("Address {id} not found in {db}", addressId, "REDIS");
            return Result.NotFound();
        }

        OrderAddress? address = JsonSerializer.Deserialize<OrderAddress>(rawJson);
        if (address is null) return Result.NotFound();

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