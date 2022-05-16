using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GazeteKapiMVC5Core.Models.News.MediaModel
{
    public class MediaBaseViewModel
    {
        public int UserId { get; set; }
        public DateTime UpdatedTime { get; set; }
        public DateTime CreatedTime { get; set; }
        public string Slug { get; set; }
        public string Title { get; set; }
    }
}
