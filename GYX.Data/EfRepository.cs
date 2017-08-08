using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using GYX.Core;
using System.Linq.Expressions;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;

namespace GYX.Data
{
    /// <summary>
    /// Entity Framework repository
    /// </summary>
    public partial class EfRepository<T> : IRepository<T> where T : class
    {
        #region Fields

        private readonly SystemContext _context;
        private DbSet<T> _entities;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="context">Object context</param>
        public EfRepository(DbContext context)
        {
            this._context = (SystemContext)context;
        }
        public EfRepository()
        {

            _context = (SystemContext)CallContext.GetData("SystemContext");
            if (_context == null)
            {
                _context = new SystemContext();
                CallContext.SetData("SystemContext", _context);

            }
        }
        #endregion

        #region Utilities

        /// <summary>
        /// Get full error
        /// </summary>
        /// <param name="exc">Exception</param>
        /// <returns>Error</returns>
        protected string GetFullErrorText(DbEntityValidationException exc)
        {
            var msg = string.Empty;
            foreach (var validationErrors in exc.EntityValidationErrors)
                foreach (var error in validationErrors.ValidationErrors)
                    msg += string.Format("Property: {0} Error: {1}", error.PropertyName, error.ErrorMessage) + Environment.NewLine;
            return msg;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get entity by identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        public virtual T GetById(object keyValue)
        {
            //see some suggested performance optimization (not tested)
            //http://stackoverflow.com/questions/11686225/dbset-find-method-ridiculously-slow-compared-to-singleordefault-on-id/11688189#comment34876113_11688189
            return this.Entities.Find(keyValue);
        }
        public virtual List<T> GetByKeyValues(params object[] keyValues)
        {
            //see some suggested performance optimization (not tested)
            //http://stackoverflow.com/questions/11686225/dbset-find-method-ridiculously-slow-compared-to-singleordefault-on-id/11688189#comment34876113_11688189
            return this.Entities.Find(keyValues) as List<T>;
        }
        /// <summary>
        /// Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual bool Insert(T entity)
        {
            try
            {
                if (entity == null)
                {
                    return false;
                    throw new ArgumentNullException("entities");
                }
                this.Entities.Add(entity);

                this._context.SaveChanges();
                return true;
            }
            catch (DbEntityValidationException dbEx)
            {
                return false;
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }

        /// <summary>
        /// Insert entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual bool Insert(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                {
                    return false;
                    throw new ArgumentNullException("entities");
                }
                foreach (var entity in entities)
                    this.Entities.Add(entity);
                this._context.SaveChanges();
                return true;
            }
            catch (DbEntityValidationException dbEx)
            {
                return false;
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual bool Update(T entity)
        {
            try
            {
                if (entity == null)
                {
                    return false;
                    throw new ArgumentNullException("entities");
                }
                _context.Entry(entity).State = EntityState.Modified;
                this._context.SaveChanges();
                return true;
            }
            catch (DbEntityValidationException dbEx)
            {
                return false;
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }

        /// <summary>
        /// Update entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual bool Update(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                {
                    return false;
                    throw new ArgumentNullException("entities");
                }
                this._context.SaveChanges();
                return true;
            }
            catch (DbEntityValidationException dbEx)
            {
                return false;
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual bool Delete(T entity)
        {
            try
            {
                if (entity == null)
                {
                    return false;
                    throw new ArgumentNullException("entity");
                }
                this.Entities.Remove(entity);
                this._context.SaveChanges();
                return true;
            }
            catch (DbEntityValidationException dbEx)
            {
                return false;
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }

        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual bool Delete(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                {
                    return false;
                    throw new ArgumentNullException("entities");
                }
                foreach (var entity in entities)
                    this.Entities.Remove(entity);
                this._context.SaveChanges();
                return true;
            }
            catch (DbEntityValidationException dbEx)
            {
                return false;
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }

        #endregion
        #region �첽Methods


        /// <summary>
        /// Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual async Task<bool> InsertAsync(T entity)
        {
            try
            {
                if (entity == null)
                {
                    return false;
                    throw new ArgumentNullException("entity");
                }
                this.Entities.Add(entity);
                await this._context.SaveChangesAsync();
                return true;
            }
            catch (DbEntityValidationException dbEx)
            {
                return false;
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }

        /// <summary>
        /// Insert entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual async Task<bool> InsertAsync(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                {
                    return false;
                    throw new ArgumentNullException("entities");
                }
                foreach (var entity in entities)
                    this.Entities.Add(entity);
                await this._context.SaveChangesAsync();
                return true;
            }
            catch (DbEntityValidationException dbEx)
            {
                return false;
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual async Task<bool> UpdateAsync(T entity)
        {
            try
            {
                if (entity == null)
                {
                    return false;
                    throw new ArgumentNullException("entity");
                }
                _context.Entry(entity).State = EntityState.Modified;
                await this._context.SaveChangesAsync();
                return true;
            }
            catch (DbEntityValidationException dbEx)
            {
                return false;
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }

        /// <summary>
        /// Update entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual async Task<bool> UpdateAsync(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                {
                    return false;
                    throw new ArgumentNullException("entities");
                }
                await this._context.SaveChangesAsync();
                return true;
            }
            catch (DbEntityValidationException dbEx)
            {
                return false;
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual async Task<bool> DeleteAsync(T entity)
        {
            try
            {
                if (entity == null)
                {
                    return false;
                    throw new ArgumentNullException("entity");
                }
                this.Entities.Remove(entity);
                await this._context.SaveChangesAsync();
                return false;
            }
            catch (DbEntityValidationException dbEx)
            {
                return false;
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }

        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual async Task<bool> DeleteAsync(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                {
                    return false;
                    throw new ArgumentNullException("entities");
                }
                foreach (var entity in entities)
                    this.Entities.Remove(entity);
                await this._context.SaveChangesAsync();
                return true;
            }
            catch (DbEntityValidationException dbEx)
            {
                return false;
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }

        #endregion  
        #region Properties

        /// <summary>
        /// Gets a table
        /// </summary>
        public virtual IQueryable<T> Table
        {
            get
            {
                return this.Entities;
            }
        }

        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        public virtual IQueryable<T> TableNoTracking
        {
            get
            {
                return this.Entities.AsNoTracking();
            }
        }

        /// <summary>
        /// Entities
        /// </summary>
        protected virtual DbSet<T> Entities
        {
            get
            {
                if (_entities == null)
                    _entities = _context.Set<T>();
                return _entities;
            }
        }

        #endregion
        #region Extension
        public int Count()
        {
            return this.Entities.Count();
        }
        public async Task<int> CountAsync()
        {
            return await this.Entities.CountAsync();
        }
        /// <summary>
        /// ժҪ:
        ///     ����һ��ԭʼ SQL ��ѯ���ò�ѯ�����ش˼��е�ʵ�塣Ĭ������£������Ļ���ٷ��ص�ʵ�壻��ͨ���Է��ص� System.Data.Entity.Infrastructure.DbSqlQuery`1
        ///     ���� AsNoTracking �����Ĵ����á���ע�ⷵ��ʵ�������ʼ���Ǵ˼������ͣ������������������͡������ѯ��һ����������ܰ�������ʵ�����͵����ݣ�������д�ʵ���
        ///     SQL ��ѯ��ȷ��ֻ�����ʵ����͵�ʵ�塣����� SQL ���κ� API һ�������κ��û�������в������Ա���� SQL ע�빥����ʮ����Ҫ�ġ��������� SQL
        ///     ��ѯ�ַ����а�������ռλ����Ȼ�󽫲���ֵ��Ϊ���Ӳ����ṩ�����ṩ���κβ���ֵ�����Զ�ת��Ϊ DbParameter��context.Blogs.SqlQuery("SELECT
        ///     * FROM dbo.Posts WHERE Author = @p0", userSuppliedAuthor); ���ߣ��������Թ���һ�� DbParameter
        ///     �������ṩ�� SqlQuery������������ SQL ��ѯ�ַ�����ʹ������������context.Blogs.SqlQuery("SELECT * FROM
        ///     dbo.Posts WHERE Author = @author", new SqlParameter("@author", userSuppliedAuthor));
        ///
        /// ����:
        ///   sql:
        ///     SQL ��ѯ�ַ�����
        ///
        ///   parameters:
        ///     ҪӦ���� SQL ��ѯ�ַ����Ĳ��������ʹ����������������ǵ�ֵ����ȫ��ȡ���֮ǰ�����á��������� DbDataReader �Ļ�����Ϊ�����µģ��й���ϸ��Ϣ����μ�
        ///    http://go.microsoft.com/fwlink/?LinkID=398589��
        ///
        /// ���ؽ��:
        ///     һ�� System.Data.Entity.Infrastructure.DbSqlQuery`1 ���󣬴˶�����ö��ʱ��ִ�в�ѯ��
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
        /// <returns>����int��string����������</returns>
        public DbRawSqlQuery<TKey> Execute<TKey>(string sqlQuery, params object[] parameters) where TKey : IComparable<TKey>
        {
            #region ����
            //System.Data.SqlClient.SqlParameter[] parameters = {
            // new System.Data.SqlClient.SqlParameter("@i",100),
            // new System.Data.SqlClient.SqlParameter("@j",100),
            //new System.Data.SqlClient.SqlParameter("@he", System.Data.SqlDbType.Int) };
            //parameters[2].Direction = System.Data.ParameterDirection.Output 
            //var slt = this._context.Database.SqlQuery<object>("exec " + storedProcedure, parameters);
            //int AllCount = Int32.Parse(parameters[2].Value.ToString()); 
            #endregion
            var slt = this._context.Database.SqlQuery<TKey>(sqlQuery, parameters);
            return slt;//.ToList(); 
        }
        /// <summary>
        /// ִ�д洢���̷���ʵ�����
        /// </summary>
        /// <typeparam name="TKey">ʵ�����</typeparam>
        /// <param name="sqlQuery">�洢����</param>
        /// <param name="parameters">����</param>
        /// <returns></returns>
        public DbRawSqlQuery<TKey> ExecuteResT<TKey>(string sqlQuery, params object[] parameters) where TKey : class
        {
            var slt = this._context.Database.SqlQuery<TKey>(sqlQuery, parameters);
            return slt;//.ToList(); 

        }
        #endregion
        #region Expression
        /// <summary>
        /// �Ƿ����
        /// </summary>
        /// <param name="whereCondition">���ʽ����</param>
        /// <returns></returns>
        public bool Contains(Expression<Func<T, bool>> whereCondition)
        {
            return this.Entities.Count(whereCondition) > 0;
        }
        /// <summary>
        /// �첽��ʽ
        /// </summary>
        /// <param name="whereCondition"></param>
        /// <returns></returns>
        public async Task<bool> ContainsAsync(Expression<Func<T, bool>> whereCondition)
        {
            return await this.Entities.CountAsync(whereCondition) > 0;
        }
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="whereCondition">���ʽ����</param>
        /// <returns></returns>
        public int Count(Expression<Func<T, bool>> whereCondition)
        {
            return whereCondition != null ? this.Entities.Where(whereCondition).Count() : this.Entities.Count();
        }
        /// <summary>
        /// �첽ȡ����
        /// </summary>
        /// <param name="whereCondition">���ʽ����</param>
        /// <returns></returns>
        public async Task<int> CountAsync(Expression<Func<T, bool>> whereCondition)
        {
            return await (whereCondition != null ? this.Entities.Where(whereCondition).CountAsync() : this.Entities.CountAsync());
        }
        /// <summary>
        /// ȡʵ�����
        /// </summary>
        /// <param name="whereCondition"></param>
        /// <returns></returns>
        public T FindByFeldName(Expression<Func<T, bool>> whereCondition)
        {
            IQueryable<T> query = this.Entities.AsQueryable<T>();
            if (whereCondition != null)
            {
                query = query.Where(whereCondition);
            }
            return query.FirstOrDefault();
        }
        /// <summary>
        /// �첽ȡʵ�����
        /// </summary>
        /// <param name="whereCondition"></param>
        /// <returns></returns>
        public async Task<T> FindByFeldNameAsync(Expression<Func<T, bool>> whereCondition)
        {
            IQueryable<T> query = this.Entities.AsQueryable<T>();
            if (whereCondition != null)
            {
                query = query.Where(whereCondition);
            }
            return await query.FirstOrDefaultAsync();
        }
        /// <summary>
        /// ͨ�����ʽ������ȡ�б�
        /// </summary>
        /// <param name="whereCondition">���ʽ</param>
        /// <param name="orderByExpression">�����������</param>
        /// <returns></returns>
        public IQueryable<T> List(Expression<Func<T, bool>> whereCondition, params IOrderByExpression<T>[] orderByExpression)
        {
            var _resetSet = this.Entities.Where(whereCondition).AsQueryable<T>();
            if (orderByExpression != null && orderByExpression.Length > 0)
            {
                IOrderedQueryable<T> output = null;
                for (int i = 0; i < orderByExpression.Length; i++)
                {
                    if (output == null)
                        output = orderByExpression[i].ApplyOrderBy(_resetSet);
                    else
                        output = orderByExpression[i].ApplyThenBy(output);
                }
                _resetSet = output;
            }
            return _resetSet;
        }
        #endregion
    }
}