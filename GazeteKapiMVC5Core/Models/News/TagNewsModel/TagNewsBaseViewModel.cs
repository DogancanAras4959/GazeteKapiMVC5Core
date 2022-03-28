using GazeteKapiMVC5Core.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GazeteKapiMVC5Core.Models.News.TagNewsModel
{
    public class TagNewsBaseViewModel
    {
        public int TagId { get; set; }
        public int NewsId { get; set; }

        public Tags tag { get; set; }
        public DataAccessLayer.Models.News news { get; set; }
    }
}
