using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Identity_Demo;
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

            builder.Services.AddTransient<IProfileService, CustomProfileService>();


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

            // AddIdentityServer

            var clients = builder.Configuration.GetSection("IdentityServer:Clients").Get<List<ClientConfig>>();

            var apiScopes = builder.Configuration.GetSection("IdentityServer:ApiScopes").Get<List<ApiScopeConfig>>();

            var roles = builder.Configuration.GetSection("IdentityServer:Roles").Get<List<string>>();

            var identity = builder.Services.AddIdentityServer(options =>
            {
                options.IssuerUri = builder.Configuration["IdentityServer:IssuerUri"];
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                options.EmitStaticAudienceClaim = true;
            }).AddInMemoryClients(clients.Select(client => new Client
            {
                ClientId = client.ClientId,
                ClientSecrets = client.ClientSecrets.Select(secret => new Secret(secret.Value.Sha256())).ToList(),
                AllowedGrantTypes = client.AllowedGrantTypes,
                AllowedScopes = client.AllowedScopes,
                RedirectUris = client.RedirectUris,
                PostLogoutRedirectUris = client.PostLogoutRedirectUris
            }).ToList())


            .AddInMemoryApiScopes(
                apiScopes.Select(scope => new ApiScope(scope.Name, scope.DisplayName)).ToList())
            .AddInMemoryIdentityResources(new List<IdentityResource>
                 {
                    new IdentityResources.OpenId(),
                    new IdentityResources.Profile(),
                    new IdentityResources.Email(),
                    new IdentityResource("role", "User roles", roles)

                 })
            .AddProfileService<CustomProfileService>()
            .AddDeveloperSigningCredential() // For Development
            .AddAspNetIdentity<ApplicationUser>();




            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

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