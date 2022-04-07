using System;

namespace GazeteKapiMVC5Core.SiteMap.Routing
{
    internal interface IReflectionHelper
    {
        UrlPropertyModel GetPropertyModel(Type type);
    }
}