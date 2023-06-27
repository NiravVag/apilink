using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.CustomerProducts
{
    public class CustomerProductSearchResponse
    {
        public int TotalCount { get; set; }

        public int Index { get; set; }

        public int PageSize { get; set; }

        public int PageCount { get; set; }

        public IEnumerable<CustomerProductSearchData> Data { get; set; }

        public CustomerProductSearchResult Result { get; set; }
    }

    public class CustomerProductSearchData
    {
        public int ID { get; set; }
        public string CustomerName { get; set; }
        public string ProductId { get; set; }
        public string ProductDescription { get; set; }
        public string FactoryReference { get; set; }
        public string ProductCategory { get; set; }
        public string ProductSubCategory { get; set; }
        public string ProductCategorySub2 { get; set; }
        public string ProductCategorySub3 { get; set; }
        public int?  SampleSize8h { get; set; }
        public double? TimePreparation { get; set; }
        public bool? IsBooked { get; set; }
    }
    public class CustomerProductExportDataResponse
    {
        public IEnumerable<CustomerProductExportData> CustomerProductExportData { get; set; }
        public CustomerProductSearchResult Result { get; set; }
    }
    public class CustomerProductExportData
    {
        public int ID { get; set; }
        public string CustomerName { get; set; }
        public string ProductId { get; set; }
        public string ProductDescription { get; set; }
        public string Barcode { get; set; }
        public string FactoryReference { get; set; }
        public string Remarks { get; set; }
        public string ProductCategory { get; set; }
        public string ProductSubCategory { get; set; }
        public string ProductCategorySub2 { get; set; }
        public string ProductCategorySub3 { get; set; }
        public int? SampleSize8h { get; set; }
        public double? TimePreparation { get; set; }
        public string IsNewProduct { get; set; }
    }
    public class CustomerProductRepoExportData
    {
        public int ID { get; set; }
        public int CustomerId { get; set; }
        public int? ProductCategoryId { get; set; }
        public int? ProductSubCategoryId { get; set; }
        public int? ProductCategorySub2Id { get; set; }
        public int? ProductCategorySub3Id { get; set; }

        public string CustomerName { get; set; }
        public string ProductId { get; set; }
        public string ProductDescription { get; set; }
        public string Barcode { get; set; }
        public string FactoryReference { get; set; }
        public string Remarks { get; set; }
        public string ProductCategory { get; set; }
        public string ProductSubCategory { get; set; }
        public string ProductCategorySub2 { get; set; }
        public bool? IsNewProduct { get; set; }
        public bool? IsStyle { get; set; }
        public string ProductCategorySub3 { get; set; }
        public int? SampleSize8h { get; set; }
        public double? TimePreparation { get; set; }
    }
    public enum CustomerProductSearchResult
    {
        Success = 1,
        NotFound = 2

    }
}
