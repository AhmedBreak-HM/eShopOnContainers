using Matgr.Products.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace Matgr.Products.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("Id");
            builder.Property(x => x.Name).HasColumnName("Name").HasMaxLength(50).IsRequired();
            builder.Property(x => x.Description).HasColumnName("Description").HasMaxLength(100);
            builder.Property(x => x.Price).HasColumnName("Price").HasDefaultValue(0);
            builder.Property(x => x.CatogryName).HasColumnName("CatogryName");
            builder.Property(x => x.ImageUrl).HasColumnName("ImageUrl");



        }

    }
}
