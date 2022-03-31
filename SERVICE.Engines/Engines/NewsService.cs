using CORE.ApplicationCommon.DTOS.NewsDTO;
using CORE.ApplicationCommon.DTOS.NewsDTO.GuestDTO;
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

        public async Task<bool> createGuets(GuestDto model)
        {
            Guest newGuest = await _unitOfWork.GetRepository<Guest>().AddAsync(new Guest
            {
                GuestName = model.GuestName,
                GuestImage = model.GuestImage,
                Biography = model.Biography,
                IsActive = true,
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
                IsActive = getGuest.IsActive,
                UpdatedTime = DateTime.Now,
                CreatedTime = getGuest.CreatedTime,
                users = getGuest.users,
                UserId = model.UserId,
            });

            return guestGet != null;
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
            IEnumerable<Guest> guestList = _unitOfWork.GetRepository<Guest>().Filter(null, x => x.OrderByDescending(y => y.Id), "users", null, null);

            return guestList.Select(x => new GuestListItemDto
            {
                Id = x.Id,
                GuestName = x.GuestName,
                GuestImage = x.GuestImage,
                Biography = x.Biography,
                UpdatedTime = x.UpdatedTime,
                CreatedTime = x.CreatedTime,
                Email = x.Email,
                UserId = x.UserId,
                user = x.users,
                IsActive = x.IsActive,
            }).ToList();
        }

        public List<NewsListItemDto> newsList()
        {
            IEnumerable<News> newsList = _unitOfWork.GetRepository<News>().Filter(null, x => x.OrderByDescending(y => y.Id), "guest,users,categories,publishtype", null,null);

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
                if (model.GuestId == 0)
                {
                    model.GuestId = 3;
                }

                News createNews = await _unitOfWork.GetRepository<News>().AddAsync(new News
                {
                    Title = model.Title,
                    Spot = model.Spot,
                    IsSlide = model.IsSlide,
                    IsActive = true,
                    IsLock = false,
                    IsCommentActive = model.IsCommentActive,
                    IsOpenNotifications = model.IsOpenNotifications,
                    UpdatedTime = DateTime.Now,
                    CreatedTime = DateTime.Now,
                    Views = 0,
                    CategoryId = model.CategoryId,
                    UserId = model.UserId,
                    GuestId = model.GuestId,
                    PublishTypeId = model.PublishTypeId,
                    NewsContent = model.NewsContent,
                    Image = model.Image,
                    Sorted = 0,
                });

                return createNews.Id;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public NewsDto getNews(int id)
        {

            News getNews = _unitOfWork.GetRepository<News>().FindAsync(x => x.Id == id).Result;

            if (getNews != null)
            {
                return new NewsDto
                {
                    Id = getNews.Id,
                    Title = getNews.Title,
                    Spot = getNews.Spot,
                    IsSlide = getNews.IsSlide,
                    IsActive = getNews.IsActive,
                    IsLock = getNews.IsLock,
                    IsCommentActive = getNews.IsCommentActive,
                    IsOpenNotifications = getNews.IsOpenNotifications,
                    UpdatedTime = getNews.UpdatedTime,
                    CreatedTime = getNews.CreatedTime,
                    Views = getNews.Views,
                    CategoryId = getNews.CategoryId,
                    UserId = getNews.UserId,
                    GuestId = getNews.GuestId,
                    PublishTypeId = getNews.PublishTypeId,
                    NewsContent = getNews.NewsContent,
                    Image = getNews.Image,
                    Sorted = getNews.Sorted,
                };
            }
            else
            {
                return null;
            }
        }

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
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<int> editNews(NewsDto model)
        {
            try
            {
                News getNews = await _unitOfWork.GetRepository<News>().FindAsync(x => x.Id == model.Id);

                if (model.GuestId == 0)
                {
                    model.GuestId = 3;
                }

                if (model.Image == null)
                {
                    model.Image = getNews.Image;
                }

                if (model.CreatedTime == null)
                {
                    model.CreatedTime = getNews.CreatedTime;
                }

                News newsGet = await _unitOfWork.GetRepository<News>().UpdateAsync(new News
                {
                    Id = model.Id,
                    Image = model.Image,
                    Title = model.Title,
                    Spot = model.Spot,
                    NewsContent = model.NewsContent,
                    IsCommentActive = model.IsCommentActive,
                    IsOpenNotifications = model.IsOpenNotifications,
                    IsSlide = model.IsSlide,
                    UpdatedTime = DateTime.Now,
                    CreatedTime = model.CreatedTime,
                    users = model.users,
                    UserId = model.UserId,
                    CategoryId = model.CategoryId,
                    categories = model.categories,
                    guest = model.guest,
                    GuestId = model.GuestId,
                    PublishTypeId = model.PublishTypeId,
                    publishtype = model.publishtype,
                    PublishedTime = model.PublishedTime,
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
            IEnumerable<Tags> newsList = _unitOfWork.GetRepository<Tags>().Filter(null, x => x.OrderByDescending(y => y.Id), "",null,null);

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

                return getNews.Select(x=> new NewsListItemDto 
                {
                    Id = x.Id,
                    Title = x.Title,
                    Spot = x.Spot,
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

                }).ToList();
            }
            catch (Exception ex)
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
                    Spot = x.Spot,
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

        public List<TagListItemDto> tagListWithSearch(string searchName)
        {
            IEnumerable<Tags> getTags = _unitOfWork.GetRepository<Tags> ().Filter(null, x => x.OrderByDescending(y => y.Id), "", null, null);

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
                IEnumerable<News> getNews = _unitOfWork.GetRepository<News>().Filter(x=> x.GuestId == guestId, x => x.OrderByDescending(y => y.Id), "users,guest,publishtype,categories", null, null);

                if (!String.IsNullOrEmpty(searchstring))
                {
                    getNews = getNews.Where(x => x.Title!.Contains(searchstring));
                }

                return getNews.Select(x => new NewsListItemDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Spot = x.Spot,
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

                }).ToList();
            }
            catch (Exception ex)
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

                }).ToList();
            }
            catch (Exception ex)
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
                    Title = x.Title,
                    Spot = x.Spot,
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

                }).ToList();
            }
            catch (Exception ex)
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

                }).ToList();
            }
            else
            {
                return null;
            }
        }
    }
}
