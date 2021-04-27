using CNX.Core.IRespository;
using CNX.Core.IServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CNX.Core.Services
{
  public  class BaseServices<T>:IBaseServices<T> where T : class, new()
    {
        protected IBaseRepository<T> baseRepository;

        //public virtual DbContext DbContext { get; set; }


        public BaseServices(IBaseRepository<T> baseRepository)
        {

            this.baseRepository = baseRepository;
        }
        public  async Task<T> QueryById(int Id) {

            return await baseRepository.QueryById(Id);
        }

    }
}
