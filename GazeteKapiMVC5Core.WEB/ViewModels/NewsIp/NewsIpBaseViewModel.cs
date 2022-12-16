using GazeteKapiMVC5Core.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GazeteKapiMVC5Core.WEB.ViewModels.NewsIp
{
    public class NewsIpBaseViewModel
    {
        public int NewsId { get; set; }
        public int IpAdressId { get; set; }
        public GazeteKapiMVC5Core.DataAccessLayer.Models.News news { get; set; }
        public IpAddresCount ip { get; set; }
    }
}
