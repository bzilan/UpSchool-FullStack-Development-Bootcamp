using Application.Common.Interfaces;
using Domain.Common;
using Domain.Entities;
using MediatR;

namespace Application.Features.Products.Commands.Add
{
    public class ProductAddCommandHandler : IRequestHandler<ProductAddCommand, Response<Guid>>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public ProductAddCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Response<Guid>> Handle(ProductAddCommand request, CancellationToken cancellationToken)
        {
            var product = new Product()
            {
                Name = request.Name,
                OrderId = request.OrderId,
                Picture = request.Picture,
                Price= request.Price,
                SalePrice= request.SalePrice,
                IsOnSale = request.IsOnSale,
                CreatedOn = DateTimeOffset.Now,
            };
            await _applicationDbContext.Products.AddAsync(product, cancellationToken);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return new Response<Guid>($"The new product {product.Name} was successfully added.");
        }
    }
}
