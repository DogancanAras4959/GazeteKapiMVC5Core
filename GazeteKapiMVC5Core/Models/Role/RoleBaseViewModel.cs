using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GazeteKapiMVC5Core.Models.Role
{
    public class RoleBaseViewModel
    {
        [Required(ErrorMessage = "Rol gereklidir.")]
        [DisplayName("Rol Adı")]
        public string RoleName { get; set; }
        public bool IsActive { get; set; }

        [DisplayName("Güncellenme")]
        [DataType(DataType.Date)]
        public DateTime? UpdatedTime { get; set; }

        [DisplayName("Oluşturulma")]
        [DataType(DataType.Date)]
        public DateTime? CreatedTime { get; set; }
    }
}
