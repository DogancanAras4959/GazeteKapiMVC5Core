using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GazeteKapiMVC5Core.WEB.CoreInjection
{
    public class extools
    {
        public extools()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static string clearTextRouting(string metin)
        {
            string deger = metin.ToLower();
            deger = deger.ToLower();
            deger = deger.Trim();
            deger = deger.Replace("ş", "s");
            deger = deger.Replace("ğ", "g");
            deger = deger.Replace("  ", "-");
            deger = deger.Replace("ü", "u");
            deger = deger.Replace("ö", "o");
            deger = deger.Replace("ç", "c");
            deger = deger.Replace("ı", "i");
            deger = deger.Replace(",", "");
            deger = deger.Replace("“", "");
            deger = deger.Replace("”", "");
            deger = deger.Replace("‘", "");
            deger = deger.Replace("´", "-");
            deger = deger.Replace("'", "");
            deger = deger.Replace("`", "");
            deger = deger.Replace("’", "");
            deger = deger.Replace("?", "");
            deger = deger.Replace("!", "");
            deger = deger.Replace(".", "");
            deger = deger.Replace("<", "");
            deger = deger.Replace(">", "");
            deger = deger.Replace("^", "");
            deger = deger.Replace("\"", "");
            deger = deger.Replace("/", "");
            deger = deger.Replace("+", "-");
            deger = deger.Replace("$", "-");
            deger = deger.Replace("#", "");
            deger = deger.Replace("%", "");
            deger = deger.Replace("&", "");
            deger = deger.Replace("{", "");
            deger = deger.Replace("(", "");
            deger = deger.Replace("[", "");
            deger = deger.Replace(")", "");
            deger = deger.Replace("]", "");
            deger = deger.Replace("=", "");
            deger = deger.Replace("}", "");
            deger = deger.Replace("*", "");
            deger = deger.Replace(" ", "-");
            deger = deger.Replace(":", "-");
            deger = deger.Replace("%", "_");
            deger = deger.Replace("/", "-");
            deger = deger.Replace(";", "_");
            deger = deger.Replace("__", "_");
            deger = deger.Replace("--", "-");
            deger = deger.Replace("--_", "-");
            deger = deger.Replace("_", "-");
            deger = deger.Replace("--", "-");


            return deger;
        }

        public static string XmlEncode(string value)
        {
            value = value.Trim();
            value = value.Replace("<", "&lt;");
            value = value.Replace(">", "&gt;");
            value = value.Replace("\"", "&quot;");
            value = value.Replace("'", "&apos;");
            //value = value.Replace("&", "&amp;");
            return value;

        }

        public static string XmlDecode(string value)
        {
            //value = value.Trim();
            value = value.Replace("&lt;", "<");
            value = value.Replace("&gt;", ">");
            value = value.Replace("&quot;", "\"");
            value = value.Replace("&apos;", "'");
            //value = value.Replace("&amp;", "&");
            return value;

        }


        public static string ClearSqlInjection(string deger)
        {
            deger = deger.Replace("&gt;", "");
            deger = deger.Replace("&lt;", "");
            deger = deger.Replace("--", "");
            deger = deger.Replace("'", "");
            deger = deger.Replace("char ", "");
            deger = deger.Replace("delete ", "");
            deger = deger.Replace("insert ", "");
            deger = deger.Replace("update ", "");
            deger = deger.Replace("select ", "");
            deger = deger.Replace("truncate ", "");
            deger = deger.Replace("union ", "");
            deger = deger.Replace("script ", "");
            deger = deger.Replace(",", "");
            deger = deger.Replace("‘", "");
            deger = deger.Replace("´", "-");
            deger = deger.Replace("'", "");
            deger = deger.Replace("`", "");
            deger = deger.Replace("’", "");
            deger = deger.Replace("?", "");
            deger = deger.Replace("!", "");
            deger = deger.Replace(".", "");
            deger = deger.Replace("<", "");
            deger = deger.Replace(">", "");
            deger = deger.Replace("^", "");
            deger = deger.Replace("\"", "");
            deger = deger.Replace("/", "");
            deger = deger.Replace("+", "-");
            deger = deger.Replace("$", "-");
            deger = deger.Replace("#", "");
            deger = deger.Replace("%", "");
            deger = deger.Replace("&", "");
            deger = deger.Replace("{", "");
            deger = deger.Replace("(", "");
            deger = deger.Replace("[", "");
            deger = deger.Replace(")", "");
            deger = deger.Replace("]", "");
            deger = deger.Replace("=", "");
            deger = deger.Replace("}", "");
            deger = deger.Replace("*", "");
            deger = deger.Replace(" ", "-");
            deger = deger.Replace(":", "-");
            deger = deger.Replace("%", "_");
            deger = deger.Replace("/", "-");
            deger = deger.Replace(";", "_");
            deger = deger.Replace("__", "_");
            deger = deger.Replace("--", "-");
            deger = deger.Replace("--_", "-");
            deger = deger.Replace("_", "-");
            deger = deger.Replace("--", "-");

            return deger;
        }
    }
}
