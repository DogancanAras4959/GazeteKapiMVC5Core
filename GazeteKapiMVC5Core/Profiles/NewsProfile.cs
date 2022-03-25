using AutoMapper;
using CORE.ApplicationCommon.DTOS.CategoryDTO;
using GazeteKapiMVC5Core.Models.Category;
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
            CreateMap<CategoryCreateViewModel, CategoryDto>();
            CreateMap<CategoryDto, CategoryEditViewModel>();
            CreateMap<CategoryEditViewModel, CategoryDto>();
            CreateMap<CategoryListItemDto, CategoryListViewModel>();
            CreateMap<CategoryDto, CategoryBaseViewModel>();
        }
    }
}
