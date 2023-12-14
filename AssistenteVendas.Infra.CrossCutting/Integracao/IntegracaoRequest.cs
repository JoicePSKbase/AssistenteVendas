using AssistenteVendas.Core.Enums;
using AssistenteVendas.Core.Interfaces;
using AssistenteVendas.Core.Utils;
using System.Net.Http.Headers;
using System.Text;

namespace AssistenteVendas.Infra.CrossCutting.Integracao
{
    public class IntegracaoRequest : IIntegracaoRequest
    {
        private readonly HttpClient _httpClient;
        public IntegracaoRequest(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.Timeout = Timeout.InfiniteTimeSpan;
        }

        public async Task<HttpResponseMessage> WebRequestAsync(EnumCore.MethodType method, string url, string token, object entity = null)
        {
            _httpClient.DefaultRequestHeaders.Clear();

            if (!token[..7].Equals("Bearer "))
            {
                token = $"Bearer {token}";
            }

            _httpClient.DefaultRequestHeaders.Add("Authorization", token);

            StringContent content = new StringContent(string.Empty);
            if (entity != null)
            {
                if (entity is string)
                {
                    content = new StringContent(entity.ToString() ?? string.Empty, Encoding.UTF8, "text/plain");
                }
                else
                {
                    content = new StringContent(Serializador.Serializar(entity));
                    content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                }
            }

            HttpResponseMessage response;

            switch (method)
            {
                case EnumCore.MethodType.Get:
                    response = await _httpClient.GetAsync(url);
                    break;
                case EnumCore.MethodType.Post:
                    response = await _httpClient.PostAsync(url, content);
                    break;
                case EnumCore.MethodType.Put:
                    response = await _httpClient.PutAsync(url, content);
                    break;
                case EnumCore.MethodType.Delete:
                    response = await _httpClient.DeleteAsync(url);
                    break;
                default:
                    response = await _httpClient.GetAsync(url);
                    break;
            }

            return response;
        }
    }
}
