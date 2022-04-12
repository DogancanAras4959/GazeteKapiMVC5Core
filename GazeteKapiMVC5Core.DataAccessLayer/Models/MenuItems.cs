using DOMAIN.DataAccessLayer.Models.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GazeteKapiMVC5Core.DataAccessLayer.Models
{
    public class MenuItems : IEntity
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string slug { get; set; }

        [ForeignKey("type")]
        public int TypeId { get; set; }
        public bool IsActive { get; set; }

        public MenuTypes type { get; set; }
    }
}
