using AutoMapper;
using CORE.ApplicationCommon.DTOS.NewsDTO;
using CORE.ApplicationCommon.DTOS.NewsDTO.GuestDTO;
using CORE.ApplicationCommon.DTOS.NewsDTO.TagDTO;
using CORE.ApplicationCommon.DTOS.NewsDTO.TagNewsDTO;
using GazeteKapiMVC5Core.Models.News.GuestModel;
using GazeteKapiMVC5Core.Models.News.NewsModel;
using GazeteKapiMVC5Core.Models.News.TagNewsModel;
using Microsoft.AspNetCore.Mvc;
using SERVICE.Engine.Interfaces;
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
        public anasayfaController(INewsService newService, IMapper mapper)
        {
            _newService = newService;
            _mapper = mapper;
        }

        public IActionResult sayfa()
        {
            List<NewsLıstItemModel> haberlist = null;
            haberlist = _mapper.Map<List<NewsListItemDto>, List<NewsLıstItemModel>>(_newService.newsList());
            ViewBag.HaberlerManset = haberlist;

            List<GuestListViewModel> guestList = null;
            guestList = _mapper.Map<List<GuestListItemDto>,List<GuestListViewModel>>(_newService.guestList());
            ViewBag.GuestList = guestList;

            List<TagNewsListViewModel> tagNewList = null;
            tagNewList = _mapper.Map<List<TagNewsListItemDto>, List<TagNewsListViewModel>>(_newService.tagsListWithNewsWeb());
            ViewBag.TagNews = tagNewList;

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
