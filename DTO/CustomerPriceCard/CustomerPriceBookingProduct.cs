using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.CustomerPriceCard
{
    public class CustomerPriceBookingProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SamplingSize { get; set; }
        public double BookingQuantity { get; set; }
        public int? SubCategory2Id { get; set; }
    }

    public class CustomerPriceBookingProductRepo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? CombineProductId { get; set; }
        public int? AQLQuantity { get; set; }
        public int? UnitCount { get; set; }
        public int? CombinedAQLQuantity { get; set; }
    }

    public class CustomerPriceBookingProducts
    {
        public int ProductId { get; set; }
        public int BookingId { get; set; }
        public double BookingQuantity { get; set; }
        public bool? IsDispalyMaster { get; set; }
        public int? ParentProductId { get; set; }
        public string ProductName { get; set; }
        public int? CombineProductId { get; set; }
        public int? AQLQuantity { get; set; }
        public double? PresentedQuantity { get; set; }
        public double? InspectedQuanity { get; set; }
        public int? UnitCount { get; set; }
        public int UnitId { get; set; }
        public int? CombinedAQLQuantity { get; set; }
        public string CusbookingNumber { get; set; }
        public int? FormSerialNumber { get; set; }
        public int? SubCategoryId { get; set; }
        public int? SubCategory2Id { get; set; }
    }

    public class CustomerPriceCardBookingRepo
    {
        public int Id { get; set; }
        public DateTime ServiceDateFrom { get; set; }
        public DateTime ServiceDateTo { get; set; }
        public int? LocationId { get; set; }
    }

    public enum BillingMethodEnum
    {
        ManDay = 1,
        Sampling = 2,
        PieceRate = 3,
        PerIntervention = 4
    }

    public enum CustomerPriceHolidayInfoEnum
    {
        Sun = 1,
        Mon = 2,
        Tue = 3,
        Wed = 4,
        Thu = 5,
        Fri = 6,
        Sat = 7,
        PublicHoliday = 8
    }




}
