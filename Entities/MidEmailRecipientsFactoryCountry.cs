using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("MID_Email_Recipients_FactoryCountry")]
    public partial class MidEmailRecipientsFactoryCountry
    {
        public int Id { get; set; }
        public int EmailConfigId { get; set; }
        [Column("Factory_CountryId")]
        public int? FactoryCountryId { get; set; }
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
        [InverseProperty("MidEmailRecipientsFactoryCountryCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("MidEmailRecipientsFactoryCountryDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("EmailConfigId")]
        [InverseProperty("MidEmailRecipientsFactoryCountries")]
        public virtual MidEmailRecipientsConfiguration EmailConfig { get; set; }
        [ForeignKey("FactoryCountryId")]
        [InverseProperty("MidEmailRecipientsFactoryCountries")]
        public virtual RefCountry FactoryCountry { get; set; }
        [ForeignKey("ModifiedBy")]
        [InverseProperty("MidEmailRecipientsFactoryCountryModifiedByNavigations")]
        public virtual ItUserMaster ModifiedByNavigation { get; set; }
    }
}