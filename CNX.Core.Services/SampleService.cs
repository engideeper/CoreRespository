using CNX.Core.IServices;
using CNX.Core.Model;
using System;

namespace CNX.Core.Services
{
    public class SampleService : ISampleService
    {
        public SampleInfo GetSample()
        {
            return new SampleInfo
            {
                ID = 1,
                Name = "Austen"
            };
        }
    }
}
