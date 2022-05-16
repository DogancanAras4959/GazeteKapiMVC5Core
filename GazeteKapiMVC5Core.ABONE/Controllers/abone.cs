using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GazeteKapiMVC5Core.ABONE.Controllers
{
    public class abone : Controller
    {
        public IActionResult abonelik()
        {
            return View();
        }
    }
}
