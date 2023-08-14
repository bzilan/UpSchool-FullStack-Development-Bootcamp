using Application.Common.Interfaces;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Features.Orders.Commands.Add
{
    public class OrderAddCommandHandler : IRequestHandler<OrderAddCommand, Response<Guid>>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IOrderHubService _orderHubService;
        private readonly ICurrentUserService _currentUserService;

        public OrderAddCommandHandler(IApplicationDbContext applicationDbContext, IOrderHubService orderHubService, ICurrentUserService currentUserService)
        {
            _applicationDbContext = applicationDbContext;
            _orderHubService = orderHubService;
            _currentUserService = currentUserService;
        }

        public async Task<Response<Guid>> Handle(OrderAddCommand request, CancellationToken cancellationToken)
        {
            var order = new Order()
            {
                Id=request.Id,
                CreatedByUserId = _currentUserService.UserId,
                RequestedAmount = (int)request.RequestedAmount,
                CreatedOn = DateTimeOffset.Now,
            };



            await _applicationDbContext.Orders.AddAsync(order, cancellationToken);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            await _orderHubService.AddedAsync(request.Id,cancellationToken);

            return new Response<Guid>($"Get {order.ProductCrawlType} from db.", order.Id);

        }
    }
}
