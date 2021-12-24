using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Salvo.Models;
using Salvo.Models.Auth;
using Salvo.Repositories;
using Salvo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebPWrecover.Services;

namespace Salvo
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
            services.AddRazorPages();

            // Inyeccion de dependencias para salvo context
            services.AddDbContext<SalvoContext>(options => options
            .UseSqlServer(Configuration.GetConnectionString("SalvoDataBase"),
            o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));

            // Inyectar repositorio de game
            services.AddScoped<IGameRepository, GameRepository>();

            // Inyectar repositorio de game players
            services.AddScoped<IGamePlayerRepository, GamePlayerRepository>();

            // Inyectar repositorio de players
            services.AddScoped<IPlayerRepository, PlayerRepository>();

            // Inyectar repositorio de los scores
            services.AddScoped<IScoreRepository, ScoreRepository>();

            // Inyectar servicio para gestionar el usuario
            services.AddScoped<IUserService, UserService>();

            // Inyectar servicio para enviar emails
            services.AddTransient<IEmailSender, EmailSender>();

            // Inyectar servicio para enviar emails
            services.AddTransient<ITokenService, TokenService>();

            //Aca debemos seguir agregando los scoped (de ser necesario)
            //**********

            //Autenticacion
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                    options.LoginPath = new PathString("/index.html");
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                    };
                });

            //Autorizacion
            services.AddAuthorization(options =>
            {
                options.AddPolicy("PlayerOnly", policy => policy.RequireClaim("Player"));
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();

            //le decimos que use autenticacion 
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=games}/{ action = Get}");
            });
        }
    }
}
