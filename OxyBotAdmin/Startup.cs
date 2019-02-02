using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.SpaServices.Webpack;
using OxyBotAdmin.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using Microsoft.Extensions.Options;
using System.IO;

namespace OxyBotAdmin
{
    public class Startup
    {
        private readonly bool IsDevelopment;

        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            IsDevelopment = hostingEnvironment.IsDevelopment();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {        
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = IsDevelopment ? false : true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = AppData.AuthOptions.ISSUER,
                        ValidateAudience = true,
                        ValidAudience = AppData.AuthOptions.AUDIENCE,
                        ValidateLifetime = true,
                        IssuerSigningKey = AppData.AuthOptions.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true
                    };

                });

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                        factory.Create(typeof(AppData.SharedResource));
                    
                });

            services.AddSingleton<ILogger, NLogLogger>();

            services.AddTransient<IGetConnectionString, GetConnectionString>();
            services.AddTransient<ICheckUser, CheckUser>();
            services.AddTransient<IWorkWithHash, WorkWithHash>();
            services.AddSingleton<IDBController, DBController>();
            services.AddTransient<ITelegramBot, TelegramBot>();
            services.AddTransient<BaseService>();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultres = new[] 
                {
                    new CultureInfo("ru"),
                    new CultureInfo("en") 
                };
                options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("ru");
                options.SupportedCultures = supportedCultres;
                options.SupportedUICultures = supportedCultres;
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (IsDevelopment)
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            var localizationOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(localizationOptions.Value);

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            //app.UseStaticFiles(new StaticFileOptions()
            //{
            //    FileProvider = new PhysicalFileResult(Path.Combine(Directory.GetCurrentDirectory(), "ClientApp"))


            //});

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "default", template: "{controller=Login}/{action=Auth}/{id?}");
                routes.MapRoute("default_api", "api/{controller}/{action}/{id?}");
                routes.MapRoute("api", "api/{controller}/{id?}");
            });

            app.Run(async (context) =>
            {
                context.Response.ContentType = "text/html";
                await context.Response.SendFileAsync(Path.Combine(env.WebRootPath, "index.html"));
            });

        }
    }
}
