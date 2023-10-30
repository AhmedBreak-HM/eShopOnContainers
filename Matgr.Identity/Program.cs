using Duende.IdentityServer.AspNetIdentity;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel.Client;
using Matgr.Identity.Data;
using Matgr.Identity.Entities;
using Matgr.Identity.IdentityConfig;
using Matgr.Identity.Services;
using Matgr.Identity.Utilits;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Matgr.Identity
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddCors();

            builder.Services.AddTransient<IProfileService, ProfileService>();

            // Add DbContext
            var connectionString = builder.Configuration.GetConnectionString("IdentityServerDbConnection");
            builder.Services.AddDbContext<IdentityServerDbContext>(x =>
            {
                x.UseSqlServer(connectionString);

            });
            // Add identity 
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                            .AddEntityFrameworkStores<IdentityServerDbContext>()
                            .AddDefaultTokenProviders();

            var identity = builder.Services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                options.EmitStaticAudienceClaim = true;
            })
            .AddInMemoryIdentityResources(SD.IdentityResources)
            .AddInMemoryApiScopes(SD.ApiScopes)
            .AddInMemoryClients(SD.Clients)
            .AddAspNetIdentity<ApplicationUser>()
            .AddProfileService<ProfileService>();

            identity.AddProfileService<ProfileService>();
            identity.AddDeveloperSigningCredential();





            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseCors(builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseIdentityServer();

            app.UseAuthorization();

            //using var scope = app.Services.CreateScope();
            //{
            //    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            //    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            //    DbInitializer.Initialize(userManager, roleManager);
            //}


            app.MapRazorPages();


            app.Run();


        }
    }
}