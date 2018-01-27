using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SpaProject.Data;
using Microsoft.AspNetCore.Identity;
using SpaProject.Data.Items;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SpaProject
{
    public class Startup
    {
		private readonly IConfiguration _config;
		private readonly IHostingEnvironment _env;

		public Startup(IConfiguration config, IHostingEnvironment env)
		{
			_config = config;
			_env = env;
		}

		public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
			services.AddIdentity<StoreUser, IdentityRole>(
				cfg => { cfg.User.RequireUniqueEmail = true; }).
				AddEntityFrameworkStores<SpaContext>();

			services.AddAuthentication().AddCookie().AddJwtBearer(cfg =>
			{
				cfg.TokenValidationParameters = new TokenValidationParameters()
				{
					ValidIssuer = _config["Tokens:Issuer"],
					ValidAudience = _config["Tokens:Audience"],
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]))
				};
			});


			services.AddMvc();

			services.AddDbContext<SpaContext>(cfg => {
				cfg.UseSqlServer(_config.GetConnectionString("SpaConnectionString"));
			});

			services.AddAutoMapper(); //needed the automapper dependency injection package.

			services.AddTransient<SpaSeeder>();
			services.AddScoped<ISpaRepository, SpaRepository>();
		}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });

			using (var scope = app.ApplicationServices.CreateScope())
			{
				var seeder = scope.ServiceProvider.GetService<SpaSeeder>();
				seeder.Seed().Wait(); //seeder is async .Wait waits for it to finish without making 
										// the whole method async.
			}
		}
    }
}
