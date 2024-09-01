using Aram.BFF.Infrastructure.Common.Middleware;

using Microsoft.AspNetCore.Builder;

namespace Aram.BFF.Infrastructure;

public static class RequestPipeline
{
    public static IApplicationBuilder AddInfrastructureMiddleware(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<EventualConsistencyMiddleware>();

        return builder;
    }
}