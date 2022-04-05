using AutoMapper;
using CORE.ApplicationCommon.DTOS.CategoryDTO;
using GazeteKapiMVC5Core.Models.Category;
using Microsoft.AspNetCore.Mvc;
using SERVICES.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GazeteKapiMVC5Core.WEB.Components
{
    public class HeaderWebViewComponent : ViewComponent
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public HeaderWebViewComponent(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        public IViewComponentResult Invoke()
        {
            List<CategoryListViewModel> categoryList = null;
            categoryList = _mapper.Map<List<CategoryListItemDto>, List<CategoryListViewModel>>(_categoryService.GetAllCategory());
            ViewBag.CategoryList = categoryList;
            return View();
        }
    }
}
