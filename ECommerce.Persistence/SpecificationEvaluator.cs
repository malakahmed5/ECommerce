using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence
{
    internal static class SpecificationEvaluator
    {
        public static IQueryable<TEntity> CreateQuery<TKey,TEntity>(IQueryable<TEntity> entryPoint , 
                                                 ISpecifications<TKey,TEntity> specifications)
            where TEntity : BaseEntity<TKey>
        {
            var query = entryPoint;
            
            if(specifications is not null)
            {
                if (specifications.Criteria is not null)
                    query = query.Where(specifications.Criteria);

                if (specifications.IncludeExpressions is not null && specifications.IncludeExpressions.Any())
                {
                    foreach (var expression in specifications.IncludeExpressions)
                        query = query.Include(expression);
                }
                if (specifications.OrderBy is not null)
                    query = query.OrderBy(specifications.OrderBy);

                if(specifications.OrderByDescending is not null)
                    query = query.OrderByDescending(specifications.OrderByDescending);

                if(specifications.IsPaginated == true)
                    query = query.Skip(specifications.Skip).Take(specifications.Take);
            }
            return query;
        }
    }
}
