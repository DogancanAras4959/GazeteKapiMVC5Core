using CORE.ApplicationCommon.DTOS.MagazineBannerDTO;
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
    public class MagazineBannerService : IMagazineBannerService
    {
        private readonly IUnitOfWork<NewsAppContext> _unitOfWork;
        public MagazineBannerService(IUnitOfWork<NewsAppContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> createMagazineBanner(MagazineBannerDto model)
        {
            Magazinebanner newMagazine = await _unitOfWork.GetRepository<Magazinebanner>().AddAsync(new Magazinebanner
            {
                BannerImage = model.BannerImage,
                CreatedTime = DateTime.Now,
                IsActive = false,
                Link = model.Link,
                UpdatedTime = DateTime.Now
            });

            return newMagazine != null && newMagazine.Id != 0;

        }

        public bool DeleteBanner(int Id)
        {
            Task<int> result = _unitOfWork.GetRepository<Magazinebanner>().DeleteAsync(new Magazinebanner { Id = Id });
            return Convert.ToBoolean(result.Result);
        }

        public async Task<bool> editMagazineBannerDto(MagazineBannerDto model)
        {
            Magazinebanner getMagazine = await _unitOfWork.GetRepository<Magazinebanner>().FindAsync(x=> x.Id == model.Id);
          
            if(model.BannerImage == null)
            {
                model.BannerImage = getMagazine.BannerImage;
            }

            Magazinebanner magazineBannerGet = await _unitOfWork.GetRepository<Magazinebanner>().UpdateAsync(new Magazinebanner
            {
                Id = getMagazine.Id,
                BannerImage = model.BannerImage,
                Link = model.Link,
                UpdatedTime = DateTime.Now,
                IsActive = model.IsActive,
            });

            return magazineBannerGet != null;
        }

        public MagazineBannerDto getMagazine(int Id)
        {
            Magazinebanner getMagazine = _unitOfWork.GetRepository<Magazinebanner>().FindAsync(x=> x.Id == Id).Result;

            if(getMagazine == null)
            {
                return new MagazineBannerDto();
            }

            return new MagazineBannerDto
            {
                Id = getMagazine.Id,
                BannerImage = getMagazine.BannerImage,
                CreatedTime = getMagazine.CreatedTime,
                UpdatedTime = getMagazine.UpdatedTime,
                IsActive = getMagazine.IsActive,
                Link = getMagazine.Link,
            };
        }

        public async Task<bool> IsActiveEnabledMagazine(int id)
        {
            Magazinebanner getMagazine = _unitOfWork.GetRepository<Magazinebanner>().FindAsync(x=> x.Id == id).Result;
           
            if(getMagazine.IsActive == false)
            {
                getMagazine.IsActive = true;
            }
            else
            {
                getMagazine.IsActive = false;
            }

            Magazinebanner currentMagazine = await _unitOfWork.GetRepository<Magazinebanner>().UpdateAsync(getMagazine);
            return currentMagazine != null;
        }

        public List<MagazineBannerListItemDto> magazineBannerList()
        {
            IEnumerable<Magazinebanner> magazineList = _unitOfWork.GetRepository<Magazinebanner>().Filter(null, x=> x.OrderBy(y=> y.Id),null,null, null);
            return magazineList.Select(x => new MagazineBannerListItemDto 
            {
              
                Id = x.Id,
                BannerImage = x.BannerImage,
                Link = x.Link,
                IsActive = x.IsActive,
                UpdatedTime = x.UpdatedTime,
                CreatedTime = x.CreatedTime

            }).ToList();
        }

        public List<MagazineBannerListItemDto> magazineListTakeOneToWeb(int count)
        {
            IEnumerable<Magazinebanner> magazineList = _unitOfWork.GetRepository<Magazinebanner>().Filter(x=> x.IsActive == true, x=> x.OrderBy(y=> y.Id),null,null, null);
            return magazineList.Select(x => new MagazineBannerListItemDto
            {

                Id = x.Id,
                BannerImage = x.BannerImage,
                Link = x.Link,
                IsActive = x.IsActive,
                UpdatedTime = x.UpdatedTime,
                CreatedTime = x.CreatedTime

            }).Take(count).ToList();
        }
    }
}
