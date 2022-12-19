using CORE.ApplicationCommon.DTOS.BannersDTO;
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
    public class BannerService : IBannerService
    {
        private readonly IUnitOfWork<NewsAppContext> _unitOfWork;
        public BannerService(IUnitOfWork<NewsAppContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<BannerListItemDto> BannerList()
        {
            IEnumerable<Banners> magazineList = _unitOfWork.GetRepository<Banners>().Filter(null, x=> x.OrderBy(y=> y.Id),"bannerRotate",null, null);
            return magazineList.Select(x => new BannerListItemDto
            {

                Id = x.Id,
                BannerImage = x.BannerImage,
                Link = x.Link,
                IsActive = x.IsActive,
                UpdatedTime = x.UpdatedTime,
                CreatedTime = x.CreatedTime,
                RotateId = x.RotateId,
                BannerFrame = x.BannerFrame,
                BannerName = x.BannerName,
                bannerRotate = x.bannerRotate,
            }).ToList();
        }

        public List<BannerListItemDto> bannerListTakeOneToWeb(int count)
        {
            IEnumerable<Banners> magazineList = _unitOfWork.GetRepository<Banners>().Filter(x=> x.IsActive == true, x=> x.OrderBy(y=> y.Id),"bannerRotate",null, null);
            return magazineList.Select(x => new BannerListItemDto
            {

                Id = x.Id,
                BannerImage = x.BannerImage,
                Link = x.Link,
                IsActive = x.IsActive,
                UpdatedTime = x.UpdatedTime,
                CreatedTime = x.CreatedTime,
                BannerName = x.BannerName,
                BannerFrame = x.BannerFrame,
                RotateId = x.RotateId,
                bannerRotate = x.bannerRotate,

            }).Take(count).ToList();
        }

        public async Task<bool> createBanner(BannerDto model)
        {
            Banners newMagazine = await _unitOfWork.GetRepository<Banners>().AddAsync(new Banners
            {
                BannerImage = model.BannerImage,
                CreatedTime = DateTime.Now,
                IsActive = false,
                Link = model.Link,
                UpdatedTime = DateTime.Now,
                BannerFrame = model.BannerFrame,
                BannerName = model.BannerName,
                RotateId = model.RotateId,

            });

            return newMagazine != null && newMagazine.Id != 0;
        }

        public bool DeleteBanner(int Id)
        {
            Task<int> result = _unitOfWork.GetRepository<Banners>().DeleteAsync(new Banners { Id = Id });
            return Convert.ToBoolean(result.Result);
        }

        public async Task<bool> editBannerDto(BannerDto model)
        {
            Banners getMagazine = await _unitOfWork.GetRepository<Banners>().FindAsync(x=> x.Id == model.Id);

            if (model.BannerImage == null)
            {
                model.BannerImage = getMagazine.BannerImage;
            }

            Banners magazineBannerGet = await _unitOfWork.GetRepository<Banners>().UpdateAsync(new Banners
            {
                Id = getMagazine.Id,
                BannerImage = model.BannerImage,
                BannerName = model.BannerName,
                BannerFrame = model.BannerFrame,
                RotateId = model.RotateId,
                Link = model.Link,
                UpdatedTime = DateTime.Now,
                IsActive = model.IsActive,
            });

            return magazineBannerGet != null;
        }

        public BannerDto getBanner(int Id)
        {
            Banners getMagazine = _unitOfWork.GetRepository<Banners>().FindAsync(x=> x.Id == Id).Result;

            if (getMagazine == null)
            {
                return new BannerDto();
            }

            return new BannerDto
            {
                Id = getMagazine.Id,
                BannerImage = getMagazine.BannerImage,
                CreatedTime = getMagazine.CreatedTime,
                UpdatedTime = getMagazine.UpdatedTime,
                BannerFrame = getMagazine.BannerFrame,
                BannerName = getMagazine.BannerName,
                RotateId = getMagazine.RotateId,
                IsActive = getMagazine.IsActive,
                Link = getMagazine.Link,
            };
        }

        public async Task<bool> IsActiveEnabledBanner(int id)
        {
            Banners getMagazine = _unitOfWork.GetRepository<Banners>().FindAsync(x=> x.Id == id).Result;

            if (getMagazine.IsActive == false)
            {
                getMagazine.IsActive = true;
            }
            else
            {
                getMagazine.IsActive = false;
            }

            Banners currentMagazine = await _unitOfWork.GetRepository<Banners>().UpdateAsync(getMagazine);
            return currentMagazine != null;
        }
    }
}
