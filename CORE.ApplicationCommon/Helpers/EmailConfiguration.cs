﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CORE.ApplicationCommon.Helpers
{
    public class EmailConfiguration
    {
        public string From { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool UseSSL { get; set; }
    }
}
