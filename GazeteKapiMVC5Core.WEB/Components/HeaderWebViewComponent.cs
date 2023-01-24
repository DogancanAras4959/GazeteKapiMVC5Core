using AutoMapper;
using CORE.ApplicationCommon.DTOS.BannersDTO;
using CORE.ApplicationCommon.DTOS.CategoryDTO;
using CORE.ApplicationCommon.DTOS.CurrencyDTO;
using CORE.ApplicationCommon.DTOS.MagazineBannerDTO;
using CORE.ApplicationCommon.DTOS.NewsDTO;
using CORE.ApplicationCommon.DTOS.SetingsDTO;
using GazeteKapiMVC5Core.WEB.Models;
using GazeteKapiMVC5Core.WEB.ViewModels.Banner;
using GazeteKapiMVC5Core.WEB.ViewModels.Categories;
using GazeteKapiMVC5Core.WEB.ViewModels.Currencies;
using GazeteKapiMVC5Core.WEB.ViewModels.MagazineBanner;
using GazeteKapiMVC5Core.WEB.ViewModels.News;
using GazeteKapiMVC5Core.WEB.ViewModels.Settings;
using Microsoft.AspNetCore.Mvc;
using SERVICE.Engine.Interfaces;
using SERVICES.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;

namespace GazeteKapiMVC5Core.WEB.Components
{
    public class HeaderWebViewComponent : ViewComponent
    {
        private readonly ICategoryService _categoryService;
        private readonly ISettingService _siteSetting;
        private readonly IMagazineBannerService _magazineBannerService;
        private readonly IBannerService _bannerService;
        private readonly INewsService _newsService;
        private readonly IMapper _mapper;

        public HeaderWebViewComponent(ICategoryService categoryService, IMapper mapper, ISettingService siteSetting, IMagazineBannerService magazineBannerService, IBannerService bannerService, INewsService newsService)
        {
            _bannerService = bannerService;
            _categoryService = categoryService;
            _magazineBannerService = magazineBannerService;
            _siteSetting = siteSetting;
            _mapper = mapper;
            _newsService = newsService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            ViewBag.CategoryActive = _mapper.Map<CategoryDto, CategoryEditViewModelWeb>(_categoryService.GetCategoryFirstOrDefault());

            List<CategoryListViewModelWeb> categoryList = _mapper.Map<List<CategoryListItemDto>, List<CategoryListViewModelWeb>>(_categoryService.GetAllCategory());
            ViewBag.CategoryList = categoryList;

            List<NewListViewModelWeb> news = _mapper.Map<List<NewsListItemDto>, List<NewListViewModelWeb>>(_newsService.newsListWithWeb());
            ViewBag.News = news;


            List<MagazineBannerListViewModelWeb> getMagazineBanner = _mapper.Map<List<MagazineBannerListItemDto>, List<MagazineBannerListViewModelWeb>>(_magazineBannerService.magazineListTakeOneToWeb(1));

            ViewBag.Magazine = getMagazineBanner;

            await GetCurrencyServiceAsync();
            var siteSetting = _mapper.Map<SettingsDto, SettingsBaseViewModelWeb>(_siteSetting.getSettings(1));
            LoadBannerData();

            return View(siteSetting);
        }

        private void LoadBannerData()
        {
            var bannerSag = _mapper.Map<BannerDto, BannerEditViewModelWeb>(_bannerService.getBanner(1));
            var bannerSol = _mapper.Map<BannerDto, BannerEditViewModelWeb>(_bannerService.getBanner(2));
            var bannerUst = _mapper.Map<BannerDto, BannerEditViewModelWeb>(_bannerService.getBanner(3));

            ViewBag.BannerSag = bannerSag;
            ViewBag.BannerSol = bannerSol;
            ViewBag.BannerUst = bannerUst;

        }

        public async Task GetCurrencyServiceAsync()
        {
            try
            {
                XmlDocument xml = new XmlDocument(); // yeni bir XML dökümü oluşturuyoruz

                xml.Load("https://www.tcmb.gov.tr/kurlar/today.xml");

                // bağlantı kuruyoruz.
                var Tarih_Date_Nodes = xml.SelectSingleNode("//Tarih_Date"); // Count değerini almak için ana boğumu seçiyoruz.

                if (Tarih_Date_Nodes == null)
                {
                    ViewData["dovizler"] = "Kur bilgisine şu anda ulaşılamıyor!";
                }

                var getSiteSettings = _mapper.Map<SettingsDto, SettingsEditViewModelWeb>(_siteSetting.getSettings(1));

                var CurrencyNodes = Tarih_Date_Nodes.SelectNodes("//Currency"); // ana boğum altındaki kur boğumunu seçiyoruz.
                int CurrencyLength = CurrencyNodes.Count; // toplam kur boğumu sayısını elde ediyor ve for döngüsünde kullanıyoruz.

                List<doviz> dovizler = new List<doviz>(); // Aşağıda oluşturduğum public class ile bir List oluşturuyoruz.
                CurrencyCreateViewModelWeb model = new CurrencyCreateViewModelWeb();

                if (getSiteSettings.IsCurrencyService == false)
                {
                    for (int i = 0; i < CurrencyLength; i++) // for u çalıştırıyoruz.
                    {
                        var cn = CurrencyNodes[i];
                        model.code = cn.Attributes["Kod"].Value;
                        model.crossorder = cn.Attributes["CrossOrder"].Value;
                        model.currencyCode = cn.Attributes["CurrencyCode"].Value;
                        model.unit = cn.Attributes[0].InnerXml;
                        model.name = cn.ChildNodes[1].InnerXml;
                        model.currencyName = cn.ChildNodes[2].InnerXml;
                        model.ForexBuying = cn.ChildNodes[3].InnerXml;
                        model.ForexSelling = cn.ChildNodes[4].InnerXml;
                        model.BanknoteBuying = cn.ChildNodes[5].InnerXml;
                        model.BanknoteSelling = cn.ChildNodes[6].InnerXml;
                        model.CrossRateOther = cn.ChildNodes[7].InnerXml;
                        model.CrossRateUSD = cn.ChildNodes[8].InnerXml;

                        var code = _mapper.Map<CurrencyDto, CurrencyEditViewModelWeb>(_siteSetting.getCurrency(model.code));
                        if(code == null)
                        {
                            await _siteSetting.createCurrencyList(_mapper.Map<CurrencyCreateViewModelWeb, CurrencyDto>(model));
                        }
                    }

                    getSiteSettings.IsCurrencyService = true;
                    await _siteSetting.editSiteSettings(_mapper.Map<SettingsEditViewModelWeb, SettingsDto>(getSiteSettings));
                }

                else
                {
                    var listCurrencyToDatabase = _mapper.Map<List<CurrencyListItemDto>, List<CurrencyListViewModelWeb>>(_siteSetting.currencyLisToDatabase());

                    for (int i = 0; i < CurrencyLength; i++) // for u çalıştırıyoruz.
                    {
                        var cn = CurrencyNodes[i]; // kur boğumunu alıyoruz.
                                                   // Listeye kur bilgirini ekliyoruz.
                        dovizler.Add(new doviz
                        {
                            Kod = cn.Attributes["Kod"].Value,
                            CrossOrder = cn.Attributes["CrossOrder"].Value,
                            CurrencyCode = cn.Attributes["CurrencyCode"].Value,
                            Unit = cn.ChildNodes[0].InnerXml,
                            Isim = cn.ChildNodes[1].InnerXml,
                            CurrencyName = cn.ChildNodes[2].InnerXml,
                            ForexBuying = cn.ChildNodes[3].InnerXml,
                            ForexSelling = cn.ChildNodes[4].InnerXml,
                            BanknoteBuying = cn.ChildNodes[5].InnerXml,
                            BanknoteSelling = cn.ChildNodes[6].InnerXml,
                            CrossRateOther = cn.ChildNodes[7].InnerXml,
                            CrossRateUSD = cn.ChildNodes[8].InnerXml,
                        });

                        foreach (var item in listCurrencyToDatabase)
                        {
                            if (item.code == cn.Attributes["Kod"].Value)
                            {
                                string code = item.code;
                                string serviceBuying = cn.ChildNodes[3].InnerXml;

                                var getCurrency = _mapper.Map<CurrencyDto, CurrencyEditViewModelWeb>(_siteSetting.getCurrency(code));

                                decimal databaseBuying = Convert.ToDecimal(getCurrency.ForexBuying);
                                decimal serviceBuyingConvert = Convert.ToDecimal(serviceBuying);

                                if (serviceBuyingConvert > databaseBuying)
                                {
                                    getCurrency.isRateOrDown = "increase";
                                    getCurrency.ForexBuying = cn.ChildNodes[3].InnerXml;
                                    await _siteSetting.editCurrencyList(_mapper.Map<CurrencyEditViewModelWeb, CurrencyDto>(getCurrency));
                                    break;

                                }
                                else if (serviceBuyingConvert < databaseBuying)
                                {
                                    getCurrency.isRateOrDown = "down";
                                    getCurrency.ForexBuying = cn.ChildNodes[3].InnerXml;
                                    await _siteSetting.editCurrencyList(_mapper.Map<CurrencyEditViewModelWeb, CurrencyDto>(getCurrency));
                                    break;

                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                    ViewBag.CurrencyList = listCurrencyToDatabase;
                }

                ViewData["dovizler"] = dovizler; // dovizler List değerini data ya atıyoruz ön tarafta viewbag ile çekeceğiz.
            }
            catch (Exception ex)
            {
                ViewData["dovizler"] = ex.ToString();
            }
        }
    }
}
