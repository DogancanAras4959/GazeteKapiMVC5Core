using AutoMapper;
using GazeteKapiMVC5Core.Core.Extensions;
using GazeteKapiMVC5Core.Profiles;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GazeteKapiMVC5Core
{
    public class Startup
    {
        private IConfiguration _configuration;
        private IWebHostEnvironment Environment { get; }

        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", false, false)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();

            _configuration = builder.Build();
            Environment = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

                services.AddDbContextDI(_configuration, Environment);
                services.AddInjections();
                services.AddControllersWithViews().SetCompatibilityVersion(CompatibilityVersion.Latest);
                services.AddDistributedMemoryCache();
                //services.AddAutoMapper(typeof(Startup));
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new UserProfile());
                    mc.AddProfile(new NewsProfile());
                    mc.AddProfile(new SiteProfile());
                    mc.AddProfile(new SeoProfiles());
                    mc.AddProfile(new MagazineBannerProfile());
                    mc.AddProfile(new BannersProfile());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                services.AddSingleton(mapper);
                services.AddSession();
                services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
                {
                    options.Cookie.HttpOnly = true;
                    options.AccessDeniedPath = new PathString("/Yonetici/GirisYap/");
                    options.LoginPath = new PathString("/Yonetici/GirisYap/");
                    options.SlidingExpiration = true;
                });
                      
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            try
            {
                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }
                else
                {
                    //app.UseExceptionHandler("/Home/hata/{0}");
                    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                    app.UseHsts();
                }
                //app.UseStatusCodePagesWithReExecute("/Home/hata/{0}");
                app.UseHttpsRedirection();
                app.UseStaticFiles();
                app.UseSession();
                app.UseRouting();
                app.UseAuthentication();
                app.UseAuthorization();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Yonetici}/{action=GirisYap}/{id?}");
                });
            }
            catch (Exception ex)
            {

                throw new Exception(ex.ToString());
            }
          
        }
    }
}
