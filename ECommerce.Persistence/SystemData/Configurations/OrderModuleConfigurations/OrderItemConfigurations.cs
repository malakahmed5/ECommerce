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
    public class OrderItemConfigurations : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.Property(x => x.Price)
                .HasPrecision(8,2);

            builder.OwnsOne(x => x.Product, OI =>
            {
                OI.Property(x => x.ProductId)
                .HasColumnName("ProductId");

                OI.Property(x => x.ProductName)
                .HasColumnName("ProductName")
                .HasMaxLength(100);

                OI.Property(x => x.PictureUrl)
                .HasColumnName("PictureUrl")
                .HasMaxLength(200);
            });

        }
    }
}
