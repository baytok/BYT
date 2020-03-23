using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BYT;
using BYT.WS.Data;
using BYT.WS.Entities;
using BYT.WS.Helpers;
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
       
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

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

            // appsettings.json i�inde olu�turdu�umuz gizli anahtar�m�z� AppSettings ile �a��raca��m�z� s�yl�yoruz.

            var appSettingsSection = Configuration.GetSection("AppSettings");

            services.Configure<AppSettings>(appSettingsSection);

            // Olu�turdu�umuz gizli anahtar�m�z� byte dizisi olarak al�yoruz.

            var appSettings = appSettingsSection.Get<AppSettings>();

            var key = Encoding.ASCII.GetBytes(appSettings.TokenSecretKey);

            //Projede farkl� authentication tipleri olabilece�i i�in varsay�lan olarak JWT ile kontrol edece�imizin bilgisini kaydediyoruz.
           
            services.AddAuthentication(x =>
            {
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })             
                .AddJwtBearer(x =>
                {
                    //Gelen isteklerin sadece HTTPS yani SSL sertifikas� olanlar� kabul etmesi(varsay�lan true)

                    x.RequireHttpsMetadata = false;

                    //E�er token onaylanm�� ise sunucu taraf�nda kay�t edilir.

                    x.SaveToken = true;

                    //Token i�inde neleri kontrol edece�imizin ayarlar�.

                    x.TokenValidationParameters = new TokenValidationParameters
                    {

                        //Token 3.k�s�m(imza) kontrol�

                        ValidateIssuerSigningKey = true,

                        //Neyle kontrol etmesi gerektigi

                        IssuerSigningKey = new SymmetricSecurityKey(key),

                        //Bu iki ayar ise "aud" ve "iss" claimlerini kontrol edelim mi diye soruyor

                        ValidateIssuer = false,

                        ValidateAudience = false

                    };

                });

            //DI i�in IUserService aray�z�n� �a��rd���m zaman UserService s�n�f�n� getirmesini s�yl�yorum.

            services.AddScoped<IKullaniciServis, KullaniciService>();

            services.AddMvc();
            services.AddCors();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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
