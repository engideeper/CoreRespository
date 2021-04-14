using CNX.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CNX.Core.IServices
{
   public interface IBaseServices<TEntity> where TEntity : class
    {
        Task<TEntity> QueryById(object objId);
    }
}
