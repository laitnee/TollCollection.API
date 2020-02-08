using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using newNet.Data;
using newNet.Data.Repository;
using newNet.Helpers;
using newNet.Services;

namespace newNet {
    public class Startup {
        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {

            var key = Encoding.ASCII.GetBytes (Configuration.GetSection ("AppSettings:Token").Value);
            services.Configure<MQTTSettings> (Configuration.GetSection ("MQTTSettings"));
            services.Configure<TwilioSettings> (Configuration.GetSection ("TwilioSettings"));
            services.AddDbContext<DataContext>(c => c.UseSqlServer(Configuration.GetConnectionString("TollCollectionDb")));

            services.AddAutoMapper(typeof(Startup));

            services.AddSingleton<MQTTMessagingService> ();
            services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, MQTTMessagingService> (
            serviceProvider => serviceProvider.GetService<MQTTMessagingService> ());
            services.AddSingleton<ISMSMessagingService, SMSMessagingService>();
            services.AddScoped<IAuthenticationRepository, AuthenticationRepository> ();
            services.AddScoped<ITollCollectionRepository, TollCollectionRepository>();
            services.AddScoped<IProcessTransaction, ProcessTransaction>();
            
            services.AddCors ();
            services.AddAuthentication (JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer (options => {
                    options.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey (key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                    };
                });
            services.AddMvc ().SetCompatibilityVersion (CompatibilityVersion.Version_2_2);

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles (configuration => {
                configuration.RootPath = "../TOLLCOLLECTION.SPA/TollCollection/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            } else {
                app.UseExceptionHandler ("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts ();
            }

            app.UseHttpsRedirection ();
            app.UseCors (x => x.AllowAnyHeader ().AllowAnyMethod ().AllowAnyOrigin ().AllowCredentials ());            

            app.UseStaticFiles ();
            app.UseSpaStaticFiles ();
            app.UseAuthentication ();

            app.UseMvc (routes => {
                routes.MapRoute (
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa (spa => {

                spa.Options.SourcePath = "../TOLLCOLLECTION.SPA/TollCollection";

                if (env.IsDevelopment ()) {
                    spa.UseAngularCliServer (npmScript: "start");
                }
            });
        }
    }
}