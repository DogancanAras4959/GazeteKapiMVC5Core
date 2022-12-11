using AutoMapper;
using CORE.ApplicationCommon.DTOS.BannersDTO;
using CORE.ApplicationCommon.DTOS.MagazineBannerDTO;
using GazeteKapiMVC5Core.WEB.ViewModels.Banner;
using GazeteKapiMVC5Core.WEB.ViewModels.MagazineBanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GazeteKapiMVC5Core.WEB.Profiles.WEB
{
    public class BannerProfileWeb : Profile
    {
        public BannerProfileWeb()
        {
            CreateMap<MagazineBannerDto, MagazineBannerEditViewModelWeb>();
            CreateMap<MagazineBannerEditViewModelWeb, MagazineBannerDto>();
            CreateMap<MagazineBannerListItemDto, MagazineBannerListViewModelWeb>();
            CreateMap<MagazineBannerDto, MagazineBannerBaseViewModelWeb>();

            CreateMap<BannerDto, BannerEditViewModelWeb>();
            CreateMap<BannerEditViewModelWeb, BannerDto>();
            CreateMap<BannerListItemDto, BannerListViewModelWeb>();
            CreateMap<BannerDto, BannerBaseViewModelWeb>();
        }
    }
}
