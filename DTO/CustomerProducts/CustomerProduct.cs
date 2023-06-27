using DTO.Common;
using DTO.CommonClass;
using DTO.References;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.CustomerProducts
{
    public class CustomerProduct
    {
        public int Id { get; set; }
        public string ProductId { get; set; }
        public string ProductDescription { get; set; }
        public int CustomerId { get; set; }
        public string Barcode { get; set; }
        public string FactoryReference { get; set; }
        public int? ProductCategory { get; set; }
        public int? ProductSubCategory { get; set; }
        public int? ProductCategorySub2 { get; set; }
        public bool isBooked { get; set; }
        public bool isProductBooked { get; set; }
        public string Remarks { get; set; }
        public bool Active { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedTime { get; set; }
        public IEnumerable<ProductAttachment> CuProductFileAttachments { get; set; }
        public List<int> ApiServiceIds { get; set; }
        public bool isNewProduct { get; set; }
        public bool? IsMsChart { get; set; }
        public bool? IsStyle { get; set; }
        public int? ProductCategorySub3 { get; set; }
        public int? SampleSize8h { get; set; }
        public double? TimePreparation { get; set; }
        public string TpAdjustmentReason { get; set; }
        public int? Unit { get; set; }
        public string TechnicalComments { get; set; }

        public int? screenCallType { get; set; }

        public string ItRemarks { get; set; }

    }

    public class ProductAttachment
    {
        public ProductAttachment()
        {
            this.ProductMsCharts = new List<ProductMSChart>();
        }
        public int Id { get; set; }

        public string uniqueld { get; set; }

        public string FileName { get; set; }

        public string FileUrl { get; set; }

        public bool IsNew { get; set; }

        public string MimeType { get; set; }

        public bool Active { get; set; }

        public int? FileTypeId { get; set; }

        public List<ProductMSChart> ProductMsCharts { get; set; }
    }

    public class ProductMSChart
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public int? ProductFileId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Mpcode { get; set; }
        public double? Required { get; set; }
        public double? Tolerance1Up { get; set; }
        public double? Tolerance1Down { get; set; }
        public double? Tolerance2Up { get; set; }
        public double? Tolerance2Down { get; set; }
        public int? Sort { get; set; }
    }

    public class PoProductRequest
    {
        public List<int> ProductIds { get; set; }
    }

    public class CustomerProductDetail
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int CustomerId { get; set; }
        public string Barcode { get; set; }
        public int? ProductCategoryId { get; set; }
        public string ProductCategoryName { get; set; }
        public int? ProductSubCategoryId { get; set; }
        public string ProductSubCategoryName { get; set; }
        public int? ProductSubCategory2Id { get; set; }
        public string ProductSubCategory2Name { get; set; }
        public int? ProductSubCategory3Id { get; set; }
        public string ProductSubCategory3Name { get; set; }
        public string FactoryReference { get; set; }
        public string Remarks { get; set; }
        public bool Active { get; set; }

        public int ProductImageCount { get; set; }
    }

    public class CustomerProductDetailResponse
    {
        public List<CustomerProductDetail> ProductList { get; set; }
        public CustomerProductDetailResult Result { get; set; }
    }

    public class CustomerProductFileResponse
    {
        public List<string> ProductFileUrls { get; set; }
        public CustomerProductFileResult Result { get; set; }
    }

  

    public enum CustomerProductFileResult
    {
        Success = 1,
        FileNotFound = 2
    }

    public enum CustomerProductDetailResult
    {
        Success = 1,
        CannotGetProducts = 2,
        NotFound = 3
    }

    public enum ProductScreenCallType
    {
        Product = 1,
        Booking = 2,
        PurchaseOrder = 3,
        PoUpload = 4
    }

}
