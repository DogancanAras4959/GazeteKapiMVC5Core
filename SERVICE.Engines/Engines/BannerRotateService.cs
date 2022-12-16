using CORE.ApplicationCommon.DTOS.BannersRotateDTO;
using CORE.ApplicationCore.UnitOfWork;
using GazeteKapiMVC5Core.DataAccessLayer;
using GazeteKapiMVC5Core.DataAccessLayer.Models;
using SERVICE.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SERVICE.Engine.Engines
{
    public class BannerRotateService : IBannerRotateService
    {
        private readonly IUnitOfWork<NewsAppContext> _unitOfWork;
        public BannerRotateService(IUnitOfWork<NewsAppContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<BannersRotateListItemDto> bannersRotateList()
        {
            IEnumerable<BannersRotate> bannersRotateList = _unitOfWork.GetRepository<BannersRotate>().Filter(null, x => x.OrderBy(y => y.Id), null, 1, 100);

            return bannersRotateList.Select(x => new BannersRotateListItemDto
            {
                Id = x.Id,
                RotateName = x.RotateName,

            }).ToList();
        }
    }
}
