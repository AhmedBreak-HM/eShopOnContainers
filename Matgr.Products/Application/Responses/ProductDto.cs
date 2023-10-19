﻿namespace Matgr.Products.Application.Responses
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        public string? Description { get; set; }

        public string? CatogryName { get; set; }

        public string? ImageUrl { get; set; }
    }
}