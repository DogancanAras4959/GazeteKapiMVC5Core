using GazeteKapiMVC5Core.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GazeteKapiMVC5Core.Models.MenuItemTypes.TypeModel
{
    public class TypeBaseViewModel
    {
        public string MenuName { get; set; }
        public string Type { get; set; }
        public bool IsActive { get; set; }
        public int UserId { get; set; }
        public Users user { get; set; }
    }
}
