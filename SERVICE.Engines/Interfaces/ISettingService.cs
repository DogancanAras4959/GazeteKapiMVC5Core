using CORE.ApplicationCommon.DTOS.CurrencyDTO;
using CORE.ApplicationCommon.DTOS.PrivacyDTO.AboutUsDto;
using CORE.ApplicationCommon.DTOS.PrivacyDTO.PrivacyDto;
using CORE.ApplicationCommon.DTOS.PrivacyDTO.TermsOfUsDto;
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

        AboutUsDto getAboutUs(int id);
        Task<bool> editAboutUs(AboutUsDto model);

        PrivacyDto getPrivacy(int id);
        Task<bool> editPrivacy(PrivacyDto model);

        TermsOfUsDto getTermsOfUs(int id);
        Task<bool> editTermsOfUs(TermsOfUsDto model);
        Task<bool> createCurrencyList(CurrencyDto currencyDto);
        List<CurrencyListItemDto> currencyLisToDatabase();
        CurrencyDto getCurrency(string code);
        Task<bool> editCurrencyList(CurrencyDto currencyDto);
    }
}
