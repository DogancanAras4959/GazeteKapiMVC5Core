using AutoMapper;
using CORE.ApplicationCommon.DTOS.CategoryDTO;
using GazeteKapiMVC5Core.Models.Category;
using GazeteKapiMVC5Core.WEB.Models;
using Microsoft.AspNetCore.Mvc;
using SERVICES.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace GazeteKapiMVC5Core.WEB.Components
{
    public class HeaderWebViewComponent : ViewComponent
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public HeaderWebViewComponent(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        public IViewComponentResult Invoke()
        {
            List<CategoryListViewModel> categoryList = null;
            categoryList = _mapper.Map<List<CategoryListItemDto>, List<CategoryListViewModel>>(_categoryService.GetAllCategory());
            ViewBag.CategoryList = categoryList;
            GetCurrencyService();
            return View();
        }

        public void GetCurrencyService()
        {
            XmlDocument xml = new XmlDocument(); // yeni bir XML dökümü oluşturuyoruz.
            xml.Load("http://www.tcmb.gov.tr/kurlar/today.xml"); // bağlantı kuruyoruz.
            var Tarih_Date_Nodes = xml.SelectSingleNode("//Tarih_Date"); // Count değerini almak için ana boğumu seçiyoruz.

            if (Tarih_Date_Nodes == null)
            {
                ViewData["dovizler"] = "Kur bilgisine şu anda ulaşılamıyor!";
            }
            
            var CurrencyNodes = Tarih_Date_Nodes.SelectNodes("//Currency"); // ana boğum altındaki kur boğumunu seçiyoruz.
            int CurrencyLength = CurrencyNodes.Count; // toplam kur boğumu sayısını elde ediyor ve for döngüsünde kullanıyoruz.

            List<doviz> dovizler = new List<doviz>(); // Aşağıda oluşturduğum public class ile bir List oluşturuyoruz.
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
                    CrossRateOther = cn.ChildNodes[8].InnerXml,
                    CrossRateUSD = cn.ChildNodes[7].InnerXml,
                    
                });
            }

            ViewData["dovizler"] = dovizler; // dovizler List değerini data ya atıyoruz ön tarafta viewbag ile çekeceğiz.
        }
    }
}
