using AssistenteVendas.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace AssistenteVendas.Core.Repository.Services
{
    public class TransactionService : ITransactionService
    {
        private DbContext _dbContext;
        private IDbContextTransaction _transacaoAtual;
        private Guid _id;

        public TransactionService()
        {
            _dbContext = null;
            _transacaoAtual = null;
        }

        public async Task BeginTransactionAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (_transacaoAtual == null)
            {
                _transacaoAtual = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted, cancellationToken);
                _id = id;
            }
        }

        public async Task CommitAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (_id == id)
            {
                await _transacaoAtual.CommitAsync(cancellationToken);
                _transacaoAtual = null;
            }
        }

        public async Task RollbackAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (_id == id)
            {
                await _transacaoAtual.RollbackAsync(cancellationToken);
                _transacaoAtual = null;
            }
        }

        public void SetDbContext(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IDbContextTransaction GetTransaction()
        {
            return _transacaoAtual;
        }
    }
}
