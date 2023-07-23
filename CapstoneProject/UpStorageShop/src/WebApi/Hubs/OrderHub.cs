using Application.Common.Models.WorkerService;
using Application.Features.Orders.Commands.Add;
using Application.Features.Orders.Queries.GetById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace WebApi.Hubs
{
    public class OrderHub:Hub
    {
        private ISender? _mediator;
        private readonly IHttpContextAccessor _contextAccessor;
        public OrderHub(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        protected ISender Mediator => _mediator ??= _contextAccessor.HttpContext.RequestServices.GetRequiredService<ISender>();

        [Authorize]
        public async Task<Guid> AddANewOrder(OrderAddCommand command)
        {
            var accessToken = Context.GetHttpContext().Request.Query["access_token"];

            var result = await Mediator.Send(command);

            var orderGetById = await Mediator.Send(new OrderGetByIdQuery(result.Data));

            await Clients.All.SendAsync("NewOrderAdded", new WorkerServiceNewOrderAddedDto(orderGetById, accessToken));

            return result.Data;

        }    
    }
}
