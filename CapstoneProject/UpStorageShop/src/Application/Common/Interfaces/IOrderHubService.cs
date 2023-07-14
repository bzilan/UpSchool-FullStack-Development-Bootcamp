namespace Application.Common.Interfaces
{
    public interface IOrderHubService
    {
        Task AddedAsync(Guid id, CancellationToken cancellationToken);

        Task UpdatedAsync(Guid id, CancellationToken cancellationToken);
        Task RemovedAsync(Guid id, CancellationToken cancellationToken);
    }
}
