using ECommerce.Domain.Entities.OrderModuleEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence.SystemData.Configurations.OrderModuleConfigurations
{
    public class DeliveryMethodConfigurations : IEntityTypeConfiguration<DeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {
            builder.Ignore(x => x.CreatedAt).Ignore(x => x.UpdatedAt);
            builder.Property(x => x.Price)
                .HasPrecision(8, 2);

            builder.Property(x => x.ShortName)
                .HasMaxLength(50);

            builder.Property(x => x.Description)
                .HasMaxLength(100);

            builder.Property(x => x.DeliveryTime)
                .HasMaxLength(50);

            builder.HasMany<Order>()
                .WithOne(o => o.DeliveryMethod) 
                .HasForeignKey(o => o.DeliveryMethodId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
