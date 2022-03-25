using DOMAIN.DataAccessLayer.Models;
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
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public int UserId { get; set; }
        public Users user { get; set; }
    }
}
