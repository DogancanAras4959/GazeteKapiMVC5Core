using DOMAIN.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GazeteKapiMVC5Core.Models.Account
{
    public class UserBaseViewModel
    {
        [Required(ErrorMessage = "Görünün ad gereklidir.")]
        [DisplayName("Görünen İsim")]
        public string DisplayName { get; set; }

        [Required(ErrorMessage = "Kullanıcı adı gereklidir.")]
        [DisplayName("Görünen İsim")]
        public string UserName { get; set; }

        [DisplayName("Şifre")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("Şifre Tekrar")]
        [DataType(DataType.Password)]
        public string RePassword { get; set; }

        [Required(ErrorMessage = "Email Adresi gereklidir.")]
        [DisplayName("Email Adresi")]
        [DataType(DataType.EmailAddress)]
        public string EmailAdress { get; set; }
        public string ProfileImage { get; set; }

        public bool IsActive { get; set; }

        public int RoleId { get; set; }

        [DisplayName("Güncellenme")]
        [DataType(DataType.DateTime)]
        public DateTime? UpdatedTime { get; set; }

        [DisplayName("Oluşturulma")]
        [DataType(DataType.DateTime)]
        public DateTime? CreatedTime { get; set; }

        public Roles roles { get; set; }
    }
}
