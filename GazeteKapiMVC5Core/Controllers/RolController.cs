using AutoMapper;
using CORE.ApplicationCommon.DTOS.AuthorizeDTO;
using CORE.ApplicationCommon.DTOS.AuthorizeRoleDto;
using CORE.ApplicationCommon.DTOS.RoleDTO;
using GazeteKapiMVC5Core.Core.Extensions;
using GazeteKapiMVC5Core.Models.Authorize;
using GazeteKapiMVC5Core.Models.AuthorizeRole;
using GazeteKapiMVC5Core.Models.Role;
using Microsoft.AspNetCore.Mvc;
using SERVICES.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GazeteKapiMVC5Core.Controllers
{
    public class RolController : Controller
    {
        #region MyRegion

        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        #endregion

        public RolController(IRoleService roleService, IMapper mapper)
        {
            _roleService = roleService;
            _mapper = mapper;
        }

        //[RoleAuthorize("Roller")]
        public IActionResult Roller()
        {
            return View(_mapper.Map<List<RoleListItemDto>, List<RolesListViewModel>>(_roleService.GetAllRole()));
        }

        [HttpGet]
        //[RoleAuthorize("RolOlustur")]
        public IActionResult RolOlustur()
        {
            return View(new RoleCreateViewModel());
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> RolOlustur(RoleCreateViewModel model)
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
                        ViewBag.Message = "Rol oluşturulamadı";
                        return RedirectToAction(nameof(Roller));
                    }
                }
                else
                {
                    ViewBag.Message = "Bu rol bulunuyor";
                    return RedirectToAction(nameof(Roller));
                }
            }
            return RedirectToAction(nameof(Roller));
        }

        [HttpGet]
        //[RoleAuthorize("RolDuzenle")]
        public IActionResult RolDuzenle(int id)
        {
            return View(_mapper.Map<RoleDto, RoleEditViewModel>(_roleService.GetRoleById(id)));
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> RolDuzenle(RoleEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _roleService.RoleIsExists(model.Id, model.RoleName))
                {
                    ModelState.AddModelError("UserName", "Bu isimde kullanıcı zaten bulunuyor!");
                    return View(model);
                }
                else
                {
                    if (await _roleService.UpdateRole(_mapper.Map<RoleEditViewModel, RoleDto>(model)))
                    {
                        return RedirectToAction(nameof(Roller));
                    }
                    else
                    {
                        ViewBag.Message = "Rol güncellenemedi! Daha sonra tekrar deneyin";
                        return View(model);

                    }
                }
            }
            return View(model);
        }

        //[RoleAuthorize("RolSil")]
        public IActionResult RolSil(int id)
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

        //[RoleAuthorize("DurumDuzenleRol")]
        public async Task<IActionResult> DurumDuzenle(int id)
        {
            if (await _roleService.EditIsActive(id))
            {
                return RedirectToAction(nameof(Roller));
            }
            else
            {
                ViewBag.Message = "Bir hata oluştu";
                return RedirectToAction(nameof(Roller));
            }
        }

        //[RoleAuthorize("RolDetay")]
        public IActionResult RolDetay(int id)
        {
            ViewBag.ListAuthorize = AuthorizeList();
            ViewBag.ListAuthorizeRole = AuthorizeRoleList();
            return View(_mapper.Map<RoleDto, RoleEditViewModel>(_roleService.GetRoleById(id)));
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

        #endregion
    }
}
