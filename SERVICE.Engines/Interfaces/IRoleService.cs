using CORE.ApplicationCommon.DTOS.AuthorizeDTO;
using CORE.ApplicationCommon.DTOS.AuthorizeRoleDto;
using CORE.ApplicationCommon.DTOS.RoleDTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SERVICES.Engine.Interfaces
{
    public interface IRoleService
    {
        Task<bool> InsertRole(RoleDto model);
        Task<bool> RoleIsExists(string roleName);
        Task<bool> RoleIsExists(int id, string roleName);
        List<RoleListItemDto> GetAllRole();
        RoleDto GetRoleById(int id);
        Task<bool> UpdateRole(RoleDto model);
        bool DeleteRoleById(int id);
        Task<bool> EditIsActive(int id);

        #region Authorize
        List<AuthorizeListItemDto> GetAllAuthorizeList();
        AuthorizeDto GetAuthorizeById(int id);
        Task<bool> InsertAuthorizeInRole(AuthorizeRoleDto dto);

        Task<bool> RemoveAuthorizeInRole(AuthorizeRoleDto dto);

        List<AuthorizeRoleListItemDto> GetAllAuthorizeRoleListItems();

        #endregion
    }
}
