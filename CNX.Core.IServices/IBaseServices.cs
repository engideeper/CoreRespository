using CNX.Core.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CNX.Core.IServices
{
   public interface IBaseServices<T> where T : class, new()
    {


        //DbContext DbContext { get; set; }
       Task<T> QueryById(int Id);

    }
}
