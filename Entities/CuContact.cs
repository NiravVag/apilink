using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_Contact")]
    public partial class CuContact
    {
        public CuContact()
        {
            AudTranCuContacts = new HashSet<AudTranCuContact>();
            CuContactBrands = new HashSet<CuContactBrand>();
            CuContactDepartments = new HashSet<CuContactDepartment>();
            CuContactEntityMaps = new HashSet<CuContactEntityMap>();
            CuContactEntityServiceMaps = new HashSet<CuContactEntityServiceMap>();
            CuContactServices = new HashSet<CuContactService>();
            CuContactSisterCompanies = new HashSet<CuContactSisterCompany>();
            CuCustomerContactTypes = new HashSet<CuCustomerContactType>();
            EsCuContacts = new HashSet<EsCuContact>();
            InspTranCuContacts = new HashSet<InspTranCuContact>();
            InspTranCuMerchandisers = new HashSet<InspTranCuMerchandiser>();
            InspTranPickingContacts = new HashSet<InspTranPickingContact>();
            InvAutTranContactDetails = new HashSet<InvAutTranContactDetail>();
            InvCreTranContacts = new HashSet<InvCreTranContact>();
            InvExfContactDetails = new HashSet<InvExfContactDetail>();
            InvTranInvoiceRequestContacts = new HashSet<InvTranInvoiceRequestContact>();
            InverseReportToNavigation = new HashSet<CuContact>();
            ItUserMasters = new HashSet<ItUserMaster>();
            QuQuotationCustomerContacts = new HashSet<QuQuotationCustomerContact>();
        }

        public int Id { get; set; }
        [Column("Customer_id")]
        public int CustomerId { get; set; }
        [Column("Contact_name")]
        [StringLength(1200)]
        public string ContactName { get; set; }
        [Column("Job_Title")]
        [StringLength(250)]
        public string JobTitle { get; set; }
        [Required]
        [StringLength(100)]
        public string Email { get; set; }
        [StringLength(20)]
        public string Mobile { get; set; }
        [Required]
        [StringLength(100)]
        public string Phone { get; set; }
        [StringLength(200)]
        public string Fax { get; set; }
        [StringLength(2200)]
        public string Others { get; set; }
        public int? Office { get; set; }
        [StringLength(2200)]
        public string Comments { get; set; }
        [Column("Promotional_Email")]
        public bool? PromotionalEmail { get; set; }
        [Required]
        public bool? Active { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public long? ZohoCustomerId { get; set; }
        public long? ZohoContactId { get; set; }
        public int? PrimaryEntity { get; set; }
        public int? ReportTo { get; set; }
        [StringLength(500)]
        public string LastName { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("CuContactCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("CustomerId")]
        [InverseProperty("CuContacts")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("CuContactDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("Office")]
        [InverseProperty("CuContacts")]
        public virtual CuAddress OfficeNavigation { get; set; }
        [ForeignKey("PrimaryEntity")]
        [InverseProperty("CuContacts")]
        public virtual ApEntity PrimaryEntityNavigation { get; set; }
        [ForeignKey("ReportTo")]
        [InverseProperty("InverseReportToNavigation")]
        public virtual CuContact ReportToNavigation { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("CuContactUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
        [InverseProperty("Contact")]
        public virtual ICollection<AudTranCuContact> AudTranCuContacts { get; set; }
        [InverseProperty("Contact")]
        public virtual ICollection<CuContactBrand> CuContactBrands { get; set; }
        [InverseProperty("Contact")]
        public virtual ICollection<CuContactDepartment> CuContactDepartments { get; set; }
        [InverseProperty("Contact")]
        public virtual ICollection<CuContactEntityMap> CuContactEntityMaps { get; set; }
        [InverseProperty("Contact")]
        public virtual ICollection<CuContactEntityServiceMap> CuContactEntityServiceMaps { get; set; }
        [InverseProperty("Contact")]
        public virtual ICollection<CuContactService> CuContactServices { get; set; }
        [InverseProperty("Contact")]
        public virtual ICollection<CuContactSisterCompany> CuContactSisterCompanies { get; set; }
        [InverseProperty("Contact")]
        public virtual ICollection<CuCustomerContactType> CuCustomerContactTypes { get; set; }
        [InverseProperty("CustomerContact")]
        public virtual ICollection<EsCuContact> EsCuContacts { get; set; }
        [InverseProperty("Contact")]
        public virtual ICollection<InspTranCuContact> InspTranCuContacts { get; set; }
        [InverseProperty("Merchandiser")]
        public virtual ICollection<InspTranCuMerchandiser> InspTranCuMerchandisers { get; set; }
        [InverseProperty("CusContact")]
        public virtual ICollection<InspTranPickingContact> InspTranPickingContacts { get; set; }
        [InverseProperty("CustomerContact")]
        public virtual ICollection<InvAutTranContactDetail> InvAutTranContactDetails { get; set; }
        [InverseProperty("CustomerContact")]
        public virtual ICollection<InvCreTranContact> InvCreTranContacts { get; set; }
        [InverseProperty("CustomerContact")]
        public virtual ICollection<InvExfContactDetail> InvExfContactDetails { get; set; }
        [InverseProperty("Contact")]
        public virtual ICollection<InvTranInvoiceRequestContact> InvTranInvoiceRequestContacts { get; set; }
        [InverseProperty("ReportToNavigation")]
        public virtual ICollection<CuContact> InverseReportToNavigation { get; set; }
        [InverseProperty("CustomerContact")]
        public virtual ICollection<ItUserMaster> ItUserMasters { get; set; }
        [InverseProperty("IdContactNavigation")]
        public virtual ICollection<QuQuotationCustomerContact> QuQuotationCustomerContacts { get; set; }
    }
}