using CORE.ApplicationCommon.DTOS.NewsDTO.IpNewsDTO;
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
    public class NewsIpService : INewsIpService
    {
        private readonly IUnitOfWork<NewsAppContext> _unitOfWork;
        public NewsIpService(IUnitOfWork<NewsAppContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> createNewsIp(IpNewsDto model)
        {
            NewsIp newIpNews = await _unitOfWork.GetRepository<NewsIp>().AddAsync(new NewsIp
            {
               IpAdressId = model.IpAdressId,
               NewsId = model.NewsId,
            });

            return newIpNews != null && newIpNews.Id != 0;
        }

        public IpNewsDto ipIsExists(int IpId)
        {
            NewsIp getIp = _unitOfWork.GetRepository<NewsIp>().FindAsync(x => x.IpAdressId == IpId).Result;

            return new IpNewsDto
            {
                Id = getIp.Id,
                IpAdressId = getIp.IpAdressId,
                NewsId = getIp.NewsId
            };
        }

        public IpNewsDto listIpByNewsId(int newsId, int ipId)
        {
            NewsIp ipsNews = _unitOfWork.GetRepository<NewsIp>().FindAsync(x=> x.NewsId == newsId && x.IpAdressId == ipId).Result;

            if(ipsNews == null)
            {
                return null;
            }
            else
            {
                return new IpNewsDto
                {
                    Id = ipsNews.Id,
                    IpAdressId = ipsNews.IpAdressId,
                    NewsId = ipsNews.NewsId
                };
            }         
        }
    }
}
