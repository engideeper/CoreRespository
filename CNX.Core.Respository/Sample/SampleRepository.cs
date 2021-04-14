using CNX.Core.Model;
using CNX.Core.IRespository;
using System;
using System.Collections.Generic;
using System.Text;
using CNX.Core.IRespository.ISampleResponsitory;

namespace CNX.Core.Respository
{
   public class SampleRepository : BaseRepository<SampleInfo>, ISampleReposiotry
    {

        public SampleRepository(DbContextBase dbContextBase) : base(serviceProvider)
        {
        }
    }
}
