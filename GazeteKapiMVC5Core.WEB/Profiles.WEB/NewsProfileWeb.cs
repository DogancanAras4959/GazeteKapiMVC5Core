using AutoMapper;
using CORE.ApplicationCommon.DTOS.CategoryDTO;
using CORE.ApplicationCommon.DTOS.CurrencyDTO;
using CORE.ApplicationCommon.DTOS.NewsDTO;
using CORE.ApplicationCommon.DTOS.NewsDTO.GuestDTO;
using CORE.ApplicationCommon.DTOS.NewsDTO.TagNewsDTO;
using CORE.ApplicationCommon.DTOS.SetingsDTO;
using GazeteKapiMVC5Core.WEB.ViewModels.Categories;
using GazeteKapiMVC5Core.WEB.ViewModels.Currencies;
using GazeteKapiMVC5Core.WEB.ViewModels.Guests;
using GazeteKapiMVC5Core.WEB.ViewModels.News;
using GazeteKapiMVC5Core.WEB.ViewModels.Settings;
using GazeteKapiMVC5Core.WEB.ViewModels.TagsNews;

namespace GazeteKapiMVC5Core.WEB.Profiles.WEB
{
    public class NewsProfileWeb : Profile
    {
        public NewsProfileWeb()
        {
            CreateMap<NewsListItemDto, NewListViewModelWeb>()
             .ForMember(x => x.users, y => y.MapFrom(t => t.users))
             .ForMember(x => x.categories, y => y.MapFrom(t => t.categories))
             .ForMember(x => x.guest, y => y.MapFrom(t => t.guest))
             .ForMember(x => x.publishtype, y => y.MapFrom(t => t.publishtype));

            CreateMap<GuestListItemDto, GuestListViewModelWeb>().ForMember(x=> x.user, y => y.MapFrom(t=> t.user));
            CreateMap<GuestDto, GuestEditViewModelWeb>();
            CreateMap<TagNewsListItemDto, TagNewsListViewModelWeb>().ForMember(x => x.news, y => y.MapFrom(t => t.news)).ForMember(x => x.tag, y => y.MapFrom(t => t.tag));

            CreateMap<CategoryBaseDto, CategoryBaseViewModelWeb>();
            CreateMap<CategoryListItemDto, CategoryListViewModelWeb>();
            CreateMap<CategoryDto, CategoryEditViewModelWeb>();
            CreateMap<NewsDto, NewsEditViewModelWeb>();
            CreateMap<NewsEditViewModelWeb, NewsDto>();

            CreateMap<CurrencyCreateViewModelWeb, CurrencyDto>();
            CreateMap<CurrencyListItemDto, CurrencyListViewModelWeb>();
            CreateMap<CurrencyListViewModelWeb, CurrencyDto>();
            CreateMap<CurrencyDto, CurrencyListViewModelWeb>();
            CreateMap<CurrencyBaseDto, CurrencyListViewModelWeb>();

            CreateMap<SettingsEditViewModelWeb, SettingsDto>();
            CreateMap<SettingsDto, SettingsEditViewModelWeb>();
            CreateMap<SettingsDto, SettingsBaseViewModelWeb>();
        }
    }
}
