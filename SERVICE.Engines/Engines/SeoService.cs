using CORE.ApplicationCommon.DTOS.SeoDTO;
using CORE.ApplicationCommon.DTOS.SeoDTO.SeoMetaDto;
using CORE.ApplicationCommon.Helpers;
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
    public class SeoService : ISeoService
    {

        private readonly IUnitOfWork<NewsAppContext> _unitOfWork;
        public SeoService(IUnitOfWork<NewsAppContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region SeoScore
        public async Task CreateSeoMetaToSeoScore(int seoScoreId)
        {
            try
            {
                List<SeoCheckMeta> items = CreateListAndBindItems(seoScoreId);

                foreach (SeoCheckMeta seo in items)
                {
                    SeoCheckMeta createMeta = await _unitOfWork.GetRepository<SeoCheckMeta>().AddAsync(seo);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        public async Task<int> CreateSeoScore(SeoScoreDto model, string uniqueCode)
        {
            try
            {
                SeoLevelTypes seo = SeoLevelTypes.skorYok;
                int output = seo.GetValue();

                SeoScore createSeoScore = await _unitOfWork.GetRepository<SeoScore>().AddAsync(new SeoScore
                {
                    UniqeCode = uniqueCode,
                    Note = "SEO Skorunuzu yükseltmek için gerekli düzenlemeleri yapmaya başlayın!",
                    Amount = 0,
                    Level = output,
                    NewsId = model.NewsId,
                    IsFinished = false,
                    IsCreated = true,
                });

                return createSeoScore.Id;

            }

            catch (Exception ex)
            {
                return 0;
            }

        }
        public SeoScoreDto GetSeoScore(int Id)
        {
            SeoScore getSeo = _unitOfWork.GetRepository<SeoScore>().FindAsync(x => x.Id == Id).Result;
            if (getSeo != null)
            {
                return new SeoScoreDto
                {
                    Id = getSeo.Id,
                    Amount = getSeo.Amount,
                    Level = getSeo.Level,
                    NewsId = getSeo.NewsId,
                    Note = getSeo.Note,
                    IsFinished = getSeo.IsFinished,
                    IsCreated = true
                };
            }
            else
            {
                return null;
            }
        }
        public SeoScoreDto GetSeoScoreByNewsId(int NewsId)
        {
            SeoScore getSeo = _unitOfWork.GetRepository<SeoScore>().FindAsync(x => x.NewsId == NewsId).Result;
            if (getSeo != null)
            {
                return new SeoScoreDto
                {
                    Id = getSeo.Id,
                    Amount = getSeo.Amount,
                    Level = getSeo.Level,
                    NewsId = getSeo.NewsId,
                    Note = getSeo.Note,
                    IsFinished = getSeo.IsFinished,
                    IsCreated = getSeo.IsCreated
                };
            }
            else
            {
                return null;
            }
        }

        #endregion

        public List<SeoCheckMeta> CreateListAndBindItems(int seoScoreId)
        {
            List<SeoCheckMeta> items = new List<SeoCheckMeta>();

            SeoCheckMeta item1 = new SeoCheckMeta { Point = 15, Requirement = "Haber ana başlığınız 35-65 karakter arasında olmalıdır", TypeLevel = "High", SeoScoreId = seoScoreId, IsDone = false, metaCode ="b-1" };
            SeoCheckMeta item2 = new SeoCheckMeta { Point = 10, Requirement = "Haberinizde 5-8 arası anahtar kelime bulunmalıdır", TypeLevel = "High", SeoScoreId = seoScoreId, IsDone = false, metaCode="k-1" };
            SeoCheckMeta item3 = new SeoCheckMeta { Point = 12, Requirement = "GIF formatında fotoğraf eklemekten kaçının", TypeLevel = "Middle", SeoScoreId = seoScoreId, IsDone = false, metaCode="i-1" };
            SeoCheckMeta item4 = new SeoCheckMeta { Point = 10, Requirement = "Haberinizin açıklama boyutu 120-320 karakter arası olmalıdır", TypeLevel = "Middle", SeoScoreId = seoScoreId, IsDone = false, metaCode="d-1" };
            SeoCheckMeta item5 = new SeoCheckMeta { Point = 10, Requirement = "Haber başlığınızda anahtar kelimelerden en az biri geçmeli", TypeLevel = "Middle", SeoScoreId = seoScoreId, IsDone = false, metaCode="b-2" };
            SeoCheckMeta item6 = new SeoCheckMeta { Point = 3, Requirement = "Haber açıklamanızda anahtar kelimelerden en az biri geçmeli", TypeLevel = "Easy", SeoScoreId = seoScoreId, IsDone = false, metaCode="d-2" };
            SeoCheckMeta item7 = new SeoCheckMeta { Point = 7, Requirement = "Haberiniz için Ana Başlık giriniz", TypeLevel = "Easy", SeoScoreId = seoScoreId, IsDone = false, metaCode="b-3" };
            SeoCheckMeta item8 = new SeoCheckMeta { Point = 9, Requirement = "Haberiniz için meta açıklaması giriniz", TypeLevel = "Easy", SeoScoreId = seoScoreId, IsDone = false, metaCode="d-3" };
            SeoCheckMeta item9 = new SeoCheckMeta { Point = 10, Requirement = "Haberiniz için bir fotoğraf yükleyin", TypeLevel = "Middle", SeoScoreId = seoScoreId, IsDone = false, metaCode="i-2" };
            SeoCheckMeta item10 = new SeoCheckMeta { Point = 14, Requirement = "Haberiniz için özgün anahtar kelimeler giriniz", TypeLevel = "High", SeoScoreId = seoScoreId, IsDone = false, metaCode="k-2" };

            items.Add(item1);
            items.Add(item2);
            items.Add(item3);
            items.Add(item4);
            items.Add(item5);
            items.Add(item6);
            items.Add(item7);
            items.Add(item8);
            items.Add(item9);
            items.Add(item10);

            return items;
        }

        public List<SeoMetaListItemDto> listSeoMetasBySeoScoreId(int seoScoreId)
        {
            IEnumerable<SeoCheckMeta> newsList = _unitOfWork.GetRepository<SeoCheckMeta>().Filter(x => x.SeoScoreId == seoScoreId && x.IsDone == false, x => x.OrderByDescending(y => y.Point), "seoScoreToMeta", null, null);

            if (newsList != null)
            {
                return newsList.Select(x => new SeoMetaListItemDto
                {

                    Id = x.Id,
                    Requirement = x.Requirement,
                    Point = x.Point,
                    TypeLevel = x.TypeLevel,
                    IsDone = x.IsDone,
                    SeoScoreId = x.SeoScoreId,
                    seoScore = x.seoScoreToMeta,
                    metaCode = x.metaCode,

                }).ToList();
            }
            else
            {
                return null;
            }
        }

        public bool UpdateSeoScoreAfterCreateTask(int Id)
        {
            SeoScore getSeoScore =  _unitOfWork.GetRepository<SeoScore>().FindAsync(x => x.Id == Id).Result;
            getSeoScore.IsCreated = false;
            
            SeoScore model =  _unitOfWork.GetRepository<SeoScore>().UpdateAsync(getSeoScore).Result;
            return model != null;
        }

        public SeoCheckMeta SeoMetaIsDone(int Id)
        {
            SeoCheckMeta getSeoMeta = _unitOfWork.GetRepository<SeoCheckMeta>().FindAsync(x => x.Id == Id).Result;

            SeoCheckMeta checkMeta = _unitOfWork.GetRepository<SeoCheckMeta>().UpdateAsync(new SeoCheckMeta
            {
                Id = getSeoMeta.Id,
                metaCode = getSeoMeta.metaCode,
                SeoScoreId = getSeoMeta.SeoScoreId,
                Point = getSeoMeta.Point,
                Requirement = getSeoMeta.Requirement,
                TypeLevel = getSeoMeta.TypeLevel,
                IsDone = true,
                seoScoreToMeta = getSeoMeta.seoScoreToMeta,

            }).Result;

            return checkMeta;
        }

        public SeoScore IncreaseSeoScore(int seoScoreId, int point)
        {
            var getSeoScore = _unitOfWork.GetRepository<SeoScore>().FindAsync(x => x.Id == seoScoreId).Result;
            getSeoScore.Amount += point;

            if (getSeoScore.Amount == 0)
            {
                getSeoScore.Level = 0;
            }
            else if (getSeoScore.Amount >= 1 && getSeoScore.Amount < 35)
            {
                getSeoScore.Level = 1;
            }
            else if (getSeoScore.Amount > 35 && getSeoScore.Amount < 60)
            {
                getSeoScore.Level = 2;
            }
            else if (getSeoScore.Amount > 60 && getSeoScore.Amount < 85)
            {
                getSeoScore.Level = 3;
            }
            else if (getSeoScore.Amount > 85 && getSeoScore.Amount <= 99)
            {
                getSeoScore.Level = 4;
            }

            return _unitOfWork.GetRepository<SeoScore>().UpdateAsync(getSeoScore).Result;
           
        }

    }
}
