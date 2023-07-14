using Domain.Entities;
using Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Application
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            // ID
            builder.HasKey(x => x.Id);

            builder.Property(x => x.RequestedAmount)
                .IsRequired();

            builder.Property(x => x.TotalFoundAmount)
                .IsRequired();

            //ProductCrawlType
            builder.Property(x => x.ProductCrawlType).IsRequired();
            builder.Property(x => x.ProductCrawlType).HasConversion<int>();


            // CreatedOn
            builder.Property(x => x.CreatedOn)
                .IsRequired();

            // CreatedByUserId
            builder.Property(x => x.CreatedByUserId).IsRequired(false);
            builder.Property(x => x.CreatedByUserId).HasMaxLength(100);

            //DeletedOn
            builder.Property(x => x.DeletedOn).IsRequired(false);

            //IsDeleted
            builder.Property(x => x.IsDeleted).IsRequired();
            builder.Property(x => x.IsDeleted).HasDefaultValueSql("0");
            builder.HasIndex(x => x.IsDeleted);


            // Configure the relationship between Order and Product entities
            builder.HasMany(x => x.Products)
                .WithOne(x => x.Order)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.OrderEvents)
               .WithOne(x => x.Order)
               .HasForeignKey(x => x.OrderId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<User>().WithMany()
            .HasForeignKey(x => x.UserId);

            builder.ToTable("Orders");
        }
    }
}
