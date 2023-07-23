using Application.Common.Dtos;

namespace Application.Common.Interfaces
{
    public interface IOrderHubService
    {
        Task AddedAsync(Guid id, CancellationToken cancellationToken);
        Task RemovedAsync(Guid id, CancellationToken cancellationToken);
        Task UpdatedAsync(OrderDto orderDto, CancellationToken cancellationToken);
    }
}
