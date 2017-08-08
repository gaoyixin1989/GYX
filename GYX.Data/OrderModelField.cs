using GYX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GYX.Data
{
    public class OrderModelField<TEntity, TOrderBy> : IOrderByExpression<TEntity> where TEntity : class
    {
        public bool IsDESC { get; set; }
        public Expression<Func<TEntity, TOrderBy>> _expression;
        public OrderModelField(Expression<Func<TEntity, TOrderBy>> expression,
        bool descending = false)
        {
            _expression = expression;
            IsDESC = descending;
        }
        public IOrderedQueryable<TEntity> ApplyOrderBy(
        IQueryable<TEntity> query)
        {
            if (IsDESC)
                return query.OrderByDescending(_expression);
            else
                return query.OrderBy(_expression);
        }
        public IOrderedQueryable<TEntity> ApplyThenBy(
        IOrderedQueryable<TEntity> query)
        {
            if (IsDESC)
                return query.ThenByDescending(_expression);
            else
                return query.ThenBy(_expression);
        }
    }
    
}
