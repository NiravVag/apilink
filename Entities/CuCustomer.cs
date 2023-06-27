using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_Customer")]
    public partial class CuCustomer
    {
        public CuCustomer()
        {
            AudBookingEmailConfigurations = new HashSet<AudBookingEmailConfiguration>();
            AudBookingRules = new HashSet<AudBookingRule>();
            AudCancelRescheduleReasons = new HashSet<AudCancelRescheduleReason>();
            AudCuProductCategories = new HashSet<AudCuProductCategory>();
            AudTransactions = new HashSet<AudTransaction>();
            CompComplaints = new HashSet<CompComplaint>();
            CuAddresses = new HashSet<CuAddress>();
            CuApiServices = new HashSet<CuApiService>();
            CuBrandpriorities = new HashSet<CuBrandpriority>();
            CuBrands = new HashSet<CuBrand>();
            CuBuyers = new HashSet<CuBuyer>();
            CuCheckPoints = new HashSet<CuCheckPoint>();
            CuCollections = new HashSet<CuCollection>();
            CuContactSisterCompanies = new HashSet<CuContactSisterCompany>();
            CuContacts = new HashSet<CuContact>();
            CuCsConfigurations = new HashSet<CuCsConfiguration>();
            CuCsOnsiteEmails = new HashSet<CuCsOnsiteEmail>();
            CuCustomerBusinessCountries = new HashSet<CuCustomerBusinessCountry>();
            CuCustomerSalesCountries = new HashSet<CuCustomerSalesCountry>();
            CuDepartments = new HashSet<CuDepartment>();
            CuEntities = new HashSet<CuEntity>();
            CuKams = new HashSet<CuKam>();
            CuPrDetails = new HashSet<CuPrDetail>();
            CuPriceCategories = new HashSet<CuPriceCategory>();
            CuPriceCategoryPcsub2S = new HashSet<CuPriceCategoryPcsub2>();
            CuProductCategories = new HashSet<CuProductCategory>();
            CuProductMschartOcrMaps = new HashSet<CuProductMschartOcrMap>();
            CuProductTypes = new HashSet<CuProductType>();
            CuProducts = new HashSet<CuProduct>();
            CuPurchaseOrders = new HashSet<CuPurchaseOrder>();
            CuReportCustomerDecisionComments = new HashSet<CuReportCustomerDecisionComment>();
            CuSalesIncharges = new HashSet<CuSalesIncharge>();
            CuSeasonConfigs = new HashSet<CuSeasonConfig>();
            CuSeasons = new HashSet<CuSeason>();
            CuServiceTypes = new HashSet<CuServiceType>();
            CuSisterCompanyCustomers = new HashSet<CuSisterCompany>();
            CuSisterCompanySisterCompanies = new HashSet<CuSisterCompany>();
            DaUserCustomers = new HashSet<DaUserCustomer>();
            DfCuConfigurations = new HashSet<DfCuConfiguration>();
            DfCuDdlSourceTypes = new HashSet<DfCuDdlSourceType>();
            DmDetails = new HashSet<DmDetail>();
            EntPagesFields = new HashSet<EntPagesField>();
            EsDetails = new HashSet<EsDetail>();
            EsSuTemplateMasters = new HashSet<EsSuTemplateMaster>();
            InspBookingEmailConfigurations = new HashSet<InspBookingEmailConfiguration>();
            InspBookingRules = new HashSet<InspBookingRule>();
            InspCancelReasons = new HashSet<InspCancelReason>();
            InspCuStatuses = new HashSet<InspCuStatus>();
            InspIcTransactions = new HashSet<InspIcTransaction>();
            InspLabCustomerContacts = new HashSet<InspLabCustomerContact>();
            InspLabCustomers = new HashSet<InspLabCustomer>();
            InspRefPaymentOptions = new HashSet<InspRefPaymentOption>();
            InspRepCusDecisionTemplates = new HashSet<InspRepCusDecisionTemplate>();
            InspRescheduleReasons = new HashSet<InspRescheduleReason>();
            InspTranPickings = new HashSet<InspTranPicking>();
            InspTransactionDrafts = new HashSet<InspTransactionDraft>();
            InspTransactions = new HashSet<InspTransaction>();
            InvDaCustomers = new HashSet<InvDaCustomer>();
            InvDisTranDetails = new HashSet<InvDisTranDetail>();
            InvExfTransactions = new HashSet<InvExfTransaction>();
            InvManTransactions = new HashSet<InvManTransaction>();
            InvTmDetails = new HashSet<InvTmDetail>();
            ItRightEntities = new HashSet<ItRightEntity>();
            ItUserMasters = new HashSet<ItUserMaster>();
            OmDetails = new HashSet<OmDetail>();
            QcBlCustomers = new HashSet<QcBlCustomer>();
            QuQuotations = new HashSet<QuQuotation>();
            RefInspCusDecisionConfigs = new HashSet<RefInspCusDecisionConfig>();
            RefKpiTeamplateCustomers = new HashSet<RefKpiTeamplateCustomer>();
            RepFastTemplateConfigs = new HashSet<RepFastTemplateConfig>();
            SuGrades = new HashSet<SuGrade>();
            SuLevelCustoms = new HashSet<SuLevelCustom>();
            SuSupplierCustomerContacts = new HashSet<SuSupplierCustomerContact>();
            SuSupplierCustomers = new HashSet<SuSupplierCustomer>();
        }

        public int Id { get; set; }
        [Required]
        [Column("Customer_Name")]
        [StringLength(500)]
        public string CustomerName { get; set; }
        [StringLength(100)]
        public string Email { get; set; }
        [StringLength(100)]
        public string Fax { get; set; }
        [Column("Complexity_Level")]
        public int? ComplexityLevel { get; set; }
        [Column("Start_Date", TypeName = "datetime")]
        public DateTime? StartDate { get; set; }
        [StringLength(100)]
        public string Website { get; set; }
        [StringLength(100)]
        public string Others { get; set; }
        [Column("Prospect_Status")]
        public int? ProspectStatus { get; set; }
        [Column("Skills_Required")]
        public int? SkillsRequired { get; set; }
        public int? Kam { get; set; }
        [StringLength(100)]
        public string Phone { get; set; }
        public int? Category { get; set; }
        [Column("Marget_Segment")]
        public int? MargetSegment { get; set; }
        [Column("Business_Country")]
        public int? BusinessCountry { get; set; }
        [Column("Other_Phone")]
        [StringLength(100)]
        public string OtherPhone { get; set; }
        public int? Language { get; set; }
        [Column("Business_Type")]
        public int? BusinessType { get; set; }
        [Column("Quatation_Name")]
        [StringLength(500)]
        public string QuatationName { get; set; }
        [Column("Ic_Required")]
        public bool? IcRequired { get; set; }
        [Column("Gl_Code")]
        [StringLength(500)]
        public string GlCode { get; set; }
        public string Comments { get; set; }
        [Column("Gl_Required")]
        public bool? GlRequired { get; set; }
        public int? Group { get; set; }
        [Column("Invoice_Type")]
        public int? InvoiceType { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        [Column("Fb_Cus_Id")]
        public int? FbCusId { get; set; }
        public string BookingDefaultComments { get; set; }
        public bool? Active { get; set; }
        public long? ZohoCustomerId { get; set; }
        public int? AccountingLeaderId { get; set; }
        public int? ActvitiesLevelId { get; set; }
        public int? RelationshipStatusId { get; set; }
        [StringLength(2000)]
        public string DirectCompetitor { get; set; }
        public int? CompanyId { get; set; }
        [Column("IsEAQF")]
        public bool? IsEaqf { get; set; }

        [ForeignKey("AccountingLeaderId")]
        [InverseProperty("CuCustomers")]
        public virtual CuRefAccountingLeader AccountingLeader { get; set; }
        [ForeignKey("ActvitiesLevelId")]
        [InverseProperty("CuCustomers")]
        public virtual CuRefActivitiesLevel ActvitiesLevel { get; set; }
        [ForeignKey("BusinessCountry")]
        [InverseProperty("CuCustomers")]
        public virtual RefCountry BusinessCountryNavigation { get; set; }
        [ForeignKey("BusinessType")]
        [InverseProperty("CuCustomers")]
        public virtual RefBusinessType BusinessTypeNavigation { get; set; }
        [ForeignKey("CompanyId")]
        [InverseProperty("CuCustomers")]
        public virtual ApEntity Company { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("CuCustomerCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("CuCustomerDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("Group")]
        [InverseProperty("CuCustomers")]
        public virtual CuCustomerGroup GroupNavigation { get; set; }
        [ForeignKey("InvoiceType")]
        [InverseProperty("CuCustomers")]
        public virtual RefInvoiceType InvoiceTypeNavigation { get; set; }
        [ForeignKey("Kam")]
        [InverseProperty("CuCustomers")]
        public virtual HrStaff KamNavigation { get; set; }
        [ForeignKey("Language")]
        [InverseProperty("CuCustomers")]
        public virtual Language LanguageNavigation { get; set; }
        [ForeignKey("MargetSegment")]
        [InverseProperty("CuCustomers")]
        public virtual RefMarketSegment MargetSegmentNavigation { get; set; }
        [ForeignKey("ProspectStatus")]
        [InverseProperty("CuCustomers")]
        public virtual RefProspectStatus ProspectStatusNavigation { get; set; }
        [ForeignKey("RelationshipStatusId")]
        [InverseProperty("CuCustomers")]
        public virtual CuRefRelationshipStatus RelationshipStatus { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("CuCustomerUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<AudBookingEmailConfiguration> AudBookingEmailConfigurations { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<AudBookingRule> AudBookingRules { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<AudCancelRescheduleReason> AudCancelRescheduleReasons { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<AudCuProductCategory> AudCuProductCategories { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<AudTransaction> AudTransactions { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<CompComplaint> CompComplaints { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<CuAddress> CuAddresses { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<CuApiService> CuApiServices { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<CuBrandpriority> CuBrandpriorities { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<CuBrand> CuBrands { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<CuBuyer> CuBuyers { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<CuCheckPoint> CuCheckPoints { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<CuCollection> CuCollections { get; set; }
        [InverseProperty("SisterCompany")]
        public virtual ICollection<CuContactSisterCompany> CuContactSisterCompanies { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<CuContact> CuContacts { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<CuCsConfiguration> CuCsConfigurations { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<CuCsOnsiteEmail> CuCsOnsiteEmails { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<CuCustomerBusinessCountry> CuCustomerBusinessCountries { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<CuCustomerSalesCountry> CuCustomerSalesCountries { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<CuDepartment> CuDepartments { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<CuEntity> CuEntities { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<CuKam> CuKams { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<CuPrDetail> CuPrDetails { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<CuPriceCategory> CuPriceCategories { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<CuPriceCategoryPcsub2> CuPriceCategoryPcsub2S { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<CuProductCategory> CuProductCategories { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<CuProductMschartOcrMap> CuProductMschartOcrMaps { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<CuProductType> CuProductTypes { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<CuProduct> CuProducts { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<CuPurchaseOrder> CuPurchaseOrders { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<CuReportCustomerDecisionComment> CuReportCustomerDecisionComments { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<CuSalesIncharge> CuSalesIncharges { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<CuSeasonConfig> CuSeasonConfigs { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<CuSeason> CuSeasons { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<CuServiceType> CuServiceTypes { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<CuSisterCompany> CuSisterCompanyCustomers { get; set; }
        [InverseProperty("SisterCompany")]
        public virtual ICollection<CuSisterCompany> CuSisterCompanySisterCompanies { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<DaUserCustomer> DaUserCustomers { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<DfCuConfiguration> DfCuConfigurations { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<DfCuDdlSourceType> DfCuDdlSourceTypes { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<DmDetail> DmDetails { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<EntPagesField> EntPagesFields { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<EsDetail> EsDetails { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<EsSuTemplateMaster> EsSuTemplateMasters { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<InspBookingEmailConfiguration> InspBookingEmailConfigurations { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<InspBookingRule> InspBookingRules { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<InspCancelReason> InspCancelReasons { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<InspCuStatus> InspCuStatuses { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<InspIcTransaction> InspIcTransactions { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<InspLabCustomerContact> InspLabCustomerContacts { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<InspLabCustomer> InspLabCustomers { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<InspRefPaymentOption> InspRefPaymentOptions { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<InspRepCusDecisionTemplate> InspRepCusDecisionTemplates { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<InspRescheduleReason> InspRescheduleReasons { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<InspTranPicking> InspTranPickings { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<InspTransactionDraft> InspTransactionDrafts { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<InspTransaction> InspTransactions { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<InvDaCustomer> InvDaCustomers { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<InvDisTranDetail> InvDisTranDetails { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<InvExfTransaction> InvExfTransactions { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<InvManTransaction> InvManTransactions { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<InvTmDetail> InvTmDetails { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<ItRightEntity> ItRightEntities { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<ItUserMaster> ItUserMasters { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<OmDetail> OmDetails { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<QcBlCustomer> QcBlCustomers { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<QuQuotation> QuQuotations { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<RefInspCusDecisionConfig> RefInspCusDecisionConfigs { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<RefKpiTeamplateCustomer> RefKpiTeamplateCustomers { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<RepFastTemplateConfig> RepFastTemplateConfigs { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<SuGrade> SuGrades { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<SuLevelCustom> SuLevelCustoms { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<SuSupplierCustomerContact> SuSupplierCustomerContacts { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<SuSupplierCustomer> SuSupplierCustomers { get; set; }
    }
}