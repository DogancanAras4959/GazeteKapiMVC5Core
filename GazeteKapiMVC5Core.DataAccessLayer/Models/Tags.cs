using DOMAIN.DataAccessLayer.Models.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace GazeteKapiMVC5Core.DataAccessLayer.Models
{
    public class Tags : IEntity
    {
        public Tags()
        {
            tagNewsForTag = new List<TagNews>();
        }
        public int Id { get; set; }
        public string TagName { get; set; }

        public virtual ICollection<TagNews> tagNewsForTag { get; set; }

    }
}
