using GazeteKapiMVC5Core.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CORE.ApplicationCommon.DTOS.AuthorizeDTO
{
    public class AuthorizeBaseDto
    {
        public string AuthorizeName { get; set; }
        public string AuthorizeCode { get; set; }

    }
}
