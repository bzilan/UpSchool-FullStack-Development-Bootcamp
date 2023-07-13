using Domain.Common;
using Domain.Enums;

namespace Domain.Entities
{
    public class Order : EntityBase<Guid>
    {
        public Guid Id { get; set; }

        public int RequestedAmount { get; set; }
        public int TotalFoundAmount { get; set; } //65
        public ProductCrawlType ProductCrawlType { get; set; }
        public ICollection<OrderEvent> OrderEvents { get; set; } //BotStarted
        public ICollection<Product> Products { get; set; } //BotStarted
        public DateTimeOffset CreatedOn { get; set; }
    }
}
