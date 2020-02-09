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
using Core_WebApp.Data;
using Microsoft.AspNetCore.Identity;

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

            // add services for Security Db Connection and identity
            services.AddDbContext<SecurityContextEV>(options =>
                    options.UseSqlServer(
                        Configuration.GetConnectionString("SecurityContextEVConnection")));
            // defuault is onle User Based Authentication
            // AddDefaultIdentity() method will resolve dependencies for
            // 1. UserManager<IdentityManager> --> Used for Creating Users
            // 2. SignInManager<IdentityManager> --> Manages User Based Authentication
            //services.AddDefaultIdentity<IdentityUser>(/*options => options.SignIn.RequireConfirmedAccount = true*/)
            //    .AddEntityFrameworkStores<SecurityContextEV>();


            // AddIdentity() method will resolve dependencies for
            // 1. UserManager<IdentityManager> --> Used for Creating Users
            // 2. SignInManager<IdentityManager> --> Manages User Based Authentication
            // 3. RoleManager<IdentityRole> --> Manages all Roles
            services.AddIdentity<IdentityUser,IdentityRole>()
                .AddDefaultUI()
              .AddEntityFrameworkStores<SecurityContextEV>();
            // ends here

            // adding the Authorization Service
            services.AddAuthorization(options=> {
                options.AddPolicy("ReadPolicy", policy =>
                {
                    policy.RequireRole("Admin", "Manager", "Clerk");
                });

                options.AddPolicy("WritePolicy", policy =>
                {
                    policy.RequireRole("Admin", "Manager");
                });
            });
            // ends here


            // register repository classes in DI Container
            services.AddScoped<IRepository<Category,int>, CategoryRepository>();
            services.AddScoped<IRepository<Product, int>, ProductRepository>();
            // the MVC Request Pipeline for Authenticating
            // controllers/actions using [Authorize] filter
            services.AddMvc();

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
            // applicartion routing , default for MVC COntroller and then API Comntrollers
            app.UseRouting();
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
            app.UseAuthentication(); // for authenticating the HTTP request

            // check for security
            app.UseAuthorization();
            // server endpoints to accept request and start routing ASP.NET Core 3.0+
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages(); // for Authentication WebForms (Razor Pages)
            });
        }
    }
}
