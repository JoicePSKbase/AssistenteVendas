using AssistenteVendas.Core.Interfaces;
using AssistenteVendas.Domain.Usuario.Entities;
using AssistenteVendas.Domain.Usuario.Interfaces;
using AssistenteVendas.Infra.Data.Context;

namespace AssistenteVendas.Infra.Data.Repositories.Usuario
{
    public class UsuarioRepository : BaseRepository<UsuarioEntity>, IUsuarioRepository
    {
        public UsuarioRepository(BaseDbContext dbContext, ITransactionService transactionService) : base(dbContext, transactionService)
        {
        }
       
    }
}
