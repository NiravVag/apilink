using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_Product_MSChart_OCR_MAP")]
    public partial class CuProductMschartOcrMap
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        [Column("OCR_CustomerName")]
        [StringLength(500)]
        public string OcrCustomerName { get; set; }
        [Column("OCR_FileFormat")]
        [StringLength(500)]
        public string OcrFileFormat { get; set; }
        public bool Active { get; set; }
        public int CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("CuProductMschartOcrMapCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("CustomerId")]
        [InverseProperty("CuProductMschartOcrMaps")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("CuProductMschartOcrMapDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
    }
}