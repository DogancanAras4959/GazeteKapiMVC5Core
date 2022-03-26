using DOMAIN.DataAccessLayer.Models;
using DOMAIN.DataAccessLayer.Models.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GazeteKapiMVC5Core.DataAccessLayer.Models
{
    public class PublishType : IEntity
    {
        public PublishType()
        {
            newList = new List<News>();
        }

        public int Id { get; set; }
        public string TypeName { get; set; }

        [ForeignKey("user")]
        public int UserId { get; set; }

        public virtual Users user { get; set; }

        public virtual ICollection<News> newList { get; set; }
    }
}
