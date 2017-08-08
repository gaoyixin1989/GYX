using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GYX.Core
{
    /// <summary>
    /// Repository
    /// </summary>
    public partial interface IRepository<T>
    {
        IQueryable<T> Table { get; }
        IQueryable<T> TableNoTracking { get; }

        bool Contains(Expression<Func<T, bool>> whereCondition);
        Task<bool> ContainsAsync(Expression<Func<T, bool>> whereCondition);
        int Count();
        int Count(Expression<Func<T, bool>> whereCondition);
        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<T, bool>> whereCondition);
        bool Delete(T entity);
        bool Delete(IEnumerable<T> entities);
        Task<bool> DeleteAsync(T entity);
        Task<bool> DeleteAsync(IEnumerable<T> entities);
        T FindByFeldName(Expression<Func<T, bool>> whereCondition);
        Task<T> FindByFeldNameAsync(Expression<Func<T, bool>> whereCondition);
        T GetById(object keyValue);
        List<T> GetByKeyValues(params object[] keyValues);
        bool Insert(T entity);
        bool Insert(IEnumerable<T> entities);
        Task<bool> InsertAsync(T entity);
        Task<bool> InsertAsync(IEnumerable<T> entities);
        IQueryable<T> List(Expression<Func<T, bool>> whereCondition, params IOrderByExpression<T>[] orderByExpression);
        bool Update(T entity);
        bool Update(IEnumerable<T> entities);
        Task<bool> UpdateAsync(T entity);
        Task<bool> UpdateAsync(IEnumerable<T> entities);
        /// <summary>
        /// 摘要:
        ///     创建一个原始 SQL 查询，该查询将返回此集中的实体。默认情况下，上下文会跟踪返回的实体；可通过对返回的 System.Data.Entity.Infrastructure.DbSqlQuery`1
        ///     调用 AsNoTracking 来更改此设置。请注意返回实体的类型始终是此集的类型，而不会是派生的类型。如果查询的一个或多个表可能包含其他实体类型的数据，则必须编写适当的
        ///     SQL 查询以确保只返回适当类型的实体。与接受 SQL 的任何 API 一样，对任何用户输入进行参数化以便避免 SQL 注入攻击是十分重要的。您可以在 SQL
        ///     查询字符串中包含参数占位符，然后将参数值作为附加参数提供。您提供的任何参数值都将自动转换为 DbParameter。context.Blogs.SqlQuery("SELECT
        ///     * FROM dbo.Posts WHERE Author = @p0", userSuppliedAuthor); 或者，您还可以构造一个 DbParameter
        ///     并将它提供给 SqlQuery。这允许您在 SQL 查询字符串中使用命名参数。context.Blogs.SqlQuery("SELECT * FROM
        ///     dbo.Posts WHERE Author = @author", new SqlParameter("@author", userSuppliedAuthor));
        ///
        /// 参数:
        ///   sql:
        ///     SQL 查询字符串。
        ///
        ///   parameters:
        ///     要应用于 SQL 查询字符串的参数。如果使用输出参数，则它们的值在完全读取结果之前不可用。这是由于 DbDataReader 的基础行为而导致的，有关详细信息，请参见
        ///    http://go.microsoft.com/fwlink/?LinkID=398589。
        ///
        /// 返回结果:
        ///     一个 System.Data.Entity.Infrastructure.DbSqlQuery`1 对象，此对象在枚举时将执行查询。
        /// </summary>
        /// <param name="sqlQuery">" pro_Add @i,@j,@he output"
        /// System.Data.SqlClient.SqlParameter[] parameters = { 
        /// new System.Data.SqlClient.SqlParameter("@i",100),  
        /// new System.Data.SqlClient.SqlParameter("@j",100),  
        /// new System.Data.SqlClient.SqlParameter("@he", System.Data.SqlDbType.Int) };
        /// parameters[2].Direction = System.Data.ParameterDirection.Output;
        /// int AllCount = Int32.Parse(parameters[2].Value.ToString());
        /// </param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        DbRawSqlQuery<TKey> Execute<TKey>(string sqlQuery, params object[] parameters) where TKey : IComparable<TKey>;
        /// <summary>
        /// 执行存储过程返回实体对象
        /// </summary>
        /// <typeparam name="TKey">实体对象</typeparam>
        /// <param name="sqlQuery">存储过程</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        DbRawSqlQuery<TKey> ExecuteResT<TKey>(string sqlQuery, params object[] parameters) where TKey : class;
    }
}
