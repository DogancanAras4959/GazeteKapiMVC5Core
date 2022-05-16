using DOMAIN.DataAccessLayer.Models.Core;
using GazeteKapiMVC5Core.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GazeteKapiMVC5Core.DataAccessLayer.Models
{
    public class Categories : IEntity
    {
        public Categories()
        {
            newList = new List<News>();
        }

        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public int? ParentCategoryId { get; set; }
        public bool IsActive { get; set; }
        public string Image { get; set; }
        public int Sorted { get; set; }
        public int Position { get; set; }

        [ForeignKey("typeToCategories")]
        public int? TypeId { get; set; }

        [ForeignKey("stylePosts")]
        public int? StyleId { get; set; }

        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        
        [ForeignKey("user")]
        public int UserId { get; set; }
        public Users user { get; set; }

        public FooterType typeToCategories { get; set; }
        public StylePosts stylePosts { get; set; }
        public virtual ICollection<News> newList { get; set; } 

    }
}
