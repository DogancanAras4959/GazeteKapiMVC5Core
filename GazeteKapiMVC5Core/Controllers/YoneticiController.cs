using AutoMapper;
using CORE.ApplicationCommon.DTOS.AccountDTO;
using CORE.ApplicationCommon.DTOS.NewsDTO;
using CORE.ApplicationCommon.DTOS.RoleDTO;
using CORE.ApplicationCommon.DTOS.SetingsDTO;
using CORE.ApplicationCommon.Helpers;
using CORE.ApplicationCommon.Helpers.Cyrptography;
using GazeteKapiMVC5Core.Core.Extensions;
using GazeteKapiMVC5Core.Models.Account;
using GazeteKapiMVC5Core.Models.Log.LogModel;
using GazeteKapiMVC5Core.Models.News.NewsModel;
using GazeteKapiMVC5Core.Models.Role;
using GazeteKapiMVC5Core.Models.Settings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SERVICE.Engine.Interfaces;
using SERVICES.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GazeteKapiMVC5Core.Controllers
{
    public class YoneticiController : Controller
    {
        #region Fields / Constructure

        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;
        private IWebHostEnvironment _webHostEnvironment;
        private readonly INewsService _newsService;
        private readonly ISettingService _settingService;
        public YoneticiController(IUserService userService, IRoleService roleService, IMapper mapper, IWebHostEnvironment hostingEnvironment, INewsService newsService, ISettingService settingService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _roleService = roleService ?? throw new ArgumentNullException(nameof(roleService));
            _webHostEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _newsService = newsService ?? throw new ArgumentNullException(nameof(newsService));
            _settingService = settingService ?? throw new ArgumentException(nameof(settingService));
        }

        #endregion

        public IActionResult GirisYap(string message)
        {
            if (message == null)    
            {
                ViewBag.LTD = Request.Cookies["LastLoggedInTime"];
                return View();
            }

            ViewBag.LTD = Request.Cookies["LastLoggedInTime"];
            ViewBag.Message = message;

            return View();
        }

        [HttpGet("[controller]/GirisYap")]
        public IActionResult NavigateLogin(string returnUrl)
        {
            return RedirectToAction("GirisYap", "Yonetici", new { ReturnUrl = returnUrl });
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> GirisYap(LoginViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string passCrypto = new Cyrpto().TryEncrypt(model.Password);
                    bool result = await _userService.Login(model.UserName, passCrypto);

                    if (result == false)
                    {
                        TempData["HataMesaji"] = "Kullanıcı adı veya şifre yanlış. Kullanıcının giriş işlemi başarısız oldu!";
                        return RedirectToAction("GirisYap", "Yonetici");
                    }
                        
                    AccountEditViewModel getUser = _mapper.Map<UserBaseDto, AccountEditViewModel>(_userService.GetUserByName(model.UserName));

                    if (getUser.IsActive != false)
                    {
                        SessionExtensionMethod.SetObject(HttpContext.Session, "user", getUser);
                        HttpContext.Session.GetString("user");
                        Response.Cookies.Append("LastLoggedInTime", DateTime.Now.ToString());

                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, getUser.DisplayName),
                            new Claim("Id", getUser.Id.ToString()),
                            new Claim(ClaimTypes.Role,getUser.RoleId.ToString())
                        };

                        var claims_identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var auth_properties = new AuthenticationProperties
                        {
                            ExpiresUtc = System.DateTimeOffset.UtcNow.AddDays(5)
                        };

                        await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claims_identity), auth_properties);

                        return result ? RedirectToAction("Index", "Home") : RedirectToAction("GirisYap", "Yonetici");
                    }
                    else
                    {
                        TempData["HataMesaji"] = "Kullanıcı aktif değil";
                        return RedirectToAction("GirisYap", "Yonetici");
                    }
                }
                else
                {
                    TempData["HataMesaji"] = "Lütfen kullanıcı adı ve şifrenizi girin!";
                    return RedirectToAction("GirisYap", "Yonetici");
                }
            }
            catch (Exception ex)
            {
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }

        }

        //[RoleAuthorize("Kullanicilar")]
        [CheckRoleAuthorize]
        public IActionResult Kullanicilar()
        {
            try
            {
                return View(_mapper.Map<List<UserListItemDto>, List<UserListViewModel>>(_userService.GetAllUsers()));
            }
            catch (Exception ex)
            {

                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }
        }

        [HttpGet]
        [CheckRoleAuthorize]
        //[RoleAuthorize("KullaniciOlustur")]
        public IActionResult KullaniciOlustur()
        {
            try
            {
                var listRoles = _mapper.Map<List<RoleListItemDto>, List<RolesListViewModel>>(_roleService.GetAllRole());
                ViewBag.Roles = new SelectList(listRoles, "Id", "RoleName");
                return View(new AccountCreateViewModel());
            }
            catch (Exception ex)
            {

                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> KullaniciOlustur(AccountCreateViewModel model, IFormFile file)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.Password == model.RePassword)
                    {
                        if (!await _userService.UserIsExists(model.UserName))
                        {
                            if (file != null)
                            {
                                model.ProfileImage = SaveImageProcess.ImageInsert(file,"Admin");
                            }
                            else
                            {
                                model.ProfileImage = "user.png";
                            }

                            if (model.Password != null || model.Password == "")
                            {
                                if (await _userService.Register(_mapper.Map<AccountCreateViewModel, UserDto>(model)))
                                {
                                    return RedirectToAction(nameof(Kullanicilar));
                                }
                            }

                            else
                            {
                                return View(model);
                            }

                            return View(model);
                        }
                        return View(model);
                    }
                    else
                    {
                        return View(model);
                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {

                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }

        }

        [HttpGet]
        [CheckRoleAuthorize]
        //[RoleAuthorize("KullaniciDuzenle")]
        public IActionResult KullaniciDuzenle(int Id)
        {
            try
            {
                var getUser = _mapper.Map<UserDto, AccountEditViewModel>(_userService.GetUserById(Id));
                var listRoles = _mapper.Map<List<RoleListItemDto>, List<RolesListViewModel>>(_roleService.GetAllRole());
                ViewBag.Roles = new SelectList(listRoles, "Id", "RoleName", getUser.RoleId);
                return View(getUser);
            }
            catch (Exception ex)
            {

                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> KullaniciDuzenle(AccountEditViewModel model, IFormFile file)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (model.Password != "" || model.Password != null)
                    {
                        if (model.Password == model.RePassword)
                        {
                            if (file != null)
                            {
                                model.ProfileImage = SaveImageProcess.ImageInsert(file,"Admin");             
                            }
                            else
                            {
                                model.ProfileImage = "user.png";
                            }

                            if (await _userService.UpdateUser(_mapper.Map<AccountEditViewModel, UserDto>(model)))
                            {
                                return RedirectToAction(nameof(Kullanicilar));
                            }

                            return View(model);
                        }

                        else
                        {
                            return View(model);
                        }
                    }
                    else
                    {
                        if (file != null)
                        {
                            //model.ProfileImage = SaveImageProcess.ImageInsert(file,"Admin"); <- bunu sunucuya atınca kullanacağız
                            var path = Path.Combine(this._webHostEnvironment.WebRootPath, "Files", file.FileName);
                            var stream = new FileStream(path, FileMode.Create);
                            await file.CopyToAsync(stream);
                            model.ProfileImage = file.FileName;
                        }

                        if (await _userService.UpdateUser(_mapper.Map<AccountEditViewModel, UserDto>(model)))
                        {
                            return RedirectToAction(nameof(Kullanicilar));
                        }

                        return View(model);
                    }

                }
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }

        }

        [CheckRoleAuthorize]
        //[RoleAuthorize("KullaniciSil")]
        public IActionResult KullaniciSil(int id)
        {
            try
            {
                AccountEditViewModel yoneticiGetir = SessionExtensionMethod.GetObject<AccountEditViewModel>(HttpContext.Session, "user");

                if (yoneticiGetir.Id != id)
                {
                    if (!_userService.DeleteUserById(id))
                    {
                        return RedirectToAction(nameof(Kullanicilar));
                    }
                    else
                    {
                        return RedirectToAction(nameof(Kullanicilar));
                    }
                }
                else
                {
                    TempData["mesaj"] = "Bu kullanıcı oturum açmış durumda kullanıcıyı sistemden kaldıramazsınız";
                    return RedirectToAction(nameof(Kullanicilar));
                }

            }
            catch (Exception ex)
            {

                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }

        }

        //[RoleAuthorize("DurumDuzenleKullanici")]
        [CheckRoleAuthorize]
        public async Task<IActionResult> DurumDuzenle(int id)
        {
            try
            {
                AccountEditViewModel yoneticiGetir = SessionExtensionMethod.GetObject<AccountEditViewModel>(HttpContext.Session, "user");

                if (yoneticiGetir.Id != id)
                {
                    if (await _userService.EditIsActive(id))
                    {
                        return RedirectToAction(nameof(Kullanicilar));
                    }
                    else
                    {
                        return RedirectToAction(nameof(Kullanicilar));
                    }
                }
                else
                {
                    TempData["mesaj"] = "Bu kullanıcı oturum açmış durumda kullanıcıyı pasifleştiremezsiniz";
                    return RedirectToAction(nameof(Kullanicilar));
                }
            }
            catch (Exception ex)
            {

                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }

        }

        //[RoleAuthorize("HesapDetay")]
        [CheckRoleAuthorize]
        public IActionResult HesapDetay(int id)
        {
            try
            {
                var user = _mapper.Map<UserDto, AccountEditViewModel>(_userService.GetUserById(id));

                var newsByUserId = _mapper.Map<List<NewsListItemDto>, List<NewsLıstItemModel>>(_newsService.newsListByUserId(id));
                ViewBag.News = newsByUserId;             
                return View(user);
            }
            catch (Exception ex)
            {
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }
        }

        public IActionResult CikisYap()
        {
            HttpContext.Session.Remove("user");
            HttpContext.Session.Clear();

            return RedirectToAction("GirisYap", "Yonetici");
        }

        #region Helpers

        private string GetModelStateErrorsHtmlString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var modelError in ModelState.SelectMany(keyValuePair => keyValuePair.Value.Errors))
            {
                sb.Append($"<p>{modelError.ErrorMessage}</p>");
            }

            return sb.ToString();
        }

        #endregion
    }
}
