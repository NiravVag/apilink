using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("SU_Address")]
    public partial class SuAddress
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public int RegionId { get; set; }
        public int CityId { get; set; }
        [StringLength(20)]
        public string ZipCode { get; set; }
        [Required]
        [StringLength(2000)]
        public string Address { get; set; }
        [StringLength(2000)]
        public string LocalLanguage { get; set; }
        [Column("Supplier_Id")]
        public int SupplierId { get; set; }
        [Column(TypeName = "decimal(12, 9)")]
        public decimal? Longitude { get; set; }
        [Column(TypeName = "decimal(12, 9)")]
        public decimal? Latitude { get; set; }
        public int? AddressTypeId { get; set; }
        public int? CountyId { get; set; }
        public int? TownId { get; set; }

        [ForeignKey("AddressTypeId")]
        [InverseProperty("SuAddresses")]
        public virtual SuAddressType AddressType { get; set; }
        [ForeignKey("CityId")]
        [InverseProperty("SuAddresses")]
        public virtual RefCity City { get; set; }
        [ForeignKey("CountryId")]
        [InverseProperty("SuAddresses")]
        public virtual RefCountry Country { get; set; }
        [ForeignKey("CountyId")]
        [InverseProperty("SuAddresses")]
        public virtual RefCounty County { get; set; }
        [ForeignKey("RegionId")]
        [InverseProperty("SuAddresses")]
        public virtual RefProvince Region { get; set; }
        [ForeignKey("SupplierId")]
        [InverseProperty("SuAddresses")]
        public virtual SuSupplier Supplier { get; set; }
        [ForeignKey("TownId")]
        [InverseProperty("SuAddresses")]
        public virtual RefTown Town { get; set; }
    }
}