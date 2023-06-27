using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Schedule
{
    public class ActualManday
    {
        public int QcId { get; set; }
        public DateTime ServiceDate { get; set; }
        public int CustomerId { get; set; }
        public int? FactoryId { get; set; }
        public int TotalBooking { get; set; }
    }

    public class ScheduleProductData
    {
        public int CuProductId { get; set; }
        public string ProductId { get; set; }
        public int OrderQty { get; set; }
        public List<string> POList { get; set; }
        public bool IsMSChart { get; set; }
    }

    public class SchedulePOData
    {
        public int CuProductId { get; set; }
        public string PONumber { get; set; }
    }

    public class ScheduleProductModel
    {
        public string ProductId { get; set; }
        public int OrderQty { get; set; }
        public string PONumber { get; set; }
        public string MSChart { get; set; }
        public int CuProductId { get; set; }
    }

    public class ScheduleProductModelResponse
    {
        public List<ScheduleProductModel> ScheduleProductModel { get; set; }
        public SaveScheduleResponseResult Result { get; set; }
    }
    public class ScheduleQcCustomerFactory
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int? FactoryId { get; set; }
        public int QcId { get; set; }
    }
}
