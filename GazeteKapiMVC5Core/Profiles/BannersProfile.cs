using AutoMapper;
using CORE.ApplicationCommon.DTOS.BannersDTO;
using GazeteKapiMVC5Core.Models.Banners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GazeteKapiMVC5Core.Profiles
{
    public class BannersProfile : Profile
    {
        public BannersProfile()
        {
            CreateMap<BannerCreateViewModel, BannerDto>();
            CreateMap<BannerDto, BannerEditViewModel>();
            CreateMap<BannerEditViewModel, BannerDto>();
            CreateMap<BannerListItemDto, BannerListViewModel>();
            CreateMap<BannerDto, BannerBaseViewModel>();
        }
    }
}
