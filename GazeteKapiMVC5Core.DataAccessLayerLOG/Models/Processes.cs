using DOMAIN.DataAccessLayer.Models.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace DOMAIN.DataAccessLayerLOG.Models
{
    public class Processes : IEntity
    {
        public Processes()
        {
            logsByProccess = new List<Logs>();
        }

        public int Id { get; set; }
        public string ProcessesName { get; set; }
        public DateTime? CreatedTime { get; set; }
        public virtual ICollection<Logs> logsByProccess { get; set; }
    }
}
