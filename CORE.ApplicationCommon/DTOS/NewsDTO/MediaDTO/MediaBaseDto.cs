using System;
using System.Collections.Generic;
using System.Text;

namespace CORE.ApplicationCommon.DTOS.NewsDTO.MediaDTO
{
    public class MediaBaseDto
    {
        public int Id { get; set; }
        public DateTime UpdatedTime { get; set; }
        public DateTime CreatedTime { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Extension { get; set; }
        public int UserId { get; set; }
    
    }
}
