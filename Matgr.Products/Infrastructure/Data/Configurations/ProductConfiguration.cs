using Matgr.Products.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Emit;

namespace Matgr.Products.Infrastructure.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("Id");
            builder.Property(x => x.Name).HasColumnName("Name").HasMaxLength(50).IsRequired();
            builder.Property(x => x.Description).HasColumnName("Description").HasMaxLength(200);
            builder.Property(x => x.Price).HasColumnName("Price").HasDefaultValue(0);
            builder.Property(x => x.CategoryName).HasColumnName("CatogryName");
            builder.Property(x => x.ImageUrl).HasColumnName("ImageUrl");

            builder.HasData(Create());
        }
        private static Product[] Create()
        {
            var products = new List<Product>()
            {
                                new Product
                {
                    Id = 1,
                    Name = "MSI GF63 Thin Gaming Laptop",
                    Price = 7800,
                    Description = "MSI GF63 Thin Gaming Laptop - Intel Core I5 - 8GB RAM - 256GB SSD - 15.6-inch FHD - 4GB GPU - Windows 10 - Black (English Keyboard).",
                    ImageUrl = "/images/products/1.jpg",
                    CategoryName = "Laptops"
                },
            new Product
            {
                Id = 2,
                Name = "DELL G3 15-3500 Gaming Laptop",
                Price = 12400,
                Description = "DELL G3 15-3500 Gaming Laptop - Intel Core I5-10300H - 8GB RAM - 256GB SSD + 1TB HDD - 15.6-inch FHD - 4GB GTX 1650 GPU - Ubuntu - Black.",
                ImageUrl = "/images/products/2.jpg",
                CategoryName = "Laptops"
            },
            new Product
            {
                Id = 3,
                Name = "Samsung UA43T5300",
                Price = 6500,
                Description = "Samsung UA43T5300 - 43-inch Full HD Smart TV With Built-In Receiver.",
                ImageUrl = "/images/products/3.jpg",
                CategoryName = "Televisions"
            },
            new Product
            {
                Id = 4,
                Name = "LG 43LM6370PVA",
                Price = 6200,
                Description = "LG 43LM6370PVA - 43-inch Full HD Smart TV With Built-in Receiver.",
                ImageUrl = "/images/products/4.jpg",
                CategoryName = "Televisions"
            }
            ,
            new Product
            {
                Id = 5,
                Name = "Samsung Galaxy A12",
                Price = 2850,
                Description = "Samsung Galaxy A12 - 6.4-inch 128GB/4GB Dual SIM Mobile Phone - White.",
                ImageUrl = "/images/products/5.jpg",
                CategoryName = "Mobile Phones"
            },
            new Product
            {
                Id = 6,
                Name = "Apple iPhone 12 Pro Max",
                Price = 22500,
                Description = "Apple iPhone 12 Pro Max Dual SIM with FaceTime - 256GB - Pacific Blue.",
                ImageUrl = "/images/products/6.jpg",
                CategoryName = "Mobile Phones"
            },
            new Product
            {
                Id = 7,
                Name = "OPPO Realme 8 Pro Case",
                Price = 115,
                Description = "OPPO Realme 8 Pro Case, Dual Layer PC Back TPU Bumper Hybrid No-Slip Shockproof Cover For OPPO Realme 8 / Realme 8 Pro 4G.",
                ImageUrl = "/images/products/7.jpg",
                CategoryName = "Mobile Accessories"
            },new Product
            {
                Id = 8,
                Name = "Galaxy Z Flip3 5G Case",
                Price = 250,
                Description = "Galaxy Z Flip3 5G Case, Slim Luxury Electroplate Frame Crystal Clear Back Protective Case Cover For Samsung Galaxy Z Flip 3 5G Purple.",
                ImageUrl = "/images/products/8.jpg",
                CategoryName = "Mobile Accessories"
            }


            };


            return products.ToArray();
        }
    }
}
