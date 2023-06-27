using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("QC_BL_Customer")]
    public partial class QcBlCustomer
    {
        public int Id { get; set; }
        [Column("QCBLId")]
        public int Qcblid { get; set; }
        [Column("Customer_Id")]
        public int? CustomerId { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("QcBlCustomers")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("CustomerId")]
        [InverseProperty("QcBlCustomers")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("Qcblid")]
        [InverseProperty("QcBlCustomers")]
        public virtual QcBlockList Qcbl { get; set; }
    }
}