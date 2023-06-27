using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("DF_CU_DDL_SourceType")]
    public partial class DfCuDdlSourceType
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int TypeId { get; set; }
        public bool Active { get; set; }

        [ForeignKey("CustomerId")]
        [InverseProperty("DfCuDdlSourceTypes")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("TypeId")]
        [InverseProperty("DfCuDdlSourceTypes")]
        public virtual DfDdlSourceType Type { get; set; }
    }
}