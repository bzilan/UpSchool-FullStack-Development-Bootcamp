using Application.Common.Models.General;
using MediatR;

namespace Application.Features.Products.Queries.GetAll
{
    public class ProductGetAllQuery: IRequest<PaginatedList<ProductGetAllDto>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public Guid OrderId { get; set; }
        

        public ProductGetAllQuery(Guid Id, Guid orderId,int pageNumber,int pageSize)
        {
            OrderId = orderId;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
