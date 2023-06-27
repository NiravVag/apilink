using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_PR_Sampling")]
    public partial class CuPrSampling
    {
        public int Id { get; set; }
        [Column("Cu_Price_Id")]
        public int CuPriceId { get; set; }
        public int? MaxProductCount { get; set; }
        public bool? SampleSizeBySet { get; set; }
        public double? MinBillingDay { get; set; }
        public int? MaxSampleSize { get; set; }
        public int? AdditionalSampleSize { get; set; }
        public double? AdditionalSamplePrice { get; set; }
        public double? Quantity8 { get; set; }
        public double? Quantity13 { get; set; }
        public double? Quantity20 { get; set; }
        public double? Quantity32 { get; set; }
        public double? Quantity50 { get; set; }
        public double? Quantity80 { get; set; }
        public double? Quantity125 { get; set; }
        public double? Quantity200 { get; set; }
        public double? Quantity315 { get; set; }
        public double? Quantity500 { get; set; }
        public double? Quantity800 { get; set; }
        public double? Quantity1250 { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public int? DeletedBy { get; set; }
        public bool? Active { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("CuPrSamplingCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("CuPriceId")]
        [InverseProperty("CuPrSamplings")]
        public virtual CuPrDetail CuPrice { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("CuPrSamplingDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("CuPrSamplingUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
    }
}