using AutoMapper;
using CORE.ApplicationCommon.DTOS.NewsDTO;
using GazeteKapiMVC5Core.Core.Extensions;
using GazeteKapiMVC5Core.Models;
using GazeteKapiMVC5Core.Models.News.NewsModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SERVICE.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace GazeteKapiMVC5Core.Controllers
{
    public class HomeController : Controller
    {
     
        private readonly ILogger<HomeController> _logger;
        private readonly ICountService _countService;
        private readonly IMapper _mapper;
        private readonly INewsService _newService;

        public HomeController(ILogger<HomeController> logger, ICountService countService, IMapper mapper, INewsService newService)
        {
            _logger = logger;
            _countService = countService;
            _mapper = mapper;
            _newService = newService;
        }

        [CheckRoleAuthorize]
        public IActionResult Index()
        {
            CountData();
            ListData();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult ErrorPage()
        {         
            return View();
        }

        public void CountData()
        {
            ViewBag.CatCount = _mapper.Map<int>(_countService.CountCategories());
            ViewBag.GuestCount = _mapper.Map<int>(_countService.CountGuests());
            ViewBag.UserCount = _mapper.Map<int>(_countService.CountUsers());
            //ViewBag.LogCount = _mapper.Map<int>(_countService.CountLogs());
            //ViewBag.ErrorCount = _mapper.Map<int>(_countService.CountSystemErrors());
            ViewBag.NewsCount = _mapper.Map<int>(_countService.CountNews());
            ViewBag.TagsCount = _mapper.Map<int>(_countService.CountTags());
        }

        public void ListData()
        {
            var newsList = _mapper.Map<List<NewsListItemDto>, List<NewsLıstItemModel>>(_newService.newsListWithLastOneToFive());
            ViewBag.NewsLastFive = newsList;

            var popularNews = _mapper.Map<List<NewsListItemDto>, List<NewsLıstItemModel>>(_newService.PopularNewsInAdminHome());

            ViewBag.PopularList = popularNews;
        }

    }
}
