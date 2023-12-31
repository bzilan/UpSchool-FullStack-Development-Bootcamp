﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Orders.Queries.GetById
{
    public class OrderGetByIdQuery:IRequest<List<OrderGetByIdDto>>
    {
        public Guid Id { get; set; }

        public OrderGetByIdQuery(Guid Id)
        {
           Id = Id;
        }
    }
}
