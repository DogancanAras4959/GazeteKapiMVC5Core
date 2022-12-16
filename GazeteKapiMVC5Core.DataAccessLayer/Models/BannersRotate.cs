using DOMAIN.DataAccessLayer.Models.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GazeteKapiMVC5Core.DataAccessLayer.Models
{
    public class BannersRotate : IEntity
    {
        public BannersRotate()
        {
            bannerList = new List<Banners>();
        }

        public int Id { get; set; }

        public string RotateName { get; set; }
       

        public virtual ICollection<Banners> bannerList { get; set; }
    }
}
