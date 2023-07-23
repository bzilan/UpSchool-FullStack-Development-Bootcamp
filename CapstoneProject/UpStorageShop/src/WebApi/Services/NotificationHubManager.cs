using Application.Common.Interfaces;
using Domain.Utilities;
using Microsoft.AspNetCore.SignalR;
using WebApi.Hubs;

namespace WebApi.Services
{
    public class NotificationHubManager : INotificationHubService
    {

        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationHubManager(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }
        public Task SendNotificationAsync(string notification, CancellationToken cancellationToken)
        {
            return _hubContext.Clients.All.SendAsync(SignalRMethodKeys.Notification.NotificationAdded, notification);
        }
    }
}
