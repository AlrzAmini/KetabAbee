using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using GoogleReCaptcha.V3;
using GoogleReCaptcha.V3.Interface;
using KetabAbee.Data.Context;
using KetabAbee.IoC;
using KetabAbee.Web.PresentationExtensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;

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


            services.AddDbContext<KetabAbeeDBContext>(option =>
            {
                option.UseSqlServer(Configuration.GetConnectionString("KetabAbeeDBConnection"));

            }, ServiceLifetime.Transient);

            #endregion

            #region IoC

            RegisterServices(services);

            #endregion

            #region ReCaptcha

            services.AddHttpClient<ICaptchaValidator, GoogleReCaptchaValidator>();

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseIpFilter();

            #region 404 control

            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404)
                {
                    context.Response.Redirect("/404Error");
                }
            });

            #endregion

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

            #region Cach Static Files

            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse =
                    r =>
                    {
                        string path = r.File.PhysicalPath;
                        if (path.EndsWith(".css") || path.EndsWith(".js") || path.EndsWith(".gif") || path.EndsWith(".jpg")
                            || path.EndsWith(".png") || path.EndsWith(".svg") || path.EndsWith(".woff2") || path.EndsWith(".woff") || path.EndsWith(".ico") || path.EndsWith(".ttf") || path.EndsWith(".webp"))
                        {
                            TimeSpan maxAge = new(12, 0, 0, 0);
                            r.Context.Response.Headers.Append("Cache-Control", "max-age=" + maxAge.TotalSeconds.ToString("0"));
                        }
                    }
            });
            app.Use(async (context, next) =>
            {
                string path = context.Request.Path;

                if (path.EndsWith(".css") || path.EndsWith(".js"))
                {

                    //Set css and js files to be cached for 12 days
                    TimeSpan maxAge = new(12, 0, 0, 0);     
                    context.Response.Headers.Append("Cache-Control", "max-age=" + maxAge.TotalSeconds.ToString("0"));

                }
                else if (path.EndsWith(".gif") || path.EndsWith(".jpg") || path.EndsWith(".png"))
                {
                    //custom headers for images goes here if needed

                }
                else
                {
                    //Request for views fall here.
                    context.Response.Headers.Append("Cache-Control", "no-cache");
                    context.Response.Headers.Append("Cache-Control", "private, no-store");

                }
                await next();
            });

            #endregion

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }

        public static void RegisterServices(IServiceCollection services)
        {
            DependencyContainer.RegisterServices(services);
        }
    }
}
