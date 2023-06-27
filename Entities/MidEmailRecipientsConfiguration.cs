using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("MID_Email_Recipients_Configuration")]
    public partial class MidEmailRecipientsConfiguration
    {
        public MidEmailRecipientsConfiguration()
        {
            MidEmailRecipientsCusBrands = new HashSet<MidEmailRecipientsCusBrand>();
            MidEmailRecipientsCusBuyers = new HashSet<MidEmailRecipientsCusBuyer>();
            MidEmailRecipientsCusContacts = new HashSet<MidEmailRecipientsCusContact>();
            MidEmailRecipientsCusDepartments = new HashSet<MidEmailRecipientsCusDepartment>();
            MidEmailRecipientsCustomers = new HashSet<MidEmailRecipientsCustomer>();
            MidEmailRecipientsDestinationCountries = new HashSet<MidEmailRecipientsDestinationCountry>();
            MidEmailRecipientsFactContacts = new HashSet<MidEmailRecipientsFactContact>();
            MidEmailRecipientsFactories = new HashSet<MidEmailRecipientsFactory>();
            MidEmailRecipientsFactoryCountries = new HashSet<MidEmailRecipientsFactoryCountry>();
            MidEmailRecipientsInternals = new HashSet<MidEmailRecipientsInternal>();
            MidEmailRecipientsOffices = new HashSet<MidEmailRecipientsOffice>();
            MidEmailRecipientsProductCategories = new HashSet<MidEmailRecipientsProductCategory>();
            MidEmailRecipientsProductSubCategories = new HashSet<MidEmailRecipientsProductSubCategory>();
            MidEmailRecipientsServiceTypes = new HashSet<MidEmailRecipientsServiceType>();
            MidEmailRecipientsServices = new HashSet<MidEmailRecipientsService>();
            MidEmailRecipientsSupContacts = new HashSet<MidEmailRecipientsSupContact>();
            MidEmailRecipientsSuppliers = new HashSet<MidEmailRecipientsSupplier>();
        }

        public int Id { get; set; }
        public int? EmailTypeId { get; set; }
        [Column("Is_CUS_BookingContact")]
        public bool? IsCusBookingContact { get; set; }
        [Column("Is_SUP_BookingContact")]
        public bool? IsSupBookingContact { get; set; }
        [Column("Is_FACT_BookingContact")]
        public bool? IsFactBookingContact { get; set; }
        public bool? Active { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("MidEmailRecipientsConfigurationCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("MidEmailRecipientsConfigurationDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("EmailTypeId")]
        [InverseProperty("MidEmailRecipientsConfigurations")]
        public virtual MidEmailType EmailType { get; set; }
        [ForeignKey("ModifiedBy")]
        [InverseProperty("MidEmailRecipientsConfigurationModifiedByNavigations")]
        public virtual ItUserMaster ModifiedByNavigation { get; set; }
        [InverseProperty("EmailConfig")]
        public virtual ICollection<MidEmailRecipientsCusBrand> MidEmailRecipientsCusBrands { get; set; }
        [InverseProperty("EmailConfig")]
        public virtual ICollection<MidEmailRecipientsCusBuyer> MidEmailRecipientsCusBuyers { get; set; }
        [InverseProperty("EmailConfig")]
        public virtual ICollection<MidEmailRecipientsCusContact> MidEmailRecipientsCusContacts { get; set; }
        [InverseProperty("EmailConfig")]
        public virtual ICollection<MidEmailRecipientsCusDepartment> MidEmailRecipientsCusDepartments { get; set; }
        [InverseProperty("EmailConfig")]
        public virtual ICollection<MidEmailRecipientsCustomer> MidEmailRecipientsCustomers { get; set; }
        [InverseProperty("EmailConfig")]
        public virtual ICollection<MidEmailRecipientsDestinationCountry> MidEmailRecipientsDestinationCountries { get; set; }
        [InverseProperty("EmailConfig")]
        public virtual ICollection<MidEmailRecipientsFactContact> MidEmailRecipientsFactContacts { get; set; }
        [InverseProperty("EmailConfig")]
        public virtual ICollection<MidEmailRecipientsFactory> MidEmailRecipientsFactories { get; set; }
        [InverseProperty("EmailConfig")]
        public virtual ICollection<MidEmailRecipientsFactoryCountry> MidEmailRecipientsFactoryCountries { get; set; }
        [InverseProperty("EmailConfig")]
        public virtual ICollection<MidEmailRecipientsInternal> MidEmailRecipientsInternals { get; set; }
        [InverseProperty("EmailConfig")]
        public virtual ICollection<MidEmailRecipientsOffice> MidEmailRecipientsOffices { get; set; }
        [InverseProperty("EmailConfig")]
        public virtual ICollection<MidEmailRecipientsProductCategory> MidEmailRecipientsProductCategories { get; set; }
        [InverseProperty("EmailConfig")]
        public virtual ICollection<MidEmailRecipientsProductSubCategory> MidEmailRecipientsProductSubCategories { get; set; }
        [InverseProperty("EmailConfig")]
        public virtual ICollection<MidEmailRecipientsServiceType> MidEmailRecipientsServiceTypes { get; set; }
        [InverseProperty("EmailConfig")]
        public virtual ICollection<MidEmailRecipientsService> MidEmailRecipientsServices { get; set; }
        [InverseProperty("EmailConfig")]
        public virtual ICollection<MidEmailRecipientsSupContact> MidEmailRecipientsSupContacts { get; set; }
        [InverseProperty("EmailConfig")]
        public virtual ICollection<MidEmailRecipientsSupplier> MidEmailRecipientsSuppliers { get; set; }
    }
}