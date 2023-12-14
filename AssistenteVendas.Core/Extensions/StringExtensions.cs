using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

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

        private static string SubstituirCaracteresEspecias(string texto, string origem, string destino)
        {
            Dictionary<string, string> dicionario = CaracteresEspeciais(origem, destino);

            foreach (var dic in dicionario)
            {
                texto = texto.Replace(dic.Key, dic.Value);
            }

            texto = RemoverEnter(texto);
            texto = RemoverTab(texto);

            return texto;
        }

        private static Dictionary<string, string> CaracteresEspeciais(string origem, string destino)
        {
            Dictionary<string, string> dicionario = new Dictionary<string, string>();
            if (origem.Length != destino.Length)
            {
                throw new Exception("O número de caracteres especiais entre origem e destino não são iguais.");
            }
            for (int i = 0; i < origem.Length; i++)
            {
                var itemOld = origem[i].ToString();
                var itemNew = destino[i].ToString().Trim();
                dicionario.Add(itemOld, itemNew);
            }
           
            return dicionario;
        }

        public static string RemoverEnter(this string texto)
        {
            if (string.IsNullOrEmpty(texto))
            {
                return texto;
            }

            texto = Regex.Replace(texto, @"^\s+$[\r\n]*", "", RegexOptions.Multiline);
            texto = texto.TrimEnd('\r', '\n');
            texto = texto.Replace("\r", "");
            texto = texto.Replace("\n", "");
            texto = texto.Replace(Environment.NewLine, "");
            return texto;
        }

        public static string RemoverTab(this string texto)
        {
            if (string.IsNullOrEmpty(texto))
            {
                return texto;
            }

            char tab = '\u0009';
            texto = Regex.Replace(texto, @"^\s+$[\t]*", "", RegexOptions.Multiline);
            texto = texto.TrimEnd('\t');
            texto = texto.TrimEnd(tab);
            texto = texto.Replace("\t", "");
            texto = texto.Replace(tab.ToString(), "");
            texto = texto.Replace(Environment.NewLine, "");
            return texto;
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
        public static string UrlDecode(this string str)
        {
            return HttpUtility.UrlDecode(str);
        }

        public static string UrlEncode(this string str)
        {
            return HttpUtility.UrlEncode(str);
        }
        public static string FiltroTranslate(this string filtro, bool retornaOriginal = true)
        {
            var valores = filtro.UrlDecode().Split("||", StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < valores.Length; i++)
            {
                valores[i] = Traduz(valores[i]);
            }

            string Traduz(string subFiltro)
            {
                var valor = subFiltro.Trim();
                try
                {
                    valor = valor.Remove(0, 1);
                    valor = valor.Remove(valor.Length - 1, 1);

                    return valor.Base64Decode();
                }
                catch
                {
                    return retornaOriginal ? subFiltro : string.Empty;
                }
            }

            return string.Join(" ", valores);
        }

        private static string Vigenere(bool embaralhar, string chave, string texto)
        {
            const string alfabeto = "0123456789ABCDEFGHIJKLMNOPQRSTUVXWYZabcdefghijklmnopqrstuvxwyz<>!@#$?=/\\-_";

            int j = 0;
            StringBuilder ret = new StringBuilder(texto.Length);
            foreach (var t in texto)
            {
                if (alfabeto.Contains(t))
                {
                    var indexChave = alfabeto.IndexOf(chave[j]);
                    if (indexChave < 0)
                    {
                        indexChave = 1;
                    }

                    if (embaralhar)
                    {

                        var index = (alfabeto.IndexOf(t) + indexChave) % alfabeto.Length;
                        ret.Append(alfabeto[index]);
                    }
                    else
                    {
                        var index = (alfabeto.IndexOf(t) - indexChave + alfabeto.Length) % alfabeto.Length;
                        ret.Append(alfabeto[index]);
                    }
                }
                else
                {
                    ret.Append(t);
                }
                j = (j + 1) % chave.Length;
            }
            return ret.ToString();
        }

    }
}
