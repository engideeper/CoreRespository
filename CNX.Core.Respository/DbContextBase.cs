using CNX.Core.IRespository;
using CNX.Core.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CNX.Core.Respository
{
    public class DbContextBase : DbContext
    {

        protected readonly IServiceProvider _serviceProvider = null;
        public DbContextBase(DbContextOptions options, IServiceProvider serviceProvider):base(options)
        {
            _serviceProvider = serviceProvider;

        }




        public IUnitOfWork UnitOfWork { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //var entityTypes = AssemblyHelper
            //                    .GetTypesFromAssembly()
            //                    .Where(type =>
            //                        !type.Namespace.IsNullOrWhiteSpace() &&
            //                        type.GetTypeInfo().IsClass &&
            //                        type.GetTypeInfo().BaseType != null &&
            //                        type != typeof(BaseEntity) &&
            //                        typeof(BaseEntity).IsAssignableFrom(type));

            //if (entityTypes?.Count() > 0)
            //{
            //    foreach (var entityType in entityTypes)
            //    {
            //        if (modelBuilder.Model.FindEntityType(entityType) != null)
            //            continue;
            //        modelBuilder.Model.AddEntityType(entityType);
            //    }
            //}
            #region 定义各个表格的名称和类型
            //Blog类要映射到数据库内的Blog表格，默认是blogs
            var blogTable = modelBuilder.Entity<SampleInfo>().ToTable("SampleInfo");
             #endregion







        }

        /// <summary>
        /// 异步保存方法
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            int count = await base.SaveChangesAsync(cancellationToken);
            return count;
        }


        /// <summary>
        /// 保存更改
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            int count = base.SaveChanges();
            return count;
        }


    }
}
