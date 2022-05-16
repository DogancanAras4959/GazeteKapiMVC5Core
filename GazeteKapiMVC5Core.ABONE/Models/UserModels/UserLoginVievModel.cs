using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GazeteKapiMVC5Core.ABONE.Models.UserModels
{
    public class UserLoginVievModel
    {
        [DisplayName("Kullanıcı Adı")]
        [Required(ErrorMessage = "Kullanıcı Adı gereklidir.")]
        public string userName { get; set; }

        [Required(ErrorMessage = "Şifre gereklidir.")]
        [Display(Name = "Şifre")]
        [DataType(DataType.Password)]
        public string password { get; set; }
    }
}
