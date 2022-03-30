using AutoMapper;
using CORE.ApplicationCommon.DTOS.SetingsDTO;
using GazeteKapiMVC5Core.Models.Settings;
using Microsoft.AspNetCore.Mvc;
using SERVICE.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GazeteKapiMVC5Core.Controllers
{
    public class SettingsController : Controller
    {
        private readonly ISettingService _settingService;
        private readonly IMapper _mapper;
        public SettingsController(ISettingService settingService, IMapper mapper)
        {
            _settingService = settingService;
            _mapper = mapper;
        }
        public IActionResult Ayarlar()
        {
            var getSiteSettings = _mapper.Map<SettingsDto, SettingsBaseViewModel>(_settingService.getSettings(1));
            return View(getSiteSettings);
        }

        public async Task<IActionResult> AyarlariDuzenle()
        {
            try
            {

                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }

    }
}
