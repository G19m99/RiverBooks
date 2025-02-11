using FastEndpoints;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace RiverBooks.OrderProcessing.Endpoints;

internal class FlushCache : EndpointWithoutRequest
{
    private readonly IDatabase _db;
    private readonly ILogger<FlushCache> _logger;

    public FlushCache(ILogger<FlushCache> logger)
    {
        var redis = ConnectionMultiplexer.Connect("localhost");
        _db = redis.GetDatabase();
        _logger = logger;
    }

    public override void Configure()
    {
        Post("/flushredis");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await _db.ExecuteAsync("FLUSHDB");
        _logger.LogWarning("FLUSED CACHE FOR DB {db}", "REDIS");
    }
}
