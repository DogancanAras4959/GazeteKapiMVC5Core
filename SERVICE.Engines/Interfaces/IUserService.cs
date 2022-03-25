using CORE.ApplicationCommon.DTOS.AccountDTO;
using DOMAIN.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SERVICES.Engine.Interfaces
{
    public interface IUserService
    {
        Task<bool> Register(UserDto model);
        Task<bool> Login(string userName, string password);
        Task<bool> UserIsExists(string userName);
        Task<bool> UserIsExists(int Id, string userName);
        UserDto GetUserById(int Id);
        List<UserListItemDto> GetAllUsers();
        Task<bool> UpdateUser(UserDto userDto);

        Task<bool> EditIsActive(int id);
        bool DeleteUserById(int id);

        UserBaseDto GetUserByName(string name);
    }
}
