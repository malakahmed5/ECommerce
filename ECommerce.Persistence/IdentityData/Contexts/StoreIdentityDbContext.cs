using ECommerce.Domain.Entities.IdentityModuleEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence.IdentityData.Contexts
{
    public class StoreIdentityDbContext:IdentityDbContext<ApplicationUser>
    {
        public StoreIdentityDbContext(DbContextOptions<StoreIdentityDbContext> options):base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // تغيير أسامي جداول الـ Identity السبعة لإزالة كلمة AspNet
            builder.Entity<ApplicationUser>(b => b.ToTable("Users"));
            builder.Entity<IdentityRole>(b => b.ToTable("Roles"));
            builder.Entity<IdentityUserRole<string>>(b => b.ToTable("UserRoles"));
            builder.Entity<IdentityUserClaim<string>>(b => b.ToTable("UserClaims"));
            builder.Entity<IdentityUserLogin<string>>(b => b.ToTable("UserLogins"));
            builder.Entity<IdentityRoleClaim<string>>(b => b.ToTable("RoleClaims"));
            builder.Entity<IdentityUserToken<string>>(b => b.ToTable("UserTokens"));

            // الجداول الخاصة بيك مثل Address
            builder.Entity<Address>().ToTable("Addresses"); // لازم اعمل كده عشان متنزلش في تابول ال context
        }
    }
}
