
using GYX.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GYX.Core;

namespace GYX.Data
{
    public class BaseService<T> : IBaseService<T> where T : class
    {
        protected IRepository<T> _entityStore;
        protected int starOrEnd = 0;//
        public BaseService()
        {
            this._entityStore = new EfRepository<T>();
        }
        #region 公共部分
        /// <summary>
        /// 实体数据源
        /// </summary>
        public IQueryable<T> Entitys
        {
            get
            {
                return this._entityStore.Table;
            }
        }
        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="entitys">实体列表</param>
        public virtual bool InsertByList(List<T> entitys)
        {
            return this._entityStore.Insert(entitys);
        }
        /// <summary>
        /// 异步批量插入
        /// </summary>
        /// <param name="entitys">实体列表</param>
        public virtual async Task<bool> InsertByListAsync(List<T> entitys)
        {
            return await this._entityStore.InsertAsync(entitys);
        }
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="entity">实体</param>
        public virtual bool Insert(T entity)
        {
            if (entity == null)
            {
                EntityIsNull();
            }
            return this._entityStore.Insert(entity);

        }

        /// <summary>
        /// 异步插入
        /// </summary>
        /// <param name="entity">实体</param>
        public virtual async Task<bool> InsertAsync(T entity)
        {
            if (entity == null)
            {
                EntityIsNull();
            }
            return await this._entityStore.InsertAsync(entity);
        }

        /// <summary>
        /// 异步删除
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public virtual async Task<bool> DeleteAsync(T entity)
        {
            if (entity == null)
            {
                EntityIsNull();
            }
            return await this._entityStore.DeleteAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public virtual bool Delete(T entity)
        {
            if (entity == null)
            {
                EntityIsNull();
            }
            return this._entityStore.Delete(entity);
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entity">实体列表</param>
        /// <returns></returns>
        public virtual bool DeleteByList(List<T> entitys)
        {
            if (entitys == null)
            {
                EntityIsNull();
            }
            return this._entityStore.Delete(entitys);

        }
        /// <summary>
        /// 异步批量删除
        /// </summary>
        /// <param name="entity">实体列表</param>
        public virtual async Task<bool> DeleteByListAsync(List<T> entitys)
        {
            if (entitys == null)
            {
                EntityIsNull();
            }
            return await this._entityStore.DeleteAsync(entitys);
        }

        //public async Task<T> FindByIdAsync(TKey Id)
        //{
        //    this.ThrowIfDisposed();
        //    return  this._entityStore.FindByFeldNameAsync (f=>f.Id  == Id);
        //}

        /// <summary>
        /// 通过ID查找
        /// </summary>
        /// <param name="entity">实体</param>
        public virtual T FindById(object Id)
        {
            return this._entityStore.GetById(Id);
        }
        public virtual List<T> FindByKeyValues(params object[] keyValues)
        {
            return this._entityStore.GetByKeyValues(keyValues);
        }
        /// <summary>
        /// 异步更新
        /// </summary>
        /// <param name="entity">实体</param>
        public virtual async Task<bool> UpdateAsync(T entity)
        {
            if (entity == null)
            {
                EntityIsNull();
            }
            return await this._entityStore.UpdateAsync(entity);
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">实体</param>
        public virtual bool Update(T entity)
        {
            if (entity == null)
            {
                EntityIsNull();
            }
            return this._entityStore.Update(entity);
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entity">实体列表</param>
        /// <returns></returns>
        public virtual bool UpdateByList(List<T> entitys)
        {
            if (entitys == null)
            {
                EntityIsNull();
            }
            return this._entityStore.Update(entitys);

        }
        /// <summary>
        /// 异步批量删除
        /// </summary>
        /// <param name="entity">实体列表</param>
        public virtual async Task<bool> UpdateByListAsync(List<T> entitys)
        {
            if (entitys == null)
            {
                EntityIsNull();
            }
            return await this._entityStore.UpdateAsync(entitys);
        }

        /// <summary>
        /// 异步通过字段名查找     
        /// </summary>
        /// <param name="fieldName">字体名</param>
        /// <returns></returns>
        public virtual async Task<T> FindByFeldNameAsync(Expression<Func<T, bool>> expfeldName)
        {
            return await this._entityStore.FindByFeldNameAsync(expfeldName);
        }
        /// <summary>
        /// 异步通过字段名查找     
        /// </summary>
        /// <param name="fieldName">字体名</param>
        /// <returns></returns>
        public virtual T FindByFeldName(Expression<Func<T, bool>> expfeldName)
        {
            return this._entityStore.FindByFeldName(expfeldName);
        }
        /// <summary>
        /// 计算总个数(分页)
        /// </summary>
        /// <param name="exp">Lambda条件的where</param>
        /// <returns></returns>
        public virtual int Count(Expression<Func<T, bool>> exp)
        {
            return this._entityStore.Count(exp);
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="total">返回的记录总数</param>
        /// <param name="objs">数据类型的数据（如Int,String,DateTime。目前只做这三种数据类型。若有需要别的类型可再修改）。不应该是一个类对象。其中DateTime类型的时间条件参数为了解决区分开始时间和结束时间问题且开始时间必须以Star结尾，结束时间必须以End结尾。若取等于就不用做这些处理。</param>
        /// <param name="pageIndex">当前页，从0开始</param>
        /// <param name="pageSize">每页显示记录条数</param>
        /// <returns></returns>
        public virtual IEnumerable<dynamic> GetForPaging(out int total, object objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            pageIndex = pageIndex < 0 ? 0 : pageIndex;
            total = this._entityStore.Count();
            if (objs != null)
            {
                var exp = ExpressionFactory(objs);
                if (exp != null)
                {
                    var data = this._entityStore.Table.Where(exp);
                    total = data.Count();
                    return data.OrderBy(p => "").Skip(pageIndex * pageSize).Take(pageSize).ToList();
                }
            }
            return this._entityStore.Table.OrderBy(p => "").Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        public virtual async Task<List<T>> ListAsync()
        {
            return await this._entityStore.Table.ToListAsync();
        }
        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<T> List()
        {
            return this._entityStore.Table;
        }
        #endregion
        #region 表达式树部分
        /// <summary> 
        /// 表达式树工厂
        /// </summary>
        /// <param name="obj">数据类型的数据（如Int,String,DateTime）。不应该是一个类对象。其中DateTime类型的时间条件参数为了解决区分开始时间和结束时间问题且开始时间必须以Star结尾，结束时间必须以End结尾。若取等于就不用做这些处理。</param>
        /// <returns></returns>
        public virtual Expression<Func<T, Boolean>> ExpressionFactory(object obj)
        {
            try
            {
                var objs = obj.GetType().GetProperties();//取到对象所有字段
                if (objs == null && objs.Count() <= 0)
                    return null;
                var p = Expression.Parameter(typeof(T), "t");
                BinaryExpression body = Expression.Equal(Expression.Constant(1), Expression.Constant(1));//这里只是为了拿到body。因为下面要用到body作参数且不能为null。这就好比在ADO.NET中拼条件时初始string where= " 1==1 "
                foreach (var item in objs)//循环条件对象中的字段
                {
                    var value = item.GetValue(obj);
                    if (value == null || string.IsNullOrEmpty(value.ToString()))
                        continue;
                    var pName = item.Name;//字段名
                    var type = value.GetType().Name.ToLower();//类型名

                    string[] typeArr = { "datetime", "int32", "decimal" };
                    if (type == "datetime")//这是为了解决区分开始时间和结束时间问题且开始时间必须以Star结尾，结束时间必须以End结尾。若取等于就不用做这些处理。
                    {
                        starOrEnd = 0;//1-开始时间，2-结束时间，其它-时间等于
                        if (pName.ToLower().EndsWith("_start")) //开始时间
                        {
                            starOrEnd = 1;
                            pName = pName.Substring(0, pName.Length - 6);
                        }
                        else if (pName.ToLower().EndsWith("_end"))//结束时间
                        {
                            starOrEnd = 2;
                            pName = pName.Substring(0, pName.Length - 4);
                        }
                    }
                    var propertyName = Expression.Property(p, pName);
                    body = GetBinaryExpression(value, propertyName, body);

                }
                Expression<Func<T, Boolean>> orExp = Expression.Lambda<Func<T, Boolean>>(body, p);
                return orExp;
            }
            catch (Exception ex)
            {
                return null;
                throw new ArgumentNullException(ex.ToString());
            }
        }
        /// <summary>
        /// 根据不同数据类型取到BinaryExpression
        /// </summary>
        /// <param name="obj">不同数据类型的数据</param>
        /// <param name="member">MemberExpression对象</param>
        /// <param name="body">BinaryExpression对象</param>
        /// <returns></returns>
        private BinaryExpression GetBinaryExpression(object obj, MemberExpression member, BinaryExpression body)
        {
            //字段类型名 
            var type = obj.GetType().Name;
            switch (type.ToLower())
            {

                case "string":                   //String类型时执行以下组合.包含Contains()
                    body = Expression.And(
                        body,
                        Expression.Call(member, typeof(string).GetMethod("Contains", new Type[] { typeof(string) }),
                        Expression.Constant(obj)));
                    break;
                case "guid":                   //String类型时执行以下组合.包含Contains()
                    body = Expression.And(
                        body,
                        Expression.Equal(member, Expression.Constant(obj, member.Type)));
                    break;
                case "boolean":
                    body = Expression.And(
                        body,
                        Expression.Equal(member, Expression.Constant(obj, member.Type)));
                    break;
                case "int32":        
                case "decimal":
                    //int，decimal类型时执行以下组合。 //starOrEnd：1-大于等于，2-小于等于，其它-等于
                    if (starOrEnd == 1)
                        body = Expression.And(
                           body,
                           Expression.GreaterThanOrEqual(member, Expression.Constant(obj, member.Type)));
                    else if (starOrEnd == 2)
                        body = Expression.And(
                           body,
                           Expression.LessThanOrEqual(member, Expression.Constant(obj, member.Type)));
                    else
                        body = Expression.And(
                            body,
                            Expression.Equal(member, Expression.Constant(obj, member.Type)));
                    break;
                case "datetime":                 //Datetime类型时执行以下组合.GreaterThanOrEqual/ LessThanOrEqual 
                    //starOrEnd的值：1-开始时间，2-结束时间，其它-时间等于
                    if (starOrEnd == 1)          //开始时间
                        body = Expression.And(
                            body,
                            Expression.GreaterThanOrEqual(member, Expression.Constant(obj, typeof(DateTime?))));
                    else if (starOrEnd == 2)     //结束时间
                        body = Expression.And(
                            body,
                            Expression.LessThanOrEqual(member, Expression.Constant(obj, typeof(DateTime?))));
                    else                         //取等值
                        body = Expression.And(
                            body,
                            Expression.Equal(member, Expression.Constant(obj, typeof(DateTime?))));
                    break;
                case "list`1":                   //List数组
                                                 //body = Expression.And(
                                                 //    body,
                                                 //    Expression.Call(Expression.Constant(obj), typeof(List<int?>).GetMethod("Contains", new Type[] { typeof(int?) }), member));

                    Type menberListType = Type.GetType(string.Format("System.Collections.Generic.List`1[[{0}]]", member.Type.FullName));
                    body = Expression.And(
                                           body,
                                           Expression.Call(Expression.Constant(obj), menberListType.GetMethod("Contains", new Type[] { member.Type }), member));
                    break;
                default:                        //其他数据类型需要时再加吧 
                    break;
            }
            return body;
        }
        private void EntityIsNull()
        {
            throw new ArgumentNullException(string.Format("实体{0}为Null", typeof(T).Name));
        }
        #endregion

        /// <summary>
        /// 将对象中的字符串类型的属性的前后空白去掉
        /// </summary>
        /// <param name="entity"></param>
        public virtual void TrimObj(T entity)
        {
            var objs = entity.GetType().GetProperties();//取到对象所有字段
            foreach (var item in objs)//循环条件对象中的字段
            {
                var value = item.GetValue(entity);
                if (value != null)
                {
                    string type = value.GetType().Name.ToLower();
                    if (type == "string")
                    {
                        item.SetValue(entity, value.ToString().Trim());
                    }
                }
            }
        }



    }
}
