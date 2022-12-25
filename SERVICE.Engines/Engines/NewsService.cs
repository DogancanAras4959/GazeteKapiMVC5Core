using CORE.ApplicationCommon.DTOS.NewsDTO;
using CORE.ApplicationCommon.DTOS.NewsDTO.GuestDTO;
using CORE.ApplicationCommon.DTOS.NewsDTO.MediaDTO;
using CORE.ApplicationCommon.DTOS.NewsDTO.PublishTypeDTO;
using CORE.ApplicationCommon.DTOS.NewsDTO.TagDTO;
using CORE.ApplicationCommon.DTOS.NewsDTO.TagNewsDTO;
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
    public class NewsService : INewsService
    {

        private readonly IUnitOfWork<NewsAppContext> _unitOfWork;
        public NewsService(IUnitOfWork<NewsAppContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Guest
        public async Task<bool> createGuets(GuestDto model)
        {
            Guest newGuest = await _unitOfWork.GetRepository<Guest>().AddAsync(new Guest
            {
                GuestName = model.GuestName,
                GuestImage = model.GuestImage,
                Biography = model.Biography,
                IsActive = true,
                Facebook = model.Facebook,
                Twitter = model.Twitter,
                Instagram = model.Instagram,
                Youtube = model.Youtube,
                Gmail = model.Gmail,
                UpdatedTime = DateTime.Now,
                CreatedTime = DateTime.Now,
                Email = model.Email,
                UserId = model.UserId,

            });

            return newGuest != null && newGuest.Id != 0;
        }
        public async Task<bool> editGuest(GuestDto model)
        {
            Guest getGuest = await _unitOfWork.GetRepository<Guest>().FindAsync(x => x.Id == model.Id);

            Guest guestGet = await _unitOfWork.GetRepository<Guest>().UpdateAsync(new Guest
            {
                Id = getGuest.Id,
                GuestImage = model.GuestImage,
                GuestName = model.GuestName,
                Biography = model.Biography,
                Email = model.Email,
                Facebook = model.Facebook,
                Twitter = model.Twitter,
                Instagram = model.Instagram,
                Gmail = model.Gmail,
                Youtube = model.Youtube,
                IsActive = getGuest.IsActive,
                UpdatedTime = DateTime.Now,
                CreatedTime = getGuest.CreatedTime,
                users = getGuest.users,
                UserId = model.UserId,
            });

            return guestGet != null;
        }
        public GuestDto getGuest(int id)
        {
            Guest getGuest = _unitOfWork.GetRepository<Guest>().FindAsync(x => x.Id == id).Result;

            if (getGuest != null)
            {
                return new GuestDto
                {
                    Id = getGuest.Id,
                    GuestImage = getGuest.GuestImage,
                    GuestName = getGuest.GuestName,
                    Biography = getGuest.Biography,
                    IsActive = getGuest.IsActive,
                    CreatedTime = getGuest.CreatedTime,
                    UpdatedTime = getGuest.UpdatedTime,
                    Facebook = getGuest.Facebook,
                    Twitter = getGuest.Twitter,
                    Instagram = getGuest.Instagram,
                    Gmail = getGuest.Gmail,
                    Youtube = getGuest.Youtube,
                    UserId = getGuest.UserId,
                    Email = getGuest.Email,
                };
            }
            else
            {
                return null;
            }
        }
        public bool guestDelete(int id)
        {
            Task<int> result = _unitOfWork.GetRepository<Guest>().DeleteAsync(new Guest { Id = id });
            return Convert.ToBoolean(result.Result);
        }
        public List<GuestListItemDto> guestList()
        {
            IEnumerable<Guest> guestList = _unitOfWork.GetRepository<Guest>().Filter(x => x.GuestName != "Yazar Yok", x => x.OrderByDescending(y => y.Id), "users", null, null);

            return guestList.Select(x => new GuestListItemDto
            {
                Id = x.Id,
                GuestName = x.GuestName,
                GuestImage = x.GuestImage,
                Biography = x.Biography,
                UpdatedTime = x.UpdatedTime,
                CreatedTime = x.CreatedTime,
                Facebook = x.Facebook,
                Twitter = x.Twitter,
                Youtube = x.Youtube,
                Instagram = x.Instagram,
                Gmail = x.Gmail,
                Email = x.Email,
                UserId = x.UserId,
                user = x.users,
                IsActive = x.IsActive,
            }).ToList();
        }
        public async Task<bool> EditIsActive(int id)
        {
            Guest getUser = _unitOfWork.GetRepository<Guest>().FindAsync(x => x.Id == id).Result;
            if (getUser.IsActive == false)
            {
                getUser.IsActive = true;
                Guest model = await _unitOfWork.GetRepository<Guest>().UpdateAsync(getUser);
                return getUser != null;
            }
            else
            {
                getUser.IsActive = false;
                Guest model = await _unitOfWork.GetRepository<Guest>().UpdateAsync(getUser);
                return getUser != null;
            }
        }

        #endregion

        #region Tag & Tag-News
        public async Task<bool> createTag(TagDto model)
        {
            Tags newTags = await _unitOfWork.GetRepository<Tags>().AddAsync(new Tags
            {
                TagName = model.TagName
            });

            return newTags != null && newTags.Id != 0;
        }
        public TagDto getTags(string name)
        {
            Tags getTags = _unitOfWork.GetRepository<Tags>().FindAsync(x => x.TagName == name).Result;

            return new TagDto
            {
                Id = getTags.Id,
                TagName = getTags.TagName
            };
        }
        public async Task InsertTagToProduct(string tag, int resultId)
        {
            try
            {
                News getNews = await _unitOfWork.GetRepository<News>().FindAsync(x => x.Id == resultId);
                string[] listTags = tag.Split(',');

                for (int i = 0; i < listTags.Count(); i++)
                {
                    if (await _unitOfWork.GetRepository<Tags>().FindAsync(x => x.TagName == listTags[i].Trim().ToString()) != null)
                    {
                        //var ise oluşturmayacak
                    }
                    else
                    {
                        Tags tags = await _unitOfWork.GetRepository<Tags>().AddAsync(new Tags
                        {
                            TagName = listTags[i].Trim().ToString()
                        });
                    }
                }

                foreach (string item in listTags) //Çalışmıyor
                {
                    string etiketAdi = item.Trim();
                    Tags etiketiGetir = await _unitOfWork.GetRepository<Tags>().FindAsync(x => x.TagName == etiketAdi);

                    if (await _unitOfWork.GetRepository<TagNews>().FindAsync(x => x.NewsId == getNews.Id && x.TagId == etiketiGetir.Id) != null)
                    {
                        //var ise eklemeyecek
                    }
                    else
                    {
                        TagNews tagNews = await _unitOfWork.GetRepository<TagNews>().AddAsync(new TagNews
                        {
                            NewsId = getNews.Id,
                            TagId = etiketiGetir.Id,
                        });
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
        public List<TagNewsListItemDto> tagsListWithNews()
        {
            IEnumerable<TagNews> newsList = _unitOfWork.GetRepository<TagNews>().Filter(null, x => x.OrderByDescending(y => y.Id), "tag,news", 1, 50);

            if (newsList != null)
            {
                return newsList.Select(x => new TagNewsListItemDto
                {

                    Id = x.Id,
                    NewsId = x.NewsId,
                    TagId = x.TagId,
                    news = x.news,
                    tag = x.tag,

                }).ToList();
            }
            else
            {
                return null;
            }
        }
        public List<TagNewsListItemDto> tagsListWithNewsWeb()
        {
            IEnumerable<TagNews> newsList = _unitOfWork.GetRepository<TagNews>().Filter(null, x => x.OrderByDescending(y => y.Id), "tag,news", null, null);

            if (newsList != null)
            {
                return newsList.Select(x => new TagNewsListItemDto
                {

                    Id = x.Id,
                    NewsId = x.NewsId,
                    TagId = x.TagId,
                    news = x.news,
                    tag = x.tag,

                }).ToList();
            }
            else
            {
                return null;
            }
        }
        public List<TagNewsListItemDto> tagsListWithNewsById(int etiketId)
        {
            IEnumerable<TagNews> newsList = _unitOfWork.GetRepository<TagNews>().Filter(x => x.TagId == etiketId, x => x.OrderByDescending(y => y.Id), "tag,news", 1, 50);

            if (newsList != null)
            {
                return newsList.Select(x => new TagNewsListItemDto
                {

                    Id = x.Id,
                    NewsId = x.NewsId,
                    TagId = x.TagId,
                    news = x.news,
                    tag = x.tag,

                }).ToList();
            }
            else
            {
                return null;
            }
        }
        public TagBaseDto tagGet(int etiketId)
        {
            Tags getTags = _unitOfWork.GetRepository<Tags>().FindAsync(x => x.Id == etiketId).Result;

            return new TagDto
            {
                Id = getTags.Id,
                TagName = getTags.TagName
            };
        }
        public List<TagNewsListItemDto> tagsListWithNewsByNewsId(int id)
        {
            IEnumerable<TagNews> newsList = _unitOfWork.GetRepository<TagNews>().Filter(x => x.NewsId == id, x => x.OrderByDescending(y => y.Id), "tag,news", 1, 50);

            if (newsList != null)
            {
                return newsList.Select(x => new TagNewsListItemDto
                {

                    Id = x.Id,
                    NewsId = x.NewsId,
                    TagId = x.TagId,
                    news = x.news,
                    tag = x.tag,

                }).ToList();
            }
            else
            {
                return null;
            }
        }
        public List<TagListItemDto> tagList()
        {
            IEnumerable<Tags> newsList = _unitOfWork.GetRepository<Tags>().Filter(null, x => x.OrderByDescending(y => y.Id), "", null, null);

            if (newsList != null)
            {

                return newsList.Select(x => new TagListItemDto
                {
                    Id = x.Id,
                    TagName = x.TagName,

                }).ToList();
            }
            else
            {
                return null;
            }
        }
        public bool tagDelete(int id)
        {
            Task<int> result = _unitOfWork.GetRepository<Tags>().DeleteAsync(new Tags { Id = id });
            return Convert.ToBoolean(result.Result);
        }
        public List<TagNewsListItemDto> tagsListWithNewsWebSearch(string search)
        {
            IEnumerable<TagNews> getTags = _unitOfWork.GetRepository<TagNews>().Filter(null, x => x.OrderByDescending(y => y.Id), "tag,news", null, null);

            if (!String.IsNullOrEmpty(search))
            {
                getTags = getTags.Where(x => x.tag.TagName!.Contains(search));
            }

            return getTags.Select(x => new TagNewsListItemDto
            {
                Id = x.Id,
                tag = x.tag,
                news = x.news

            }).ToList();
        }
        public List<TagNewsListItemDto> tagsListWithNewsByTagId(int? id)
        {
            IEnumerable<TagNews> newsList = _unitOfWork.GetRepository<TagNews>().Filter(x => x.TagId == id, x => x.OrderByDescending(y => y.Id), "tag,news", null, null);

            if (newsList != null)
            {
                return newsList.Select(x => new TagNewsListItemDto
                {

                    Id = x.Id,
                    NewsId = x.NewsId,
                    TagId = x.TagId,
                    news = x.news,
                    tag = x.tag,

                }).ToList();
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region News
        public List<NewsListItemDto> newsList()
        {
            IEnumerable<News> newsList = _unitOfWork.GetRepository<News>().Filter(null, x => x.OrderByDescending(y => y.Id), "guest,users,categories,publishtype", null, null);

            if (newsList != null)
            {
                return newsList.Select(x => new NewsListItemDto
                {

                    Id = x.Id,
                    Title = x.Title,
                    Spot = x.Spot,
                    RowNo = x.RowNo,
                    MetaTitle = x.MetaTitle,
                    ColNo = x.ColNo,
                    doublePlace = x.doublePlace,
                    Image = x.Image,
                    isArchive = x.isArchive,
                    fourthPlace = x.fourthPlace,
                    NewsContent = x.NewsContent,
                    IsSlide = x.IsSlide,
                    IsOpenNotifications = x.IsOpenNotifications,
                    IsLock = x.IsLock,
                    IsActive = x.IsActive,
                    VideoSlug = x.VideoSlug,
                    VideoUploaded = x.VideoUploaded,
                    Views = x.Views,
                    IsTitle = x.IsTitle,
                    UpdatedTime = x.UpdatedTime,
                    CreatedTime = x.CreatedTime,
                    CategoryId = x.CategoryId,
                    UserId = x.UserId,
                    GuestId = x.GuestId,
                    ParentNewsId = x.ParentNewsId,
                    PublishTypeId = x.PublishTypeId,
                    PublishedTime = x.PublishedTime,
                    IsCommentActive = x.IsCommentActive,
                    Sorted = x.Sorted,
                    guest = x.guest,
                    publishtype = x.publishtype,
                    users = x.users,
                    categories = x.categories,
                    Sound = x.Sound,

                }).ToList();
            }
            else
            {
                return null;
            }
        }
        public List<NewsListItemDto> newsListWebInfinityNews()
        {
            IEnumerable<News> newsList = _unitOfWork.GetRepository<News>().Filter(null, x => x.OrderByDescending(y => y.Id), null, null, null);

            if (newsList != null)
            {
                return newsList.Select(x => new NewsListItemDto
                {

                    Id = x.Id,
                    Title = x.Title,
                    Spot = x.Spot,
                    RowNo = x.RowNo,
                    MetaTitle = x.MetaTitle,
                    ColNo = x.ColNo,
                    doublePlace = x.doublePlace,
                    Image = x.Image,
                    isArchive = x.isArchive,
                    fourthPlace = x.fourthPlace,
                    NewsContent = x.NewsContent,
                    IsSlide = x.IsSlide,
                    IsOpenNotifications = x.IsOpenNotifications,
                    IsLock = x.IsLock,
                    IsActive = x.IsActive,
                    VideoSlug = x.VideoSlug,
                    VideoUploaded = x.VideoUploaded,
                    Views = x.Views,
                    IsTitle = x.IsTitle,
                    UpdatedTime = x.UpdatedTime,
                    CreatedTime = x.CreatedTime,
                    CategoryId = x.CategoryId,
                    UserId = x.UserId,
                    GuestId = x.GuestId,
                    ParentNewsId = x.ParentNewsId,
                    PublishTypeId = x.PublishTypeId,
                    PublishedTime = x.PublishedTime,
                    IsCommentActive = x.IsCommentActive,
                    Sorted = x.Sorted,
                    Sound = x.Sound,

                }).ToList();
            }
            else
            {
                return null;
            }
        }
        public List<NewsListItemDto> newsListByDatetimeBigNow()
        {
            IEnumerable<News> newsList = _unitOfWork.GetRepository<News>().Filter(x=> x.IsActive == false && x.PublishedTime > DateTime.Now, x => x.OrderBy(y => y.Id), null, null, null);

            if (newsList != null)
            {
                return newsList.Select(x => new NewsListItemDto
                {

                    Id = x.Id,
                    Title = x.Title,
                    Spot = x.Spot,
                    MetaTitle = x.MetaTitle,
                    RowNo = x.RowNo,
                    ColNo = x.ColNo,
                    doublePlace = x.doublePlace,
                    Image = x.Image,
                    isArchive = x.isArchive,
                    fourthPlace = x.fourthPlace,
                    NewsContent = x.NewsContent,
                    IsSlide = x.IsSlide,
                    IsOpenNotifications = x.IsOpenNotifications,
                    IsLock = x.IsLock,
                    IsActive = x.IsActive,
                    VideoSlug = x.VideoSlug,
                    VideoUploaded = x.VideoUploaded,
                    Views = x.Views,
                    IsTitle = x.IsTitle,
                    UpdatedTime = x.UpdatedTime,
                    CreatedTime = x.CreatedTime,
                    CategoryId = x.CategoryId,
                    UserId = x.UserId,
                    GuestId = x.GuestId,
                    ParentNewsId = x.ParentNewsId,
                    PublishTypeId = x.PublishTypeId,
                    PublishedTime = x.PublishedTime,
                    IsCommentActive = x.IsCommentActive,
                    Sorted = x.Sorted,
                    Sound = x.Sound,

                }).ToList();
            }
            else
            {
                return null;
            }
        }
        public List<PublishTypeListItem> publishTypeList()
        {
            IEnumerable<PublishType> listTypes = _unitOfWork.GetRepository<PublishType>().Filter(null, x => x.OrderByDescending(y => y.Id), "user", 1, 50);

            return listTypes.Select(x => new PublishTypeListItem
            {
                Id = x.Id,
                TypeName = x.TypeName,
                user = x.user,
                UserId = x.UserId,

            }).ToList();
        }
        public async Task<bool> NewsIfExists(string title) =>
            await _unitOfWork.GetRepository<News>().FindAsync(x => x.Title == title) != null;
        public async Task<int> createNews(NewsDto model)
        {
            try
            {
                if (model.GuestId == 0 || model.GuestId == 999)
                {
                    var getGuest = _unitOfWork.GetRepository<Guest>().FindAsync(x => x.GuestName == "Yazar Yok").Result;

                    model.GuestId = getGuest.Id;
                }

                News createNews = await _unitOfWork.GetRepository<News>().AddAsync(new News
                {
                    Title = model.Title,
                    Spot = model.Spot,
                    //IsSlide = model.IsSlide,
                    IsActive = true,
                    RowNo = 0,
                    ColNo = 0,
                    VideoSlug = model.VideoSlug,
                    IsLock = false,
                    IsCommentActive = model.IsCommentActive,
                    IsOpenNotifications = model.IsOpenNotifications,
                    UpdatedTime = DateTime.Now,
                    MetaTitle = model.MetaTitle,
                    CreatedTime = DateTime.Now,
                    IsTitle = model.IsTitle,
                    
                    Views = 0,
                    VideoUploaded = model.VideoUploaded,
                    CategoryId = model.CategoryId,
                    UserId = model.UserId,
                    GuestId = model.GuestId,
                    PublishTypeId = model.PublishTypeId,
                    NewsContent = model.NewsContent,
                    Image = model.Image,
                    Sorted = model.Sorted,
                });

                return createNews.Id;
            }
            catch (Exception)
            {
                throw;
            }

        }
        public NewsDto getNews(int id)
        {

            News getNews = _unitOfWork.GetRepository<News>().Filter(x => x.Id == id, null, "guest", null, null).SingleOrDefault();

            if (getNews != null)
            {
                return new NewsDto
                {
                    Id = getNews.Id,
                    Title = getNews.Title,
                    Spot = getNews.Spot,
                    doublePlace = getNews.doublePlace,
                    RowNo = getNews.RowNo,
                    ColNo = getNews.ColNo,
                    guest = getNews.guest,
                    IsSlide = getNews.IsSlide,
                    IsActive = getNews.IsActive,
                    VideoSlug = getNews.VideoSlug,
                    VideoUploaded = getNews.VideoUploaded,
                    IsLock = getNews.IsLock,
                    IsCommentActive = getNews.IsCommentActive,
                    IsOpenNotifications = getNews.IsOpenNotifications,
                    UpdatedTime = getNews.UpdatedTime,
                    CreatedTime = getNews.CreatedTime,
                    PublishedTime = getNews.PublishedTime,
                    Views = getNews.Views,
                    CategoryId = getNews.CategoryId,
                    MetaTitle = getNews.MetaTitle,
                    UserId = getNews.UserId,
                    GuestId = getNews.GuestId,
                    PublishTypeId = getNews.PublishTypeId,
                    NewsContent = getNews.NewsContent,
                    Image = getNews.Image,
                    Sorted = getNews.Sorted,
                    IsTitle = getNews.IsTitle,
                    Sound = getNews.Sound,
                };
            }
            else
            {
                return null;
            }
        }
        public async Task<int> editNews(NewsDto model)
        {
            try
            {
                News getNews = await _unitOfWork.GetRepository<News>().FindAsync(x => x.Id == model.Id);

                if (model.GuestId == 0 || model.GuestId == 999)
                {
                    var getGuest = _unitOfWork.GetRepository<Guest>().FindAsync(x => x.GuestName == "Yazar Yok").Result;

                    model.GuestId = getGuest.Id;
                }

                if (model.Image == null)
                {
                    model.Image = getNews.Image;
                }

                if (model.CreatedTime == null)
                {
                    model.CreatedTime = getNews.CreatedTime;
                }

                if(model.PublishedTime == null)
                {
                    model.PublishedTime = getNews.PublishedTime;
                }

                News newsGet = await _unitOfWork.GetRepository<News>().UpdateAsync(new News
                {
                    Id = model.Id,
                    Image = model.Image,
                    Title = model.Title,
                    VideoSlug = model.VideoSlug,
                    Spot = model.Spot,
                    VideoUploaded = model.VideoUploaded,
                    NewsContent = model.NewsContent,
                    RowNo = getNews.RowNo,
                    Sorted = model.Sorted,
                    ColNo = model.ColNo,
                    IsCommentActive = model.IsCommentActive,
                    IsOpenNotifications = model.IsOpenNotifications,
                    IsSlide = getNews.IsSlide,
                    Views = model.Views,
                    IsTitle = model.IsTitle,
                    doublePlace = model.doublePlace,
                    fourthPlace = model.fourthPlace,
                    isArchive = model.isArchive,
                    MetaTitle = model.MetaTitle,
                    IsActive = model.IsActive,
                    IsLock = getNews.IsLock,
                    UpdatedTime = DateTime.Now,
                    CreatedTime = getNews.CreatedTime,
                    users = model.users,
                    UserId = model.UserId,
                   
                    CategoryId = model.CategoryId,
                    ParentNewsId = model.ParentNewsId,
                    categories = model.categories,
                    guest = model.guest,
                    GuestId = model.GuestId,
                    PublishTypeId = model.PublishTypeId,
                    publishtype = model.publishtype,
                    PublishedTime = model.PublishedTime,
                    Sound = model.Sound,
                });

                return newsGet.Id;

            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public bool newsDelete(int id)
        {
            Task<int> result = _unitOfWork.GetRepository<News>().DeleteAsync(new News { Id = id });
            return Convert.ToBoolean(result.Result);
        }
        public async Task<bool> SetYourNewsToUp(int id)
        {
            News getNews = _unitOfWork.GetRepository<News>().FindAsync(x => x.Id == id).Result;
            if (getNews.IsSlide == false)
            {
                getNews.IsSlide = true;
                News model = await _unitOfWork.GetRepository<News>().UpdateAsync(getNews);
                return getNews != null;
            }
            else
            {
                getNews.IsSlide = false;
                getNews.RowNo = 0;
                News model = await _unitOfWork.GetRepository<News>().UpdateAsync(getNews);
                return getNews != null;
            }
        }
        public async Task<bool> IsOpenNotificationSet(int id)
        {
            News getNews = _unitOfWork.GetRepository<News>().FindAsync(x => x.Id == id).Result;
            if (getNews.IsOpenNotifications == false)
            {
                getNews.IsOpenNotifications = true;
                News model = await _unitOfWork.GetRepository<News>().UpdateAsync(getNews);
                return getNews != null;
            }
            else
            {
                getNews.IsOpenNotifications = false;
                News model = await _unitOfWork.GetRepository<News>().UpdateAsync(getNews);
                return getNews != null;
            }
        }
        public async Task<bool> IsLockNews(int id)
        {
            News getNews = _unitOfWork.GetRepository<News>().FindAsync(x => x.Id == id).Result;
            if (getNews.IsLock == false)
            {
                getNews.IsLock = true;
                News model = await _unitOfWork.GetRepository<News>().UpdateAsync(getNews);
                return getNews != null;
            }
            else
            {
                getNews.IsLock = false;
                News model = await _unitOfWork.GetRepository<News>().UpdateAsync(getNews);
                return getNews != null;
            }
        }
        public async Task<bool> IsActiveEnabled(int id)
        {
            News getNews = _unitOfWork.GetRepository<News>().FindAsync(x => x.Id == id).Result;
            if (getNews.IsActive == false)
            {
                getNews.IsActive = true;
                News model = await _unitOfWork.GetRepository<News>().UpdateAsync(getNews);
                return getNews != null;
            }
            else
            {
                getNews.IsActive = false;
                News model = await _unitOfWork.GetRepository<News>().UpdateAsync(getNews);
                return getNews != null;
            }
        }
        public List<NewsListItemDto> newsListByUserId(int id)
        {
            IEnumerable<News> newsList = _unitOfWork.GetRepository<News>().Filter(x => x.UserId == id, x => x.OrderByDescending(y => y.Id), "users,guest,publishtype,categories", 1, 5);

            if (newsList != null)
            {
                return newsList.Select(x => new NewsListItemDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Spot = x.Spot,
                    VideoSlug = x.VideoSlug,
                    Image = x.Image,
                    doublePlace = x.doublePlace,
                    isArchive = x.isArchive,
                    MetaTitle = x.MetaTitle,
                    NewsContent = x.NewsContent,
                    IsSlide = x.IsSlide,
                    IsOpenNotifications = x.IsOpenNotifications,
                    VideoUploaded = x.VideoUploaded,
                    ColNo = x.ColNo,
                    RowNo = x.RowNo,
                    IsLock = x.IsLock,
                    IsTitle = x.IsTitle,
                    IsActive = x.IsActive,
                    Views = x.Views,
                    UpdatedTime = x.UpdatedTime,
                    CreatedTime = x.CreatedTime,
                    CategoryId = x.CategoryId,
                    UserId = x.UserId,
                    GuestId = x.GuestId,
                    PublishTypeId = x.PublishTypeId,
                    PublishedTime = x.PublishedTime,
                    IsCommentActive = x.IsCommentActive,
                    Sorted = x.Sorted,
                    guest = x.guest,
                    publishtype = x.publishtype,
                    users = x.users,
                    categories = x.categories,
                    Sound = x.Sound,

                }).ToList();
            }
            else
            {
                return null;
            }
        }
        public List<NewsListItemDto> searchDataInNews(string searhNameNews)
        {
            try
            {
                IEnumerable<News> getNews = _unitOfWork.GetRepository<News>().Filter(null, x => x.OrderByDescending(y => y.Id), "users,guest,publishtype,categories", null, null);

                if (!String.IsNullOrEmpty(searhNameNews))
                {
                    getNews = getNews.Where(x => x.Title!.Contains(searhNameNews));
                }

                return getNews.Select(x => new NewsListItemDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Spot = x.Spot,
                    RowNo = x.RowNo,
                    VideoSlug = x.VideoSlug,
                    VideoUploaded = x.VideoUploaded,
                    MetaTitle = x.MetaTitle,
                    ColNo = x.ColNo,
                    Image = x.Image,
                    doublePlace = x.doublePlace,
                    fourthPlace = x.fourthPlace,
                    isArchive = x.isArchive,
                    
                    NewsContent = x.NewsContent,
                    IsSlide = x.IsSlide,
                    IsOpenNotifications = x.IsOpenNotifications,
                    IsLock = x.IsLock,
                    IsActive = x.IsActive,
                    Views = x.Views,
                    UpdatedTime = x.UpdatedTime,
                    CreatedTime = x.CreatedTime,
                    IsTitle = x.IsTitle,
                    CategoryId = x.CategoryId,
                    UserId = x.UserId,
                    GuestId = x.GuestId,
                    PublishTypeId = x.PublishTypeId,
                    PublishedTime = x.PublishedTime,
                    IsCommentActive = x.IsCommentActive,
                    Sorted = x.Sorted,
                    guest = x.guest,
                    publishtype = x.publishtype,
                    users = x.users,
                    categories = x.categories,
                    Sound = x.Sound,

                }).ToList();
            }
            catch (Exception)
            {

                return null;
            }
        }
        public List<NewsListItemDto> newsListByCategoryId(int? categoryId)
        {
            IEnumerable<News> newsList = _unitOfWork.GetRepository<News>().Filter(x => x.CategoryId == categoryId, x => x.OrderByDescending(y => y.Id), "users,guest,publishtype,categories", null, null);

            if (newsList != null)
            {
                return newsList.Select(x => new NewsListItemDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Spot = x.Spot,
                    RowNo = x.RowNo,
                    ColNo = x.ColNo,
                    doublePlace = x.doublePlace,
                    fourthPlace = x.fourthPlace,
                    isArchive = x.isArchive,
                    MetaTitle = x.MetaTitle,
                    VideoSlug = x.VideoSlug,
                    VideoUploaded = x.VideoUploaded,
                    IsTitle = x.IsTitle,
                    Image = x.Image,
                    NewsContent = x.NewsContent,
                    IsSlide = x.IsSlide,
                    IsOpenNotifications = x.IsOpenNotifications,
                    IsLock = x.IsLock,
                    IsActive = x.IsActive,
                    Views = x.Views,
                    UpdatedTime = x.UpdatedTime,
                    CreatedTime = x.CreatedTime,
                    CategoryId = x.CategoryId,
                    UserId = x.UserId,
                    GuestId = x.GuestId,
                    PublishTypeId = x.PublishTypeId,
                    PublishedTime = x.PublishedTime,
                    IsCommentActive = x.IsCommentActive,
                    Sorted = x.Sorted,
                    guest = x.guest,
                    publishtype = x.publishtype,
                    users = x.users,
                    categories = x.categories,
                    Sound = x.Sound,

                }).ToList();
            }
            else
            {
                return null;
            }
        }
        public List<NewsListItemDto> newsListByUserIdInAll(int? userId)
        {
            IEnumerable<News> newsList = _unitOfWork.GetRepository<News>().Filter(x => x.UserId == userId, x => x.OrderByDescending(y => y.Id), "users,guest,publishtype,categories", null, null);

            if (newsList != null)
            {
                return newsList.Select(x => new NewsListItemDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    doublePlace = x.doublePlace,
                    fourthPlace = x.fourthPlace,
                    isArchive = x.isArchive,
                    MetaTitle = x.MetaTitle,
                    Spot = x.Spot,
                    Image = x.Image,
                    IsTitle = x.IsTitle,
                    RowNo = x.RowNo,
                    ColNo = x.ColNo,
                    VideoSlug = x.VideoSlug,
                    VideoUploaded = x.VideoUploaded,
                    NewsContent = x.NewsContent,
                    IsSlide = x.IsSlide,
                    IsOpenNotifications = x.IsOpenNotifications,
                    IsLock = x.IsLock,
                    IsActive = x.IsActive,
                    Views = x.Views,
                    UpdatedTime = x.UpdatedTime,
                    CreatedTime = x.CreatedTime,
                    CategoryId = x.CategoryId,
                    UserId = x.UserId,
                    GuestId = x.GuestId,
                    PublishTypeId = x.PublishTypeId,
                    PublishedTime = x.PublishedTime,
                    IsCommentActive = x.IsCommentActive,
                    Sorted = x.Sorted,
                    guest = x.guest,
                    publishtype = x.publishtype,
                    users = x.users,
                    categories = x.categories,
                    Sound = x.Sound,

                }).ToList();
            }
            else
            {
                return null;
            }
        }
        public List<TagListItemDto> tagListWithSearch(string searchName)
        {
            IEnumerable<Tags> getTags = _unitOfWork.GetRepository<Tags>().Filter(null, x => x.OrderByDescending(y => y.Id), "", null, null);

            if (!String.IsNullOrEmpty(searchName))
            {
                getTags = getTags.Where(x => x.TagName!.Contains(searchName));
            }

            return getTags.Select(x => new TagListItemDto
            {
                Id = x.Id,
                TagName = x.TagName,

            }).ToList();
        }
        public List<NewsListItemDto> searchDataInNewsWithGuest(string searchstring, int guestId)
        {
            try
            {
                IEnumerable<News> getNews = _unitOfWork.GetRepository<News>().Filter(x => x.GuestId == guestId, x => x.OrderByDescending(y => y.Id), "users,guest,publishtype,categories", null, null);

                if (!String.IsNullOrEmpty(searchstring))
                {
                    getNews = getNews.Where(x => x.Title!.Contains(searchstring));
                }

                return getNews.Select(x => new NewsListItemDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Spot = x.Spot,
                    RowNo = x.RowNo,
                    doublePlace = x.doublePlace,
                    fourthPlace = x.fourthPlace,
                    isArchive = x.isArchive,
                    MetaTitle = x.MetaTitle,
                    ColNo = x.ColNo,
                    Image = x.Image,
                    VideoSlug = x.VideoSlug,
                    VideoUploaded = x.VideoUploaded,
                    NewsContent = x.NewsContent,
                    IsSlide = x.IsSlide,
                    IsTitle = x.IsTitle,
                    IsOpenNotifications = x.IsOpenNotifications,
                    IsLock = x.IsLock,
                    IsActive = x.IsActive,
                    Views = x.Views,
                    UpdatedTime = x.UpdatedTime,
                    CreatedTime = x.CreatedTime,
                    CategoryId = x.CategoryId,
                    UserId = x.UserId,
                    GuestId = x.GuestId,
                    PublishTypeId = x.PublishTypeId,
                    PublishedTime = x.PublishedTime,
                    IsCommentActive = x.IsCommentActive,
                    Sorted = x.Sorted,
                    guest = x.guest,
                    publishtype = x.publishtype,
                    users = x.users,
                    categories = x.categories,
                    Sound = x.Sound,

                }).ToList();
            }
            catch (Exception)
            {

                return null;
            }
        }
        public List<NewsListItemDto> newsListWithGuest(int guestId)
        {
            IEnumerable<News> newsList = _unitOfWork.GetRepository<News>().Filter(x => x.GuestId == guestId, x => x.OrderByDescending(y => y.Id), "users,guest,publishtype,categories", null, null);

            if (newsList != null)
            {
                return newsList.Select(x => new NewsListItemDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Spot = x.Spot,
                    Image = x.Image,
                    doublePlace = x.doublePlace,
                    fourthPlace = x.fourthPlace,
                    isArchive = x.isArchive,
                    MetaTitle = x.MetaTitle,
                    RowNo = x.RowNo,
                    VideoSlug = x.VideoSlug,
                    VideoUploaded = x.VideoUploaded,
                    ColNo = x.ColNo,
                    NewsContent = x.NewsContent,
                    IsSlide = x.IsSlide,
                    IsOpenNotifications = x.IsOpenNotifications,
                    IsLock = x.IsLock,
                    IsActive = x.IsActive,
                    Views = x.Views,
                    IsTitle = x.IsTitle,
                    UpdatedTime = x.UpdatedTime,
                    CreatedTime = x.CreatedTime,
                    CategoryId = x.CategoryId,
                    UserId = x.UserId,
                    GuestId = x.GuestId,
                    PublishTypeId = x.PublishTypeId,
                    PublishedTime = x.PublishedTime,
                    IsCommentActive = x.IsCommentActive,
                    Sorted = x.Sorted,
                    guest = x.guest,
                    publishtype = x.publishtype,
                    users = x.users,
                    categories = x.categories,
                    Sound = x.Sound,

                }).ToList();
            }
            else
            {
                return null;
            }
        }
        public List<NewsListItemDto> FilterCategoryInNewsWithGuest(int? categoryId, int ıd)
        {
            try
            {
                IEnumerable<News> getNews = _unitOfWork.GetRepository<News>().Filter(x => x.GuestId == ıd, x => x.OrderByDescending(y => y.Id), "users,guest,publishtype,categories", null, null);

                getNews = getNews.Where(x => x.CategoryId == categoryId);

                return getNews.Select(x => new NewsListItemDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Spot = x.Spot,
                    doublePlace = x.doublePlace,
                    fourthPlace = x.fourthPlace,
                    isArchive = x.isArchive,
                    MetaTitle = x.MetaTitle,
                    Image = x.Image,
                    RowNo = x.RowNo,
                    ColNo = x.ColNo,
                    VideoSlug = x.VideoSlug,
                    VideoUploaded = x.VideoUploaded,
                    NewsContent = x.NewsContent,
                    IsSlide = x.IsSlide,
                    IsOpenNotifications = x.IsOpenNotifications,
                    IsLock = x.IsLock,
                    IsActive = x.IsActive,
                    Views = x.Views,
                    UpdatedTime = x.UpdatedTime,
                    CreatedTime = x.CreatedTime,
                    CategoryId = x.CategoryId,
                    UserId = x.UserId,
                    GuestId = x.GuestId,
                    PublishTypeId = x.PublishTypeId,
                    IsTitle = x.IsTitle,
                    PublishedTime = x.PublishedTime,
                    IsCommentActive = x.IsCommentActive,
                    Sorted = x.Sorted,
                    guest = x.guest,
                    publishtype = x.publishtype,
                    users = x.users,
                    categories = x.categories,
                    Sound = x.Sound,

                }).ToList();
            }
            catch (Exception)
            {

                return null;
            }
        }
        public List<NewsListItemDto> FilterUserInNewsWithGuest(int? userId, int ıd)
        {
            try
            {
                IEnumerable<News> getNews = _unitOfWork.GetRepository<News>().Filter(x => x.GuestId == ıd, x => x.OrderByDescending(y => y.Id), "users,guest,publishtype,categories", null, null);

                getNews = getNews.Where(x => x.UserId == userId);

                return getNews.Select(x => new NewsListItemDto
                {
                    Id = x.Id,
                    IsTitle = x.IsTitle,
                    Title = x.Title,
                    doublePlace = x.doublePlace,
                    fourthPlace = x.fourthPlace,
                    isArchive = x.isArchive,
                    MetaTitle = x.MetaTitle,
                    Spot = x.Spot,
                    Image = x.Image,
                    VideoSlug = x.VideoSlug,
                    VideoUploaded = x.VideoUploaded,
                    RowNo = x.RowNo,
                    ColNo = x.ColNo,
                    NewsContent = x.NewsContent,
                    IsSlide = x.IsSlide,
                    IsOpenNotifications = x.IsOpenNotifications,
                    IsLock = x.IsLock,
                    IsActive = x.IsActive,
                    Views = x.Views,
                    UpdatedTime = x.UpdatedTime,
                    CreatedTime = x.CreatedTime,
                    CategoryId = x.CategoryId,
                    UserId = x.UserId,
                    GuestId = x.GuestId,
                    PublishTypeId = x.PublishTypeId,
                    PublishedTime = x.PublishedTime,
                    IsCommentActive = x.IsCommentActive,
                    Sorted = x.Sorted,
                    guest = x.guest,
                    publishtype = x.publishtype,
                    users = x.users,
                    categories = x.categories,
                    Sound = x.Sound,

                }).ToList();
            }
            catch (Exception)
            {

                return null;
            }
        }
        public List<NewsListItemDto> newsListWithGuestOneToFive(int id)
        {
            IEnumerable<News> newsList = _unitOfWork.GetRepository<News>().Filter(x => x.GuestId == id, x => x.OrderByDescending(y => y.Id), "users,guest,publishtype,categories", 1, 5);

            if (newsList != null)
            {
                return newsList.Select(x => new NewsListItemDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Spot = x.Spot,
                    IsTitle = x.IsTitle,
                    RowNo = x.RowNo,
                    ColNo = x.ColNo,
                    doublePlace = x.doublePlace,
                    fourthPlace = x.fourthPlace,
                    isArchive = x.isArchive,
                    MetaTitle = x.MetaTitle,
                    VideoSlug = x.VideoSlug,
                    VideoUploaded = x.VideoUploaded,
                    Image = x.Image,
                    NewsContent = x.NewsContent,
                    IsSlide = x.IsSlide,
                    IsOpenNotifications = x.IsOpenNotifications,
                    IsLock = x.IsLock,
                    IsActive = x.IsActive,
                    Views = x.Views,
                    UpdatedTime = x.UpdatedTime,
                    CreatedTime = x.CreatedTime,
                    CategoryId = x.CategoryId,
                    UserId = x.UserId,
                    GuestId = x.GuestId,
                    PublishTypeId = x.PublishTypeId,
                    PublishedTime = x.PublishedTime,
                    IsCommentActive = x.IsCommentActive,
                    Sorted = x.Sorted,
                    guest = x.guest,
                    publishtype = x.publishtype,
                    users = x.users,
                    categories = x.categories,
                    Sound = x.Sound,

                }).ToList();
            }
            else
            {
                return null;
            }
        }
        public List<NewsListItemDto> newsListWithLastOneToFive()
        {
            IEnumerable<News> newsList = _unitOfWork.GetRepository<News>().Filter(null, x => x.OrderByDescending(y => y.Id), "users,guest,publishtype,categories", 1, 5);

            if (newsList != null)
            {
                return newsList.Select(x => new NewsListItemDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Spot = x.Spot,
                    VideoSlug = x.VideoSlug,
                    VideoUploaded = x.VideoUploaded,
                    IsTitle = x.IsTitle,
                    doublePlace = x.doublePlace,
                    fourthPlace = x.fourthPlace,
                    isArchive = x.isArchive,
                    MetaTitle = x.MetaTitle,
                    RowNo = x.RowNo,
                    ColNo = x.ColNo,
                    Image = x.Image,
                    NewsContent = x.NewsContent,
                    IsSlide = x.IsSlide,
                    IsOpenNotifications = x.IsOpenNotifications,
                    IsLock = x.IsLock,
                    IsActive = x.IsActive,
                    Views = x.Views,
                    UpdatedTime = x.UpdatedTime,
                    CreatedTime = x.CreatedTime,
                    CategoryId = x.CategoryId,
                    UserId = x.UserId,
                    GuestId = x.GuestId,
                    PublishTypeId = x.PublishTypeId,
                    PublishedTime = x.PublishedTime,
                    IsCommentActive = x.IsCommentActive,
                    Sorted = x.Sorted,
                    guest = x.guest,
                    publishtype = x.publishtype,
                    users = x.users,
                    categories = x.categories,
                    Sound = x.Sound,

                }).ToList();
            }
            else
            {
                return null;
            }
        }
        public List<NewsListItemDto> PopularNewsInAdminHome()
        {
            IEnumerable<News> newsList = _unitOfWork.GetRepository<News>().Filter(null, x => x.OrderByDescending(y => y.Views), "users,guest,publishtype,categories", 1, 1);

            if (newsList != null)
            {
                return newsList.Select(x => new NewsListItemDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Spot = x.Spot,
                    Image = x.Image,
                    VideoSlug = x.VideoSlug,
                    VideoUploaded = x.VideoUploaded,
                    RowNo = x.RowNo,
                    ColNo = x.ColNo,
                    doublePlace = x.doublePlace,
                    fourthPlace = x.fourthPlace,
                    isArchive = x.isArchive,
                    MetaTitle = x.MetaTitle,
                    NewsContent = x.NewsContent,
                    IsSlide = x.IsSlide,
                    IsTitle = x.IsTitle,
                    IsOpenNotifications = x.IsOpenNotifications,
                    IsLock = x.IsLock,
                    IsActive = x.IsActive,
                    Views = x.Views,
                    UpdatedTime = x.UpdatedTime,
                    CreatedTime = x.CreatedTime,
                    CategoryId = x.CategoryId,
                    UserId = x.UserId,
                    GuestId = x.GuestId,
                    PublishTypeId = x.PublishTypeId,
                    PublishedTime = x.PublishedTime,
                    IsCommentActive = x.IsCommentActive,
                    Sorted = x.Sorted,
                    guest = x.guest,
                    publishtype = x.publishtype,
                    users = x.users,
                    categories = x.categories,
                    Sound = x.Sound,

                }).ToList();
            }
            else
            {
                return null;
            }
        }
        public List<NewsListItemDto> PopularNewsInWeb()
        {
            IEnumerable<News> newsList = _unitOfWork.GetRepository<News>().Filter(null, x => x.OrderByDescending(y => y.Views), "users,guest,publishtype,categories", 1, 3);

            if (newsList != null)
            {
                return newsList.Select(x => new NewsListItemDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    VideoSlug = x.VideoSlug,
                    VideoUploaded = x.VideoUploaded,
                    Spot = x.Spot,
                    IsTitle = x.IsTitle,
                    RowNo = x.RowNo,
                    ColNo = x.ColNo,
                    doublePlace = x.doublePlace,
                    fourthPlace = x.fourthPlace,
                    isArchive = x.isArchive,
                    MetaTitle = x.MetaTitle,
                    Image = x.Image,
                    NewsContent = x.NewsContent,
                    IsSlide = x.IsSlide,
                    IsOpenNotifications = x.IsOpenNotifications,
                    IsLock = x.IsLock,
                    IsActive = x.IsActive,
                    Views = x.Views,
                    UpdatedTime = x.UpdatedTime,
                    CreatedTime = x.CreatedTime,
                    CategoryId = x.CategoryId,
                    UserId = x.UserId,
                    GuestId = x.GuestId,
                    PublishTypeId = x.PublishTypeId,
                    PublishedTime = x.PublishedTime,
                    IsCommentActive = x.IsCommentActive,
                    Sorted = x.Sorted,
                    guest = x.guest,
                    publishtype = x.publishtype,
                    users = x.users,
                    categories = x.categories,
                    Sound = x.Sound,

                }).ToList();
            }
            else
            {
                return null;
            }
        }
        public List<NewsListItemDto> PopularNewsInWebInCategory(int categoryId)
        {
            IEnumerable<News> newsList = _unitOfWork.GetRepository<News>().Filter(x => x.CategoryId == categoryId, x => x.OrderByDescending(y => y.Views), "users,guest,publishtype,categories", 1, 1);

            if (newsList != null)
            {
                return newsList.Select(x => new NewsListItemDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    RowNo = x.RowNo,
                    ColNo = x.ColNo,
                    Spot = x.Spot,
                    Image = x.Image,
                    doublePlace = x.doublePlace,
                    fourthPlace = x.fourthPlace,
                    isArchive = x.isArchive,
                    MetaTitle = x.MetaTitle,
                    NewsContent = x.NewsContent,
                    IsSlide = x.IsSlide,
                    VideoSlug = x.VideoSlug,
                    VideoUploaded = x.VideoUploaded,
                    IsOpenNotifications = x.IsOpenNotifications,
                    IsLock = x.IsLock,
                    IsTitle = x.IsTitle,
                    IsActive = x.IsActive,
                    Views = x.Views,
                    UpdatedTime = x.UpdatedTime,
                    CreatedTime = x.CreatedTime,
                    CategoryId = x.CategoryId,
                    UserId = x.UserId,
                    GuestId = x.GuestId,
                    PublishTypeId = x.PublishTypeId,
                    PublishedTime = x.PublishedTime,
                    IsCommentActive = x.IsCommentActive,
                    Sorted = x.Sorted,
                    guest = x.guest,
                    publishtype = x.publishtype,
                    users = x.users,
                    categories = x.categories,
                    Sound = x.Sound,

                }).ToList();
            }
            else
            {
                return null;
            }
        }
        public List<NewsListItemDto> newsListLoadByScroll(int pageIndex, int pageSize)
        {
            IEnumerable<News> newsList = _unitOfWork.GetRepository<News>().Filter(null, x => x.OrderBy(y => y.Id), "users,guest,publishtype,categories", pageIndex, pageSize);

            if (newsList != null)
            {
                return newsList.Select(x => new NewsListItemDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Spot = x.Spot,
                    Image = x.Image,
                    NewsContent = x.NewsContent,
                    IsSlide = x.IsSlide,
                    RowNo = x.RowNo,
                    VideoSlug = x.VideoSlug,
                    VideoUploaded = x.VideoUploaded,
                    ColNo = x.ColNo,
                    IsOpenNotifications = x.IsOpenNotifications,
                    IsLock = x.IsLock,
                    IsActive = x.IsActive,
                    doublePlace = x.doublePlace,
                    fourthPlace = x.fourthPlace,
                    isArchive = x.isArchive,
                    MetaTitle = x.MetaTitle,
                    Views = x.Views,
                    IsTitle = x.IsTitle,
                    UpdatedTime = x.UpdatedTime,
                    CreatedTime = x.CreatedTime,
                    CategoryId = x.CategoryId,
                    UserId = x.UserId,
                    GuestId = x.GuestId,
                    PublishTypeId = x.PublishTypeId,
                    PublishedTime = x.PublishedTime,
                    IsCommentActive = x.IsCommentActive,
                    Sorted = x.Sorted,
                    guest = x.guest,
                    publishtype = x.publishtype,
                    users = x.users,
                    categories = x.categories,
                    Sound = x.Sound,

                }).ToList();
            }
            else
            {
                return null;
            }
        }
        public List<NewsListItemDto> newsListWithWeb()
        {
            IEnumerable<News> newsList = _unitOfWork.GetRepository<News>().Filter(null, null, "guest,users,categories,publishtype", null, null);

            if (newsList != null)
            {
                return newsList.Select(x => new NewsListItemDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Spot = x.Spot,
                    Image = x.Image,
                    IsTitle = x.IsTitle,
                    VideoSlug = x.VideoSlug,
                    RowNo = x.RowNo,
                    doublePlace = x.doublePlace,
                    fourthPlace = x.fourthPlace,
                    isArchive = x.isArchive,
                    MetaTitle = x.MetaTitle,
                    ColNo = x.ColNo,
                    NewsContent = x.NewsContent,
                    IsSlide = x.IsSlide,
                    IsOpenNotifications = x.IsOpenNotifications,
                    IsLock = x.IsLock,
                    ParentNewsId = x.ParentNewsId,
                    VideoUploaded = x.VideoUploaded,
                    IsActive = x.IsActive,
                    Views = x.Views,
                    UpdatedTime = x.UpdatedTime,
                    CreatedTime = x.CreatedTime,
                    CategoryId = x.CategoryId,
                    UserId = x.UserId,
                    GuestId = x.GuestId,
                    PublishTypeId = x.PublishTypeId,
                    PublishedTime = x.PublishedTime,
                    IsCommentActive = x.IsCommentActive,
                    Sorted = x.Sorted,
                    guest = x.guest,
                    publishtype = x.publishtype,
                    users = x.users,
                    categories = x.categories,
                    Sound = x.Sound,

                }).ToList();
            }
            else
            {
                return null;
            }
        }
        public async Task<bool> ChangeSorted(int id, int sira)
        {
            try
            {
                News getNews = _unitOfWork.GetRepository<News>().FindAsync(x => x.Id == id).Result;
                getNews.Sorted = sira;

                News model = await _unitOfWork.GetRepository<News>().UpdateAsync(getNews);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public List<NewsListItemDto> newsListOrderRow()
        {
            IEnumerable<News> newsList = _unitOfWork.GetRepository<News>().Filter(x=> x.IsActive == true && x.IsLock == false && x.IsSlide == true , x => x.OrderBy(y => y.RowNo), "guest,users,categories,publishtype", 1, 10);

            if (newsList != null)
            {
                return newsList.Select(x => new NewsListItemDto
                {

                    Id = x.Id,
                    Title = x.Title,
                    Spot = x.Spot,
                    RowNo = x.RowNo,
                    ColNo = x.ColNo,
                    Image = x.Image,
                    NewsContent = x.NewsContent,
                    IsSlide = x.IsSlide,
                    IsOpenNotifications = x.IsOpenNotifications,
                    IsLock = x.IsLock,
                    IsActive = x.IsActive,
                    Views = x.Views,
                    IsTitle = x.IsTitle,
                    MetaTitle = x.MetaTitle,
                    VideoSlug = x.VideoSlug,
                    VideoUploaded = x.VideoUploaded,
                    doublePlace = x.doublePlace,
                    fourthPlace = x.fourthPlace,
                    isArchive = x.isArchive,
                    UpdatedTime = x.UpdatedTime,
                    CreatedTime = x.CreatedTime,
                    CategoryId = x.CategoryId,
                    UserId = x.UserId,
                    GuestId = x.GuestId,
                    ParentNewsId = x.ParentNewsId,
                    PublishTypeId = x.PublishTypeId,
                    PublishedTime = x.PublishedTime,
                    IsCommentActive = x.IsCommentActive,
                    Sorted = x.Sorted,
                    guest = x.guest,
                    publishtype = x.publishtype,
                    users = x.users,
                    categories = x.categories,
                    Sound = x.Sound,

                }).ToList();
            }
            else
            {
                return null;
            }

        }
        public List<NewsListItemDto> newsListJsonData()
        {
            IEnumerable<News> newsList = _unitOfWork.GetRepository<News>().Filter(x => x.IsOpenNotifications == true, x => x.OrderBy(y => y.Id), "", null, null);

            if (newsList != null)
            {
                return newsList.Select(x => new NewsListItemDto
                {

                    Id = x.Id,
                    Image = x.Image,
                    Title = x.Title,
                    VideoSlug = x.VideoSlug,
                    VideoUploaded = x.VideoUploaded,
                    RowNo = x.RowNo,
                    Sorted = x.Sorted,
                    ColNo = x.ColNo,
                    IsCommentActive = x.IsCommentActive,
                    IsOpenNotifications = x.IsOpenNotifications,
                    IsSlide = x.IsSlide,
                    Views = x.Views,
                    IsTitle = x.IsTitle,
                    doublePlace = x.doublePlace,
                    fourthPlace = x.fourthPlace,
                    isArchive = x.isArchive,
                    MetaTitle = x.MetaTitle,
                    IsActive = x.IsActive,
                    IsLock = x.IsLock,
                    UpdatedTime = DateTime.Now,
                    CreatedTime = x.CreatedTime,
                    UserId = x.UserId,
                    CategoryId = x.CategoryId,
                    ParentNewsId = x.ParentNewsId,
                    GuestId = x.GuestId,
                    PublishTypeId = x.PublishTypeId,
                    PublishedTime = x.PublishedTime,
                    Sound = x.Sound,
                   
                }).ToList();
            }
            else
            {
                return null;
            }
        }

        public async Task<int> insertViewNews(int Id)
        {
            News getNews = await _unitOfWork.GetRepository<News>().FindAsync(x => x.Id == Id);
            getNews.Views += 1;
            News model = await _unitOfWork.GetRepository<News>().UpdateAsync(getNews);
            return model.Id;
        }

        public async Task<bool> editParentId(int haberId, int currentId)
        {
            News getNews = await _unitOfWork.GetRepository<News>().FindAsync(x => x.Id == currentId);
            getNews.ParentNewsId = haberId;
            News model = await _unitOfWork.GetRepository<News>().UpdateAsync(getNews);
            return true;
        }

        public async Task<bool> dropParentRelation(int haberId)
        {
            News getNews = await _unitOfWork.GetRepository<News>().FindAsync(x => x.Id == haberId);
            getNews.ParentNewsId = 0;
            News model = await _unitOfWork.GetRepository<News>().UpdateAsync(getNews);
            return true;
        }

        public async Task<bool> changeSortedItem(int itemId, int count)
        {
            News getNews = await _unitOfWork.GetRepository<News>().FindAsync(x => x.Id == itemId);
            getNews.RowNo = count;
            News model = await _unitOfWork.GetRepository<News>().UpdateAsync(getNews);
            return true;
        }
        public async Task<bool> insertVideoToNews(int haberId, string videoSlug)
        {
            News getNews = await _unitOfWork.GetRepository<News>().FindAsync(x => x.Id == haberId);
            getNews.VideoUploaded = videoSlug;
            News model = await _unitOfWork.GetRepository<News>().UpdateAsync(getNews);
            return true;
        }

        public async Task<bool> deleteVideoFromNews(int Id)
        {
            News getNews = await _unitOfWork.GetRepository<News>().FindAsync(x => x.Id == Id);
            getNews.VideoUploaded = null;
            News model = await _unitOfWork.GetRepository<News>().UpdateAsync(getNews);
            return true;
        }

        #endregion

        #region Media

        public List<MediaListItemDto> mediaList()
        {
            IEnumerable<Media> newsList = _unitOfWork.GetRepository<Media>().Filter(null, x => x.OrderBy(y => y.Id), "", null, null);

            if (newsList != null)
            {
                return newsList.Select(x => new MediaListItemDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    CreatedTime = x.CreatedTime,
                    Extension = x.Extension,
                    UpdatedTime = x.UpdatedTime,
                    UserId = x.userId,
                    Slug = x.Slug
                }).ToList();
            }
            else
            {
                return null;
            }
        }

        public async Task<int> insertMedia(MediaDto model)
        {
            Media newMedia = await _unitOfWork.GetRepository<Media>().AddAsync(new Media
            {
               CreatedTime = DateTime.Now,
               UpdatedTime = DateTime.Now,
               Extension = model.Extension,
               Slug = model.Slug,
               Title = model.Title,
               userId = model.UserId

            });

            return newMedia.Id;
        }

        public async Task<bool> placeDoubleHolder(int Id)
        {
            News getNews = _unitOfWork.GetRepository<News>().FindAsync(x => x.Id == Id).Result;
            if (getNews.doublePlace == false)
            {
                getNews.doublePlace = true;
                News model = await _unitOfWork.GetRepository<News>().UpdateAsync(getNews);
                return getNews != null;
            }
            else
            {
                getNews.doublePlace = false;
                News model = await _unitOfWork.GetRepository<News>().UpdateAsync(getNews);
                return getNews != null;
            }
        }

        public async Task<bool> placeFourthHolder(int Id)
        {
            News getNews = _unitOfWork.GetRepository<News>().FindAsync(x => x.Id == Id).Result;
            if (getNews.fourthPlace == false)
            {
                getNews.fourthPlace = true;
                News model = await _unitOfWork.GetRepository<News>().UpdateAsync(getNews);
                return getNews != null;
            }
            else
            {
                getNews.fourthPlace = false;
                News model = await _unitOfWork.GetRepository<News>().UpdateAsync(getNews);
                return getNews != null;
            }
        }

        public async Task<bool> setArchiveNews(int Id)
        {
            News getNews = _unitOfWork.GetRepository<News>().FindAsync(x => x.Id == Id).Result;
            if (getNews.isArchive == false)
            {
                getNews.isArchive = true;
                getNews.IsCommentActive = false;
                getNews.IsSlide = false;
                getNews.IsLock = false;
                getNews.IsTitle = false;
                getNews.IsOpenNotifications = false;
                getNews.doublePlace = false;
                getNews.fourthPlace = false;

                News model = await _unitOfWork.GetRepository<News>().UpdateAsync(getNews);
                return getNews != null;
            }
            else
            {
                getNews.isArchive = false;
                News model = await _unitOfWork.GetRepository<News>().UpdateAsync(getNews);
                return getNews != null;
            }
        }

        public MediaDto getMedia(int Id)
        {
            Media getMedia = _unitOfWork.GetRepository<Media>().FindAsync(x => x.Id == Id).Result;

            if (getMedia != null)
            {
                return new MediaDto
                {
                    Id = getMedia.Id,
                    Extension = getMedia.Extension,
                    CreatedTime = getMedia.CreatedTime,
                    Slug = getMedia.Slug,
                    Title = getMedia.Title,
                    UpdatedTime = getMedia.UpdatedTime,
                    UserId = getMedia.userId,
                };
            }
            else
            {
                return null;
            }
        }

        public async Task<int> updateSliderRow(int Id)
        {
            News getNews = _unitOfWork.GetRepository<News>().FindAsync(x => x.Id == Id).Result;

            int currentRowNo = getNews.RowNo;

            getNews.IsSlide = false;
            getNews.RowNo = 0;

            News model = await _unitOfWork.GetRepository<News>().UpdateAsync(getNews);
            _unitOfWork.Dispose();

            return currentRowNo;
        }

        public async Task<int> updateAllSliderItemRow(int itemId, int rowNo)
        {
            News getNews = _unitOfWork.GetRepository<News>().FindAsync(x => x.Id == itemId).Result;
            if (getNews.RowNo == 11)
            {
                getNews.RowNo = rowNo;
                getNews.IsSlide = true;
            }
            else
            {
                getNews.RowNo = rowNo;
            }

            News model = await _unitOfWork.GetRepository<News>().UpdateAsync(getNews);
            return model.RowNo;
        }

        public async Task<int> updateAllSliderItemRowInsert(int itemId, int rowNo)
        {
            News getNews = _unitOfWork.GetRepository<News>().FindAsync(x => x.Id == itemId).Result;
            if (getNews.RowNo == 10)
            {
                getNews.RowNo = rowNo;
                getNews.IsSlide = false;
            }
            else
            {
                getNews.RowNo = rowNo;
            }

            News model = await _unitOfWork.GetRepository<News>().UpdateAsync(getNews);
            return model.RowNo;
        }

        public async Task<int> updateSliderRowInsert(int Id)
        {
            News getNews = _unitOfWork.GetRepository<News>().FindAsync(x => x.Id == Id).Result;
            getNews.IsSlide = true;
            getNews.doublePlace = false;
            getNews.fourthPlace = false;
            getNews.RowNo = 1;

            News model = await _unitOfWork.GetRepository<News>().UpdateAsync(getNews);
            _unitOfWork.Dispose();

            return model.RowNo;
        }

        public List<NewsListItemDto> newsListBySortedOrder(int resultrow)
        {
            IEnumerable<News> newsList = _unitOfWork.GetRepository<News>().Filter(x=> x.RowNo > resultrow, x => x.OrderBy(y => y.RowNo), "guest,users,categories,publishtype", null, null);

            if (newsList != null)
            {
                return newsList.Select(x => new NewsListItemDto
                {

                    Id = x.Id,
                    Title = x.Title,
                    Spot = x.Spot,
                    RowNo = x.RowNo,
                    MetaTitle = x.MetaTitle,
                    ColNo = x.ColNo,
                    doublePlace = x.doublePlace,
                    Image = x.Image,
                    isArchive = x.isArchive,
                    fourthPlace = x.fourthPlace,
                    NewsContent = x.NewsContent,
                    IsSlide = x.IsSlide,
                    IsOpenNotifications = x.IsOpenNotifications,
                    IsLock = x.IsLock,
                    IsActive = x.IsActive,
                    VideoSlug = x.VideoSlug,
                    VideoUploaded = x.VideoUploaded,
                    Views = x.Views,
                    IsTitle = x.IsTitle,
                    UpdatedTime = x.UpdatedTime,
                    CreatedTime = x.CreatedTime,
                    CategoryId = x.CategoryId,
                    UserId = x.UserId,
                    GuestId = x.GuestId,
                    ParentNewsId = x.ParentNewsId,
                    PublishTypeId = x.PublishTypeId,
                    PublishedTime = x.PublishedTime,
                    IsCommentActive = x.IsCommentActive,
                    Sorted = x.Sorted,
                    guest = x.guest,
                    publishtype = x.publishtype,
                    users = x.users,
                    categories = x.categories,
                    Sound = x.Sound,

                }).ToList();
            }
            else
            {
                return null;
            }
        }

        #endregion

    }
}
