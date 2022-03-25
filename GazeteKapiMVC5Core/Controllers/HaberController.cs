using AutoMapper;
using CORE.ApplicationCommon.DTOS.CategoryDTO;
using GazeteKapiMVC5Core.Core.Extensions;
using GazeteKapiMVC5Core.Models.Account;
using GazeteKapiMVC5Core.Models.Category;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SERVICES.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GazeteKapiMVC5Core.Controllers
{
    public class HaberController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        public HaberController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        #region Categories

        [HttpGet]
        public IActionResult Kategoriler()
        {
            return View(_mapper.Map<List<CategoryListItemDto>, List<CategoryListViewModel>>(_categoryService.GetAllCategory()));
        }

        [HttpGet]
        public IActionResult KategoriOlustur()
        {
            var list = _mapper.Map<List<CategoryListItemDto>, List<CategoryListViewModel>>(_categoryService.GetParentCategoryList());
            ViewBag.CategoriesParent = new SelectList(list, "Id", "CategoryName");
            return View(new CategoryCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> KategoriOlustur(CategoryCreateViewModel category)
        {
            AccountEditViewModel yoneticiGetir = SessionExtensionMethod.GetObject<AccountEditViewModel>(HttpContext.Session, "user");
            category.UserId = yoneticiGetir.Id;
            if (ModelState.IsValid)
            {
                if (!await _categoryService.CategoryIfExists(category.CategoryName))
                {
                    if (await _categoryService.CreateCategory(_mapper.Map<CategoryCreateViewModel, CategoryDto>(category)))
                    {
                        return RedirectToAction(nameof(Kategoriler));
                    }
                }
            }
            return View(category);
        }

        public IActionResult KategoriSil(int id)
        {
            if (!_categoryService.DeleteCategoryById(id))
            {
                ViewBag.Message = "Kategori silinirken bir hata meydana geldi!";
                return RedirectToAction(nameof(Kategoriler));
            }
            else
            {
                ViewBag.Message = "Kategori başarıyla silindi!";
                return RedirectToAction(nameof(Kategoriler));
            }
        }

        [HttpGet]
        public IActionResult KategoriDuzenle(int id)
        {
            var categories = _mapper.Map<CategoryDto, CategoryEditViewModel>(_categoryService.GetCategoryById(id));
            var list = _mapper.Map<List<CategoryListItemDto>, List<CategoryListViewModel>>(_categoryService.GetParentCategoryList());
            ViewBag.CategoriesParent = new SelectList(list, "Id", "CategoryName", categories.Id);
            return View(categories);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> KategoriDuzenle(CategoryEditViewModel category)
        {
            if (ModelState.IsValid)
            {
                if (await _categoryService.CategoryIfExists(category.CategoryName, category.Id))
                {
                    ModelState.AddModelError("CategoryName", "Bu isimde kategori zaten bulunuyor!");
                    return View(category);
                }
                else
                {
                    if (await _categoryService.UpdateCategory(_mapper.Map<CategoryEditViewModel, CategoryDto>(category)))
                    {
                        return RedirectToAction(nameof(Kategoriler));
                    }
                }
            }
            return View(category);
        }

        public async Task<IActionResult> KategoriDurumDuzenle(int id)
        {
            if (await _categoryService.EditIsActiveById(id))
            {
                return RedirectToAction(nameof(Kategoriler));
            }
            else
            {
                ViewBag.Message = "Bir hata oluştu";
                return RedirectToAction(nameof(Kategoriler));
            }
        }

        #endregion

        #region Haberler

        //[RoleAuthorize("Haberler")]
        public IActionResult Haberler()
        {
            return View();
        }

        #endregion
    }
}
