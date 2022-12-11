using CORE.ApplicationCommon.DTOS.BannersDTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SERVICE.Engine.Interfaces
{
    public interface IBannerService
    {
        List<BannerListItemDto> BannerList();
        List<BannerListItemDto> bannerListTakeOneToWeb(int count);
        BannerDto getBanner(int Id);
        Task<bool> createBanner(BannerDto model);
        Task<bool> editBannerDto(BannerDto model);
        Task<bool> IsActiveEnabledBanner(int id);
        bool DeleteBanner(int Id);
    }
}
