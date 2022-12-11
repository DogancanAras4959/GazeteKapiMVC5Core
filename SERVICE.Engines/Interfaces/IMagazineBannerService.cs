using CORE.ApplicationCommon.DTOS.MagazineBannerDTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SERVICE.Engine.Interfaces
{
    public interface IMagazineBannerService
    {
        List<MagazineBannerListItemDto> magazineBannerList();
        List<MagazineBannerListItemDto> magazineListTakeOneToWeb(int count);
        MagazineBannerDto getMagazine(int Id);
        Task<bool> createMagazineBanner(MagazineBannerDto model);
        Task<bool> editMagazineBannerDto(MagazineBannerDto model);
        Task<bool> IsActiveEnabledMagazine(int id);
        bool DeleteBanner(int Id);
    }
}
