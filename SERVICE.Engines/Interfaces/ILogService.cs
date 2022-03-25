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
        LogDto GetLogDetail(int id);
    }
}
