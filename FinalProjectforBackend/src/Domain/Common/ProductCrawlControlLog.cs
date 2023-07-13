using Domain.Enums;

namespace Domain.Common
{
    public class ProductCrawlControlLog
    {
        public Guid Id { get; set; }
        public DateTimeOffset Date { get; set; }
        public ProductCrawlType ProductCrawlType { get; set; }
        public static ProductCrawlType ConvertToProductCrawlType(string productCrawlType)
        {
            switch (productCrawlType)
            {
                case "All": return ProductCrawlType.All;

                case "OnDiscount": return ProductCrawlType.OnDiscount;

                case "NonDiscount": return ProductCrawlType.NonDiscount;

                default:
                    throw new Exception("ProductCrawlType couldn't identified.");
            }
        }
    }
}
