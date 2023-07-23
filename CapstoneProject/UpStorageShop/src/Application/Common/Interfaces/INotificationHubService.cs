using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface INotificationHubService
    {
        Task SendNotificationAsync(string notification, CancellationToken cancellationToken);
    }
}
