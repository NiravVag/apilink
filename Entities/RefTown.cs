using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REF_Town")]
    public partial class RefTown
    {
        public RefTown()
        {
            EcAutQcTravelExpenses = new HashSet<EcAutQcTravelExpense>();
            EcAutTravelTariffs = new HashSet<EcAutTravelTariff>();
            SuAddresses = new HashSet<SuAddress>();
        }

        public int Id { get; set; }
        public int CountyId { get; set; }
        [Required]
        [StringLength(500)]
        public string TownName { get; set; }
        [StringLength(500)]
        public string TownCode { get; set; }
        public bool Active { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }

        [ForeignKey("CountyId")]
        [InverseProperty("RefTowns")]
        public virtual RefCounty County { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("RefTownCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("RefTownDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("ModifiedBy")]
        [InverseProperty("RefTownModifiedByNavigations")]
        public virtual ItUserMaster ModifiedByNavigation { get; set; }
        [InverseProperty("FactoryTownNavigation")]
        public virtual ICollection<EcAutQcTravelExpense> EcAutQcTravelExpenses { get; set; }
        [InverseProperty("Town")]
        public virtual ICollection<EcAutTravelTariff> EcAutTravelTariffs { get; set; }
        [InverseProperty("Town")]
        public virtual ICollection<SuAddress> SuAddresses { get; set; }
    }
}