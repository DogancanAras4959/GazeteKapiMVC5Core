using AutoMapper;
using CORE.ApplicationCommon.DTOS.SeoDTO;
using CORE.ApplicationCommon.DTOS.SeoDTO.SeoMetaDto;
using GazeteKapiMVC5Core.Models.Seo.SeoMeta;
using GazeteKapiMVC5Core.Models.Seo.SeoScore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GazeteKapiMVC5Core.Profiles
{
    public class SeoProfiles : Profile
    {
        public SeoProfiles()
        {
            CreateMap<SeoScoreDto, SeoScoreBaseViewModel>();
            CreateMap<SeoScoreCreateViewModel, SeoScoreDto>();
            CreateMap<SeoScoreEditViewModel, SeoScoreDto>();
            CreateMap<SeoScoreDto, SeoScoreEditViewModel>();
            CreateMap<SeoMetaListItemDto,SeoMetaListViewModel>();
            CreateMap<SeoMetaCreateViewModel, SeoMetaDto>();
        }
    }
}
