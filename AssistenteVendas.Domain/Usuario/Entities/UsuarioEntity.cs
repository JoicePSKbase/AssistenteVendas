using AssistenteVendas.Core.Entities;

namespace AssistenteVendas.Domain.Usuario.Entities
{
    public record UsuarioEntity : BaseEntity
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        //  public string Password { get; set; }
    }
}
