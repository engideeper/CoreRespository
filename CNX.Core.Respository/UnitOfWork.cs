using CNX.Core.IRespository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CNX.Core.Respository
{
    public class UnitOfWork<TDbContext> : IUnitOfWork where TDbContext : DbContextBase
    {
        private readonly DbContextBase dbContextBase;

        public UnitOfWork(DbContextBase dbContextBase)
        {
            this.dbContextBase = dbContextBase;
        }

        public DbContextBase GetDbContext()
        {
            return dbContextBase;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await dbContextBase.SaveChangesAsync();
        }
    }
}
