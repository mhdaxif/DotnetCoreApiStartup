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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Db;
using WebApi.Services;

namespace WebApi
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
            // MSSQL server
            services.AddDbContext<AppDbContext>(options =>
               options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]));

            // Register services
            services.AddTransient<IUserService, UserService>();
            services.AddControllersWithViews()
                     .AddNewtonsoftJson(options =>
                     options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddScoped<GreetFilter>();
            services.AddScoped<SampleActionFilter>();

            var Issuer = Configuration["Jwt:Issuer"];
            var Audience = Configuration["Jwt:Audience"];

            var sigInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]));
            // var creds = new SigningCredentials(sigInKey, SecurityAlgorithms.HmacSha256);


            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = Issuer,
                IssuerSigningKey = sigInKey,
                ValidAudience = Audience,
                RequireExpirationTime = false, // todo: - change
                RequireSignedTokens = true,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
             .AddJwtBearer(opt =>
             {
                 opt.RequireHttpsMetadata = false;
                 opt.SaveToken = true;
                 opt.TokenValidationParameters = tokenValidationParameters;
             });

            services.AddCors(opt =>
            {
                opt.AddPolicy(name: "CORS", policy =>
                {
                    policy.AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowAnyOrigin();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseCors("CORS");
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
