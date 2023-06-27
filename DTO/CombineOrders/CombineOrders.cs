using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.CombineOrders
{
    public class CombineOrders
    {
        public int Id { get; set; }
        public int InspectionId { get; set; }
        public int ProductId { get; set; }
        public int? CombineProductId { get; set; }
        public string ProductName { get; set; }
        public string CombineProductName { get; set; }
        public string ProductDescription { get; set; }
        public int TotalBookingQuantity { get; set; }
        public int? SamplingQuantity { get; set; }
        public int? CombinedAqlQuantity { get; set; }
        public string ColorCode { get; set; }
        public int? AqlLevel { get; set; }
        public string FactoryReference { get; set; }
        public string PoName { get; set; }
        public bool? IsDisplayMaster { get; set; }
        public int? ParentProductId { get; set; }
        public string ParentProductName { get; set; }
        public IEnumerable<PoDetails> PoList { get; set; }
    }


    public class PoDetails
    {
        public int Id { get; set; }
        public int InspectionId { get; set; }
        public int PoId { get; set; }
        public string PoName { get; set; }
        public int BookingQuantity { get; set; }
    }


    public class PoDetailsResponse
    {
        public IEnumerable<PoDetails> PoList { get; set; }      

        public PoDetailsResponseResult Result { get; set; }

    }    

    public enum PoDetailsResponseResult
    {
        success = 1,
        failed = 2
    }

    public class SaveCombineOrdersRequest
    {
        public int Id { get; set; }
        public int InspectionId { get; set; }
        public int? AqlId { get; set; }
        public int ProductId { get; set; }
        public int? CombineProductId { get; set; }
        public int SamplingQuantity { get; set; }
        public int? AqlQuantity { get; set; }
        public int? CombinedAqlQuantity { get; set; }
    }
}
