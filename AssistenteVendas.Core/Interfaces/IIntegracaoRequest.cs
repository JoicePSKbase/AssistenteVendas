using AssistenteVendas.Core.Enums;

namespace AssistenteVendas.Core.Interfaces
{
    public interface IIntegracaoRequest
    {
        Task<HttpResponseMessage> WebRequestAsync(EnumCore.MethodType method, string url, string token, object entity = null);
    }
}
