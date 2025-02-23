﻿using GazeteKapiMVC5Core.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CORE.ApplicationCommon.DTOS.SeoDTO.SeoMetaDto
{
    public class SeoMetaBaseDto
    {
        public string TypeLevel { get; set; }
        public string Requirement { get; set; }
        public int Point { get; set; }
        public int SeoScoreId { get; set; }
        public string metaCode { get; set; }

        public bool IsDone { get; set; }
        public SeoScore seoScore { get; set; }
    }
}
