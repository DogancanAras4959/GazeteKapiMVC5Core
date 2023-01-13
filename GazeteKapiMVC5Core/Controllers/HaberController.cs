using AutoMapper;
using CORE.ApplicationCommon.DTOS.AccountDTO;
using CORE.ApplicationCommon.DTOS.CategoryDTO;
using CORE.ApplicationCommon.DTOS.CategoryDTO.FooterTypeDTO;
using CORE.ApplicationCommon.DTOS.CategoryDTO.StylePageDTO;
using CORE.ApplicationCommon.DTOS.NewsDTO;
using CORE.ApplicationCommon.DTOS.NewsDTO.GuestDTO;
using CORE.ApplicationCommon.DTOS.NewsDTO.MediaDTO;
using CORE.ApplicationCommon.DTOS.NewsDTO.PublishTypeDTO;
using CORE.ApplicationCommon.DTOS.NewsDTO.TagDTO;
using CORE.ApplicationCommon.DTOS.NewsDTO.TagNewsDTO;
using CORE.ApplicationCommon.DTOS.SeoDTO;
using CORE.ApplicationCommon.DTOS.SeoDTO.SeoMetaDto;
using CORE.ApplicationCommon.DTOS.SetingsDTO;
using CORE.ApplicationCommon.Helpers;
using GazeteKapiMVC5Core.Core.Extensions;
using GazeteKapiMVC5Core.Core.Models.ConfigTTS;
using GazeteKapiMVC5Core.Models.Account;
using GazeteKapiMVC5Core.Models.Category;
using GazeteKapiMVC5Core.Models.Category.FooterTypeModel;
using GazeteKapiMVC5Core.Models.Category.StyleTypeModel;
using GazeteKapiMVC5Core.Models.News.GuestModel;
using GazeteKapiMVC5Core.Models.News.MediaModel;
using GazeteKapiMVC5Core.Models.News.NewsModel;
using GazeteKapiMVC5Core.Models.News.PublishTypeModel;
using GazeteKapiMVC5Core.Models.News.TagModel;
using GazeteKapiMVC5Core.Models.News.TagNewsModel;
using GazeteKapiMVC5Core.Models.Seo.SeoMeta;
using GazeteKapiMVC5Core.Models.Seo.SeoScore;
using GazeteKapiMVC5Core.Models.Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SERVICE.Engine.Interfaces;
using SERVICES.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GazeteKapiMVC5Core.Controllers
{
    public class HaberController : Controller
    {

        #region Fields / Constructure

        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly INewsService _newService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUserService _userService;
        private readonly ISettingService _settingService;
        private readonly ISeoService _seoService;
        public HaberController(ICategoryService categoryService, IMapper mapper, INewsService newsService, IWebHostEnvironment webHostEnvironment, IUserService userService, ISettingService settingService, ISeoService seoService)
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _newService = newsService;
            _webHostEnvironment = webHostEnvironment;
            _userService = userService;
            _settingService = settingService;
            _seoService = seoService;
        }

        #endregion

        #region Categories

        [HttpGet]
        [CheckRoleAuthorize]
        public IActionResult Kategoriler()
        {
            try
            {
                var list = _mapper.Map<List<CategoryListItemDto>, List<CategoryListViewModel>>(_categoryService.GetAllCategory());
                return View(list);
            }
            catch (Exception ex)
            {

                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }

        }

        [HttpGet]
        [CheckRoleAuthorize]
        public IActionResult KategoriOlustur()
        {
            try
            {
                var list = _mapper.Map<List<CategoryListItemDto>, List<CategoryListViewModel>>(_categoryService.GetParentCategoryList());
                ViewBag.CategoriesParent = new SelectList(list, "Id", "CategoryName");

                var listStyleType = _mapper.Map<List<StylePageListItemDto>, List<StyleTypeListViewModel>>(_categoryService.GetAllStyleTypes());
                ViewBag.StyleTypes = new SelectList(listStyleType, "Id", "styleName");

                var listFooterType = _mapper.Map<List<FooterTypeListItemDto>, List<FooterTypeListViewModel>>(_categoryService.GetAllFooterTypes());
                ViewBag.FooterTypes = new SelectList(listFooterType, "Id", "TypeName");

                return View(new CategoryCreateViewModel());
            }
            catch (Exception ex)
            {

                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> KategoriOlustur(CategoryCreateViewModel category, IFormFile file)
        {
            try
            {
                AccountEditViewModel yoneticiGetir = SessionExtensionMethod.GetObject<AccountEditViewModel>(HttpContext.Session, "user");
                category.UserId = yoneticiGetir.Id;

                if (ModelState.IsValid)
                {
                    if (!await _categoryService.CategoryIfExists(category.CategoryName))
                    {
                        if (file != null)
                        {

                            category.Image = SaveImageProcess.ImageInsert(file, "Admin");
                        }
                        else
                        {
                            category.Image = "categorydefault.jpg";
                        }

                        if (await _categoryService.CreateCategory(_mapper.Map<CategoryCreateViewModel, CategoryDto>(category)))
                        {
                            return RedirectToAction(nameof(Kategoriler));
                        }
                        else
                        {
                            return RedirectToAction(nameof(Kategoriler));
                        }
                    }
                    else
                    {
                        return RedirectToAction(nameof(Kategoriler));
                    }
                }
                return View(category);
            }
            catch (Exception ex)
            {

                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }

        }

        [CheckRoleAuthorize]
        public IActionResult KategoriSil(int id)
        {
            try
            {
                if (!_categoryService.DeleteCategoryById(id))
                {
                    return RedirectToAction(nameof(Kategoriler));
                }
                else
                {
                    return RedirectToAction(nameof(Kategoriler));
                }
            }
            catch (Exception ex)
            {

                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }

        }

        [HttpGet]
        [CheckRoleAuthorize]
        public IActionResult KategoriDuzenle(int id)
        {
            try
            {
                var categories = _mapper.Map<CategoryDto, CategoryEditViewModel>(_categoryService.GetCategoryById(id));

                var list = _mapper.Map<List<CategoryListItemDto>, List<CategoryListViewModel>>(_categoryService.GetParentCategoryList());

                var listStyleType = _mapper.Map<List<StylePageListItemDto>, List<StyleTypeListViewModel>>(_categoryService.GetAllStyleTypes());
                ViewBag.StyleTypes = new SelectList(listStyleType, "Id", "styleName");

                var listFooterType = _mapper.Map<List<FooterTypeListItemDto>, List<FooterTypeListViewModel>>(_categoryService.GetAllFooterTypes());
                ViewBag.FooterTypes = new SelectList(listFooterType, "Id", "TypeName");

                ViewBag.CategoriesParent = new SelectList(list, "Id", "CategoryName", categories.Id);
                return View(categories);
            }
            catch (Exception ex)
            {
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> KategoriDuzenle(CategoryEditViewModel category, IFormFile file)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    AccountEditViewModel yoneticiGetir = SessionExtensionMethod.GetObject<AccountEditViewModel>(HttpContext.Session, "user");
                    category.UserId = yoneticiGetir.Id;

                    if (file != null)
                    {

                        category.Image = SaveImageProcess.ImageInsert(file, "Admin");
                    }
                    else
                    {
                        category.Image = "categorydefault.jpg";
                    }

                    if (await _categoryService.UpdateCategory(_mapper.Map<CategoryEditViewModel, CategoryDto>(category)))
                    {
                        return RedirectToAction(nameof(Kategoriler));
                    }
                    else
                    {
                        return RedirectToAction(nameof(Kategoriler));
                    }
                }
                return View(category);
            }
            catch (Exception ex)
            {
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }

        }

        [CheckRoleAuthorize]
        public async Task<IActionResult> KategoriDurumDuzenle(int id)
        {
            try
            {
                if (await _categoryService.EditIsActiveById(id))
                {
                    return RedirectToAction(nameof(Kategoriler));
                }
                else
                {
                    return RedirectToAction(nameof(Kategoriler));
                }
            }
            catch (Exception ex)
            {
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }

        }

        [CheckRoleAuthorize]
        public async Task<IActionResult> KategoriMenuYerlestir(int id)
        {
            try
            {
                if (await _categoryService.EditIsMenuById(id))
                {
                    return RedirectToAction(nameof(Kategoriler));
                }
                else
                {
                    return RedirectToAction(nameof(Kategoriler));
                }
            }
            catch (Exception ex)
            {
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }

        }
        #endregion

        #region Yazarlar

        [CheckRoleAuthorize]
        public IActionResult Yazarlar()
        {
            try
            {
                var listGuest = _mapper.Map<List<GuestListItemDto>, List<GuestListViewModel>>(_newService.guestList());

                return View(listGuest);
            }
            catch (Exception ex)
            {
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }
        }

        [CheckRoleAuthorize]
        public IActionResult YazarOlustur()
        {
            try
            {
                return View(new GuestCreateViewModel());
            }
            catch (Exception ex)
            {
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> YazarOlustur(GuestCreateViewModel model, IFormFile file)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    AccountEditViewModel yoneticiGetir = SessionExtensionMethod.GetObject<AccountEditViewModel>(HttpContext.Session, "user");
                    model.UserId = yoneticiGetir.Id;

                    if (file != null)
                    {

                        model.GuestImage = SaveImageProcess.ImageInsert(file, "Admin");
                    }
                    else
                    {
                        model.GuestImage = "user.png";
                    }

                    if (await _newService.createGuets(_mapper.Map<GuestCreateViewModel, GuestDto>(model)))
                    {
                        return RedirectToAction(nameof(Yazarlar));
                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {

                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }
        }

        [CheckRoleAuthorize]
        public IActionResult YazarSil(int Id)
        {
            try
            {
                if (!_newService.guestDelete(Id))
                {
                    return RedirectToAction(nameof(Yazarlar));
                }
                else
                {
                    return RedirectToAction(nameof(Yazarlar));
                }
            }
            catch (Exception ex)
            {
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction(nameof(Yazarlar));
            }
        }

        [CheckRoleAuthorize]
        public async Task<IActionResult> YazarDurumDuzenle(int Id)
        {
            try
            {
                if (await _newService.EditIsActive(Id))
                {
                    return RedirectToAction(nameof(Yazarlar));
                }
                else
                {
                    return RedirectToAction(nameof(Yazarlar));
                }
            }
            catch (Exception ex)
            {

                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }
        }

        [CheckRoleAuthorize]
        public IActionResult YazarDuzenle(int Id)
        {
            try
            {
                var getUser = _mapper.Map<GuestDto, GuestEditViewModel>(_newService.getGuest(Id));
                return View(getUser);
            }
            catch (Exception ex)
            {

                TempData["HataMesaji"] = ex.ToString();

                return RedirectToAction("ErrorPage", "Home");
            }
        }

        [CheckRoleAuthorize]
        public IActionResult YazarinYazilari(int Id, int? pageNumber, int? categoryId, string searchstring, int? userId)
        {
            try
            {
                int pageSize = 20;
                List<NewsLıstItemModel> haberlist = null;

                var guest = _mapper.Map<GuestBaseDto, GuestBaseViewModel>(_newService.getGuest(Id));
                TempData["Name"] = guest.GuestName;

                var categories = _mapper.Map<List<CategoryListItemDto>, List<CategoryListViewModel>>(_categoryService.GetAllCategory());
                ViewBag.Categories = new SelectList(categories.ToList(), "Id", "CategoryName");

                var users = _mapper.Map<List<UserListItemDto>, List<UserListViewModel>>(_userService.GetAllUsers());
                ViewBag.Users = new SelectList(users.ToList(), "Id", "DisplayName");

                if (searchstring != "" && searchstring != null)
                {
                    haberlist = _mapper.Map<List<NewsListItemDto>, List<NewsLıstItemModel>>(_newService.searchDataInNewsWithGuest(searchstring, Id));
                    return View(PaginationList<NewsLıstItemModel>.Create(haberlist.ToList(), pageNumber ?? 1, pageSize));
                }

                if (categoryId != null && categoryId != 0)
                {
                    haberlist = _mapper.Map<List<NewsListItemDto>, List<NewsLıstItemModel>>(_newService.FilterCategoryInNewsWithGuest(categoryId, Id));
                    return View(PaginationList<NewsLıstItemModel>.Create(haberlist.ToList(), pageNumber ?? 1, pageSize));
                }

                if (userId != null && userId != 0)
                {
                    haberlist = _mapper.Map<List<NewsListItemDto>, List<NewsLıstItemModel>>(_newService.FilterUserInNewsWithGuest(userId, Id));
                    return View(PaginationList<NewsLıstItemModel>.Create(haberlist.ToList(), pageNumber ?? 1, pageSize));
                }

                haberlist = _mapper.Map<List<NewsListItemDto>, List<NewsLıstItemModel>>(_newService.newsListWithGuest(Id));
                return View(PaginationList<NewsLıstItemModel>.Create(haberlist.ToList(), pageNumber ?? 1, pageSize));
            }
            catch (Exception ex)
            {

                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> YazarDuzenle(GuestEditViewModel model, IFormFile file)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    AccountEditViewModel yoneticiGetir = SessionExtensionMethod.GetObject<AccountEditViewModel>(HttpContext.Session, "user");
                    model.UserId = yoneticiGetir.Id;

                    if (file != null)
                    {
                        model.GuestImage = SaveImageProcess.ImageInsert(file, "Admin");
                    }
                    else
                    {
                        model.GuestImage = "user.png";
                    }

                    if (await _newService.editGuest(_mapper.Map<GuestEditViewModel, GuestDto>(model)))
                    {
                        return RedirectToAction(nameof(Yazarlar));
                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {

                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }
        }

        [HttpGet]
        [CheckRoleAuthorize]
        public IActionResult YazarDetay(int id)
        {
            try
            {
                var haberlist = _mapper.Map<List<NewsListItemDto>, List<NewsLıstItemModel>>(_newService.newsListWithGuestOneToFive(id));
                ViewBag.Yazilar = haberlist;

                return View(_mapper.Map<GuestDto, GuestEditViewModel>(_newService.getGuest(id)));
            }
            catch (Exception ex)
            {

                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }
        }
        #endregion

        #region Haberler

        //[RoleAuthorize("Haberler")]
        [HttpGet]
        [CheckRoleAuthorize]
        public IActionResult Haberler()
        {
            try
            {
                List<NewsLıstItemModel> haberlist = _mapper.Map<List<NewsListItemDto>, List<NewsLıstItemModel>>(_newService.newsList());
                foreach (var item in haberlist)
                {
                    var tags = _mapper.Map<List<TagNewsListItemDto>, List<TagNewsListViewModel>>(_newService.tagsListWithNewsByNewsId(item.Id));

                    List<string> list = new List<string>();

                    foreach (var item2 in tags)
                    {
                        list.Add(item2.tag.TagName);
                    }

                    string[] tagsList = list.ToArray();

                    for (int i = 0; i < tagsList.Count(); i++)
                    {
                        if (item.Tag != null)
                        {
                            item.Tag = item.Tag + "," + tagsList[i];
                        }
                        else
                        {
                            item.Tag = tagsList[i];
                        }
                    }

                }
                return View(haberlist);
            }
            catch (Exception ex)
            {
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }

        }

        [HttpGet]
        [CheckRoleAuthorize]
        public IActionResult HaberOlustur()
        {
            try
            {
                LoadData();
                return View(new NewsCreateViewModel());
            }
            catch (Exception ex)
            {
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HaberOlustur(NewsCreateViewModel model, IFormFile file)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!await _newService.NewsIfExists(model.Title))
                    {
                        if (file != null && file.Length > 0)
                        {

                            model.Image = SaveImageProcess.ImageInsert(file, "Admin");

                        }
                        else
                        {
                            model.Image = "imgdefault.jpg";
                        }

                        if (model.PublishTypeId == 999)
                        {
                            ViewBag.Hata = "Haber tipi seçimiyle ilgili bir sorun çıktı. Haber tipi seçiniz"!;
                            LoadData();
                            return View(model);
                        }

                        else
                        {
                            if (model.CategoryId != 0)
                            {
                                AccountEditViewModel yoneticiGetir = SessionExtensionMethod.GetObject<AccountEditViewModel>(HttpContext.Session, "user");

                                model.RowNo = 0;
                                model.Sorted = 0;
                                model.UserId = yoneticiGetir.Id;

                            }
                            else
                            {
                                ViewBag.Hata = "Haber için kategori seçilmelidir. Haber bu yüzden oluşturulamadı!"!;
                                LoadData();
                                return View(model);
                            }
                        }

                        int resultId = Convert.ToInt32(await _newService.createNews(_mapper.Map<NewsCreateViewModel, NewsDto>(model)));

                        if (resultId > 0)
                        {

                            if (!string.IsNullOrEmpty(model.Tag))
                            {
                                if (model.Tag[^1] == ',')
                                {
                                    await _newService.InsertTagToProduct(model.Tag[0..^1], resultId);
                                }
                                else
                                {
                                    await _newService.InsertTagToProduct(model.Tag, resultId);
                                }
                            }
                            //if(model.IsActive == true)
                            //await sirayaEkle(resultId);

                            string durum = "Haber başarıyla oluşturuldu. Son kontrollerinizi yapıp yayınlayın!";
                            return RedirectToAction("HaberDuzenle", "Haber", new { Id = resultId, durum = durum });
                        }
                        else
                        {

                            ViewBag.Hata = "Haberiniz oluşturulurken bir hata meydana geldi! Etiketlerinizi kontrol edin"!;
                            LoadData();
                            return View(model);
                        }
                    }
                    else
                    {
                        ViewBag.Hata = "Oluşturmak istediğiniz haber zaten sistemde bulunuyor"!;
                        LoadData();
                        return View(model);
                    }
                }
                else
                {
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                TempData["HataMesajı"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }
        }

        [HttpGet]
        [CheckRoleAuthorize]
        public async Task<IActionResult> HaberDuzenle(int id, string durum = "")
        {
            try
            {

                #region GetTags

                var tags = _mapper.Map<List<TagNewsListItemDto>, List<TagNewsListViewModel>>(_newService.tagsListWithNewsByNewsId(id));
                var news = _mapper.Map<NewsDto, NewsEditViewModel>(_newService.getNews(id));

                List<string> list = new List<string>();

                foreach (var item in tags)
                {
                    list.Add(item.tag.TagName);
                }

                string[] tagsList = list.ToArray();

                for (int i = 0; i < tagsList.Count(); i++)
                {
                    if (news.Tag != null)
                    {
                        news.Tag = news.Tag + "," + tagsList[i];
                    }
                    else
                    {
                        news.Tag = tagsList[i];
                    }
                }

                #endregion

                #region Load Data

                var publishTypeList = _mapper.Map<List<PublishTypeListItem>, List<PublishTypeListViewModel>>(_newService.publishTypeList());
                ViewBag.PublishTypes = new SelectList(publishTypeList, "Id", "TypeName", news.PublishTypeId);

                var categoryList = _mapper.Map<List<CategoryListItemDto>, List<CategoryListViewModel>>(_categoryService.GetAllCategory());
                ViewBag.Categories = new SelectList(categoryList, "Id", "CategoryName", news.CategoryId);

                var guestList = _mapper.Map<List<GuestListItemDto>, List<GuestListViewModel>>(_newService.guestList());
                ViewBag.Guests = new SelectList(guestList, "Id", "GuestName", news.GuestId);

                var newList = _mapper.Map<List<NewsListItemDto>, List<NewsLıstItemModel>>(_newService.newsList());
                ViewBag.News = newList;

                if (durum == "")
                {
                    TempData["durum"] = null;
                }
                else
                {
                    TempData["durum"] = durum;
                }

                #endregion

                await seoCreateOrUpdate(news);

                return View(news);
            }
            catch (Exception ex)
            {
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }

        }

        [HttpPost]
    
        public async Task<IActionResult> HaberDuzenle(NewsEditViewModel model, IFormFile file)
        {

            var news = _mapper.Map<NewsDto, NewsEditViewModel>(_newService.getNews(model.Id));

            if (model.PublishTypeId == 999)
            {
                ViewBag.Hata = "Haber tipi seçimiyle ilgili bir sorun çıktı. Haber tipi seçiniz"!;
                LoadData();
                return RedirectToAction("HaberDuzenle", "Haber", new { Id = news.Id });
            }
           
            else
            {
                if (model.CategoryId != 0)
                {
                    AccountEditViewModel yoneticiGetir = SessionExtensionMethod.GetObject<AccountEditViewModel>(HttpContext.Session, "user");
                    model.UserId = yoneticiGetir.Id;

                    //if (model.Sound == null)
                    //{
                    //    string friendlyUrl = GoogleTTS.GenerateSlug(model.Title, model.Id);
                    //    string content = GoogleTTS.HtmlToPlainTextTTS(model.NewsContent);
                    //    string outputSound = GoogleTTS.Speak(model.Spot, model.Title, friendlyUrl, content, "Admin");

                    //    model.Sound = outputSound;
                    //}

                    if (file != null)
                    {

                        model.Image = SaveImageProcess.ImageInsert(file, "Admin");

                        int resultId = Convert.ToInt32(await _newService.editNews(_mapper.Map<NewsEditViewModel, NewsDto>(model)));

                        if (resultId > 0)
                        {

                            if (!string.IsNullOrEmpty(model.Tag))
                            {
                                if (model.Tag[^1] == ',')
                                {
                                    await _newService.InsertTagToProduct(model.Tag[0..^1], resultId);
                                }
                                else
                                {
                                    await _newService.InsertTagToProduct(model.Tag, resultId);
                                }
                            }
                            return RedirectToAction("HaberDuzenle", "Haber", new { Id = news.Id });
                        }
                    }

                    else
                    {

                        int resultId = Convert.ToInt32(await _newService.editNews(_mapper.Map<NewsEditViewModel, NewsDto>(model)));

                        if (resultId > 0)
                        {

                            if (!string.IsNullOrEmpty(model.Tag))
                            {
                                if (model.Tag[^1] == ',')
                                {
                                    await _newService.InsertTagToProduct(model.Tag[0..^1], resultId);
                                }
                                else
                                {
                                    await _newService.InsertTagToProduct(model.Tag, resultId);
                                }
                            }
                            return RedirectToAction("HaberDuzenle", "Haber", new { Id = news.Id });

                        }
                    }
                }
            }

            LoadData();
            return RedirectToAction("HaberDuzenle", "Haber", new { Id = news.Id });

        }

        public async Task<IActionResult> HaberCogalt(int Id)
        {
            try
            {

                var news = _mapper.Map<NewsDto, NewsEditViewModel>(_newService.getNews(Id));

                AccountEditViewModel yoneticiGetir = SessionExtensionMethod.GetObject<AccountEditViewModel>(HttpContext.Session, "user");
                news.UserId = yoneticiGetir.Id;

                NewsCreateViewModel model = new NewsCreateViewModel();
                model.CategoryId = news.CategoryId;
                model.ColNo = news.ColNo;
                model.GuestId = news.GuestId;
                model.IsActive = news.IsActive;
                model.IsCommentActive = news.IsCommentActive;
                model.IsLock = news.IsLock;
                model.IsOpenNotifications = news.IsOpenNotifications;
                model.IsSlide = news.IsSlide;
                model.IsTitle = news.IsTitle;
                model.Title = news.Title;
                model.Spot = news.Spot;
                model.NewsContent = news.NewsContent;
                model.MetaTitle = news.MetaTitle;
                model.Image = news.Image;
                model.ParentNewsId = news.ParentNewsId;
                model.PublishedTime = news.PublishedTime;
                model.PublishTypeId = news.PublishTypeId;
                model.Sound = news.Sound;
                model.Sorted = news.Sorted;
                model.Tag = news.Tag;
                model.VideoSlug = news.VideoSlug;
                model.Views = news.Views;
                model.RowNo = news.RowNo;
                model.UserId = news.UserId;

                int resultId = Convert.ToInt32(await _newService.createNews(_mapper.Map<NewsCreateViewModel, NewsDto>(model)));

                LoadData();
                return RedirectToAction("Haberler", "Haber");

            }
            catch (Exception ex)
            {
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }
        }

        public IActionResult HaberSirala()
        {
            List<NewsLıstItemModel> haberlist = _mapper.Map<List<NewsListItemDto>, List<NewsLıstItemModel>>(_newService.newsListOrderRow());
            return View(haberlist);
        }

        [HttpPost]
        public async Task<IActionResult> siraDegistir(int id, int sira)
        {
            if (await _newService.ChangeSorted(id, sira))
            {
                List<NewsLıstItemModel> haberlist = _mapper.Map<List<NewsListItemDto>, List<NewsLıstItemModel>>(_newService.newsList());

                foreach (var item in haberlist.Where(x => x.Sorted >= sira).OrderBy(x => x.Sorted).Take(10))
                {
                    item.Sorted = item.Sorted + 1;
                    await _newService.ChangeSorted(item.Id, item.Sorted);
                }

                //await CreateModeratorLog("Başarılı", "Güncelleme", "siraDegistir", "Haber", "Haber başarıyla sıralandı!");
                return RedirectToAction("HaberSirala", "Haber");
            }
            return RedirectToAction("HaberSirala", "Haber");
        }

        [CheckRoleAuthorize]
        public IActionResult HaberSil(int id)
        {
            try
            {
                if (_newService.newsDelete(id))
                {
                    return RedirectToAction("Haberler", "Haber");
                }
                else
                {
                    return RedirectToAction("Haberler", "Haber");
                }
            }
            catch (Exception ex)
            {
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }
        }

        [CheckRoleAuthorize]
        public async Task<IActionResult> HaberiKilitle(int id)
        {
            try
            {
                if (await _newService.IsLockNews(id))
                {
                    return RedirectToAction(nameof(Haberler));
                }
                return View(nameof(Haberler));
            }
            catch (Exception ex)
            {
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }
        }

        [CheckRoleAuthorize]
        public async Task<IActionResult> BildirimleriAc(int id)
        {
            try
            {
                if (await _newService.IsOpenNotificationSet(id))
                {
                    return RedirectToAction(nameof(Haberler));
                }
                return View(nameof(Haberler));
            }
            catch (Exception ex)
            {
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }
        }

        [CheckRoleAuthorize]
        public async Task<IActionResult> ikiliyeyerlestir(int id)
        {
            try
            {
                if (await _newService.placeDoubleHolder(id))
                {
                    return RedirectToAction(nameof(Haberler));
                }
                return View(nameof(Haberler));
            }
            catch (Exception ex)
            {
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }
        }

        [CheckRoleAuthorize]
        public async Task<IActionResult> dortluyeyerlestir(int id)
        {
            try
            {
                if (await _newService.placeFourthHolder(id))
                {
                    return RedirectToAction(nameof(Haberler));
                }
                return View(nameof(Haberler));
            }
            catch (Exception ex)
            {
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }
        }

        [CheckRoleAuthorize]
        public async Task<IActionResult> haberiarsivle(int id)
        {
            try
            {
                if (await _newService.setArchiveNews(id))
                {
                    return RedirectToAction(nameof(Haberler));
                }
                return View(nameof(Haberler));
            }
            catch (Exception ex)
            {
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }
        }

        [CheckRoleAuthorize]
        public async Task<IActionResult> HaberiManseteTasi(int id)
        {
            try
            {
                if (await _newService.SetYourNewsToUp(id))
                {
                    return RedirectToAction(nameof(Haberler));
                }
                return View(nameof(Haberler));
            }
            catch (Exception ex)
            {
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }
        }

        public async Task<IActionResult> HaberiAktifEt(int id)
        {
            try
            {
                if (await _newService.IsActiveEnabled(id))
                {
                    return RedirectToAction(nameof(Haberler));
                }
                return View(nameof(Haberler));
            }
            catch (Exception ex)
            {
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }
        }

        [HttpGet]
        [CheckRoleAuthorize]
        public IActionResult Etiketler()
        {
            try
            {
                var etiketlerHaberler = _mapper.Map<List<TagNewsListItemDto>, List<TagNewsListViewModel>>(_newService.tagsListWithNews());
                ViewBag.EtiketHaberler = etiketlerHaberler;

                List<TagListViewModel> tags = _mapper.Map<List<TagListItemDto>, List<TagListViewModel>>(_newService.tagList());
                return View(tags);

            }
            catch (Exception ex)
            {
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }
        }

        [CheckRoleAuthorize]
        public IActionResult EtiketeGoreHaberler(int Id, int? pageNumber)
        {
            try
            {
                int pageSize = 20;
                var etiket = _mapper.Map<TagBaseDto, TagEditViewModel>(_newService.tagGet(Id));
                ViewBag.Tag = etiket;

                var etiketlerHaberler = _mapper.Map<List<TagNewsListItemDto>, List<TagNewsListViewModel>>(_newService.tagsListWithNewsById(Id));
                return View(PaginationList<TagNewsListViewModel>.Create(etiketlerHaberler.ToList(), pageNumber ?? 1, pageSize));
            }
            catch (Exception ex)
            {
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }
        }

        [CheckRoleAuthorize]
        public IActionResult EtiketSil(int id)
        {
            try
            {
                if (_newService.tagDelete(id))
                {
                    return RedirectToAction(nameof(Etiketler));
                }
                else
                {
                    return RedirectToAction(nameof(Etiketler));
                }
            }
            catch (Exception ex)
            {
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }
        }

        [HttpPost]
        public IActionResult UploadImages(IList<IFormFile> files)
        {
            var filePath = "";
            foreach (IFormFile photo in Request.Form.Files)
            {
                filePath = SaveImageProcess.ImageInsert(photo, "Admin");
            }
            return Json(new { url = "https://uploads.gazetekpai.com/images/" + filePath });
        }

        [Route("upload_editor")]
        [HttpPost]
        public IActionResult UploadEditor(IFormFile file)
        {
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + file.FileName;
            var path = Path.Combine(Directory.GetCurrentDirectory(), _webHostEnvironment.WebRootPath,"upload");
            var stream = new FileStream(path, FileMode.Create);
            file.CopyToAsync(stream);
            return new JsonResult(new { parh = "/upload" + fileName });
        }

        [HttpPost]
        public async Task HaberIliskilendir(string haberlist, int haberId)
        {
            string[] array = haberlist.Split(',');
            foreach (var item in array)
            {
                int CurrentId = Convert.ToInt32(item);
                await _newService.editParentId(haberId, CurrentId);
            }
        }

        public async Task<IActionResult> IliskiliHaberiKaldir(int id, int newsId)
        {
            try
            {
                await _newService.dropParentRelation(id);
                return RedirectToAction("HaberDuzenle", "Haber", new { id = newsId });
            }
            catch (Exception ex)
            {
                return RedirectToAction("HaberDuzenle", "Haber", new { id = newsId });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SiralamayiGuncelle(string itemIds)
        {
            int count = 1;
            List<int> itemIdList = new List<int>();
            itemIdList = itemIds.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

            foreach (var itemId in itemIdList)
            {
                try
                {
                    await _newService.changeSortedItem(itemId, count);
                }
                catch (Exception ex)
                {
                    continue;
                }
                count++;
            }
            return Json(true);
        }
        public async Task<IActionResult> siradanCikar(int Id)
        {
            try
            {
                int resultRow = await _newService.updateSliderRow(Id);

                if (resultRow > 0)
                {
                    List<NewsLıstItemModel> listNews = _mapper.Map<List<NewsListItemDto>,List<NewsLıstItemModel>>(_newService.newsListBySortedOrder(resultRow));

                    int lastIndex = listNews.Select(x=> x.RowNo).Max() + 1;

                    foreach (var item in listNews)
                    {
                        int topCount = 0;

                        if (item.RowNo > 1)
                        {
                            topCount = item.RowNo - 1;
                            int resultRowItem = await _newService.updateAllSliderItemRow(item.Id, topCount);
                        }

                        else
                        {
                            if (item.RowNo == 0 && item.Id != Id)
                            {
                                topCount = lastIndex;
                                int resultRowItem = await _newService.updateAllSliderItemRow(item.Id, topCount);
                            }

                            else if (item.RowNo == 1 && item.Id != Id)
                            {
                                topCount = 1;
                                int resultRowItem = await _newService.updateAllSliderItemRow(item.Id, topCount);
                            }

                            else
                            { }
                        }

                    }
                }

                return RedirectToAction("HaberSirala", "Haber");
            }
            catch (Exception ex)
            {
                return RedirectToAction("HaberSirala", "Haber");
            }

        }
        public async Task<IActionResult> sirayaEkle(int Id)
        {
            try
            {
                int resultRow = await _newService.updateSliderRowInsert(Id);

                if (resultRow > 0)
                {
                    List<NewsLıstItemModel> listNews = _mapper.Map<List<NewsListItemDto>,List<NewsLıstItemModel>>(_newService.newsList());

                    int lastIndex = listNews.Select(x=> x.RowNo).Max() + 1;

                    foreach (var item in listNews)
                    {
                        int topCount = 0;

                        if (item.RowNo > 1)
                        {
                            topCount = item.RowNo + 1;
                            int resultRowItem = await _newService.updateAllSliderItemRowInsert(item.Id, topCount);
                        }
                        else
                        {
                            if (item.RowNo == 0 && item.Id != Id)
                            {
                                topCount = lastIndex;
                                int resultRowItem = await _newService.updateAllSliderItemRowInsert(item.Id, topCount);
                            }

                            else if (item.RowNo == 1 && item.Id != Id)
                            {
                                topCount = 2;
                                int resultRowItem = await _newService.updateAllSliderItemRowInsert(item.Id, topCount);
                            }
                            else
                            { }
                        }
                    }
                }

                return RedirectToAction("HaberSirala", "Haber");
            }
            catch (Exception ex)
            {
                return RedirectToAction("HaberSirala", "Haber");
            }
        }
        public IActionResult HaberYerlestirme()
        {
            var categories = _mapper.Map<List<CategoryListItemDto>, List<CategoryListViewModel>>(_categoryService.GetAllCategory());

            var news = _mapper.Map<List<NewsListItemDto>, List<NewsLıstItemModel>>(_newService.newsList());
            ViewBag.NewsList = news;

            return View(categories);
        }
        [HttpPost]
        public async Task<IActionResult> KategoriyiDegistir(string itemsId, string categoriesId)
        {
            int newsId = Convert.ToInt32(itemsId);
            var getNews = _mapper.Map<NewsDto, NewsEditViewModel>(_newService.getNews(newsId));

            int categoryId = Convert.ToInt32(categoriesId);
            getNews.CategoryId = categoryId;

            int result = Convert.ToInt32(await _newService.editNews(_mapper.Map<NewsEditViewModel, NewsDto>(getNews)));

            if (result != 0)
            {
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }

        #endregion

        #region OrtamMedyası

        public IActionResult OrtamMedyasi(int? pagenumber)
        {
            int pageSize = 50;
            List<MediaListViewModel> mediaList = null;

            mediaList = _mapper.Map<List<MediaListItemDto>, List<MediaListViewModel>>(_newService.mediaList());
            return View(PaginationList<MediaListViewModel>.Create(mediaList.ToList(), pagenumber ?? 1, pageSize));

        }

        public async Task<IActionResult> MedyaEkle(IFormFile fileupload)
        {
            if (fileupload != null)
            {
                AccountEditViewModel yoneticiGetir = SessionExtensionMethod.GetObject<AccountEditViewModel>(HttpContext.Session, "user");

                MediaCreateViewModel model = new MediaCreateViewModel
                {
                    UserId = yoneticiGetir.Id,
                    Slug = SaveImageProcess.VideoInsert(fileupload, "Videos"),
                    Title = fileupload.FileName,
                };

                int resultId = Convert.ToInt32(await _newService.insertMedia(_mapper.Map<MediaCreateViewModel, MediaDto>(model)));

            }
            return RedirectToAction("OrtamMedyasi", "Haber");
        }

        [HttpPost]
        public async Task<JsonResult> uploadVideoForNews(IFormFile file, int haberId)
        {
            try
            {
                if (file != null)
                {
                    AccountEditViewModel yoneticiGetir = SessionExtensionMethod.GetObject<AccountEditViewModel>(HttpContext.Session, "user");

                    MediaCreateViewModel model = new MediaCreateViewModel
                    {
                        UserId = yoneticiGetir.Id,
                        Slug = SaveImageProcess.VideoInsert(file, "Videos"),
                        Title = file.FileName,
                    };

                    int resultId = Convert.ToInt32(await _newService.insertMedia(_mapper.Map<MediaCreateViewModel, MediaDto>(model)));

                    if (resultId > 0)
                    {
                        var video = _mapper.Map<MediaDto, MediaEditViewModel>(_newService.getMedia(resultId));

                        if (await _newService.insertVideoToNews(haberId, video.Slug))
                        {
                            return Json(true);
                        }
                        else
                        {
                            return Json(false);
                        }

                    }
                    else
                    {
                        return Json(false);
                    }
                }
                else
                {
                    return Json(false);
                }
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> uploadImageForNews(IFormFile file)
        {
            try
            {
                if (file != null)
                {
                    AccountEditViewModel yoneticiGetir = SessionExtensionMethod.GetObject<AccountEditViewModel>(HttpContext.Session, "user");

                    MediaCreateViewModel model = new MediaCreateViewModel
                    {
                        UserId = yoneticiGetir.Id,
                        Slug = SaveImageProcess.ImageInsert(file, "Admin"),
                        Title = file.FileName,
                    };

                    int resultId = Convert.ToInt32(await _newService.insertMedia(_mapper.Map<MediaCreateViewModel, MediaDto>(model)));
                    string url = "https://uploads.gazetekapi.com/images/"+model.Slug;
                    return Json(url);

                }
                else
                {
                    return Json(false);
                }
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }

        [HttpGet]
        public async Task<IActionResult> deleteVideoFromNews(int Id)
        {
            if (await _newService.deleteVideoFromNews(Id))
            {
                return RedirectToAction("HaberDuzenle", "Haber", new { Id = Id });
            }
            else
            {
                return RedirectToAction("HaberDuzenle", "Haber", new { Id = Id });
            }
        }

        #endregion

        #region Extend Methods

        public void LoadData()
        {
            var publishTypeList = _mapper.Map<List<PublishTypeListItem>, List<PublishTypeListViewModel>>(_newService.publishTypeList());
            ViewBag.PublishTypes = new SelectList(publishTypeList, "Id", "TypeName");

            var categoryList = _mapper.Map<List<CategoryListItemDto>, List<CategoryListViewModel>>(_categoryService.GetAllCategory());
            ViewBag.Categories = new SelectList(categoryList, "Id", "CategoryName");

            var guestList = _mapper.Map<List<GuestListItemDto>, List<GuestListViewModel>>(_newService.guestList());
            ViewBag.Guests = new SelectList(guestList, "Id", "GuestName");
        }

        #endregion

        #region Seo Methods
        private async Task seoCreateOrUpdate(NewsEditViewModel news)
        {
            #region SEO Create or Updates

            var getSeoIfExists = _mapper.Map<SeoScoreDto, SeoScoreBaseViewModel>(_seoService.GetSeoScoreByNewsId(news.Id));

            if (getSeoIfExists == null)
            {

                SeoScoreCreateViewModel newModel = new SeoScoreCreateViewModel();
                string code = RandomStringForUniqueCode(20);
                newModel.NewsId = news.Id;

                int resultId = Convert.ToInt32(await _seoService.CreateSeoScore(_mapper.Map<SeoScoreCreateViewModel, SeoScoreDto>(newModel), code));

                if (resultId > 0 && resultId != -1)
                {
                    var getSeo = _mapper.Map<SeoScoreDto, SeoScoreBaseViewModel>(_seoService.GetSeoScoreByNewsId(news.Id));

                    #region Seo Meta Create

                    await _seoService.CreateSeoMetaToSeoScore(getSeo.Id);
                    //_seoService.UpdateSeoScoreAfterCreateTask(getSeo.Id);
                    // Bu mesele çok önemli. IsCreated false'a çevrilmeli
                    #endregion

                    LevelAnalyze(getSeo.Id);
                    ViewBag.SeoScore = getSeo;

                    #region Seo Meta Create

                    var listSeoMetas = _mapper.Map<List<SeoMetaListItemDto>, List<SeoMetaListViewModel>>(_seoService.listSeoMetasBySeoScoreId(getSeo.Id));

                    if (listSeoMetas.Count == 0 && listSeoMetas.Where(x => x.IsDone == true).ToList().Count == 0)
                    {
                        if (getSeoIfExists.IsFinished == false && getSeoIfExists.IsCreated == true)
                        {
                            await _seoService.CreateSeoMetaToSeoScore(getSeoIfExists.Id);
                            //_seoService.UpdateSeoScoreAfterCreateTask(getSeoIfExists.Id);
                            // Bu mesele çok önemli. IsCreated false'a çevrilmeli
                        }
                    }
                    else
                    {
                        ViewBag.listSeoMeta = listSeoMetas;
                    }

                    #endregion

                }
            }

            else
            {

                #region Seo Meta Create

                var listSeoMetas = _mapper.Map<List<SeoMetaListItemDto>, List<SeoMetaListViewModel>>(_seoService.listSeoMetasBySeoScoreId(getSeoIfExists.Id));

                if (listSeoMetas.Count == 0 && listSeoMetas.Where(x => x.IsDone == true).ToList().Count == 0)
                {
                    if (getSeoIfExists.IsFinished == false && getSeoIfExists.IsCreated == true)
                    {
                        await _seoService.CreateSeoMetaToSeoScore(getSeoIfExists.Id);
                        //_seoService.UpdateSeoScoreAfterCreateTask(getSeoIfExists.Id);
                        // Bu mesele çok önemli. IsCreated false'a çevrilmeli
                    }
                }
                else
                {
                    ViewBag.listSeoMeta = listSeoMetas;
                }

                #endregion

                ViewBag.SeoScore = getSeoIfExists;
                LevelAnalyze(getSeoIfExists.Id);
            }

            #endregion
        }
        public IActionResult RefreshSeoScore(int Id)
        {
            var news = _mapper.Map<NewsDto, NewsEditViewModel>(_newService.getNews(Id));
            AnalyzePostToSeoAsync(news);
            return RedirectToAction("HaberDuzenle", "Haber", new { Id = news.Id, durum = "" });
        }

        private static Random random = new Random();
        public static string RandomStringForUniqueCode(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public void AnalyzePostToSeoAsync(NewsEditViewModel model)
        {

            #region fields

            var seoScoreByNewsId = _mapper.Map<SeoScoreDto, SeoScoreBaseViewModel>(_seoService.GetSeoScoreByNewsId(model.Id));

            var listSeoMetas = _mapper.Map<List<SeoMetaListItemDto>, List<SeoMetaListViewModel>>(_seoService.listSeoMetasBySeoScoreIdByAnalyze(seoScoreByNewsId.Id));

            List<SeoMetaListViewModel> newList = new List<SeoMetaListViewModel>();

            var tags = _mapper.Map<List<TagNewsListItemDto>, List<TagNewsListViewModel>>(_newService.tagsListWithNewsByNewsId(model.Id));

            #endregion

            int count = 0;
            int point = 0;

            foreach (var item in listSeoMetas)
            {

                count += 1;

                if (item.metaCode.Contains("b-3") && item.IsDone == false)
                {
                    if (model.MetaTitle != null || model.MetaTitle == "")
                        newList.Add(item);
                }

                if (item.metaCode.Contains("b-1") && item.IsDone == false)
                {
                    if (model.MetaTitle != null || model.MetaTitle == "")
                    {
                        int lengthOfTitle = model.Title.Length;
                        bool isRight = (lengthOfTitle < 35) ? false :
                            (lengthOfTitle > 65) ? false :
                            (lengthOfTitle > 35 && lengthOfTitle < 65) ? true :
                            (lengthOfTitle == 0) ? false : false;

                        if (isRight)
                            newList.Add(item);
                    }

                }

                if (item.metaCode.Contains("i-2") && item.IsDone == false)
                {
                    if (model.Image != null || model.Image == "")
                        newList.Add(item);
                }

                if (item.metaCode.Contains("d-3") && item.IsDone == false)
                {
                    if (model.Spot != null || model.Spot == "")
                        newList.Add(item);
                }

                if (item.metaCode.Contains("d-1") && item.IsDone == false)
                {
                    int lengthOfTitle = model.Spot.Length;
                    bool isRight = (lengthOfTitle < 120) ? false :
                        (lengthOfTitle > 160) ? false :
                        (lengthOfTitle > 120 && lengthOfTitle < 160) ? true :
                        (lengthOfTitle == 0) ? false : false;

                    if (isRight)
                        newList.Add(item);
                }

                if (item.metaCode.Contains("i-1") && item.IsDone == false)
                {
                    if (model.Image != null || model.Image == "")
                    {
                        string extension = Path.GetExtension(model.Image);
                        bool isRight = extension != ".gif" ? true : false;

                        if (isRight)
                            newList.Add(item);
                    }
                }

                if (item.metaCode.Contains("k-1") && item.IsDone == false)
                {

                    int countTags = tags.Count();

                    bool isRight = countTags < 5 ? false :
                                   countTags > 8 ? false :
                                   countTags >= 5 && countTags <= 8 ? true : false;

                    if (isRight)
                        newList.Add(item);
                }

                if (item.metaCode.Contains("d-2") && item.IsDone == false)
                {
                    if (model.Spot != null || model.Spot == "")
                    {
                        int tagCount = 0;
                        foreach (var tagItem in tags)
                        {
                            if (model.Spot.Contains(tagItem.tag.TagName))
                            {
                                tagCount++;
                            }
                        }
                        bool isRight = tagCount > 0 ? true :
                                          tagCount == 0 ? false : false;

                        if (isRight)
                            newList.Add(item);
                    }

                }

                if (item.metaCode.Contains("b-2") && item.IsDone == false)
                {
                    if (model.MetaTitle != null || model.MetaTitle == "")
                    {
                        int tagCount = 0;
                        foreach (var tagItem in tags)
                        {
                            if (model.MetaTitle.Contains(tagItem.tag.TagName))
                            {
                                tagCount++;
                            }
                        }
                        bool isRight = tagCount > 0 ? true :
                                         tagCount == 0 ? false : false;

                        if (isRight)
                            newList.Add(item);
                    }
                }

                if (count == 10)
                {
                    point += ChangeSeoMetaStatus(newList);
                    _seoService.IncreaseSeoScore(seoScoreByNewsId.Id, point);
                }
            }

        }

        private int ChangeSeoMetaStatus(List<SeoMetaListViewModel> newList)
        {
            int point = 0;
            foreach (var item in newList)
            {
                _seoService.SeoMetaIsDone(item.Id);
                point += item.Point;
            }

            return point;
        }

        public string ScoreAnalyze(int seoScoreId)
        {
            var getSeoIfExists = _mapper.Map<SeoScoreDto, SeoScoreBaseViewModel>(_seoService.GetSeoScore(seoScoreId));
            if (getSeoIfExists.Amount == 0)
            {
                return "Skor Yok";
            }
            else if (getSeoIfExists.Amount >= 1 && getSeoIfExists.Amount <= 34)
            {
                return "Kötü";
            }
            else if (getSeoIfExists.Amount > 34 && getSeoIfExists.Amount <= 60)
            {
                return "Ortalama";
            }
            else if (getSeoIfExists.Amount > 60 && getSeoIfExists.Amount <= 85)
            {
                return "İyi";
            }
            else if (getSeoIfExists.Amount > 85 && getSeoIfExists.Amount <= 9)
            {
                return "Çok İyi";
            }
            else
            {
                return "Hata";
            }
        }

        private void LevelAnalyze(int Id)
        {

            // 1-34 = Kötü | 34-60 = Ortalama | 60-85 = İyi | 85-99 = Çok İyi //
            var getSeoIfExists = _mapper.Map<SeoScoreDto, SeoScoreBaseViewModel>(_seoService.GetSeoScore(Id));
            int levelCase = getSeoIfExists.Level;

            switch (levelCase)
            {
                case 0:
                    ViewData["scoreNote"] = ScoreAnalyze(getSeoIfExists.Id);
                    break;
                case 1:
                    ViewData["scoreNote"] = ScoreAnalyze(getSeoIfExists.Id);
                    ViewData["progress-color"] = "#ea553d";
                    ViewData["bg"] = "text-danger";
                    break;

                case 2:
                    ViewData["scoreNote"] = ScoreAnalyze(getSeoIfExists.Id);
                    ViewData["progress-color"] = "#fb8c00";
                    ViewData["bg"] = "text-warning";
                    break;

                case 3:
                    ViewData["scoreNote"] = ScoreAnalyze(getSeoIfExists.Id);
                    ViewData["progress-color"] = "#67a8e4";
                    ViewData["bg"] = "text-primary";
                    break;
                case 4:
                    ViewData["scoreNote"] = ScoreAnalyze(getSeoIfExists.Id);
                    ViewData["progress-color"] = "#4ac18e";
                    ViewData["bg"] = "text-success";
                    break;

                default:
                    ViewData["scoreNote"] = "Bilgi Alınamadı!";
                    break;
            }
        }

        #endregion

    }
}

