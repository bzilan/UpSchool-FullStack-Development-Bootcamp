using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Orders.Queries.GetById
{
    public class OrderGetByIdDto
    {
        public Guid Id { get; set; }

        public int RequestedAmount { get; set; }
        public int TotalFoundAmount { get; set; } //65
        public ProductCrawlType ProductCrawlType { get; set; }
        public ICollection<OrderEvent> OrderEvents { get; set; } //BotStarted
        public ICollection<Product> Products { get; set; } //BotStarted
        public DateTimeOffset CreatedOn { get; set; }

        public string ConnectionId { get; set; }
    }
}
