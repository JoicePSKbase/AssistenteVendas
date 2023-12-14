using AssistenteVendas.Core.Commands;
using MediatR;

namespace AssistenteVendas.Domain.Usuario.Commands
{
    public class BuscaUsuarioPorEmailCommand : BuscaBaseCommand, IRequest<UsuarioResult>
    {
        public string Email { get; set; }
    }
}