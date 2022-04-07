using System;
using GazeteKapiMVC5Core.SiteMap.Routing;

namespace GazeteKapiMVC5Core.WEB.Models.ConfigSiteMap
{
    public class BaseUrlProvider : IBaseUrlProvider
    {
        public Uri BaseUrl => new Uri("https://www.gazetekapi.com");
    }
}