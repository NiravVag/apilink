using DTO.CommonClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Schedule
{
    public class QCBlockSummaryModel
    {
        public int OfficeId { get; set; }
        public int QCId { get; set; }
    }

    public class QCBlockSummaryItem
    {
        public string QCName { get; set; }
        public string CustomerNames { get; set; }
        public string SupplierNames { get; set; }
        public string FactoryNames { get; set; }
        public string ProductCategoryNames { get; set; }
        public string ProductCategorySubNames { get; set; }
        public string ProductCategorySub2Names { get; set; }
        public int QCBlockId { get; set; }
    }

    public class QCBlockSummaryResponse
    {
        public List<QCBlockSummaryItem> Data { get; set; }
        public int TotalCount { get; set; }
        public int Index { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public QCBlockResponseResult Result { get; set; }
    }

    public class QCBlockSummaryRepo
    {
        public int QCBlockId { get; set; }
        public string QCName { get; set; }
        public int QCId { get; set; }
        public List<int> OfficeIds { get; set; }     
    }
    public class QcblockSummaryDetailsRepo
    {
        public List<CommonDataSource> CustomerNameList { get; set; }
        public List<CommonDataSource> SupplierList { get; set; }
        public List<CommonDataSource> FactoryList { get; set; }
        public List<CommonDataSource> ProductCategoryList { get; set; }
        public List<CommonDataSource> ProductCateogrySubList { get; set; }
        public List<CommonDataSource> ProductCategorySub2List { get; set; }
    }
    public class QCBlockModelRepo
    {
        public int QCId { get; set; }
        public int Id { get; set; }
        public IEnumerable<int> CustomerIdList { get; set; }
        public IEnumerable<int> SupplierIdList { get; set; }
        public IEnumerable<int> FactoryIdList { get; set; }
        public IEnumerable<int?> ProductCategoryIdList { get; set; }
        public IEnumerable<int?> ProductCateogrySubIdList { get; set; }
        public IEnumerable<int?> ProductCategorySub2IdList { get; set; }
    }
}
