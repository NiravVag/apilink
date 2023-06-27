using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("ES_Sup_Fact_Config")]
    public partial class EsSupFactConfig
    {
        public int Id { get; set; }
        [Column("Supplier_OR_Factory_Id")]
        public int SupplierOrFactoryId { get; set; }
        public int EsDetailsId { get; set; }

        [ForeignKey("EsDetailsId")]
        [InverseProperty("EsSupFactConfigs")]
        public virtual EsDetail EsDetails { get; set; }
        [ForeignKey("SupplierOrFactoryId")]
        [InverseProperty("EsSupFactConfigs")]
        public virtual SuSupplier SupplierOrFactory { get; set; }
    }
}