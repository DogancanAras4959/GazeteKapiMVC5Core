using GazeteKapiMVC5Core.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GazeteKapiMVC5Core.Models.MenuItemTypes.ItemModel
{
    public class ItemBaseViewModel
    {
        public string ItemName { get; set; }
        public string slug { get; set; }
        public int TypeId { get; set; }
        public bool IsActive { get; set; }
        public MenuTypes type { get; set; }
    }
}
