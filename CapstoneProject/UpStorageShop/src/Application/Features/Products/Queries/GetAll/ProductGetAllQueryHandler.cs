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

            dbQuery = dbQuery.Where(x => x.OrderId == request.OrderId);

            dbQuery = dbQuery.Include(x => x.Order);


            var products=await dbQuery
                .Select(x => MapToGetAllDto(x))
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return PaginatedList<ProductGetAllDto>.Create(request.PageNumber, request.PageSize);
        }

        private static ProductGetAllDto MapToGetAllDto(Product product)
        {
            return new ProductGetAllDto()
            {
                Id = product.Id,
                OrderId = product.OrderId,
                Name = product.Name,
                Picture = product.Picture,
                IsOnSale = product.IsOnSale,
                Price = product.Price,
                SalePrice = product.SalePrice
            };
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

                };
            }
        }
    }

}
