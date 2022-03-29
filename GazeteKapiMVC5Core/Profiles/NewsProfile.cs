using AutoMapper;
using CORE.ApplicationCommon.DTOS.CategoryDTO;
using CORE.ApplicationCommon.DTOS.NewsDTO;
using CORE.ApplicationCommon.DTOS.NewsDTO.GuestDTO;
using CORE.ApplicationCommon.DTOS.NewsDTO.PublishTypeDTO;
using CORE.ApplicationCommon.DTOS.NewsDTO.TagDTO;
using CORE.ApplicationCommon.DTOS.NewsDTO.TagNewsDTO;
using GazeteKapiMVC5Core.Models.Category;
using GazeteKapiMVC5Core.Models.News.GuestModel;
using GazeteKapiMVC5Core.Models.News.NewsModel;
using GazeteKapiMVC5Core.Models.News.PublishTypeModel;
using GazeteKapiMVC5Core.Models.News.TagModel;
using GazeteKapiMVC5Core.Models.News.TagNewsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

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
            CreateMap<NewsEditViewModel, NewsDto>();
            CreateMap<NewsDto, NewsEditViewModel>();

            CreateMap<PublishTypeListItem, PublishTypeListViewModel>();

            CreateMap<TagBaseViewModel, TagDto>();
            CreateMap<CreateTagViewModel, TagDto>();
            CreateMap<TagEditViewModel, TagDto>();
            CreateMap<TagDto, TagEditViewModel>();

            CreateMap<TagNewsBaseViewModel, TagNewsDto>();
            CreateMap<TagNewsDto, TagNewsBaseViewModel>();
            CreateMap<TagNewsCreateViewModel, TagNewsBaseDto>();
            CreateMap<TagNewsEditViewModel, TagNewsDto>();
            CreateMap<TagNewsDto, TagNewsEditViewModel>();

            CreateMap<TagListItemDto, TagListViewModel>();

            CreateMap<TagNewsListItemDto, TagNewsListViewModel>()
                .ForMember(x => x.news, y => y.MapFrom(t => t.news))
                .ForMember(x => x.tag, y => y.MapFrom(t => t.tag));

            #endregion

        }
    }
}
