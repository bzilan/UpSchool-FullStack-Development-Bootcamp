using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Orders.Queries.GetAll
{
    public class OrderGetAllQueryHandler : IRequestHandler<OrderGetAllQuery, List<OrderGetAllDto>>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public OrderGetAllQueryHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<List<OrderGetAllDto>> Handle(OrderGetAllQuery request, CancellationToken cancellationToken)
        {
            var dbQuery = _applicationDbContext.Orders.AsQueryable();

            var orders = await dbQuery.ToListAsync(cancellationToken);
            var orderDtos = MapOrdersToGettAllDtos(orders);
            return orderDtos.ToList();
        }

        private IEnumerable<OrderGetAllDto> MapOrdersToGettAllDtos(List<Order> orders)
        {
            List<OrderGetAllDto> orderGetAllDtos = new List<OrderGetAllDto>();
            foreach (var order in orders)
            {

                yield return new OrderGetAllDto()
                {
                    Id = order.Id,
                    UserId = order.UserId,
                    RequestedAmount = order.RequestedAmount,
                    TotalFoundAmount = order.TotalFoundAmount,
                    ProductCrawlType = order.ProductCrawlType,
                    CreatedOn = order.CreatedOn,

                };
            }
        }
    }
}
