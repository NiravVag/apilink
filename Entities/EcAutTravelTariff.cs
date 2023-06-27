using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("EC_AUT_TravelTariff")]
    public partial class EcAutTravelTariff
    {
        public int Id { get; set; }
        public int StartPort { get; set; }
        public int TownId { get; set; }
        public double? TravelTariff { get; set; }
        public int? TravelCurrency { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? StartDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EndDate { get; set; }
        [Column("Entity_Id")]
        public int? EntityId { get; set; }
        public bool? Active { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public int? DeletedBy { get; set; }
        public int? CreatedBy { get; set; }
        public bool? Status { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("EcAutTravelTariffCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("EcAutTravelTariffDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("EcAutTravelTariffs")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("StartPort")]
        [InverseProperty("EcAutTravelTariffs")]
        public virtual EcAutRefStartPort StartPortNavigation { get; set; }
        [ForeignKey("TownId")]
        [InverseProperty("EcAutTravelTariffs")]
        public virtual RefTown Town { get; set; }
        [ForeignKey("TravelCurrency")]
        [InverseProperty("EcAutTravelTariffs")]
        public virtual RefCurrency TravelCurrencyNavigation { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("EcAutTravelTariffUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
    }
}