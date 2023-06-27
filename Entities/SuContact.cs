using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("SU_Contact")]
    public partial class SuContact
    {
        public SuContact()
        {
            AudTranFaContacts = new HashSet<AudTranFaContact>();
            AudTranSuContacts = new HashSet<AudTranSuContact>();
            InspTranFaContacts = new HashSet<InspTranFaContact>();
            InspTranSuContacts = new HashSet<InspTranSuContact>();
            InvAutTranContactDetailFactoryContacts = new HashSet<InvAutTranContactDetail>();
            InvAutTranContactDetailSupplierContacts = new HashSet<InvAutTranContactDetail>();
            InvExfContactDetailFactoryContacts = new HashSet<InvExfContactDetail>();
            InvExfContactDetailSupplierContacts = new HashSet<InvExfContactDetail>();
            ItUserMasterFactoryContacts = new HashSet<ItUserMaster>();
            ItUserMasterSupplierContacts = new HashSet<ItUserMaster>();
            QuQuotationFactoryContacts = new HashSet<QuQuotationFactoryContact>();
            QuQuotationSupplierContacts = new HashSet<QuQuotationSupplierContact>();
            SuContactApiServices = new HashSet<SuContactApiService>();
            SuContactEntityMaps = new HashSet<SuContactEntityMap>();
            SuContactEntityServiceMaps = new HashSet<SuContactEntityServiceMap>();
            SuSupplierCustomerContacts = new HashSet<SuSupplierCustomerContact>();
        }

        public int Id { get; set; }
        [Column("Supplier_id")]
        public int SupplierId { get; set; }
        [Column("Contact_name")]
        [StringLength(100)]
        public string ContactName { get; set; }
        [StringLength(200)]
        public string Phone { get; set; }
        [StringLength(200)]
        public string Fax { get; set; }
        [StringLength(100)]
        public string Mail { get; set; }
        [Required]
        [Column("active")]
        public bool? Active { get; set; }
        [StringLength(500)]
        public string JobTitle { get; set; }
        [StringLength(100)]
        public string Mobile { get; set; }
        [StringLength(1000)]
        public string Comment { get; set; }
        public int? PrimaryEntity { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("SuContacts")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("PrimaryEntity")]
        [InverseProperty("SuContacts")]
        public virtual ApEntity PrimaryEntityNavigation { get; set; }
        [ForeignKey("SupplierId")]
        [InverseProperty("SuContacts")]
        public virtual SuSupplier Supplier { get; set; }
        [InverseProperty("Contact")]
        public virtual ICollection<AudTranFaContact> AudTranFaContacts { get; set; }
        [InverseProperty("Contact")]
        public virtual ICollection<AudTranSuContact> AudTranSuContacts { get; set; }
        [InverseProperty("Contact")]
        public virtual ICollection<InspTranFaContact> InspTranFaContacts { get; set; }
        [InverseProperty("Contact")]
        public virtual ICollection<InspTranSuContact> InspTranSuContacts { get; set; }
        [InverseProperty("FactoryContact")]
        public virtual ICollection<InvAutTranContactDetail> InvAutTranContactDetailFactoryContacts { get; set; }
        [InverseProperty("SupplierContact")]
        public virtual ICollection<InvAutTranContactDetail> InvAutTranContactDetailSupplierContacts { get; set; }
        [InverseProperty("FactoryContact")]
        public virtual ICollection<InvExfContactDetail> InvExfContactDetailFactoryContacts { get; set; }
        [InverseProperty("SupplierContact")]
        public virtual ICollection<InvExfContactDetail> InvExfContactDetailSupplierContacts { get; set; }
        [InverseProperty("FactoryContact")]
        public virtual ICollection<ItUserMaster> ItUserMasterFactoryContacts { get; set; }
        [InverseProperty("SupplierContact")]
        public virtual ICollection<ItUserMaster> ItUserMasterSupplierContacts { get; set; }
        [InverseProperty("IdContactNavigation")]
        public virtual ICollection<QuQuotationFactoryContact> QuQuotationFactoryContacts { get; set; }
        [InverseProperty("IdContactNavigation")]
        public virtual ICollection<QuQuotationSupplierContact> QuQuotationSupplierContacts { get; set; }
        [InverseProperty("Contact")]
        public virtual ICollection<SuContactApiService> SuContactApiServices { get; set; }
        [InverseProperty("Contact")]
        public virtual ICollection<SuContactEntityMap> SuContactEntityMaps { get; set; }
        [InverseProperty("Contact")]
        public virtual ICollection<SuContactEntityServiceMap> SuContactEntityServiceMaps { get; set; }
        [InverseProperty("Contact")]
        public virtual ICollection<SuSupplierCustomerContact> SuSupplierCustomerContacts { get; set; }
    }
}