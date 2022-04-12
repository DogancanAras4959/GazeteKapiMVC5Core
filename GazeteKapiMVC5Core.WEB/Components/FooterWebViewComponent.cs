using AutoMapper;
using CORE.ApplicationCommon.DTOS.SetingsDTO;
using GazeteKapiMVC5Core.Models.Settings;
using Microsoft.AspNetCore.Mvc;
using SERVICE.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            var siteSettingGet = _mapper.Map<SettingsDto, SettingsEditViewModel>(_siteSetting.getSettings(1));
            return View(siteSettingGet);
        }
    }
}
