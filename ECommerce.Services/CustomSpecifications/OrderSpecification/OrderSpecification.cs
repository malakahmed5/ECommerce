using ECommerce.Domain.Entities.OrderModuleEntities;
using ECommerce.Domain.Entities.ProductModuleEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.CustomSpecifications.OrderSpecification
{
    internal class OrderSpecification : BaseSpecifications<Guid, Order>
    {
        public OrderSpecification(string email)
            :base(o => o.UserEmail == email)
        {
            AddInclude(o => o.Items);
            AddInclude(o => o.DeliveryMethod);
            AddOrderByDesendingExpression(o => o.OrderDate);
        }
        public OrderSpecification(Guid id , string email)
            :base(o => o.UserEmail == email && (o.Id == id))
        {
            AddInclude(o => o.Items);
            AddInclude(o => o.DeliveryMethod);
        }
    }
}
