using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("SU_OwnlerShip")]
    public partial class SuOwnlerShip
    {
        public SuOwnlerShip()
        {
            SuSuppliers = new HashSet<SuSupplier>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Column("Name_TranId")]
        public int? NameTranId { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("EntityId")]
        [InverseProperty("SuOwnlerShips")]
        public virtual ApEntity Entity { get; set; }
        [InverseProperty("OwnerShip")]
        public virtual ICollection<SuSupplier> SuSuppliers { get; set; }
    }
}