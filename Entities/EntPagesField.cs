using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("ENT_Pages_Fields")]
    public partial class EntPagesField
    {
        public int Id { get; set; }
        public int? EntityId { get; set; }
        public int? CustomerId { get; set; }
        public bool? Active { get; set; }
        [Column("ENTFieldId")]
        public int? EntfieldId { get; set; }
        public int? UserTypeId { get; set; }

        [ForeignKey("CustomerId")]
        [InverseProperty("EntPagesFields")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("EntfieldId")]
        [InverseProperty("EntPagesFields")]
        public virtual EntField Entfield { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("EntPagesFields")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("UserTypeId")]
        [InverseProperty("EntPagesFields")]
        public virtual ItUserType UserType { get; set; }
    }
}