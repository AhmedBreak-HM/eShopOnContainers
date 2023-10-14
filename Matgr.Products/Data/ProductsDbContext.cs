using Matgr.Products.Configurations;
using Matgr.Products.Models;
using Microsoft.EntityFrameworkCore;

namespace Matgr.Products.Data
{
    public class ProductsDbContext:DbContext
    {
        public ProductsDbContext(DbContextOptions<ProductsDbContext> options)
                                                                   : base(options) { }

        public DbSet<Product>  Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
        }

    }
}
