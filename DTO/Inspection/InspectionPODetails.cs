using DTO.Common;
using DTO.CommonClass;
using DTO.References;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Inspection
{
    public class InspectionPODetails
    {
        public int Id { get; set; }

        public int? PoId { get; set; }

        public string PoName { get; set; }

        public int PoDetailId { get; set; }

        public string ProductName { get; set; }

        public int ProductId { get; set; }

        public string ProductDesc { get; set; }

        public int? ProductCategoryId { get; set; }

        public int? ProductSubCategoryId { get; set; }

        public int? ProductCategorySub2Id { get; set; }

        public int InspectionId { get; set; }

        public int Unit { get; set; }

        public int? UnitCount { get; set; }

        public int BookingQuantity { get; set; }

        public int ExistingBookingQuantity { get; set; }

        public int PoQuantity { get; set; }

        public int PoReminingQuantity { get; set; }

        public int? PickingQuantity { get; set; }

        public string Remarks { get; set; }

        public int? Aql { get; set; }

        public int? Critical { get; set; }

        public int? Major { get; set; }

        public int? Minor { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int? DeletedBy { get; set; }

        public DateTime? DeletedOn { get; set; }

        public bool? Active { get; set; }

        public string ProductCategoryName { get; set; }

        public string ProductSubCategoryName { get; set; }

        public string ProductCategorySub2Name { get; set; }

        public string AqlName { get; set; }

        public string UnitName { get; set; }

        public int? DestinationCountryID { get; set; }

        public int? SamplingSize { get; set; }

        public int? CombineSamplingSize { get; set; }

        public int? CombineGroupId { get; set; }

        public int? ReportId { get; set; }

        public string FactoryReference { get; set; }

        public DateObject ETD { get; set; }

        public string Barcode { get; set; }

        public IEnumerable<ProductFileAttachmentRepsonse> ProductFileAttachments { get; set; }

        public IEnumerable<ProductSubCategory> BookingCategorySubProductList { get; set; }

        public IEnumerable<ProductCategorySub2> BookingCategorySub2ProductList { get; set; }

        public IEnumerable<InspectionPickingData> InspectionPickingList { get; set; }

    }


    public class InspectionProductDetails
    {
        public int Id { get; set; }

        public string ProductName { get; set; }

        public int ProductId { get; set; }

        public string ProductDesc { get; set; }

        public int? ProductCategoryId { get; set; }

        public int? ProductSubCategoryId { get; set; }

        public int? ProductCategorySub2Id { get; set; }

        public int InspectionId { get; set; }

        public int Unit { get; set; }

        public int? UnitCount { get; set; }

        public int TotalBookingQuantity { get; set; }

        public string Remarks { get; set; }

        public int? Aql { get; set; }

        public int? Critical { get; set; }

        public int? Major { get; set; }

        public int? Minor { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int? DeletedBy { get; set; }

        public DateTime? DeletedOn { get; set; }

        public bool? Active { get; set; }

        public string ProductCategoryName { get; set; }

        public string ProductSubCategoryName { get; set; }

        public string ProductCategorySub2Name { get; set; }

        public string AqlName { get; set; }

        public string UnitName { get; set; }

        public int? AqlQuantity { get; set; }

        public int? SampleType { get; set; }

        public int? CombineSamplingSize { get; set; }

        public int? CombineGroupId { get; set; }

        public int? ReportId { get; set; }

        public string FactoryReference { get; set; }

        public string Barcode { get; set; }

        public List<InspectionPODetail> ProductPODetails { get; set; }


        public List<ProductFileAttachmentRepsonse> ProductFileAttachments { get; set; }

        public IEnumerable<CommonDataSource> BookingCategorySubProductList { get; set; }

        public List<CommonDataSource> BookingCategorySub2ProductList { get; set; }

        public IEnumerable<InspectionPickingData> InspectionPickingList { get; set; }

        public bool IsEcopack { get; set; }

        public bool IsDisplayMaster { get; set; }

        public int? FBTemplateId { get; set; }

        public string FbTemplateName { get; set; }

        public int? ParentProductId { get; set; }

        public string ParentProductName { get; set; }

        public bool? IsNewProduct { get; set; }

        public DateObject AsReceivedDate { get; set; }

        public DateObject TfReceivedDate { get; set; }
    }

    public class InspectionProductPODetails
    {
        public int Id { get; set; }

        public int? PoId { get; set; }

        public string PoName { get; set; }

        public int InspectionId { get; set; }

        public int BookingQuantity { get; set; }

        public int ExistingBookingQuantity { get; set; }

        public int PoQuantity { get; set; }

        public int PoReminingQuantity { get; set; }

        public int? PickingQuantity { get; set; }

        public string Remarks { get; set; }

        public int? DestinationCountryID { get; set; }

        public int? ContainerId { get; set; }

        public int? ReportId { get; set; }

        public DateObject ETD { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int? DeletedBy { get; set; }

        public DateTime? DeletedOn { get; set; }

        public bool? Active { get; set; }

        public string CustomerReferencePo { get; set; }

        public string BaseCustomerReferencePo { get; set; }

        public IEnumerable<InspectionPickingData> InspectionPickingList { get; set; }

    }

    public class PoProductDetailRequest
    {
        public int PoId { get; set; }
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public int SupplierId { get; set; }
        public bool FilterPoByProduct { get; set; }
    }

    public class BookingPOProductDataSourceRequest
    {
        public string SearchText { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public int? PoId { get; set; }
        public int? SupplierId { get; set; }
        public List<int> CustomerIds { get; set; }
        public int filterPoProduct { get; set; }
        public BookingPOProductDataSourceRequest()
        {
            this.SearchText = "";
            this.Skip = 0;
            this.Take = 10;
        }
    }

    public class POProductDetailResponse
    {
        public POProductDetail POProductDetail { get; set; }
        public POProductDetailResult Result { get; set; }
    }

    public enum POProductDetailResult
    {
        Success = 1,
        NotFound = 2
    }

    public class BookingPoProductSearchData
    {
        public int PoId { get; set; }
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public int SupplierId { get; set; }

    }

    public class POProductDetailRepo
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

        public DateTime? Etd { get; set; }

        public int? DestinationCountryId { get; set; }

        public int ProductImageCount { get; set; }
    }

    public class POProductDetail
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

        public DateObject Etd { get; set; }

        public int? DestinationCountryId { get; set; }

        public int ProductImageCount { get; set; }


    }

    public class BookingPOProductListResponse
    {
        public List<BookingPOProductData> ProductList { get; set; }
        public BookingPOProductResult Result { get; set; }
    }

    public class BookingPOProductData
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

    public enum BookingPOProductResult
    {
        Success = 1,
        NotFound = 2
    }
}
