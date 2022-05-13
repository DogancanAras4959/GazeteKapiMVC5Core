using CORE.ApplicationCommon.DTOS.SeoDTO;
using CORE.ApplicationCommon.DTOS.SeoDTO.SeoMetaDto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SERVICE.Engine.Interfaces
{
    public interface ISeoService
    {
        Task<int> CreateSeoScore(SeoScoreDto model, string uniqueCode);
        SeoScoreDto GetSeoScoreByNewsId(int NewsId);
        SeoScoreDto GetSeoScore(int Id);
        Task CreateSeoMetaToSeoScore(int seoScoreId);

        //Task<int> UpdateSeoScore(SeoScoreDto model);
    }
}
