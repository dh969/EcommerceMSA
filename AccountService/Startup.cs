using AccountService.Entities;
using AccountService.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountService
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
            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "IdentityServer.Cookie";
                config.LoginPath = "/Account/Login";
            });
            services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<IndentityDbContext>().AddDefaultTokenProviders().AddSignInManager<SignInManager<AppUser>>();
            services.AddIdentityServer().AddInMemoryApiResources(Config.GetApis()).AddAspNetIdentity<AppUser>().
                AddInMemoryClients(Config.GetClients()).AddInMemoryApiScopes(Config.Scopes).AddDeveloperSigningCredential().AddProfileService<IdentityProfileService>();
       
            services.AddControllersWithViews();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AccountService", Version = "v1" });
            });
            services.AddDbContext<IndentityDbContext>(x => { x.UseSqlServer(Configuration.GetConnectionString("IdentityConnection")); });
          
            //services.AddAuthentication(option =>
            //{

            //    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //    option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            //}).AddJwtBearer(option =>
            //{
            //    option.SaveToken = true;
            //    option.RequireHttpsMetadata = false;
            //    option.TokenValidationParameters = new TokenValidationParameters()
            //    {
            //        ValidateIssuer = true,
            //        ValidateAudience = true,
            //        ValidAudience = Configuration["JWT:ValidAudience"],
            //        ValidIssuer = Configuration["JWT:ValidIssuer"],
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
            //    };
            //})
            //    ;
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("AdminOnly", policy =>
            //    policy.RequireRole("admin"));
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AccountService v1"));
            }

           // app.UseHttpsRedirection();
            app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            app.UseStaticFiles();
            app.UseRouting();
            app.UseIdentityServer();
            app.UseCookiePolicy(new CookiePolicyOptions()
            {
                MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.Lax
            });
            //app.UseAuthentication();
            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
