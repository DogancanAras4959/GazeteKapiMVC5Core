using DOMAIN.DataAccessLayer.Models.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace GazeteKapiMVC5Core.DataAccessLayer.Models
{
    public class members : IEntity
    {
        public int Id { get; set; }
        public string nameSurname { get; set; }
        public string phoneNumber { get; set; }
        public string emailAdress { get; set; }
        public string description { get; set; }
        public string jobs { get; set; }
        public DateTime submitDate { get; set; }
        
    }
}
