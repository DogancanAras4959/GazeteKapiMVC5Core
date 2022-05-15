using DOMAIN.DataAccessLayer.Models.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GazeteKapiMVC5Core.DataAccessLayer.Models
{
    public class SeoScore : IEntity
    {
        public SeoScore()
        {
            seoMetas = new List<SeoCheckMeta>();
        }

        public string UniqeCode { get; set; }
        public int Id { get; set; }
        public string Note { get; set; }
        public int Amount { get; set; }
        public int Level { get; set; }
        public bool IsFinished { get; set; }
        public bool IsCreated { get; set; }

        [ForeignKey("news")]
        public int NewsId { get; set; }

        public News news { get; set; }
        public virtual ICollection<SeoCheckMeta> seoMetas { get; set; }

}
}
