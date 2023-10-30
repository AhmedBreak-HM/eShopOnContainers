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
using IdentityModel.Client;
using Duende.IdentityServer.AspNetIdentity;

namespace Matgr.Products
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Add Product RegisterServices
            builder.Services.AddProductRegisterServices(builder.Configuration);
            builder.Services.AddHttpClient<IHttpClientFactory>();

            builder.Services.AddCors();

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddAuthentication("Bearer")
                    .AddJwtBearer("Bearer", opt =>
                    {
                        opt.Authority = "https://localhost:7146/";
                        opt.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateAudience = false

                        };
                    });
            builder.Services.AddAuthorization(opt =>
            {
                opt.AddPolicy("ApiScope", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "matgr");
                });
            });

            builder.Services.AddSwaggerGen(c =>
            {

                c.SwaggerDoc("v1", new OpenApiInfo() { Title = "Matgr.ProductsAPI" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"Enter 'Bearer' [space] and your token",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement{
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type= ReferenceType.SecurityScheme,
                            Id= "Bearer"
                        },
                        Scheme="outh2",
                        Name="Bearer",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }});
            });




            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();

                app.UseSwaggerUI(c =>
                {
                    //c.SwaggerEndpoint("/swagger/v1/swagger.json", "Matgr.Products API V1");
                    //c.OAuthClientId(builder.Configuration["IdentityServer:ClientId"]);
                    //c.OAuthClientSecret(builder.Configuration["IdentityServer:ClientSecret"]);
                    //c.OAuthUsePkce();
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
            //app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


            app.Run();
        }
    }
}