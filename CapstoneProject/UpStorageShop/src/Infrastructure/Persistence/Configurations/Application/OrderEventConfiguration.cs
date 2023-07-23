using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Application
{
    public class OrderEventConfiguration : IEntityTypeConfiguration<OrderEvent>
    {
        public void Configure(EntityTypeBuilder<OrderEvent> builder)
        {
            // ID
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            // Status
            builder.Property(x => x.Status).IsRequired();

            // CreatedOn
            builder.Property(x => x.CreatedOn).IsRequired();

            // CreatedByUserId
            builder.Property(x => x.CreatedByUserId).IsRequired(false);
            builder.Property(x => x.CreatedByUserId).HasMaxLength(100);



            //builder.HasOne(x => x.Order)
            //    .WithMany(x => x.OrderEvents)
            //    .HasForeignKey(x => x.OrderId);


            builder.ToTable("OrderEvents");
        }
    }
}
