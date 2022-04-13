using AutoMapper;
using CORE.ApplicationCommon.DTOS.LogsDTO.LogDTO;
using CORE.ApplicationCommon.DTOS.LogsDTO.ProcessDTO;
using CORE.ApplicationCommon.DTOS.LogsDTO.TransactionDTO;
using CORE.ApplicationCommon.DTOS.SetingsDTO;
using CORE.ApplicationCommon.Helpers;
using GazeteKapiMVC5Core.Core.Extensions;
using GazeteKapiMVC5Core.DataAccessLayer.Models;
using GazeteKapiMVC5Core.Models.Log.LogModel;
using GazeteKapiMVC5Core.Models.Log.ProcessModel;
using GazeteKapiMVC5Core.Models.Log.TransactionModel;
using GazeteKapiMVC5Core.Models.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SERVICE.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GazeteKapiMVC5Core.Controllers
{
    public class LogController : Controller
    {

        #region Fields / Constructre

        private readonly ILogService _logService;
        private readonly IMapper _mapper;
        private readonly ISettingService _settingService;
        public LogController(ILogService logService, IMapper mapper, ISettingService settingService)
        {
            _settingService = settingService;
            _logService = logService;
            _mapper = mapper;
        }

        #endregion

        [HttpGet]
        public IActionResult Loglar(int? pageNumber, /*string searchLogName,*/ int? processId, int? transactionId)
        {
            int pageSize = 20;
            var listLogWithProcess = _mapper.Map<List<ProcessesItemListDto>, List<ProcessesListItemModel>>(_logService.GetAllProcess());
            ViewBag.Islemler = new SelectList(listLogWithProcess, "Id", "ProcessesName");

            var listLogWithTransaction = _mapper.Map<List<TransactionListItemDto>, List<TransactionListItemModel>>(_logService.GetAllTransaction());
            ViewBag.Durumlar = new SelectList(listLogWithTransaction, "Id", "TransactionNames");

            List<LogListViewModel> listLog;
            if (processId != null && processId != 0)
            {
                listLog = _mapper.Map<List<LogListItemDto>, List<LogListViewModel>>(_logService.GetAllLogsByProcess(processId));
                return View(PaginationList<LogListViewModel>.Create(listLog.ToList(), pageNumber ?? 1, pageSize));
            }

            if (transactionId != null && transactionId != 0)
            {
                listLog = _mapper.Map<List<LogListItemDto>, List<LogListViewModel>>(_logService.GetAllLogsByTransactions(transactionId));
                return View(PaginationList<LogListViewModel>.Create(listLog.ToList(), pageNumber ?? 1, pageSize));
            }

            var settingLog = _mapper.Map<SettingsBaseDto, SettingsBaseViewModel>(_settingService.getSettings(1));

            if (settingLog.LogIsActive == false)
            {
                TempData["LogMessage"] = "Loglama işlemleri aktif değildir. Aktifleştirmek için >";
            }

            //if (searchLogName != null && searchLogName != "")
            //{
            //    listLog = _mapper.Map<List<LogListItemDto>, List<LogListViewModel>>(_logService.GetAllLogsBySearchName(searchLogName));
            //}

            listLog = _mapper.Map<List<LogListItemDto>, List<LogListViewModel>>(_logService.GetAllLogs());
            return View(PaginationList<LogListViewModel>.Create(listLog.ToList(), pageNumber ?? 1, pageSize));
        }
    
        [HttpGet]
        public IActionResult LogDetay(int id)
        {
            var getLogDetail = _mapper.Map<LogDto, LogBaseViewModel>(_logService.GetLogDetail(id));
            return View(getLogDetail);
        }

    }
}
