using System;
using System.Collections.Generic;
using System.Text;

namespace CORE.ApplicationCommon.DTOS.MagazineBannerDTO
{
    public class MagazineBannerBaseDto
    {
        public string BannerImage { get; set; }
        public string Link { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public bool IsActive { get; set; }
    }
}
