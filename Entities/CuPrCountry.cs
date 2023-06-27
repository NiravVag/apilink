using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_PR_Country")]
    public partial class CuPrCountry
    {
        public int Id { get; set; }
        [Column("CU_PR_Id")]
        public int CuPrId { get; set; }
        public int FactoryCountryId { get; set; }
        public bool? Active { get; set; }
        public int? CreatedBy { get; set; }
        public int? DeletedBy { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("CuPrCountryCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("CuPrId")]
        [InverseProperty("CuPrCountries")]
        public virtual CuPrDetail CuPr { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("CuPrCountryDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("FactoryCountryId")]
        [InverseProperty("CuPrCountries")]
        public virtual RefCountry FactoryCountry { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("CuPrCountryUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
    }
}