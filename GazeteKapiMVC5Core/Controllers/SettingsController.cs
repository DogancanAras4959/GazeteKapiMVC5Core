using AutoMapper;
using CORE.ApplicationCommon.DTOS.PrivacyDTO.AboutUsDto;
using CORE.ApplicationCommon.DTOS.PrivacyDTO.PolicyDto;
using CORE.ApplicationCommon.DTOS.PrivacyDTO.PrivacyDto;
using CORE.ApplicationCommon.DTOS.PrivacyDTO.TermsOfUsDto;
using CORE.ApplicationCommon.DTOS.SetingsDTO;
using GazeteKapiMVC5Core.Core.Extensions;
using GazeteKapiMVC5Core.Models.Account;
using GazeteKapiMVC5Core.Models.Settings;
using GazeteKapiMVC5Core.Models.Site.AboutUsModel;
using GazeteKapiMVC5Core.Models.Site.PolicyModel;
using GazeteKapiMVC5Core.Models.Site.PrivacyModel;
using GazeteKapiMVC5Core.Models.Site.TermsOfUsModel;
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
        #region Fields / Constructure

        private readonly ISettingService _settingService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;
        public SettingsController(ISettingService settingService, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _settingService = settingService;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        #endregion

        #region SettingBasse

        [CheckRoleAuthorize]
        public IActionResult Ayarlar()
        {
            var getSiteSettings = _mapper.Map<SettingsDto, SettingsEditViewModel>(_settingService.getSettings(1));
            return View(getSiteSettings);
        }

        [CheckRoleAuthorize]
        public async Task<IActionResult> AyarlariDuzenle(SettingsEditViewModel model, IFormFile file, IFormFile fileFooter)
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

                    if (fileFooter != null)
                    {
                        string uploadfilename = Path.GetFileNameWithoutExtension(fileFooter.FileName);
                        string extension = Path.GetExtension(fileFooter.FileName);
                        uploadfilename = uploadfilename + DateTime.Now.ToString("yymmssfff") + extension;
                        var path = Path.Combine(this._webHostEnvironment.WebRootPath, "Files", uploadfilename);
                        var stream = new FileStream(path, FileMode.Create);
                        await fileFooter.CopyToAsync(stream);
                        model.FooterLogo = uploadfilename;
                    }

                    if (await _settingService.editSiteSettings(_mapper.Map<SettingsEditViewModel, SettingsDto>(model)))
                    {
                        return RedirectToAction("Ayarlar", "Settings");
                    }
                }

                return RedirectToAction("Ayarlar", "Settings");
            }
            catch (Exception)
            {
                return RedirectToAction("Ayarlar", "Settings");
            }
        }


        [CheckRoleAuthorize]
        public IActionResult GizlilikPolitikası()
        {
            var getPrivacy = _mapper.Map<PrivacyDto, PrivacyEditModel>(_settingService.getPrivacy(1));
            return View(getPrivacy);
        }

        [CheckRoleAuthorize]
        public async Task<IActionResult> GizlilikPolitikasiniDuzenle(PrivacyEditModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    AccountEditViewModel yoneticiGetir = SessionExtensionMethod.GetObject<AccountEditViewModel>(HttpContext.Session, "user");
                    model.UserId = yoneticiGetir.Id;
 
                    if (await _settingService.editPrivacy(_mapper.Map<PrivacyEditModel, PrivacyDto>(model)))
                    {
                        return RedirectToAction("GizlilikPolitikası", "Settings");
                    }
                }

                return RedirectToAction("GizlilikPolitikası", "Settings");
            }
            catch (Exception)
            {
                return RedirectToAction("GizlilikPolitikası", "Settings");
            }
        }

        [CheckRoleAuthorize]
        public IActionResult Hakkimizda()
        {
            var getAboutUs = _mapper.Map<AboutUsDto, AboutUsEditModel>(_settingService.getAboutUs(1));
            return View(getAboutUs);
        }

        [CheckRoleAuthorize]
        public async Task<IActionResult> HakkimizdaDuzenle(AboutUsEditModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    AccountEditViewModel yoneticiGetir = SessionExtensionMethod.GetObject<AccountEditViewModel>(HttpContext.Session, "user");
                    model.UserId = yoneticiGetir.Id;
   
                    if (await _settingService.editAboutUs(_mapper.Map<AboutUsEditModel, AboutUsDto>(model)))
                    {
                        return RedirectToAction("Hakkimizda", "Settings");
                    }
                }

                return RedirectToAction("Hakkimizda", "Settings");
            }
            catch (Exception)
            {
                return RedirectToAction("Hakkimizda", "Settings");
            }
        }

        [CheckRoleAuthorize]
        public IActionResult KullanimKosullari()
        {
            var getSiteSettings = _mapper.Map<TermsOfUsDto, TermsOfUsEditModel>(_settingService.getTermsOfUs(1));
            return View(getSiteSettings);
        }

        [CheckRoleAuthorize]
        public async Task<IActionResult> KullanimKosullariDuzenle(TermsOfUsEditModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    AccountEditViewModel yoneticiGetir = SessionExtensionMethod.GetObject<AccountEditViewModel>(HttpContext.Session, "user");
                    model.UserId = yoneticiGetir.Id;

                    if (await _settingService.editTermsOfUs(_mapper.Map<TermsOfUsEditModel, TermsOfUsDto>(model)))
                    {
                        return RedirectToAction("KullanimKosullari", "Settings");
                    }
                }

                return RedirectToAction("KullanimKosullari", "Settings");
            }
            catch (Exception)
            {
                return RedirectToAction("KullanimKosullari", "Settings");
            }
        }

        [CheckRoleAuthorize]
        public IActionResult YayinIlkeleri()
        {
            var getSiteSettings = _mapper.Map<StreamPolicyDto, StreamEditModel>(_settingService.getStreamPrivacy(2));
            return View(getSiteSettings);
        }

        [CheckRoleAuthorize]
        public async Task<IActionResult> YayinIlkeleriDuzenle(StreamEditModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    AccountEditViewModel yoneticiGetir = SessionExtensionMethod.GetObject<AccountEditViewModel>(HttpContext.Session, "user");
                    model.UserId = yoneticiGetir.Id;

                    if (await _settingService.editStreamPrivacy(_mapper.Map<StreamEditModel, StreamPolicyDto>(model)))
                    {
                        return RedirectToAction("YayinIlkeleri", "Settings");
                    }
                }

                return RedirectToAction("YayinIlkeleri", "Settings");
            }
            catch (Exception)
            {
                return RedirectToAction("YayinIlkeleri", "Settings");
            }
        }

        [CheckRoleAuthorize]
        public IActionResult Kunye()
        {
            var getSiteSettings = _mapper.Map<BrandPolicyDto, BrandEditModel>(_settingService.getBrandPrivacy(1));
            return View(getSiteSettings);
        }

        [CheckRoleAuthorize]
        public async Task<IActionResult> KunyeDuzenle(BrandEditModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    AccountEditViewModel yoneticiGetir = SessionExtensionMethod.GetObject<AccountEditViewModel>(HttpContext.Session, "user");
                    model.UserId = yoneticiGetir.Id;

                    if (await _settingService.editBrandPrivacy(_mapper.Map<BrandEditModel, BrandPolicyDto>(model)))
                    {
                        return RedirectToAction("Kunye", "Settings");
                    }
                }

                return RedirectToAction("Kunye", "Settings");
            }
            catch (Exception)
            {
                return RedirectToAction("Kunye", "Settings");
            }
        }

        [CheckRoleAuthorize]
        public IActionResult CerezPolitikasi()
        {
            var getSiteSettings = _mapper.Map<CookiePolicyDto, CookieEditModel>(_settingService.getCookiePrivacy(1));
            return View(getSiteSettings);
        }

        [CheckRoleAuthorize]
        public async Task<IActionResult> CerezPolitikasiDuzenle(CookieEditModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    AccountEditViewModel yoneticiGetir = SessionExtensionMethod.GetObject<AccountEditViewModel>(HttpContext.Session, "user");
                    model.UserId = yoneticiGetir.Id;

                    if (await _settingService.editCookiePrivacy(_mapper.Map<CookieEditModel, CookiePolicyDto>(model)))
                    {
                        return RedirectToAction("CerezPolitikasi", "Settings");
                    }
                }

                return RedirectToAction("CerezPolitikasi", "Settings");
            }
            catch (Exception)
            {
                return RedirectToAction("CerezPolitikasi", "Settings");
            }
        }

        #endregion

        #region Menus Edit / Setting

        [HttpGet]
        public IActionResult Menus(int? id)
        {
            if (id is null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            return View();
        }

        [HttpGet]
        public IActionResult PartialMenusList()
        {
            return View("PartialMenusList");
        }

        #endregion
    }
}
