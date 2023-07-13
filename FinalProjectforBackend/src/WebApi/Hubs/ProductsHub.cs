using Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Hubs
{
    public class ProductsHub:Hub
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public ProductsHub(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        //public async Task<bool> DeleteAsync(Guid productId)
        //{
        //    var product = await _applicationDbContext.Products.FirstOrDefaultAsync(x => x.Id == productId);

        //    if (product is null) return false;

        //    _applicationDbContext.Products.Remove(product);

        //    await _applicationDbContext.SaveChangesAsync();

        //    await Clients.AllExcept(Context.ConnectionId).SendAsync(productId);

        //    return true;
        //}

        public override Task OnDisconnectedAsync(Exception? exception)
        {


            return base.OnDisconnectedAsync(exception);
        }
    }
}
