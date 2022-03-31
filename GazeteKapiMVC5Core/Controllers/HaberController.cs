using AutoMapper;
using CORE.ApplicationCommon.DTOS.AccountDTO;
using CORE.ApplicationCommon.DTOS.CategoryDTO;
using CORE.ApplicationCommon.DTOS.NewsDTO;
using CORE.ApplicationCommon.DTOS.NewsDTO.GuestDTO;
using CORE.ApplicationCommon.DTOS.NewsDTO.PublishTypeDTO;
using CORE.ApplicationCommon.DTOS.NewsDTO.TagDTO;
using CORE.ApplicationCommon.DTOS.NewsDTO.TagNewsDTO;
using CORE.ApplicationCommon.DTOS.SetingsDTO;
using GazeteKapiMVC5Core.Core.Extensions;
using GazeteKapiMVC5Core.Models.Account;
using GazeteKapiMVC5Core.Models.Category;
using GazeteKapiMVC5Core.Models.News.GuestModel;
using GazeteKapiMVC5Core.Models.News.NewsModel;
using GazeteKapiMVC5Core.Models.News.PublishTypeModel;
using GazeteKapiMVC5Core.Models.News.TagModel;
using GazeteKapiMVC5Core.Models.News.TagNewsModel;
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
        private readonly ILogService _logService;
        private readonly INewsService _newService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUserService _userService;
        private readonly ISettingService _settingService;
        public HaberController(ICategoryService categoryService, IMapper mapper, ILogService logSerivce, INewsService newsService, IWebHostEnvironment webHostEnvironment, IUserService userService, ISettingService settingService)
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _logService = logSerivce;
            _newService = newsService;
            _webHostEnvironment = webHostEnvironment;
            _userService = userService;
            _settingService = settingService;
        }

        #endregion

        #region Categories

        [HttpGet]
        public async Task<IActionResult> Kategoriler()
        {
            try
            {
                var list = _mapper.Map<List<CategoryListItemDto>, List<CategoryListViewModel>>(_categoryService.GetAllCategory());
                await CreateModeratorLog("Başarılı", "Sayfa Girişi", "Kategoriler", "Haber", "Kategori sayfasına giriş başarılı!");
                return View(list);
            }
            catch (Exception ex)
            {
                string detay = "Sistemden kaynaklı bir hata meydana geldi: " + ex.ToString();
                await CreateModeratorLog("Sistem Hatası", "Sayfa Girişi", "Kategoriler", "Haber", detay);
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }

        }

        [HttpGet]
        public async Task<IActionResult> KategoriOlustur()
        {
            try
            {
                var list = _mapper.Map<List<CategoryListItemDto>, List<CategoryListViewModel>>(_categoryService.GetParentCategoryList());
                ViewBag.CategoriesParent = new SelectList(list, "Id", "CategoryName");
                await CreateModeratorLog("Başarılı", "Sayfa Girişi", "KategoriOlustur", "Haber", "Kategori oluşturma sayfasına giriş başarılı!");
                return View(new CategoryCreateViewModel());
            }
            catch (Exception ex)
            {
                string detay = "Sistemden kaynaklı bir hata meydana geldi: " + ex.ToString();
                await CreateModeratorLog("Sistem Hatası", "Sayfa Girişi", "KategoriOlustur", "Haber", detay);
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
                            string uploadfilename = Path.GetFileNameWithoutExtension(file.FileName);
                            string extension = Path.GetExtension(file.FileName);
                            uploadfilename = uploadfilename + DateTime.Now.ToString("yymmssfff") + extension;
                            var path = Path.Combine(this._webHostEnvironment.WebRootPath, "Files", uploadfilename);
                            var stream = new FileStream(path, FileMode.Create);
                            await file.CopyToAsync(stream);
                            category.Image = uploadfilename;
                        }
                        else
                        {
                            category.Image = "categorydefault.jpg";
                        }

                        if (await _categoryService.CreateCategory(_mapper.Map<CategoryCreateViewModel, CategoryDto>(category)))
                        {
                            await CreateModeratorLog("Başarılı", "Ekleme", "KategoriOlustur", "Haber", "Kategori eklemesi başarılı oldu!");
                            return RedirectToAction(nameof(Kategoriler));
                        }
                        else
                        {
                            await CreateModeratorLog("Başarısız", "Ekleme", "KategoriOlustur", "Haber", "Kategori eklenirken bir hata meydana geldi!");
                            return RedirectToAction(nameof(Kategoriler));
                        }
                    }
                    else
                    {
                        await CreateModeratorLog("Başarısız", "Ekleme", "KategoriOlustur", "Haber", "Bu kategori sistemde zaten bulunuyor");
                        return RedirectToAction(nameof(Kategoriler));
                    }
                }
                return View(category);
            }
            catch (Exception ex)
            {
                string detay = "Sistemden kaynaklı bir hata meydana geldi: " + ex.ToString();
                await CreateModeratorLog("Sistem Hatası", "Ekleme", "KategoriOlustur", "Haber", detay);
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }

        }

        public async Task<IActionResult> KategoriSil(int id)
        {
            try
            {
                if (!_categoryService.DeleteCategoryById(id))
                {
                    await CreateModeratorLog("Başarısız", "Silme", "KategoriSil", "Haber", "Kategori silinirken bir hata meydana geldi");
                    return RedirectToAction(nameof(Kategoriler));
                }
                else
                {
                    await CreateModeratorLog("Başarılı", "Silme", "KategoriSil", "Haber", "Kategori başarıyla sistemden kaldırıldı");
                    return RedirectToAction(nameof(Kategoriler));
                }
            }
            catch (Exception ex)
            {
                string detay = "Sistemden kaynaklı bir hata meydana geldi: " + ex.ToString();
                await CreateModeratorLog("Sistem Hatası", "Silme", "KategoriSil", "Haber", detay);
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }

        }

        [HttpGet]
        public async Task<IActionResult> KategoriDuzenle(int id)
        {
            try
            {
                var categories = _mapper.Map<CategoryDto, CategoryEditViewModel>(_categoryService.GetCategoryById(id));
                var list = _mapper.Map<List<CategoryListItemDto>, List<CategoryListViewModel>>(_categoryService.GetParentCategoryList());
                ViewBag.CategoriesParent = new SelectList(list, "Id", "CategoryName", categories.Id);
                await CreateModeratorLog("Başarılı", "Sayfa Girişi", "KategoriDuzenle", "Haber", "Kategori düzenleme sayfasına başarıyla girildi!");
                return View(categories);
            }
            catch (Exception ex)
            {
                string detay = "Sistemden kaynaklı bir hata meydana geldi: " + ex.ToString();
                await CreateModeratorLog("Sistem Hatası", "Sayfa Girişi", "KategoriDuzenle", "Haber", detay);
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
                        string uploadfilename = Path.GetFileNameWithoutExtension(file.FileName);
                        string extension = Path.GetExtension(file.FileName);
                        uploadfilename = uploadfilename + DateTime.Now.ToString("yymmssfff") + extension;
                        var path = Path.Combine(this._webHostEnvironment.WebRootPath, "Files", uploadfilename);
                        var stream = new FileStream(path, FileMode.Create);
                        await file.CopyToAsync(stream);
                        category.Image = uploadfilename;
                    }
                    else
                    {
                        category.Image = "categorydefault.jpg";
                    }

                    if (await _categoryService.UpdateCategory(_mapper.Map<CategoryEditViewModel, CategoryDto>(category)))
                    {
                        await CreateModeratorLog("Başarılı", "Güncelleme", "KategoriDuzenle", "Haber", "Kategori başarıyla düzenlendi!");
                        return RedirectToAction(nameof(Kategoriler));
                    }
                    else
                    {
                        await CreateModeratorLog("Başarısız", "Güncelleme", "KategoriDuzenle", "Haber", "Kategori düzenleme işlemi sırasında bir hata meydana geldi");
                        return RedirectToAction(nameof(Kategoriler));
                    }
                }
                return View(category);
            }
            catch (Exception ex)
            {
                string detay = "Sistemden kaynaklı bir hata meydana geldi: " + ex.ToString();
                await CreateModeratorLog("Sistem Hatası", "Güncelleme", "KategoriDuzenle", "Haber", detay);
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }

        }

        public async Task<IActionResult> KategoriDurumDuzenle(int id)
        {
            try
            {
                if (await _categoryService.EditIsActiveById(id))
                {
                    await CreateModeratorLog("Başarılı", "Güncelleme", "KategoriDurumDuzenle", "Haber", "Kategori durumunu düzenleme işlemi başarıyla gerçekleşti");
                    return RedirectToAction(nameof(Kategoriler));
                }
                else
                {
                    await CreateModeratorLog("Başarısız", "Güncelleme", "KategoriDurumDuzenle", "Haber", "Kategori durumu düzenlerken bir hata meydana geldi");
                    return RedirectToAction(nameof(Kategoriler));
                }
            }
            catch (Exception ex)
            {
                string detay = "Sistemden kaynaklı bir hata meydana geldi: " + ex.ToString();
                await CreateModeratorLog("Sistem Hatası", "Güncelleme", "KategoriDurumDuzenle", "Haber", detay);
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }

        }

        #endregion

        #region Yazarlar
        public async Task<IActionResult> Yazarlar()
        {
            try
            {
                var listGuest = _mapper.Map<List<GuestListItemDto>, List<GuestListViewModel>>(_newService.guestList());

                await CreateModeratorLog("Başarılı", "Sayfa Girişi", "Yazarlar", "Haber", "Yazarlar sayfasına giriş başarılı!");

                return View(listGuest);
            }
            catch (Exception ex)
            {
                string detay = "Sistemden kaynaklı bir hata meydana geldi: " + ex.ToString();
                await CreateModeratorLog("Sistem Hatası", "Sayfa Girişi", "Yazarlar", "Haber", detay);
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }
        }
        public async Task<IActionResult> YazarOlustur()
        {
            try
            {
                await CreateModeratorLog("Başarılı", "Sayfa Girişi", "YazarOlustur", "Haber", "Yazar oluşturma sayfasına giriş başarılı!");
                return View(new GuestCreateViewModel());
            }
            catch (Exception ex)
            {
                string detay = "Sistemden kaynaklı bir hata meydana geldi: " + ex.ToString();
                await CreateModeratorLog("Sistem Hatası", "Sayfa Girişi", "YazarOlustur", "Haber", detay);
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
                        string uploadfilename = Path.GetFileNameWithoutExtension(file.FileName);
                        string extension = Path.GetExtension(file.FileName);
                        uploadfilename = uploadfilename + DateTime.Now.ToString("yymmssfff") + extension;
                        var path = Path.Combine(this._webHostEnvironment.WebRootPath, "Files", uploadfilename);
                        var stream = new FileStream(path, FileMode.Create);
                        await file.CopyToAsync(stream);
                        model.GuestImage = uploadfilename;
                    }
                    else
                    {
                        model.GuestImage = "user.png";
                    }

                    if (await _newService.createGuets(_mapper.Map<GuestCreateViewModel, GuestDto>(model)))
                    {
                        await CreateModeratorLog("Başarılı", "Ekleme", "YazarOlustur", "Haber", "Yazar başarıyla eklendi!");
                        return RedirectToAction(nameof(Yazarlar));
                    }
                    else
                    {
                        await CreateModeratorLog("Başarısız", "Ekleme", "YazarOlustur", "Haber", "Yazar oluşturulurken bir sorun ortaya çıktı");
                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {
                string detay = "Sistemden kaynaklı bir hata meydana geldi: " + ex.ToString();
                await CreateModeratorLog("Sistem Hatası", "Ekleme", "YazarOlustur", "Haber", detay); 
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }
        }

        public async Task<IActionResult> YazarSil(int Id)
        {
            try
            {
                if (!_newService.guestDelete(Id))
                {
                    await CreateModeratorLog("Başarısız", "Silme", "YazarSil", "Haber", "Yazar silinirken bir hata meydana geldi");
                    return RedirectToAction(nameof(Yazarlar));
                }
                else
                {
                    await CreateModeratorLog("Başarılı", "Silme", "YazarSil", "Haber", "Yazar başarıyla sistemden kaldırıldı");
                    return RedirectToAction(nameof(Yazarlar));
                }
            }
            catch (Exception ex)
            {
                string detay = "Sistemden kaynaklı bir hata meydana geldi: " + ex.ToString();
                await CreateModeratorLog("Sistem Hatası", "Silme", "YazarSil", "Haber", detay);
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }
        }

        public async Task<IActionResult> YazarDurumDuzenle(int Id)
        {
            try
            {
                if (await _newService.EditIsActive(Id))
                {
                    await CreateModeratorLog("Başarılı", "Güncelleme", "YazarDurumDuzenle", "Haber", "Yazar başarıyla güncellendi. Kullanıcını durumu düzenlendi!");
                    return RedirectToAction(nameof(Yazarlar));
                }
                else
                {
                    await CreateModeratorLog("Başarısız", "Güncelleme", "YazarDurumDuzenle", "Haber", "Yazar durumu güncellenemedi!");
                    return RedirectToAction(nameof(Yazarlar));
                }
            }
            catch (Exception ex)
            {
                string detay = "Sistemden kaynaklı bir hata meydana geldi: " + ex.ToString();
                await CreateModeratorLog("Sistem Hatası", "Silme", "YazarDurumDuzenle", "Haber", detay);
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }
        }

        public async Task<IActionResult> YazarDuzenle(int Id)
        {
            try
            {
                var getUser = _mapper.Map<GuestDto, GuestEditViewModel>(_newService.getGuest(Id));
                await CreateModeratorLog("Başarılı", "Sayfa Girişi", "YazarDuzenle", "Haber", "Yazar düzenleme ekranına giriş başarılı!");
                return View(getUser);
            }
            catch (Exception ex)
            {
                string detay = "Sistemden kaynaklı bir hata meydana geldi: " + ex.ToString();
                await CreateModeratorLog("Sistem Hatası", "Sayfa Girişi", "YazarDuzenle", "Haber", detay);
                TempData["HataMesaji"] = ex.ToString();

                return RedirectToAction("ErrorPage", "Home");
            }
        }

        public async Task<IActionResult> YazarinYazilari(int Id, int? pageNumber, int? categoryId, string searchstring, int? userId)
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
                    await CreateModeratorLog("Başarılı", "Sayfa Girişi", "YazarinYazilari", "Haber", "Yazarın Yazılarına giriş başarılı!");
                    return View(PaginationList<NewsLıstItemModel>.Create(haberlist.ToList(), pageNumber ?? 1, pageSize));
                }

                if (categoryId != null && categoryId != 0)
                {
                    haberlist = _mapper.Map<List<NewsListItemDto>, List<NewsLıstItemModel>>(_newService.FilterCategoryInNewsWithGuest(categoryId, Id));
                    await CreateModeratorLog("Başarılı", "Sayfa Girişi", "YazarinYazilari", "Haber", "Yazarın Yazılarına giriş başarılı!");
                    return View(PaginationList<NewsLıstItemModel>.Create(haberlist.ToList(), pageNumber ?? 1, pageSize));
                }

                if (userId != null && userId != 0)
                {
                    haberlist = _mapper.Map<List<NewsListItemDto>, List<NewsLıstItemModel>>(_newService.FilterUserInNewsWithGuest(userId, Id));
                    await CreateModeratorLog("Başarılı", "Sayfa Girişi", "YazarinYazilari", "Haber", "Yazarın Yazılarına giriş başarılı!");
                    return View(PaginationList<NewsLıstItemModel>.Create(haberlist.ToList(), pageNumber ?? 1, pageSize));
                }

                haberlist = _mapper.Map<List<NewsListItemDto>, List<NewsLıstItemModel>>(_newService.newsListWithGuest(Id));
                await CreateModeratorLog("Başarılı", "Sayfa Girişi", "YazarinYazilari", "Haber", "Yazarın Yazılarına giriş başarılı!");
                return View(PaginationList<NewsLıstItemModel>.Create(haberlist.ToList(), pageNumber ?? 1, pageSize));
            }
            catch (Exception ex)
            {
                string detay = "Sistemden kaynaklı bir hata meydana geldi: " + ex.ToString();
                await CreateModeratorLog("Sistem Hatası", "Sayfa Girişi", "YazarinYazilari", "Haber", detay);
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
                        string uploadfilename = Path.GetFileNameWithoutExtension(file.FileName);
                        string extension = Path.GetExtension(file.FileName);
                        uploadfilename = uploadfilename + DateTime.Now.ToString("yymmssfff") + extension;
                        var path = Path.Combine(this._webHostEnvironment.WebRootPath, "Files", uploadfilename);
                        var stream = new FileStream(path, FileMode.Create);
                        await file.CopyToAsync(stream);
                        model.GuestImage = uploadfilename;
                    }
                    else
                    {
                        model.GuestImage = "user.png";
                    }

                    if (await _newService.editGuest(_mapper.Map<GuestEditViewModel, GuestDto>(model)))
                    {
                        await CreateModeratorLog("Başarılı", "Güncelleme", "YazarDuzenle", "Haber", "Yazar başarıyla güncellendi!");
                        return RedirectToAction(nameof(Yazarlar));
                    }
                    else
                    {
                        await CreateModeratorLog("Başarısız", "Güncelleme", "YazarDuzenle", "Haber", "Yazar güncellenirken bir sorun ortaya çıktı");
                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {
                string detay = "Sistemden kaynaklı bir hata meydana geldi: " + ex.ToString();
                await CreateModeratorLog("Sistem Hatası", "Güncelleme", "YazarDuzenle", "Haber", detay);
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> YazarDetay(int id)
        {
            try
            {
                var haberlist = _mapper.Map<List<NewsListItemDto>, List<NewsLıstItemModel>>(_newService.newsListWithGuestOneToFive(id));
                ViewBag.Yazilar = haberlist;

                await CreateModeratorLog("Başarılı", "Sayfa Girişi", "YazarDetay", "Haber", "Yazar detayına başarılı bir şekilde erişildi!");
                return View(_mapper.Map<GuestDto, GuestEditViewModel>(_newService.getGuest(id)));
            }
            catch (Exception ex)
            {
                string detay = "Sistemden kaynaklı bir hata meydana geldi: " + ex.ToString();
                await CreateModeratorLog("Sistem Hatası", "Sayfa Girişi", "YazarDetay", "Haber", detay); 
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }
        }
        #endregion

        #region Haberler

        //[RoleAuthorize("Haberler")]
        [HttpGet]
        public async Task<IActionResult> Haberler(int? pageNumber, string searchstring, int? CategoryId, int? UserId)
        {
            try
            {
                int pageSize = 20;
                List<NewsLıstItemModel> haberlist = null;

                var categories = _mapper.Map<List<CategoryListItemDto>, List<CategoryListViewModel>>(_categoryService.GetAllCategory());
                ViewBag.Categories = new SelectList(categories.ToList(), "Id", "CategoryName");

                var users = _mapper.Map<List<UserListItemDto>, List<UserListViewModel>>(_userService.GetAllUsers());
                ViewBag.Users = new SelectList(users.ToList(), "Id", "DisplayName");

                if (searchstring != "" && searchstring != null)
                {
                    haberlist = _mapper.Map<List<NewsListItemDto>, List<NewsLıstItemModel>>(_newService.searchDataInNews(searchstring)); 
                    await CreateModeratorLog("Başarılı", "Sayfa Girişi", "Haberler", "Haber", "Haber sayfasına giriş başarılı!");
                    return View(PaginationList<NewsLıstItemModel>.Create(haberlist.ToList(), pageNumber ?? 1, pageSize));
                }
              
                if (CategoryId != 0 && CategoryId != null)
                {
                    haberlist = _mapper.Map<List<NewsListItemDto>, List<NewsLıstItemModel>>(_newService.newsListByCategoryId(CategoryId));
                    await CreateModeratorLog("Başarılı", "Sayfa Girişi", "Haberler", "Haber", "Haber sayfasına giriş başarılı!");
                    return View(PaginationList<NewsLıstItemModel>.Create(haberlist.ToList(), pageNumber ?? 1, pageSize));
                }
              
                if (UserId != 0 && UserId != null)
                {
                    haberlist = _mapper.Map<List<NewsListItemDto>, List<NewsLıstItemModel>>(_newService.newsListByUserIdInAll(UserId));
                    await CreateModeratorLog("Başarılı", "Sayfa Girişi", "Haberler", "Haber", "Haber sayfasına giriş başarılı!");
                    return View(PaginationList<NewsLıstItemModel>.Create(haberlist.ToList(), pageNumber ?? 1, pageSize));
                }
              
                haberlist = _mapper.Map<List<NewsListItemDto>, List<NewsLıstItemModel>>(_newService.newsList());
                await CreateModeratorLog("Başarılı", "Sayfa Girişi", "Haberler", "Haber", "Haber sayfasına giriş başarılı!");
                return View(PaginationList<NewsLıstItemModel>.Create(haberlist.ToList(), pageNumber ?? 1, pageSize));
            }
            catch (Exception ex)
            {
                string detay = "Sistemden kaynaklı bir hata meydana geldi: " + ex.ToString();
                await CreateModeratorLog("Sistem Hatası", "Sayfa Girişi", "Haberler", "Haber", detay);
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }

        }

        [HttpGet]
        public async Task<IActionResult> HaberOlustur()
        {
            try
            {
                var publishTypeList = _mapper.Map<List<PublishTypeListItem>, List<PublishTypeListViewModel>>(_newService.publishTypeList());
                ViewBag.PublishTypes = new SelectList(publishTypeList, "Id", "TypeName");

                var categoryList = _mapper.Map<List<CategoryListItemDto>, List<CategoryListViewModel>>(_categoryService.GetAllCategory());
                ViewBag.Categories = new SelectList(categoryList, "Id", "CategoryName");

                var guestList = _mapper.Map<List<GuestListItemDto>, List<GuestListViewModel>>(_newService.guestList());
                ViewBag.Guests = new SelectList(guestList, "Id", "GuestName");

                //var publishedTypeList = _mapper.Map<List<PublishedTypeListItemDto>
                return View(new NewsCreateViewModel());
            }
            catch (Exception ex)
            {
                string detay = "Sistemden kaynaklı bir hata meydana geldi: " + ex.ToString();
                await CreateModeratorLog("Sistem Hatası", "Sayfa Girişi", "HaberOlustur", "Haber", detay);
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
                        if (file != null)
                        {
                            if (model.CategoryId != 0)
                            {
                                AccountEditViewModel yoneticiGetir = SessionExtensionMethod.GetObject<AccountEditViewModel>(HttpContext.Session, "user");
                                model.UserId = yoneticiGetir.Id;

                                string uploadfilename = Path.GetFileNameWithoutExtension(file.FileName);
                                string extension = Path.GetExtension(file.FileName);
                                uploadfilename = uploadfilename + DateTime.Now.ToString("yymmssfff") + extension;
                                var path = Path.Combine(this._webHostEnvironment.WebRootPath, "Files", uploadfilename);
                                var stream = new FileStream(path, FileMode.Create);
                                await file.CopyToAsync(stream);
                                model.Image = uploadfilename;

                                int resultId = Convert.ToInt32( await _newService.createNews(_mapper.Map<NewsCreateViewModel, NewsDto>(model)));

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

                                    await CreateModeratorLog("Başarılı", "Ekleme", "HaberOlustur", "Haber", "Haber oluşturulması başarıyla gerçekleşti. Haberinizi istediğiniz gibi düzenleyebilirsiniz!");
                                    return RedirectToAction("HaberDuzenle", "Haber", new { Id = resultId });
                                }
                                else
                                {
                                    await CreateModeratorLog("Başarısız", "Ekleme", "HaberOlustur", "Haber", "Haberiniz oluşturulurken bir hata meydana geldi. Haber oluşturulamadı!");
                                    return View(model);
                                }
                            }
                            else
                            {
                                await CreateModeratorLog("Başarısız", "Ekleme", "HaberOlustur", "Haber", "Haber için kategori girilmelidir. Haber bu yüzden oluşturulamadı!");
                                return View(model);
                            }
                        }
                        else
                        {
                            await CreateModeratorLog("Başarısız", "Ekleme", "HaberOlustur", "Haber", "Haber için öne çıkan görsel girilmelidir. Haber bu yüzden oluşturulamadı");
                            return View(model);
                        }
                    }
                    else
                    {
                        await CreateModeratorLog("Başarısız", "Ekleme", "HaberOlustur", "Haber", "Bu haber zaten sistemde bulunuyor!");
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
                string detay = "Sistemden kaynaklı bir hata meydana geldi: " + ex.ToString();
                await CreateModeratorLog("Sistem Hatası", "Ekleme", "HaberOlustur", "Haber", detay);
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> HaberDuzenle(int id)
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

                var publishTypeList = _mapper.Map<List<PublishTypeListItem>, List<PublishTypeListViewModel>>(_newService.publishTypeList());
                ViewBag.PublishTypes = new SelectList(publishTypeList, "Id", "TypeName", news.PublishTypeId);

                var categoryList = _mapper.Map<List<CategoryListItemDto>, List<CategoryListViewModel>>(_categoryService.GetAllCategory());
                ViewBag.Categories = new SelectList(categoryList, "Id", "CategoryName", news.CategoryId);

                var guestList = _mapper.Map<List<GuestListItemDto>, List<GuestListViewModel>>(_newService.guestList());
                ViewBag.Guests = new SelectList(guestList, "Id", "GuestName", news.GuestId);

                return View(news);
            }
            catch (Exception ex)
            {
                string detay = "Sistemden kaynaklı bir hata meydana geldi: " + ex.ToString();
                await CreateModeratorLog("Sistem Hatası", "Sayfa Girişi", "HaberDuzenle", "Haber", detay); 
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
                    if (model.CategoryId != 0)
                    {
                        AccountEditViewModel yoneticiGetir = SessionExtensionMethod.GetObject<AccountEditViewModel>(HttpContext.Session, "user");
                        model.UserId = yoneticiGetir.Id;

                        if (file != null)
                        {
                            string uploadfilename = Path.GetFileNameWithoutExtension(file.FileName);
                            string extension = Path.GetExtension(file.FileName);
                            uploadfilename = uploadfilename + DateTime.Now.ToString("yymmssfff") + extension;
                            var path = Path.Combine(this._webHostEnvironment.WebRootPath, "Files", uploadfilename);
                            var stream = new FileStream(path, FileMode.Create);
                            await file.CopyToAsync(stream);
                            model.Image = uploadfilename;

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

                                await CreateModeratorLog("Başarılı", "Güncelleme", "HaberDuzenle", "Haber", "Haber güncellemesi başarıyla gerçekleşti. Haberinizi istediğiniz gibi düzenleyebilirsiniz!");
                                return RedirectToAction("HaberDuzenle", "Haber", new { Id = resultId });
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

                                await CreateModeratorLog("Başarılı", "Güncelleme", "HaberDuzenle", "Haber", "Haber güncellemesi başarıyla gerçekleşti. Haberinizi istediğiniz gibi düzenleyebilirsiniz!");
                                return RedirectToAction("HaberDuzenle", "Haber", new { Id = resultId });
                            }
                        }

                    }

                    return View(model);
                }
                else
                {
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                string detay = "Sistemden kaynaklı bir hata meydana geldi: " + ex.ToString();
                await CreateModeratorLog("Sistem Hatası", "Güncelleme", "HaberDuzenle", "Haber", detay);
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }
        }

        public async Task<IActionResult> HaberSil(int id)
        {
            try
            {
                if (_newService.newsDelete(id))
                {
                    await CreateModeratorLog("Başarılı", "Silme", "HaberSil", "Haber", "Haber silme işlemi başarıyla gerçekleşti!");
                    return RedirectToAction("Haberler", "Haber");
                }
                else
                {
                    await CreateModeratorLog("Başarısız", "Silme", "HaberSil", "Haber", "Haber silme işlemi başarısız gerçekleşti!");
                    return RedirectToAction("Haberler", "Haber");
                }
            }
            catch (Exception ex)
            {
                string detay = "Sistemden kaynaklı bir hata meydana geldi: " + ex.ToString();
                await CreateModeratorLog("Sistem Hatası", "Silme", "HaberSil", "Haber", detay);
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }
        }

        public async Task<IActionResult> HaberiKilitle(int id)
        {
            try
            {
                if (await _newService.IsLockNews(id))
                {
                    await CreateModeratorLog("Başarılı", "Güncelleme", "HaberKilitle", "Haber", "Haber başarıyla kilitlendi!");
                    return RedirectToAction(nameof(Haberler));
                }
                return View(nameof(Haberler));
            }
            catch (Exception ex)
            {
                string detay = "Sistemden kaynaklı bir hata meydana geldi: " + ex.ToString();
                await CreateModeratorLog("Sistem Hatası", "Güncelleme", "HaberiKilitle", "Haber", detay);
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }
        }

        public async Task<IActionResult> BildirimleriAc(int id)
        {
            try
            {
                if (await _newService.IsOpenNotificationSet(id))
                {
                    await CreateModeratorLog("Başarılı", "Güncelleme", "BildirimleriAc", "Haber", "Haber bildirimleri başarıyla açıldı!");
                    return RedirectToAction(nameof(Haberler));
                }
                return View(nameof(Haberler));
            }
            catch (Exception ex)
            {
                string detay = "Sistemden kaynaklı bir hata meydana geldi: " + ex.ToString();
                await CreateModeratorLog("Sistem Hatası", "Güncelleme", "BildirimleriAc", "Haber", detay); 
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }
        }

        public async Task<IActionResult> HaberiManseteTasi(int id)
        {
            try
            {
                if (await _newService.SetYourNewsToUp(id))
                {
                    await CreateModeratorLog("Başarılı", "Güncelleme", "HaberiManseteTasi", "Haber", "Haber başarıyla manşete taşındı!");
                    return RedirectToAction(nameof(Haberler));
                }
                return View(nameof(Haberler));
            }
            catch (Exception ex)
            {
                string detay = "Sistemden kaynaklı bir hata meydana geldi: " + ex.ToString();
                await CreateModeratorLog("Sistem Hatası", "Güncelleme", "HaberiManseteTasi", "Haber", detay);
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
                    await CreateModeratorLog("Başarılı", "Güncelleme", "HaberiAktifEt", "Haber", "Haber başarıyla manşete taşındı!");
                    return RedirectToAction(nameof(Haberler));
                }
                return View(nameof(Haberler));
            }
            catch (Exception ex)
            {
                string detay = "Sistemden kaynaklı bir hata meydana geldi: " + ex.ToString();
                await CreateModeratorLog("Sistem Hatası", "Güncelleme", "HaberiAktifEt", "Haber", detay);
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> Etiketler(int? pageNumber, string tagNameSearch)
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
                    await CreateModeratorLog("Başarılı", "Sayfa Girişi", "Etiketler", "Haber", "Haber sayfasına giriş başarılı!");
                    return View(PaginationList<TagListViewModel>.Create(tags.ToList(), pageNumber ?? 1, pageSize));
                }

                tags = _mapper.Map<List<TagListItemDto>, List<TagListViewModel>>(_newService.tagList());
                await CreateModeratorLog("Başarılı", "Sayfa Girişi", "Etiketler", "Haber", "Haber sayfasına giriş başarılı!");
                return View(PaginationList<TagListViewModel>.Create(tags.ToList(), pageNumber ?? 1, pageSize));
            }
            catch (Exception ex)
            {
                string detay = "Sistemden kaynaklı bir hata meydana geldi: " + ex.ToString();
                await CreateModeratorLog("Sistem Hatası", "Sayfa Girişi", "Etiketler", "Haber", detay);
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }
        }

        public async Task<IActionResult> EtiketeGoreHaberler(int Id, int? pageNumber)
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
                string detay = "Sistemden kaynaklı bir hata meydana geldi: " + ex.ToString();
                await CreateModeratorLog("Sistem Hatası", "Sayfa Girişi", "EtiketeGoreHaberler", "Haber", detay);
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }
        }

        public async Task<IActionResult> EtiketSil(int id)
        {
            try
            {
                if (_newService.tagDelete(id))
                {
                    await CreateModeratorLog("Başarılı", "Silme", "EtiketSil", "Haber", "Etiket silme işlemi başarıyla gerçekleşti!");
                    return RedirectToAction(nameof(Etiketler));
                }
                else
                {
                    await CreateModeratorLog("Başarısız", "Silme", "HaberSil", "Haber", "Etiket silme işlemi başarısız gerçekleşti!");
                    return RedirectToAction(nameof(Etiketler));
                }
            }
            catch (Exception ex)
            {
                string detay = "Sistemden kaynaklı bir hata meydana geldi: " + ex.ToString();
                await CreateModeratorLog("Sistem Hatası", "Silme", "HaberSil", "Haber", detay);
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }
        }

        #endregion

        #region Extend Methods
        public async Task<CheckLogService> CreateModeratorLog(string durum, string islem, string action, string controller, string details)
        {
            AccountEditViewModel yoneticiGetir = SessionExtensionMethod.GetObject<AccountEditViewModel>(HttpContext.Session, "user");

            var setting = _mapper.Map<SettingsDto, SettingsBaseViewModel>(_settingService.getSettings(1));

            if (setting.LogIsActive == true)
            {
                if (setting.LogSystemErrorActive == true)
                {
                    CheckLogService ct = new CheckLogService(_logService, _mapper);
                    if (yoneticiGetir.UserName == "" || yoneticiGetir.UserName == null)
                    {
                        durum = "Sistem Hatası";

                        await ct.CreateLogs(durum, islem, action, controller, details, "Sistem");
                        return ct;
                    }
                    await ct.CreateLogs(durum, islem, action, controller, details, yoneticiGetir.UserName);
                    return ct;
                }
                else
                {
                    CheckLogService ct = new CheckLogService(_logService, _mapper);
                    if (yoneticiGetir.UserName == "" || yoneticiGetir.UserName == null)
                    {
                        await ct.CreateLogs(durum, islem, action, controller, details, "Sistem");
                        return ct;
                    }
                    await ct.CreateLogs(durum, islem, action, controller, details, yoneticiGetir.UserName);
                    return ct;
                }
              
            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}
