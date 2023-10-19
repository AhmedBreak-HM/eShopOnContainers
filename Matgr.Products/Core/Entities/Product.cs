using Common.Core.Entities.Base;

namespace Matgr.Products.Core.Entities
{
    public class Product : EntityBase<int, string>
    {
        public double Price { get; set; }

        public string? Description { get; set; }

        public string? CategoryName { get; set; }

        public string? ImageUrl { get; set; }
    }
}
