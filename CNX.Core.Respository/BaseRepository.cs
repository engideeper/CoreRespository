using CNX.Core.IRespository;
using CNX.Core.Respository.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CNX.Core.Respository
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, new()
    {
        private readonly DbSet<TEntity> _dbSet = null;

        private readonly DbContext _dbContext = null;

        public virtual IUnitOfWork UnitOfWork { get; }
        public BaseRepository(IServiceProvider serviceProvider)
        {
            UnitOfWork = (serviceProvider.GetService(typeof(IUnitOfWork)) as IUnitOfWork);
            _dbContext = UnitOfWork.GetDbContext();
            _dbSet = _dbContext.Set<TEntity>();

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objId"></param>
        /// <returns></returns>
        public async Task<TEntity> QueryById(object objId)
        {
            //return await Task.Run(() => _db.Queryable<TEntity>().InSingle(objId));
            return await _dbSet.FindAsync(objId);
        }


    }


}
