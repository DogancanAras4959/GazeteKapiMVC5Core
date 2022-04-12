using DOMAIN.DataAccessLayer.Models;
using DOMAIN.DataAccessLayer.Models.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GazeteKapiMVC5Core.DataAccessLayer.Models
{
    public class MenuTypes : IEntity
    {
        public MenuTypes()
        {
            items = new List<MenuItems>();
        }
        public int Id { get; set; }
        public string MenuName { get; set; }
        public string Type { get; set; }
        public bool IsActive { get; set; }
        
        [ForeignKey("user")]
        public int UserId { get; set; }
        public Users user { get; set; }

        public ICollection<MenuItems> items { get; set; }
    }
}
