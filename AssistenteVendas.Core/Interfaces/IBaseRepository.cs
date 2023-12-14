using AssistenteVendas.Core.Entities;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage;

namespace AssistenteVendas.Core.Interfaces
{
    public interface IBaseRepository<TEntity> : IDisposable where TEntity : class
    {
        void Adicionar(TEntity entity, Action<TEntity> preLog = null);
        Task<TEntity> Obter(Expression<Func<TEntity, bool>> expression, params string[] includes);
        Task<TEntity> Obter(string ordem, Expression<Func<TEntity, bool>> expression, params string[] includes);
        Task<TEntity> ObterTracking(Expression<Func<TEntity, bool>> expression, params string[] includes);
        Task<TEntity> ObterTracking(string ordem, Expression<Func<TEntity, bool>> expression, params string[] includes);
        Task<TEntity> ObterPorId(Guid id);
        Task<TEntity> ObterPorIdAsNoTracking(Guid id, string idFieldName = "Id");
        Task<List<TEntity>> BuscarLista(Expression<Func<TEntity, bool>> expressao, params string[] includes);
        Task<List<TEntity>> BuscarLista(string ordem, Expression<Func<TEntity, bool>> expressao, params string[] includes);
        Task<List<TEntity>> BuscarListaTracking(Expression<Func<TEntity, bool>> expressao, params string[] includes);
        Task<List<TEntity>> BuscarListaTracking(string ordem, Expression<Func<TEntity, bool>> expressao, params string[] includes);
        Task<ListaPaginada<TEntity>> Buscar(Expression<Func<TEntity, bool>> expressao, string ordenacao = "id asc", int pagina = 1, int qtdeRegistros = 10, params string[] includes);
        Task<ListaPaginada<TEntity>> Buscar(string filtro, string ordenacao = "id asc", int pagina = 1, int qtdeRegistros = 10, params string[] includes);
        void Atualizar(TEntity entity, Action<TEntity, TEntity> preLog = null);
        void Remover(TEntity entity, Action<TEntity> preLog = null);
        void Remover(List<TEntity> listaEntity, Action<TEntity> preLog = null);
        Task<int> Count();
        Task<int> Count(Expression<Func<TEntity, bool>> expressao);
        Task<int> Count(string filtro);
        Task<int> SaveChanges(CancellationToken cancellationToken = default);
        IDbContextTransaction GetTransactionAsync();
        Task BeginTransactionAsync(Guid chaveTransacao, CancellationToken cancellationToken = default);
        Task CommitAsync(Guid chaveTransacao, CancellationToken cancellationToken = default);
        Task RollbackAsync(Guid chaveTransacao, CancellationToken cancellationToken = default);
        bool IsTracking(TEntity entity);
        void Track(TEntity entity);
    }
}
