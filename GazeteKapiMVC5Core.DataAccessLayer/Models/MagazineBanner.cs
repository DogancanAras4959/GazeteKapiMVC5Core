using DOMAIN.DataAccessLayer.Models.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace GazeteKapiMVC5Core.DataAccessLayer.Models
{
    public class Magazinebanner : IEntity
    {
        public Magazinebanner()
        {

        }
        public int Id { get; set; }
        public string BannerImage { get; set; }
        public string Link { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
       
    }
}
