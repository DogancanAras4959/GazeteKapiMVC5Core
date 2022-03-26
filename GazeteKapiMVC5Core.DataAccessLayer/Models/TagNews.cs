using DOMAIN.DataAccessLayer.Models.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GazeteKapiMVC5Core.DataAccessLayer.Models
{
    public class TagNews : IEntity
    {
        public TagNews()
        {

        }
        public int Id { get; set; }

        [ForeignKey("news")]
        public int NewsId { get; set; }
        
        [ForeignKey("tag")]
        public int TagId { get; set; }

        public virtual Tags tag { get; set; }
        public virtual News news { get; set; }
    }
}
