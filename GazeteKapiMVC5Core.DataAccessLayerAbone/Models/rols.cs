using GazeteKapiMVC5Core.DataAccessLayerAbone.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace GazeteKapiMVC5Core.DataAccessLayerAbone.Models
{
    public class rols : IEntity
    {
        public rols()
        {
            userList = new List<users>();
        }
        public int Id { get; set; }
        public string rolename { get; set; }
        public string isActive { get; set; }

        public virtual ICollection<users> userList { get; set; }
    }
}
