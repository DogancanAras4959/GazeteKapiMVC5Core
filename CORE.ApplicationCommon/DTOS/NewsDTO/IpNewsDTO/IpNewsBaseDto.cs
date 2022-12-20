using GazeteKapiMVC5Core.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CORE.ApplicationCommon.DTOS.NewsDTO.IpNewsDTO
{
    public class IpNewsBaseDto
    {
        public int NewsId { get; set; }
        public int IpAdressId { get; set; }
        public DateTime ClickTime { get; set; }
        public IpAddresCount ipAdress { get; set; }
        public News news { get; set; }
    }
}
