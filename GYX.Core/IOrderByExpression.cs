using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYX.Core
{
    public interface IOrderByExpression<TEntity> 
    {
        IOrderedQueryable<TEntity> ApplyOrderBy(IQueryable<TEntity> query);
        IOrderedQueryable<TEntity> ApplyThenBy(IOrderedQueryable<TEntity> query);
    }
}
