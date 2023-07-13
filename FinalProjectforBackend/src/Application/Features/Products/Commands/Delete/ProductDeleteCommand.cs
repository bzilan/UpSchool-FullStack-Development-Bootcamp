using Domain.Common;
using Domain.Entities;
using MediatR;

namespace Application.Features.Products.Commands.Delete
{
    public class ProductDeleteCommand : IRequest<Response<int>>
    {
        public Guid Id { get; set; }
    }
}
