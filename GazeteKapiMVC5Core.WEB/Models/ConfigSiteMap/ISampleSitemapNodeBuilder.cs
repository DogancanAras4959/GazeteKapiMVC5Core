using GazeteKapiMVC5Core.SiteMap;
using System.Collections.Generic;

namespace GazeteKapiMVC5Core.WEB.Models.ConfigSiteMap
{
    public interface ISampleSitemapNodeBuilder
    {
        IEnumerable<SitemapIndexNode> BuildSitemapIndex();
        SitemapModel BuildSitemapModel();
    }
}