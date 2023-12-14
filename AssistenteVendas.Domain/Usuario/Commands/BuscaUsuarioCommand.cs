using AssistenteVendas.Core.Commands;
using AssistenteVendas.Core.Entities;
using MediatR;

namespace AssistenteVendas.Domain.Usuario.Commands
{
    public class BuscaUsuarioCommand : BuscaBaseCommand, IRequest<ListaPaginada<UsuarioResult>>
    {
    }
}