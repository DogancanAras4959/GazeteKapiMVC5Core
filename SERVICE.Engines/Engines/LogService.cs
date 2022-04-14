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
            IEnumerable<Logs> logs = _unitOfWork.GetRepository<Logs>().Filter(null, x => x.OrderByDescending(y => y.Id), "transactions,processes,userslog", null, null);
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
        public List<LogListItemDto> GetAllLogsByProcess(int? ProcessId)
        {

            IEnumerable<Logs> logs = _unitOfWork.GetRepository<Logs>().Filter(x=> x.ProcessID == ProcessId, x => x.OrderByDescending(y => y.Id), "transactions,processes,userslog", null, null);

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
        public List<LogListItemDto> GetAllLogsBySearchName(string searchLogName)
        {
            IEnumerable<Logs> logs = _unitOfWork.GetRepository<Logs>().Filter(null, x => x.OrderByDescending(y => y.Id), "transactions,processes,userslog", null, null);

            if (!String.IsNullOrEmpty(searchLogName))
            {
                logs = logs.Where(x => x.Action!.Contains(searchLogName));
            }
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
        public List<LogListItemDto> GetAllLogsByTransactions(int? TransactionId)
        {
            IEnumerable<Logs> logs = _unitOfWork.GetRepository<Logs>().Filter(x=> x.TransactionID == TransactionId, x => x.OrderByDescending(y => y.Id), "transactions,processes,userslog", null, null);

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
        public List<ProcessesItemListDto> GetAllProcess()
        {
            IEnumerable<Processes> process = _unitOfWork.GetRepository<Processes>().Filter(null, x => x.OrderByDescending(y => y.Id),null, null, null);
            return process.Select(x => new ProcessesItemListDto
            {
                Id = x.Id,
                ProcessesName = x.ProcessesName,

            }).ToList();
        }
        public List<TransactionListItemDto> GetAllTransaction()
        {
            IEnumerable<Transactions> logs = _unitOfWork.GetRepository<Transactions>().Filter(null, x => x.OrderByDescending(y => y.Id), null, null, null);
            return logs.Select(x => new TransactionListItemDto
            {
                Id = x.Id,
                TransactionNames = x.TransactionNames,

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
        public List<LogListItemDto> getLogsByUser(string userName)
        {
            UsersLog getUser = _unitOfWork.GetRepository<UsersLog>().FindAsync(x => x.UserNameLog == userName).Result;

            if (getUser == null)
            {
                return null;
            }

            IEnumerable<Logs> logs = _unitOfWork.GetRepository<Logs>().Filter(x => x.UserID == getUser.Id, x => x.OrderByDescending(y => y.Id), "transactions,processes,userslog", 1, 5);

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
        public ProcessDto GetProcessByName(string processName)
        {
            Processes getProcess = _unitOfWork.GetRepository<Processes>().FindAsync(x => x.ProcessesName == processName).Result;

            return new ProcessDto
            {
                Id = getProcess.Id,
                ProcessesName = getProcess.ProcessesName,
            };
        }
        public TransactionDto GetTransactionByName(string transactionName)
        {
            Transactions getProcess = _unitOfWork.GetRepository<Transactions>().FindAsync(x => x.TransactionNames == transactionName).Result;

            return new TransactionDto
            {
                Id = getProcess.Id,
                TransactionNames = getProcess.TransactionNames,
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
