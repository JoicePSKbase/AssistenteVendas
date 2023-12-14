namespace AssistenteVendas.Domain.Usuario.Commands
{
    public class UsuarioResult
    {
        public Guid Id { get; set; }
        public bool Ativo { get; set; }
        public string Nome { get; init; }
        public string Email { get; init; }
        public bool Multiempresa { get; init; }
        public DateTime? DataUltimoAcesso { get; set; }
        public bool AcessoLiberado { get; set; }
    }
}
