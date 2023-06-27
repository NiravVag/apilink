using DTO.Common;
using DTO.CommonClass;
using DTO.CustomerProducts;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.PurchaseOrder
{
    public class PurchaseOrderUploadResponse
    {
        public IEnumerable<PurchaseOrderUpload> PurchaseOrderUploadList { get; set; }
        public PurchaseOrderUploadResult Result { get; set; }
    }

    public enum PurchaseOrderUploadResult
    {
        Success = 1,
        NotAbleToProcess = 2,
        GivenDateFormatIsWrong = 3
    }

    public class POProductUploadResponse
    {
        public List<PoProductUploadSuccessData> PurchaseOrderSuccessList { get; set; }
        public List<POProductUploadData> PoProductUploadErrorList { get; set; }
        public POProductUploadResult Result { get; set; }
    }

    public class PoProductUploadSuccessData
    {
        public List<CommonDataSource> PoData { get; set; }
        public List<CustomerProductDetail> ProductData { get; set; }
        public int? DestinationCountryId { get; set; }
        public string ProductDescription { get; set; }
        public DateObject Etd { get; set; }
        public int Quantity { get; set; }
        public string ColorCode { get; set; }
        public string ColorName { get; set; }
    }

    public enum POProductUploadResult
    {
        Success = 1,
        NotAbleToProcess = 2,
        ValidationError=3,
        EmptyRows=4
    }

    public class PoProductErrorResultData
    {
        public string PoNumber { get; set; }
        public string ProductReference { get; set; }
        public string ProductDescription { get; set; }
        public string Etd { get; set; }
        public string DestinationCountry { get; set; }
        public int Quantity { get; set; }
    }

    public class CountryMasterData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }

}
