namespace AuthenticationSystem.Infrastructure;

internal class UnitOfWork(ApplicationDbContext _dbContext) : IUnitOfWork
{
    public IDbTransaction BeginTransaction()
    {
        var transaction = _dbContext.Database.BeginTransaction();

        return transaction.GetDbTransaction();
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}