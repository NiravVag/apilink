using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INV_EXF_ContactDetails")]
    public partial class InvExfContactDetail
    {
        public int Id { get; set; }
        public int? ExtraFeeId { get; set; }
        public int? CustomerContactId { get; set; }
        public int? SupplierContactId { get; set; }
        public int? FactoryContactId { get; set; }

        [ForeignKey("CustomerContactId")]
        [InverseProperty("InvExfContactDetails")]
        public virtual CuContact CustomerContact { get; set; }
        [ForeignKey("ExtraFeeId")]
        [InverseProperty("InvExfContactDetails")]
        public virtual InvExfTransaction ExtraFee { get; set; }
        [ForeignKey("FactoryContactId")]
        [InverseProperty("InvExfContactDetailFactoryContacts")]
        public virtual SuContact FactoryContact { get; set; }
        [ForeignKey("SupplierContactId")]
        [InverseProperty("InvExfContactDetailSupplierContacts")]
        public virtual SuContact SupplierContact { get; set; }
    }
}