namespace AssistenteVendas.Core.Extensions
{
    public static class UtilExtension
    {
        public static string GetDecodedEnvironmentVariable(string chave)
        {
            chave = chave.Replace("<", "").Replace(">", "");
            var valor = Environment.GetEnvironmentVariable(chave) ?? chave;

            try
            {
                return valor.Base64Decode();
            }
            catch
            {
                return valor;
            }
        }
    }
}
