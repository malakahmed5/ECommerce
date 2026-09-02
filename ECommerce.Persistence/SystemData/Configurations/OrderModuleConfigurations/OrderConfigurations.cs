using ECommerce.Domain.Entities.IdentityModuleEntities;
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
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(o => o.SubTotal)
                .HasPrecision(8, 2);

            builder.OwnsOne(o => o.Address, OE =>
            {
                OE.Property(x => x.FirstName)
                .HasColumnName("FirstName")
                .HasMaxLength(50);

                OE.Property(x => x.LastName)
                .HasColumnName("LastName")
                .HasMaxLength(50);

                OE.Property(x => x.Country)
                .HasColumnName("Country")
                .HasMaxLength(50);

                OE.Property(x => x.City)
                .HasColumnName("City")
                .HasMaxLength(50);

                OE.Property(x => x.Street)
                .HasColumnName("Street")
                .HasMaxLength(50);
            });
        }
    }
}
