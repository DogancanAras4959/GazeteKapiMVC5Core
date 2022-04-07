using System.Xml;
using GazeteKapiMVC5Core.SiteMap.StyleSheets;

namespace GazeteKapiMVC5Core.SiteMap.Serialization
{
    interface IXmlProcessingInstructionHandler
    {
        void AddStyleSheets(XmlWriter xmlWriter, IHasStyleSheets model);
    }
}