using CORE.ApplicationCommon.DTOS.SeoDTO;
using CORE.ApplicationCommon.DTOS.SeoDTO.SeoMetaDto;
using GazeteKapiMVC5Core.DataAccessLayer.Models;
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
        List<SeoMetaListItemDto> listSeoMetasBySeoScoreId(int seoScoreId);
        List<SeoMetaListItemDto> listSeoMetasBySeoScoreIdByAnalyze(int seoScoreId);

        bool UpdateSeoScoreAfterCreateTask(int Id);
        SeoCheckMeta SeoMetaIsDone(int Id);
        SeoCheckMeta SeoMetaIsNotDone(int Id);
        SeoScore DownPointSeoScore(int SeoScoreId, int point);
        SeoScore IncreaseSeoScore(int seoScoreId, int point);
    }
}
