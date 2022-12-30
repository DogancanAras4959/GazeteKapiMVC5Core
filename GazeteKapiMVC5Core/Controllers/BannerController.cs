using AutoMapper;
using CORE.ApplicationCommon.DTOS.BannersDTO;
using CORE.ApplicationCommon.DTOS.MagazineBannerDTO;
using CORE.ApplicationCommon.Helpers;
using GazeteKapiMVC5Core.Core.Extensions;
using GazeteKapiMVC5Core.Models.Banners;
using GazeteKapiMVC5Core.Models.MagazineBanner;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SERVICE.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GazeteKapiMVC5Core.Controllers
{
    public class BannerController : Controller
    {
        private readonly IMagazineBannerService _magazineBannerService;
        private readonly IBannerService _bannerService;
        private readonly IBannerRotateService _bannerRotateService;
        private readonly IMapper _mapper;
        public BannerController(IMagazineBannerService magazineBannerService, IBannerRotateService bannerRotateService, IMapper mapper, IBannerService bannerService)
        {
            _magazineBannerService = magazineBannerService;
            _bannerRotateService = bannerRotateService;
            _bannerService = bannerService;
            _mapper = mapper;
        }

        #region Dergi

        [HttpGet]
        [CheckRoleAuthorize]
        public IActionResult magazineBannerList()
        {
            try
            {
                List<MagazineBannerListViewModel> magazineList = _mapper.Map<List<MagazineBannerListItemDto>, List<MagazineBannerListViewModel>>(_magazineBannerService.magazineBannerList());
                return View(magazineList);
            }
            catch (Exception ex)
            {
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }
        }

        [HttpGet]
        public IActionResult DergiBannerOlustur()
        {
            return View();
        }

        [HttpGet]
        public IActionResult DergiBannerGuncelle(int Id)
        {
            var banner = _mapper.Map<MagazineBannerDto, MagazineBannerEditViewModel>(_magazineBannerService.getMagazine(Id));
            return View(banner);
        }

        [HttpPost]
        public async Task<IActionResult> DergiBannerEkle(MagazineBannerCreateViewModel magazineBanner, IFormFile file)
        {
            try
            {

                if (ModelState.IsValid)
                {
                   
                    if (file != null)
                    {

                        magazineBanner.BannerImage = SaveImageProcess.ImageInsert(file, "Admin");
                    }

                    else
                    {
                        magazineBanner.BannerImage = "categorydefault.jpg";
                    }

                    if (await _magazineBannerService.createMagazineBanner(_mapper.Map<MagazineBannerCreateViewModel, MagazineBannerDto>(magazineBanner)))
                    {
                        return RedirectToAction(nameof(magazineBannerList));
                    }

                    else
                    {
                        return RedirectToAction(nameof(magazineBannerList));
                    }

                }
                return View(magazineBanner);
            }
            catch (Exception ex)
            {

                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }

        }

        [CheckRoleAuthorize]
        public IActionResult DergiBannerSil(int id)
        {
            try
            {
                if (!_magazineBannerService.DeleteBanner(id))
                {
                    return RedirectToAction(nameof(magazineBannerList));
                }
                else
                {
                    return RedirectToAction(nameof(magazineBannerList));
                }
            }
            catch (Exception ex)
            {

                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }

        }

        [HttpPost]
        public async Task<IActionResult> DergiBannerDuzenle(MagazineBannerEditViewModel banner, IFormFile file)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (file != null)
                    {

                        banner.BannerImage = SaveImageProcess.ImageInsert(file, "Admin");
                    }
                    else
                    {
                        banner.BannerImage = "categorydefault.jpg";
                    }

                    if (await _magazineBannerService.editMagazineBannerDto(_mapper.Map<MagazineBannerEditViewModel, MagazineBannerDto>(banner)))
                    {
                        return RedirectToAction(nameof(magazineBannerList));
                    }
                    else
                    {
                        return RedirectToAction(nameof(magazineBannerList));
                    }
                }
                return View(banner);
            }
            catch (Exception ex)
            {
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }

        }

        [CheckRoleAuthorize]
        public async Task<IActionResult> DergiBannerAktifEt(int id)
        {
            try
            {
                if (await _magazineBannerService.IsActiveEnabledMagazine(id))
                {
                    return RedirectToAction(nameof(magazineBannerList));
                }
                else
                {
                    return RedirectToAction(nameof(magazineBannerList));
                }
            }
            catch (Exception ex)
            {
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }

        }

        #endregion

        #region Reklamlar

        [HttpGet]
        [CheckRoleAuthorize]
        public IActionResult reklamlar()
        {
            var listBanner = _mapper.Map<List<BannerListItemDto>, List<BannerListViewModel>>(_bannerService.BannerList());
            return View(listBanner);
        }

        [HttpGet]
        [CheckRoleAuthorize]
        public IActionResult reklamEkle()
        {
            ViewBag.BannersRotate = new SelectList(_bannerRotateService.bannersRotateList(),"Id","RotateName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> reklamolustur(BannerCreateViewModel model, IFormFile file)
        {
            try
            {
                if (file != null)
                {
                    model.BannerImage = SaveImageProcess.ImageInsert(file, "Admin");

                    if (await _bannerService.createBanner(_mapper.Map<BannerCreateViewModel, BannerDto>(model)))
                    {
                        return RedirectToAction(nameof(reklamlar));
                    }
                    else
                    {
                        return RedirectToAction(nameof(reklamlar));
                    }
                }
                else
                {
                    return RedirectToAction(nameof(reklamEkle));
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(reklamEkle));
            }
        }

        [HttpGet]
        [CheckRoleAuthorize]
        public IActionResult reklamDuzenle(int Id)
        {
            var getBanner = _mapper.Map<BannerDto, BannerEditViewModel>(_bannerService.getBanner(Id));
            ViewBag.BannersRotate = new SelectList(_bannerRotateService.bannersRotateList(), "Id", "RotateName");
            return View(getBanner);
        }

        [HttpPost]
        public async Task<IActionResult> reklamGuncelle(BannerEditViewModel model, IFormFile file)
        {
            try
            {
                var getBanner = _mapper.Map<BannerDto, BannerEditViewModel>(_bannerService.getBanner(model.Id));
                getBanner.IsActive = true;
                getBanner.Link = model.Link;
                getBanner.UpdatedTime = DateTime.Now;
                getBanner.RotateId = model.RotateId;
                getBanner.BannerName = model.BannerName;

                if (file != null)
                {
                    model.BannerImage = SaveImageProcess.ImageInsert(file, "Admin");
                    getBanner.BannerImage = model.BannerImage;

                    if (await _bannerService.editBannerDto(_mapper.Map<BannerEditViewModel, BannerDto>(getBanner)))
                    {
                        return RedirectToAction("reklamDuzenle", "Banner", new { Id = model.Id });
                    }
                    else
                    {
                        return RedirectToAction("reklamDuzenle", "Banner", new { Id = model.Id });
                    }
                }
                else
                {
                    if (await _bannerService.editBannerDto(_mapper.Map<BannerEditViewModel, BannerDto>(model)))
                    {
                        return RedirectToAction("reklamDuzenle", "Banner", new { Id = model.Id });
                    }
                    else
                    {
                        return RedirectToAction("reklamDuzenle", "Banner", new { Id = model.Id });
                    }
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("reklamDuzenle", "Banner", new { Id = model.Id });

            }
        }

        [HttpGet]
        [CheckRoleAuthorize]
        public IActionResult reklamSil(int Id)
        {
            try
            {
                if (!_bannerService.DeleteBanner(Id))
                {
                    return RedirectToAction(nameof(reklamlar));
                }
                else
                {
                    return RedirectToAction(nameof(reklamlar));
                }
            }
            catch (Exception ex)
            {

                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }
        }

        [HttpGet]
        [CheckRoleAuthorize]
        public async Task<IActionResult> reklamDurumDuzenle(int Id)
        {
            try
            {
                if (await _bannerService.IsActiveEnabledBanner(Id))
                {
                    return RedirectToAction(nameof(reklamlar));
                }
                else
                {
                    return RedirectToAction(nameof(reklamlar));
                }
            }
            catch (Exception ex)
            {
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }
        }

        #endregion
    }
}
