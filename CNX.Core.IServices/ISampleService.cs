using CNX.Core.Model;
using System;
using System.Threading.Tasks;

namespace CNX.Core.IServices
{
    public interface ISampleService: IBaseServices<SampleInfo>
    {
        /// <summary>
        /// 通过ID获取样本记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SampleInfo> GetSampleById(int id);

        Task<SampleInfo> AddSampleInfo(SampleInfo sampleInfo);

    }
       
}
