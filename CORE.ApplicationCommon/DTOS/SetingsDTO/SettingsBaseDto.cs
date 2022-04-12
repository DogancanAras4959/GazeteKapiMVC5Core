﻿using DOMAIN.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CORE.ApplicationCommon.DTOS.SetingsDTO
{
    public class SettingsBaseDto
    {
        public string Logo { get; set; }
        public bool LogIsActive { get; set; }
        public bool IsCurrencyService { get; set; }
        public bool LogSystemErrorActive { get; set; }
        public bool GetAgencyNewsService { get; set; }
        public string SiteSlogan { get; set; }
        public string SiteName { get; set; }
        public bool IsActiveSettings { get; set; }
        public string FooterLogo { get; set; }
        public string CopyrightText { get; set; }
        public string CopyrightTextTitle { get; set; }
        public int UserId { get; set; }
        public Users user { get; set; }
    }
}
