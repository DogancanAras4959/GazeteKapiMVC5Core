using DOMAIN.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GazeteKapiMVC5Core.Models.News.GuestModel
{
    public class GuestBaseViewModel
    {
        public string GuestName { get; set; }
        public string GuestImage { get; set; }
        public string Biography { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public DateTime? UpdatedTime { get; set; }
        public DateTime? CreatedTime { get; set; }
        public int UserId { get; set; }
        public Users user { get; set; }
    }
}
