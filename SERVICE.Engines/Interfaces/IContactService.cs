using CORE.ApplicationCommon.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SERVICE.Engine.Interfaces
{
    public interface IContactService
    {
        Task<string> SendFormToSubscribe(EmailConfig config);
    }
}
