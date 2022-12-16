using DOMAIN.DataAccessLayer.Models.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace GazeteKapiMVC5Core.DataAccessLayer.Models
{
    public class IpAddresCount : IEntity
    {
        public IpAddresCount()
        {
            newsIpListIp = new List<NewsIp>();
        }
        public int Id { get; set; }
        public string ipAddress { get; set; }
        public virtual ICollection<NewsIp> newsIpListIp { get; set; }

    }
}
