using CORE.ApplicationCommon.DTOS.RoleDTO;
using GazeteKapiMVC5Core.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CORE.ApplicationCommon.DTOS.AccountDTO
{
    public class UserBaseDto
    {
        public string DisplayName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string EmailAdress { get; set; }
        public string ProfileImage { get; set; }
        public bool IsActive { get; set; }
        public DateTime? UpdatedTime { get; set; }
        public DateTime? CreatedTime { get; set; }
        public Roles role { get; set; }
        public int RoleId { get; set; }
    }
}
