using Matgr.Products.Application.Mappers;
using Matgr.Products.Helper.ConfigureServices;
using Matgr.Products.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;


namespace Matgr.Products
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Add Product RegisterServices
            builder.Services.AddProductRegisterServices(builder.Configuration);


            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();


            builder.Services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = builder.Configuration["IdentityServer:Authority"];
                    options.Audience = builder.Configuration["IdentityServer:ApiScope"];

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
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


            //builder.Services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo() { Title = "Matgr.ProductsAPI" });
            //    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            //    {
            //        Description = @"Enter 'Bearer' [space] and your token",
            //        Name = "Authorization",
            //        In = ParameterLocation.Header,
            //        Type = SecuritySchemeType.ApiKey,
            //        Scheme = "Bearer"
            //    });

            //    c.AddSecurityRequirement(new OpenApiSecurityRequirement{
            //            {
            //                new OpenApiSecurityScheme
            //                {
            //                    Reference = new OpenApiReference
            //                    {
            //                        Type= ReferenceType.SecurityScheme,
            //                        Id= "Beaer"
            //                    },
            //                    Scheme="outh2",
            //                    Name="Bearer",
            //                    In = ParameterLocation.Header
            //                },
            //                new List<string>()
            //                }
            //            });
            //});

            //builder.Services.AddAuthorization(opt =>
            //{
            //    opt.AddPolicy("ApiScope", policy =>
            //    {
            //        policy.RequireAuthenticatedUser();
            //        policy.RequireClaim("scope", "matgr");
            //    });
            //});

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