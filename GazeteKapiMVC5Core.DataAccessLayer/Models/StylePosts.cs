using DOMAIN.DataAccessLayer.Models.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace GazeteKapiMVC5Core.DataAccessLayer.Models
{
    public class StylePosts : IEntity
    {
        public StylePosts()
        {
            categories = new List<Categories>();
        }
        public int Id { get; set; }
        public string StyleName { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<Categories> categories { get; set; }
    }
}
