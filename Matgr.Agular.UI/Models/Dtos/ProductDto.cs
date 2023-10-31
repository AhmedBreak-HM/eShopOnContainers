using System.ComponentModel.DataAnnotations;

namespace Matgr.UI.Models.Dtos
{
    public class ProductDto
    {
        public int ProductId { get; set; }

        [Required]
        public string Name { get; set; }

        [Range(1, Double.MaxValue)]
        public double Price { get; set; }

        public string Description { get; set; }
        public string CategoryName { get; set; }
        public string ImageUrl { get; set; }

        [Range(1, 100)]
        public int Quantity { get; set; } = 1;
    }
}
