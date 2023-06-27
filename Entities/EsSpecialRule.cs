using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("ES_Special_Rule")]
    public partial class EsSpecialRule
    {
        public int Id { get; set; }
        [Column("Special_Rule_Id")]
        public int? SpecialRuleId { get; set; }
        [Column("Es_Details_Id")]
        public int? EsDetailsId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("EsSpecialRules")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("EsDetailsId")]
        [InverseProperty("EsSpecialRules")]
        public virtual EsDetail EsDetails { get; set; }
        [ForeignKey("SpecialRuleId")]
        [InverseProperty("EsSpecialRules")]
        public virtual EsRefSpecialRule SpecialRule { get; set; }
    }
}