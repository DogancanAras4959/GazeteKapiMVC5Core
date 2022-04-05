using AutoMapper;
using CORE.ApplicationCommon.DTOS.CategoryDTO;
using CORE.ApplicationCommon.DTOS.NewsDTO;
using CORE.ApplicationCommon.DTOS.NewsDTO.GuestDTO;
using CORE.ApplicationCommon.DTOS.NewsDTO.TagDTO;
using CORE.ApplicationCommon.DTOS.NewsDTO.TagNewsDTO;
using GazeteKapiMVC5Core.Core.Extensions;
using GazeteKapiMVC5Core.Models.Category;
using GazeteKapiMVC5Core.Models.News.GuestModel;
using GazeteKapiMVC5Core.Models.News.NewsModel;
using GazeteKapiMVC5Core.Models.News.TagModel;
using GazeteKapiMVC5Core.Models.News.TagNewsModel;
using Microsoft.AspNetCore.Mvc;
using SERVICE.Engine.Interfaces;
using SERVICES.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GazeteKapiMVC5Core.WEB.Controllers
{
    public class anasayfaController : Controller
    {

        private readonly IMapper _mapper;
        private readonly INewsService _newService;
        private readonly ICategoryService _categoryService;
        public anasayfaController(INewsService newService, ICategoryService categoryService, IMapper mapper)
        {
            _newService = newService;
            _categoryService = categoryService;
            _mapper = mapper;
        }

        public IActionResult sayfa()
        {
            List<NewsLıstItemModel> haberlist = null;
            haberlist = _mapper.Map<List<NewsListItemDto>, List<NewsLıstItemModel>>(_newService.newsList());

            List<GuestListViewModel> guestList = null;
            guestList = _mapper.Map<List<GuestListItemDto>,List<GuestListViewModel>>(_newService.guestList());

            List<TagNewsListViewModel> tagNewList = null;
            tagNewList = _mapper.Map<List<TagNewsListItemDto>, List<TagNewsListViewModel>>(_newService.tagsListWithNewsWeb());

            List<CategoryListViewModel> categoryList = null;
            categoryList = _mapper.Map<List<CategoryListItemDto>, List<CategoryListViewModel>>(_categoryService.GetAllCategory());

            ViewBag.TagNews = tagNewList;
            ViewBag.HaberlerManset = haberlist;
            ViewBag.GuestList = guestList;
            ViewBag.CategoryList = categoryList;

            return View();
        }

        public IActionResult aramasonucu(int? pageNumber, string searchnews) 
        {
            int pageSize = 20;
            List<NewsLıstItemModel> haberlist = null;
            List<TagNewsListViewModel> tagNewList = null;
            List<CategoryListViewModel> categoryList = null;
            List<NewsLıstItemModel> modelNew = new List<NewsLıstItemModel>();

            if (searchnews != null && searchnews != "")
            {
                haberlist = _mapper.Map<List<NewsListItemDto>, List<NewsLıstItemModel>>(_newService.searchDataInNews(searchnews));

                tagNewList = _mapper.Map<List<TagNewsListItemDto>, List<TagNewsListViewModel>>(_newService.tagsListWithNewsWebSearch(searchnews));

                foreach (var item in haberlist)
                {
                    NewsLıstItemModel model = new NewsLıstItemModel();
                    model.Id = item.Id;
                    model.Image = item.Image;
                    model.Title = item.Title;
                    model.Spot = item.Spot;
                    model.PublishedTime = item.PublishedTime;
                    model.CategoryId = item.CategoryId;
                    modelNew.Add(model);
                }

                foreach (var item in tagNewList)
                {

                    NewsLıstItemModel model = new NewsLıstItemModel();
                    model.Id = item.news.Id;
                    model.Image = item.news.Image;
                    model.Title = item.news.Title;
                    model.Spot = item.news.Spot;
                    model.PublishedTime = item.news.PublishedTime;
                    model.CategoryId = item.news.CategoryId;
                    modelNew.Add(model);
                }

                TempData["kelime"] = searchnews;
                
                int count = modelNew.Count;
                TempData["toplam"] = count;

                categoryList = _mapper.Map<List<CategoryListItemDto>, List<CategoryListViewModel>>(_categoryService.GetAllCategory());

                ViewBag.Categories = categoryList;

                NewsLıstItemModel[] tagsList = modelNew.GroupBy(o => new { o.Title }).Select(x=> x.FirstOrDefault()).ToArray();

                return View(PaginationList<NewsLıstItemModel>.Create(tagsList.ToList(), pageNumber ?? 1, pageSize));
            }
            return View();
        }

        public IActionResult kategori(int? pageNumber, string categoryName)
        {
            TempData["kategori"] = categoryName;
            return View();
        }

        public IActionResult Header()
        {
            return PartialView("Header");
        }

        public IActionResult Slider()
        {
            return PartialView("Slider");
        }

        public IActionResult Carousel()
        {
            return PartialView("Carousel");
        }

        public IActionResult Gundem()
        {
            return PartialView("Gundem");
        }

        public IActionResult Yazar()
        {
            return PartialView("Yazar");
        }

        public IActionResult Footer()
        {
            return PartialView("Footer");
        }

    }
}
