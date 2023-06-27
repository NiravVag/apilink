using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_PR_City")]
    public partial class CuPrCity
    {
        public int Id { get; set; }
        [Column("CU_PR_Id")]
        public int CuPrId { get; set; }
        [Column("Factory_City_Id")]
        public int FactoryCityId { get; set; }
        public bool? Active { get; set; }
        [Column("Created_By")]
        public int? CreatedBy { get; set; }
        [Column("Deleted_By")]
        public int? DeletedBy { get; set; }
        [Column("Updated_By")]
        public int? UpdatedBy { get; set; }
        [Column("Updated_On", TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        [Column("Created_On", TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        [Column("Deleted_On", TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        [Column("Entity_Id")]
        public int? EntityId { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("CuPrCityCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("CuPrId")]
        [InverseProperty("CuPrCities")]
        public virtual CuPrDetail CuPr { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("CuPrCityDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("CuPrCities")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("FactoryCityId")]
        [InverseProperty("CuPrCities")]
        public virtual RefCity FactoryCity { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("CuPrCityUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
    }
}