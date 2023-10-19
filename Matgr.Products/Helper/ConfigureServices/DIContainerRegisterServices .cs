using Matgr.Products.Application.Mappers;
using Matgr.Products.Core.Repositories;
using Matgr.Products.Infrastructure.Data;
using Matgr.Products.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Matgr.Products.Helper.ConfigureServices
{
    public static class DIContainerRegisterServices
    {
        public static IServiceCollection AddProductRegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Register Db Context
            var connectionString = configuration.GetConnectionString("ProductDbConnection");
            services.AddDbContext<ProductsDbContext>(x =>
            {
                x.UseSqlServer(connectionString);
            });

            // Register AutoMapper
            services.AddAutoMapper(typeof(ProductMappingProfile).Assembly);

            // Register Product Repository
            services.AddScoped<IProductRepository, ProductRepository>();

            return services;
        }
    }
}
