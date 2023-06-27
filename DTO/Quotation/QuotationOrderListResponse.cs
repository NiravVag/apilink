using DTO.Inspection;
using DTO.References;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTO.Quotation
{
    public class QuotationOrderListResponse
    {
        public IEnumerable<Order> Data { get; set;  }

        public OrderListResult  Result { get; set;  }

    }

    public class Order
    {
        public int Id { get; set;  }       
       
        public IEnumerable<ServiceType> ServiceTypeList { get; set;  }

        public string InspectionFrom { get; set; }

        public string InspectionTo { get; set;  }

        public string ServiceTypeStr => ServiceTypeList == null ? "" : string.Join(", ", ServiceTypeList.Select(x => x.Name).ToArray());

        public string Office { get; set;  }

        public int? LocationId { get; set;  }

        public string ProductCategory { get; set; }

        public IEnumerable<QuotProduct> ProductList { get; set;  }

        public IEnumerable<BookingContainersData> ContainerList { get; set; }

        public string InternalBookingRemarks { get; set; }

        public string QcBookingRemarks { get; set; }

        public OrderCost orderCost { get; set; }

        public IEnumerable<QuotationManday> QuotationMandayList { get; set; }

        public int StatusId { get; set; }

        public bool IsContainerServiceType { get; set; }

        public bool? IsPicking { get; set; }

        public string PriceCategoryName { get; set; }

        public List<int> PreviousBookingNo { get; set; }

        public string StatusName { get; set; }

        public string BookingZipFileUrl { get; set; }
        public string PaymentOption { get; set; }
        public List<DynamicFieldData> DynamicFieldData { get; set; }
        public int UserId { get; set; }
    }
    public class OrderCost
    {
        public double? UnitPrice { get; set; }

        public double? NoOfManday { get; set; }

        public double? InspFees { get; set; }

        public double? TravelAir { get; set; }

        public double? TravelLand { get; set; }

        public double? TravelHotel { get; set; }

        public int CustomerPriceCardId { get; set; }

        public double? TravelManday { get; set; }

        public double? TravelDistance { get; set; }

        public double? TravelTime { get; set; }
        public double CalculatedWorkingHours { get; set; }
        public double CalculatedManday { get; set; }
        public int? Quantity { get; set; }
        public int? BilledQtyType { get; set; }
    }
    public class QuotProduct
    {
        public int Id { get; set;  }

        public string ProductId { get; set; }

        public string ProductDescription { get; set; }

        public string AqlLevel { get; set; }
        public string AqlLevelAndSampleType { get; set; }

        public int SampleQty { get; set; }

        public string AqlLevelDescription { get; set; }

        public int BookingQty { get; set; }
        
        public string unitAndUnitCount { get; set; }
        
        public string FactoryReference { get; set; }

        public IEnumerable<QuotProduct> CombineProductList { get; set;  }

        public IEnumerable<int> PictList { get; set;  }

        public  string PoNo { get; set;  }

        public string Destination { get; set; }

        public int InspPoId { get; set;  }

        public string ProductCategory { get; set; }

        public string ProductSubCategory { get; set; }

        public string ProductSubCategory2 { get; set; }

        public string ProductSubCategory3 { get; set; }

        public int? PickingQty { get; set; }

        public string ProductRemarks { get; set; }

        public int CombineProductCount { get; set; }

        public int CombineProductId { get; set; }

        public int CombineAqlQuantity { get; set; }

        public bool IsParentProduct { get; set; }

        public int ProductRefId { get; set; }

        public double? TimePreparation { get; set; }

        public double? CustomerRequirementIndex { get; set; }
        public int? SampleSize8h { get; set; }
        public int UserId { get; set; }
    }


    public enum OrderListResult
    {
        Success = 1, 
        NotFound  = 2
    }
}
