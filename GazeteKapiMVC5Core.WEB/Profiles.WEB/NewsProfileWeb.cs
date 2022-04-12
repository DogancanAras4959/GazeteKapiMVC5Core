using AutoMapper;
using CORE.ApplicationCommon.DTOS.CategoryDTO;
using CORE.ApplicationCommon.DTOS.CurrencyDTO;
using CORE.ApplicationCommon.DTOS.NewsDTO;
using CORE.ApplicationCommon.DTOS.NewsDTO.GuestDTO;
using CORE.ApplicationCommon.DTOS.NewsDTO.TagNewsDTO;
using CORE.ApplicationCommon.DTOS.SetingsDTO;
using GazeteKapiMVC5Core.Models.Category;
using GazeteKapiMVC5Core.Models.CurrencyModel;
using GazeteKapiMVC5Core.Models.News.GuestModel;
using GazeteKapiMVC5Core.Models.News.NewsModel;
using GazeteKapiMVC5Core.Models.News.TagModel;
using GazeteKapiMVC5Core.Models.News.TagNewsModel;
using GazeteKapiMVC5Core.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GazeteKapiMVC5Core.WEB.Profiles.WEB
{
    public class NewsProfileWeb : Profile
    {
        public NewsProfileWeb()
        {
            CreateMap<NewsListItemDto, NewsLıstItemModel>()
             .ForMember(x => x.users, y => y.MapFrom(t => t.users))
             .ForMember(x => x.categories, y => y.MapFrom(t => t.categories))
             .ForMember(x => x.guest, y => y.MapFrom(t => t.guest))
             .ForMember(x => x.publishtype, y => y.MapFrom(t => t.publishtype));

            CreateMap<GuestListItemDto, GuestListViewModel>().ForMember(x=> x.user, y => y.MapFrom(t=> t.user));
            CreateMap<GuestDto, GuestEditViewModel>();
            CreateMap<TagNewsListItemDto, TagNewsListViewModel>().ForMember(x => x.news, y => y.MapFrom(t => t.news)).ForMember(x => x.tag, y => y.MapFrom(t => t.tag));

            CreateMap<CategoryBaseDto, CategoryBaseViewModel>();
            CreateMap<CategoryListItemDto, CategoryListViewModel>();
            CreateMap<CategoryDto, CategoryEditViewModel>();
            CreateMap<NewsDto, NewsEditViewModel>();
            CreateMap<NewsEditViewModel, NewsDto>();

            CreateMap<CurrencyCreateViewModel, CurrencyDto>();
            CreateMap<CurrencyListItemDto, CurrencyListViewModel>();
            CreateMap<CurrencyEditViewModel, CurrencyDto>();
            CreateMap<CurrencyDto, CurrencyEditViewModel>();
            CreateMap<CurrencyBaseDto, CurrencyEditViewModel>();

            CreateMap<SettingsEditViewModel, SettingsDto>();
            CreateMap<SettingsDto, SettingsEditViewModel>();
            CreateMap<SettingsDto, SettingsBaseViewModel>();
        }
    }
}
