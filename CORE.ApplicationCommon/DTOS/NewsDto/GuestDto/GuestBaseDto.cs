using DOMAIN.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CORE.ApplicationCommon.DTOS.NewsDTO.GuestDTO
{
    public class GuestBaseDto
    {
        public string GuestName { get; set; }
        public string GuestImage { get; set; }
        public string Biography { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public DateTime? UpdatedTime { get; set; }
        public DateTime? CreatedTime { get; set; }

        [ForeignKey("user")]
        public int UserId { get; set; }

        public Users user { get; set; }
    }
}
