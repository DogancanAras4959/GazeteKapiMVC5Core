using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GazeteKapiMVC5Core.WEB.ViewModels.Members
{
    public class MembersBaseViewModelWeb
    {
        public string nameSurname { get; set; }
        public string phoneNumber { get; set; }
        public string emailAdress { get; set; }
        public string description { get; set; }
        public string jobs { get; set; }

        public string ReCaptchaToken { get; set; }
        public DateTime submitDate { get; set; }
    }
}
