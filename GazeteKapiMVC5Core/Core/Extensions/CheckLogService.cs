using AutoMapper;
using CORE.ApplicationCommon.DTOS.LogsDTO.LogDTO;
using CORE.ApplicationCommon.DTOS.LogsDTO.ProcessDTO;
using CORE.ApplicationCommon.DTOS.LogsDTO.TransactionDTO;
using CORE.ApplicationCommon.DTOS.LogsDTO.UserLogDTO;
using GazeteKapiMVC5Core.Models.Log.LogModel;
using GazeteKapiMVC5Core.Models.Log.TransactionModel;
using SERVICE.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GazeteKapiMVC5Core.Core.Extensions
{
    public class CheckLogService
    {
        public ILogService _logService { get; set; }
        public IMapper _mapper { get; set; }
        public CheckLogService(ILogService logService, IMapper mapper)
        {
            _logService = logService;
            _mapper = mapper;
        }

        public async Task<LogDto> CreateLogs(string durumAdi, string IslemAdi, string action, string controller, string kulladi)
        {
            LogDto newLog = new LogDto();

            TransactionDto getTransaction = _logService.GetTransactionByName(durumAdi);

            if (getTransaction != null)
            {
                ProcessDto getProcess = _logService.GetProcessByName(IslemAdi);

                if (getProcess != null)
                {

                    UserLogDto getUser = _logService.GetUserByName(kulladi);

                    if (getUser == null)
                    {

                        UserLogDto yeniYoneticiGetir = await YoneticiOlustur(kulladi); 
                        newLog.UserId = yeniYoneticiGetir.Id;
                        newLog.Action = action;
                        newLog.Controller = controller;
                        newLog.ProcessId = getProcess.Id;     
                        newLog.TransactionId = getTransaction.Id;
                        newLog.Hour = DateTime.Now;
                        newLog.Date = DateTime.Now;

                        await _logService.CreateLog(newLog);
                        return newLog;
                    }
                    else
                    {

                        newLog.UserId = getUser.Id;
                        newLog.Action = action;
                        newLog.Controller = controller;
                        newLog.ProcessId = getProcess.Id;
                        newLog.TransactionId = getTransaction.Id;
                        newLog.Hour = DateTime.Now;
                        newLog.Date = DateTime.Now;

                        await _logService.CreateLog(newLog);
                        return newLog;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        private async Task<UserLogDto> YoneticiOlustur(string name)
        {
            if (name != null || name != "")
            {
                UserLogDto y = new UserLogDto
                {
                    UserNameLog = name
                };

                await _logService.CreateUserByLog(y);
                return y;
            }
            else
            {
                return null;
            }

        }
    }
}
