using CORE.ApplicationCommon.DTOS.AccountDTO;
using CORE.ApplicationCommon.Helpers.Cyrptography;
using CORE.ApplicationCore.BackEndExceptionHandler;
using CORE.ApplicationCore.UnitOfWork;
using GazeteKapiMVC5Core.DataAccessLayer;
using GazeteKapiMVC5Core.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using SERVICES.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICES.Engine.Engines
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork<NewsAppContext> _unitOfWork;

        public UserService(IUnitOfWork<NewsAppContext> unitOfWork, IBackEndExceptionHandler backEndExceptionHandler)
        {
            _unitOfWork = unitOfWork;
        }

        public bool DeleteUserById(int id)
        {
            Task<int> result = _unitOfWork.GetRepository<Users>().DeleteAsync(new Users { Id = id });
            return Convert.ToBoolean(result.Result);
        }

        public async Task<bool> EditIsActive(int id)
        {
            Users getUser = _unitOfWork.GetRepository<Users>().FindAsync(x => x.Id == id).Result;
            if (getUser.IsActive == false)
            {
                getUser.IsActive = true;
                Users model = await _unitOfWork.GetRepository<Users>().UpdateAsync(getUser);
                return getUser != null;
            }
            else
            {
                getUser.IsActive = false;
                Users model = await _unitOfWork.GetRepository<Users>().UpdateAsync(getUser);
                return getUser != null;
            }
        }

        public List<UserListItemDto> GetAllUsers()
        {
            IEnumerable<Users> users = _unitOfWork.GetRepository<Users>().Filter(null, x => x.OrderBy(y => y.Id),"roles",1,30);

            return users.Select(x => new UserListItemDto
            {
                Id = x.Id,
                UserName = x.UserName,
                DisplayName = x.DisplayName,
                EmailAdress = x.EmailAdress,
                Password = x.Password,
                IsActive = x.IsActive,
                UpdatedTime = x.UpdatedTime,
                CreatedTime = x.CreatedTime,
                ProfileImage = x.ProfileImage,
                role = x.roles

            }).ToList();
        }

        public UserDto GetUserById(int Id)
        {
            Users getUser = _unitOfWork.GetRepository<Users>().FindAsync(x => x.Id == Id).Result;
            string passwordDeCrypto = new Cyrpto().TryDecrypt(getUser.Password);

            if (getUser == null)
            {
                return new UserDto();
            }

            return new UserDto
            {
                UserName = getUser.UserName,
                DisplayName = getUser.DisplayName,
                Id = getUser.Id,
                CreatedTime = getUser.CreatedTime,
                UpdatedTime = getUser.UpdatedTime,
                IsActive = getUser.IsActive,
                Password = passwordDeCrypto,
                EmailAdress = getUser.EmailAdress,
                RoleId = getUser.RoleId,
                ProfileImage = getUser.ProfileImage,
                role = getUser.roles,
            };
        }

        public UserBaseDto GetUserByName(string name)
        {
            Users userGet = _unitOfWork.GetRepository<Users>().FindAsync(x => x.UserName == name).Result;
            string passwordDeCrypto = new Cyrpto().TryDecrypt(userGet.Password);

            if (userGet == null)
            {
                return new UserDto();
            }

            return new UserDto
            {
                UserName = userGet.UserName,
                DisplayName = userGet.DisplayName,
                Id = userGet.Id,
                CreatedTime = userGet.CreatedTime,
                UpdatedTime = userGet.UpdatedTime,
                IsActive = userGet.IsActive,
                Password = passwordDeCrypto,
                EmailAdress = userGet.EmailAdress,
                ProfileImage = userGet.ProfileImage,
                role = userGet.roles,
            };
        }

        public async Task<bool> Login(string userName, string password)
        {
            Users user = (Users) await _unitOfWork.GetRepository<Users>().FindAsync(x => x.UserName == userName && x.Password == password);
            if (user != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    
        public async Task<bool> Register(UserDto model)
        {
            Users newUser = await _unitOfWork.GetRepository<Users>().AddAsync(new Users 
            {
                DisplayName = model.DisplayName,
                IsActive = model.IsActive,
                Password = model.Password,
                UserName = model.UserName,
                CreatedTime = DateTime.Now,
                UpdatedTime = DateTime.Now,
                ProfileImage = model.ProfileImage,
                EmailAdress = model.EmailAdress,
                RoleId = model.RoleId,
            });

            return newUser != null && newUser.Id != 0;
        }

        public async Task<bool> UpdateUser(UserDto model)
        {
            Users userGet = await _unitOfWork.GetRepository<Users>().FindAsync(x => x.Id == model.Id);
            string passwordCrypto = "";

            if (model.Password != null)
            {
                passwordCrypto = new Cyrpto().TryEncrypt(model.Password);
            }
            else
            {
                passwordCrypto = userGet.Password;
            }

            if (model.ProfileImage == null)
            {
                model.ProfileImage = userGet.ProfileImage;
            }

            if (model.RoleId == 999)
            {
                model.RoleId = userGet.RoleId;
            }
        
            Users getUser = await _unitOfWork.GetRepository<Users>().UpdateAsync(new Users
            {
                Id = model.Id,
                UserName = model.UserName,
                DisplayName = model.DisplayName,
                EmailAdress = model.EmailAdress,
                Password = passwordCrypto,
                UpdatedTime = DateTime.Now,
                ProfileImage = model.ProfileImage,
                RoleId = model.RoleId,
                CreatedTime = userGet.CreatedTime,
                IsActive = model.IsActive,
            });

            return getUser != null;
        }

        public async Task<bool> UserIsExists(string userName) => 
            await _unitOfWork.GetRepository<Users>().FindAsync(x => x.UserName == userName) != null;

        public async Task<bool> UserIsExists(int id, string userName) =>
         await _unitOfWork.GetRepository<Users>().FindAsync(x => x.Id == id && x.UserName == userName) != null;

    }
}
