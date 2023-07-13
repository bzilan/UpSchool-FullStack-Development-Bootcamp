using Domain.Entities;
using System.Security.Principal;

namespace Domain.Dtos
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public bool IsOnSale { get; set; }
        public decimal Price { get; set; }
        public decimal SalePrice { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
    }

    //public static ProductDto MapFromProduct(Product product)
    //{
    //    return new ProductDto()
    //    {
    //        Id = product.Id,
    //        Name = product.Name,
    //        Picture = product.Picture,
    //        Price = product.Price,
    //        IsOnSale = product.IsOnSale,
    //        CreatedOn = product.CreatedOn
    //    };
    //}
}
