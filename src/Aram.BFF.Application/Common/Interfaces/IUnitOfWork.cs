namespace Aram.BFF.Application.Common.Interfaces;

public interface IUnitOfWork
{
    Task CommitChangesAsync();
}