﻿using DOMAIN.DataAccessLayer.Models.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GazeteKapiMVC5Core.DataAccessLayer.Models
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

        public virtual Roles role { get; set; }
        public virtual Authorizes authroize { get; set; }
    }
}
