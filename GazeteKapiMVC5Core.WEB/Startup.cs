using AutoMapper;
using GazeteKapiMVC5Core.WEB.CoreInjection;
using GazeteKapiMVC5Core.WEB.Models.ConfigreCaptcha;
using GazeteKapiMVC5Core.WEB.Profiles.WEB;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

namespace GazeteKapiMVC5Core.WEB
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
        [Obsolete]
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages().AddRazorPagesOptions(options => { options.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute()); });
            services.AddHealthChecks()
                        .AddCheck("ping1", new PingHealthCheck("www.google.com", 100))
                        .AddCheck("ping2", new PingHealthCheck("www.bing.com", 100, 30));
            services.AddDbContextDI(_configuration, Environment);
            services.AddInjections();
            services.AddControllersWithViews().SetCompatibilityVersion(CompatibilityVersion.Latest);
            services.AddDistributedMemoryCache();
            services.Configure<reCaptchaSettings>(_configuration.GetSection("Google"));
          
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new NewsProfileWeb());
                mc.AddProfile(new PolicyProfileWeb());
                mc.AddProfile(new BannerProfileWeb());
            });

            IMapper mapper = mappingConfig.CreateMapper();

            services.AddSingleton(mapper);

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.IsEssential = true;
                options.Cookie.HttpOnly = true;
            });

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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseStatusCodePagesWithReExecute("/anasayfa/hata/{0}");
            app.UseHttpsRedirection();
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseCookiePolicy();
            app.UseSession();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseHealthChecks("/hc");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                name: "news",
                pattern: "/news/{Id}/{Title}", new { controller = "anasayfa", action = "haber" }
            );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=anasayfa}/{action=sayfa}/{id?}");
            });
        }
    }
}
