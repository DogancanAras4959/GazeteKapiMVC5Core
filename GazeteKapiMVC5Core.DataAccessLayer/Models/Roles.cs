using DOMAIN.DataAccessLayer.Models.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GazeteKapiMVC5Core.DataAccessLayer.Models
{
    public class Roles : IEntity
    {
        public Roles()
        {
            usersForRoles = new List<Users>();
            roleAuthroizeForRole = new List<RoleAuthorize>();
        }

        public int Id { get; set; }

        public string RoleName { get; set; }

        public bool IsActive { get; set; }

        public DateTime? CreatedTime { get; set; }

        public DateTime? UpdatedTime { get; set; }

        public virtual ICollection<Users> usersForRoles { get; set; }
        public virtual ICollection<RoleAuthorize> roleAuthroizeForRole { get; set; }
    }
}
