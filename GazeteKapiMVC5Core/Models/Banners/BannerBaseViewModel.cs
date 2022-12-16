using GazeteKapiMVC5Core.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GazeteKapiMVC5Core.Models.Banners
{
    public class BannerBaseViewModel
    {
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
