using MediatR;

namespace AssistenteVendas.Domain.Usuario.Commands
{
    public class AdicionarUsuarioCommand : IRequest<UsuarioResult>
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
    }
}
