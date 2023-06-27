using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_Contact_SisterCompany")]
    public partial class CuContactSisterCompany
    {
        public int Id { get; set; }
        public int ContactId { get; set; }
        public bool? Active { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }        
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public int EntityId { get; set; }
        public int SisterCompanyId { get; set; }

        [ForeignKey("ContactId")]
        [InverseProperty("CuContactSisterCompanies")]
        public virtual CuContact Contact { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("CuContactSisterCompanyCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("CuContactSisterCompanyDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("CuContactSisterCompanies")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("SisterCompanyId")]
        [InverseProperty("CuContactSisterCompanies")]
        public virtual CuCustomer SisterCompany { get; set; }        
    }
}