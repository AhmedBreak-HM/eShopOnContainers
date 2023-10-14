using Common;

namespace Matgr.Products.Models
{
    public class Product : BaseEntity<int, string>
    {
        public double Price { get; set; }

        public string? Description { get; set; }

        public string? CatogryName { get; set; }

        public string? ImageUrl { get; set; }
    }
}
