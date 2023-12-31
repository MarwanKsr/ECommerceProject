﻿namespace OrderApi.Models.ViewModel
{
    public class ProductDto
    {
        public long ProductId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        public Product ToEntity()
        {
            return new()
            {
                ProductId = ProductId,
                Name = Name,
                Price = Price
            };
        }
    }
}
