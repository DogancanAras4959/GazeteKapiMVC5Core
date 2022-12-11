using AutoMapper;
using CORE.ApplicationCommon.DTOS.MagazineBannerDTO;
using GazeteKapiMVC5Core.Models.MagazineBanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GazeteKapiMVC5Core.Profiles
{
    public class MagazineBannerProfile : Profile
    {
        public MagazineBannerProfile()
        {
            CreateMap<MagazineBannerCreateViewModel, MagazineBannerDto>();
            CreateMap<MagazineBannerDto, MagazineBannerEditViewModel>();
            CreateMap<MagazineBannerEditViewModel, MagazineBannerDto>();
            CreateMap<MagazineBannerListItemDto, MagazineBannerListViewModel>();
            CreateMap<MagazineBannerDto, MagazineBannerBaseViewModel>();
        }
    }
}
