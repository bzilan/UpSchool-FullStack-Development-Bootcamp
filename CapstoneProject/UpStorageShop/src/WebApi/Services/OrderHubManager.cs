using Application.Common.Dtos;
using Application.Common.Interfaces;
using Microsoft.AspNetCore.SignalR;
using WebApi.Hubs;

namespace WebApi.Services
{
    public class OrderHubManager:IOrderHubService
    {
        private readonly IHubContext<OrderHub> _hubContext;

        public OrderHubManager(IHubContext<OrderHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public Task AddedAsync(Guid id, CancellationToken cancellationToken)
        {
            return _hubContext.Clients.All.SendAsync("Added", id, cancellationToken);
        }

        public Task RemovedAsync(Guid id, CancellationToken cancellationToken)
        {
            return _hubContext.Clients.All.SendAsync("Removed", id, cancellationToken);
        }

        public Task UpdatedAsync(OrderDto orderDto, CancellationToken cancellationToken)
        {
            return _hubContext.Clients.All.SendAsync("Updated", orderDto, cancellationToken);
        }
    }
}
