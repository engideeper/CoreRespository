using AutoMapper;
using CNX.Core.Model;
using CNX.Core.Model.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CNX.Core.Common.AutoMapper
{
   public class CustomProfile: Profile
    {
        /// <summary>
        /// 配置构造函数，用来创建关系映射
        /// </summary>
        public CustomProfile()
        {
            CreateMap<SampleInfo, SampleInfoView>().ForMember(d=>d.SampleName,o=>o.MapFrom(s=>s.Name));
            CreateMap<SampleInfoView, SampleInfo>().ForMember(d=>d.Name,o=>o.MapFrom(s=>s.SampleName));
        }


    }
}
