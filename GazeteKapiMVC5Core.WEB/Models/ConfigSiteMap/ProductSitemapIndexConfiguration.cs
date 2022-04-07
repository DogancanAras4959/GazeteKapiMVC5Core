using System.Linq;
using CORE.ApplicationCommon.DTOS.NewsDTO;
using GazeteKapiMVC5Core.DataAccessLayer.Models;
using GazeteKapiMVC5Core.Models.News.NewsModel;
using GazeteKapiMVC5Core.SiteMap;
using Microsoft.AspNetCore.Mvc;

namespace GazeteKapiMVC5Core.WEB.Models.ConfigSiteMap
{
    public class ProductSitemapIndexConfiguration : SitemapIndexConfiguration<NewsLıstItemModel>
    {
        private readonly IUrlHelper urlHelper;

        public ProductSitemapIndexConfiguration(IQueryable<NewsLıstItemModel> dataSource, int? currentPage, IUrlHelper urlHelper)
            : base(dataSource, currentPage)
        {
            this.urlHelper = urlHelper;
            Size = 45;
        }

        public override SitemapIndexNode CreateSitemapIndexNode(int currentPage)
        {
            return new SitemapIndexNode(urlHelper.Action("sayfa", "anasayfa", new { id = currentPage }));
        }

        public override SitemapNode CreateNode(NewsLıstItemModel source)
        {
            return new SitemapNode(urlHelper.Action("haber", "anasayfa", new { id = source.Id, title = source.GenerateSlug() }));
        }
    }
}