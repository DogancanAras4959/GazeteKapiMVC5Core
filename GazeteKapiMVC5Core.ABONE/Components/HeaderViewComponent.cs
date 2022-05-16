using GazeteKapiMVC5Core.ABONE.Core.Extensions;
using GazeteKapiMVC5Core.ABONE.Models.UserModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GazeteKapiMVC5Core.ABONE.Components
{
    public class HeaderViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            KullaniciGetir();
            return View();
        }

        public void KullaniciGetir()
        {
            UserEditViewModel yoneticiGetir = SessionExtensionMethod.GetObject<UserEditViewModel>(HttpContext.Session, "user");
            if (yoneticiGetir != null)
            {
                ViewBag.Yonetici = yoneticiGetir;
            }
        }
    }
}
