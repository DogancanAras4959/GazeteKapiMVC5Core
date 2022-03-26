using CORE.ApplicationCommon.DTOS.NewsDto;
using CORE.ApplicationCommon.DTOS.NewsDto.GuestDto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SERVICE.Engine.Interfaces
{
    public interface INewsService
    {
        List<GuestListItemDto> guestList();
        Task<bool> createGuets(GuestDto model);
        bool guestDelete(int id);
        Task<bool> EditIsActive(int id);
        GuestDto getGuest(int id);
        Task<bool> editGuest(GuestDto model);
        List<NewsListItemDto> newsList();
    }
}
