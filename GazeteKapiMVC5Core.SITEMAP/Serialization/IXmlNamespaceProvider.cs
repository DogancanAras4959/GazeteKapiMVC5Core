using System.Collections.Generic;

namespace GazeteKapiMVC5Core.SiteMap.Serialization
{
    interface IXmlNamespaceProvider
    {
        /// <summary>
        /// Gets the XML namespaces.
        /// Exposed for XML serializer.
        /// </summary>
        IEnumerable<string> GetNamespaces();
    }
}