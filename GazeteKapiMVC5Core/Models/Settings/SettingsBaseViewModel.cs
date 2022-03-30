using DOMAIN.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GazeteKapiMVC5Core.Models.Settings
{
    public class SettingsBaseViewModel
    {
        public string Logo { get; set; }
        public bool LogIsActive { get; set; }
        public bool LogSystemErrorActive { get; set; }
        public bool GetAgencyNewsService { get; set; }
        public string SiteSlogan { get; set; }
        public string SiteName { get; set; }
        public bool IsActiveSettings { get; set; }
        public int UserId { get; set; }
        public Users user { get; set; }
    }
}
