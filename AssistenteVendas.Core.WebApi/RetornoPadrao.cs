using System.Text.Json.Serialization;

namespace AssistenteVencas.Core.WebApi
{
    public class RetornoPadrao<T>
    {
        public bool Success { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public T Data { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string> Errors { get; set; }

    }
}
