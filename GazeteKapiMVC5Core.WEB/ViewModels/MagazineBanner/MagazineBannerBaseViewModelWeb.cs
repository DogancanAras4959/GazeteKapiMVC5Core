using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GazeteKapiMVC5Core.WEB.ViewModels.MagazineBanner
{
    public class MagazineBannerBaseViewModelWeb
    {
        public string BannerImage { get; set; }
        public string Link { get; set; }
        public bool IsActive { get; set; }
    }
}
