using AutoMapper;
using CORE.ApplicationCommon.DTOS.NewsDTO;
using GazeteKapiMVC5Core.Models.News.NewsModel;
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

            return View();
        }
    }
}
