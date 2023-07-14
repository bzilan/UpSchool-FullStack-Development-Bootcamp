using Application.Common.Interfaces;
using Application.Common.Models.General;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Products.Queries.GetAll
{
    public class ProductGetAllQueryHandler : IRequestHandler<ProductGetAllQuery, PaginatedList<ProductGetAllDto>>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public ProductGetAllQueryHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<PaginatedList<ProductGetAllDto>> Handle(ProductGetAllQuery request, CancellationToken cancellationToken)
        {
            var dbQuery = _applicationDbContext.Products.AsQueryable();

            if (request.IsDeleted.HasValue)
            {
                dbQuery = dbQuery.Where(x => x.IsDeleted == request.IsDeleted.Value && x.OrderId == request.OrderId);
            }
            else
            {
                dbQuery = dbQuery.Where(x => x.OrderId == request.OrderId);
            }
            var products = await dbQuery.ToListAsync(cancellationToken);
            var orderDtos = MapProductsToGettAllDtos(products);

            return PaginatedList<ProductGetAllDto>.Create(request.PageNumber, request.PageSize);
        }

        private static IEnumerable<ProductGetAllDto> MapProductsToGettAllDtos(List<Product> products)
        {
            List<ProductGetAllDto> productGetAllDtos = new List<ProductGetAllDto>();
            foreach (var product in products)
            {

                yield return new ProductGetAllDto()
                {
                    Id = product.Id,
                    OrderId = product.OrderId,
                    Name = product.Name,
                    Picture = product.Picture,
                    IsOnSale = product.IsOnSale,
                    Price = product.Price,
                    SalePrice = product.SalePrice,
                    IsDeleted = product.IsDeleted,

                };
            }
        }
    }

}
