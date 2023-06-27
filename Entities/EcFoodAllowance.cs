using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("EC_FoodAllowance")]
    public partial class EcFoodAllowance
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime StartDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime EndDate { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal FoodAllowance { get; set; }
        public int CurrencyId { get; set; }
        public int UserId { get; set; }
        public int? EntityId { get; set; }
        public bool? Active { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }

        [ForeignKey("CountryId")]
        [InverseProperty("EcFoodAllowances")]
        public virtual RefCountry Country { get; set; }
        [ForeignKey("CurrencyId")]
        [InverseProperty("EcFoodAllowances")]
        public virtual RefCurrency Currency { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("EcFoodAllowanceDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("EcFoodAllowances")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("EcFoodAllowanceUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("EcFoodAllowanceUsers")]
        public virtual ItUserMaster User { get; set; }
    }
}