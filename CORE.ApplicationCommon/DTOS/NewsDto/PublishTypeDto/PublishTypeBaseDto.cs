using GazeteKapiMVC5Core.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CORE.ApplicationCommon.DTOS.NewsDTO.PublishTypeDTO
{
    public class PublishTypeBaseDto
    {
        public string TypeName { get; set; }
        public int UserId { get; set; }
        public Users user { get; set; }
    }
}
