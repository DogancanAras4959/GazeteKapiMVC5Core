using CORE.ApplicationCommon.DTOS.AuthorizeDTO;
using CORE.ApplicationCommon.DTOS.AuthorizeRoleDto;
using CORE.ApplicationCommon.DTOS.RoleDTO;
using CORE.ApplicationCore.BackEndExceptionHandler;
using CORE.ApplicationCore.UnitOfWork;
using GazeteKapiMVC5Core.DataAccessLayer.Models;
using GazeteKapiMVC5Core.DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using SERVICES.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICES.Engine.Engines
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork<NewsAppContext> _unitOfWork;

        public RoleService(IUnitOfWork<NewsAppContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool DeleteRoleById(int id)
        {
            Task<int> result = _unitOfWork.GetRepository<Roles>().DeleteAsync(new Roles { Id = id });
            return Convert.ToBoolean(result.Result);
        }

        public async Task<bool> EditIsActive(int id)
        {
            Roles getRoles = _unitOfWork.GetRepository<Roles>().FindAsync(x => x.Id == id).Result;
            if (getRoles.IsActive == false)
            {
                getRoles.IsActive = true;
                Roles model = await _unitOfWork.GetRepository<Roles>().UpdateAsync(getRoles);
                return getRoles != null;
            }
            else
            {
                getRoles.IsActive = false;
                Roles model = await _unitOfWork.GetRepository<Roles>().UpdateAsync(getRoles);
                return getRoles != null;
            }
        }

        public List<AuthorizeListItemDto> GetAllAuthorizeList()
        {
            IEnumerable<Authorizes> roles = _unitOfWork.GetRepository<Authorizes>().Filter(null, x => x.OrderBy(y => y.Id), "", 1, 30);

            return roles.Select(x => new AuthorizeListItemDto
            {
                Id = x.Id,
                AuthorizeCode = x.AuthorizeCode,
                AuthorizeName = x.AuthorizeName

            }).ToList();
        }

        public List<AuthorizeRoleListItemDto> GetAllAuthorizeRoleListItems()
        {
            IEnumerable<RoleAuthorize> roleAuthorizes = _unitOfWork.GetRepository<RoleAuthorize>().Filter(null, x => x.OrderBy(y => y.Id), "role,authroize", 1, 200);
            return roleAuthorizes.Select(x => new AuthorizeRoleListItemDto
            {
                Id = x.Id,
                AuthorizeId = x.AuthorizeId,
                RoleId = x.RoleId,
                IsChecked = x.IsChecked

            }).ToList();
        }

        public List<RoleListItemDto> GetAllRole()
        {
            IEnumerable<Roles> roles = _unitOfWork.GetRepository<Roles>().Filter(null, x => (IOrderedQueryable<Roles>)x.OrderBy(y => y.RoleName));

            return roles.Select(x => new RoleListItemDto
            {
                Id = x.Id,
                RoleName = x.RoleName,
                IsActive = x.IsActive,
                CreatedTime = x.CreatedTime,
                UpdatedTime = x.UpdatedTime
            }).ToList();
        }

        public AuthorizeDto GetAuthorizeById(int id)
        {
            Authorizes authorize = _unitOfWork.GetRepository<Authorizes>().FindAsync(x => x.Id == id).Result;
            if (authorize == null)
            {
                return new AuthorizeDto();
            }

            return new AuthorizeDto
            {
                AuthorizeCode = authorize.AuthorizeCode,
                AuthorizeName = authorize.AuthorizeName,
            };
        }

        public RoleDto GetRoleById(int id)
        {
            Roles role = _unitOfWork.GetRepository<Roles>().FindAsync(x => x.Id == id).Result;
            if (role == null)
            {
                return new RoleDto();
            }

            return new RoleDto
            {
                RoleName = role.RoleName,
                IsActive = role.IsActive,
                CreatedTime = role.CreatedTime,
                UpdatedTime = role.UpdatedTime,
                Id = role.Id
            };
        }

        public async Task<bool> InsertAuthorizeInRole(AuthorizeRoleDto dto)
        {
            RoleAuthorize getRoles = _unitOfWork.GetRepository<RoleAuthorize>().FindAsync(x => x.AuthorizeId == dto.AuthorizeId && x.RoleId == dto.RoleId).Result;
            if (getRoles != null)
            {
                if (getRoles.IsChecked == false)
                {
                    getRoles.IsChecked = true;
                    RoleAuthorize model = await _unitOfWork.GetRepository<RoleAuthorize>().UpdateAsync(getRoles);
                    return getRoles != null;
                }
                else
                {
                    getRoles.IsChecked = false;
                    RoleAuthorize model = await _unitOfWork.GetRepository<RoleAuthorize>().UpdateAsync(getRoles);
                    return getRoles != null;
                }
            }
            else
            {
                RoleAuthorize newRoleAuthorize = await _unitOfWork.GetRepository<RoleAuthorize>().AddAsync(new RoleAuthorize
                {
                    RoleId = dto.RoleId,
                    AuthorizeId = dto.AuthorizeId,
                    IsChecked = true,

                });
                return newRoleAuthorize != null && newRoleAuthorize.Id != 0;

            }

        }

        public async Task<bool> InsertRole(RoleDto model)
        {

            Roles newRole = await _unitOfWork.GetRepository<Roles>().AddAsync(new Roles
            {
                RoleName = model.RoleName,
                CreatedTime = DateTime.Now,
                UpdatedTime = DateTime.Now,
                IsActive = true
            });

            return newRole != null && newRole.Id != 0;
        }

        public async Task<bool> RemoveAuthorizeInRole(AuthorizeRoleDto dto)
        {
            RoleAuthorize getRoles = _unitOfWork.GetRepository<RoleAuthorize>().FindAsync(x => x.AuthorizeId == dto.AuthorizeId && x.RoleId == dto.RoleId).Result;
            getRoles.IsChecked = false;

            RoleAuthorize model = await _unitOfWork.GetRepository<RoleAuthorize>().UpdateAsync(getRoles);
            return getRoles != null;
        }

        public async Task<bool> RoleIsExists(string roleName) =>
        await _unitOfWork.GetRepository<Roles>().FindAsync(x => x.RoleName == roleName) != null;

        public async Task<bool> RoleIsExists(int id, string roleName) =>
            await _unitOfWork.GetRepository<Roles>().FindAsync(x => x.Id != id && x.RoleName == roleName) != null;

        public async Task<bool> UpdateRole(RoleDto model)
        {
            Roles roleGet = await _unitOfWork.GetRepository<Roles>().FindAsync(x => x.Id == model.Id);

            Roles getRole = await _unitOfWork.GetRepository<Roles>().UpdateAsync(new Roles
            {
                Id = model.Id,
                RoleName = model.RoleName,
                UpdatedTime = DateTime.Now,
                CreatedTime = roleGet.CreatedTime,
                IsActive = roleGet.IsActive
            });

            return getRole != null;
        }

    }
}
