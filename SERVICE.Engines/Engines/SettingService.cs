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
        public Task<bool> editSiteSettings(int id)
        {
            throw new NotImplementedException();
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
