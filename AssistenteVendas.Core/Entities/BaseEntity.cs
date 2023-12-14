namespace AssistenteVendas.Core.Entities
{
    public record BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}
