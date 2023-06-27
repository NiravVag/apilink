using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.CustomerProducts
{
    public class ProductCategoryDataSourceRequest
    {
        public string SearchText { get; set; }
        public IEnumerable<int> ProductCategoryIds { get; set; }
        public int ServiceId { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }

    public class ProductSubCategoryDataSourceRequest
    {
        public string SearchText { get; set; }
        public IEnumerable<int> ProductCategoryIds { get; set; }
        public IEnumerable<int> ProductSubCategoryIds { get; set; }
        public int ServiceId { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }

    public class ProductSubCategory2DataSourceRequest
    {
        public string SearchText { get; set; }
        public int ProductCategoryId { get; set; }
        public IEnumerable<int> ProductSubCategoryIds { get; set; }
        public IEnumerable<int> ProductSubCategory2Ids { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }

    public class ProductCategorySourceRepo
    {
        public int ProductCategoryId { get; set; }
        public string ProductCategoryName { get; set; }
        public IEnumerable<int> ServiceIds { get; set; }
    }

    public class ProductSubCategorySourceRepo
    {
        public int ProductCategoryId { get; set; }
        public string ProductSubCategoryName { get; set; }
    }
    public class CustomerProductDataSourceRequest
    {
        public string SearchText { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public IEnumerable<int?> CustomerIds { get; set; }
        public IEnumerable<int?> ProductIds { get; set; }
        public IEnumerable<int?> SupplierIdList { get; set; }
        public IEnumerable<int?> FactoryIdList { get; set; }
    }

    public class CustomerProductDetailsDataSourceRequest
    {
        public string SearchText { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public IEnumerable<int?> CustomerIds { get; set; }
    }

    public class ProductSubCategory3DataSourceRequest
    {
        public string SearchText { get; set; }
        public int ProductCategoryId { get; set; }
        public int ProductSubCategoryId { get; set; }
        public IEnumerable<int> ProductSubCategory3Ids { get; set; }
        public IEnumerable<int> ProductSubCategory2Ids { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}
