using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SERVICE.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GazeteKapiMVC5Core.Controllers
{
    public class OnbellekController : Controller
    {
        private readonly IMyCache _myCache;

        public OnbellekController(IMyCache myCache)
        {
            _myCache = myCache;
        }

        public IActionResult onbellegitemizle()
        {
            this._myCache.Clear();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult onbellek()
        {
            var result = this._myCache.Select(t => new
            {
                Key = t.Key,
                Value = t.Value
            }).ToArray();
            return new JsonResult(result);
        }


    }
}
