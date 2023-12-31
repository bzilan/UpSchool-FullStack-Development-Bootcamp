﻿using Domain.Enums;

namespace Application.Common.Dtos
{
    public class OrderEventDto
    {
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }

        public OrderStatus Status { get; set; }

        public DateTimeOffset CreatedOn { get; set; }
        public int TotalFoundAmount { get; set; }
    }
}
