﻿using GazeteKapiMVC5Core.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CORE.ApplicationCommon.DTOS.PrivacyDTO.TermsOfUsDto
{
    public class TermsOfUsBaseDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public int UserId { get; set; }
        public Users user { get; set; }
    }
}
