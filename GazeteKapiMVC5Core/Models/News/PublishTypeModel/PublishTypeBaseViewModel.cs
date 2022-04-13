using GazeteKapiMVC5Core.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GazeteKapiMVC5Core.Models.News.PublishTypeModel
{
    public class PublishTypeBaseViewModel
    {
        public string TypeName { get; set; }
        public int UserId { get; set; }
        public Users user { get; set; }
    }
}
