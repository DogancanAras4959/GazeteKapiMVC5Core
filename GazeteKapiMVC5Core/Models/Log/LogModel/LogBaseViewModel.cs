
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GazeteKapiMVC5Core.Models.Log.LogModel
{
    public class LogBaseViewModel
    {
        public string Action { get; set; }
        public string Controller { get; set; }
        public string Details { get; set; }
        public DateTime? Hour { get; set; }
        public DateTime? Time { get; set; }
        public int UserId { get; set; }
        public int TransactionId { get; set; }
        public int ProcessId { get; set; }
    }
}
