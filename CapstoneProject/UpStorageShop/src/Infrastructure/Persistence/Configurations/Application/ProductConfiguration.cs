using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Application
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            // ID
            builder.HasKey(x => x.Id);

            // Name
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Name).HasMaxLength(150);
            builder.HasIndex(x => x.Name);

            //Picture
            builder.Property(x => x.Picture)
                .IsRequired()
                .HasMaxLength(500);

            // IsOnSale
            builder.Property(x => x.IsOnSale).IsRequired();

            // Price
            builder.Property(x => x.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired();


            // SalePrice
            builder.Property(x => x.SalePrice)
                 .HasColumnType("decimal(18, 2)")
                 .IsRequired();

            // CreatedOn
            builder.Property(x => x.CreatedOn).IsRequired();

            // CreatedByUserId
            builder.Property(x => x.CreatedByUserId).IsRequired(false);
            builder.Property(x => x.CreatedByUserId).HasMaxLength(100);

            //DeletedOn
            builder.Property(x => x.DeletedOn).IsRequired(false);

            //IsDeleted
            builder.Property(x => x.IsDeleted).IsRequired();
            builder.Property(x => x.IsDeleted).HasDefaultValueSql("0");
            builder.HasIndex(x => x.IsDeleted);

            // Configure the relationship between Product and Order entities
            builder.HasOne(x => x.Order)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Products");
        }
    }
}
