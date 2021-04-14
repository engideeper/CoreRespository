using CNX.Core.IRespository;
using CNX.Core.IServices;
using CNX.Core.Model;
using System;
using System.Threading.Tasks;

namespace CNX.Core.Services
{
    public class SampleService : BaseServices<SampleInfo>, ISampleService
    {
        IBaseRepository<SampleInfo> _dal;
        public async Task<SampleInfo> GetSampleById(int id)
        {

            SampleInfo model = new SampleInfo();
            var sampleInfo = await base.QueryById(id);
            model = sampleInfo;
            return model;

        }
    }
}
