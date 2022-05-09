using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GazeteKapiMVC5Core.WEB.Models.MetaConfig
{
    public class MetaViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Keywords { get; set; }
        public string ogTitle { get; set; }
        public string ogDescription { get; set; }
        public string ogImage { get; set; }
        public string Url { get; set; }
    }
}
