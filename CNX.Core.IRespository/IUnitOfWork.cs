﻿//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace CNX.Core.IRespository
//{
//    public interface IUnitOfWork: IDisposable
//    {

//        /// <summary>
//        /// 是否提交
//        /// </summary>
//        bool HasCommit();


//        /// <summary>
//        /// 释放时触发
//        /// </summary>
//        Action OnDispose { get; set; }

//        /// <summary>
//        /// 得到上下文
//        /// </summary>
//        /// <returns></returns>
//        DbContext GetDbContext();

//        /// <summary>
//        /// 开启事务
//        /// </summary>
//        void BeginTransaction();

//        /// <summary>
//        /// 提交事务
//        /// </summary>
//        void Commit();

//        /// <summary>
//        /// 开启异步事务
//        /// </summary>
//        /// <param name="cancellationToken"></param>
//        /// <returns></returns>
//        Task BeginTransactionAsync(CancellationToken cancellationToken = default(CancellationToken));

//        /// <summary>
//        /// 回滚事务
//        /// </summary>
//        void Rollback();


//        /// <summary>
//        /// 异步提交事务
//        /// </summary>
//        /// <returns></returns>
//        Task CommitAsync();

//        /// <summary>
//        /// 异步回滚
//        /// </summary>
//        /// <returns></returns>
//        Task RollbackAsync();

//        void Push();

//        void Pop();
//    }
//}
