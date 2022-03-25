using DOMAIN.DataAccessLayerLOG.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CORE.ApplicationCommon.DTOS.LogsDTO.LogDTO
{
    public class LogBaseDto
    {
        public string Action { get; set; }
        public string Controller { get; set; }
        public string Details { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Hour { get; set; }
        public int UserId { get; set; }
        public int TransactionId { get; set; }
        public int ProcessId { get; set; }
        public Processes process { get; set; }
        public Transactions transaction { get; set; }
        public UsersLog userlogs { get; set; }
    }
}
