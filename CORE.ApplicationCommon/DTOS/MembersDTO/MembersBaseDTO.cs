using System;
using System.Collections.Generic;
using System.Text;

namespace CORE.ApplicationCommon.DTOS.MembersDTO
{
    public class MembersBaseDTO
    {
        public string nameSurname { get; set; }
        public string phoneNumber { get; set; }
        public string emailAdress { get; set; }
        public string description { get; set; }
        public string jobs { get; set; }
        public DateTime submitDate { get; set; }
    }
}
