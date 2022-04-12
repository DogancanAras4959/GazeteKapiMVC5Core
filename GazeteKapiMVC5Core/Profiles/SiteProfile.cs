using AutoMapper;
using CORE.ApplicationCommon.DTOS.CurrencyDTO;
using CORE.ApplicationCommon.DTOS.PrivacyDTO.AboutUsDto;
using CORE.ApplicationCommon.DTOS.PrivacyDTO.PrivacyDto;
using CORE.ApplicationCommon.DTOS.PrivacyDTO.TermsOfUsDto;
using CORE.ApplicationCommon.DTOS.SetingsDTO;
using GazeteKapiMVC5Core.Models.CurrencyModel;
using GazeteKapiMVC5Core.Models.Settings;
using GazeteKapiMVC5Core.Models.Site.AboutUsModel;
using GazeteKapiMVC5Core.Models.Site.PrivacyModel;
using GazeteKapiMVC5Core.Models.Site.TermsOfUsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GazeteKapiMVC5Core.Profiles
{
    public class SiteProfile : Profile
    {
        public SiteProfile()
        {
            CreateMap<SettingsEditViewModel, SettingsDto>();
            CreateMap<SettingsDto, SettingsEditViewModel>();
            CreateMap<SettingsDto, SettingsBaseViewModel>();

            CreateMap<PrivacyEditModel, PrivacyDto>();
            CreateMap<PrivacyDto, PrivacyEditModel>();
            CreateMap<PrivacyDto, PrivacyBaseModel>();

            CreateMap<AboutUsEditModel, AboutUsDto>();
            CreateMap<AboutUsDto, AboutUsEditModel>();
            CreateMap<AboutUsDto, AboutUsBaseModel>();

            CreateMap<TermsOfUsEditModel, TermsOfUsDto>();
            CreateMap<TermsOfUsDto, TermsOfUsEditModel>();
            CreateMap<TermsOfUsDto, PrivacyBaseModel>();

       
        }
    }
}
