using DOMAIN.DataAccessLayer.Models;
using DOMAIN.DataAccessLayer.Models.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GazeteKapiMVC5Core.DataAccessLayer.Models
{
    public class Guest : IEntity
    {
        public Guest()
        {
            newList = new List<News>();
        }

        public int Id { get; set; }
        public string GuestName { get; set; }
        public string GuestImage { get; set; }
        public string Biography { get; set; }
        public string Email { get; set; }

        public bool IsActive { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? UpdatedTime { get; set; }

        [ForeignKey("users")]
        public int UserId { get; set; }

        public virtual Users users { get; set; }

        public virtual ICollection<News> newList { get; set; }
    }
}
