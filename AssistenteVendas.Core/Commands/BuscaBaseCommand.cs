namespace AssistenteVendas.Core.Commands
{
    public class BuscaBaseCommand
    {
        public string Filtro { get; set; }
        public bool Ativo { get; set; }
        public string Ordenacao { get; set; }
        public int Pagina { get; set; }
        public int QtdeRegistros { get; set; }
    }
}
