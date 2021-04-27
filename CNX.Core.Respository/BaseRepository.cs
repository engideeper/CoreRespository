using CNX.Core.IRespository;
using CNX.Core.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
//using System.Transactions;

namespace CNX.Core.Respository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly DbContextBase _dbContextBase = null;
        private readonly DbSet<T> _dbSet;

        public BaseRepository(DbContextBase _dbContextBase)
        {
            this._dbContextBase = _dbContextBase;
            if (_dbContextBase == null)
            {
                return;
            }
            _dbSet = _dbContextBase.Set<T>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public virtual async Task<T> QueryById(int Id)
        {
            return await _dbContextBase.Set<T>().FindAsync(Id);
        }

        /// <summary>
        /// 显式开启数据上下文事务
        /// </summary>
        /// <param name="isolationLevel">指定连接的事务锁定行为</param>
        public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified)
        {
            if (_dbContextBase.Database.CurrentTransaction == null)
            {
                _dbContextBase.Database.BeginTransaction(isolationLevel);
            }
        }
        /// <summary>
        /// 提交事务的更改
        /// </summary>
        public void Commit()
        {
            var transaction = this._dbContextBase.Database.CurrentTransaction;
            if (transaction != null)
            {
                try
                {
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
        /// <summary>
        /// 显式回滚事务，仅在显式开启事务后有用
        /// </summary>
        public void Rollback()
        {
            if (_dbContextBase.Database.CurrentTransaction != null)
            {
                _dbContextBase.Database.CurrentTransaction.Rollback();
            }
        }
        /// <summary>
        /// 提交当前单元操作的更改
        /// </summary>
        public int SaveChanges()
        {
            return _dbContextBase.SaveChanges();
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _dbContextBase.SaveChangesAsync();
        }

        /// <summary>
        /// 获取 当前实体类型的查询数据集，数据将使用不跟踪变化的方式来查询，当数据用于展现时，推荐使用此数据集，如果用于新增，更新，删除时，请使用<see cref="TrackEntities"/>数据集
        /// </summary>
        public IQueryable<T> Entities
        {
            get { return _dbSet.AsNoTracking(); }
        }
        /// <summary>
        /// 获取 当前实体类型的查询数据集，当数据用于新增，更新，删除时，使用此数据集，如果数据用于展现，推荐使用<see cref="Entities"/>数据集
        /// </summary>
        public IQueryable<T> TrackEntities
        {
            get { return _dbSet; }
        }
        /// <summary>
        /// 插入 - 通过实体对象添加
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="isSave">是否执行</param>
        /// /// <returns></returns>
        public T Add(T entity, bool isSave = true)
        {

            _dbSet.Add(entity);
            if (isSave)
            {
                SaveChanges();
            }
            return entity;
        }

        public async Task<T> AysAdd(T entity, bool isSave = true)
        {
            await _dbSet.AddAsync(entity);
            if (isSave)
            {
                await SaveChangesAsync();
            }
            return entity;
        }
        /// <summary>
        /// 批量插入 - 通过实体对象集合添加
        /// </summary>
        /// <param name="entitys">实体对象集合</param>
        /// <param name="isSave">是否执行</param>
        public void AddRange(IEnumerable<T> entitys, bool isSave = true)
        {
            _dbSet.AddRange(entitys);
            if (isSave)
            {
                SaveChanges();
            }
        }
        /// <summary>
        /// 删除 - 通过实体对象删除
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="isSave">是否执行</param>
        public void Delete(T entity, bool isSave = true)
        {
            this._dbSet.Remove(entity);
            if (isSave)
            {
                this.SaveChanges();
            }
        }
        /// <summary>
        /// 批量删除 - 通过实体对象集合删除
        /// </summary>
        /// <param name="entitys">实体对象集合</param>
        /// <param name="isSave">是否执行</param>
        public void Delete(bool isSave = true, params T[] entitys)
        {
            this._dbSet.RemoveRange(entitys);
            if (isSave)
            {
                this.SaveChanges();
            }
        }
        /// <summary>
        /// 删除 - 通过主键ID删除
        /// </summary>
        /// <param name="id">主键ID</param>
        public async Task<int> AsyDelete(object id)
        {
            int de = 0;
            //执行查询
            var todoItem = await _dbSet.FindAsync(id);
            if (todoItem == null)
            {
                //return NotFound();
                de = 0;
            }
            else
            {
                _dbSet.Remove(todoItem);
                de = SaveChanges();
            }
            return de;
        }

        public int Delete(object id)
        {
            int de = 0;
            //执行查询
            var todoItem = _dbSet.Find(id);
            if (todoItem == null)
            {
                //return NotFound();
                de = 0;
            }
            else
            {
                _dbSet.Remove(todoItem);
                de = SaveChanges();
            }
            return de;
        }
        /// <summary>
        /// 批量删除 - 通过条件删除
        /// </summary>
        /// <param name="where">过滤条件</param>
        /// <param name="isSave">是否执行</param>
        public void Delete(Expression<Func<T, bool>> @where, bool isSave = true)
        {
            T[] entitys = this._dbSet.Where(@where).ToArray();
            if (entitys.Length > 0)
            {
                this._dbSet.RemoveRange(entitys);
            }
            if (isSave)
            {
                this.SaveChanges();
            }
        }
        /// <summary>
        /// 修改 - 通过实体对象修改
        /// </summary>
        /// <param name="entity">实体对象</param>
        public async Task<int> AysUpdate(T entity)
        {

            var entry = this._dbContextBase.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                entry.State = EntityState.Modified;
            }
            var da = await Task.Run(SaveChangesAsync);
            return da;
        }

        public int Update(T entity)
        {

            var entry = this._dbContextBase.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                entry.State = EntityState.Modified;
            }
            var da = SaveChanges();
            return da;
        }
        /// <summary>
        /// 批量修改 - 通过实体对象集合修改
        /// </summary>
        /// <param name="entitys">实体对象集合</param>
        public void Update(params T[] entitys)
        {
            var entry = this._dbContextBase.Entry(entitys);
            if (entry.State == EntityState.Detached)
            {
                entry.State = EntityState.Modified;
            }
            SaveChanges();
        }
        /// <summary>
        /// 是否满足条件
        /// </summary>
        /// <param name="where">过滤条件</param>
        /// <returns></returns>
        public bool Any(Expression<Func<T, bool>> @where)
        {
            return this._dbSet.AsNoTracking().Any(@where);
        }
        /// <summary>
        /// 返回总条数
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return this._dbSet.AsNoTracking().Count();
        }
        /// <summary>
        /// 返回总条数 - 通过条件过滤
        /// </summary>
        /// <param name="where">过滤条件</param>
        /// <returns></returns>
        public int Count(Expression<Func<T, bool>> @where)
        {
            return this._dbSet.AsNoTracking().Count(@where);
        }
        /// <summary>
        /// 返回第一条记录
        /// </summary>
        /// <param name="where">过滤条件</param>
        /// <returns></returns>
        public T FirstOrDefault(Expression<Func<T, bool>> @where)
        {
            return this._dbSet.AsNoTracking().FirstOrDefault(@where);
        }
        /// <summary>
        /// 返回第一条记录 - 通过条件过滤
        /// </summary>
        /// <typeparam name="TOrder">排序约束</typeparam>
        /// <param name="where">过滤条件</param>
        /// <param name="order">排序条件</param>
        /// <param name="isDesc">排序方式</param>
        /// <returns></returns>
        public T FirstOrDefault<TOrder>(Expression<Func<T, bool>> @where, Expression<Func<T, TOrder>> order, bool isDesc = false)
        {
            if (isDesc)
            {
                return this._dbSet.AsNoTracking().OrderByDescending(order).FirstOrDefault(@where);
            }
            else
            {
                return this._dbSet.AsNoTracking().OrderBy(order).FirstOrDefault(@where);
            }
        }
        /// <summary>
        /// 去重查询
        /// </summary>
        /// <param name="where">过滤条件</param>
        /// <returns></returns>
        public IQueryable<T> Distinct(Expression<Func<T, bool>> @where)
        {
            return this._dbSet.AsNoTracking().Where(@where).Distinct();
        }
        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="where">过滤条件</param>
        /// <returns></returns>
        public IQueryable<T> Where(Expression<Func<T, bool>> @where)
        {
            return this._dbSet.Where(@where);
        }
        /// <summary>
        /// 条件查询 - 支持排序
        /// </summary>
        /// <typeparam name="TOrder">排序约束</typeparam>
        /// <param name="where">过滤条件</param>
        /// <param name="order">排序条件</param>
        /// <param name="isDesc">排序方式</param>
        /// <returns></returns>
        public IQueryable<T> Where<TOrder>(Expression<Func<T, bool>> @where, Expression<Func<T, TOrder>> order, bool isDesc = false)
        {
            if (isDesc)
            {
                return this._dbSet.Where(@where).OrderByDescending(order);
            }
            else
            {
                return this._dbSet.Where(@where).OrderBy(order);
            }
        }
        /// <summary>
        /// 条件分页查询 - 支持排序
        /// </summary>
        /// <typeparam name="TOrder">排序约束</typeparam>
        /// <param name="where">过滤条件</param>
        /// <param name="order">排序条件</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页记录条数</param>
        /// <param name="count">返回总条数</param>
        /// <param name="isDesc">是否倒序</param>
        /// <returns></returns>
        public IEnumerable<T> Where<TOrder>(Func<T, bool> @where, Func<T, TOrder> order, int pageIndex, int pageSize, out int count, bool isDesc = false)
        {
            count = Count();
            if (isDesc)
            {
                return this._dbSet.Where(@where).OrderByDescending(order).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
            else
            {
                return this._dbSet.Where(@where).OrderBy(order).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
        }
        /// <summary>
        /// 条件分页查询 - 支持排序 - 支持Select导航属性查询
        /// </summary>
        /// <typeparam name="TOrder">排序约束</typeparam>
        /// <param name="where">过滤条件</param>
        /// <param name="order">排序条件</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页记录条数</param>
        /// <param name="count">返回总条数</param>
        /// <param name="isDesc">是否倒序</param>
        /// <returns></returns>
        public IQueryable<T> Where<TOrder>(Expression<Func<T, bool>> @where, Expression<Func<T, TOrder>> order, int pageIndex, int pageSize, out int count, bool isDesc = false)
        {
            count = Count();
            if (isDesc)
            {
                return this._dbSet.Where(@where).OrderByDescending(order).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
            else
            {
                return this._dbSet.Where(@where).OrderBy(order).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
        }
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> GetAll()
        {
            return this._dbSet.AsNoTracking();
        }
        /// <summary>
        /// 获取所有数据 - 支持排序
        /// </summary>
        /// <typeparam name="TOrder">排序约束</typeparam>
        /// <param name="order">排序条件</param>
        /// <param name="isDesc">排序方式</param>
        /// <returns></returns>
        public IQueryable<T> GetAll<TOrder>(Expression<Func<T, TOrder>> order, bool isDesc = false)
        {
            if (isDesc)
            {
                return this._dbSet.AsNoTracking().OrderByDescending(order);
            }
            else
            {
                return this._dbSet.AsNoTracking().OrderBy(order);
            }
        }
        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <typeparam name="TType">字段类型</typeparam>
        /// <param name="id">主键ID</param>
        /// <returns></returns>
        public T GetById<TType>(TType id)
        {
            return this._dbSet.Find(id);
        }
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <typeparam name="TType">字段类型</typeparam>
        /// <param name="column">字段条件</param>
        /// <returns></returns>
        public TType Max<TType>(Expression<Func<T, TType>> column)
        {
            if (this._dbSet.AsNoTracking().Any())
            {
                return this._dbSet.AsNoTracking().Max(column);
            }
            return default(TType);
        }
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <typeparam name="TType">字段类型</typeparam>
        /// <param name="column">字段条件</param>
        /// <param name="where">过滤条件</param>
        /// <returns></returns>
        public TType Max<TType>(Expression<Func<T, TType>> column, Expression<Func<T, bool>> @where)
        {
            if (this._dbSet.AsNoTracking().Any(@where))
            {
                return this._dbSet.AsNoTracking().Where(@where).Max(column);
            }
            return default(TType);
        }
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <typeparam name="TType">字段类型</typeparam>
        /// <param name="column">字段条件</param>
        /// <returns></returns>
        public TType Min<TType>(Expression<Func<T, TType>> column)
        {
            if (this._dbSet.AsNoTracking().Any())
            {
                return this._dbSet.AsNoTracking().Min(column);
            }
            return default(TType);
        }
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <typeparam name="TType">字段类型</typeparam>
        /// <param name="column">字段条件</param>
        /// <param name="where">过滤条件</param>
        /// <returns></returns>
        public TType Min<TType>(Expression<Func<T, TType>> column, Expression<Func<T, bool>> @where)
        {
            if (this._dbSet.AsNoTracking().Any(@where))
            {
                return this._dbSet.AsNoTracking().Where(@where).Min(column);
            }
            return default(TType);
        }
        /// <summary>
        /// 获取总数
        /// </summary>
        /// <typeparam name="TType">字段类型</typeparam>
        /// <param name="selector">字段条件</param>
        /// <param name="where">过滤条件</param>
        /// <returns></returns>
        public TType Sum<TType>(Expression<Func<T, TType>> selector, Expression<Func<T, bool>> @where) where TType : new()
        {
            object result = 0;

            if (new TType().GetType() == typeof(decimal))
            {
                result = this._dbSet.AsNoTracking().Where(where).Sum(selector as Expression<Func<T, decimal>>);
            }
            if (new TType().GetType() == typeof(decimal?))
            {
                result = this._dbSet.AsNoTracking().Where(where).Sum(selector as Expression<Func<T, decimal?>>);
            }
            if (new TType().GetType() == typeof(double))
            {
                result = this._dbSet.AsNoTracking().Where(where).Sum(selector as Expression<Func<T, double>>);
            }
            if (new TType().GetType() == typeof(double?))
            {
                result = this._dbSet.AsNoTracking().Where(where).Sum(selector as Expression<Func<T, double?>>);
            }
            if (new TType().GetType() == typeof(float))
            {
                result = this._dbSet.AsNoTracking().Where(where).Sum(selector as Expression<Func<T, float>>);
            }
            if (new TType().GetType() == typeof(float?))
            {
                result = this._dbSet.AsNoTracking().Where(where).Sum(selector as Expression<Func<T, float?>>);
            }
            if (new TType().GetType() == typeof(int))
            {
                result = this._dbSet.AsNoTracking().Where(where).Sum(selector as Expression<Func<T, int>>);
            }
            if (new TType().GetType() == typeof(int?))
            {
                result = this._dbSet.AsNoTracking().Where(where).Sum(selector as Expression<Func<T, int?>>);
            }
            if (new TType().GetType() == typeof(long))
            {
                result = this._dbSet.AsNoTracking().Where(where).Sum(selector as Expression<Func<T, long>>);
            }
            if (new TType().GetType() == typeof(long?))
            {
                result = this._dbSet.AsNoTracking().Where(where).Sum(selector as Expression<Func<T, long?>>);
            }
            return (TType)result;
        }

        public void Dispose()
        {
            this._dbContextBase.Dispose();
        }

    }


}
