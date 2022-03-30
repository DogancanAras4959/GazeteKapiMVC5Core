using CORE.ApplicationCommon.DTOS.NewsDTO;
using CORE.ApplicationCommon.DTOS.NewsDTO.GuestDTO;
using CORE.ApplicationCommon.DTOS.NewsDTO.PublishTypeDTO;
using CORE.ApplicationCommon.DTOS.NewsDTO.TagDTO;
using CORE.ApplicationCommon.DTOS.NewsDTO.TagNewsDTO;
using GazeteKapiMVC5Core.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SERVICE.Engine.Interfaces
{
    public interface INewsService
    {

        #region Yazar
        List<GuestListItemDto> guestList();
        Task<bool> createGuets(GuestDto model);
        bool guestDelete(int id);
        Task<bool> EditIsActive(int id);
        GuestDto getGuest(int id);
        Task<bool> editGuest(GuestDto model);

        #endregion

        #region Haber
        List<NewsListItemDto> newsList();
        List<NewsListItemDto> newsListByUserId(int id);
        List<PublishTypeListItem> publishTypeList();
        List<NewsListItemDto> searchDataInNews(string searchName);
        List<NewsListItemDto> newsListByCategoryId(int? categoryId);
        List<NewsListItemDto> newsListByUserIdInAll(int? userId);
        Task<bool> NewsIfExists(string title);
        Task<int> createNews(NewsDto model);
        Task<int> editNews(NewsDto model);
        NewsDto getNews(int id);
        bool newsDelete(int id);
        Task<bool> SetYourNewsToUp(int id);
        Task<bool> IsOpenNotificationSet(int id);
        Task<bool> IsLockNews(int id);
        Task<bool> IsActiveEnabled(int id);

        #endregion

        #region Etiket

        Task<bool> createTag(TagDto model);
        TagDto getTags(string name);
        bool tagDelete(int id);
        Task InsertTagToProduct(string v, int resultId);
        List<TagNewsListItemDto> tagsListWithNews();
        List<TagNewsListItemDto> tagsListWithNewsById(int etiketId);
        TagBaseDto tagGet(int etiketId);
        List<TagNewsListItemDto> tagsListWithNewsByNewsId(int id);
        List<TagListItemDto> tagList();
        List<TagListItemDto> tagListWithSearch(string searchName);

        #endregion

    }
}
