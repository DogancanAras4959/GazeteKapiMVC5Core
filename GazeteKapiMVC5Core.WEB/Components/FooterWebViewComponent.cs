using AutoMapper;
using CORE.ApplicationCommon.DTOS.CategoryDTO;
using CORE.ApplicationCommon.DTOS.SetingsDTO;
using GazeteKapiMVC5Core.WEB.ViewModels.Categories;
using GazeteKapiMVC5Core.WEB.ViewModels.Settings;
using Microsoft.AspNetCore.Mvc;
using SERVICE.Engine.Interfaces;
using SERVICES.Engine.Interfaces;
using System.Collections.Generic;

namespace GazeteKapiMVC5Core.WEB.Components
{
    public class FooterWebViewComponent : ViewComponent
    {
        private readonly ISettingService _siteSetting;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        public FooterWebViewComponent(ISettingService settingService, ICategoryService categoryService, IMapper mapper)
        {
            _siteSetting = settingService;
            _categoryService = categoryService;
            _mapper = mapper;
        }

        public IViewComponentResult Invoke()
        {
            var siteSettingGet = _mapper.Map<SettingsDto, SettingsEditViewModelWeb>(_siteSetting.getSettings(1));

            List<CategoryListViewModelWeb> categoryList = _mapper.Map<List<CategoryListItemDto>, List<CategoryListViewModelWeb>>(_categoryService.GetAllCategory());

            ViewBag.Categories = categoryList;

            return View(siteSettingGet);
        }
    }
}
