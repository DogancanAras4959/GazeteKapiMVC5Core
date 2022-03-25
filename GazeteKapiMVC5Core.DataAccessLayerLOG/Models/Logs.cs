using DOMAIN.DataAccessLayer.Models.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DOMAIN.DataAccessLayerLOG.Models
{
    public class Logs : IEntity
    {
        public int Id { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public string Details { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Hour { get; set; }
        
        [ForeignKey("transactions")]
        public int TransactionID { get; set; }

        [ForeignKey("processes")]
        public int ProcessID { get; set; }

        [ForeignKey("userslog")]
        public int UserID { get; set; }

        public Transactions transactions { get; set; }
        public Processes processes { get; set; }
        public UsersLog userslog { get; set; }
    }
}
