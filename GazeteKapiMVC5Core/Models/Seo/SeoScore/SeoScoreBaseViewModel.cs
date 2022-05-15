using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GazeteKapiMVC5Core.Models.Seo.SeoScore
{
    public class SeoScoreBaseViewModel
    {
        public int Id { get; set; }
        public int NewsId { get; set; }
        public string Note { get; set; }
        public int Amount { get; set; }
        public int Level { get; set; }
        public bool IsFinished { get; set; }
        public string UniqueCode { get; set; }
        public bool IsCreated { get; set; }

        public GazeteKapiMVC5Core.DataAccessLayer.Models.News newsToSeo { get; set; }
    
    }
}
