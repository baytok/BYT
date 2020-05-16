using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BYT;
using BYT.WS.Data;
using BYT.WS.Entities;
using BYT.WS.Helpers;
using BYT.WS.Services;
using BYT.WS.Services.Kullanici;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace BYT
{
    public class ServisCredential
    {
        public string username { get; set; }
        public string password { get; set; }
    }

  

    public class Startup
    {
        public Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment { get; set; }
        public Startup(IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
        services.Configure<ServisCredential>(Configuration.GetSection("ServisCredential"));

           
            services.AddDbContext<IslemTarihceDataContext>(x =>
             {
                 x.UseSqlServer(Configuration.GetConnectionString("BYTConnection"));
             });
            services.AddDbContext<BeyannameDataContext>(x =>
            {
                x.UseSqlServer(Configuration.GetConnectionString("BYTConnection"));
            });
            services.AddDbContext<BeyannameSonucDataContext>(x =>
            {
                x.UseSqlServer(Configuration.GetConnectionString("BYTConnection"));
            });


            services.AddDbContext<KullaniciDataContext>();

            // appsettings.json içinde oluþturduðumuz gizli anahtarýmýzý AppSettings ile çaðýracaðýmýzý söylüyoruz.

            var appSettingsSection = Configuration.GetSection("AppSettings");

            services.Configure<AppSettings>(appSettingsSection);

            // Oluþturduðumuz gizli anahtarýmýzý byte dizisi olarak alýyoruz.

            var appSettings = appSettingsSection.Get<AppSettings>();

            var key = Encoding.ASCII.GetBytes(appSettings.TokenSecretKey);

            //Projede farklý authentication tipleri olabileceði için varsayýlan olarak JWT ile kontrol edeceðimizin bilgisini kaydediyoruz.
           
            services.AddAuthentication(x =>
            {
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })             
                .AddJwtBearer(x =>
                {
                    //Gelen isteklerin sadece HTTPS yani SSL sertifikasý olanlarý kabul etmesi(varsayýlan true)

                    x.RequireHttpsMetadata = false;

                    //Eðer token onaylanmýþ ise sunucu tarafýnda kayýt edilir.

                    x.SaveToken = true;

                    //Token içinde neleri kontrol edeceðimizin ayarlarý.

                    x.TokenValidationParameters = new TokenValidationParameters
                    {

                        //Token 3.kýsým(imza) kontrolü

                        ValidateIssuerSigningKey = true,

                        //Neyle kontrol etmesi gerektigi

                        IssuerSigningKey = new SymmetricSecurityKey(key),

                        //Bu iki ayar ise "aud" ve "iss" claimlerini kontrol edelim mi diye soruyor

                        ValidateIssuer = false,

                        ValidateAudience = false

                    };

                });

            //DI için IUserService arayüzünü çaðýrdýðým zaman UserService sýnýfýný getirmesini söylüyorum.

            services.AddScoped<IKullaniciServis, KullaniciService>();

            services.AddMvc();
            services.AddCors();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            loggerFactory.AddProvider(new LoggerProvider(_hostingEnvironment));
            app.UseHttpsRedirection();
            app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            app.UseRouting();
            app.UseAuthorization();
         

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
