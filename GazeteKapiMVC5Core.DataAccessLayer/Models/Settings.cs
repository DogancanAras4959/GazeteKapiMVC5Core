using DOMAIN.DataAccessLayer.Models;
using DOMAIN.DataAccessLayer.Models.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GazeteKapiMVC5Core.DataAccessLayer.Models
{
    public class Settings : IEntity
    {
        public Settings()
        {

        }
        public int Id { get; set; }
        public string Logo { get; set; }
        public bool LogIsActive { get; set; }
        public bool LogSystemErrorActive { get; set; }
        public bool GetAgencyNewsService { get; set; }
        public string SiteSlogan { get; set; }
        public string SiteName { get; set; }
        public bool IsActiveSettings { get; set; }
        public bool IsCurrencyService { get; set; }
        public string FooterLogo { get; set; }
        public string CopyrightText { get; set; }
        public string CopyrightTextTitle { get; set; }

        [ForeignKey("user")]
        public int UserId { get; set; }
        public Users user { get; set; }
    }
}
