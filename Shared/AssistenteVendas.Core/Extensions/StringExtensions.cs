using System.Globalization;
using System.Text;

namespace AssistenteVendas.Core.Extensions
{
    public static class StringExtensions
    {
        public static DateTime GetData(this string sData)
        {
            try
            {
                return DateTime.ParseExact(sData.Trim()[..10], "yyyy-MM-dd", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                try
                {
                    return DateTime.ParseExact(sData.Trim()[..10], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                    return DateTime.ParseExact(sData.Trim()[..10], "MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
            }
        }

        public static string Base64Decode(this string base64Encoded)
        {
            if (string.IsNullOrEmpty(base64Encoded))
                return string.Empty;

            byte[] data = Convert.FromBase64String(base64Encoded);

            return Encoding.ASCII.GetString(data);
        }

        public static string Base64Encode(this string plainText)
        {
            var plainTextBytes = Encoding.ASCII.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static bool ContemCaracteresEspeciais(this string nome) =>
            "¹²³°ºªØµ¿Ð¤¢®©øÞ×¨`~±|»«þ¸¦ðæ÷§¯¬¶¥Æ¡£<>@{}!#$%&*()[]+=.,;?/\"\\".Any(nome.Contains);

        public static string RemoverCaracteresEspeciais(this string texto)
        {
            const string origem = "ÃÀÁÂÄÅßÉÊÈËÌÍÎÏÕÒÓÔÖÙÚÛÜÝÇÑãàáâäåéêèëìíîïõòóôöùúûüýÿçñ¹²³°ºªØµ¿Ð¤¢®©øÞ×¨`~±|»«þ¸¦ðæ÷§¯¬¶¥Æ¡£<>@{}!#$%&*()[]+=.,;?/\"\\";
            const string destino = "AAAAAABEEEEIIIIOOOOOUUUUYCNaaaaaaeeeeiiiiooooouuuuyycn123ooa0uCDocRC0px                                             ";

            texto = SubstituirCaracteresEspecias(texto, origem, destino);

            return texto.Trim();
        }

        public static string RemoverEspaco(this string texto)
        {
            return texto.Replace(" ", "");
        }


        public static string Embaralha(this string texto, string chave)
        {
            return Vigenere(true, chave, texto);
        }

        public static string Desembaralha(this string texto, string chave)
        {
            return Vigenere(false, chave, texto);
        }

    }
}
