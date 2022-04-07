using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GazeteKapiMVC5Core.WEB.Models.ConfigRss
{
    public class rss
    {
        public int Id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string link { get; set; }
        public string date { get; set; }
    }
}
