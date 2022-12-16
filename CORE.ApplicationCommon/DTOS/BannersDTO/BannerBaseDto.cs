using GazeteKapiMVC5Core.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CORE.ApplicationCommon.DTOS.BannersDTO
{
    public class BannerBaseDto
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
