using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GYX.Core;

namespace GYX.Core
{
    public interface IBaseService<T> where T : class
    {
        IQueryable<T> Entitys { get; }

        int Count(Expression<Func<T, bool>> exp);
        bool Delete(T entity);
        Task<bool> DeleteAsync(T entity);
        bool DeleteByList(List<T> entitys);
        Task<bool> DeleteByListAsync(List<T> entitys);
        Expression<Func<T, bool>> ExpressionFactory(object obj);
        T FindByFeldName(Expression<Func<T, bool>> expfeldName);
        Task<T> FindByFeldNameAsync(Expression<Func<T, bool>> expfeldName);
        T FindById(object Id);
        List<T> FindByKeyValues(params object[] keyValues);
        IEnumerable<dynamic> Get(object objs = null);
        IEnumerable<dynamic> GetForPaging(out int total, object objs = null, int pageIndex = 0, int pageSize = int.MaxValue);
        bool Insert(T entity);
        Task<bool> InsertAsync(T entity);
        bool InsertByList(List<T> entitys);
        Task<bool> InsertByListAsync(List<T> entitys);
        IQueryable<T> List();
        Task<List<T>> ListAsync();
        bool Update(T entity);
        Task<bool> UpdateAsync(T entity);
        bool UpdateByList(List<T> entitys);
        Task<bool> UpdateByListAsync(List<T> entitys);
        void TrimObj(T entity);
    }
}