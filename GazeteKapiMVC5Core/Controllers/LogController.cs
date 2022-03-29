using AutoMapper;
using CORE.ApplicationCommon.DTOS.LogsDTO.LogDTO;
using CORE.ApplicationCommon.DTOS.LogsDTO.ProcessDTO;
using CORE.ApplicationCommon.DTOS.LogsDTO.TransactionDTO;
using GazeteKapiMVC5Core.Core.Extensions;
using GazeteKapiMVC5Core.Models.Log.LogModel;
using GazeteKapiMVC5Core.Models.Log.ProcessModel;
using GazeteKapiMVC5Core.Models.Log.TransactionModel;
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
        public LogController(ILogService logService, IMapper mapper)
        {
            _logService = logService;
            _mapper = mapper;
        }

        #endregion

        [HttpGet]
        public IActionResult Loglar(int? pageNumber)
        {
            int pageSize = 20;

            var listLogWithProcess = _mapper.Map<List<ProcessesItemListDto>, List<ProcessesListItemModel>>(_logService.GetAllProcess());
            ViewBag.Islemler = new SelectList(listLogWithProcess, "Id", "ProcessesName");

            var listLogWithTransaction = _mapper.Map<List<TransactionListItemDto>, List<TransactionListItemModel>>(_logService.GetAllTransaction());
            ViewBag.Durumlar = new SelectList(listLogWithTransaction, "Id", "TransactionNames");

            var listLog = _mapper.Map<List<LogListItemDto>, List<LogListViewModel>>(_logService.GetAllLogs());
            return View(PaginationList<LogListViewModel>.Create(listLog.ToList(), pageNumber ?? 1, pageSize));
        }

        public IActionResult LogListeleIslem(string ProcessesName, int? pageNumber)
        {
            int pageSize = 20;
            TempData["processId"] = ProcessesName;
            var listLogWithProcess = _mapper.Map<List<ProcessesItemListDto>, List<ProcessesListItemModel>>(_logService.GetAllProcess());
            ViewBag.Islemler = new SelectList(listLogWithProcess, "Id", "ProcessesName");

            var listLogWithTransaction = _mapper.Map<List<TransactionListItemDto>, List<TransactionListItemModel>>(_logService.GetAllTransaction());
            ViewBag.Durumlar = new SelectList(listLogWithTransaction, "Id", "TransactionNames");

            var listLogByIslem = _mapper.Map<List<LogListItemDto>, List<LogListViewModel>>(_logService.GetAllLogsByProcess(ProcessesName));
            return View(PaginationList<LogListViewModel>.Create(listLogByIslem.ToList(), pageNumber ?? 1, pageSize));
        }

        public IActionResult LogListeleDurum(string TransactionNames, int? pageNumber)
        {
            int pageSize = 20;
            TempData["transactionId"] = TransactionNames;

            var listLogWithProcess = _mapper.Map<List<ProcessesItemListDto>, List<ProcessesListItemModel>>(_logService.GetAllProcess());
            ViewBag.Islemler = new SelectList(listLogWithProcess, "Id", "ProcessesName");

            var listLogWithTransaction = _mapper.Map<List<TransactionListItemDto>, List<TransactionListItemModel>>(_logService.GetAllTransaction());
            ViewBag.Durumlar = new SelectList(listLogWithTransaction, "Id", "TransactionNames");

            var listLogbyDurum = _mapper.Map<List<LogListItemDto>, List<LogListViewModel>>(_logService.GetAllLogsByTransactions(TransactionNames));
            return View(PaginationList<LogListViewModel>.Create(listLogbyDurum.ToList(), pageNumber ?? 1, pageSize));
        }

        [HttpGet]
        public IActionResult LogDetay(int id)
        {
            var getLogDetail = _mapper.Map<LogDto, LogBaseViewModel>(_logService.GetLogDetail(id));
            return View(getLogDetail);
        }

    }
}
