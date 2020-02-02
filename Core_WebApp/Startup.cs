using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core_WebApp.Models;
using Core_WebApp.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Core_WebApp.CustomFilters;

namespace Core_WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        /// <summary>
        /// Provides Dependency Container for 
        /// 1. MVC Services
        ///     Filters
        /// 2. Controller Services
        ///     Razor View
        ///     WEB APIs
        /// 3. Custom Repository Services by developers.
        /// 4. Identity Services for Authentication and Authorization
        ///     User Based Auth
        ///     Role Based Autho
        ///     JWT Based Autho
        /// 5. CORS 
        /// 6. Authorization Policies
        ///     Role Based Policies
        /// 7. Sessions
        /// 8. Coockies Configuration
        /// 9. The DbContext for EF Core
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            // MVC Controllers +view and API Controllers
            // register the Custom Exception Filter
            services.AddControllersWithViews(options => {
                options.Filters.Add(typeof(MyExceptionFilterAttribute));
                options.Filters.Add(typeof(LogActionFilter));
            });
            // register the DbContext class in DI
            services.AddDbContext<CoreAppDbContext>((options) =>
            {
                // use Sql Server defined using the Connection string
                options.UseSqlServer(Configuration.GetConnectionString("AppDbConnection"));
            });

            // register repository classes in DI Container
            services.AddScoped<IRepository<Category,int>, CategoryRepository>();
            services.AddScoped<IRepository<Product, int>, ProductRepository>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// Starts HTTP Request Pipeline
        /// IApplicationBuilder -> Interface that defines 'Middlewares' to execute request
        /// IWebHostEnvironment -> Interface that check the execution environment
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // standard dev error page
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // error view redirect
                app.UseExceptionHandler("/Home/Error");
            }
            // use .js/.css/.img files from wwwroot folder
            app.UseStaticFiles();
            // applicartion routing , default for MVC COntroller and then API Comntrollers
            app.UseRouting();
            // check for security
            app.UseAuthorization();
            // server endpoints to accept request and start routing ASP.NET Core 3.0+
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
