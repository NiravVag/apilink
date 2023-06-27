using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_Products")]
    public partial class CuProduct
    {
        public CuProduct()
        {
            CompTranComplaintsDetails = new HashSet<CompTranComplaintsDetail>();
            CuProductApiServices = new HashSet<CuProductApiService>();
            CuProductFileAttachments = new HashSet<CuProductFileAttachment>();
            CuProductMscharts = new HashSet<CuProductMschart>();
            CuPurchaseOrderDetails = new HashSet<CuPurchaseOrderDetail>();
            FbReportAdditionalPhotos = new HashSet<FbReportAdditionalPhoto>();
            FbReportComments = new HashSet<FbReportComment>();
            FbReportInspSummaryPhotos = new HashSet<FbReportInspSummaryPhoto>();
            FbReportOtherInformations = new HashSet<FbReportOtherInformation>();
            FbReportPackingBatteryInfos = new HashSet<FbReportPackingBatteryInfo>();
            FbReportPackingDimentions = new HashSet<FbReportPackingDimention>();
            FbReportPackingInfos = new HashSet<FbReportPackingInfo>();
            FbReportPackingWeights = new HashSet<FbReportPackingWeight>();
            FbReportProblematicRemarks = new HashSet<FbReportProblematicRemark>();
            FbReportProductBarcodesInfos = new HashSet<FbReportProductBarcodesInfo>();
            FbReportProductDimentions = new HashSet<FbReportProductDimention>();
            FbReportProductWeights = new HashSet<FbReportProductWeight>();
            FbReportSamplePickings = new HashSet<FbReportSamplePicking>();
            FbReportSampleTypes = new HashSet<FbReportSampleType>();
            InspProductTransactionParentProducts = new HashSet<InspProductTransaction>();
            InspProductTransactionProducts = new HashSet<InspProductTransaction>();
        }

        public int Id { get; set; }
        [Required]
        [Column("ProductID")]
        [StringLength(200)]
        public string ProductId { get; set; }
        [Required]
        [Column("Product Description")]
        [StringLength(3500)]
        public string ProductDescription { get; set; }
        [Column("CustomerID")]
        public int CustomerId { get; set; }
        [StringLength(100)]
        public string Barcode { get; set; }
        public int? ProductCategory { get; set; }
        public int? ProductSubCategory { get; set; }
        [StringLength(1000)]
        public string Remarks { get; set; }
        public bool Active { get; set; }
        public int CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedTime { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedTime { get; set; }
        public int? ProductCategorySub2 { get; set; }
        [Column("Fb_Cus_Prod_Id")]
        public int? FbCusProdId { get; set; }
        [StringLength(1000)]
        public string FactoryReference { get; set; }
        public bool? IsNewProduct { get; set; }
        [Column("IsMS_Chart")]
        public bool? IsMsChart { get; set; }
        public bool? IsStyle { get; set; }
        public int? EntityId { get; set; }
        public int? ProductCategorySub3 { get; set; }
        [Column("SampleSize_8h")]
        public int? SampleSize8h { get; set; }
        public double? TimePreparation { get; set; }
        [Column("Tp_AdjustmentReason")]
        [StringLength(1000)]
        public string TpAdjustmentReason { get; set; }
        public int? Unit { get; set; }
        [StringLength(1000)]
        public string TechnicalComments { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        [Column("IT_Remarks")]
        public string ItRemarks { get; set; }

        [ForeignKey("CustomerId")]
        [InverseProperty("CuProducts")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("CuProducts")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("ProductCategory")]
        [InverseProperty("CuProducts")]
        public virtual RefProductCategory ProductCategoryNavigation { get; set; }
        [ForeignKey("ProductCategorySub2")]
        [InverseProperty("CuProducts")]
        public virtual RefProductCategorySub2 ProductCategorySub2Navigation { get; set; }
        [ForeignKey("ProductCategorySub3")]
        [InverseProperty("CuProducts")]
        public virtual RefProductCategorySub3 ProductCategorySub3Navigation { get; set; }
        [ForeignKey("ProductSubCategory")]
        [InverseProperty("CuProducts")]
        public virtual RefProductCategorySub ProductSubCategoryNavigation { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("CuProducts")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<CompTranComplaintsDetail> CompTranComplaintsDetails { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<CuProductApiService> CuProductApiServices { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<CuProductFileAttachment> CuProductFileAttachments { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<CuProductMschart> CuProductMscharts { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<CuPurchaseOrderDetail> CuPurchaseOrderDetails { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<FbReportAdditionalPhoto> FbReportAdditionalPhotos { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<FbReportComment> FbReportComments { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<FbReportInspSummaryPhoto> FbReportInspSummaryPhotos { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<FbReportOtherInformation> FbReportOtherInformations { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<FbReportPackingBatteryInfo> FbReportPackingBatteryInfos { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<FbReportPackingDimention> FbReportPackingDimentions { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<FbReportPackingInfo> FbReportPackingInfos { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<FbReportPackingWeight> FbReportPackingWeights { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<FbReportProblematicRemark> FbReportProblematicRemarks { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<FbReportProductBarcodesInfo> FbReportProductBarcodesInfos { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<FbReportProductDimention> FbReportProductDimentions { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<FbReportProductWeight> FbReportProductWeights { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<FbReportSamplePicking> FbReportSamplePickings { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<FbReportSampleType> FbReportSampleTypes { get; set; }
        [InverseProperty("ParentProduct")]
        public virtual ICollection<InspProductTransaction> InspProductTransactionParentProducts { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<InspProductTransaction> InspProductTransactionProducts { get; set; }
    }
}