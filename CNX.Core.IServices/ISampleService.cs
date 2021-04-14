using CNX.Core.Model;
using System;
using System.Threading.Tasks;

namespace CNX.Core.IServices
{
    public interface ISampleService: IBaseServices<SampleInfo>
    {


        Task<SampleInfo> GetSampleById(int id);

    }
       
}
