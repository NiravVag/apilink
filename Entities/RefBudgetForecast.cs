using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REF_Budget_Forecast")]
    public partial class RefBudgetForecast
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int? CountryId { get; set; }
        public int ManDay { get; set; }
        public bool Active { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public int? DeletedBy { get; set; }
        public int? CurrencyId { get; set; }
        public double? Fees { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("CountryId")]
        [InverseProperty("RefBudgetForecasts")]
        public virtual RefCountry Country { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("RefBudgetForecastCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("CurrencyId")]
        [InverseProperty("RefBudgetForecasts")]
        public virtual RefCurrency Currency { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("RefBudgetForecastDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("RefBudgetForecasts")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("RefBudgetForecastUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
    }
}