using DOMAIN.DataAccessLayer.Models.Core;
using GazeteKapiMVC5Core.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DOMAIN.DataAccessLayer.Models
{
    public class Users : IEntity
    {
        public Users()
        {
            categoriesList = new List<Categories>();
            guestList = new List<Guest>();
            newsList = new List<News>();
            publishTypeList = new List<PublishType>();

        }

        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string EmailAdress { get; set; }
        public string ProfileImage { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? UpdatedTime { get; set; }
        public bool IsActive { get; set; }
       
        [ForeignKey("roles")]
        public int RoleId { get; set; }
        public virtual Roles roles { get; set; }

        public virtual ICollection<Categories> categoriesList { get; set; }
        public virtual ICollection<Guest> guestList { get; set; }
        public virtual ICollection<News> newsList { get; set; }
        public virtual ICollection<PublishType> publishTypeList { get; set; }

    }
}
