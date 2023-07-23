using Application.Common.Dtos;
using Application.Common.Models.WorkerService;
using Domain.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace WebApi.Hubs
{
    public class SeleniumLogHub:Hub
    {
        public async Task SendLogNotificationAsync(SeleniumLogDto log)
        {
            await Clients.AllExcept(Context.ConnectionId).SendAsync("NewSeleniumLogAdded", log);
        }

        [Authorize]
        public async Task SendTokenAsync()
        {
            var accessToken = Context.GetHttpContext().Request.Query["access_token"];

            Console.WriteLine(accessToken);

            await Clients.All.SendAsync(SignalRMethodKeys.Log.SendToken, new WorkerServiceNewOrderAddedDto(accessToken));
        }
    }
}
