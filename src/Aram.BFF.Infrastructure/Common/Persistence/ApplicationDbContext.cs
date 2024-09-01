using System.Reflection;
using Aram.BFF.Application.Common.Interfaces;
using Aram.BFF.Domain.Common;
using Aram.BFF.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Aram.BFF.Infrastructure.Common.Persistence;

public class ApplicationDbContext(
    DbContextOptions options,
    IHttpContextAccessor httpContextAccessor,
    IPublisher publisher) : DbContext(options), IUnitOfWork
{
    public DbSet<Sample> Samples { get; set; } = null!;

    public async Task CommitChangesAsync()
    {
        // get hold of all the domain events
        List<IDomainEvent> domainEvents = ChangeTracker.Entries<Entity>()
            .Select(entry => entry.Entity.PopDomainEvents())
            .SelectMany(x => x)
            .ToList();

        // store them in the http context for later if user is waiting online
        if (IsUserWaitingOnline())
        {
            AddDomainEventsToOfflineProcessingQueue(domainEvents);
        }
        else
        {
            await PublishDomainEvents(publisher, domainEvents);
        }

        UpdateTimestamps();
        await SaveChangesAsync();
    }

    private static async Task PublishDomainEvents(IPublisher publisher, List<IDomainEvent> domainEvents)
    {
        foreach (IDomainEvent domainEvent in domainEvents)
        {
            await publisher.Publish(domainEvent);
        }
    }

    private bool IsUserWaitingOnline()
    {
        return httpContextAccessor.HttpContext is not null;
    }

    private void AddDomainEventsToOfflineProcessingQueue(List<IDomainEvent> domainEvents)
    {
        // fetch queue from http context or create a new queue if it doesn't exist
        Queue<IDomainEvent> domainEventsQueue = httpContextAccessor.HttpContext!.Items
            .TryGetValue("DomainEventsQueue", out object? value) && value is Queue<IDomainEvent> existingDomainEvents
            ? existingDomainEvents
            : new Queue<IDomainEvent>();

        // add the domain events to the end of the queue
        domainEvents.ForEach(domainEventsQueue.Enqueue);

        // store the queue in the http context
        httpContextAccessor.HttpContext!.Items["DomainEventsQueue"] = domainEventsQueue;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    private void UpdateTimestamps()
    {
        IEnumerable<EntityEntry<Entity>> entries = ChangeTracker
            .Entries<Entity>()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        foreach (EntityEntry<Entity> entityEntry in entries)
        {
            if (entityEntry.State == EntityState.Added)
            {
                entityEntry.Entity.CreationDate = DateTime.UtcNow;
            }
            else if (entityEntry.State == EntityState.Modified)
            {
                ((Entity)entityEntry.Entity).ModifiedDate = DateTime.UtcNow;
            }
        }
    }
}