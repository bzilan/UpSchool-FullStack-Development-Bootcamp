using Application.Features.Orders.Queries.GetById;

namespace Application.Common.Models.WorkerService
{
    public class WorkerServiceNewOrderAddedDto
    {
        public OrderGetByIdDto Order { get; set; }
        public string AccessToken { get; set; }

        public WorkerServiceNewOrderAddedDto(OrderGetByIdDto order, string accessToken)
        {
            Order = order;

            AccessToken = accessToken;
        }
    }
}
