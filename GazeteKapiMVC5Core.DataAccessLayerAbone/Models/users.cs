using GazeteKapiMVC5Core.DataAccessLayerAbone.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GazeteKapiMVC5Core.DataAccessLayerAbone.Models
{
    public class users : IEntity
    {
        public users()
        {
            //orders = new List<orders>();
        }
        public int Id { get; set; }
        public string displayName { get; set; }
        public string namesurName { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public bool IsActive { get; set; }
        public DateTime UpdatedTime { get; set; }
        public DateTime CreatedTime { get; set; }

        [ForeignKey("roles")]
        public int rolId { get; set; }
        public rols roles { get; set; }
        //public virtual ICollection<orders> orders { get; set; }

    }
}
