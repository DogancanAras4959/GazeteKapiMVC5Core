using CORE.ApplicationCommon.DTOS.CurrencyDTO;
using CORE.ApplicationCommon.DTOS.PrivacyDTO.AboutUsDto;
using CORE.ApplicationCommon.DTOS.PrivacyDTO.PolicyDto;
using CORE.ApplicationCommon.DTOS.PrivacyDTO.PrivacyDto;
using CORE.ApplicationCommon.DTOS.PrivacyDTO.TermsOfUsDto;
using CORE.ApplicationCommon.DTOS.SetingsDTO;
using CORE.ApplicationCore.UnitOfWork;
using GazeteKapiMVC5Core.DataAccessLayer;
using GazeteKapiMVC5Core.DataAccessLayer.Models;
using SERVICE.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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

        #region Setting Base

        public async Task<bool> createCurrencyList(CurrencyDto currencyDto)
        {
            Currency newCurrency = await _unitOfWork.GetRepository<Currency>().AddAsync(new Currency
            {
                code = currencyDto.code,
                crossorder = currencyDto.crossorder,
                CrossRateOther = currencyDto.CrossRateOther,
                CrossRateUSD = currencyDto.CrossRateUSD,
                BanknoteSelling = currencyDto.BanknoteSelling,
                BanknoteBuying = currencyDto.BanknoteBuying,
                ForexBuying = currencyDto.ForexBuying,
                ForexSelling = currencyDto.ForexSelling,
                name = currencyDto.name,
                isRateOrDown = "",
                unit = currencyDto.unit,
                currencyCode = currencyDto.currencyCode,
                currencyName = currencyDto.currencyName,

            });

            return newCurrency != null && newCurrency.Id != 0;
        }

        public List<CurrencyListItemDto> currencyLisToDatabase()
        {
            IEnumerable<Currency> newsList = _unitOfWork.GetRepository<Currency>().Filter(null, x => x.OrderBy(y => y.Id), "", null, null);

            if (newsList != null)
            {
                return newsList.Select(x => new CurrencyListItemDto
                {
                    Id = x.Id,
                    currencyCode = x.currencyCode,
                    name = x.name,
                    crossorder = x.crossorder,
                    CrossRateOther = x.CrossRateOther,
                    BanknoteBuying = x.BanknoteBuying,
                    BanknoteSelling = x.BanknoteSelling,
                    code = x.code,
                    currencyName = x.currencyName,
                    CrossRateUSD = x.CrossRateUSD,
                    ForexBuying = x.ForexBuying,
                    ForexSelling = x.ForexSelling,
                    unit = x.unit,
                    isRateOrDown = x.isRateOrDown,

                }).ToList();
            }
            else
            {
                return null;
            }
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

        public async Task<bool> editCurrencyList(CurrencyDto currencyDto)
        {
            Currency currencyGet = await _unitOfWork.GetRepository<Currency>().FindAsync(x => x.Id == currencyDto.Id);

            Currency getCurrency = await _unitOfWork.GetRepository<Currency>().UpdateAsync(new Currency
            {
                Id = currencyGet.Id,
                code = currencyGet.code,
                CrossRateUSD = currencyGet.CrossRateUSD,
                crossorder = currencyGet.crossorder,
                currencyCode = currencyGet.currencyCode,
                currencyName = currencyGet.currencyName,
                CrossRateOther = currencyGet.CrossRateOther,
                BanknoteBuying = currencyGet.BanknoteBuying,
                BanknoteSelling = currencyGet.BanknoteSelling,
                ForexBuying = currencyDto.ForexBuying,
                isRateOrDown = currencyDto.isRateOrDown,
                ForexSelling = currencyGet.ForexSelling,
                name = currencyGet.name,
                unit = currencyGet.unit,

            });

            return getCurrency != null;
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
                IsCurrencyService = model.IsCurrencyService,
                GetAgencyNewsService = model.GetAgencyNewsService,
                IsActiveSettings = model.IsActiveSettings,
                LogSystemErrorActive = model.LogSystemErrorActive,
                FooterLogo = model.FooterLogo,
                CopyrightText = model.CopyrightText,
                CopyrightTextTitle = model.CopyrightTextTitle,
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

        public CurrencyDto getCurrency(string code)
        {
            Currency getCurrency = _unitOfWork.GetRepository<Currency>().FindAsync(x => x.code == code).Result;

            if (getCurrency == null)
            {
                return null;
            }

            else
            {

                return new CurrencyDto
                {
                    Id = getCurrency.Id,
                    code = getCurrency.code,
                    BanknoteBuying = getCurrency.BanknoteBuying,
                    BanknoteSelling = getCurrency.BanknoteSelling,
                    crossorder = getCurrency.crossorder,
                    CrossRateOther = getCurrency.CrossRateOther,
                    currencyCode = getCurrency.currencyCode,
                    CrossRateUSD = getCurrency.CrossRateUSD,
                    currencyName = getCurrency.currencyName,
                    ForexBuying = getCurrency.ForexBuying,
                    ForexSelling = getCurrency.ForexSelling,
                    isRateOrDown = getCurrency.isRateOrDown,
                    name = getCurrency.name,
                    unit = getCurrency.unit,
                };
            }
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
                IsCurrencyService = getSetting.IsCurrencyService,
                SiteSlogan = getSetting.SiteSlogan,
                IsActiveSettings = getSetting.IsActiveSettings,
                GetAgencyNewsService = getSetting.GetAgencyNewsService,
                LogSystemErrorActive = getSetting.LogSystemErrorActive,
                FooterLogo = getSetting.FooterLogo,
                CopyrightText = getSetting.CopyrightText,
                CopyrightTextTitle = getSetting.CopyrightTextTitle,
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

        #endregion

        #region Policy

        public CookiePolicyDto getCookiePrivacy(int id)
        {
            CookiePolicy getPrivacy = _unitOfWork.GetRepository<CookiePolicy>().FindAsync(x => x.Id == id).Result;

            if (getPrivacy == null)
            {
                return new CookiePolicyDto();
            }

            return new CookiePolicyDto
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

        public async Task<bool> editCookiePrivacy(CookiePolicyDto model)
        {
            CookiePolicy privacyGet = await _unitOfWork.GetRepository<CookiePolicy>().FindAsync(x => x.Id == model.Id);

            CookiePolicy getPrivacy = await _unitOfWork.GetRepository<CookiePolicy>().UpdateAsync(new CookiePolicy
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

        public BrandPolicyDto getBrandPrivacy(int id)
        {
            BrandPolicy getPrivacy = _unitOfWork.GetRepository<BrandPolicy>().FindAsync(x => x.Id == id).Result;

            if (getPrivacy == null)
            {
                return new BrandPolicyDto();
            }

            return new BrandPolicyDto
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

        public async Task<bool> editBrandPrivacy(BrandPolicyDto model)
        {
            BrandPolicy privacyGet = await _unitOfWork.GetRepository<BrandPolicy>().FindAsync(x => x.Id == model.Id);

            BrandPolicy getPrivacy = await _unitOfWork.GetRepository<BrandPolicy>().UpdateAsync(new BrandPolicy
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

        public StreamPolicyDto getStreamPrivacy(int id)
        {
            StreamPolicy getPrivacy = _unitOfWork.GetRepository<StreamPolicy>().FindAsync(x => x.Id == id).Result;

            if (getPrivacy == null)
            {
                return new StreamPolicyDto();
            }

            return new StreamPolicyDto
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

        public async Task<bool> editStreamPrivacy(StreamPolicyDto model)
        {
            StreamPolicy privacyGet = await _unitOfWork.GetRepository<StreamPolicy>().FindAsync(x => x.Id == model.Id);

            StreamPolicy getPrivacy = await _unitOfWork.GetRepository<StreamPolicy>().UpdateAsync(new StreamPolicy
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

        #endregion
    }
}
