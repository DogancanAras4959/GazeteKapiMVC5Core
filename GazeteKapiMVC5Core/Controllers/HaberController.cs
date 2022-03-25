using AutoMapper;
using CORE.ApplicationCommon.DTOS.CategoryDTO;
using GazeteKapiMVC5Core.Core.Extensions;
using GazeteKapiMVC5Core.Models.Account;
using GazeteKapiMVC5Core.Models.Category;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SERVICE.Engine.Interfaces;
using SERVICES.Engine.Interfaces;
using System;
using System.Collections.Generic;
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

        public HaberController(ICategoryService categoryService, IMapper mapper, ILogService logSerivce)
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _logService = logSerivce;
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
