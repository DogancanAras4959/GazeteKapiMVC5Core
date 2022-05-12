﻿using CORE.ApplicationCommon.DTOS.NewsDTO;
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
        List<NewsListItemDto> newsListWithGuest(int guestId);
        List<NewsListItemDto> FilterCategoryInNewsWithGuest(int? categoryId, int ıd);
        List<NewsListItemDto> FilterUserInNewsWithGuest(int? userId, int ıd);
        List<NewsListItemDto> newsListWithGuestOneToFive(int id);

        #endregion

        #region Haber
        List<NewsListItemDto> newsList();
        List<NewsListItemDto> newsListOrderRow();
        List<NewsListItemDto> newsListWithWeb();
        List<NewsListItemDto> newsListJsonData();
        List<NewsListItemDto> newsListByUserId(int id);
        List<PublishTypeListItem> publishTypeList();
        List<NewsListItemDto> searchDataInNews(string searchName);
        List<NewsListItemDto> newsListByCategoryId(int? categoryId);
        List<NewsListItemDto> newsListByUserIdInAll(int? userId);
        List<NewsListItemDto> newsListLoadByScroll(int pageIndex, int pageSize);
        List<NewsListItemDto> searchDataInNewsWithGuest(string searchstring, int guestId);
        Task<bool> NewsIfExists(string title);
        Task<int> createNews(NewsDto model);
        Task<int> editNews(NewsDto model);
        NewsDto getNews(int id);
        bool newsDelete(int id);
        Task<bool> SetYourNewsToUp(int id);
        Task<bool> IsOpenNotificationSet(int id);
        Task<bool> IsLockNews(int id);
        Task<bool> ChangeSorted(int id, int sira);
        Task<bool> IsActiveEnabled(int id);

        #endregion

        #region Etiket

        Task<bool> createTag(TagDto model);
        TagDto getTags(string name);
        bool tagDelete(int id);
        Task InsertTagToProduct(string v, int resultId);
        List<TagNewsListItemDto> tagsListWithNews();
        List<TagNewsListItemDto> tagsListWithNewsWeb();
        List<TagNewsListItemDto> tagsListWithNewsById(int etiketId);
        List<TagNewsListItemDto> tagsListWithNewsWebSearch(string search);
        TagBaseDto tagGet(int etiketId);
        List<TagNewsListItemDto> tagsListWithNewsByNewsId(int id); 
        List<TagNewsListItemDto> tagsListWithNewsByTagId(int? id);
        List<TagListItemDto> tagList();
        List<NewsListItemDto> newsListWithLastOneToFive();
        List<TagListItemDto> tagListWithSearch(string searchName);
        List<NewsListItemDto> PopularNewsInAdminHome();
        List<NewsListItemDto> PopularNewsInWeb();
        List<NewsListItemDto> PopularNewsInWebInCategory(int categoryId);
        #endregion

    }
}
