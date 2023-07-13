namespace Domain.Common
{
    public interface IDeletedByEntity
    {
        DateTimeOffset? ModifiedOn { get; set; }
        string? DeletedByUserId { get; set; }
        bool IsDeleted { get; set; }
    }
}
