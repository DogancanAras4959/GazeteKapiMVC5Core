using AutoMapper;
using CORE.ApplicationCommon.DTOS.CategoryDTO;
using CORE.ApplicationCommon.DTOS.NewsDto.GuestDto;
using GazeteKapiMVC5Core.Models.Category;
using GazeteKapiMVC5Core.Models.News.GuestModel;
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
            #region News Create Map

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


        }
    }
}
