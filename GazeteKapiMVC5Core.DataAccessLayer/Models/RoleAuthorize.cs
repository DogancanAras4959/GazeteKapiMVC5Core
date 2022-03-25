using DOMAIN.DataAccessLayer.Models.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DOMAIN.DataAccessLayer.Models
{
    public class RoleAuthorize : IEntity
    {
        public RoleAuthorize()
        {

        }
        public int Id { get; set; }

        [ForeignKey("role")]
        public int RoleId { get; set; }

        [ForeignKey("authorize")]
        public int AuthorizeId { get; set; }

        public bool IsChecked { get; set; }

        public Roles role { get; set; }
        public Authorizes authroize { get; set; }
    }
}
