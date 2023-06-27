using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("HR_Staff")]
    public partial class HrStaff
    {
        public HrStaff()
        {
            AudTranAuditors = new HashSet<AudTranAuditor>();
            AudTranCS = new HashSet<AudTranC>();
            CompTranPersonInCharges = new HashSet<CompTranPersonInCharge>();
            CuCsConfigurations = new HashSet<CuCsConfiguration>();
            CuCustomers = new HashSet<CuCustomer>();
            CuKams = new HashSet<CuKam>();
            CuSalesIncharges = new HashSet<CuSalesIncharge>();
            DmRights = new HashSet<DmRight>();
            DmRoles = new HashSet<DmRole>();
            EcAutQcFoodExpenses = new HashSet<EcAutQcFoodExpense>();
            EcAutQcTravelExpenses = new HashSet<EcAutQcTravelExpense>();
            EcExpencesClaims = new HashSet<EcExpencesClaim>();
            EsApiContacts = new HashSet<EsApiContact>();
            EsApiDefaultContacts = new HashSet<EsApiDefaultContact>();
            HrAttachments = new HashSet<HrAttachment>();
            HrEntityMaps = new HashSet<HrEntityMap>();
            HrFileAttachments = new HashSet<HrFileAttachment>();
            HrLeaves = new HashSet<HrLeave>();
            HrOfficeControls = new HashSet<HrOfficeControl>();
            HrPhotos = new HashSet<HrPhoto>();
            HrRenews = new HashSet<HrRenew>();
            HrStaffEntityServiceMaps = new HashSet<HrStaffEntityServiceMap>();
            HrStaffExpertises = new HashSet<HrStaffExpertise>();
            HrStaffHistories = new HashSet<HrStaffHistory>();
            HrStaffMarketSegments = new HashSet<HrStaffMarketSegment>();
            HrStaffOpCountries = new HashSet<HrStaffOpCountry>();
            HrStaffPhotos = new HashSet<HrStaffPhoto>();
            HrStaffProductCategories = new HashSet<HrStaffProductCategory>();
            HrStaffProfiles = new HashSet<HrStaffProfile>();
            HrStaffServices = new HashSet<HrStaffService>();
            HrStaffTrainings = new HashSet<HrStaffTraining>();
            InvDaTransactions = new HashSet<InvDaTransaction>();
            InverseManager = new HashSet<HrStaff>();
            InverseParentStaff = new HashSet<HrStaff>();
            ItUserMasters = new HashSet<ItUserMaster>();
            OmDetails = new HashSet<OmDetail>();
            QcBlockLists = new HashSet<QcBlockList>();
            QuQuotationContacts = new HashSet<QuQuotationContact>();
            SchScheduleCS = new HashSet<SchScheduleC>();
            SchScheduleQcs = new HashSet<SchScheduleQc>();
        }

        public int Id { get; set; }
        [Column("Person_Name")]
        [StringLength(50)]
        public string PersonName { get; set; }
        [StringLength(1)]
        public string Gender { get; set; }
        [Column("Marital_Status")]
        [StringLength(1)]
        public string MaritalStatus { get; set; }
        [Column("Birth_Date", TypeName = "datetime")]
        public DateTime? BirthDate { get; set; }
        [Column("Join_Date", TypeName = "datetime")]
        public DateTime? JoinDate { get; set; }
        [Column("salary_Currency_Id")]
        public int? SalaryCurrencyId { get; set; }
        [Column("Parent_Staff_Id")]
        public int? ParentStaffId { get; set; }
        [Column("Department_Id")]
        public int? DepartmentId { get; set; }
        [Column("Qualification_Id")]
        public int? QualificationId { get; set; }
        [Column("Location_Id")]
        public int? LocationId { get; set; }
        public bool? Active { get; set; }
        [Column("Paid_Holiday_Days_Per_Year")]
        public double? PaidHolidayDaysPerYear { get; set; }
        [StringLength(400)]
        public string Comments { get; set; }
        [Column("Nationality_Country_Id")]
        public int? NationalityCountryId { get; set; }
        [Column("Name_Chinese")]
        [StringLength(50)]
        public string NameChinese { get; set; }
        [Column("Working_Days_Of_Week")]
        public double? WorkingDaysOfWeek { get; set; }
        [Column("Passport_no")]
        [StringLength(100)]
        public string PassportNo { get; set; }
        [Column("Probation_Period")]
        public int? ProbationPeriod { get; set; }
        [Column("place_of_Birth")]
        [StringLength(100)]
        public string PlaceOfBirth { get; set; }
        [Column("Bank_Name")]
        [StringLength(100)]
        public string BankName { get; set; }
        [Column("Bank_Account_No")]
        [StringLength(50)]
        public string BankAccountNo { get; set; }
        public double? Salary { get; set; }
        [Column("Emp_no")]
        [StringLength(50)]
        public string EmpNo { get; set; }
        [Column("Emergency_Call")]
        [StringLength(50)]
        public string EmergencyCall { get; set; }
        [Column("Leave_Date", TypeName = "datetime")]
        public DateTime? LeaveDate { get; set; }
        [Column("GL_Code")]
        [StringLength(50)]
        public string GlCode { get; set; }
        [Column("Probation_Expired_Date", TypeName = "datetime")]
        public DateTime? ProbationExpiredDate { get; set; }
        [Column("Labor_Contract_Expired_Date", TypeName = "datetime")]
        public DateTime? LaborContractExpiredDate { get; set; }
        [Column("Created_At", TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }
        [Column("Created_By", TypeName = "numeric(18, 0)")]
        public decimal? CreatedBy { get; set; }
        [Column("Modified_At", TypeName = "datetime")]
        public DateTime? ModifiedAt { get; set; }
        [Column("Modified_By", TypeName = "numeric(18, 0)")]
        public decimal? ModifiedBy { get; set; }
        [Column("Prefer_Currency_Id")]
        public int? PreferCurrencyId { get; set; }
        public int? OutSource { get; set; }
        [Column("Position_Id")]
        public int? PositionId { get; set; }
        [Column("Start_Port")]
        public int? StartPort { get; set; }
        [StringLength(200)]
        public string GraduateSchool { get; set; }
        [StringLength(200)]
        public string EmergencyContactName { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? GraduateDate { get; set; }
        [StringLength(100)]
        public string EmergencyContactPhone { get; set; }
        [Column("EmaiLAddress")]
        [StringLength(50)]
        public string EmaiLaddress { get; set; }
        [StringLength(50)]
        public string CompanyMobileNo { get; set; }
        [StringLength(50)]
        public string SocialInsuranceCardNo { get; set; }
        [StringLength(50)]
        public string HousingFuncard { get; set; }
        [Column("PlacePurchasingSIHF")]
        [StringLength(50)]
        public string PlacePurchasingSihf { get; set; }
        [StringLength(50)]
        public string LaborContractPeriod { get; set; }
        [Column("Current_ZipCode")]
        [StringLength(50)]
        public string CurrentZipCode { get; set; }
        [Column("Current_Town")]
        [StringLength(50)]
        public string CurrentTown { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? StartWorkingDate { get; set; }
        public int? TotalWorkingYears { get; set; }
        [Column("Home_CityId")]
        public int? HomeCityId { get; set; }
        [Column("Home_Address")]
        [StringLength(200)]
        public string HomeAddress { get; set; }
        [Column("Current_CityId")]
        public int? CurrentCityId { get; set; }
        [Column("Current_Address")]
        [StringLength(200)]
        public string CurrentAddress { get; set; }
        public int? PayrollCurrencyId { get; set; }
        public int EmployeeTypeId { get; set; }
        public int? ManagerId { get; set; }
        [StringLength(10)]
        public string SkypeId { get; set; }
        [StringLength(10)]
        public string LocalLanguage { get; set; }
        [StringLength(100)]
        public string CompanyEmail { get; set; }
        [StringLength(10)]
        public string AnnualLeave { get; set; }
        public bool? IsForecastApplicable { get; set; }
        [Column("Current_CountyId")]
        public int? CurrentCountyId { get; set; }
        public int? StatusId { get; set; }
        public int? BandId { get; set; }
        public int? SocialInsuranceTypeId { get; set; }
        public int? HukoLocationId { get; set; }
        [StringLength(500)]
        public string MajorSubject { get; set; }
        [StringLength(500)]
        public string EmergencyContactRelationship { get; set; }
        [StringLength(500)]
        public string GlobalGrading { get; set; }
        public int? NoticePeriod { get; set; }
        public int? PrimaryEntity { get; set; }
        public int? StartPortId { get; set; }
        [Column("HROutSourceCompanyId")]
        public int? HroutSourceCompanyId { get; set; }
        public int? CompanyId { get; set; }
        public int? PayrollCompany { get; set; }
        [Column("Xero_DeptId")]
        public int? XeroDeptId { get; set; }

        [ForeignKey("BandId")]
        [InverseProperty("HrStaffs")]
        public virtual HrRefBand Band { get; set; }
        [ForeignKey("CompanyId")]
        [InverseProperty("HrStaffCompanies")]
        public virtual ApEntity Company { get; set; }
        [ForeignKey("CurrentCityId")]
        [InverseProperty("HrStaffCurrentCities")]
        public virtual RefCity CurrentCity { get; set; }
        [ForeignKey("CurrentCountyId")]
        [InverseProperty("HrStaffs")]
        public virtual RefCounty CurrentCounty { get; set; }
        [ForeignKey("DepartmentId")]
        [InverseProperty("HrStaffs")]
        public virtual HrDepartment Department { get; set; }
        [ForeignKey("EmployeeTypeId")]
        [InverseProperty("HrStaffs")]
        public virtual HrEmployeeType EmployeeType { get; set; }
        [ForeignKey("HomeCityId")]
        [InverseProperty("HrStaffHomeCities")]
        public virtual RefCity HomeCity { get; set; }
        [ForeignKey("HroutSourceCompanyId")]
        [InverseProperty("HrStaffs")]
        public virtual HrOutSourceCompany HroutSourceCompany { get; set; }
        [ForeignKey("HukoLocationId")]
        [InverseProperty("HrStaffHukoLocations")]
        public virtual RefCity HukoLocation { get; set; }
        [ForeignKey("LocationId")]
        [InverseProperty("HrStaffs")]
        public virtual RefLocation Location { get; set; }
        [ForeignKey("ManagerId")]
        [InverseProperty("InverseManager")]
        public virtual HrStaff Manager { get; set; }
        [ForeignKey("NationalityCountryId")]
        [InverseProperty("HrStaffs")]
        public virtual RefCountry NationalityCountry { get; set; }
        [ForeignKey("ParentStaffId")]
        [InverseProperty("InverseParentStaff")]
        public virtual HrStaff ParentStaff { get; set; }
        [ForeignKey("PayrollCompany")]
        [InverseProperty("HrStaffs")]
        public virtual HrPayrollCompany PayrollCompanyNavigation { get; set; }
        [ForeignKey("PayrollCurrencyId")]
        [InverseProperty("HrStaffPayrollCurrencies")]
        public virtual RefCurrency PayrollCurrency { get; set; }
        [ForeignKey("PositionId")]
        [InverseProperty("HrStaffs")]
        public virtual HrPosition Position { get; set; }
        [ForeignKey("PreferCurrencyId")]
        [InverseProperty("HrStaffPreferCurrencies")]
        public virtual RefCurrency PreferCurrency { get; set; }
        [ForeignKey("PrimaryEntity")]
        [InverseProperty("HrStaffPrimaryEntityNavigations")]
        public virtual ApEntity PrimaryEntityNavigation { get; set; }
        [ForeignKey("QualificationId")]
        [InverseProperty("HrStaffs")]
        public virtual HrQualification Qualification { get; set; }
        [ForeignKey("SalaryCurrencyId")]
        [InverseProperty("HrStaffSalaryCurrencies")]
        public virtual RefCurrency SalaryCurrency { get; set; }
        [ForeignKey("SocialInsuranceTypeId")]
        [InverseProperty("HrStaffs")]
        public virtual HrRefSocialInsuranceType SocialInsuranceType { get; set; }
        [ForeignKey("StartPortId")]
        [InverseProperty("HrStaffs")]
        public virtual EcAutRefStartPort StartPortNavigation { get; set; }
        [ForeignKey("StatusId")]
        [InverseProperty("HrStaffs")]
        public virtual HrRefStatus Status { get; set; }
        [ForeignKey("XeroDeptId")]
        [InverseProperty("HrStaffs")]
        public virtual HrStaffXeroDept XeroDept { get; set; }
        [InverseProperty("Staff")]
        public virtual ICollection<AudTranAuditor> AudTranAuditors { get; set; }
        [InverseProperty("Staff")]
        public virtual ICollection<AudTranC> AudTranCS { get; set; }
        [InverseProperty("PsersonInChargeNavigation")]
        public virtual ICollection<CompTranPersonInCharge> CompTranPersonInCharges { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<CuCsConfiguration> CuCsConfigurations { get; set; }
        [InverseProperty("KamNavigation")]
        public virtual ICollection<CuCustomer> CuCustomers { get; set; }
        [InverseProperty("Kam")]
        public virtual ICollection<CuKam> CuKams { get; set; }
        [InverseProperty("Staff")]
        public virtual ICollection<CuSalesIncharge> CuSalesIncharges { get; set; }
        [InverseProperty("IdStaffNavigation")]
        public virtual ICollection<DmRight> DmRights { get; set; }
        [InverseProperty("Staff")]
        public virtual ICollection<DmRole> DmRoles { get; set; }
        [InverseProperty("Qc")]
        public virtual ICollection<EcAutQcFoodExpense> EcAutQcFoodExpenses { get; set; }
        [InverseProperty("Qc")]
        public virtual ICollection<EcAutQcTravelExpense> EcAutQcTravelExpenses { get; set; }
        [InverseProperty("Staff")]
        public virtual ICollection<EcExpencesClaim> EcExpencesClaims { get; set; }
        [InverseProperty("ApiContact")]
        public virtual ICollection<EsApiContact> EsApiContacts { get; set; }
        [InverseProperty("ApiContact")]
        public virtual ICollection<EsApiDefaultContact> EsApiDefaultContacts { get; set; }
        [InverseProperty("Staff")]
        public virtual ICollection<HrAttachment> HrAttachments { get; set; }
        [InverseProperty("Staff")]
        public virtual ICollection<HrEntityMap> HrEntityMaps { get; set; }
        [InverseProperty("Staff")]
        public virtual ICollection<HrFileAttachment> HrFileAttachments { get; set; }
        [InverseProperty("Staff")]
        public virtual ICollection<HrLeave> HrLeaves { get; set; }
        [InverseProperty("Staff")]
        public virtual ICollection<HrOfficeControl> HrOfficeControls { get; set; }
        [InverseProperty("Staff")]
        public virtual ICollection<HrPhoto> HrPhotos { get; set; }
        [InverseProperty("Staff")]
        public virtual ICollection<HrRenew> HrRenews { get; set; }
        [InverseProperty("Staff")]
        public virtual ICollection<HrStaffEntityServiceMap> HrStaffEntityServiceMaps { get; set; }
        [InverseProperty("Staff")]
        public virtual ICollection<HrStaffExpertise> HrStaffExpertises { get; set; }
        [InverseProperty("Staff")]
        public virtual ICollection<HrStaffHistory> HrStaffHistories { get; set; }
        [InverseProperty("Staff")]
        public virtual ICollection<HrStaffMarketSegment> HrStaffMarketSegments { get; set; }
        [InverseProperty("Staff")]
        public virtual ICollection<HrStaffOpCountry> HrStaffOpCountries { get; set; }
        [InverseProperty("Staff")]
        public virtual ICollection<HrStaffPhoto> HrStaffPhotos { get; set; }
        [InverseProperty("Staff")]
        public virtual ICollection<HrStaffProductCategory> HrStaffProductCategories { get; set; }
        [InverseProperty("Staff")]
        public virtual ICollection<HrStaffProfile> HrStaffProfiles { get; set; }
        [InverseProperty("Staff")]
        public virtual ICollection<HrStaffService> HrStaffServices { get; set; }
        [InverseProperty("Staff")]
        public virtual ICollection<HrStaffTraining> HrStaffTrainings { get; set; }
        [InverseProperty("Staff")]
        public virtual ICollection<InvDaTransaction> InvDaTransactions { get; set; }
        [InverseProperty("Manager")]
        public virtual ICollection<HrStaff> InverseManager { get; set; }
        [InverseProperty("ParentStaff")]
        public virtual ICollection<HrStaff> InverseParentStaff { get; set; }
        [InverseProperty("Staff")]
        public virtual ICollection<ItUserMaster> ItUserMasters { get; set; }
        [InverseProperty("Qc")]
        public virtual ICollection<OmDetail> OmDetails { get; set; }
        [InverseProperty("Qc")]
        public virtual ICollection<QcBlockList> QcBlockLists { get; set; }
        [InverseProperty("IdContactNavigation")]
        public virtual ICollection<QuQuotationContact> QuQuotationContacts { get; set; }
        [InverseProperty("Cs")]
        public virtual ICollection<SchScheduleC> SchScheduleCS { get; set; }
        [InverseProperty("Qc")]
        public virtual ICollection<SchScheduleQc> SchScheduleQcs { get; set; }
    }
}