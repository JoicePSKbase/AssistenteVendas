namespace AssistenteVendas.Core.Entities
{
    public class ListaPaginada<T>
    {
        public int Total { get; set; }
        public int Pagina { get; set; }
        public List<T> Itens { get; set; } = new();

        public T First => Itens.FirstOrDefault();
    }
}
