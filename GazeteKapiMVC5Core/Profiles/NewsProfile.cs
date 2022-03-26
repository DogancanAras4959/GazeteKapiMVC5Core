using AutoMapper;
using CORE.ApplicationCommon.DTOS.CategoryDTO;
using CORE.ApplicationCommon.DTOS.NewsDTO;
using CORE.ApplicationCommon.DTOS.NewsDTO.GuestDTO;
using CORE.ApplicationCommon.DTOS.NewsDTO.PublishTypeDTO;
using GazeteKapiMVC5Core.Models.Category;
using GazeteKapiMVC5Core.Models.News.GuestModel;
using GazeteKapiMVC5Core.Models.News.NewsModel;
using GazeteKapiMVC5Core.Models.News.PublishTypeModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GazeteKapiMVC5Core.Profiles
{
    public class NewsProfile : Profile
    {
        public NewsProfile()
        {
            #region Category Create Map

            CreateMap<CategoryCreateViewModel, CategoryDto>();
            CreateMap<CategoryDto, CategoryEditViewModel>();
            CreateMap<CategoryEditViewModel, CategoryDto>();
            CreateMap<CategoryListItemDto, CategoryListViewModel>();
            CreateMap<CategoryDto, CategoryBaseViewModel>();

            #endregion

            #region Guest CreateMap

            CreateMap<GuestCreateViewModel, GuestDto>();
            CreateMap<GuestDto, GuestEditViewModel>();
            CreateMap<GuestEditViewModel, GuestDto>();
            CreateMap<GuestListItemDto, GuestListViewModel>();
            CreateMap<GuestDto, GuestBaseViewModel>();

            #endregion

            #region Haber CreateMap

            CreateMap<NewsListItemDto, NewsLıstItemModel>()
                .ForMember(x => x.users, y => y.MapFrom(t => t.users))
                .ForMember(x => x.categories, y => y.MapFrom(t => t.categories))
                .ForMember(x => x.guest, y => y.MapFrom(t => t.guest))
                .ForMember(x => x.publishtype, y => y.MapFrom(t => t.publishtype));

            CreateMap<NewsCreateViewModel, NewsDto>();

            CreateMap<PublishTypeListItem, PublishTypeListViewModel>();
            #endregion
        }
    }
}
