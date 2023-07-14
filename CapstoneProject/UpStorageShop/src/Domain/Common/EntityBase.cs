namespace Domain.Common
{
    public abstract class EntityBase<TKey>:IEntityBase<TKey>,ICreatedByEntity,IDeletedByEntity
    {
        public TKey Id { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string? CreatedByUserId { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
        public bool IsDeleted { get ; set ; }
    }
}
