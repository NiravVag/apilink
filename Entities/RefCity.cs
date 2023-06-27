using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REF_City")]
    public partial class RefCity
    {
        public RefCity()
        {
            CuAddresses = new HashSet<CuAddress>();
            CuPrCities = new HashSet<CuPrCity>();
            EcAutRefStartPorts = new HashSet<EcAutRefStartPort>();
            EcExpensesClaimDetaiArrivalCities = new HashSet<EcExpensesClaimDetai>();
            EcExpensesClaimDetaiStartCities = new HashSet<EcExpensesClaimDetai>();
            HrStaffCurrentCities = new HashSet<HrStaff>();
            HrStaffHomeCities = new HashSet<HrStaff>();
            HrStaffHukoLocations = new HashSet<HrStaff>();
            InspLabAddresses = new HashSet<InspLabAddress>();
            InvTmDetailCities = new HashSet<InvTmDetail>();
            InvTmDetailInspPortCities = new HashSet<InvTmDetail>();
            RefCityDetails = new HashSet<RefCityDetail>();
            RefCounties = new HashSet<RefCounty>();
            RefLocations = new HashSet<RefLocation>();
            SuAddresses = new HashSet<SuAddress>();
        }

        public int Id { get; set; }
        [Column("Province_Id")]
        public int ProvinceId { get; set; }
        [Required]
        [Column("City_Name")]
        [StringLength(150)]
        public string CityName { get; set; }
        public bool Active { get; set; }
        [Column("Ph_Code")]
        [StringLength(20)]
        public string PhCode { get; set; }

        [ForeignKey("ProvinceId")]
        [InverseProperty("RefCities")]
        public virtual RefProvince Province { get; set; }
        [InverseProperty("City")]
        public virtual ICollection<CuAddress> CuAddresses { get; set; }
        [InverseProperty("FactoryCity")]
        public virtual ICollection<CuPrCity> CuPrCities { get; set; }
        [InverseProperty("City")]
        public virtual ICollection<EcAutRefStartPort> EcAutRefStartPorts { get; set; }
        [InverseProperty("ArrivalCity")]
        public virtual ICollection<EcExpensesClaimDetai> EcExpensesClaimDetaiArrivalCities { get; set; }
        [InverseProperty("StartCity")]
        public virtual ICollection<EcExpensesClaimDetai> EcExpensesClaimDetaiStartCities { get; set; }
        [InverseProperty("CurrentCity")]
        public virtual ICollection<HrStaff> HrStaffCurrentCities { get; set; }
        [InverseProperty("HomeCity")]
        public virtual ICollection<HrStaff> HrStaffHomeCities { get; set; }
        [InverseProperty("HukoLocation")]
        public virtual ICollection<HrStaff> HrStaffHukoLocations { get; set; }
        [InverseProperty("City")]
        public virtual ICollection<InspLabAddress> InspLabAddresses { get; set; }
        [InverseProperty("City")]
        public virtual ICollection<InvTmDetail> InvTmDetailCities { get; set; }
        [InverseProperty("InspPortCity")]
        public virtual ICollection<InvTmDetail> InvTmDetailInspPortCities { get; set; }
        [InverseProperty("City")]
        public virtual ICollection<RefCityDetail> RefCityDetails { get; set; }
        [InverseProperty("City")]
        public virtual ICollection<RefCounty> RefCounties { get; set; }
        [InverseProperty("City")]
        public virtual ICollection<RefLocation> RefLocations { get; set; }
        [InverseProperty("City")]
        public virtual ICollection<SuAddress> SuAddresses { get; set; }
    }
}