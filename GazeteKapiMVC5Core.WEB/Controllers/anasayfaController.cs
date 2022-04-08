using AutoMapper;
using CORE.ApplicationCommon.DTOS.CategoryDTO;
using CORE.ApplicationCommon.DTOS.NewsDTO;
using CORE.ApplicationCommon.DTOS.NewsDTO.GuestDTO;
using CORE.ApplicationCommon.DTOS.NewsDTO.TagDTO;
using CORE.ApplicationCommon.DTOS.NewsDTO.TagNewsDTO;
using GazeteKapiMVC5Core.Core.Extensions;
using GazeteKapiMVC5Core.Models.Category;
using GazeteKapiMVC5Core.Models.News.GuestModel;
using GazeteKapiMVC5Core.Models.News.NewsModel;
using GazeteKapiMVC5Core.Models.News.TagModel;
using GazeteKapiMVC5Core.Models.News.TagNewsModel;
using GazeteKapiMVC5Core.SiteMap;
using GazeteKapiMVC5Core.WEB.Models.ConfigSiteMap;
using GazeteKapiMVC5Core.WEB.Models.ConfigTTS;
using GazeteKapiMVC5Core.WEB.Models.ConfigUrl;
using Microsoft.AspNetCore.Mvc;
using SERVICE.Engine.Interfaces;
using SERVICES.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Syndication;
using System.Speech.AudioFormat;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace GazeteKapiMVC5Core.WEB.Controllers
{
    public class anasayfaController : Controller
    {

        private readonly IMapper _mapper;
        private readonly INewsService _newService;
        private readonly ICategoryService _categoryService;
        private readonly ISitemapProvider _siteMapProvider;
        private SpeechSynthesizer ss;
        private string plainTextGlobal = "";

        public anasayfaController(INewsService newService, ICategoryService categoryService, IMapper mapper, ISitemapProvider siteMapProvider)
        {
            _newService = newService;
            _categoryService = categoryService;
            _mapper = mapper;
            _siteMapProvider = siteMapProvider;
            ss = new SpeechSynthesizer();
        }

        public IActionResult sayfa()
        {
            var haberlist = _mapper.Map<List<NewsListItemDto>, List<NewsLıstItemModel>>(_newService.newsList());

            List<GuestListViewModel> guestList = null;
            guestList = _mapper.Map<List<GuestListItemDto>, List<GuestListViewModel>>(_newService.guestList());

            List<TagNewsListViewModel> tagNewList = null;
            tagNewList = _mapper.Map<List<TagNewsListItemDto>, List<TagNewsListViewModel>>(_newService.tagsListWithNewsWeb());

            ViewBag.TagNews = tagNewList;
            ViewBag.HaberlerManset = haberlist;
            ViewBag.GuestList = guestList;

            return View();

        }
        public IActionResult aramasonucu(int? pageNumber, string searchnews, int? TagId)
        {
            int pageSize = 20;
            List<NewsLıstItemModel> haberlist = null;
            List<TagNewsListViewModel> tagNewList = null;
            List<CategoryListViewModel> categoryList = null;
            List<NewsLıstItemModel> modelNew = new List<NewsLıstItemModel>();

            if (searchnews != null && searchnews != "")
            {
                haberlist = _mapper.Map<List<NewsListItemDto>, List<NewsLıstItemModel>>(_newService.searchDataInNews(searchnews));

                tagNewList = _mapper.Map<List<TagNewsListItemDto>, List<TagNewsListViewModel>>(_newService.tagsListWithNewsWebSearch(searchnews));

                foreach (var item in haberlist)
                {
                    NewsLıstItemModel model = new NewsLıstItemModel();
                    model.Id = item.Id;
                    model.Image = item.Image;
                    model.Title = item.Title;
                    model.Spot = item.Spot;
                    model.PublishedTime = item.PublishedTime;
                    model.CategoryId = item.CategoryId;
                    modelNew.Add(model);
                }

                foreach (var item in tagNewList)
                {

                    NewsLıstItemModel model = new NewsLıstItemModel();
                    model.Id = item.news.Id;
                    model.Image = item.news.Image;
                    model.Title = item.news.Title;
                    model.Spot = item.news.Spot;
                    model.PublishedTime = item.news.PublishedTime;
                    model.CategoryId = item.news.CategoryId;
                    modelNew.Add(model);
                }

                TempData["kelime"] = searchnews;

                int count = modelNew.Count;
                TempData["toplam"] = count;

                categoryList = _mapper.Map<List<CategoryListItemDto>, List<CategoryListViewModel>>(_categoryService.GetAllCategory());

                ViewBag.Categories = categoryList;

                NewsLıstItemModel[] tagsList = modelNew.GroupBy(o => new { o.Title }).Select(x => x.FirstOrDefault()).ToArray();

                return View(PaginationList<NewsLıstItemModel>.Create(tagsList.ToList(), pageNumber ?? 1, pageSize));
            }

            if (TagId != null && TagId > 0)
            {
                tagNewList = _mapper.Map<List<TagNewsListItemDto>, List<TagNewsListViewModel>>(_newService.tagsListWithNewsByTagId(TagId));
                string nameTag = "";
                foreach (var item in tagNewList)
                {
                    nameTag = item.tag.TagName;

                    NewsLıstItemModel model = new NewsLıstItemModel();
                    model.Id = item.news.Id;
                    model.Image = item.news.Image;
                    model.Title = item.news.Title;
                    model.Spot = item.news.Spot;
                    model.PublishedTime = item.news.PublishedTime;
                    model.CategoryId = item.news.CategoryId;
                    modelNew.Add(model);
                }

                TempData["kelime"] = nameTag;

                int count = modelNew.Count;

                TempData["toplam"] = count;

                categoryList = _mapper.Map<List<CategoryListItemDto>, List<CategoryListViewModel>>(_categoryService.GetAllCategory());

                ViewBag.Categories = categoryList;

                NewsLıstItemModel[] tagsList = modelNew.GroupBy(o => new { o.Title }).Select(x => x.FirstOrDefault()).ToArray();

                return View(PaginationList<NewsLıstItemModel>.Create(tagsList.ToList(), pageNumber ?? 1, pageSize));
            }
            return View();
        }
        public IActionResult kategori(int? pageNumber, int Id)
        {

            var category = _mapper.Map<CategoryDto, CategoryEditViewModel>(_categoryService.GetCategoryById(Id));

            TempData["kategori"] = category.CategoryName;
            int pageSize = 18;
            List<NewsLıstItemModel> newList = null;
            newList = _mapper.Map<List<NewsListItemDto>, List<NewsLıstItemModel>>(_newService.newsListByCategoryId(Id));

            return View(PaginationList<NewsLıstItemModel>.Create(newList.ToList(), pageNumber ?? 1, pageSize));
        }
        public IActionResult Slider()
        {
            return PartialView("Slider");
        }
        public IActionResult Carousel()
        {
            return PartialView("Carousel");
        }
        public IActionResult Gundem()
        {
            return PartialView("Gundem");
        }
        public IActionResult Yazar()
        {
            return PartialView("Yazar");
        }
        public IActionResult Footer()
        {
            return PartialView("Footer");
        }
        public IActionResult yazaryazilari(int id, int? pageNumber)
        {
            int pageSize = 20;
            var guest = _mapper.Map<GuestDto, GuestEditViewModel>(_newService.getGuest(id));
            ViewBag.Guest = guest;

            List<CategoryListViewModel> categoryList = null;
            categoryList = _mapper.Map<List<CategoryListItemDto>, List<CategoryListViewModel>>(_categoryService.GetAllCategory());
            ViewBag.Categories = categoryList;

            List<NewsLıstItemModel> newList = _mapper.Map<List<NewsListItemDto>, List<NewsLıstItemModel>>(_newService.newsListWithGuest(guest.Id));
            return View(PaginationList<NewsLıstItemModel>.Create(newList.ToList(), pageNumber ?? 1, pageSize));
        }
        public IActionResult yazarlar()
        {
            List<GuestListViewModel> guestList = null;
            guestList = _mapper.Map<List<GuestListItemDto>, List<GuestListViewModel>>(_newService.guestList());
            return View(guestList);
        }

        [HttpGet("haber/{Id}/{Title}", Name = "haber")]
        public IActionResult haber(int Id, string Title)
        {
            var newsGet = _mapper.Map<NewsDto, NewsEditViewModel>(_newService.getNews(Id));
            ViewBag.Content = HtmlToPlainText(newsGet.NewsContent);

            string friendlyTitle = Title;

            List<TagNewsListViewModel> tagNewsList = null;
            tagNewsList = _mapper.Map<List<TagNewsListItemDto>, List<TagNewsListViewModel>>(_newService.tagsListWithNewsByNewsId(Id));
            ViewBag.TagNews = tagNewsList;

            List<NewsLıstItemModel> newsListRelational = null;
            newsListRelational = _mapper.Map<List<NewsListItemDto>, List<NewsLıstItemModel>>(_newService.newsListByCategoryId(newsGet.CategoryId));
            ViewBag.Relational = newsListRelational;

            List<NewsLıstItemModel> newsListPopularite = null;
            newsListPopularite = _mapper.Map<List<NewsListItemDto>, List<NewsLıstItemModel>>(_newService.PopularNewsInWeb());
            ViewBag.Popularite = newsListPopularite;

            List<NewsLıstItemModel> newListPopulariteByCategory = null;
            newListPopulariteByCategory = _mapper.Map<List<NewsListItemDto>, List<NewsLıstItemModel>>(_newService.PopularNewsInWebInCategory(newsGet.CategoryId));
            ViewBag.PopulariteByCategory = newListPopulariteByCategory;

            List<CategoryListViewModel> categoryNewList = null;
            categoryNewList = _mapper.Map<List<CategoryListItemDto>, List<CategoryListViewModel>>(_categoryService.GetAllCategory());
            ViewBag.CategotyList = categoryNewList;

            if (!string.Equals(friendlyTitle, Title, StringComparison.Ordinal))
            {
                // If the title is null, empty or does not match the friendly title, return a 301 Permanent
                // Redirect to the correct friendly URL.
                return this.RedirectToRoutePermanent("haber", new { id = Id, title = friendlyTitle });
            }

            return View(newsGet);
        }

        [HttpGet]
        public IActionResult RSS()
        {
            List<NewsLıstItemModel> haberlist = null;
            haberlist = _mapper.Map<List<NewsListItemDto>, List<NewsLıstItemModel>>(_newService.newsList());

            var feed = new SyndicationFeed("Title", "Description", new Uri("https://www.gazetekapi.com"), "RSSUrl", DateTime.Now);
            feed.Copyright = new TextSyndicationContent($"{DateTime.Now.Year} Gazete Kapı");

            var items = new List<SyndicationItem>();

            foreach (var item in haberlist)
            {
                items.Add(new SyndicationItem(item.Title, item.Spot, new Uri("https://www.gazetekapi.com/anasayfa/haber/" + item.Id + "/" + item.GenerateSlug()), item.Id.ToString(), DateTime.Now));
            }

            feed.Items = items;
            var setting = new XmlWriterSettings
            {
                Encoding = System.Text.Encoding.UTF8,
                NewLineHandling = NewLineHandling.Entitize,
                NewLineOnAttributes = true,
                Indent = true,
            };

            using (var stream = new MemoryStream())
            {
                using (var xmlWriter = XmlWriter.Create(stream, setting))
                {
                    var rssFormator = new Rss20FeedFormatter(feed, false);
                    rssFormator.WriteTo(xmlWriter);
                    xmlWriter.Flush();
                }
                return File(stream.ToArray(), "application/rss+xml;charset=utf-8");
            }
        }

        //public IActionResult newssitemap()
        //{
        //    var haberlist = _mapper.Map<List<NewsListItemDto>, List<NewsLıstItemModel>>(_newService.newsList()).AsQueryable();

        //    var productSitemapIndexConfiguration = new ProductSitemapIndexConfiguration(haberlist, null, Url);

        //    return _siteMapProvider.CreateSitemapIndex(new SitemapIndexModel(new List<SitemapIndexNode> {

        //        new SitemapIndexNode(Url.Action("sayfa")),
        //        new SitemapIndexNode(Url.Action("haber")),
        //        new SitemapIndexNode(Url.Action("kategori")),
        //        new SitemapIndexNode(Url.Action("yazarlar")),
        //        new SitemapIndexNode(Url.Action("aramasonucu")),
        //        new SitemapIndexNode(Url.Action("yazaryazilari")),
        //    }
        //    ));

        //    //return new DynamicSitemapIndexProvider().CreateSitemapIndex(new SitemapProvider(new BaseUrlProvider()), productSitemapIndexConfiguration);

        //}7

        #region Text to Speech

        private static string HtmlToPlainText(string html)
        {
            const string tagWhiteSpace = @"(>|$)(\W|\n|\r)+<";//matches one or more (white space or line breaks) between '>' and '<'
            const string stripFormatting = @"<[^>]*(>|$)";//match any character between '<' and '>', even when end tag is missing
            const string lineBreak = @"<(br|BR)\s{0,1}\/{0,1}>";//matches: <br>,<br/>,<br />,<BR>,<BR/>,<BR />
            var lineBreakRegex = new Regex(lineBreak, RegexOptions.Multiline);
            var stripFormattingRegex = new Regex(stripFormatting, RegexOptions.Multiline);
            var tagWhiteSpaceRegex = new Regex(tagWhiteSpace, RegexOptions.Multiline);

            var text = html;
            //Decode html specific characters
            text = System.Net.WebUtility.HtmlDecode(text);
            //Remove tag whitespace/line breaks
            text = tagWhiteSpaceRegex.Replace(text, "><");
            //Replace <br /> with line breaks
            text = lineBreakRegex.Replace(text, Environment.NewLine);
            //Strip formatting
            text = stripFormattingRegex.Replace(text, string.Empty);

            return text;
        }

        #endregion

    }


}
