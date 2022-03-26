using CORE.ApplicationCommon.DTOS.NewsDto;
using CORE.ApplicationCommon.DTOS.NewsDto.GuestDto;
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
            IEnumerable<Guest> guestList = _unitOfWork.GetRepository<Guest>().Filter(null, x => x.OrderByDescending(y => y.Id), "users", 1, 30);

            return guestList.Select(x=> new GuestListItemDto 
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
            IEnumerable<News> newsList = _unitOfWork.GetRepository<News>().Filter(null, x => x.OrderByDescending(y => y.Id), "guest,users,categories,publishtype", 1, 700000);

            if (newsList != null)
            {
                return newsList.Select(x => new NewsListItemDto {
                
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
