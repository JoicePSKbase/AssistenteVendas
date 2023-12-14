using System.ComponentModel;

namespace AssistenteVendas.Core.Enums
{
    public static class EnumCore
    {
        public enum EnumPermissaoAmbiente
        {
            [Description("Ambos")] Ambos = 1,
            [Description("Teste")] Teste = 2,
            [Description("Produção")] Producao = 3
        }

        public enum EnumAmbiente
        {
            [Description("DevLocal")] DevLocal = 1,
            [Description("Desenvolvimento")] Desenvolvimento = 2,
            [Description("Homologação")] Homologacao = 3,
            [Description("Produção")] Producao = 4,
            [Description("Teste")] Teste = 5
        }

        public enum MethodType
        {
            [Description("Get")] Get,
            [Description("Post")] Post,
            [Description("Put")] Put,
            [Description("Delete")] Delete
        }
    }
}