using AutoMapper;
using CNX.Core.IRespository;
using CNX.Core.IServices;
using CNX.Core.Model;
using CNX.Core.Model.Dto;
using System;
using System.Threading.Tasks;

namespace CNX.Core.Services
{
    public class SampleService : BaseServices<SampleInfo>, ISampleService
    {
        private readonly IMapper _mapper;

        public SampleService(IBaseRepository<SampleInfo> baseRepository,IMapper mapper):base(baseRepository)
        {
            this.baseRepository = baseRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// 通过ID获取样本记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<SampleInfo> GetSampleById(int id)
        {
            SampleInfo model = new SampleInfo();
            var sampleInfo = await baseRepository.QueryById(id);
            model = sampleInfo;
            return model;

        }


        /// <summary>
        /// autoMapper演示
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<SampleInfoView> GetSampleById_View(int id)
        {
            SampleInfoView model = new SampleInfoView();
            var sampleInfo = await baseRepository.QueryById(id);
            model = _mapper.Map<SampleInfoView>(sampleInfo);
            return model;

        }


        /// <summary>
        /// 添加样本信息
        /// </summary>
        /// <param name="sampleInfo"></param>
        /// <returns></returns>
        public async Task<SampleInfo> AddSampleInfo(SampleInfo sampleInfo)
        {
            return await baseRepository.AysAdd(sampleInfo);
        }
    }
}
