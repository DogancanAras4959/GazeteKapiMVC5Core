using GazeteKapiMVC5Core.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GazeteKapiMVC5Core.Models.AuthorizeRole
{
    public class AuthorizeRoleBaseModel
    {
        public int RoleId { get; set; }
        public int AuthorizeId { get; set; }
        public bool isChecked { get; set; }
        public Roles role { get; set; }
        public Authorizes authorize { get; set; }
    }
}
