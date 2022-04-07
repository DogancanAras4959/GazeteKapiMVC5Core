using System.Collections.Generic;
using System.Xml.Serialization;

namespace GazeteKapiMVC5Core.SiteMap.Serialization
{
    interface IXmlNamespaceBuilder
    {
        XmlSerializerNamespaces Create(IEnumerable<string> namespaces);
    }
}