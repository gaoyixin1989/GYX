// Copyright (c) Microsoft Corporation, Inc. All rights reserved.
// Licensed under the MIT License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace LocalData.Store
{
    /// <summary>
    ///    基于EntityFramework IIdentityEntityStore允许查询/操纵TEntity集
    /// </summary>
    /// <typeparam name="TEntity">具体的实体类型,我。e .User</typeparam>
    internal class EntityStore<TEntity> : IEntityStore<TEntity> where TEntity : class
    {
        /// <summary>
        ///   构造函数接受一个上下文
        /// </summary>
        /// <param name="context"></param>
        public EntityStore(DbContext context)
        {
            Context = context;
            DbEntitySet = context.Set<TEntity>();
        }

        /// <summary>
        ///     Context for the store
        /// </summary>
        public DbContext Context { get; private set; }

        /// <summary>
        ///     用于查询的实体
        /// </summary>
        public IQueryable<TEntity> EntitySet
        {
            get { return DbEntitySet; }
        }

        /// <summary>
        ///    EntitySet这个商店
        /// </summary>
        public DbSet<TEntity> DbEntitySet { get; private set; }

        /// <summary>
        ///     FindAsync一个实体的ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual Task<TEntity> GetByIdAsync(object id)
        {
            return DbEntitySet.FindAsync(id);
        }
        public virtual TEntity GetById(object id)
        {
            return this.DbEntitySet.Find(new object[] { id });
        }


        /// <summary>
        ///     插入一个实体
        /// </summary>
        /// <param name="entity"></param>
        public void Create(TEntity entity)
        {
            DbEntitySet.Add(entity);
        }

        /// <summary>
        ///     马克一个实体来删除
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(TEntity entity)
        {
            DbEntitySet.Remove(entity);
        }

        /// <summary>
        ///     更新一个实体
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Update(TEntity entity)
        {
            if (entity != null)
            {
                Context.Entry(entity).State = EntityState.Modified;
            }
        }
        public virtual TEntity FindByFeldName(Func<TEntity, bool> expfeldName)
        {
            return DbEntitySet.Where(expfeldName).SingleOrDefault();
        }

        /// <summary>
        /// 计算总个数(分页)
        /// </summary>
        /// <param name="exp">Lambda条件的where</param>
        /// <returns></returns>
        public virtual int GetEntitiesCount(Func<TEntity, bool> exp)
        {
            return DbEntitySet.Where(exp).ToList().Count();
        }

        /// <summary>
        /// 分页查询(Linq分页方式)
        /// </summary>
        /// <param name="pageNumber">当前页</param>
        /// <param name="pageSize">页码</param>
        /// <param name="orderName">lambda排序名称</param>
        /// <param name="sortOrder">排序(升序or降序)</param>
        /// <param name="exp">lambda查询条件where</param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> GetEntitiesForPaging(int pageNumber, int pageSize, Func<TEntity, string> orderName, string sortOrder, Func<TEntity, bool> exp)
        {
            if (sortOrder == "asc") //升序排列
            {
                return DbEntitySet.Where(exp).OrderBy(orderName).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return DbEntitySet.Where(exp).OrderByDescending(orderName).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            }
        } 
    }
}