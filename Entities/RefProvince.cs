using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REF_Province")]
    public partial class RefProvince
    {
        public RefProvince()
        {
            CuPrProvinces = new HashSet<CuPrProvince>();
            InspLabAddresses = new HashSet<InspLabAddress>();
            InvTmDetails = new HashSet<InvTmDetail>();
            RefCities = new HashSet<RefCity>();
            SuAddresses = new HashSet<SuAddress>();
        }

        public int Id { get; set; }
        [Column("Province_Code")]
        [StringLength(50)]
        public string ProvinceCode { get; set; }
        [Column("Country_Id")]
        public int CountryId { get; set; }
        [Required]
        [Column("Province_Name")]
        [StringLength(50)]
        public string ProvinceName { get; set; }
        [Required]
        public bool? Active { get; set; }
        [Column("FB_ProvinceId")]
        public int? FbProvinceId { get; set; }
        [Column(TypeName = "decimal(12, 9)")]
        public decimal? Longitude { get; set; }
        [Column(TypeName = "decimal(12, 9)")]
        public decimal? Latitude { get; set; }

        [ForeignKey("CountryId")]
        [InverseProperty("RefProvinces")]
        public virtual RefCountry Country { get; set; }
        [InverseProperty("FactoryProvince")]
        public virtual ICollection<CuPrProvince> CuPrProvinces { get; set; }
        [InverseProperty("Province")]
        public virtual ICollection<InspLabAddress> InspLabAddresses { get; set; }
        [InverseProperty("Province")]
        public virtual ICollection<InvTmDetail> InvTmDetails { get; set; }
        [InverseProperty("Province")]
        public virtual ICollection<RefCity> RefCities { get; set; }
        [InverseProperty("Region")]
        public virtual ICollection<SuAddress> SuAddresses { get; set; }
    }
}