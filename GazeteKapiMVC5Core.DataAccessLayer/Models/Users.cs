﻿using DOMAIN.DataAccessLayer.Models.Core;
using GazeteKapiMVC5Core.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GazeteKapiMVC5Core.DataAccessLayer.Models
{
    public class Users : IEntity
    {
        public Users()
        {
            categoriesList = new List<Categories>();
            guestList = new List<Guest>();
            newsList = new List<News>();
            publishTypeList = new List<PublishType>();
            settings = new List<Settings>();
            privacy = new List<Privacy>();
            aboutus = new List<AboutUs>();
            termsofuse = new List<TermsOfUse>();
            brand = new List<BrandPolicy>();
            cookie = new List<CookiePolicy>();
            stream = new List<StreamPolicy>();
            media = new List<Media>();
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
        public virtual ICollection<Settings> settings { get; set; }
        public virtual ICollection<Privacy> privacy { get; set; }
        public virtual ICollection<AboutUs> aboutus { get; set; }
        public virtual ICollection<TermsOfUse> termsofuse { get; set; }
        public virtual ICollection<BrandPolicy> brand { get; set; }
        public virtual ICollection<CookiePolicy> cookie { get; set; }
        public virtual ICollection<StreamPolicy> stream { get; set; }
        public virtual ICollection<Media> media { get; set; }
    }
}
