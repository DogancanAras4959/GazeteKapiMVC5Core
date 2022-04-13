using GazeteKapiMVC5Core.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CORE.ApplicationCommon.DTOS.AuthorizeRoleDto
{
    public class AuthorizeRoleBaseDto
    {
        public int RoleId { get; set; }
        public int AuthorizeId { get; set; }
        public bool IsChecked { get; set; }

        public Roles role { get; set; }
        public Authorizes authorize { get; set; }
    }
}
