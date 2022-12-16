using DOMAIN.DataAccessLayer.Models.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GazeteKapiMVC5Core.DataAccessLayer.Models
{
    public class NewsIp : IEntity
    {
        public NewsIp()
        {

        }

        public int Id { get; set; }

        [ForeignKey("news")]
        public int NewsId { get; set; }

        [ForeignKey("ip")]
        public int IpAdressId { get; set; }

        public News news { get; set; }
        public IpAddresCount ip { get; set; }
    }
}
