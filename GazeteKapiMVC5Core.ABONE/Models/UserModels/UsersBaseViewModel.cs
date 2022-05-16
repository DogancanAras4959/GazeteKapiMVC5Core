using GazeteKapiMVC5Core.DataAccessLayerAbone.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GazeteKapiMVC5Core.ABONE.Models.UserModels
{
    public class UsersBaseViewModel
    {
        [DisplayName("Görünen İsim")]
        public string displayName { get; set; }

        [Required(ErrorMessage = "Kullanıcı adı gereklidir.")]
        [DisplayName("Görünen İsim")]
        public string username { get; set; }

        [DisplayName("Şifre")]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [DisplayName("Şifre Tekrar")]
        [DataType(DataType.Password)]
        public string rePassword { get; set; }

        [DisplayName("Email Adresi")]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }

        [DisplayName("Telefon Numarası")]
        [DataType(DataType.PhoneNumber)]
        public string phonenumber { get; set; }
        public bool IsActive { get; set; }
        public int rolId { get; set; }

        [DisplayName("Oluşturulma")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedTime { get; set; }

        [DisplayName("Güncellenme")]
        [DataType(DataType.DateTime)]
        public DateTime UpdatedTime { get; set; }
        public rols rols { get; set; }
    }
}
