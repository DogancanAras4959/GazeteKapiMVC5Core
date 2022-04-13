using AutoMapper;
using CORE.ApplicationCommon.DTOS.SetingsDTO;
using GazeteKapiMVC5Core.WEB.ViewModels.Settings;
using Microsoft.AspNetCore.Mvc;
using SERVICE.Engine.Interfaces;

namespace GazeteKapiMVC5Core.WEB.Components
{
    public class FooterWebViewComponent : ViewComponent
    {
        private readonly ISettingService _siteSetting;
        private readonly IMapper _mapper;
        public FooterWebViewComponent(ISettingService settingService, IMapper mapper)
        {
            _siteSetting = settingService;
            _mapper = mapper;
        }

        public IViewComponentResult Invoke()
        {
            var siteSettingGet = _mapper.Map<SettingsDto, SettingsEditViewModelWeb>(_siteSetting.getSettings(1));
            return View(siteSettingGet);
        }
    }
}
