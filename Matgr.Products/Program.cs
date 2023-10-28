using Matgr.Products.Application.Mappers;
using Matgr.Products.Helper.ConfigureServices;
using Matgr.Products.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Configuration;
using Duende.IdentityServer.Models;
using System.Net;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace Matgr.Products
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Add Product RegisterServices
            builder.Services.AddProductRegisterServices(builder.Configuration);

            builder.Services.AddCors();

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();


            var identityServerConfig =builder.Configuration.GetSection("IdentityServer");

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Bearer";
                options.DefaultChallengeScheme = "oidc";
            }).AddCookie("Cookies")

            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = identityServerConfig["Authority"];
                options.Audience = identityServerConfig["ApiScope"];
            })
            .AddOpenIdConnect("oidc", options =>
            {
                options.Authority = identityServerConfig["Authority"];
                options.ClientId = identityServerConfig["ClientId"];
                options.ClientSecret = identityServerConfig["ClientSecret"].Sha256();
                options.ResponseType = "code";
                options.SaveTokens = true;
                options.GetClaimsFromUserInfoEndpoint = true;
                options.Scope.Clear();
                options.Scope.Add("Matgr.Products.API");
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("email");
                options.Scope.Add("role");
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name",
                    RoleClaimType = "role"
                };
                options.ClaimActions.MapUniqueJsonKey("website", "website");


            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("ProductsScope", policy => policy.RequireClaim("scope", "Matgr.Products.API"));
                //options.AddPolicy("ReadScope", policy => policy.RequireClaim("scope", "read"));
                //options.AddPolicy("WriteScope", policy => policy.RequireClaim("scope", "write"));
                options.AddPolicy("AdminRole", policy => policy.RequireClaim("role", "Admin"));
                options.AddPolicy("CustomerRole", policy => policy.RequireClaim("role", "Customer"));

            });

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Matgr.Products API", Version = "v1" });

                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {

                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri($"{builder.Configuration["IdentityServer:Authority"]}/connect/authorize"),
                            TokenUrl = new Uri($"{builder.Configuration["IdentityServer:Authority"]}/connect/token"),
                            Scopes = new Dictionary<string, string>
                    {
                        { "Matgr.Products.API", "Matgr Products API" }
                    }
                        }
                    }
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
                            },
                            new[] { "Matgr.Products.API" }
                        }});
            });


           

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();

                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Matgr.Products API V1");
                    c.OAuthClientId(builder.Configuration["IdentityServer:ClientId"]);
                    c.OAuthClientSecret(builder.Configuration["IdentityServer:ClientSecret"]);
                    c.OAuthUsePkce();
                });
            }
            app.UseCors(c =>
            {
                c.AllowAnyOrigin();
                c.AllowAnyHeader();
                c.AllowAnyMethod();
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });



            app.Run();
        }
    }
}