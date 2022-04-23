using AutoMapper;
using CORE.ApplicationCommon.DTOS.PrivacyDTO.PolicyDto;
using CORE.ApplicationCommon.DTOS.PrivacyDTO.PrivacyDto;
using CORE.ApplicationCommon.DTOS.PrivacyDTO.TermsOfUsDto;
using GazeteKapiMVC5Core.WEB.ViewModels.Policy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GazeteKapiMVC5Core.WEB.Profiles.WEB
{
    public class PolicyProfileWeb : Profile
    {
        public PolicyProfileWeb()
        {
            CreateMap<PrivacyDto, PrivacyBaseViewModel>();
            CreateMap<CookiePolicyDto, CookieBaseViewModel>();
            CreateMap<BrandPolicyDto, BrandBaseViewModel>();
            CreateMap<StreamPolicyDto, StreamBaseViewModel>();
            CreateMap<TermsOfUsDto, TermsOfUsBaseViewModel>();
        }
    }
}
