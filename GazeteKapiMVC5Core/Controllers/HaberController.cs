using AutoMapper;
using CORE.ApplicationCommon.DTOS.AccountDTO;
using CORE.ApplicationCommon.DTOS.CategoryDTO;
using CORE.ApplicationCommon.DTOS.NewsDTO;
using CORE.ApplicationCommon.DTOS.NewsDTO.GuestDTO;
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
using GazeteKapiMVC5Core.Models.News.GuestModel;
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
                            //string uploadfilename = Path.GetFileNameWithoutExtension(file.FileName);
                            //string extension = Path.GetExtension(file.FileName);
                            //uploadfilename = uploadfilename + DateTime.Now.ToString("yymmssfff") + extension;
                            //var path = Path.Combine(this._webHostEnvironment.WebRootPath, "Files", uploadfilename);
                            //var stream = new FileStream(path, FileMode.Create);
                            //await file.CopyToAsync(stream);
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
                return RedirectToAction("ErrorPage", "Home");
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
        public IActionResult Haberler(int? pageNumber, string searchstring, int? CategoryId, int? UserId)
        {
            try
            {
                int pageSize = 50;
                List<NewsLıstItemModel> haberlist = null;

                var categories = _mapper.Map<List<CategoryListItemDto>, List<CategoryListViewModel>>(_categoryService.GetAllCategory());
                ViewBag.Categories = new SelectList(categories.ToList(), "Id", "CategoryName");

                var users = _mapper.Map<List<UserListItemDto>, List<UserListViewModel>>(_userService.GetAllUsers());
                ViewBag.Users = new SelectList(users.ToList(), "Id", "DisplayName");

                if (searchstring != "" && searchstring != null)
                {
                    haberlist = _mapper.Map<List<NewsListItemDto>, List<NewsLıstItemModel>>(_newService.searchDataInNews(searchstring));
                    return View(PaginationList<NewsLıstItemModel>.Create(haberlist.ToList(), pageNumber ?? 1, pageSize));
                }

                if (CategoryId != 0 && CategoryId != null)
                {
                    haberlist = _mapper.Map<List<NewsListItemDto>, List<NewsLıstItemModel>>(_newService.newsListByCategoryId(CategoryId));
                    return View(PaginationList<NewsLıstItemModel>.Create(haberlist.ToList(), pageNumber ?? 1, pageSize));
                }

                if (UserId != 0 && UserId != null)
                {
                    haberlist = _mapper.Map<List<NewsListItemDto>, List<NewsLıstItemModel>>(_newService.newsListByUserIdInAll(UserId));
                    return View(PaginationList<NewsLıstItemModel>.Create(haberlist.ToList(), pageNumber ?? 1, pageSize));
                }

                haberlist = _mapper.Map<List<NewsListItemDto>, List<NewsLıstItemModel>>(_newService.newsList());
                return View(PaginationList<NewsLıstItemModel>.Create(haberlist.ToList(), pageNumber ?? 1, pageSize));
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

                                    string[] allowImageTypes = new string[] { "image/jpeg", "image(png" };

                                    if (!allowImageTypes.Contains(file.ContentType.ToLower()))
                                    {
                                        return View(model);
                                    }

                                    model.Image = SaveImageProcess.ImageInsert(file, "Admin");
                                    model.UserId = yoneticiGetir.Id;
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
                                    ViewBag.Hata = "Haber için kategori seçilmelidir. Haber bu yüzden oluşturulamadı!"!;
                                    LoadData();
                                    return View(model);
                                }
                            }
                        }
                        else
                        {
                            ViewBag.Hata = "Haber için öne çıkan görsel girilmelidir. Haber bu yüzden oluşturulamadı"!;
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HaberDuzenle(NewsEditViewModel model, IFormFile file)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var news = _mapper.Map<NewsDto, NewsEditViewModel>(_newService.getNews(model.Id));

                    if (model.PublishTypeId == 999)
                    {
                        ViewBag.Hata = "Haber tipi seçimiyle ilgili bir sorun çıktı. Haber tipi seçiniz"!;
                        LoadData();
                        return View(news);
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
                                    return RedirectToAction("Haberler", "Haber");
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
                                    return RedirectToAction("Haberler", "Haber");

                                }
                            }

                        }
                    }

                    LoadData();
                    return View(news);
                }
                else
                {
                    LoadData();
                    return View(model);
                }
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
        public IActionResult Etiketler(int? pageNumber, string tagNameSearch)
        {
            try
            {
                var etiketlerHaberler = _mapper.Map<List<TagNewsListItemDto>, List<TagNewsListViewModel>>(_newService.tagsListWithNews());
                ViewBag.EtiketHaberler = etiketlerHaberler;

                int pageSize = 20;
                List<TagListViewModel> tags = null;

                if (tagNameSearch != null && tagNameSearch != "")
                {
                    tags = _mapper.Map<List<TagListItemDto>, List<TagListViewModel>>(_newService.tagListWithSearch(tagNameSearch));
                    return View(PaginationList<TagListViewModel>.Create(tags.ToList(), pageNumber ?? 1, pageSize));
                }

                tags = _mapper.Map<List<TagListItemDto>, List<TagListViewModel>>(_newService.tagList());
                return View(PaginationList<TagListViewModel>.Create(tags.ToList(), pageNumber ?? 1, pageSize));
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
            return Json(new { url = "https://uploads.gazetekapi.com/images/" + filePath });
        }

        [HttpPost]
        public async Task HaberIliskilendir(string haberlist, int haberId)
        {
            string[] array = haberlist.Split(',');
            foreach (var item in array)
            {
                int CurrentId = Convert.ToInt32(item);
                var news = _mapper.Map<NewsDto, NewsEditViewModel>(_newService.getNews(CurrentId));
                news.ParentNewsId = haberId;
                int resultId = Convert.ToInt32(await _newService.editNews(_mapper.Map<NewsEditViewModel, NewsDto>(news)));
            }
        }

        public async Task<IActionResult> IliskiliHaberiKaldir(int id)
        {
            var news = _mapper.Map<NewsDto, NewsEditViewModel>(_newService.getNews(id));
            try
            {
                news.ParentNewsId = 0;
                int resultId = Convert.ToInt32(await _newService.editNews(_mapper.Map<NewsEditViewModel, NewsDto>(news)));
                return RedirectToAction("HaberDuzenle", "Haber", new { id = news.ParentNewsId });
            }
            catch (Exception ex)
            {
                return RedirectToAction("HaberDuzenle", "Haber", new { id = news.ParentNewsId });
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
                    var getNews = _mapper.Map<NewsDto, NewsEditViewModel>(_newService.getNews(itemId));
                    getNews.RowNo = count;

                    int result = Convert.ToInt32(await _newService.editNews(_mapper.Map<NewsEditViewModel, NewsDto>(getNews)));
                }
                catch (Exception ex)
                {
                    continue;
                }
                count++;
            }
            return Json(true);
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
        public IActionResult OrtamMedyasi()
        {
            return View();
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

