using DOMAIN.DataAccessLayer.Models.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GazeteKapiMVC5Core.DataAccessLayer.Models
{
    public class SeoCheckMeta : IEntity
    {
        public SeoCheckMeta()
        {

        }
        public int Id { get; set; }
        public string TypeLevel { get; set; }
        public string Requirement { get; set; }
        public int Point { get; set; }
        public bool IsDone { get; set; }

        [ForeignKey("seoScoreToMeta")]
        public int SeoScoreId { get; set; }

        public SeoScore seoScoreToMeta { get; set; }
    }
}
