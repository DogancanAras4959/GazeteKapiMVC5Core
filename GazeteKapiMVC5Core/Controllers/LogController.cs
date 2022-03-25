using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GazeteKapiMVC5Core.Controllers
{
    public class LogController : Controller
    {
        public IActionResult Loglar()
        {
            return View();
        }
    }
}
