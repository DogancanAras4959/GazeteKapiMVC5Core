using CORE.ApplicationCommon.DTOS.BannersRotateDTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace SERVICE.Engine.Interfaces
{
    public interface IBannerRotateService
    {
        List<BannersRotateListItemDto> bannersRotateList();
    }
}
