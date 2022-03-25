using DOMAIN.DataAccessLayer.Models.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace DOMAIN.DataAccessLayerLOG.Models
{
    public class Transactions : IEntity
    {
        public Transactions()
        {
            logsByTransactions = new List<Logs>();
        }

        public int Id { get; set; }
        public string TransactionNames { get; set; }
        public virtual ICollection<Logs> logsByTransactions { get; set; }
    }
}
