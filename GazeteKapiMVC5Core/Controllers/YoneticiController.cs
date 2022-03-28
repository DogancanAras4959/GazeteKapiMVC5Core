using AutoMapper;
using CORE.ApplicationCommon.DTOS.AccountDTO;
using CORE.ApplicationCommon.DTOS.RoleDTO;
using CORE.ApplicationCommon.Helpers.Cyrptography;
using GazeteKapiMVC5Core.Core.Extensions;
using GazeteKapiMVC5Core.Models.Account;
using GazeteKapiMVC5Core.Models.Role;
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
        private readonly ILogService _logService;

        public YoneticiController(IUserService userService, IRoleService roleService, IMapper mapper, IWebHostEnvironment hostingEnvironment, ILogService logService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _roleService = roleService ?? throw new ArgumentNullException(nameof(roleService));
            _webHostEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
        }

        #endregion
        public IActionResult GirisYap(string message)
        {
            if (message == null)
            {
                return View();
            }

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
        public async Task<IActionResult> GirisYap(LoginViewModel model, string returnUrl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string passCrypto = new Cyrpto().TryEncrypt(model.Password);
                    bool result = await _userService.Login(model.UserName, passCrypto);

                    if (result && !string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        await CreateModeratorLog("Başarısız", "Giriş İşlemi", "GirisYap", "Yonetici", "Kullanıcı adı veya şifre yanlış. Kullanıcının giriş işlemi başarısız oldu!");
                        return Redirect(returnUrl);
                    }

                    AccountEditViewModel getUser = _mapper.Map<UserBaseDto, AccountEditViewModel>(_userService.GetUserByName(model.UserName));

                    if (getUser.IsActive != false)
                    {
                        SessionExtensionMethod.SetObject(HttpContext.Session, "user", getUser);
                        HttpContext.Session.GetString("user");

                        await CreateModeratorLog("Başarılı", "Giriş İşlemi", "GirisYap", "Yonetici", "Başarılı bir giriş işlemi gerçekleştirildi");
                        return result ? RedirectToAction("Index", "Home") : RedirectToAction("GirisYap", "Yonetici", new { message = "<p> Kullanıcı adı veya şifre yanlış. Lütfen tekrar deneyin! </p>" });
                    }
                    else
                    {
                        await CreateModeratorLog("Başarısız", "Giriş İşlemi", "GirisYap", "Yonetici", "Kullanıcının giriş işlemi başarısız oldu. Kullanıcı akti değil!");
                        return RedirectToAction("GirisYap", "Yonetici", new { message = GetModelStateErrorsHtmlString() });
                    }
                }
                return RedirectToAction("GirisYap", "Yonetici", new { message = GetModelStateErrorsHtmlString() });
            }
            catch (Exception ex)
            {
                string detay = "Sistemden kaynaklı bir hata meydana geldi: " + ex.ToString();
                await CreateModeratorLog("Sistem Hatası", "Giriş İşlemi", "GirisYap", "Yonetici", detay);
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }

        }

        //[RoleAuthorize("Kullanicilar")]
        public async Task<IActionResult> Kullanicilar()
        {
            try
            {
                await CreateModeratorLog("Başarılı", "Sayfa Girişi", "Kullanicilar", "Yonetici", "Kullanıcının yönetici sayfasına girişi başarılı!");
                return View(_mapper.Map<List<UserListItemDto>, List<UserListViewModel>>(_userService.GetAllUsers()));
            }
            catch (Exception ex)
            {
                string detay = "Sistemden kaynaklı bir hata meydana geldi: " + ex.ToString();
                await CreateModeratorLog("Sistem Hatası", "Sayfa Girişi", "Kullanicilar", "Yonetici", detay);
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }
        }

        [HttpGet]
        //[RoleAuthorize("KullaniciOlustur")]
        public async Task<IActionResult> KullaniciOlustur()
        {
            try
            {
                var listRoles = _mapper.Map<List<RoleListItemDto>, List<RolesListViewModel>>(_roleService.GetAllRole());
                ViewBag.Roles = new SelectList(listRoles, "Id", "RoleName");
                await CreateModeratorLog("Başarılı", "Sayfa Girişi", "KullaniciOlustur", "Yonetici","Kullanıcı ekleme sayfasına giriş başarılı oldu!");
                return View(new AccountCreateViewModel());
            }
            catch (Exception ex)
            {
                string detay = "Sistemden kaynaklı bir hata meydana geldi: " + ex.ToString();
                await CreateModeratorLog("Sistem Hatası", "Sayfa Girişi", "KullaniciOlustur", "Yonetici", detay);
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
                                //model.ProfileImage = SaveImageProcess.ImageInsert(file,"Admin"); <- bunu sunucuya atınca kullanacağız
                                string uploadfilename = Path.GetFileNameWithoutExtension(file.FileName);
                                string extension = Path.GetExtension(file.FileName);
                                uploadfilename = uploadfilename + DateTime.Now.ToString("yymmssfff") + extension;
                                var path = Path.Combine(this._webHostEnvironment.WebRootPath, "Files", uploadfilename);
                                var stream = new FileStream(path, FileMode.Create);
                                await file.CopyToAsync(stream);
                                model.ProfileImage = uploadfilename;
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
                                await CreateModeratorLog("Başarısız", "Ekleme", "KullaniciOlustur", "Yonetici", "Kullanıcının oluşturulması için şifre bilgisi gerekmektedir.");
                                return View(model);
                            }

                            await CreateModeratorLog("Başarısız", "Ekleme", "KullaniciOlustur", "Yonetici", "Oluşurulmaya çalışılan yönetici sistemde bulunuyor");
                            return View(model);
                        }
                        return View(model);
                    }
                    else
                    {
                        await CreateModeratorLog("Başarısız", "Ekleme", "KullaniciOlustur", "Yonetici", "Şifreler birbiriyle uyuşmuyor. Bu yüzden kullanıcı oluşturulamadı!");
                        return View(model);
                    }
                }
                return View(model);
            }
            catch (Exception ex) 
            {
                string detay = "Sistemden kaynaklı bir hata meydana geldi: " + ex.ToString();
                await CreateModeratorLog("Sistem Hatası", "Ekleme", "KullaniciOlustur", "Yonetici", detay);
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }

        }

        [HttpGet]
        //[RoleAuthorize("KullaniciDuzenle")]
        public async Task<IActionResult> KullaniciDuzenle(int Id)
        {
            try
            {
                var getUser = _mapper.Map<UserDto, AccountEditViewModel>(_userService.GetUserById(Id));
                var listRoles = _mapper.Map<List<RoleListItemDto>, List<RolesListViewModel>>(_roleService.GetAllRole());
                ViewBag.Roles = new SelectList(listRoles, "Id", "RoleName", getUser.RoleId);
                await CreateModeratorLog("Başarılı","Sayfa Girişi","KullaniciDuzenle","Yonetici","Kullanıcı Düzenleme sayfasına giriş başarılı!");
                return View(getUser);
            }
            catch (Exception ex)
            {
                string detay = "Sistemden kaynaklı bir hata meydana geldi: " + ex.ToString();
                await CreateModeratorLog("Sistem Hatası", "Sayfa Girişi", "KullaniciDuzenle", "Yonetici", detay);
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
                                //model.ProfileImage = SaveImageProcess.ImageInsert(file,"Admin"); <- bunu sunucuya atınca kullanacağız
                                string uploadfilename = Path.GetFileNameWithoutExtension(file.FileName);
                                string extension = Path.GetExtension(file.FileName);
                                uploadfilename = uploadfilename + DateTime.Now.ToString("yymmssfff") + extension;
                                var path = Path.Combine(this._webHostEnvironment.WebRootPath, "Files", uploadfilename);
                                var stream = new FileStream(path, FileMode.Create);
                                await file.CopyToAsync(stream);
                                model.ProfileImage = uploadfilename;
                            }
                            else
                            {
                                model.ProfileImage = "user.png";
                            }

                            if (await _userService.UpdateUser(_mapper.Map<AccountEditViewModel, UserDto>(model)))
                            {
                                await CreateModeratorLog("Başarılı", "Güncelleme", "KullaniciDuzenle", "Yonetici", "Kullanıcı başarıyla güncellendi!");
                                return RedirectToAction(nameof(Kullanicilar));
                            }
                           
                            return View(model);
                        }

                        else
                        {
                            await CreateModeratorLog("Başarısız", "Güncelleme", "KullaniciDuzenle", "Yonetici", "Kullanıcı güncellenemedi. Şifreler birbiriyle uyuşmuyor!");
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
                            await CreateModeratorLog("Başarılı", "Güncelleme", "KullaniciDuzenle", "Yonetici", "Kullanıcı başarıyla güncellendi!");
                            return RedirectToAction(nameof(Kullanicilar));
                        }

                        return View(model);
                    }

                }
                return View(model);
            }
            catch (Exception ex)
            {
                string detay = "Sistemden kaynaklı bir hata meydana geldi: " + ex.ToString();
                await CreateModeratorLog("Sistem Hatası", "Güncelleme", "KullaniciDuzenle", "Yonetici", detay);
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }

        }

        //[RoleAuthorize("KullaniciSil")]
        public async Task<IActionResult> KullaniciSil(int id)
        {
            try
            {
                if (!_userService.DeleteUserById(id))
                {
                    await CreateModeratorLog("Başarısız", "Silme", "KullaniciSil", "Yonetici", "Kullanıcı sistemden kaldırılırken bir hata meydana geldi!");
                    return RedirectToAction(nameof(Kullanicilar));
                }
                else
                {
                    await CreateModeratorLog("Başarılı", "Silme", "KullaniciSil", "Yonetici", "Kullanıcı başarıyla sistemden kaldırıldı");
                    return RedirectToAction(nameof(Kullanicilar));
                }
            }
            catch (Exception ex)
            {
                string detay = "Sistemden kaynaklı bir hata meydana geldi: " + ex.ToString();
                await CreateModeratorLog("Sistem Hatası", "Silme", "KullaniciSil", "Yonetici", detay);
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }

        }

        //[RoleAuthorize("DurumDuzenleKullanici")]
        public async Task<IActionResult> DurumDuzenle(int id)
        {
            try
            {
                if (await _userService.EditIsActive(id))
                {
                    await CreateModeratorLog("Başarılı", "Güncelleme", "DurumDuzenle", "Yonetici", "Kullanıcı başarıyla güncellendi. Kullanıcını durumu düzenlendi!");
                    return RedirectToAction(nameof(Kullanicilar));
                }
                else
                {
                    await CreateModeratorLog("Başarısız", "Güncelleme", "DurumDuzenle", "Yonetici", "Kullanıcının durumu güncellenemedi!");
                    return RedirectToAction(nameof(Kullanicilar));
                }
            }
            catch (Exception ex)
            {
                string detay = "Sistemden kaynaklı bir hata meydana geldi: " + ex.ToString();
                await CreateModeratorLog("Sistem Hatası", "Güncelleme", "DurumDuzenle", "Yonetici", detay);
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }

        }

        //[RoleAuthorize("HesapDetay")]
        public async Task<IActionResult> HesapDetay(int id)
        {
            try
            {
                await CreateModeratorLog("Başarılı", "Sayfa Girişi", "HesapDetay", "Yonetici", "Kullanıcı hesabına başarılı bir şekilde erişildi!");
                return View(_mapper.Map<UserDto, AccountEditViewModel>(_userService.GetUserById(id)));
            }
            catch (Exception ex)
            {
                string detay = "Sistemden kaynaklı bir hata meydana geldi: " + ex.ToString();
                await CreateModeratorLog("Sistem Hatası", "Sayfa Girişi", "HesapDetay", "Yonetici", detay);
                TempData["HataMesaji"] = ex.ToString();
                return RedirectToAction("ErrorPage", "Home");
            }
        }

        public IActionResult CikisYap()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("user");
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

        public async Task<CheckLogService> CreateModeratorLog(string durum, string islem, string action, string controller, string details)
        {
            AccountEditViewModel yoneticiGetir = SessionExtensionMethod.GetObject<AccountEditViewModel>(HttpContext.Session, "user"); 
            CheckLogService ct = new CheckLogService(_logService, _mapper);
            if (yoneticiGetir.UserName == "" || yoneticiGetir.UserName == null)
            {
                await ct.CreateLogs(durum, islem, action, controller, details, "Sistem");
                return ct;
            }
            await ct.CreateLogs(durum, islem, action, controller, details, yoneticiGetir.UserName);
            return ct;
        }

        #endregion
    }
}
