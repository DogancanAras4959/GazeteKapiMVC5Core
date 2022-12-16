using DOMAIN.DataAccessLayer.Models.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GazeteKapiMVC5Core.DataAccessLayer.Models
{
    public class Banners : IEntity
    {
        public Banners()
        {

        }
        
        public int Id { get; set; }


        [ForeignKey("bannerRotate")]
        public int RotateId { get; set; }
        
        public string BannerImage { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public string BannerFrame { get; set; }
        public bool IsActive { get; set; }
        public string Link { get; set; }
        public string BannerName { get; set; }

        public BannersRotate bannerRotate { get; set; }
    }
}
