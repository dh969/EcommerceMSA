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
using Microsoft.OpenApi.Models;
using ProductsService.Data;
using ProductsService.Entity;
using ProductsService.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsService
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ProductsService", Version = "v1" });
            });
            services.AddDbContext<StoreContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddAuthentication("Bearer").AddJwtBearer("Bearer", config => {

                config.Authority = "http://localhost:5002/";
                config.Audience = "ApiOne";
                config.RequireHttpsMetadata = false;
                
            });
            services.AddHttpClient();
            services.AddAuthorization();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<IProductRepository, ProductRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProductsService v1"));
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            using var scope = app.ApplicationServices.CreateScope();//in .net 6 or above use just Services
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<StoreContext>();
            //var identityContext = services.GetRequiredService<IndentityDbContext>();
            //var userManager = services.GetRequiredService<UserManager<AppUser>>();
            //var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var logger = services.GetRequiredService<ILogger<Program>>();
            try
            {
                await context.Database.MigrateAsync();
               // await identityContext.Database.MigrateAsync();
                await StoreContextSeed.SeedAsync(context);
                //await IdentitySeed.SeedUsersAsync(userManager, roleManager);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "an error has occured");
            }
        }
    }
}
