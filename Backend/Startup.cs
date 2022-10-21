using System;
using System.IO;
using Backend.Data;
using Backend.Data.ImportData;
using Backend.Domain.Auth;
using Backend.Domain.Game.Services;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Backend
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
            services.AddDbContext<GameContext>(options =>
            {
                options.UseSqlite($"Data Source={Path.Combine("Data", "game.db")}");
            });
            services.AddIdentity<User, IdentityRole<int>>()
                .AddEntityFrameworkStores<GameContext>()
                .AddUserManager<UserManager<User>>()
                .AddRoleManager<RoleManager<IdentityRole<int>>>();

            services.AddAuthentication();

            services.AddControllers();

            services.AddHttpContextAccessor();

            services.AddScoped<IGameService, GameService>();

            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "Backend", Version = "v1"}); });


            // MediatR is used to dispatch requests to our domain. See https://github.com/jbogard/MediatR for more information.
            services.AddMediatR(typeof(Startup));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // GameContext added here to allow for testData, this may change...
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, GameContext db)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Backend v1"));
            }

            Console.WriteLine(db.Database.ProviderName);
            //Trick for avoiding imports when running tests
            if (db.Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
            {
                var dataImporter = new DataImporter(db);
                Console.WriteLine(dataImporter.Import());
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseDefaultFiles();

            app.UseStaticFiles();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}