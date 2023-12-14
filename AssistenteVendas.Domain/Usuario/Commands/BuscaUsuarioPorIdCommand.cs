using AssistenteVendas.Core.Commands;
using MediatR;

namespace AssistenteVendas.Domain.Usuario.Commands
{
    public class BuscaUsuarioPorIdCommand : BuscaBaseCommand, IRequest<UsuarioResult>
    {
        public Guid Id { get; set; }
    }
}