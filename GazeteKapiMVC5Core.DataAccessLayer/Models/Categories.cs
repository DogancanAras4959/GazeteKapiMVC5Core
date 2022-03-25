using DOMAIN.DataAccessLayer.Models.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DOMAIN.DataAccessLayer.Models
{
    public class Categories : IEntity
    {
        public Categories()
        {

        }

        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public int? ParentCategoryId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        
        [ForeignKey("user")]
        public int UserId { get; set; }
        public Users user { get; set; }
    }
}
