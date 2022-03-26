using GazeteKapiMVC5Core.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CORE.ApplicationCommon.DTOS.NewsDTO.TagNewsDTO
{
    public class TagNewsBaseDto
    {
        public int TagId { get; set; }
        public int NewsId { get; set; }

        public Tags tag { get; set; }
        public News news { get; set; }
    }
}
