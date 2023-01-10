using AutoMapper;
using CORE.ApplicationCommon.DTOS.BannersDTO;
using CORE.ApplicationCommon.DTOS.CategoryDTO;
using CORE.ApplicationCommon.DTOS.IpAddressDTO;
using CORE.ApplicationCommon.DTOS.NewsDTO;
using CORE.ApplicationCommon.DTOS.NewsDTO.GuestDTO;
using CORE.ApplicationCommon.DTOS.NewsDTO.IpNewsDTO;
using CORE.ApplicationCommon.DTOS.NewsDTO.TagNewsDTO;
using CORE.ApplicationCommon.DTOS.PrivacyDTO.PolicyDto;
using CORE.ApplicationCommon.DTOS.PrivacyDTO.PrivacyDto;
using CORE.ApplicationCommon.DTOS.PrivacyDTO.TermsOfUsDto;
using CORE.ApplicationCommon.Helpers;
using GazeteKapiMVC5Core.WEB.Models.ConfigreCaptcha;
using GazeteKapiMVC5Core.WEB.Models.MetaConfig;
using GazeteKapiMVC5Core.WEB.Models.RenderService;
using GazeteKapiMVC5Core.WEB.ViewModels.Banner;
using GazeteKapiMVC5Core.WEB.ViewModels.Categories;
using GazeteKapiMVC5Core.WEB.ViewModels.Guests;
using GazeteKapiMVC5Core.WEB.ViewModels.Members;
using GazeteKapiMVC5Core.WEB.ViewModels.News;
using GazeteKapiMVC5Core.WEB.ViewModels.NewsIp;
using GazeteKapiMVC5Core.WEB.ViewModels.Policy;
using GazeteKapiMVC5Core.WEB.ViewModels.TagsNews;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SERVICE.Engine.Interfaces;
using SERVICES.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace GazeteKapiMVC5Core.WEB.Controllers
{
    public class anasayfaController : Controller
    {

        #region Constructre

        private readonly IMapper _mapper;
        private readonly INewsService _newService;
        private readonly ICategoryService _categoryService;
        private readonly ISettingService _settingService;
        private readonly IViewRenderService _viewRender;
        private readonly IIPAddresService _ipAddressService;
        private readonly INewsIpService _newsIpService;
        private readonly IBannerService _bannerService;
        private readonly IContactService _contactService;
        private readonly reCaptchaService _repService;
        private int BATCH_SIZE = 1;

        [Obsolete]
        public anasayfaController(INewsService newService, ICategoryService categoryService, IMapper mapper, ISettingService settingService, IViewRenderService viewRender, reCaptchaService repService, IIPAddresService ipAddressService, IContactService contactService, INewsIpService newsIpService, IBannerService bannerService)
        {
            _newService = newService;
            _categoryService = categoryService;
            _mapper = mapper;
            _settingService = settingService;
            _viewRender = viewRender;
            _repService = repService;
            _ipAddressService = ipAddressService;
            _newsIpService = newsIpService;
            _contactService = contactService;
            _bannerService = bannerService;
        }

        #endregion

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult sayfa()
        {

            #region Data

            TimerControl();
            var haberlist = _mapper.Map<List<NewsListItemDto>, List<NewListViewModelWeb>>(_newService.newsListWithWeb());

            List<GuestListViewModelWeb> guestList = _mapper.Map<List<GuestListItemDto>, List<GuestListViewModelWeb>>(_newService.guestList());
            List<TagNewsListViewModelWeb> tagNewList = _mapper.Map<List<TagNewsListItemDto>, List<TagNewsListViewModelWeb>>(_newService.tagsListWithNewsWeb());
            List<CategoryListViewModelWeb> categoryList = _mapper.Map<List<CategoryListItemDto>, List<CategoryListViewModelWeb>>(_categoryService.GetAllCategory());

            ViewBag.CategoryList = categoryList;
            ViewBag.TagNews = tagNewList;
            ViewBag.HaberlerManset = haberlist;
            ViewBag.GuestList = guestList;

            var category = _mapper.Map<CategoryDto, CategoryEditViewModelWeb>(_categoryService.GetCategoryStyleId(5));

            ViewBag.CategoryIdOne = category;

            LoadJsonData();

            #endregion

            #region Meta

            MetaViewModel meta = new MetaViewModel();
            meta.Title = "Gazetekapı | Le Monde diplomatique Türkiye";
            meta.Keywords = "Gazetekapı, Lemonde, Yaşam, Kültür, Sanat, Seçim, Ekonomi, Siyaset";
            meta.Description = "Gazetekapı yeni haberciliğiyle yola çıktı! Gazetekapı ile bilim teknoloji, yaşam, siyaset, ekonomiye dair bütün haberleri sizlerle buluşturacağız!";
            meta.Image = "https://uploads.gazetekapi.com/images/placeholder/kapi-logo.png";
            meta.ogDescription = "Gazetekapı yeni haberciliğiyle yola çıktı! Gazetekapı ile bilim teknoloji, yaşam, siyaset, ekonomiye dair bütün haberleri sizlerle buluşturacağız!";
            meta.ogTitle = "Gazetekapı | Le Monde diplomatique Türkiye";
            meta.ogImage = "https://uploads.gazetekapi.com/images/placeholder/kapi-logo.png";
            meta.Url = "https://www.gazetekapi.com/";
            ViewBag.Meta = meta;

            #endregion

            #region LoadBannerData

            LoadBannerData();

            #endregion

            return View();
        }
        private void LoadBannerData()
        {
            var bannerOrta = _mapper.Map<BannerDto, BannerEditViewModelWeb>(_bannerService.getBanner(4));
            var bannerAlt = _mapper.Map<BannerDto, BannerEditViewModelWeb>(_bannerService.getBanner(5));

            ViewBag.BannerOrta = bannerOrta;
            ViewBag.BannerAlt = bannerAlt;
        }
        public IActionResult aramasonucu(int? pageNumber, string searchnews, int? TagId)
        {
            int pageSize = 20;
            List<NewListViewModelWeb> haberlist = null;
            List<TagNewsListViewModelWeb> tagNewList = null;
            List<CategoryListViewModelWeb> categoryList = null;
            List<NewListViewModelWeb> modelNew = new List<NewListViewModelWeb>();
            MetaViewModel meta = new MetaViewModel();

            //var haberlistHaber = _mapper.Map<List<NewsListItemDto>, List<NewListViewModelWeb>>(_newService.newsListWithWeb());

            //ViewBag.HaberlerManset = haberlistHaber;

            if (searchnews != null && searchnews != "")
            {
                haberlist = _mapper.Map<List<NewsListItemDto>, List<NewListViewModelWeb>>(_newService.searchDataInNews(searchnews));

                tagNewList = _mapper.Map<List<TagNewsListItemDto>, List<TagNewsListViewModelWeb>>(_newService.tagsListWithNewsWebSearch(searchnews));

                foreach (var item in haberlist)
                {
                    NewListViewModelWeb model = new NewListViewModelWeb
                    {
                        Id = item.Id,
                        Image = item.Image,
                        Title = item.Title,
                        MetaTitle = item.MetaTitle,
                        Spot = item.Spot,
                        PublishedTime = item.PublishedTime,
                        CategoryId = item.CategoryId
                    };
                    modelNew.Add(model);
                }

                foreach (var item in tagNewList)
                {

                    NewListViewModelWeb model = new NewListViewModelWeb
                    {
                        Id = item.news.Id,
                        Image = item.news.Image,
                        Title = item.news.Title,
                        MetaTitle = item.news.MetaTitle,
                        Spot = item.news.Spot,
                        PublishedTime = item.news.PublishedTime,
                        CategoryId = item.news.CategoryId
                    };
                    modelNew.Add(model);
                }

                TempData["kelime"] = searchnews;

                int count = modelNew.Count;
                TempData["toplam"] = count;

                categoryList = _mapper.Map<List<CategoryListItemDto>, List<CategoryListViewModelWeb>>(_categoryService.GetAllCategory());

                ViewBag.CategoryList = categoryList;

                NewListViewModelWeb[] tagsList = modelNew.GroupBy(o => new { o.Title }).Select(x => x.FirstOrDefault()).ToArray();

                #region Meta

                meta.Title = searchnews;
                meta.Keywords = "Gazetekapı, Lemonde, Yaşam, Kültür, Sanat, Seçim, Ekonomi, Siyaset";
                meta.Description = "Gazetekapı yeni haberciliğiyle yola çıktı! Gazetekapı ile bilim teknoloji, yaşam, siyaset, ekonomiye dair bütün haberleri sizlerle buluşturacağız!";
                meta.Image = "https://uploads.gazetekapi.com/images/placeholder/kapi-logo.png";
                meta.ogDescription = "Gazetekapı yeni haberciliğiyle yola çıktı! Gazetekapı ile bilim teknoloji, yaşam, siyaset, ekonomiye dair bütün haberleri sizlerle buluşturacağız!";
                meta.ogTitle = searchnews;
                meta.ogImage = "https://uploads.gazetekapi.com/images/placeholder/kapi-logo.png";
                meta.Url = "https://www.gazetekapi.com/anasayfa/aramasonucu";
                ViewBag.Meta = meta;

                #endregion

                return View(tagsList.ToList());

            }

            if (TagId != null && TagId > 0)
            {
                tagNewList = _mapper.Map<List<TagNewsListItemDto>, List<TagNewsListViewModelWeb>>(_newService.tagsListWithNewsByTagId(TagId));
                string nameTag = "";
                foreach (var item in tagNewList)
                {
                    nameTag = item.tag.TagName;

                    NewListViewModelWeb model = new NewListViewModelWeb
                    {
                        Id = item.news.Id,
                        Image = item.news.Image,
                        Title = item.news.Title,
                        MetaTitle = item.news.MetaTitle,
                        Spot = item.news.Spot,
                        PublishedTime = item.news.PublishedTime,
                        CategoryId = item.news.CategoryId
                    };
                    modelNew.Add(model);
                }

                TempData["kelime"] = nameTag;

                int count = modelNew.Count;

                TempData["toplam"] = count;

                categoryList = _mapper.Map<List<CategoryListItemDto>, List<CategoryListViewModelWeb>>(_categoryService.GetAllCategory());

                ViewBag.CategoryList = categoryList;

                NewListViewModelWeb[] tagsList = modelNew.GroupBy(o => new { o.Title }).Select(x => x.FirstOrDefault()).ToArray();

                #region Meta

                meta.Title = searchnews;
                meta.Keywords = "Gazetekapı, Lemonde, Yaşam, Kültür, Sanat, Seçim, Ekonomi, Siyaset";
                meta.Description = "Gazetekapı yeni haberciliğiyle yola çıktı! Gazetekapı ile bilim teknoloji, yaşam, siyaset, ekonomiye dair bütün haberleri sizlerle buluşturacağız!";
                meta.Image = "https://uploads.gazetekapi.com/images/placeholder/kapi-logo.png";
                meta.ogDescription = "Gazetekapı yeni haberciliğiyle yola çıktı! Gazetekapı ile bilim teknoloji, yaşam, siyaset, ekonomiye dair bütün haberleri sizlerle buluşturacağız!";
                meta.ogTitle = searchnews;
                meta.ogImage = "https://uploads.gazetekapi.com/images/placeholder/kapi-logo.png";
                meta.Url = "https://www.gazetekapi.com/anasayfa/aramasonucu";
                ViewBag.Meta = meta;

                #endregion

                return View(tagsList.ToList());
            }

            #region Meta

            meta.Title = searchnews;
            meta.Keywords = "Gazetekapı, Lemonde, Yaşam, Kültür, Sanat, Seçim, Ekonomi, Siyaset";
            meta.Description = "Gazetekapı yeni haberciliğiyle yola çıktı! Gazetekapı ile bilim teknoloji, yaşam, siyaset, ekonomiye dair bütün haberleri sizlerle buluşturacağız!";
            meta.Image = "https://uploads.gazetekapi.com/images/placeholder/kapi-logo.png";
            meta.ogDescription = "Gazetekapı yeni haberciliğiyle yola çıktı! Gazetekapı ile bilim teknoloji, yaşam, siyaset, ekonomiye dair bütün haberleri sizlerle buluşturacağız!";
            meta.ogTitle = searchnews;
            meta.ogImage = "https://uploads.gazetekapi.com/images/placeholder/kapi-logo.png";
            meta.Url = "https://www.gazetekapi.com/anasayfa/aramasonucu";
            ViewBag.Meta = meta;

            #endregion

            return View();
        }
        public IActionResult kategori(int? pageNumber, int Id)
        {
            List<CategoryListViewModelWeb> categoryList = _mapper.Map<List<CategoryListItemDto>, List<CategoryListViewModelWeb>>(_categoryService.GetAllCategory());
            ViewBag.CategoryList = categoryList;

            //var haberlist = _mapper.Map<List<NewsListItemDto>, List<NewListViewModelWeb>>(_newService.newsListWithWeb());
            //ViewBag.HaberlerManset = haberlist;

            var category = _mapper.Map<CategoryDto, CategoryEditViewModelWeb>(_categoryService.GetCategoryById(Id));

            TempData["kategori"] = category.CategoryName;
            ViewData["id"] = category.Id;

            #region Meta

            MetaViewModel meta = new MetaViewModel();
            meta.Title = category.CategoryName;
            meta.Keywords = "Gazetekapı, Lemonde, Yaşam, Kültür, Sanat, Seçim, Ekonomi, Siyaset";
            meta.Description = category.Description;
            meta.Image = "https://uploads.gazetekapi.com/images/placeholder/kapi-logo.png";
            meta.ogDescription = category.Description;
            meta.ogTitle = category.CategoryName;
            meta.ogImage = "https://uploads.gazetekapi.com/images/placeholder/kapi-logo.png";
            meta.Url = "https://www.gazetekapi.com/anasayfa/kategpri/" + category.Id;
            ViewBag.Meta = meta;

            #endregion

            int pageSize = 18;

            if(category.Id == 25)
            {
                List<NewListViewModelWeb> newListView = _mapper.Map<List<NewsListItemDto>, List<NewListViewModelWeb>>(_newService.newsList());
                return View(PaginationList<NewListViewModelWeb>.Create(newListView.ToList(), pageNumber ?? 1, pageSize));
            }
            else
            {
                List<NewListViewModelWeb> newList = _mapper.Map<List<NewsListItemDto>, List<NewListViewModelWeb>>(_newService.newsListByCategoryId(Id));
                return View(PaginationList<NewListViewModelWeb>.Create(newList.ToList(), pageNumber ?? 1, pageSize));
            }
        }
        public IActionResult yazaryazilari(int id, int? pageNumber)
        {
            int pageSize = 20;
            var guest = _mapper.Map<GuestDto, GuestEditViewModelWeb>(_newService.getGuest(id));
            ViewBag.Guest = guest;

            //List<CategoryListViewModelWeb> categoryList = _mapper.Map<List<CategoryListItemDto>, List<CategoryListViewModelWeb>>(_categoryService.GetAllCategory());
            //ViewBag.CategoryList = categoryList;
            //ViewBag.Categories = categoryList;

            List<NewListViewModelWeb> newList = _mapper.Map<List<NewsListItemDto>, List<NewListViewModelWeb>>(_newService.newsListWithGuest(guest.Id));
            //ViewBag.HaberlerManset = newList;

            #region Meta

            MetaViewModel meta = new MetaViewModel();
            meta.Title = guest.GuestName;
            meta.Keywords = "Gazetekapı, Lemonde, Yaşam, Kültür, Sanat, Seçim, Ekonomi, Siyaset";
            meta.Description = guest.Biography;
            meta.Image = "https://uploads.gazetekapi.com/images/" + guest.GuestImage;
            meta.ogDescription = guest.Biography;
            meta.ogTitle = guest.GuestName;
            meta.ogImage = "https://uploads.gazetekapi.com/images/" + guest.GuestImage;
            meta.Url = "https://www.gazetekapi.com/anasayfa/yazaryazilari/" + guest.Id;
            ViewBag.Meta = meta;

            #endregion

            return View(PaginationList<NewListViewModelWeb>.Create(newList.ToList(), pageNumber ?? 1, pageSize));
        }
        public IActionResult yazarlar()
        {
            List<GuestListViewModelWeb> guestList = _mapper.Map<List<GuestListItemDto>, List<GuestListViewModelWeb>>(_newService.guestList());

            //List<CategoryListViewModelWeb> categoryList = _mapper.Map<List<CategoryListItemDto>, List<CategoryListViewModelWeb>>(_categoryService.GetAllCategory());
            //ViewBag.CategoryList = categoryList;

            #region Meta

            MetaViewModel meta = new MetaViewModel();
            meta.Title = "Gazetekapı | Le Monde diplomatique Türkiye";
            meta.Keywords = "Gazetekapı, Lemonde, Yaşam, Kültür, Sanat, Seçim, Ekonomi, Siyaset";
            meta.Description = "Gazetekapı yeni haberciliğiyle yola çıktı! Gazetekapı ile bilim teknoloji, yaşam, siyaset, ekonomiye dair bütün haberleri sizlerle buluşturacağız!";
            meta.Image = "https://uploads.gazetekapi.com/images/placeholder/kapi-logo.png";
            meta.ogDescription = "Gazetekapı yeni haberciliğiyle yola çıktı! Gazetekapı ile bilim teknoloji, yaşam, siyaset, ekonomiye dair bütün haberleri sizlerle buluşturacağız!";
            meta.ogTitle = "Gazetekapı | Le Monde diplomatique Türkiye";
            meta.ogImage = "https://uploads.gazetekapi.com/images/placeholder/kapi-logo.png";
            meta.Url = "https://www.gazetekapi.com/";
            ViewBag.Meta = meta;

            #endregion

            return View(guestList);
        }

        [HttpGet("haber/{Id}/{MetaTitle}", Name = "haber")]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> haber(int Id, string MetaTitle)
        {
            var bannerSagAlt = _mapper.Map<BannerDto, BannerEditViewModelWeb>(_bannerService.getBanner(8));
            ViewBag.BannerSagAlt = bannerSagAlt;

            //int resultId = Convert.ToInt32(await _newService.insertViewNews(Id));

            List<CategoryListViewModelWeb> categoryList = _mapper.Map<List<CategoryListItemDto>, List<CategoryListViewModelWeb>>(_categoryService.GetAllCategory());
            ViewBag.CategoryList = categoryList;

            var newsGet = _mapper.Map<NewsDto, NewsEditViewModelWeb>(_newService.getNews(Id));
            ViewBag.Content = HtmlToPlainText(newsGet.NewsContent);

            var haberlist = _mapper.Map<List<NewsListItemDto>, List<NewListViewModelWeb>>(_newService.newsListWithWeb());
            ViewBag.HaberlerManset = haberlist;

            var guest = _mapper.Map<GuestDto, GuestEditViewModelWeb>(_newService.getGuest(newsGet.GuestId));
            ViewBag.Guest = guest;

            #region Calculate View Count

            string ipAddress = getUserIp();
            var ip = _ipAddressService.getIpAdress(ipAddress);

            if (ip != null)
            {
                var ipAdressExistsInNews = _newsIpService.listIpByNewsId(newsGet.Id, ip.Id);
                if (ipAdressExistsInNews == null)
                {
                    IpNewsDto model = new IpNewsDto
                    {
                        IpAdressId = ip.Id,
                        NewsId = newsGet.Id,
                        ClickTime = DateTime.Now,
                    };
                    await _newsIpService.createNewsIp(model);
                    int resultId = Convert.ToInt32(await _newService.insertViewNews(newsGet.Id));
                }
            }
            else
            {
                IpAdressDto newIp = new IpAdressDto
                {
                    ipAddress = ipAddress
                };
                int resultId = Convert.ToInt32(await _ipAddressService.createIpAddressInDatabase(_mapper.Map<IpAdressBaseDto, IpAdressDto>(newIp)));

                IpNewsDto model = new IpNewsDto
                {
                    IpAdressId = resultId,
                    NewsId = newsGet.Id,
                    ClickTime = DateTime.Now,

                };
                await _newsIpService.createNewsIp(model);

                int resultNewsId = Convert.ToInt32(await _newService.insertViewNews(newsGet.Id));

            }

            #endregion

            #region Datas

            string friendlyTitle = MetaTitle;
            List<TagNewsListViewModelWeb> tagNewsList = _mapper.Map<List<TagNewsListItemDto>, List<TagNewsListViewModelWeb>>(_newService.tagsListWithNewsByNewsId(Id));
            ViewBag.TagNews = tagNewsList;

            List<NewListViewModelWeb> newsListRelational = _mapper.Map<List<NewsListItemDto>, List<NewListViewModelWeb>>(_newService.newsListByCategoryId(newsGet.CategoryId));
            ViewBag.Relational = newsListRelational;

            List<CategoryListViewModelWeb> categoryNewList = _mapper.Map<List<CategoryListItemDto>, List<CategoryListViewModelWeb>>(_categoryService.GetAllCategory());
            ViewBag.CategotyList = categoryNewList;

            List<NewListViewModelWeb> newListAll = _mapper.Map<List<NewsListItemDto>, List<NewListViewModelWeb>>(_newService.newsListWithWeb());
            ViewBag.AllNewsList = newListAll;

            #endregion

            #region GetTags

            var tags = _mapper.Map<List<TagNewsListItemDto>, List<TagNewsListViewModelWeb>>(_newService.tagsListWithNewsByNewsId(Id));

            List<string> list = new List<string>();

            foreach (var item in tags)
            {
                list.Add(item.tag.TagName);
            }

            string[] tagsList = list.ToArray();

            for (int i = 0; i < tagsList.Count(); i++)
            {
                if (newsGet.Tag != null)
                {
                    newsGet.Tag = newsGet.Tag + "," + tagsList[i];
                }
                else
                {
                    newsGet.Tag = tagsList[i];
                }
            }

            #endregion

            #region Meta

            MetaViewModel meta = new MetaViewModel();
            meta.Title = newsGet.MetaTitle;
            meta.Keywords = newsGet.Tag;
            meta.Description = newsGet.Spot;
            meta.Image = "https://uploads.gazetekapi.com/images/" + newsGet.Image;
            meta.ogDescription = newsGet.Spot;
            meta.ogTitle = newsGet.Title;
            meta.ogImage = "https://uploads.gazetekapi.com/images/" + newsGet.Image;
            meta.Url = "https://www.gazetekapi.com/" + Id + newsGet.Title;
            ViewBag.Meta = meta;

            #endregion

            if (!string.Equals(friendlyTitle, MetaTitle, StringComparison.Ordinal))
            {
                return this.RedirectToRoutePermanent("haber", new { id = Id, metatitle = friendlyTitle });
            }

            return View(newsGet);
        }
        private string getUserIp()
        {
            IPAddress remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;
            string result = "";
            if (remoteIpAddress != null)
            {
                // If we got an IPV6 address, then we need to ask the network for the IPV4 address 
                // This usually only happens when the browser is on the same machine as the server.
                if (remoteIpAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                {
                    remoteIpAddress = System.Net.Dns.GetHostEntry(remoteIpAddress).AddressList
            .First(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
                }
                result = remoteIpAddress.ToString();
            }
            return result;
        }
        public IActionResult gizlilikpolitikasi()
        {
            //List<CategoryListViewModelWeb> categoryList = _mapper.Map<List<CategoryListItemDto>, List<CategoryListViewModelWeb>>(_categoryService.GetAllCategory());
            //ViewBag.CategoryList = categoryList;

            //var haberlist = _mapper.Map<List<NewsListItemDto>, List<NewListViewModelWeb>>(_newService.newsListWithWeb());
            //ViewBag.HaberlerManset = haberlist;

            var getPrivacy = _mapper.Map<PrivacyDto, PrivacyBaseViewModel>(_settingService.getPrivacy(1));

            #region Meta

            MetaViewModel meta = new MetaViewModel();
            meta.Title = "Gazetekapı | Le Monde diplomatique Türkiye";
            meta.Keywords = "Gazetekapı, Lemonde, Yaşam, Kültür, Sanat, Seçim, Ekonomi, Siyaset";
            meta.Description = "Gazetekapı yeni haberciliğiyle yola çıktı! Gazetekapı ile bilim teknoloji, yaşam, siyaset, ekonomiye dair bütün haberleri sizlerle buluşturacağız!";
            meta.Image = "https://uploads.gazetekapi.com/images/placeholder/kapi-logo.png";
            meta.ogDescription = "Gazetekapı yeni haberciliğiyle yola çıktı! Gazetekapı ile bilim teknoloji, yaşam, siyaset, ekonomiye dair bütün haberleri sizlerle buluşturacağız!";
            meta.ogTitle = "Gazetekapı | Le Monde diplomatique Türkiye";
            meta.ogImage = "https://uploads.gazetekapi.com/images/placeholder/kapi-logo.png";
            meta.Url = "https://www.gazetekapi.com/";
            ViewBag.Meta = meta;

            #endregion

            return View(getPrivacy);
        }
        public IActionResult cerezpolitikasi()
        {
            //List<CategoryListViewModelWeb> categoryList = _mapper.Map<List<CategoryListItemDto>, List<CategoryListViewModelWeb>>(_categoryService.GetAllCategory());
            //ViewBag.CategoryList = categoryList;

            //var haberlist = _mapper.Map<List<NewsListItemDto>, List<NewListViewModelWeb>>(_newService.newsListWithWeb());
            //ViewBag.HaberlerManset = haberlist;

            var getCookiesPolicy = _mapper.Map<CookiePolicyDto, CookieBaseViewModel>(_settingService.getCookiePrivacy(1));

            #region Meta

            MetaViewModel meta = new MetaViewModel();
            meta.Title = "Gazetekapı | Le Monde diplomatique Türkiye";
            meta.Keywords = "Gazetekapı, Lemonde, Yaşam, Kültür, Sanat, Seçim, Ekonomi, Siyaset";
            meta.Description = "Gazetekapı yeni haberciliğiyle yola çıktı! Gazetekapı ile bilim teknoloji, yaşam, siyaset, ekonomiye dair bütün haberleri sizlerle buluşturacağız!";
            meta.Image = "https://uploads.gazetekapi.com/images/placeholder/kapi-logo.png";
            meta.ogDescription = "Gazetekapı yeni haberciliğiyle yola çıktı! Gazetekapı ile bilim teknoloji, yaşam, siyaset, ekonomiye dair bütün haberleri sizlerle buluşturacağız!";
            meta.ogTitle = "Gazetekapı | Le Monde diplomatique Türkiye";
            meta.ogImage = "https://uploads.gazetekapi.com/images/placeholder/kapi-logo.png";
            meta.Url = "https://www.gazetekapi.com/";
            ViewBag.Meta = meta;

            #endregion

            return View(getCookiesPolicy);
        }
        public IActionResult kullanimsartlari()
        {
            //List<CategoryListViewModelWeb> categoryList = _mapper.Map<List<CategoryListItemDto>, List<CategoryListViewModelWeb>>(_categoryService.GetAllCategory());
            //ViewBag.CategoryList = categoryList;

            //var haberlist = _mapper.Map<List<NewsListItemDto>, List<NewListViewModelWeb>>(_newService.newsListWithWeb());
            //ViewBag.HaberlerManset = haberlist;

            var getTermsOfUs = _mapper.Map<TermsOfUsDto, TermsOfUsBaseViewModel>(_settingService.getTermsOfUs(1));

            #region Meta

            MetaViewModel meta = new MetaViewModel();
            meta.Title = "Gazetekapı | Le Monde diplomatique Türkiye";
            meta.Keywords = "Gazetekapı, Lemonde, Yaşam, Kültür, Sanat, Seçim, Ekonomi, Siyaset";
            meta.Description = "Gazetekapı yeni haberciliğiyle yola çıktı! Gazetekapı ile bilim teknoloji, yaşam, siyaset, ekonomiye dair bütün haberleri sizlerle buluşturacağız!";
            meta.Image = "https://uploads.gazetekapi.com/images/placeholder/kapi-logo.png";
            meta.ogDescription = "Gazetekapı yeni haberciliğiyle yola çıktı! Gazetekapı ile bilim teknoloji, yaşam, siyaset, ekonomiye dair bütün haberleri sizlerle buluşturacağız!";
            meta.ogTitle = "Gazetekapı | Le Monde diplomatique Türkiye";
            meta.ogImage = "https://uploads.gazetekapi.com/images/placeholder/kapi-logo.png";
            meta.Url = "https://www.gazetekapi.com/";
            ViewBag.Meta = meta;

            #endregion

            return View(getTermsOfUs);
        }   
        public IActionResult kunye()
        {

            //List<CategoryListViewModelWeb> categoryList = _mapper.Map<List<CategoryListItemDto>, List<CategoryListViewModelWeb>>(_categoryService.GetAllCategory());
            //ViewBag.CategoryList = categoryList;

            //var haberlist = _mapper.Map<List<NewsListItemDto>, List<NewListViewModelWeb>>(_newService.newsListWithWeb());
            //ViewBag.HaberlerManset = haberlist;

            var getBrand = _mapper.Map<BrandPolicyDto, BrandBaseViewModel>(_settingService.getBrandPrivacy(1));

            #region Meta

            MetaViewModel meta = new MetaViewModel();
            meta.Title = "Gazetekapı | Le Monde diplomatique Türkiye";
            meta.Keywords = "Gazetekapı, Lemonde, Yaşam, Kültür, Sanat, Seçim, Ekonomi, Siyaset";
            meta.Description = "Gazetekapı yeni haberciliğiyle yola çıktı! Gazetekapı ile bilim teknoloji, yaşam, siyaset, ekonomiye dair bütün haberleri sizlerle buluşturacağız!";
            meta.Image = "https://uploads.gazetekapi.com/images/placeholder/kapi-logo.png";
            meta.ogDescription = "Gazetekapı yeni haberciliğiyle yola çıktı! Gazetekapı ile bilim teknoloji, yaşam, siyaset, ekonomiye dair bütün haberleri sizlerle buluşturacağız!";
            meta.ogTitle = "Gazetekapı | Le Monde diplomatique Türkiye";
            meta.ogImage = "https://uploads.gazetekapi.com/images/placeholder/kapi-logo.png";
            meta.Url = "https://www.gazetekapi.com/";
            ViewBag.Meta = meta;

            #endregion

            return View(getBrand);
        }
        public IActionResult yayinilkeleri()
        {
            //List<CategoryListViewModelWeb> categoryList = _mapper.Map<List<CategoryListItemDto>, List<CategoryListViewModelWeb>>(_categoryService.GetAllCategory());
            //ViewBag.CategoryList = categoryList;

            //var haberlist = _mapper.Map<List<NewsListItemDto>, List<NewListViewModelWeb>>(_newService.newsListWithWeb());
            //ViewBag.HaberlerManset = haberlist;

            var getStreamPolicy = _mapper.Map<StreamPolicyDto, StreamBaseViewModel>(_settingService.getStreamPrivacy(2));

            #region Meta

            MetaViewModel meta = new MetaViewModel();
            meta.Title = "Gazetekapı | Le Monde diplomatique Türkiye";
            meta.Keywords = "Gazetekapı, Lemonde, Yaşam, Kültür, Sanat, Seçim, Ekonomi, Siyaset";
            meta.Description = "Gazetekapı yeni haberciliğiyle yola çıktı! Gazetekapı ile bilim teknoloji, yaşam, siyaset, ekonomiye dair bütün haberleri sizlerle buluşturacağız!";
            meta.Image = "https://uploads.gazetekapi.com/images/placeholder/kapi-logo.png";
            meta.ogDescription = "Gazetekapı yeni haberciliğiyle yola çıktı! Gazetekapı ile bilim teknoloji, yaşam, siyaset, ekonomiye dair bütün haberleri sizlerle buluşturacağız!";
            meta.ogTitle = "Gazetekapı | Le Monde diplomatique Türkiye";
            meta.ogImage = "https://uploads.gazetekapi.com/images/placeholder/kapi-logo.png";
            meta.Url = "https://www.gazetekapi.com/";
            ViewBag.Meta = meta;

            #endregion

            return View(getStreamPolicy);
        }
        public IActionResult arsiv(int? pageNumber)
        {

            //List<CategoryListViewModelWeb> categoryList = _mapper.Map<List<CategoryListItemDto>, List<CategoryListViewModelWeb>>(_categoryService.GetAllCategory());
            //ViewBag.CategoryList = categoryList;

            //var haberlist = _mapper.Map<List<NewsListItemDto>, List<NewListViewModelWeb>>(_newService.newsListWithWeb());
            //ViewBag.HaberlerManset = haberlist;

            #region Meta

            MetaViewModel meta = new MetaViewModel();
            meta.Title = "Gazetekapı | Le Monde diplomatique Türkiye";
            meta.Keywords = "Gazetekapı, Lemonde, Yaşam, Kültür, Sanat, Seçim, Ekonomi, Siyaset";
            meta.Description = "Gazetekapı yeni haberciliğiyle yola çıktı! Gazetekapı ile bilim teknoloji, yaşam, siyaset, ekonomiye dair bütün haberleri sizlerle buluşturacağız!";
            meta.Image = "https://uploads.gazetekapi.com/images/placeholder/kapi-logo.png";
            meta.ogDescription = "Gazetekapı yeni haberciliğiyle yola çıktı! Gazetekapı ile bilim teknoloji, yaşam, siyaset, ekonomiye dair bütün haberleri sizlerle buluşturacağız!";
            meta.ogTitle = "Gazetekapı | Le Monde diplomatique Türkiye";
            meta.ogImage = "https://uploads.gazetekapi.com/images/placeholder/kapi-logo.png";
            meta.Url = "https://www.gazetekapi.com/";
            ViewBag.Meta = meta;

            #endregion

            int pageSize = 18;

            List<NewListViewModelWeb> newList = _mapper.Map<List<NewsListItemDto>, List<NewListViewModelWeb>>(_newService.newsListWithWeb());

            return View(PaginationList<NewListViewModelWeb>.Create(newList.ToList(), pageNumber ?? 1, pageSize));
        }

        [Route("/anasayfa/hata/{code:int}")]
        public IActionResult hata(int code)
        {
            //List<CategoryListViewModelWeb> categoryList = _mapper.Map<List<CategoryListItemDto>, List<CategoryListViewModelWeb>>(_categoryService.GetAllCategory());
            //ViewBag.CategoryList = categoryList;

            //var haberlist = _mapper.Map<List<NewsListItemDto>, List<NewListViewModelWeb>>(_newService.newsListWithWeb());
            //ViewBag.HaberlerManset = haberlist;

            MetaViewModel meta = new MetaViewModel();
            meta.Title = "Gazetekapı | Le Monde diplomatique Türkiye";
            meta.Keywords = "Gazetekapı, Lemonde, Yaşam, Kültür, Sanat, Seçim, Ekonomi, Siyaset";
            meta.Description = "Gazetekapı yeni haberciliğiyle yola çıktı! Gazetekapı ile bilim teknoloji, yaşam, siyaset, ekonomiye dair bütün haberleri sizlerle buluşturacağız!";
            meta.Image = "https://uploads.gazetekapi.com/images/placeholder/kapi-logo.png";
            meta.ogDescription = "Gazetekapı yeni haberciliğiyle yola çıktı! Gazetekapı ile bilim teknoloji, yaşam, siyaset, ekonomiye dair bütün haberleri sizlerle buluşturacağız!";
            meta.ogTitle = "Gazetekapı | Le Monde diplomatique Türkiye";
            meta.ogImage = "https://uploads.gazetekapi.com/images/placeholder/kapi-logo.png";
            meta.Url = "https://www.gazetekapi.com/";
            ViewBag.Meta = meta;

            ViewData["ErrorCode"] = $"{code}";
            return View("~/Views/anasayfa/hata.cshtml");
        }

        #region Partial Views
        public IActionResult Slider()
        {
            return PartialView("Slider");
        }
        public IActionResult SubCategories()
        {
            return PartialView("SubCategories");
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
        [HttpGet]
        public IActionResult RSS()
        {
            List<NewListViewModelWeb> haberlist = _mapper.Map<List<NewsListItemDto>, List<NewListViewModelWeb>>(_newService.newsList());

            var feed = new SyndicationFeed("Title", "Description", new Uri("https://www.gazetekapi.com"), "RSSUrl", DateTime.Now)
            {
                Copyright = new TextSyndicationContent($"{DateTime.Now.Year} Gazete Kapı")
            };

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

            using var stream = new MemoryStream();
            using (var xmlWriter = XmlWriter.Create(stream, setting))
            {
                var rssFormator = new Rss20FeedFormatter(feed, false);
                rssFormator.WriteTo(xmlWriter);
                xmlWriter.Flush();
            }
            return File(stream.ToArray(), "application/rss+xml;charset=utf-8");
        }
        public IActionResult ShareButtons()
        {
            return PartialView("ShareButtons");
        }

        public IActionResult HamburgerMenuContent()
        {
            return PartialView("HamburgerMenuContent");
        }

        #endregion

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

        #region Load More With Scroll Page

        [HttpPost]
        public IActionResult _loadNews(string sortOrder, string searchString, int firstItem = 0)
        {
            bool ok = false;
         
            try
            {
                var newListAll = _mapper.Map<List<NewsListItemDto>, List<NewListViewModelWeb>>(_newService.newsListWebInfinityNews());
                List<NewListViewModelWeb> model = null;
                CategoryEditViewModelWeb category = null;
                Random rnd = new Random();

                int CategoryNumber = 0;

                while (ok == false)
                {

                    if (ok == true)
                    {
                        break;
                    }
                    else
                    {
                        //BATCH_SIZE = rnd.Next(1, 5);
                        CategoryNumber = rnd.Next(1, 30);

                        category = _mapper.Map<CategoryDto, CategoryEditViewModelWeb>(_categoryService.GetCategoryById(CategoryNumber));

                        ViewBag.Category = category.CategoryName;

                        if (category.CategoryName != null && category.Id > 0)
                        {
                            model = newListAll.Where(x => x.CategoryId == CategoryNumber).Skip(firstItem).Take(BATCH_SIZE).ToList();

                            ok = true;
                        }

                        else
                        {
                            ok = false;
                            continue;
                        }
                    }
                }

                return PartialView(model);

            }

            catch (Exception ex)
            {
                ok = false;
                return View();
            }
         
        }

        #endregion

        #region Abonelik

        public IActionResult aboneol()
        {
            MetaViewModel meta = new MetaViewModel();
            meta.Title = "Gazetekapı";
            meta.Keywords = "Gazetekapı, Lemonde, Yaşam, Kültür, Sanat, Seçim, Ekonomi, Siyaset";
            meta.Description = "Gazetekapı yeni haberciliğiyle yola çıktı! Gazetekapı ile bilim teknoloji, yaşam, siyaset, ekonomiye dair bütün haberleri sizlerle buluşturacağız!";
            meta.Image = "https://uploads.gazetekapi.com/images/placeholder/kapi-logo.png";
            meta.ogDescription = "Gazetekapı yeni haberciliğiyle yola çıktı! Gazetekapı ile bilim teknoloji, yaşam, siyaset, ekonomiye dair bütün haberleri sizlerle buluşturacağız!";
            meta.ogTitle = "Gazetekapı.com";
            meta.ogImage = "https://uploads.gazetekapi.com/images/placeholder/kapi-logo.png";
            meta.Url = "https://www.gazetekapi.com/";
            ViewBag.Meta = meta;

            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> aboneol(MembersCreateViewModel model)
        {
            //var rep = _repService.RecVer(model.ReCaptchaToken);
            string messageForm = "";

            //if (!rep.Result.success && rep.Result.score < 0.5)
            //{
            //    ViewBag.Hata = "Siz gerçek kullanıcı değilsiniz!";
            //    return View(aboneol());
            //}
            //else
            //{
                var message = new EmailConfig()
                {
                    to = "editor@gazetekapi.com",
                    subject = "Le Monde Diplomatique Abonelik Başvurusu " + DateTime.Now.ToString("dd MMMM yyyy | hh:mm"),
                    phoneNumber = model.phoneNumber,
                    emailAdress = model.emailAdress,
                    nameSurname = model.nameSurname,
                    description = model.description,
                    content = $@"<p>{model.nameSurname}, abonelik başvurusu yaptı. (Bu form https://www.gazetekapi.org.tr/anasayfa/aboneol sayfası üzerinden gelmiştir.) </p> </hr><p><strong>Email Adresi:</strong> {model.emailAdress}</p> </hr> <p><strong>Telefon No:</strong> {model.phoneNumber}</p> <hr/> <p><strong>Mesaj:</strong></p> <p>{model.content}</p>",
                };

                messageForm = await _contactService.SendFormToSubscribe(message); 
            //}

            ViewBag.Message = messageForm;
            return View(aboneol());
        }

        #endregion

        #region Extends Method

        public void LoadJsonData()
        {
            var listNewsJsonData = _mapper.Map<List<NewsListItemDto>, List<NewListViewModelWeb>>(_newService.newsListJsonData());
            string data = JsonConvert.SerializeObject(listNewsJsonData);
            ViewData["data"] = data;
        }

        public async void TimerControl()
        {
            DateTime timeNow = DateTime.Now;
            var haberlist = _mapper.Map<List<NewsListItemDto>, List<NewListViewModelWeb>>(_newService.newsListByDatetimeBigNow());
           
            foreach (var item in haberlist)
            {
                var news = _mapper.Map<NewsDto, NewsEditViewModelWeb>(_newService.getNews(item.Id));
                if(news.PublishedTime == timeNow)
                {
                    news.IsActive = true;
                    int result = Convert.ToInt32(await _newService.editNews(_mapper.Map<NewsEditViewModelWeb, NewsDto>(news)));
                }
            }

        }


        #endregion

    }
}
