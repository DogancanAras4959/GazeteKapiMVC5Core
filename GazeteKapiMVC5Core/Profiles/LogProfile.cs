using AutoMapper;
using CORE.ApplicationCommon.DTOS.LogsDTO.LogDTO;
using CORE.ApplicationCommon.DTOS.LogsDTO.ProcessDTO;
using CORE.ApplicationCommon.DTOS.LogsDTO.TransactionDTO;
using CORE.ApplicationCommon.DTOS.LogsDTO.UserLogDTO;
using GazeteKapiMVC5Core.Models.Log.LogModel;
using GazeteKapiMVC5Core.Models.Log.ProcessModel;
using GazeteKapiMVC5Core.Models.Log.TransactionModel;
using GazeteKapiMVC5Core.Models.Log.UserLogModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GazeteKapiMVC5Core.Profiles
{
    public class LogProfile : Profile
    {
        public LogProfile()
        {
            CreateMap<LogDto, LogBaseViewModel>();
            CreateMap<LogCreateViewModel, LogDto>();
            CreateMap<LogListItemDto, LogListViewModel>()
                .ForMember(x => x.processes, y => y.MapFrom(t => t.process))
                .ForMember(x => x.transaction, y => y.MapFrom(t => t.transaction))
                .ForMember(x => x.userlogs, y => y.MapFrom(t => t.userlogs));

            CreateMap<TransactionDto, TransactionBaseViewModel>();
            CreateMap<ProcessDto, ProcessBaseViewModel>();
            CreateMap<UserLogDto, UserLogBaseViewModel>();
        }
    }
}
