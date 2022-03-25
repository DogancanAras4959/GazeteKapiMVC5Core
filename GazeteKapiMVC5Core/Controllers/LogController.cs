using AutoMapper;
using CORE.ApplicationCommon.DTOS.LogsDTO.LogDTO;
using GazeteKapiMVC5Core.Models.Log.LogModel;
using Microsoft.AspNetCore.Mvc;
using SERVICE.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GazeteKapiMVC5Core.Controllers
{
    public class LogController : Controller
    {
        private readonly ILogService _logService;
        private readonly IMapper _mapper;
        public LogController(ILogService logService, IMapper mapper)
        {
            _logService = logService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Loglar()
        {
            var listLog = _mapper.Map<List<LogListItemDto>, List<LogListViewModel>>(_logService.GetAllLogs());
            return View(listLog);
        }

        [HttpGet]
        public IActionResult LogDetay(int id)
        {
            var getLogDetail = _mapper.Map<LogDto, LogBaseViewModel>(_logService.GetLogDetail(id));
            return View(getLogDetail);
        }
    }
}
