using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("EM_ExchangeRate")]
    public partial class EmExchangeRate
    {
        public int Id { get; set; }
        public int CurrencyId1 { get; set; }
        public int Currencyid2 { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreateDate { get; set; }
        [Column("User_id")]
        public int UserId { get; set; }
        public double Rate { get; set; }
        [Required]
        public bool? Active { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime BeginDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime EndDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastUpdateDate { get; set; }
        public int EntityId { get; set; }
        public int? ExRateTypeId { get; set; }

        [ForeignKey("CurrencyId1")]
        [InverseProperty("EmExchangeRateCurrencyId1Navigations")]
        public virtual RefCurrency CurrencyId1Navigation { get; set; }
        [ForeignKey("Currencyid2")]
        [InverseProperty("EmExchangeRateCurrencyid2Navigations")]
        public virtual RefCurrency Currencyid2Navigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("EmExchangeRates")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("ExRateTypeId")]
        [InverseProperty("EmExchangeRates")]
        public virtual EmExchangeRateType ExRateType { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("EmExchangeRates")]
        public virtual ItUserMaster User { get; set; }
    }
}