using GazeteKapiMVC5Core.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CORE.ApplicationCommon.DTOS.CategoryDTO
{
    public class CategoryBaseDto
    {
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public int? ParentCategoryId { get; set; }
        public bool IsActive { get; set; }
        public int Sorted { get; set; }

        public string Image { get; set; }
        public int Position { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
       
        public int UserId { get; set; }
        public int? StyleId { get; set; }
        public int? TypeId { get; set; }

        public StylePosts styles { get; set; }
        public Users user { get; set; }
        public FooterType typeFooter { get; set; }
    }
}
