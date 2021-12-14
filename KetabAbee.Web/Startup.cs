using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;

namespace KetabAbee.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region MVC

            services.AddControllersWithViews();
            services.AddMvc();

            #endregion

            #region Razor

            services.AddRazorPages().AddRazorRuntimeCompilation();

            #endregion

            #region Max length Upload

            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 30000000;
            });

            #endregion

            #region Authentication

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                options.LoginPath = "/Login";
                options.LogoutPath = "/Logout";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(43200);
            });

            #endregion

            #region Context

            //services.AddDbContext<ShooraDahakContext>(option =>
            //{
            //    option.UseSqlServer(Configuration.GetConnectionString("KetabAbeeConnection"));
            //});

            #endregion

            #region IoC

            //services.AddScoped<IUserService, UserService>();

            //services.AddScoped<IViewRenderService, RenderViewToString>();

            //services.AddScoped<IPermissionService, PermissionService>();

            //services.AddScoped<IServiceService, ServiceService>(); 

            //services.AddScoped<IDiscussionService, DiscussionService>();

            //services.AddScoped<ILetterService, LetterService>();

            //services.AddScoped<IQuoteService, QuoteService>();

            //services.AddScoped<IBlogService, BlogService>();

            //services.AddScoped<ICommentService, CommentService>();

            //services.AddScoped<IReportAbuseService, ReportAbuseService>();


            #endregion
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
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
