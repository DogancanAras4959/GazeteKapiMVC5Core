using DOMAIN.DataAccessLayer.Models.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DOMAIN.DataAccessLayer.Models
{
    public class Authorizes : IEntity
    {
        public Authorizes()
        {
            roleAuthroizeForRole = new List<RoleAuthorize>();
        }
        public int Id { get; set; }
        public string AuthorizeName { get; set; }
        public string AuthorizeCode { get; set; }
        public virtual ICollection<RoleAuthorize> roleAuthroizeForRole { get; set; }
    }
}
