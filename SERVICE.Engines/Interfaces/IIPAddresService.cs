using CORE.ApplicationCommon.DTOS.IpAddressDTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SERVICE.Engine.Interfaces
{
    public interface IIPAddresService 
    {
        IpAdressDto getIpAdress(string ip);
        Task<int> createIpAddressInDatabase(IpAdressDto model);
    }
}
