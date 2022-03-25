using CORE.ApplicationCommon.DTOS.LogsDTO.LogDTO;
using CORE.ApplicationCommon.DTOS.LogsDTO.ProcessDTO;
using CORE.ApplicationCommon.DTOS.LogsDTO.TransactionDTO;
using CORE.ApplicationCommon.DTOS.LogsDTO.UserLogDTO;
using CORE.ApplicationCore.UnitOfWork;
using DOMAIN.DataAccessLayerLOG.Models;
using GazeteKapiMVC5Core.DataAccessLayerLOG;
using SERVICE.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICE.Engine.Engines
{
    public class LogService : ILogService
    {
        private readonly IUnitOfWork<NewsAppContextLog> _unitOfWork;
        public LogService(IUnitOfWork<NewsAppContextLog> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CreateLog(LogDto model)
        {
            Logs newLogs = await _unitOfWork.GetRepository<Logs>().AddAsync(new Logs 
            {
                Action = model.Action,
                Controller = model.Controller,
                Details = model.Details,
                Hour = model.Hour,
                Date = model.Date,
                TransactionID = model.TransactionId,
                ProcessID = model.ProcessId,
                UserID = model.UserId
            });

            return newLogs != null && newLogs.Id != 0;
        }

        public async Task<bool> CreateUserByLog(UserLogDto model)
        {
            UsersLog newUsers = await _unitOfWork.GetRepository<UsersLog>().AddAsync(new UsersLog
            {
                UserNameLog = model.UserNameLog,
            });

            return newUsers != null && newUsers.Id != 0;

        }

        public List<LogListItemDto> GetAllLogs()
        {
            IEnumerable<Logs> logs = _unitOfWork.GetRepository<Logs>().Filter(null, x => x.OrderBy(y => y.Id), "transactions,processes,userslog", 1, 300000);
            return logs.Select(x => new LogListItemDto 
            {
                Id = x.Id,
                Action = x.Action,
                Controller = x.Controller,
                Details = x.Details,
                Date = x.Date,
                Hour = x.Hour,
                ProcessId = x.ProcessID,
                TransactionId = x.TransactionID,
                UserId = x.UserID,
                process = x.processes,
                transaction = x.transactions,
                userlogs = x.userslog

            }).ToList();
        }

        public LogDto GetLogDetail(int id)
        {
            Logs getLogs = _unitOfWork.GetRepository<Logs>().FindAsync(x => x.Id == id).Result;
            UsersLog getUsersLog =  _unitOfWork.GetRepository<UsersLog>().FindAsync(x => x.Id == getLogs.UserID).Result;
            Transactions getTransactions = _unitOfWork.GetRepository<Transactions>().FindAsync(x => x.Id == getLogs.TransactionID).Result;
            Processes getProcesess = _unitOfWork.GetRepository<Processes>().FindAsync(x => x.Id == getLogs.ProcessID).Result;

            return new LogDto
            {
                Id = getLogs.Id,
                Action = getLogs.Action,
                Controller = getLogs.Controller,
                Details = getLogs.Details,
                Date = getLogs.Date,
                Hour = getLogs.Hour,
                ProcessId = getLogs.ProcessID,
                TransactionId = getLogs.TransactionID,
                UserId = getLogs.UserID,
                process = getProcesess,
                transaction = getTransactions,
                userlogs = getUsersLog
            };
        }

        public ProcessDto GetProcessByName(string processName)
        {
            Processes getProcess = _unitOfWork.GetRepository<Processes>().FindAsync(x => x.ProcessesName == processName).Result;

            return new ProcessDto
            {
                Id = getProcess.Id,
                ProcessNane = getProcess.ProcessesName,
            };
        }

        public TransactionDto GetTransactionByName(string transactionName)
        {
            Transactions getProcess = _unitOfWork.GetRepository<Transactions>().FindAsync(x => x.TransactionNames == transactionName).Result;

            return new TransactionDto
            {
                Id = getProcess.Id,
                TransactionName = getProcess.TransactionNames,
            };
        }

        public UserLogDto GetUserByName(string userName)
        {
            UsersLog getUser = _unitOfWork.GetRepository<UsersLog>().FindAsync(x => x.UserNameLog == userName).Result;

            if (getUser != null)
            {
                return new UserLogDto
                {
                    Id = getUser.Id,
                    UserNameLog = getUser.UserNameLog,
                };
            }
            else
            {
                return null;
            }
           
        }
    }
}
