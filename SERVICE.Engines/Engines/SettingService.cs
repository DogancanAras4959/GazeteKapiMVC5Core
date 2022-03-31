using CORE.ApplicationCommon.DTOS.PrivacyDTO.AboutUsDto;
using CORE.ApplicationCommon.DTOS.PrivacyDTO.PrivacyDto;
using CORE.ApplicationCommon.DTOS.PrivacyDTO.TermsOfUsDto;
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

        public async Task<bool> editAboutUs(AboutUsDto model)
        {
            AboutUs aboutUsGet = await _unitOfWork.GetRepository<AboutUs>().FindAsync(x => x.Id == model.Id);

            AboutUs getAboutUs = await _unitOfWork.GetRepository<AboutUs>().UpdateAsync(new AboutUs
            {
                Id = model.Id,
                CreatedTime = aboutUsGet.CreatedTime,
                UpdatedTime = model.UpdatedTime,
                Content = model.Content,
                Title = model.Title,
                UserId = model.UserId,
                user = model.user,
            });

            return getAboutUs != null;
        }

        public async Task<bool> editPrivacy(PrivacyDto model)
        {
            Privacy privacyGet = await _unitOfWork.GetRepository<Privacy>().FindAsync(x => x.Id == model.Id);

            Privacy getPrivacy = await _unitOfWork.GetRepository<Privacy>().UpdateAsync(new Privacy
            {
                Id = model.Id,
                CreatedTime = privacyGet.CreatedTime,
                UpdatedTime = model.UpdatedTime,
                Content = model.Content,
                Title = model.Title,
                UserId = model.UserId,
                user = model.user,
            });

            return getPrivacy != null;
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

        public async Task<bool> editTermsOfUs(TermsOfUsDto model)
        {
            TermsOfUse termsOfUsGet = await _unitOfWork.GetRepository<TermsOfUse>().FindAsync(x => x.Id == model.Id);

            TermsOfUse getTermsOfUs = await _unitOfWork.GetRepository<TermsOfUse>().UpdateAsync(new TermsOfUse
            {
                Id = model.Id,
                CreatedTime = termsOfUsGet.CreatedTime,
                UpdatedTime = model.UpdatedTime,
                Content = model.Content,
                Title = model.Title,
                UserId = model.UserId,
                user = model.user,
            });

            return getTermsOfUs != null;
        }

        public AboutUsDto getAboutUs(int id)
        {
            AboutUs getAboutUs = _unitOfWork.GetRepository<AboutUs>().FindAsync(x => x.Id == id).Result;

            if (getAboutUs == null)
            {
                return new AboutUsDto();
            }

            return new AboutUsDto
            {
                Id = getAboutUs.Id,
                Content = getAboutUs.Content,
                Title = getAboutUs.Title,
                CreatedTime = getAboutUs.CreatedTime,
                UpdatedTime = getAboutUs.UpdatedTime,
                UserId = getAboutUs.UserId,
                user = getAboutUs.user,
            };
        }

        public PrivacyDto getPrivacy(int id)
        {
            Privacy getPrivacy = _unitOfWork.GetRepository<Privacy>().FindAsync(x => x.Id == id).Result;

            if (getPrivacy == null)
            {
                return new PrivacyDto();
            }

            return new PrivacyDto
            {
                Id = getPrivacy.Id,
                Content = getPrivacy.Content,
                Title = getPrivacy.Title,
                CreatedTime = getPrivacy.CreatedTime,
                UpdatedTime = getPrivacy.UpdatedTime,
                UserId = getPrivacy.UserId,
                user = getPrivacy.user,
            };
        }

        public SettingsDto getSettings(int id)
        {
            Settings getSetting = _unitOfWork.GetRepository<Settings>().FindAsync(x => x.Id == id).Result;

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

        public TermsOfUsDto getTermsOfUs(int id)
        {
            TermsOfUse getTermsOfUs = _unitOfWork.GetRepository<TermsOfUse>().FindAsync(x => x.Id == id).Result;

            if (getTermsOfUs == null)
            {
                return new TermsOfUsDto();
            }

            return new TermsOfUsDto
            {
                Id = getTermsOfUs.Id,
                Content = getTermsOfUs.Content,
                Title = getTermsOfUs.Title,
                CreatedTime = getTermsOfUs.CreatedTime,
                UpdatedTime = getTermsOfUs.UpdatedTime,
                UserId = getTermsOfUs.UserId,
                user = getTermsOfUs.user,
            };
        }
    }
}
