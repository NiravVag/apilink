using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("EC_AUT_REF_StartPort")]
    public partial class EcAutRefStartPort
    {
        public EcAutRefStartPort()
        {
            EcAutQcTravelExpenses = new HashSet<EcAutQcTravelExpense>();
            EcAutTravelTariffs = new HashSet<EcAutTravelTariff>();
            HrStaffs = new HashSet<HrStaff>();
        }

        public int Id { get; set; }
        [StringLength(1000)]
        public string StartPortName { get; set; }
        public bool? Active { get; set; }
        public bool? Sort { get; set; }
        [Column("Entity_Id")]
        public int? EntityId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public int? DeletedBy { get; set; }
        public int? CreatedBy { get; set; }
        public int? CityId { get; set; }

        [ForeignKey("CityId")]
        [InverseProperty("EcAutRefStartPorts")]
        public virtual RefCity City { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("EcAutRefStartPortCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("EcAutRefStartPortDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("EcAutRefStartPorts")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("EcAutRefStartPortUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
        [InverseProperty("StartPortNavigation")]
        public virtual ICollection<EcAutQcTravelExpense> EcAutQcTravelExpenses { get; set; }
        [InverseProperty("StartPortNavigation")]
        public virtual ICollection<EcAutTravelTariff> EcAutTravelTariffs { get; set; }
        [InverseProperty("StartPortNavigation")]
        public virtual ICollection<HrStaff> HrStaffs { get; set; }
    }
}