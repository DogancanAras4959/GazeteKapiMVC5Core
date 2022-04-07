using System.IO;

namespace GazeteKapiMVC5Core.SiteMap.Serialization
{
    interface IXmlSerializer
    {
        string Serialize<T>(T data);
        void SerializeToStream<T>(T data, Stream stream);
    }
}