namespace Application.Common.Interfaces
{
    public interface IOrderHubService
    {
        Task RemovedAsync(Guid id, CancellationToken cancellationToken);
    }
}
