using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CNX.Core.IRespository
{
    public interface IUnitOfWork 
    {
        //DbContextBase.GetDbContext();

        Task<int> SaveChangesAsync();

    }
}
