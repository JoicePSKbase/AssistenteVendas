using System.Text.Json;
using System.Text.Json.Serialization;

namespace AssistenteVendas.Core.Utils
{
    public static class Serializador
    {
        public static string Serializar<T>(T param)
        {
            return JsonSerializer.Serialize(param, GetSerializeOptions());
        }

        public static T Deserializar<T>(string param)
        {
            return param is null ? default(T) : JsonSerializer.Deserialize<T>(param, GetDeserializeOptions());
        }
        public static T Deserializar<T>(byte[] param)
        {
            return JsonSerializer.Deserialize<T>(param, GetDeserializeOptions());
        }

        private static JsonSerializerOptions GetSerializeOptions()
        {
            var options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            };
            options.Converters.Add(new JsonStringEnumConverter());
            return options;
        }

        private static JsonSerializerOptions GetDeserializeOptions()
        {
            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            options.Converters.Add(new JsonStringEnumConverter());
            return options;
        }
    }
}
