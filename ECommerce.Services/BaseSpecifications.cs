using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services
{
    public abstract class BaseSpecifications<TKey, TEntity> : ISpecifications<TKey, TEntity>
        where TEntity : BaseEntity<TKey>
    {
        protected BaseSpecifications(Expression<Func<TEntity,bool>> criteria)
        {
            Criteria = criteria;
        }
        public ICollection<Expression<Func<TEntity, object>>> IncludeExpressions { get; } = [];

        public Expression<Func<TEntity, bool>> Criteria { get; }

        public Expression<Func<TEntity, object>> OrderBy { private set; get; }
        protected void AddOrderByExpression(Expression<Func<TEntity,object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }

        public Expression<Func<TEntity, object>> OrderByDescending { private set; get; }

        public int Skip {  private set; get; }

        public int Take {  private set; get; }
        public bool IsPaginated{ private set; get; }
        protected void ApplyPagination(int pageSize , int pageIndex)
        {
            IsPaginated = true;
            Skip = (pageIndex - 1)*pageSize;
            Take = pageSize;
        }

        protected void AddOrderByDesendingExpression(Expression<Func<TEntity, object>> orderByDesendingExpression)
        {
            OrderByDescending = orderByDesendingExpression;
        }

        protected void AddInclude(Expression<Func<TEntity,object>> expression)
        {
            IncludeExpressions.Add(expression);
        }
    }
}
