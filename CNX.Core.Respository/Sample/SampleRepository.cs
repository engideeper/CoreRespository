using CNX.Core.Model;
using CNX.Core.IRespository;
using System;
using System.Collections.Generic;
using System.Text;
using CNX.Core.IRespository.ISampleResponsitory;
using Microsoft.EntityFrameworkCore;

namespace CNX.Core.Respository
{
   public class SampleRepository : BaseRepository<SampleInfo>, ISampleReposiotry
    {

        public SampleRepository(DbContextBase dbContextBase) :base(dbContextBase) { 
        
        }
       
    }
}
