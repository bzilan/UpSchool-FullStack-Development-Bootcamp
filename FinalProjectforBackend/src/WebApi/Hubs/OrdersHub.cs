using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Hubs
{
    public class OrdersHub:Hub
    {

        public override Task OnConnectedAsync()
        {
            //get conntection Id Context.ConnectionId;
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        //private readonly ApplicationDbContext _applicationDbContext;

        //public OrdersHub(ApplicationDbContext applicationDbContext)
        //{
        //    _applicationDbContext = applicationDbContext;
        //}

        //public async Task<bool> LogAsync(string log)
        //{
        //    var order = await _applicationDbContext.Orders.FirstOrDefaultAsync(x => x.Id ==log.ToString());

        //    if (order is null) return false;

        //    _applicationDbContext.Orders.Remove(order);
        //    await _applicationDbContext.SaveChangesAsync();
        //    return true;
        //}
    }
}
