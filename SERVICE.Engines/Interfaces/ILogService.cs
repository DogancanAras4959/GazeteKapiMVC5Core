using CORE.ApplicationCommon.DTOS.LogsDTO.LogDTO;
using CORE.ApplicationCommon.DTOS.LogsDTO.ProcessDTO;
using CORE.ApplicationCommon.DTOS.LogsDTO.TransactionDTO;
using CORE.ApplicationCommon.DTOS.LogsDTO.UserLogDTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SERVICE.Engine.Interfaces
{
    public interface ILogService
    {
        TransactionDto GetTransactionByName(string transactionName);
        ProcessDto GetProcessByName(string processName);
        UserLogDto GetUserByName(string userName);
        Task<bool> CreateUserByLog(UserLogDto model);
        Task<bool> CreateLog(LogDto model);
        List<LogListItemDto> GetAllLogs();
        List<LogListItemDto> GetAllLogsByProcess(int? ProcessId);
        List<LogListItemDto> GetAllLogsByTransactions(int? TransactionId);
        List<LogListItemDto> GetAllLogsBySearchName(string searchLogName);
        List<ProcessesItemListDto> GetAllProcess();
        List<TransactionListItemDto> GetAllTransaction();
        LogDto GetLogDetail(int id);
        List<LogListItemDto> getLogsByUser(string userName);
    }
}
