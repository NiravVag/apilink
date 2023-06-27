using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("OM_Details")]
    public partial class OmDetail
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public int? OfficeCountryId { get; set; }
        public int? QcId { get; set; }
        public int? OperationalCountryId { get; set; }
        public int? PurposeId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ServiceDate { get; set; }
        public double? ManDay { get; set; }
        [StringLength(1000)]
        public string Remarks { get; set; }
        public bool? Active { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("OmDetailCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("CustomerId")]
        [InverseProperty("OmDetails")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("OmDetailDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("OmDetails")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("OfficeCountryId")]
        [InverseProperty("OmDetailOfficeCountries")]
        public virtual RefCountry OfficeCountry { get; set; }
        [ForeignKey("OperationalCountryId")]
        [InverseProperty("OmDetailOperationalCountries")]
        public virtual RefCountry OperationalCountry { get; set; }
        [ForeignKey("PurposeId")]
        [InverseProperty("OmDetails")]
        public virtual OmRefPurpose Purpose { get; set; }
        [ForeignKey("QcId")]
        [InverseProperty("OmDetails")]
        public virtual HrStaff Qc { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("OmDetailUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
    }
}