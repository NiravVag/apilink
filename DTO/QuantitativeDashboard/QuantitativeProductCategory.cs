using DTO.ManagementDashboard;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.QuantitativeDashboard
{
    public class QuantitativeProductCategory
    {
    }

    public class QuantitativeProductCategoryChartExport
    {
        public QuantitativeDashboardResult Result { get; set; }
        public List<ProductCategoryDashboardExportItem> Data { get; set; }
        public QuantitativeDashboardRequestExport RequestFilters { get; set; }
    }

    public class ProductCategoryDashboardExportItem
    {
        public int? Id { get; set; }
        public int? SubCategoryId { get; set; }
        public string ProductCategoryName { get; set; }
        public int TotalCount { get; set; }
        public string ProductSubCategoryName { get; set; }
    }
}
