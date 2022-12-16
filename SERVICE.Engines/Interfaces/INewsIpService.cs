using CORE.ApplicationCommon.DTOS.NewsDTO.IpNewsDTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SERVICE.Engine.Interfaces
{
    public interface INewsIpService
    {
        Task<bool> createNewsIp(IpNewsDto model);
        IpNewsDto listIpByNewsId(int newsId, int ipId);
        IpNewsDto ipIsExists(int IpId);
    }
}
