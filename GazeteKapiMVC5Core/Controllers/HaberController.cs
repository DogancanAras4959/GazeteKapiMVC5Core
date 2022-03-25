using AutoMapper;
using CORE.ApplicationCommon.DTOS.CategoryDTO;
using CORE.ApplicationCommon.DTOS.NewsDto.GuestDto;
using GazeteKapiMVC5Core.Core.Extensions;
using GazeteKapiMVC5Core.Models.Account;
using GazeteKapiMVC5Core.Models.Category;
using GazeteKapiMVC5Core.Models.News.GuestModel;
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

        public HaberController(ICategoryService categoryService, IMapper mapper, ILogService logSerivce, INewsService newsService, IWebHostEnvironment webHostEnvironment)
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _logService = logSerivce;
            _newService = newsService;
            _webHostEnvironment = webHostEnvironment;
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
                return RedirectToAction("Home", "ErrorPage");
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
                return RedirectToAction("Home", "ErrorPage");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> KategoriOlustur(CategoryCreateViewModel category)
        {
            try
            {
                AccountEditViewModel yoneticiGetir = SessionExtensionMethod.GetObject<AccountEditViewModel>(HttpContext.Session, "user");
                category.UserId = yoneticiGetir.Id;

                if (ModelState.IsValid)
                {
                    if (!await _categoryService.CategoryIfExists(category.CategoryName))
                    {
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
                return RedirectToAction("Home", "ErrorPage");
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
                return RedirectToAction("Home", "ErrorPage");
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
                return RedirectToAction("Home", "ErrorPage");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> KategoriDuzenle(CategoryEditViewModel category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await _categoryService.UpdateCategory(_mapper.Map<CategoryEditViewModel, CategoryDto>(category)))
                    {
                        await CreateModeratorLog("Sistem Hatası", "Güncelleme", "KategoriDuzenle", "Haber", "Kategori başarıyla düzenlendi!");
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
                return RedirectToAction("Home", "ErrorPage");
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
                return RedirectToAction("Home", "ErrorPage");
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
                return RedirectToAction("Home", "ErrorPage");
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
                return RedirectToAction("Home", "ErrorPage");
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
                return RedirectToAction("Home", "ErrorPage");
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
                return RedirectToAction("Home", "ErrorPage");
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
                return RedirectToAction("Home", "ErrorPage");
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
                return RedirectToAction("Home", "ErrorPage");
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
                return RedirectToAction("Home", "ErrorPage");
            }
        }

        [HttpGet]
        public async Task<IActionResult> YazarDetay(int id)
        {
            try
            {
                await CreateModeratorLog("Başarılı", "Sayfa Girişi", "YazarDetay", "Haber", "Yazar detayına başarılı bir şekilde erişildi!");
                return View(_mapper.Map<GuestDto, GuestEditViewModel>(_newService.getGuest(id)));
            }
            catch (Exception ex)
            {
                string detay = "Sistemden kaynaklı bir hata meydana geldi: " + ex.ToString();
                await CreateModeratorLog("Sistem Hatası", "Sayfa Girişi", "YazarDetay", "Haber", detay);
                return RedirectToAction("Home", "ErrorPage");
            }
        }
        #endregion

        #region Haberler

        //[RoleAuthorize("Haberler")]
        public async Task<IActionResult> Haberler()
        {
            try
            {
                //var haberlist = "";
                return View();
            }
            catch (Exception ex)
            {
                string detay = "Sistemden kaynaklı bir hata meydana geldi: " + ex.ToString();
                await CreateModeratorLog("Sistem Hatası", "Sayfa Girişi", "Haberler", "Haber", detay);
                return RedirectToAction("Home", "ErrorPage");
            }
           
        }

        #endregion

        #region Extend Methods

        public async Task<CheckLogService> CreateModeratorLog(string durum, string islem, string action, string controller, string details)
        {
            AccountEditViewModel yoneticiGetir = SessionExtensionMethod.GetObject<AccountEditViewModel>(HttpContext.Session, "user");
            CheckLogService ct = new CheckLogService(_logService, _mapper);
            if (yoneticiGetir.UserName == "" || yoneticiGetir.UserName == null)
            {
                await ct.CreateLogs(durum, islem, action, controller, details, "Sistem");
                return ct;
            }
            await ct.CreateLogs(durum, islem, action, controller, details, yoneticiGetir.UserName);
            return ct;
        }

        #endregion
    }
}
