using Application.Common.Interfaces;
using Microsoft.AspNetCore.SignalR;
using WebApi.Hubs;

namespace WebApi.Services
{
    public class OrderHubManager:IOrderHubService
    {
        private readonly IHubContext<OrdersHub> _hubContext;

        public OrderHubManager(IHubContext<OrdersHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public Task RemovedAsync(Guid id, CancellationToken cancellationToken)
        {
            return _hubContext.Clients.All.SendAsync("Removed", id, cancellationToken);
        }
    }
}
