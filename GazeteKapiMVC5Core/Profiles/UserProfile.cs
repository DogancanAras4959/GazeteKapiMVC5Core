using AutoMapper;
using CORE.ApplicationCommon.DTOS.AccountDTO;
using CORE.ApplicationCommon.DTOS.AuthorizeDTO;
using CORE.ApplicationCommon.DTOS.AuthorizeRoleDto;
using CORE.ApplicationCommon.DTOS.RoleDTO;
using GazeteKapiMVC5Core.Models.Account;
using GazeteKapiMVC5Core.Models.Authorize;
using GazeteKapiMVC5Core.Models.AuthorizeRole;
using GazeteKapiMVC5Core.Models.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GazeteKapiMVC5Core.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserListItemDto, UserListViewModel>()
                .ForMember(x => x.roles, y => y.MapFrom(t => t.role));
          
            CreateMap<AccountCreateViewModel, UserDto>();
            CreateMap<UserDto, AccountEditViewModel>();
            CreateMap<AccountEditViewModel, UserDto>();
            CreateMap<UserDto, UserBaseViewModel>();

            CreateMap<RoleListItemDto, RolesListViewModel>();
            CreateMap<RoleCreateViewModel, RoleDto>();
            CreateMap<RoleDto, RoleEditViewModel>();
            CreateMap<RoleEditViewModel, RoleDto>();

            CreateMap<AuthorizeListItemDto, AuthorizeListViewModel>();
            CreateMap<AuthorizeRoleCreateModel, AuthorizeRoleDto>();
            CreateMap<AuthorizeRoleDto, AuthorizeRoleEditModel>();
            CreateMap<AuthorizeRoleEditModel, AuthorizeRoleDto>();

            CreateMap<AuthorizeRoleListItemDto, AuthorizeRoleListViewModel>()
                .ForMember(x => x.role, y => y.MapFrom(t => t.role))
                .ForMember(x => x.authorize, y => y.MapFrom(t => t.authorize));
            
        }
    }
}
