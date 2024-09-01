namespace Aram.BFF.Domain.Common;

public abstract class Entity
{
    protected readonly List<IDomainEvent> _domainEvents = [];

    protected Entity(Guid id)
    {
        Id = id;
    }

    protected Entity()
    {
    }

    public Guid Id { get; init; }

    public bool IsDeleted { get; private set; }
    public DateTime CreationDate { get; set; }
    public DateTime? ModifiedDate { get; set; }

    public List<IDomainEvent> PopDomainEvents()
    {
        List<IDomainEvent> copy = _domainEvents.ToList();

        _domainEvents.Clear();

        return copy;
    }

    public void Delete()
    {
        IsDeleted = true;
    }
}