using DOMAIN.DataAccessLayer.Models.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace GazeteKapiMVC5Core.DataAccessLayer.Models
{
    public class FooterType : IEntity
    {
        public FooterType()
        {
            categoriesList = new List<Categories>();
        }
        public int Id { get; set; }
        public string TypeName { get; set; }
        public bool IsActive { get; set; }

        public ICollection<Categories> categoriesList { get; set; }
    }
}
