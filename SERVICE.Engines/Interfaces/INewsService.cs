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

        #endregion

        #region Haber
        List<NewsListItemDto> newsList();
        List<PublishTypeListItem> publishTypeList();
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
        Task InsertTagToProduct(string v, int resultId);
        List<TagNewsListItemDto> tagsList();

        #endregion

    }
}
