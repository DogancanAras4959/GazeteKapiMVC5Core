using AutoMapper;
using CORE.ApplicationCommon.DTOS.AccountDTO;
using CORE.ApplicationCommon.DTOS.AuthorizeDTO;
using CORE.ApplicationCommon.DTOS.AuthorizeRoleDto;
using CORE.ApplicationCommon.DTOS.RoleDTO;
using CORE.ApplicationCommon.DTOS.SetingsDTO;
using GazeteKapiMVC5Core.Core.Extensions;
using GazeteKapiMVC5Core.Models.Account;
using GazeteKapiMVC5Core.Models.Authorize;
using GazeteKapiMVC5Core.Models.AuthorizeRole;
using GazeteKapiMVC5Core.Models.Role;
using GazeteKapiMVC5Core.Models.Settings;
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
        private readonly ISettingService _settingService;
        private readonly IUserService _userService;

        public RolController(IUserService userService, IRoleService roleService, IMapper mapper, ISettingService settingService)
        {
            _roleService = roleService;
            _mapper = mapper;
            _settingService = settingService;
            _userService = userService;
        }

        #endregion

        //[RoleAuthorize("Roller")]
        
        [CheckRoleAuthorize]
        public IActionResult Roller()
        {
            try
            {
                return View(_mapper.Map<List<RoleListItemDto>, List<RolesListViewModel>>(_roleService.GetAllRole()));
            }
            catch (Exception ex)
            {

                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }
        }

        [HttpGet]
        [CheckRoleAuthorize]
        //[RoleAuthorize("RolOlustur")]
        public IActionResult RolOlustur()
        {
            try
            {
                return View(new RoleCreateViewModel());
            }
            catch (Exception ex)
            {
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
                            return RedirectToAction(nameof(Roller));
                        }
                        else
                        {
                            return RedirectToAction(nameof(Roller));
                        }
                    }
                    else
                    {
                        return RedirectToAction(nameof(Roller));
                    }
                }
                return RedirectToAction(nameof(Roller));
            }
            catch (Exception ex)
            {
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }

        }

        [HttpGet]
        [CheckRoleAuthorize]
        //[RoleAuthorize("RolDuzenle")]
        public IActionResult RolDuzenle(int id)
        {
            try
            {
                var rol = _mapper.Map<RoleDto, RoleEditViewModel>(_roleService.GetRoleById(id));
                return View(rol);
            }
            catch (Exception ex)
            {
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
                        return RedirectToAction(nameof(Roller));
                    }
                    else
                    {
                        return View(model);

                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }

        }

        //[RoleAuthorize("RolSil")]
        
        [CheckRoleAuthorize]
        public IActionResult RolSil(int id)
        {
            try
            {
                AccountEditViewModel yoneticiGetir = SessionExtensionMethod.GetObject<AccountEditViewModel>(HttpContext.Session, "user");
                var user = _mapper.Map<UserDto, AccountEditViewModel>(_userService.GetUserById(yoneticiGetir.Id));

                if (user.RoleId != id)
                {
                    if (!_roleService.DeleteRoleById(id))
                    {
                        return RedirectToAction(nameof(Roller));
                    }
                    else
                    {
                        return RedirectToAction(nameof(Roller));
                    }
                }
                else
                {
                    TempData["mesaj"] = "Bu kullanıcı oturum açmış durumda bu rolü silemezsiniz";
                    return RedirectToAction(nameof(Roller));
                }
            }
            catch (Exception ex)
            {
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }

        }

        //[RoleAuthorize("DurumDuzenleRol")]
        [CheckRoleAuthorize]
        public async Task<IActionResult> DurumDuzenle(int id)
        {
            try
            {
                AccountEditViewModel yoneticiGetir = SessionExtensionMethod.GetObject<AccountEditViewModel>(HttpContext.Session, "user");
                var user = _mapper.Map<UserDto, AccountEditViewModel>(_userService.GetUserById(yoneticiGetir.Id));

                if (user.RoleId != id)
                {
                    if (await _roleService.EditIsActive(id))
                    {
                        return RedirectToAction(nameof(Roller));
                    }
                    else
                    {
                        return RedirectToAction(nameof(Roller));
                    }
                }
                else
                {
                    TempData["mesaj"] = "Bu kullanıcı oturum açmış durumda bu rolü pasifleştiremezsiniz";
                    return RedirectToAction(nameof(Roller));
                }
            }
            catch (Exception ex)
            {

                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }

        }

        //[RoleAuthorize("RolDetay")]
        [CheckRoleAuthorize]
        public IActionResult RolDetay(int id)
        {
            try
            {
                var getRole = _mapper.Map<RoleDto, RoleEditViewModel>(_roleService.GetRoleById(id));
                ViewBag.ListAuthorize = AuthorizeList();
                ViewBag.ListAuthorizeRole = AuthorizeRoleList();
                return View(getRole);
            }
            catch (Exception ex)
            {
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }

        }

        [HttpPost]
        public async Task InsertAuthorizeInRole(int authorizeId, int roleId)
        {
            RoleBaseDto roledto = _roleService.GetRoleById(roleId);
            AuthorizeBaseDto authorizedto = _roleService.GetAuthorizeById(authorizeId);
            AuthorizeRoleCreateModel dto = new AuthorizeRoleCreateModel
            {
                RoleId = roleId,
                AuthorizeId = authorizeId,
                isChecked = false
            };

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
        public async Task RemoveAuthorizeInRole(int authorizeId, int roleId)
        {
            RoleBaseDto roledto = _roleService.GetRoleById(roleId);
            AuthorizeBaseDto authorizedto = _roleService.GetAuthorizeById(authorizeId);
            AuthorizeRoleEditModel dto = new AuthorizeRoleEditModel
            {
                RoleId = roleId,
                AuthorizeId = authorizeId,
                isChecked = true
            };

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

        #endregion

    }
}
