using Common.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Matgr.Products.Infrastructure.Data.Configurations;
using Matgr.Products.Core.Entities;

namespace Matgr.Products.Infrastructure.Data
{
    public class ProductsDbContext : DbContext
    {
        public ProductsDbContext(DbContextOptions<ProductsDbContext> options)
                                                                   : base(options) { }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
        }

    }
}
