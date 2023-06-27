using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INV_TM_Details")]
    public partial class InvTmDetail
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public int TravelMatrixTypeId { get; set; }
        public int CountryId { get; set; }
        public int ProvinceId { get; set; }
        public int? CityId { get; set; }
        public int? CountyId { get; set; }
        public int? InspPortCountyId { get; set; }
        [Column("DistanceKM")]
        public double? DistanceKm { get; set; }
        public double? TravelTime { get; set; }
        public double? BusCost { get; set; }
        public double? TrainCost { get; set; }
        public double? TaxiCost { get; set; }
        public double? HotelCost { get; set; }
        public double? OtherCost { get; set; }
        public double? MarkUpCost { get; set; }
        public double? AirCost { get; set; }
        public double? MarkUpAirCost { get; set; }
        public int? TravelCurrencyId { get; set; }
        public int? SourceCurrencyId { get; set; }
        public double? FixedExchangeRate { get; set; }
        [StringLength(2000)]
        public string Remarks { get; set; }
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
        public int? EntityId { get; set; }
        public int? InspPortCityId { get; set; }

        [ForeignKey("CityId")]
        [InverseProperty("InvTmDetailCities")]
        public virtual RefCity City { get; set; }
        [ForeignKey("CountryId")]
        [InverseProperty("InvTmDetails")]
        public virtual RefCountry Country { get; set; }
        [ForeignKey("CountyId")]
        [InverseProperty("InvTmDetailCounties")]
        public virtual RefCounty County { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("InvTmDetailCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("CustomerId")]
        [InverseProperty("InvTmDetails")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("InvTmDetailDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("InvTmDetails")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("InspPortCityId")]
        [InverseProperty("InvTmDetailInspPortCities")]
        public virtual RefCity InspPortCity { get; set; }
        [ForeignKey("InspPortCountyId")]
        [InverseProperty("InvTmDetailInspPortCounties")]
        public virtual RefCounty InspPortCounty { get; set; }
        [ForeignKey("ProvinceId")]
        [InverseProperty("InvTmDetails")]
        public virtual RefProvince Province { get; set; }
        [ForeignKey("SourceCurrencyId")]
        [InverseProperty("InvTmDetailSourceCurrencies")]
        public virtual RefCurrency SourceCurrency { get; set; }
        [ForeignKey("TravelCurrencyId")]
        [InverseProperty("InvTmDetailTravelCurrencies")]
        public virtual RefCurrency TravelCurrency { get; set; }
        [ForeignKey("TravelMatrixTypeId")]
        [InverseProperty("InvTmDetails")]
        public virtual InvTmType TravelMatrixType { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("InvTmDetailUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
    }
}