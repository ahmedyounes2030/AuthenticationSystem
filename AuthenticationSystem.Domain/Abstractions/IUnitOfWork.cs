using System.Data;

namespace AuthenticationSystem.Domain.Abstractions;

public interface IUnitOfWork
{
    IDbTransaction BeginTransaction();
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
