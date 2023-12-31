﻿using Application.Common.Models.General;
using MediatR;

namespace Application.Features.Products.Queries.GetAll
{
    public class ProductGetAllQuery:IRequest<List<ProductGetAllDto>>
    {
        //public int PageNumber { get; set; }
        //public int PageSize { get; set; }
        public Guid OrderId { get; set; }
        public bool? IsOnSale { get; set; }
        public bool? IsDeleted { get; set; }

        public ProductGetAllQuery(Guid Id, bool? isOnSale, Guid orderId, bool? ısDeleted)
        {
            OrderId = orderId;

            IsOnSale = isOnSale;
            IsDeleted = ısDeleted;
        }
    }
}
