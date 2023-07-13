using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Orders.Queries.GetById
{
    public class OrderGetByIdQueryHandler : IRequestHandler<OrderGetByIdQuery, List<OrderGetByIdDto>>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public OrderGetByIdQueryHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

       
        public async Task<List<OrderGetByIdDto>> Handle(OrderGetByIdQuery request, CancellationToken cancellationToken)
        {
            var dbQuery = _applicationDbContext.Orders.AsQueryable();

            dbQuery = dbQuery.Where(x => x.Id == request.Id);

            var orders = await dbQuery.ToListAsync(cancellationToken);

            var orderDtos = MapAddressesToGetByIdDtos(orders);

            return orderDtos.ToList();
        }
        private IEnumerable<OrderGetByIdDto> MapAddressesToGetByIdDtos(List<Order> orders)
        {
            foreach (var order in orders)
            {
                yield return new OrderGetByIdDto()
                {
                    Id = order.Id,
                };
            }
        }
    }
}
