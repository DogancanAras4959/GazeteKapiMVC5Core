using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GazeteKapiMVC5Core.Models.Seo.SeoMeta
{
    public class SeoMetaBaseViewModel
    {
        public string TypeLevel { get; set; }
        public int SeoScoreId { get; set; }
        public int Point { get; set; }
        public string Requirement { get; set; }
        public string metaCode { get; set; }

        public bool IsDone { get; set; }

    }
}
