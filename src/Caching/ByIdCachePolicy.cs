using Microsoft.AspNetCore.OutputCaching;

namespace aspnetcore_outputcaching_with_redis;

public class ByIdCachePolicy : IOutputCachePolicy
{
    public ValueTask CacheRequestAsync(OutputCacheContext context, CancellationToken cancellation)
    {
        var idRouteValue = context.HttpContext.Request.RouteValues["id"];

        if (idRouteValue is null) return ValueTask.CompletedTask;

        context.Tags.Add(idRouteValue.ToString()!);

        context.EnableOutputCaching = true;
        context.AllowCacheLookup = true;
        context.AllowCacheStorage = true;
        context.AllowLocking = true;
        context.CacheVaryByRules.QueryKeys = "*";

        return ValueTask.CompletedTask;
    }

    public ValueTask ServeFromCacheAsync(OutputCacheContext context, CancellationToken cancellation)
    {
        return ValueTask.CompletedTask;
    }

    public ValueTask ServeResponseAsync(OutputCacheContext context, CancellationToken cancellation)
    {
        return ValueTask.CompletedTask;
    }
}