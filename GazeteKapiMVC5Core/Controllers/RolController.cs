using AutoMapper;
using CORE.ApplicationCommon.DTOS.AuthorizeDTO;
using CORE.ApplicationCommon.DTOS.AuthorizeRoleDto;
using CORE.ApplicationCommon.DTOS.RoleDTO;
using GazeteKapiMVC5Core.Core.Extensions;
using GazeteKapiMVC5Core.Models.Account;
using GazeteKapiMVC5Core.Models.Authorize;
using GazeteKapiMVC5Core.Models.AuthorizeRole;
using GazeteKapiMVC5Core.Models.Role;
using Microsoft.AspNetCore.Mvc;
using SERVICE.Engine.Interfaces;
using SERVICES.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GazeteKapiMVC5Core.Controllers
{
    public class RolController : Controller
    {
        #region Fields / Constructre

        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;
        private readonly ILogService _logService;

        public RolController(IRoleService roleService, IMapper mapper, ILogService logService)
        {
            _roleService = roleService;
            _mapper = mapper;
            _logService = logService;
        }

        #endregion

        //[RoleAuthorize("Roller")]
        public async Task<IActionResult> Roller()
        {
            try
            {
                await CreateModeratorLog("Başarılı", "Sayfa Girişi", "Roller", "Rol", "Sayfa girişi başarıyla gerçekleşti");
                return View(_mapper.Map<List<RoleListItemDto>, List<RolesListViewModel>>(_roleService.GetAllRole()));
            }
            catch (Exception ex)
            {
                string detay = "Sistemden kaynaklı bir hata meydana geldi: " + ex.ToString();
                await CreateModeratorLog("Sistem Hatası", "Sayfa Girişi", "Roller", "Rol", detay);
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }
        }

        [HttpGet]
        //[RoleAuthorize("RolOlustur")]
        public async Task<IActionResult> RolOlustur()
        {
            try
            {
                await CreateModeratorLog("Başarılı", "Sayfa Girişi", "RolOlustur", "Rol", "Sayfa girişi başarıyla gerçekleşti");
                return View(new RoleCreateViewModel());
            }
            catch (Exception ex)
            {
                string detay = "Sistemden kaynaklı bir hata meydana geldi: " + ex.ToString();
                await CreateModeratorLog("Sistem Hatası", "Sayfa Girişi", "RolOlustur", "Rol", detay);
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> RolOlustur(RoleCreateViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!await _roleService.RoleIsExists(model.RoleName))
                    {
                        if (await _roleService.InsertRole(_mapper.Map<RoleCreateViewModel, RoleDto>(model)))
                        {
                            await CreateModeratorLog("Başarılı", "Ekleme", "RolOlustur", "Rol", "Kullanıcı için rol başarıyla oluşturuldu");
                            return RedirectToAction(nameof(Roller));
                        }
                        else
                        {
                            await CreateModeratorLog("Başarısız", "Ekleme", "RolOlustur", "Rol", "Rol oluşturulurken bir hata meydana geldi");
                            return RedirectToAction(nameof(Roller));
                        }
                    }
                    else
                    {
                        await CreateModeratorLog("Başarısız", "Ekleme", "RolOlustur", "Rol", "Oluşturulmak istenen rol zaten sistemde bulunuyor");
                        return RedirectToAction(nameof(Roller));
                    }
                }
                return RedirectToAction(nameof(Roller));
            }
            catch (Exception ex)
            {
                string detay = "Sistemden kaynaklı bir hata meydana geldi: " + ex.ToString();
                await CreateModeratorLog("Sistem Hatası", "Ekleme", "RolOlustur", "Rol", detay);
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }

        }

        [HttpGet]
        //[RoleAuthorize("RolDuzenle")]
        public async Task<IActionResult> RolDuzenle(int id)
        {
            try
            {
                var rol = _mapper.Map<RoleDto, RoleEditViewModel>(_roleService.GetRoleById(id));
                await CreateModeratorLog("Başarılı", "Sayfa Girişi", "RolDuzenle", "Rol", "Rol düzenleme sayfasına başarıyla girildi");
                return View(rol);
            }
            catch (Exception ex)
            {
                string detay = "Sistemden kaynaklı bir hata meydana geldi: " + ex.ToString();
                await CreateModeratorLog("Sistem Hatası", "Sayfa Girişi", "RolDuzenle", "Rol", detay); 
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> RolDuzenle(RoleEditViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (await _roleService.UpdateRole(_mapper.Map<RoleEditViewModel, RoleDto>(model)))
                    {
                        await CreateModeratorLog("Başarılı", "Güncelleme", "RolDuzenle", "Rol", "Rol güncellemesi başarılı!");
                        return RedirectToAction(nameof(Roller));
                    }
                    else
                    {
                        await CreateModeratorLog("Başarısız", "Güncelleme", "RolDuzenle", "Rol", "Rol güncellenirken bir hata meydana geldi");
                        return View(model);

                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {
                string detay = "Sistemden kaynaklı bir hata meydana geldi: " + ex.ToString();
                await CreateModeratorLog("Sistem Hatası", "Güncelleme", "RolDuzenle", "Rol", detay);
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }

        }

        //[RoleAuthorize("RolSil")]
        public async Task<IActionResult> RolSil(int id)
        {
            try
            {
                if (!_roleService.DeleteRoleById(id))
                {
                    await CreateModeratorLog("Başarısız", "Silme", "RolSil", "Rol", "Rol silinirken bir hata meydana geldi");
                    return RedirectToAction(nameof(Roller));
                }
                else
                {
                    await CreateModeratorLog("Başarılı", "Silme", "RolSil", "Rol", "Kullanıcı rolü başarıyla sistemden kaldırıldı");
                    return RedirectToAction(nameof(Roller));
                }
            }
            catch (Exception ex)
            {
                string detay = "Sistemden kaynaklı bir hata meydana geldi: " + ex.ToString();
                await CreateModeratorLog("Sistem Hatası", "Silme", "RolSil", "Rol", detay);
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }

        }

        //[RoleAuthorize("DurumDuzenleRol")]
        public async Task<IActionResult> DurumDuzenle(int id)
        {
            try
            {
                if (await _roleService.EditIsActive(id))
                {
                    await CreateModeratorLog("Başarılı", "Güncelleme", "DurumDuzenle", "Rol", "Kullanıcı rolünün durumu başarıyla değiştirildi!");

                    return RedirectToAction(nameof(Roller));
                }
                else
                {
                    await CreateModeratorLog("Başarısız", "Güncelleme", "DurumDuzenle", "Rol", "Kullanıcı rolü değiştirilirken bir hata meydana geldi!");
                    return RedirectToAction(nameof(Roller));
                }
            }
            catch (Exception ex)
            {
                string detay = "Sistemden kaynaklı bir hata meydana geldi: " + ex.ToString();
                await CreateModeratorLog("Sistem Hatası", "Güncelleme", "DurumDuzenle", "Rol", detay);
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }

        }

        //[RoleAuthorize("RolDetay")]
        public async Task<IActionResult> RolDetay(int id)
        {
            try
            {
                var getRole = _mapper.Map<RoleDto, RoleEditViewModel>(_roleService.GetRoleById(id));
                ViewBag.ListAuthorize = AuthorizeList();
                ViewBag.ListAuthorizeRole = AuthorizeRoleList();
                await CreateModeratorLog("Başarılı", "Sayfa Girişi", "RolDetay", "Rol", "Rol detayı sayfasına başarıyla giriş yapıldı");
                return View(getRole);
            }
            catch (Exception ex)
            {
                string detay = "Sistemden kaynaklı bir hata meydana geldi: " + ex.ToString();
                await CreateModeratorLog("Sistem Hatası", "Sayfa Girişi", "RolDetay", "Rol", detay);
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }

        }

        [HttpPost]
        public async Task InsertAuthorizeInRole(int authorizeId, int roleId)
        {
            RoleBaseDto roledto = _roleService.GetRoleById(roleId);
            AuthorizeBaseDto authorizedto = _roleService.GetAuthorizeById(authorizeId);
            AuthorizeRoleCreateModel dto = new AuthorizeRoleCreateModel();
            dto.RoleId = roleId;
            dto.AuthorizeId = authorizeId;
            dto.isChecked = false;

            if (roledto != null)
            {
                if (authorizedto != null)
                {
                    await _roleService
                        .InsertAuthorizeInRole(_mapper.Map<AuthorizeRoleCreateModel, AuthorizeRoleDto>(dto));
                }
            }
        }

        [HttpPost]
        public async Task RemoveAuthorizeInRole(int authorizeId, int roleId, int id)
        {
            RoleBaseDto roledto = _roleService.GetRoleById(roleId);
            AuthorizeBaseDto authorizedto = _roleService.GetAuthorizeById(authorizeId);
            AuthorizeRoleEditModel dto = new AuthorizeRoleEditModel();
            dto.RoleId = roleId;
            dto.AuthorizeId = authorizeId;
            dto.isChecked = true;

            if (roledto != null)
            {
                if (authorizedto != null)
                {
                    await _roleService.RemoveAuthorizeInRole(_mapper.Map<AuthorizeRoleEditModel, AuthorizeRoleDto>(dto));
                }
            }
        }

        #region ExtendsMethods

        public List<AuthorizeListViewModel> AuthorizeList()
        {
            return _mapper.Map<List<AuthorizeListItemDto>, List<AuthorizeListViewModel>>(_roleService.GetAllAuthorizeList());
        }
        public List<AuthorizeRoleListViewModel> AuthorizeRoleList()
        {
            return _mapper.Map<List<AuthorizeRoleListItemDto>, List<AuthorizeRoleListViewModel>>(_roleService.GetAllAuthorizeRoleListItems());
        }
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
