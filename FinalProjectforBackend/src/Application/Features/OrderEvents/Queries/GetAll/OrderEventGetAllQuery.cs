using MediatR;

namespace Application.Features.OrderEvents.Queries.GetAll
{
    public class OrderEventGetAllQuery : IRequest<List<OrderEventGetAllDto>>
    {
        public OrderEventGetAllQuery(bool? isDeleted)
        {
            IsDeleted = isDeleted;
        }
        public bool? IsDeleted { get; set; }
    }
}
