using AutoMapper;
using CORE.ApplicationCommon.DTOS.SetingsDTO;
using GazeteKapiMVC5Core.Core.Extensions;
using GazeteKapiMVC5Core.Models.Account;
using GazeteKapiMVC5Core.Models.Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SERVICE.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GazeteKapiMVC5Core.Controllers
{
    public class SettingsController : Controller
    {
        private readonly ISettingService _settingService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;
        public SettingsController(ISettingService settingService, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _settingService = settingService;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Ayarlar()
        {
            var getSiteSettings = _mapper.Map<SettingsDto, SettingsEditViewModel>(_settingService.getSettings(1));
            return View(getSiteSettings);
        }

        public async Task<IActionResult> AyarlariDuzenle(SettingsEditViewModel model, IFormFile file)
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
                        model.Logo = uploadfilename;
                    }
                    if (await _settingService.editSiteSettings(_mapper.Map<SettingsEditViewModel, SettingsDto>(model)))
                    {
                        return RedirectToAction("Ayarlar", "Settings");
                    }
                }

                return RedirectToAction("Ayarlar", "Settings");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Ayarlar", "Settings");
            }
        }

    }
}
