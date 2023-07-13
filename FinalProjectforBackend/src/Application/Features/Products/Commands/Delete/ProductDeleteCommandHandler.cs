using Application.Common.Interfaces;
using Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Products.Commands.Delete
{
    public class ProductDeleteCommandHandler : IRequestHandler<ProductDeleteCommand, Response<int>>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public ProductDeleteCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<Response<int>> Handle(ProductDeleteCommand request, CancellationToken cancellationToken)
        {
            var product = await _applicationDbContext.Products
                .FirstOrDefaultAsync(x => x.Id == request.Id,cancellationToken);

            if(product != null)
            {
                _applicationDbContext.Products.Remove(product);

                await _applicationDbContext.SaveChangesAsync(cancellationToken);
            }

            return new Response<int>($"The product was successfully moved deleted.");
        }
    }
}
