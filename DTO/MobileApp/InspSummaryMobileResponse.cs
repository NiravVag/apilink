using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.MobileApp
{
    public class InspSummaryMobileResponse
    {
        public MobileResult meta { get; set; }
        public List<MobileInspectionData> data { get; set; }
    }

    public class MobileInspectionData
    {
        public int key { get; set; }
        public int inspectionId { get; set; }
        public int supplierId { get; set; }
        public string supplierName { get; set; }
        public int factoryId { get; set; }
        public string factoryName { get; set; }
        public int serviceTypeId { get; set; }
        public string serviceTypeName { get; set; }
        public string serviceDate { get; set; }
        public int reportCount { get; set; }
        public int bookingStatusId { get; set; }
        public string bookingStatusName { get; set; }
        public bool isInspected { get; set; }
        public SvgColor statusColor { get; set; }
    }

    public class FilterDataSourceResponse
    {
        public List<FilterDataSource> data { get; set; }
        public MobileResult meta { get; set; }
    }

    public class FilterDataSource
    {
        public int key { get; set; }
        public int id { get; set; }
        public string name { get; set; }
    }

    public class CommonFilterListResponse
    {
        public CommonFilterList data { get; set; }
        public MobileResult meta { get; set; }
    }

    public class CommonFilterList
    {
        public List<FilterDataSource> statusList { get; set; }
        public List<FilterDataSource> serviceTypeList { get; set; }
    }

}

