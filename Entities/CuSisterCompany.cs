using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_SisterCompany")]
    public partial class CuSisterCompany
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public bool? Active { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }        
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public int EntityId { get; set; }
        public int SisterCompanyId { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("CuSisterCompanyCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("CustomerId")]
        [InverseProperty("CuSisterCompanyCustomers")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("CuSisterCompanyDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("CuSisterCompanies")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("SisterCompanyId")]
        [InverseProperty("CuSisterCompanySisterCompanies")]
        public virtual CuCustomer SisterCompany { get; set; } 
    }
}