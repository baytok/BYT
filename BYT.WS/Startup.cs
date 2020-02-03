using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BYT;
using BYT.WS.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
