using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Orders.Queries.GetAll
{
    public class OrderGetAllQuery : IRequest<List<OrderGetAllDto>>
    {
        public Guid Id { get; set; }
        public bool? IsDeleted { get; set; }
        public OrderGetAllQuery(Guid id,bool? isDeleted)
        {
            Id = id;

            IsDeleted = isDeleted;
        }
    }
}
