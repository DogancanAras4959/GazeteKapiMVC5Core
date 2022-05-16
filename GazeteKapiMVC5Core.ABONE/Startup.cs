using AutoMapper;
using GazeteKapiMVC5Core.ABONE.Core.Extensions;
using GazeteKapiMVC5Core.ABONE.Profiles;
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

namespace GazeteKapiMVC5Core.ABONE
{
    public class Startup
    {
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

        private IConfiguration _configuration;
        private IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            try
            {
                services.AddDbContextDI(_configuration, Environment);
                services.AddControllersWithViews().SetCompatibilityVersion(CompatibilityVersion.Latest);
                services.AddDistributedMemoryCache();
                services.AddSession();
                           
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new UserProfile());
                });

                IMapper mapper = mappingConfig.CreateMapper();

                services.AddSingleton(mapper);

                services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
                {
                    options.Cookie.HttpOnly = true;
                    //options.AccessDeniedPath = new PathString("/Yonetici/GirisYap/");
                    //options.LoginPath = new PathString("/Yonetici/GirisYap/");
                    options.SlidingExpiration = true;
                });
            }
            catch (Exception)
            {

                throw;
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/hata/{0}");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseStatusCodePagesWithReExecute("/Home/hata/{0}");
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
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
