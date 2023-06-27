using DTO.Manday;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.QuantitativeDashboard
{
    public class OrderQuantityCountChartExport
    {
        public IEnumerable<OrderQtyChartItem> Data { get; set; }
        public int Total { get; set; }
        public QuantitativeDashboardResult Result { get; set; }
        public QuantitativeDashboardRequestExport RequestFilters { get; set; }
    }

    public class PiecesInspectedChartResponse
    {
        public IEnumerable<OrderQuantityYearChart> Data { get; set; }
        public IEnumerable<MandayYear> MonthYearXAxis { get; set; }
        public QuantitativeDashboardResult Result { get; set; }
    }

    public class OrderQtyListCurrenctPrevious
    {
        public List<OrderQtyChartItem> CurrentOrderQtyList { get; set; }
        public List<OrderQtyChartItem> PreviousOrderQtyList { get; set; }
    }

    public class OrderQuantityYearChart
    {
        public int Year { get; set; }
        public int OrderCount { get; set; }
        public string Color { get; set; }
        public double Percentage { get; set; }
        public IEnumerable<OrderQtyChartItem> MonthlyData { get; set; }
    }

    public class OrderQtyChartItem
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public int MonthOrderQuantity { get; set; }
        public string MonthName { get; set; }
    }
}
