using DOMAIN.DataAccessLayer.Models.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace DOMAIN.DataAccessLayerLOG.Models
{
    public class UsersLog : IEntity
    {
        public UsersLog()
        {
            logsByUsers = new List<Logs>();
        }

        public int Id { get; set; }
        public string UserNameLog { get; set; }
        public virtual ICollection<Logs> logsByUsers { get; set; }
    }
}
