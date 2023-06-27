using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.CombineOrders
{
    public class SamplingProductListResponse
    {
        public IEnumerable<SamplingInfo> SamplingDataList { get; set; }

        public SamplingProductLisResponseResult Result { get; set; }

    }

    public class SamplingInfo
    {
        public int ProductId { get; set; }

        public int SamplingQuantity { get; set; }
    }

    public enum SamplingProductLisResponseResult
    {
        success = 1,
        failed = 2
    }


    public class CombineSamplingProductListResponse
    {
        public List<CombineOrderSamplingData> SamplingDataList { get; set; }

        public CombineSamplingProductListResult Result { get; set; }

    }

    public class CombineSamplingInfo
    {
        public int ProductId { get; set; }

        public int PoId { get; set; }

        public int SamplingQuantity { get; set; }
    }

    public enum CombineSamplingProductListResult
    {
        success = 1,
        failed = 2,
        reportExist = 3
    }
}

public class AqlProductListResponse
{
    public IEnumerable<AqlInfo> AqlDataList { get; set; }
    public AqlProductLisResponseResult Result { get; set; }
}

public class AqlInfo
{
    public int ProductId { get; set; }
    public int PoId { get; set; }
    public int? CombineProductId { get; set; }
    public int? AqlQuantity { get; set; }
}

public enum AqlProductLisResponseResult
{
    success = 1,
    failed = 2
}


