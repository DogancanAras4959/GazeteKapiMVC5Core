using CORE.ApplicationCommon.DTOS.IpAddressDTO;
using CORE.ApplicationCore.UnitOfWork;
using GazeteKapiMVC5Core.DataAccessLayer;
using GazeteKapiMVC5Core.DataAccessLayer.Models;
using SERVICE.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SERVICE.Engine.Engines
{
    public class IPAddresService : IIPAddresService
    {

        private readonly IUnitOfWork<NewsAppContext> _unitOfWork;
        public IPAddresService(IUnitOfWork<NewsAppContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> createIpAddressInDatabase(IpAdressDto model)
        {

            IpAddresCount newIp = await _unitOfWork.GetRepository<IpAddresCount>().AddAsync(new IpAddresCount
            {
                ipAddress = model.ipAddress
            });

            return newIp != null && newIp.Id != 0;
        }

        public IpAdressDto getIpAdress(string ip)
        {
            IpAddresCount getIp = _unitOfWork.GetRepository<IpAddresCount>().FindAsync(x=> x.ipAddress == ip).Result;

            if (getIp == null)
            {
                return new IpAdressDto();
            }

            return new IpAdressDto
            {
                Id = getIp.Id,
                ipAddress = getIp.ipAddress,
            };
        }
    }
}
