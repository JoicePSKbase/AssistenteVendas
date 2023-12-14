using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace AssistenteVendas.Core.Interfaces
{
    public interface ITransactionService
    {
        Task BeginTransactionAsync(Guid id, CancellationToken cancellationToken = default);
        Task CommitAsync(Guid id, CancellationToken cancellationToken = default);
        Task RollbackAsync(Guid id, CancellationToken cancellationToken = default);
        void SetDbContext(DbContext dbContext);
        IDbContextTransaction GetTransaction();
    }
}
