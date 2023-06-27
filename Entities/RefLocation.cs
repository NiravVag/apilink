using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REF_Location")]
    public partial class RefLocation
    {
        public RefLocation()
        {
            AudBookingContacts = new HashSet<AudBookingContact>();
            AudTransactions = new HashSet<AudTransaction>();
            CompComplaints = new HashSet<CompComplaint>();
            CuCsConfigurations = new HashSet<CuCsConfiguration>();
            CuPurchaseOrders = new HashSet<CuPurchaseOrder>();
            DaUserRoleNotificationByOffices = new HashSet<DaUserRoleNotificationByOffice>();
            EcExpencesClaims = new HashSet<EcExpencesClaim>();
            EsOfficeConfigs = new HashSet<EsOfficeConfig>();
            HrHolidays = new HashSet<HrHoliday>();
            HrOfficeControls = new HashSet<HrOfficeControl>();
            HrStaffs = new HashSet<HrStaff>();
            InspTransactions = new HashSet<InspTransaction>();
            InvCreTransactions = new HashSet<InvCreTransaction>();
            InvDaOffices = new HashSet<InvDaOffice>();
            InverseHeadOfficeNavigation = new HashSet<RefLocation>();
            QuQuotations = new HashSet<QuQuotation>();
            RefCityDetails = new HashSet<RefCityDetail>();
            RefCountryLocations = new HashSet<RefCountryLocation>();
            RefLocationCountries = new HashSet<RefLocationCountry>();
            RefZones = new HashSet<RefZone>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(1500)]
        public string Address { get; set; }
        [Column("City_Id")]
        public int CityId { get; set; }
        [StringLength(30)]
        public string ZipCode { get; set; }
        [Column("LocationType_Id")]
        public int LocationTypeId { get; set; }
        [Required]
        [Column("Location_Name")]
        [StringLength(1000)]
        public string LocationName { get; set; }
        [Column("Master_Currency_Id")]
        public int? MasterCurrencyId { get; set; }
        [Column("Default_Currency_Id")]
        public int? DefaultCurrencyId { get; set; }
        [StringLength(100)]
        public string Tel { get; set; }
        [StringLength(40)]
        public string Fax { get; set; }
        [StringLength(150)]
        public string Email { get; set; }
        public int? ParentId { get; set; }
        [StringLength(2000)]
        public string Comment { get; set; }
        public bool Active { get; set; }
        public int? EntityId { get; set; }
        [StringLength(200)]
        public string OfficeCode { get; set; }
        [Required]
        [StringLength(1500)]
        public string Address2 { get; set; }
        public int? HeadOffice { get; set; }

        [ForeignKey("CityId")]
        [InverseProperty("RefLocations")]
        public virtual RefCity City { get; set; }
        [ForeignKey("DefaultCurrencyId")]
        [InverseProperty("RefLocationDefaultCurrencies")]
        public virtual RefCurrency DefaultCurrency { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("RefLocations")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("HeadOffice")]
        [InverseProperty("InverseHeadOfficeNavigation")]
        public virtual RefLocation HeadOfficeNavigation { get; set; }
        [ForeignKey("LocationTypeId")]
        [InverseProperty("RefLocations")]
        public virtual RefLocationType LocationType { get; set; }
        [ForeignKey("MasterCurrencyId")]
        [InverseProperty("RefLocationMasterCurrencies")]
        public virtual RefCurrency MasterCurrency { get; set; }
        [InverseProperty("Office")]
        public virtual ICollection<AudBookingContact> AudBookingContacts { get; set; }
        [InverseProperty("Office")]
        public virtual ICollection<AudTransaction> AudTransactions { get; set; }
        [InverseProperty("OfficeNavigation")]
        public virtual ICollection<CompComplaint> CompComplaints { get; set; }
        [InverseProperty("OfficeLocation")]
        public virtual ICollection<CuCsConfiguration> CuCsConfigurations { get; set; }
        [InverseProperty("Office")]
        public virtual ICollection<CuPurchaseOrder> CuPurchaseOrders { get; set; }
        [InverseProperty("Office")]
        public virtual ICollection<DaUserRoleNotificationByOffice> DaUserRoleNotificationByOffices { get; set; }
        [InverseProperty("Location")]
        public virtual ICollection<EcExpencesClaim> EcExpencesClaims { get; set; }
        [InverseProperty("Office")]
        public virtual ICollection<EsOfficeConfig> EsOfficeConfigs { get; set; }
        [InverseProperty("Location")]
        public virtual ICollection<HrHoliday> HrHolidays { get; set; }
        [InverseProperty("Location")]
        public virtual ICollection<HrOfficeControl> HrOfficeControls { get; set; }
        [InverseProperty("Location")]
        public virtual ICollection<HrStaff> HrStaffs { get; set; }
        [InverseProperty("Office")]
        public virtual ICollection<InspTransaction> InspTransactions { get; set; }
        [InverseProperty("OfficeNavigation")]
        public virtual ICollection<InvCreTransaction> InvCreTransactions { get; set; }
        [InverseProperty("Office")]
        public virtual ICollection<InvDaOffice> InvDaOffices { get; set; }
        [InverseProperty("HeadOfficeNavigation")]
        public virtual ICollection<RefLocation> InverseHeadOfficeNavigation { get; set; }
        [InverseProperty("Office")]
        public virtual ICollection<QuQuotation> QuQuotations { get; set; }
        [InverseProperty("Location")]
        public virtual ICollection<RefCityDetail> RefCityDetails { get; set; }
        [InverseProperty("Location")]
        public virtual ICollection<RefCountryLocation> RefCountryLocations { get; set; }
        [InverseProperty("Location")]
        public virtual ICollection<RefLocationCountry> RefLocationCountries { get; set; }
        [InverseProperty("Location")]
        public virtual ICollection<RefZone> RefZones { get; set; }
    }
}