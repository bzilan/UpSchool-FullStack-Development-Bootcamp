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
            var id = Guid.NewGuid();
            var order = new Order()
            {
                Id = id,
                UserId = _currentUserService.UserId,
                RequestedAmount = (int)request.RequestedAmount,
                TotalFoundAmount = (int)request.TotalFoundAmount,
                ProductCrawlType = request.ProductCrawlType,
                CreatedOn = DateTimeOffset.Now,
                CreatedByUserId = _currentUserService.UserId,

                OrderEvents = new List<OrderEvent>()
                {
                    new OrderEvent()
                    {
                        Id = Guid.NewGuid(),
                        OrderId = id, // OrderEvent'in OrderId'sini Order'ın kimliğiyle eşleştiriyoruz.
                        CreatedOn = DateTimeOffset.Now,
                        Status = OrderStatus.BotStarted
                    }
                }
            };



            await _applicationDbContext.Orders.AddAsync(order, cancellationToken);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            //await _orderHubService.AddedAsync(request.Id,cancellationToken);

            return new Response<Guid>($"Get {order.ProductCrawlType} from db.", order.Id);

        }
    }
}
