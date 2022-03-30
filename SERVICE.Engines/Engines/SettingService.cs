using CORE.ApplicationCommon.DTOS.SetingsDTO;
using CORE.ApplicationCore.UnitOfWork;
using GazeteKapiMVC5Core.DataAccessLayer;
using GazeteKapiMVC5Core.DataAccessLayer.Models;
using SERVICE.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SERVICE.Engine.Engines
{
    public class SettingService : ISettingService
    {
        private readonly IUnitOfWork<NewsAppContext> _unitOfWork;
        public SettingService(IUnitOfWork<NewsAppContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> editSiteSettings(SettingsDto model)
        {
            Settings settingsGet = await _unitOfWork.GetRepository<Settings>().FindAsync(x => x.Id == model.Id);

            if (model.Logo == null)
            {
                model.Logo = settingsGet.Logo;
            }

            Settings getSettings = await _unitOfWork.GetRepository<Settings>().UpdateAsync(new Settings
            {
                Id = model.Id,
                Logo = model.Logo,
                SiteName = model.SiteName,
                SiteSlogan = model.SiteSlogan,
                LogIsActive = model.LogIsActive,
                GetAgencyNewsService = model.GetAgencyNewsService,
                IsActiveSettings = model.IsActiveSettings,
                LogSystemErrorActive = model.LogSystemErrorActive,
                UserId = model.UserId,
                user = model.user,
            });

            return getSettings != null;
        }
        public SettingsDto getSettings(int id)
        {
            Settings getSetting =  _unitOfWork.GetRepository<Settings>().FindAsync(x => x.Id == id).Result;

            if (getSetting == null)
            {
                return new SettingsDto();
            }

            return new SettingsDto
            {
                Id = getSetting.Id,
                LogIsActive = getSetting.LogIsActive,
                Logo = getSetting.Logo,
                SiteName = getSetting.SiteName,
                SiteSlogan = getSetting.SiteSlogan,
                IsActiveSettings = getSetting.IsActiveSettings,
                GetAgencyNewsService = getSetting.GetAgencyNewsService,
                LogSystemErrorActive = getSetting.LogSystemErrorActive,
                UserId = getSetting.UserId,
                user = getSetting.user,                
            };
        }
    }
}
