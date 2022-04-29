using CORE.ApplicationCore.BackEndExceptionHandler;
using CORE.ApplicationCore.UnitOfWork;
using GazeteKapiMVC5Core.DataAccessLayer;
using GazeteKapiMVC5Core.DataAccessLayerLOG;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SERVICE.Engine.Engines;
using SERVICE.Engine.Interfaces;
using SERVICES.Engine.Engines;
using SERVICES.Engine.Interfaces;

namespace GazeteKapiMVC5Core.Core.Extensions
{
    internal static class RegisterExtensions
    {
        internal static void AddDbContextDI(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            var contextConnectionString = configuration.GetConnectionString("Default");
            services.AddDbContextPool<NewsAppContext>(x => x.UseSqlServer(contextConnectionString, o =>
            {
                o.EnableRetryOnFailure(3);
            })
                .EnableSensitiveDataLogging(environment.IsDevelopment())
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
        }

        internal static void AddDbContextLog(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            var contextConnectionString = configuration.GetConnectionString("Log");
            services.AddDbContextPool<NewsAppContextLog>(x => x.UseSqlServer(contextConnectionString, o =>
            {
                o.EnableRetryOnFailure(3);
            })
               .EnableSensitiveDataLogging(environment.IsDevelopment())
               .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
        }

        internal static void AddInjections(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IMyCache, MyMemoryCache>();
            services.AddTransient(typeof(IBackEndExceptionHandler), typeof(BackEndExceptionHandler));
            services.AddTransient(typeof(IUserService), typeof(UserService));
            services.AddTransient(typeof(IRoleService), typeof(RoleService));
            services.AddTransient(typeof(ICategoryService), typeof(CategoryService));
            services.AddTransient(typeof(ILogService), typeof(LogService));
            services.AddTransient(typeof(INewsService), typeof(NewsService));
            services.AddTransient(typeof(ISettingService), typeof(SettingService));
            services.AddTransient(typeof(ICountService), typeof(CountService));
            //services.AddTransient(typeof(IPagedList), typeof(PagedList));
            services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
        }
    }
}
