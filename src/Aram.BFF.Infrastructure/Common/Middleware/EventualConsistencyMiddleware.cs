using Aram.BFF.Domain.Common;
using Aram.BFF.Infrastructure.Common.Persistence;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;

namespace Aram.BFF.Infrastructure.Common.Middleware;

public class EventualConsistencyMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IPublisher publisher, ApplicationDbContext dbContext)
    {
        IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync();
        context.Response.OnCompleted(async () =>
        {
            try
            {
                await PublishDomainEvent(context, publisher);
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                // notify the client that even though they got a good response, the changes didn't take place
                // due to an unexpected error
            }
            finally
            {
                await transaction.DisposeAsync();
            }
        });

        await next(context);
    }

    private static async Task PublishDomainEvent(HttpContext context, IPublisher publisher)
    {
        if (context.Items.TryGetValue("DomainEventsQueue", out object? value) &&
            value is Queue<IDomainEvent> domainEventsQueue)
        {
            while (domainEventsQueue.TryDequeue(out IDomainEvent? domainEvent))
            {
                await publisher.Publish(domainEvent);
            }
        }
    }
}