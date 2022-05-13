using GazeteKapiMVC5Core.Core.Extensions;
using GazeteKapiMVC5Core.Models;
using GazeteKapiMVC5Core.Models.Account;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace GazeteKapiMVC5Core.PanelComponent
{
    public class HeaderViewComponent : ViewComponent
    {
        [CheckRoleAuthorize]
        public IViewComponentResult Invoke()
        {
            KullaniciGetir();
            return View();      
        }

        public void KullaniciGetir()
        {           
            AccountEditViewModel yoneticiGetir = SessionExtensionMethod.GetObject<AccountEditViewModel>(HttpContext.Session, "user");
            if (yoneticiGetir != null)
            {
                ViewBag.Yonetici = yoneticiGetir;
            }
        }    
    }
}
