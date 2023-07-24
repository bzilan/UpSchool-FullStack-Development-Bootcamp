using Domain.Enums;

namespace Application.Common.Dtos
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; }
        public int RequestedAmount { get; set; }
        public int TotalFoundAmount { get; set; }
        public ProductCrawlType ProductCrawlType { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string? CreatedByUserId { get; set; }
        public List<OrderEventDto> OrderEvents { get; set; }
        public List<ProductDto> Products { get; set; }
    }
}
