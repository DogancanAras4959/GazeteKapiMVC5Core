using DOMAIN.DataAccessLayer.Models.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GazeteKapiMVC5Core.DataAccessLayer.Models
{
    public class Media : IEntity
    {
        public Media()
        {

        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Extension { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }

        [ForeignKey("user")]
        public int userId{ get; set; }
        public Users user { get; set; }
    }
}
