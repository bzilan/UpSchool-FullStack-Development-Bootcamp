using Application.Common.Dtos;
using Application.Common.Interfaces;
using Domain.Common;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Orders.Commands.Update
{
    public class OrderUpdateCommandHandler : IRequestHandler<OrderUpdateCommand, Response<Guid>>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public OrderUpdateCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Response<Guid>> Handle(OrderUpdateCommand request, CancellationToken cancellationToken)
        {
            var order = await _applicationDbContext.Orders.Where(x => x.Id == request.Id).FirstOrDefaultAsync();

            if (order == null) throw new ArgumentNullException(nameof(request.Id));

            order.TotalFoundAmount = request.TotalFoundAmount;

            _applicationDbContext.Orders.Update(order);

            await _applicationDbContext.SaveChangesAsync(cancellationToken);


            return new Response<Guid>($"The order was successfully updated.");
        }

        public OrderDto MapOrderToOrderDto(Order order)
        {
            return new OrderDto()
            {
                Id = order.Id,
                RequestedAmount = order.RequestedAmount,
                TotalFoundAmount = order.TotalFoundAmount,
                ProductCrawlType = order.ProductCrawlType,
                CreatedOn = order.CreatedOn
            };
        }
    }
}
