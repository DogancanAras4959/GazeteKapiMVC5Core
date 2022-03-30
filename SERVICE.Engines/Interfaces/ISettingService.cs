using CORE.ApplicationCommon.DTOS.SetingsDTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SERVICE.Engine.Interfaces
{
    public interface ISettingService
    {
        SettingsDto getSettings(int id);
        Task<bool> editSiteSettings(SettingsDto model);
    }
}
