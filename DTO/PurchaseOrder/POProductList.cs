using DTO.Common;
using DTO.CommonClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.PurchaseOrder
{
    public class POProductListResponse
    {
        public List<POProductList> ProductList { get; set; }
        public POProductResult Result { get; set; }
    }

    public class POProductList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int PoQuantity { get; set; }
        public int? ProductCategoryId { get; set; }
        public string ProductCategoryName { get; set; }
        public int? ProductSubCategoryId { get; set; }
        public string ProductSubCategoryName { get; set; }
        public int? ProductSubCategory2Id { get; set; }
        public string ProductSubCategory2Name { get; set; }
        public int? ProductSubCategory3Id { get; set; }
        public string ProductSubCategory3Name { get; set; }
        public string BarCode { get; set; }
        public string FactoryReference { get; set; }
        public bool? IsNewProduct { get; set; }
        public string Remarks { get; set; }

        public int ProductImageCount { get; set; }
    }

    public enum POProductResult
    {
        Success=1,
        NotFound=2
    }

    public class POProductDataResponse
    {
        public List<POProductData> ProductList { get; set; }
        public POProductDataResult Result { get; set; }
    }

    public class POProductData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int PoQuantity { get; set; }
        public int? ProductCategoryId { get; set; }
        public string ProductCategoryName { get; set; }
        public int? ProductSubCategoryId { get; set; }
        public string ProductSubCategoryName { get; set; }
        public int? ProductSubCategory2Id { get; set; }
        public string ProductSubCategory2Name { get; set; }
        public int? ProductSubCategory3Id { get; set; }
        public string ProductSubCategory3Name { get; set; }
        public string BarCode { get; set; }
        public string FactoryReference { get; set; }
        public bool? IsNewProduct { get; set; }
        public int PoId { get; set; }
        public string PoName { get; set; }
        public DateObject Etd { get; set; }
        public int? DestinationCountryId { get; set; }
        public string DestinationCountryName { get; set; }
        public string Remarks { get; set; }

        public List<CommonDataSource> ProductSubCategoryList { get; set; }
        public List<CommonDataSource> ProductSubCategory2List { get; set; }
        public List<CommonDataSource> ProductSubCategory3List { get; set; }
    }

    public class POProductRelatedData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int PoQuantity { get; set; }
        public int? ProductCategoryId { get; set; }
        public string ProductCategoryName { get; set; }
        public int? ProductSubCategoryId { get; set; }
        public string ProductSubCategoryName { get; set; }
        public int? ProductSubCategory2Id { get; set; }
        public string ProductSubCategory2Name { get; set; }
        public int? ProductSubCategory3Id { get; set; }
        public string ProductSubCategory3Name { get; set; }
        public string BarCode { get; set; }
        public string FactoryReference { get; set; }
        public bool? IsNewProduct { get; set; }
        public int PoId { get; set; }
        public string PoName { get; set; }
        public DateTime? Etd { get; set; }
        public int? DestinationCountryId { get; set; }
        public string DestinationCountryName { get; set; }
        public string Remarks { get; set; }
    }

    public enum POProductDataResult
    {
        Success = 1,
        NotFound = 2
    }
}
