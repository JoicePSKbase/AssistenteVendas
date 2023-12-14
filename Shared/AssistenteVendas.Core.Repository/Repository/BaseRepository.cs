using System.Collections.Generic;
using System.Linq.Expressions;

namespace AssistenteVendas.Core.Repository.Repository
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext DbContext;
        protected readonly DbSet<TEntity> DbContextSet;
        private readonly ITransactionService _transactionService;

        protected BaseRepository(
            DbContext dbContext,
            ITransactionService transactionService
        )
        {
            DbContext = dbContext;
            _transactionService = transactionService;
            _transactionService.SetDbContext(DbContext);
            DbContextSet = dbContext.Set<TEntity>();
        }

        public virtual async Task<TEntity> Obter(Expression<Func<TEntity, bool>> expression, params string[] includes)
        {
            return await Obter(null, expression, includes);
        }

        public async Task<TEntity> Obter(string ordem, Expression<Func<TEntity, bool>> expression, params string[] includes)
        {
            var query = DbContextSet.AsNoTracking();
            return await Obter(ordem, query, expression, includes);
        }

        public virtual async Task<TEntity> ObterTracking(Expression<Func<TEntity, bool>> expression, params string[] includes)
        {
            return await ObterTracking(null, expression, includes);
        }

        public async Task<TEntity> ObterTracking(string ordem, Expression<Func<TEntity, bool>> expression, params string[] includes)
        {
            var query = DbContextSet.AsQueryable();
            return await Obter(ordem, query, expression, includes);
        }

        protected async Task<TEntity> Obter(string ordem, IQueryable<TEntity> query, Expression<Func<TEntity, bool>> expression, params string[] includes)
        {
            if (includes != null && includes.Any())
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            if (!string.IsNullOrEmpty(ordem))
            {
                query = query.OrderBy(ordem);
            }

            query = query.AsSplitQuery();

            if (expression != null)
            {
                return await query.FirstOrDefaultAsync(expression);
            }

            return await query.FirstOrDefaultAsync();
        }

        public virtual async Task<TEntity> ObterPorId(Guid id)
        {
            return await DbContextSet.FindAsync(id);
        }

        public virtual async Task<TEntity> ObterPorIdAsNoTracking(Guid id, string idFieldName)
        {
            var query = DbContextSet.AsNoTracking().Where($"{idFieldName} == \"{id}\"");
            return await ObterPorIdAsNoTracking(query);
        }

        protected virtual async Task<TEntity> ObterPorIdAsNoTracking(IQueryable<TEntity> query)
        {
            return await query.FirstOrDefaultAsync();
        }

        protected async Task<List<TEntity>> BuscarLista(string ordem, IQueryable<TEntity> query, Expression<Func<TEntity, bool>> expressao, params string[] includes)
        {
            if (includes != null && includes.Any())
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            if (!string.IsNullOrEmpty(ordem))
            {
                query = query.OrderBy(ordem);
            }

            return await query.Where(expressao).AsSplitQuery().ToListAsync();
        }

        public virtual async Task<List<TEntity>> BuscarLista(Expression<Func<TEntity, bool>> expressao, params string[] includes)
        {
            return await BuscarLista(null, expressao, includes);
        }

        public virtual async Task<List<TEntity>> BuscarLista(string ordem, Expression<Func<TEntity, bool>> expressao, params string[] includes)
        {
            var query = DbContextSet.AsNoTracking();

            return await BuscarLista(null, query, expressao, includes);
        }

        public virtual async Task<List<TEntity>> BuscarListaTracking(Expression<Func<TEntity, bool>> expressao, params string[] includes)
        {
            return await BuscarLista(null, expressao, includes);
        }

        public virtual async Task<List<TEntity>> BuscarListaTracking(string ordem, Expression<Func<TEntity, bool>> expressao, params string[] includes)
        {
            var query = DbContextSet.AsQueryable();

            return await BuscarLista(ordem, query, expressao, includes);
        }

        public virtual async Task<ListaPaginada<TEntity>> Buscar(string filtro, string ordenacao = "id asc",
            int pagina = 1, int qtdeRegistros = 10, params string[] includes)
        {
            var query = string.IsNullOrEmpty(filtro) ? DbContextSet : DbContextSet.Where(filtro.FiltroTranslate());
            return await Buscar(query, ordenacao, pagina, qtdeRegistros, includes);
        }

        public virtual async Task<ListaPaginada<TEntity>> Buscar(Expression<Func<TEntity, bool>> expressao,
            string ordenacao = "id asc", int pagina = 1, int qtdeRegistros = 10, params string[] includes)
        {
            var query = DbContextSet.AsNoTracking().Where(expressao).OrderBy(ordenacao);
            return await Buscar(query, ordenacao, pagina, qtdeRegistros, includes);
        }

        protected async Task<ListaPaginada<TEntity>> Buscar(IQueryable<TEntity> query, string ordenacao = "id asc",
            int pagina = 1, int qtdeRegistros = 10, params string[] includes)
        {
            if (includes != null && includes.Any())
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }

                query = query.AsSplitQuery();
            }

            var result = new ListaPaginada<TEntity>();
            result.Itens.AddRange(await query.AsNoTracking().OrderBy(ordenacao).Skip((pagina - 1) * qtdeRegistros).Take(qtdeRegistros).ToListAsync());
            result.Total = await query.CountAsync();
            result.Pagina = pagina;
            return result;
        }

        public virtual void Adicionar(TEntity entity, Action<TEntity> preLog = null)
        {
            var trackerEntrie = GetTraked(entity);

            if (trackerEntrie == null)
            {
                DbContextSet.Add(entity);
            }
            else
            {
                trackerEntrie.State = EntityState.Added;
                trackerEntrie.CurrentValues.SetValues(entity);
            }
        }

        public virtual void Atualizar(TEntity entity, Action<TEntity, TEntity> preLog = null)
        {
            var trackerEntrie = GetTraked(entity);

            if (trackerEntrie == null)
            {
                DbContextSet.Update(entity);
            }
            else
            {
                trackerEntrie.State = EntityState.Modified;
                trackerEntrie.CurrentValues.SetValues(entity);
            }
        }

        private EntityEntry GetTraked(TEntity entity)
        {

            if (entity is BaseEntity)
            {
                var trackerEntrie = DbContext.ChangeTracker.Entries().FirstOrDefault(r =>
                    r.Entity is TEntity && (r.Entity as BaseEntity)?.Id == (entity as BaseEntity)?.Id);

                return trackerEntrie;
            }

            //TODO Verificar as anotation para ver se há campo de PK diferente do ID
            return null;
        }

        public virtual void Remover(TEntity entity, Action<TEntity> preLog = null)
        {
            var trackerEntrie = GetTraked(entity);

            if (trackerEntrie == null)
            {
                DbContextSet.Remove(entity);
            }
            else
            {
                trackerEntrie.State = EntityState.Deleted;
            }
        }

        public virtual void Remover(List<TEntity> listaEntity, Action<TEntity> preLog = null)
        {
            foreach (var entity in listaEntity)
            {
                Remover(entity, preLog);
            }
        }

        public virtual async Task<int> Count() => await DbContextSet.CountAsync();

        public virtual async Task<int> Count(Expression<Func<TEntity, bool>> expressao) => await this.DbContextSet.Where(expressao).CountAsync();

        public virtual async Task<int> Count(string filtro) => string.IsNullOrEmpty(filtro)
            ? await Count() : await DbContextSet.Where(filtro.FiltroTranslate()).CountAsync();

        public async Task<int> SaveChanges(CancellationToken cancellationToken = default) => await DbContext.SaveChangesAsync(cancellationToken);

        public async Task BeginTransactionAsync(Guid chaveTransacao, CancellationToken cancellationToken = default)
        {
            await _transactionService.BeginTransactionAsync(chaveTransacao, cancellationToken);
        }

        public IDbContextTransaction GetTransactionAsync()
        {
            return _transactionService.GetTransaction();
        }

        public async Task CommitAsync(Guid chaveTransacao, CancellationToken cancellationToken = default)
        {
            await _transactionService.CommitAsync(chaveTransacao, cancellationToken);
        }

        public async Task RollbackAsync(Guid chaveTransacao, CancellationToken cancellationToken = default)
        {
            await _transactionService.RollbackAsync(chaveTransacao, cancellationToken);
        }

        public bool IsTracking(TEntity entity)
        {
            return GetTraked(entity) != null;
        }

        public void Track(TEntity entity)
        {
            DbContextSet.Attach(entity);
        }

        public virtual Task CarregarReferencias(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public virtual Task CarregarDetalhes(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Dispose() => DbContext?.Dispose();

    }
}
