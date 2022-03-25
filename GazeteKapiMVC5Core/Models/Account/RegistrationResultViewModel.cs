using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GazeteKapiMVC5Core.Models.Account
{
    public class RegistrationResultViewModel
    {
        public bool? IsSuccess { get; set; }

        public string Message { get; set; }
    }
}
