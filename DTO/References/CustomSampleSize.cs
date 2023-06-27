using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.References
{
    public class CustomSampleSize
    {
        public int Id { get; set; }
        public string SampleType { get; set; }
        public string SampleSize { get; set; }
    }


    public enum CustomSampleSizeResult
    {
        Success = 1,
        CannotGetList = 2,
        Failed = 3
    }
    public class CustomSampleSizeResponse
    {
        public IEnumerable<CustomSampleSize> DataSourceList { get; set; }
        public CustomSampleSizeResult Result { get; set; }
    }

}
