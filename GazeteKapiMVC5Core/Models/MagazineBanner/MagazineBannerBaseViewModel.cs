using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GazeteKapiMVC5Core.Models.MagazineBanner
{
    public class MagazineBannerBaseViewModel
    {
        public string BannerImage { get; set; }
        public string Link { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public bool IsActive { get; set; }
    }
}
