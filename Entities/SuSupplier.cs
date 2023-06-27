using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("SU_Supplier")]
    public partial class SuSupplier
    {
        public SuSupplier()
        {
            AudTransactionFactories = new HashSet<AudTransaction>();
            AudTransactionSuppliers = new HashSet<AudTransaction>();
            CuPoFactories = new HashSet<CuPoFactory>();
            CuPoSuppliers = new HashSet<CuPoSupplier>();
            CuPrSuppliers = new HashSet<CuPrSupplier>();
            EsSupFactConfigs = new HashSet<EsSupFactConfig>();
            InspIcTransactions = new HashSet<InspIcTransaction>();
            InspTransactionDraftFactories = new HashSet<InspTransactionDraft>();
            InspTransactionDraftSuppliers = new HashSet<InspTransactionDraft>();
            InspTransactionFactories = new HashSet<InspTransaction>();
            InspTransactionSuppliers = new HashSet<InspTransaction>();
            InvExfTransactionFactories = new HashSet<InvExfTransaction>();
            InvExfTransactionSuppliers = new HashSet<InvExfTransaction>();
            InvManTransactions = new HashSet<InvManTransaction>();
            ItUserMasterFactories = new HashSet<ItUserMaster>();
            ItUserMasterSuppliers = new HashSet<ItUserMaster>();
            QcBlSupplierFactories = new HashSet<QcBlSupplierFactory>();
            QuQuotationFactories = new HashSet<QuQuotation>();
            QuQuotationSuppliers = new HashSet<QuQuotation>();
            SuAddresses = new HashSet<SuAddress>();
            SuApiServices = new HashSet<SuApiService>();
            SuContacts = new HashSet<SuContact>();
            SuEntities = new HashSet<SuEntity>();
            SuGrades = new HashSet<SuGrade>();
            SuSupplierCustomerContacts = new HashSet<SuSupplierCustomerContact>();
            SuSupplierCustomers = new HashSet<SuSupplierCustomer>();
            SuSupplierFactoryParents = new HashSet<SuSupplierFactory>();
            SuSupplierFactorySuppliers = new HashSet<SuSupplierFactory>();
        }

        public int Id { get; set; }
        [Required]
        [Column("Supplier_Name")]
        [StringLength(500)]
        public string SupplierName { get; set; }
        [Column("Level_Id")]
        public int? LevelId { get; set; }
        [StringLength(500)]
        public string Comments { get; set; }
        public bool Active { get; set; }
        [Column("Type_id")]
        public int? TypeId { get; set; }
        [StringLength(500)]
        public string LegalName { get; set; }
        [StringLength(500)]
        public string Email { get; set; }
        [StringLength(500)]
        public string Phone { get; set; }
        [StringLength(500)]
        public string Fax { get; set; }
        [StringLength(500)]
        public string Website { get; set; }
        [StringLength(500)]
        public string Mobile { get; set; }
        [StringLength(500)]
        public string LocalName { get; set; }
        [StringLength(500)]
        public string ContactPerson { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        [StringLength(100)]
        public string GlCode { get; set; }
        [Column("OwnerShip_Id")]
        public int? OwnerShipId { get; set; }
        [Column("total_staff")]
        [StringLength(50)]
        public string TotalStaff { get; set; }
        [Column("daily_production")]
        [StringLength(50)]
        public string DailyProduction { get; set; }
        public int? CreditTermId { get; set; }
        public int? StatusId { get; set; }
        public int? ModifiedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        [Column("VATNo")]
        [StringLength(500)]
        public string Vatno { get; set; }
        [Column("Fb_FactSup_Id")]
        public int? FbFactSupId { get; set; }
        public int? CompanyId { get; set; }
        [Column("IsEAQF")]
        public bool? IsEaqf { get; set; }

        [ForeignKey("CompanyId")]
        [InverseProperty("SuSuppliers")]
        public virtual ApEntity Company { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("SuSupplierCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("CreditTermId")]
        [InverseProperty("SuSuppliers")]
        public virtual SuCreditTerm CreditTerm { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("SuSupplierDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("LevelId")]
        [InverseProperty("SuSuppliers")]
        public virtual SuLevel Level { get; set; }
        [ForeignKey("ModifiedBy")]
        [InverseProperty("SuSupplierModifiedByNavigations")]
        public virtual ItUserMaster ModifiedByNavigation { get; set; }
        [ForeignKey("OwnerShipId")]
        [InverseProperty("SuSuppliers")]
        public virtual SuOwnlerShip OwnerShip { get; set; }
        [ForeignKey("StatusId")]
        [InverseProperty("SuSuppliers")]
        public virtual SuStatus Status { get; set; }
        [ForeignKey("TypeId")]
        [InverseProperty("SuSuppliers")]
        public virtual SuType Type { get; set; }
        [InverseProperty("Factory")]
        public virtual ICollection<AudTransaction> AudTransactionFactories { get; set; }
        [InverseProperty("Supplier")]
        public virtual ICollection<AudTransaction> AudTransactionSuppliers { get; set; }
        [InverseProperty("Factory")]
        public virtual ICollection<CuPoFactory> CuPoFactories { get; set; }
        [InverseProperty("Supplier")]
        public virtual ICollection<CuPoSupplier> CuPoSuppliers { get; set; }
        [InverseProperty("Supplier")]
        public virtual ICollection<CuPrSupplier> CuPrSuppliers { get; set; }
        [InverseProperty("SupplierOrFactory")]
        public virtual ICollection<EsSupFactConfig> EsSupFactConfigs { get; set; }
        [InverseProperty("Supplier")]
        public virtual ICollection<InspIcTransaction> InspIcTransactions { get; set; }
        [InverseProperty("Factory")]
        public virtual ICollection<InspTransactionDraft> InspTransactionDraftFactories { get; set; }
        [InverseProperty("Supplier")]
        public virtual ICollection<InspTransactionDraft> InspTransactionDraftSuppliers { get; set; }
        [InverseProperty("Factory")]
        public virtual ICollection<InspTransaction> InspTransactionFactories { get; set; }
        [InverseProperty("Supplier")]
        public virtual ICollection<InspTransaction> InspTransactionSuppliers { get; set; }
        [InverseProperty("Factory")]
        public virtual ICollection<InvExfTransaction> InvExfTransactionFactories { get; set; }
        [InverseProperty("Supplier")]
        public virtual ICollection<InvExfTransaction> InvExfTransactionSuppliers { get; set; }
        [InverseProperty("Supplier")]
        public virtual ICollection<InvManTransaction> InvManTransactions { get; set; }
        [InverseProperty("Factory")]
        public virtual ICollection<ItUserMaster> ItUserMasterFactories { get; set; }
        [InverseProperty("Supplier")]
        public virtual ICollection<ItUserMaster> ItUserMasterSuppliers { get; set; }
        [InverseProperty("SupplierFactory")]
        public virtual ICollection<QcBlSupplierFactory> QcBlSupplierFactories { get; set; }
        [InverseProperty("Factory")]
        public virtual ICollection<QuQuotation> QuQuotationFactories { get; set; }
        [InverseProperty("Supplier")]
        public virtual ICollection<QuQuotation> QuQuotationSuppliers { get; set; }
        [InverseProperty("Supplier")]
        public virtual ICollection<SuAddress> SuAddresses { get; set; }
        [InverseProperty("Supplier")]
        public virtual ICollection<SuApiService> SuApiServices { get; set; }
        [InverseProperty("Supplier")]
        public virtual ICollection<SuContact> SuContacts { get; set; }
        [InverseProperty("Supplier")]
        public virtual ICollection<SuEntity> SuEntities { get; set; }
        [InverseProperty("Supplier")]
        public virtual ICollection<SuGrade> SuGrades { get; set; }
        [InverseProperty("Supplier")]
        public virtual ICollection<SuSupplierCustomerContact> SuSupplierCustomerContacts { get; set; }
        [InverseProperty("Supplier")]
        public virtual ICollection<SuSupplierCustomer> SuSupplierCustomers { get; set; }
        [InverseProperty("Parent")]
        public virtual ICollection<SuSupplierFactory> SuSupplierFactoryParents { get; set; }
        [InverseProperty("Supplier")]
        public virtual ICollection<SuSupplierFactory> SuSupplierFactorySuppliers { get; set; }
    }
}