using System.IO;
using System.Text;

namespace GazeteKapiMVC5Core.SiteMap.Serialization
{
    sealed class StringWriterWithEncoding : StringWriter
    {
        public StringWriterWithEncoding(Encoding encoding)
        {
            Encoding = encoding;
        }

        public override Encoding Encoding { get; }
    }
}