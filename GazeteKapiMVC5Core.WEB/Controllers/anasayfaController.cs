using AutoMapper;
using CORE.ApplicationCommon.DTOS.CategoryDTO;
using CORE.ApplicationCommon.DTOS.NewsDTO;
using CORE.ApplicationCommon.DTOS.NewsDTO.GuestDTO;
using CORE.ApplicationCommon.DTOS.NewsDTO.TagNewsDTO;
using CORE.ApplicationCommon.DTOS.PrivacyDTO.PolicyDto;
using CORE.ApplicationCommon.DTOS.PrivacyDTO.PrivacyDto;
using CORE.ApplicationCommon.DTOS.PrivacyDTO.TermsOfUsDto;
using CORE.ApplicationCommon.Helpers;
using GazeteKapiMVC5Core.WEB.Models.ConfigreCaptcha;
using GazeteKapiMVC5Core.WEB.Models.MetaConfig;
using GazeteKapiMVC5Core.WEB.Models.RenderService;
using GazeteKapiMVC5Core.WEB.ViewModels.Categories;
using GazeteKapiMVC5Core.WEB.ViewModels.Guests;
using GazeteKapiMVC5Core.WEB.ViewModels.Members;
using GazeteKapiMVC5Core.WEB.ViewModels.News;
using GazeteKapiMVC5Core.WEB.ViewModels.Policy;
using GazeteKapiMVC5Core.WEB.ViewModels.TagsNews;
using Microsoft.AspNetCore.Mvc;
using SERVICE.Engine.Interfaces;
using SERVICES.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Syndication;
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
        private readonly reCaptchaService _repService;
        private int BATCH_SIZE = 1;

        [Obsolete]
        public anasayfaController(INewsService newService, ICategoryService categoryService, IMapper mapper, ISettingService settingService, IViewRenderService viewRender, reCaptchaService repService)
        {
            _newService = newService;
            _categoryService = categoryService;
            _mapper = mapper;
            _settingService = settingService;
            _viewRender = viewRender;
            _repService = repService;
        }

        #endregion

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]

        #region Pages
        public IActionResult sayfa()
        {

            #region Data

            var haberlist = _mapper.Map<List<NewsListItemDto>, List<NewListViewModelWeb>>(_newService.newsListWithWeb());

            List<GuestListViewModelWeb> guestList = _mapper.Map<List<GuestListItemDto>, List<GuestListViewModelWeb>>(_newService.guestList());

            List<TagNewsListViewModelWeb> tagNewList = _mapper.Map<List<TagNewsListItemDto>, List<TagNewsListViewModelWeb>>(_newService.tagsListWithNewsWeb());

            List<CategoryListViewModelWeb> categoryList = _mapper.Map<List<CategoryListItemDto>, List<CategoryListViewModelWeb>>(_categoryService.GetAllCategory());

            ViewBag.CategoryList = categoryList;
            ViewBag.TagNews = tagNewList;
            ViewBag.HaberlerManset = haberlist;
            ViewBag.GuestList = guestList;

            #endregion

            #region Meta

            MetaViewModel meta = new MetaViewModel();
            meta.Title = "Gazetekapı | Gündem, Bilim, Yaşam, Kültür / Sanat, Foto-Galeri, Video-Haber";
            meta.Keywords = "Gazetekapı, Lemonde, Yaşam, Kültür, Sanat, Foto, Video, Türkiye, 2022, 2023, Seçim, Ekonomi, Siyaset, Futbol, Bilim, Teknoloji, Dünya, ABD, Rusya, Avrupa";
            meta.Description = "Gazetekapı yeni bir habercilik anlayışıyla gerçekleri anlatacak!";
            meta.Image = "https://uploads.gazetekapi.com/images/placeholder/kapi-logo.png";
            meta.ogDescription = "Gazetekapı yeni bir habercilik anlayışıyla gerçekleri anlatacak!";
            meta.ogTitle = "Gazetekapı | Gündem, Bilim, Yaşam, Kültür / Sanat, Foto-Galeri, Video-Haber";
            meta.ogImage = "https://uploads.gazetekapi.com/images/placeholder/kapi-logo.png";
            meta.Url = "https://www.gazetekapi.com/";
            ViewBag.Meta = meta;

            #endregion

            return View();
        }
        public IActionResult aramasonucu(int? pageNumber, string searchnews, int? TagId)
        {
            int pageSize = 20;
            List<NewListViewModelWeb> haberlist = null;
            List<TagNewsListViewModelWeb> tagNewList = null;
            List<CategoryListViewModelWeb> categoryList = null;
            List<NewListViewModelWeb> modelNew = new List<NewListViewModelWeb>();
            MetaViewModel meta = new MetaViewModel();

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

                ViewBag.Categories = categoryList;

                NewListViewModelWeb[] tagsList = modelNew.GroupBy(o => new { o.Title }).Select(x => x.FirstOrDefault()).ToArray();

                #region Meta

                meta.Title = searchnews;
                meta.Keywords = "Gazetekapı, Lemonde, Yaşam, Kültür, Sanat, Foto, Video, Türkiye, 2022, 2023, Seçim, Ekonomi, Siyaset, Futbol, Bilim, Teknoloji, Dünya, ABD, Rusya, Avrupa";
                meta.Description = "Gazetekapı yeni bir habercilik anlayışıyla gerçekleri anlatacak!";
                meta.Image = "https://uploads.gazetekapi.com/images/placeholder/kapi-logo.png";
                meta.ogDescription = "Gazetekapı yeni bir habercilik anlayışıyla gerçekleri anlatacak!";
                meta.ogTitle = searchnews;
                meta.ogImage = "https://uploads.gazetekapi.com/images/placeholder/kapi-logo.png";
                meta.Url = "https://www.gazetekapi.com/anasayfa/aramasonucu";
                ViewBag.Meta = meta;

                #endregion

                return View(PaginationList<NewListViewModelWeb>.Create(tagsList.ToList(), pageNumber ?? 1, pageSize));
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

                ViewBag.Categories = categoryList;

                NewListViewModelWeb[] tagsList = modelNew.GroupBy(o => new { o.Title }).Select(x => x.FirstOrDefault()).ToArray();

                #region Meta

                meta.Title = searchnews;
                meta.Keywords = "Gazetekapı, Lemonde, Yaşam, Kültür, Sanat, Foto, Video, Türkiye, 2022, 2023, Seçim, Ekonomi, Siyaset, Futbol, Bilim, Teknoloji, Dünya, ABD, Rusya, Avrupa";
                meta.Description = "Gazetekapı yeni bir habercilik anlayışıyla gerçekleri anlatacak!";
                meta.Image = "https://uploads.gazetekapi.com/images/placeholder/kapi-logo.png";
                meta.ogDescription = "Gazetekapı yeni bir habercilik anlayışıyla gerçekleri anlatacak!";
                meta.ogTitle = searchnews;
                meta.ogImage = "https://uploads.gazetekapi.com/images/placeholder/kapi-logo.png";
                meta.Url = "https://www.gazetekapi.com/anasayfa/aramasonucu";
                ViewBag.Meta = meta;

                #endregion

                return View(PaginationList<NewListViewModelWeb>.Create(tagsList.ToList(), pageNumber ?? 1, pageSize));
            }

            #region Meta

            meta.Title = searchnews;
            meta.Keywords = "Gazetekapı, Lemonde, Yaşam, Kültür, Sanat, Foto, Video, Türkiye, 2022, 2023, Seçim, Ekonomi, Siyaset, Futbol, Bilim, Teknoloji, Dünya, ABD, Rusya, Avrupa";
            meta.Description = "Gazetekapı yeni bir habercilik anlayışıyla gerçekleri anlatacak!";
            meta.Image = "https://uploads.gazetekapi.com/images/placeholder/kapi-logo.png";
            meta.ogDescription = "Gazetekapı yeni bir habercilik anlayışıyla gerçekleri anlatacak!";
            meta.ogTitle = searchnews;
            meta.ogImage = "https://uploads.gazetekapi.com/images/placeholder/kapi-logo.png";
            meta.Url = "https://www.gazetekapi.com/anasayfa/aramasonucu";
            ViewBag.Meta = meta;

            #endregion

            return View();
        }
        public IActionResult kategori(int? pageNumber, int Id)
        {
            var category = _mapper.Map<CategoryDto, CategoryEditViewModelWeb>(_categoryService.GetCategoryById(Id));

            TempData["kategori"] = category.CategoryName;
            ViewData["id"] = category.Id;

            #region Meta

            MetaViewModel meta = new MetaViewModel();
            meta.Title = category.CategoryName;
            meta.Keywords = "Gazetekapı, Lemonde, Yaşam, Kültür, Sanat, Foto, Video, Türkiye, 2022, 2023, Seçim, Ekonomi, Siyaset, Futbol, Bilim, Teknoloji, Dünya, ABD, Rusya, Avrupa";
            meta.Description = category.Description;
            meta.Image = "https://uploads.gazetekapi.com/images/placeholder/kapi-logo.png";
            meta.ogDescription = category.Description;
            meta.ogTitle = category.CategoryName;
            meta.ogImage = "https://uploads.gazetekapi.com/images/placeholder/kapi-logo.png";
            meta.Url = "https://www.gazetekapi.com/anasayfa/kategpri/" + category.Id;
            ViewBag.Meta = meta;

            #endregion

            int pageSize = 18;

            List<NewListViewModelWeb> newList = _mapper.Map<List<NewsListItemDto>, List<NewListViewModelWeb>>(_newService.newsListByCategoryId(Id));

            return View(PaginationList<NewListViewModelWeb>.Create(newList.ToList(), pageNumber ?? 1, pageSize));
        }
        public IActionResult yazaryazilari(int id, int? pageNumber)
        {
            int pageSize = 20;
            var guest = _mapper.Map<GuestDto, GuestEditViewModelWeb>(_newService.getGuest(id));
            ViewBag.Guest = guest;
            List<CategoryListViewModelWeb> categoryList = _mapper.Map<List<CategoryListItemDto>, List<CategoryListViewModelWeb>>(_categoryService.GetAllCategory());
            ViewBag.Categories = categoryList;

            List<NewListViewModelWeb> newList = _mapper.Map<List<NewsListItemDto>, List<NewListViewModelWeb>>(_newService.newsListWithGuest(guest.Id));

            #region Meta

            MetaViewModel meta = new MetaViewModel();
            meta.Title = guest.GuestName;
            meta.Keywords = "Gazetekapı, Lemonde, Yaşam, Kültür, Sanat, Foto, Video, Türkiye, 2022, 2023, Seçim, Ekonomi, Siyaset, Futbol, Bilim, Teknoloji, Dünya, ABD, Rusya, Avrupa";
            meta.Description = guest.Biography;
            meta.Image = "https://uploads.gazetekapi.com/images/" + guest.GuestImage;
            meta.ogDescription = guest.Biography;
            meta.ogTitle = guest.GuestName;
            meta.ogImage = "https://uploads.gazetekapi.com/images/" +guest.GuestImage;
            meta.Url = "https://www.gazetekapi.com/anasayfa/yazaryazilari/" + guest.Id;
            ViewBag.Meta = meta;

            #endregion

            return View(PaginationList<NewListViewModelWeb>.Create(newList.ToList(), pageNumber ?? 1, pageSize));
        }
        public IActionResult yazarlar()
        {
            List<GuestListViewModelWeb> guestList = _mapper.Map<List<GuestListItemDto>, List<GuestListViewModelWeb>>(_newService.guestList());

            #region Meta

            MetaViewModel meta = new MetaViewModel();
            meta.Title = "Gazetekapı | Gündem, Bilim, Yaşam, Kültür / Sanat, Foto-Galeri, Video-Haber";
            meta.Keywords = "Gazetekapı, Lemonde, Yaşam, Kültür, Sanat, Foto, Video, Türkiye, 2022, 2023, Seçim, Ekonomi, Siyaset, Futbol, Bilim, Teknoloji, Dünya, ABD, Rusya, Avrupa";
            meta.Description = "Gazetekapı yeni bir habercilik anlayışıyla gerçekleri anlatacak!";
            meta.Image = "https://uploads.gazetekapi.com/images/placeholder/kapi-logo.png";
            meta.ogDescription = "Gazetekapı yeni bir habercilik anlayışıyla gerçekleri anlatacak!";
            meta.ogTitle = "Gazetekapı | Gündem, Bilim, Yaşam, Kültür / Sanat, Foto-Galeri, Video-Haber";
            meta.ogImage = "https://uploads.gazetekapi.com/images/placeholder/kapi-logo.png";
            meta.Url = "https://www.gazetekapi.com/";
            ViewBag.Meta = meta;

            #endregion

            return View(guestList);
        }

        [HttpGet("haber/{Id}/{Title}", Name = "haber")]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult haber(int Id, string Title)
        {
            var newsGet = _mapper.Map<NewsDto, NewsEditViewModelWeb>(_newService.getNews(Id));
            ViewBag.Content = HtmlToPlainText(newsGet.NewsContent);

            var guest = _mapper.Map<GuestDto, GuestEditViewModelWeb>(_newService.getGuest(newsGet.GuestId));
            ViewBag.Guest = guest;

            #region Datas

            string friendlyTitle = Title;
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

            if (!string.Equals(friendlyTitle, Title, StringComparison.Ordinal))
            {
                return this.RedirectToRoutePermanent("haber", new { id = Id, title = friendlyTitle });
            }

            return View(newsGet);
        }
        public IActionResult gizlilikpolitikasi()
        {
            var getPrivacy = _mapper.Map<PrivacyDto, PrivacyBaseViewModel>(_settingService.getPrivacy(1));

            #region Meta

            MetaViewModel meta = new MetaViewModel();
            meta.Title = "Gazetekapı | Gündem, Bilim, Yaşam, Kültür / Sanat, Foto-Galeri, Video-Haber";
            meta.Keywords = "Gazetekapı, Lemonde, Yaşam, Kültür, Sanat, Foto, Video, Türkiye, 2022, 2023, Seçim, Ekonomi, Siyaset, Futbol, Bilim, Teknoloji, Dünya, ABD, Rusya, Avrupa";
            meta.Description = "Gazetekapı yeni bir habercilik anlayışıyla gerçekleri anlatacak!";
            meta.Image = "https://uploads.gazetekapi.com/images/placeholder/kapi-logo.png";
            meta.ogDescription = "Gazetekapı yeni bir habercilik anlayışıyla gerçekleri anlatacak!";
            meta.ogTitle = "Gazetekapı | Gündem, Bilim, Yaşam, Kültür / Sanat, Foto-Galeri, Video-Haber";
            meta.ogImage = "https://uploads.gazetekapi.com/images/placeholder/kapi-logo.png";
            meta.Url = "https://www.gazetekapi.com/";
            ViewBag.Meta = meta;

            #endregion

            return View(getPrivacy);
        }   
        public IActionResult cerezpolitikasi()
        {
            var getCookiesPolicy = _mapper.Map<CookiePolicyDto, CookieBaseViewModel>(_settingService.getCookiePrivacy(1));

            #region Meta

            MetaViewModel meta = new MetaViewModel();
            meta.Title = "Gazetekapı | Gündem, Bilim, Yaşam, Kültür / Sanat, Foto-Galeri, Video-Haber";
            meta.Keywords = "Gazetekapı, Lemonde, Yaşam, Kültür, Sanat, Foto, Video, Türkiye, 2022, 2023, Seçim, Ekonomi, Siyaset, Futbol, Bilim, Teknoloji, Dünya, ABD, Rusya, Avrupa";
            meta.Description = "Gazetekapı yeni bir habercilik anlayışıyla gerçekleri anlatacak!";
            meta.Image = "https://uploads.gazetekapi.com/images/placeholder/kapi-logo.png";
            meta.ogDescription = "Gazetekapı yeni bir habercilik anlayışıyla gerçekleri anlatacak!";
            meta.ogTitle = "Gazetekapı | Gündem, Bilim, Yaşam, Kültür / Sanat, Foto-Galeri, Video-Haber";
            meta.ogImage = "https://uploads.gazetekapi.com/images/placeholder/kapi-logo.png";
            meta.Url = "https://www.gazetekapi.com/";
            ViewBag.Meta = meta;

            #endregion

            return View(getCookiesPolicy);
        }
        public IActionResult kullanimsartlari()
        {
            var getTermsOfUs = _mapper.Map<TermsOfUsDto, TermsOfUsBaseViewModel>(_settingService.getTermsOfUs(1));

            #region Meta

            MetaViewModel meta = new MetaViewModel();
            meta.Title = "Gazetekapı | Gündem, Bilim, Yaşam, Kültür / Sanat, Foto-Galeri, Video-Haber";
            meta.Keywords = "Gazetekapı, Lemonde, Yaşam, Kültür, Sanat, Foto, Video, Türkiye, 2022, 2023, Seçim, Ekonomi, Siyaset, Futbol, Bilim, Teknoloji, Dünya, ABD, Rusya, Avrupa";
            meta.Description = "Gazetekapı yeni bir habercilik anlayışıyla gerçekleri anlatacak!";
            meta.Image = "https://uploads.gazetekapi.com/images/placeholder/kapi-logo.png";
            meta.ogDescription = "Gazetekapı yeni bir habercilik anlayışıyla gerçekleri anlatacak!";
            meta.ogTitle = "Gazetekapı | Gündem, Bilim, Yaşam, Kültür / Sanat, Foto-Galeri, Video-Haber";
            meta.ogImage = "https://uploads.gazetekapi.com/images/placeholder/kapi-logo.png";
            meta.Url = "https://www.gazetekapi.com/";
            ViewBag.Meta = meta;

            #endregion

            return View(getTermsOfUs);
        }
        public IActionResult kunye()
        {
            var getBrand = _mapper.Map<BrandPolicyDto, BrandBaseViewModel>(_settingService.getBrandPrivacy(1));

            #region Meta

            MetaViewModel meta = new MetaViewModel();
            meta.Title = "Gazetekapı | Gündem, Bilim, Yaşam, Kültür / Sanat, Foto-Galeri, Video-Haber";
            meta.Keywords = "Gazetekapı, Lemonde, Yaşam, Kültür, Sanat, Foto, Video, Türkiye, 2022, 2023, Seçim, Ekonomi, Siyaset, Futbol, Bilim, Teknoloji, Dünya, ABD, Rusya, Avrupa";
            meta.Description = "Gazetekapı yeni bir habercilik anlayışıyla gerçekleri anlatacak!";
            meta.Image = "https://uploads.gazetekapi.com/images/placeholder/kapi-logo.png";
            meta.ogDescription = "Gazetekapı yeni bir habercilik anlayışıyla gerçekleri anlatacak!";
            meta.ogTitle = "Gazetekapı | Gündem, Bilim, Yaşam, Kültür / Sanat, Foto-Galeri, Video-Haber";
            meta.ogImage = "https://uploads.gazetekapi.com/images/placeholder/kapi-logo.png";
            meta.Url = "https://www.gazetekapi.com/";
            ViewBag.Meta = meta;

            #endregion

            return View(getBrand);
        }
        public IActionResult yayinilkeleri()
        {
            var getStreamPolicy = _mapper.Map<StreamPolicyDto, StreamBaseViewModel>(_settingService.getStreamPrivacy(2));

            #region Meta

            MetaViewModel meta = new MetaViewModel();
            meta.Title = "Gazetekapı | Gündem, Bilim, Yaşam, Kültür / Sanat, Foto-Galeri, Video-Haber";
            meta.Keywords = "Gazetekapı, Lemonde, Yaşam, Kültür, Sanat, Foto, Video, Türkiye, 2022, 2023, Seçim, Ekonomi, Siyaset, Futbol, Bilim, Teknoloji, Dünya, ABD, Rusya, Avrupa";
            meta.Description = "Gazetekapı yeni bir habercilik anlayışıyla gerçekleri anlatacak!";
            meta.Image = "https://uploads.gazetekapi.com/images/placeholder/kapi-logo.png";
            meta.ogDescription = "Gazetekapı yeni bir habercilik anlayışıyla gerçekleri anlatacak!";
            meta.ogTitle = "Gazetekapı | Gündem, Bilim, Yaşam, Kültür / Sanat, Foto-Galeri, Video-Haber";
            meta.ogImage = "https://uploads.gazetekapi.com/images/placeholder/kapi-logo.png";
            meta.Url = "https://www.gazetekapi.com/";
            ViewBag.Meta = meta;

            #endregion

            return View(getStreamPolicy);
        }
        public IActionResult arsiv()
        {
            #region Meta

            MetaViewModel meta = new MetaViewModel();
            meta.Title = "Gazetekapı | Gündem, Bilim, Yaşam, Kültür / Sanat, Foto-Galeri, Video-Haber";
            meta.Keywords = "Gazetekapı, Lemonde, Yaşam, Kültür, Sanat, Foto, Video, Türkiye, 2022, 2023, Seçim, Ekonomi, Siyaset, Futbol, Bilim, Teknoloji, Dünya, ABD, Rusya, Avrupa";
            meta.Description = "Gazetekapı yeni bir habercilik anlayışıyla gerçekleri anlatacak!";
            meta.Image = "https://uploads.gazetekapi.com/images/placeholder/kapi-logo.png";
            meta.ogDescription = "Gazetekapı yeni bir habercilik anlayışıyla gerçekleri anlatacak!";
            meta.ogTitle = "Gazetekapı | Gündem, Bilim, Yaşam, Kültür / Sanat, Foto-Galeri, Video-Haber";
            meta.ogImage = "https://uploads.gazetekapi.com/images/placeholder/kapi-logo.png";
            meta.Url = "https://www.gazetekapi.com/";
            ViewBag.Meta = meta;

            #endregion

            return View();
        }

        #endregion

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
            var newListAll = _mapper.Map<List<NewsListItemDto>, List<NewListViewModelWeb>>(_newService.newsListWithWeb());
            List<NewListViewModelWeb> model = null;
            CategoryEditViewModelWeb category = null;
            Random rnd = new Random();
            bool ok = false;
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
                    CategoryNumber = rnd.Next(14, 20);

                    category = _mapper.Map<CategoryDto, CategoryEditViewModelWeb>(_categoryService.GetCategoryById(CategoryNumber));

                    ViewBag.Category = category.CategoryName;

                    if (category.CategoryName != null && category.Id > 0)
                    {
                        model = newListAll.Where(x => x.CategoryId == CategoryNumber).Skip(firstItem).Take(BATCH_SIZE).ToList();
                        ok = true;
                        return PartialView(model);
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

        #endregion

        #region Abonelik

        public IActionResult aboneol()
        {
            MetaViewModel meta = new MetaViewModel();
            meta.Title = "Gazetekapı | Gündem, Bilim, Yaşam, Kültür / Sanat, Foto-Galeri, Video-Haber";
            meta.Keywords = "Gazetekapı, Lemonde, Yaşam, Kültür, Sanat, Foto, Video, Türkiye, 2022, 2023, Seçim, Ekonomi, Siyaset, Futbol, Bilim, Teknoloji, Dünya, ABD, Rusya, Avrupa";
            meta.Description = "Gazetekapı yeni bir habercilik anlayışıyla gerçekleri anlatacak!";
            meta.Image = "https://uploads.gazetekapi.com/images/placeholder/kapi-logo.png";
            meta.ogDescription = "Gazetekapı yeni bir habercilik anlayışıyla gerçekleri anlatacak!";
            meta.ogTitle = "Gazetekapı | Gündem, Bilim, Yaşam, Kültür / Sanat, Foto-Galeri, Video-Haber";
            meta.ogImage = "https://uploads.gazetekapi.com/images/placeholder/kapi-logo.png";
            meta.Url = "https://www.gazetekapi.com/";
            ViewBag.Meta = meta;

            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult aboneol(MembersCreateViewModel model)
        {
            var rep = _repService.RecVer(model.ReCaptchaToken);

            if (!rep.Result.success && rep.Result.score < 0.5)
            {
                ViewBag.Hata = "Siz gerçek kullanıcı değilsiniz!";
                return View(aboneol());
            }

            return View(aboneol());
        }

        #endregion

    }
}
