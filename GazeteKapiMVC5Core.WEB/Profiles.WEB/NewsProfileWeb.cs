using AutoMapper;
using CORE.ApplicationCommon.DTOS.NewsDTO;
using GazeteKapiMVC5Core.Models.News.NewsModel;
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
        }
    }
}
