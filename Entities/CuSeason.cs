using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_Season")]
    public partial class CuSeason
    {
        public int Id { get; set; }
        [Column("Customer_Id")]
        public int CustomerId { get; set; }
        [Column("Season_Id")]
        public int SeasonId { get; set; }
        public bool Active { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("CustomerId")]
        [InverseProperty("CuSeasons")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("CuSeasons")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("SeasonId")]
        [InverseProperty("CuSeasons")]
        public virtual RefSeason Season { get; set; }
    }
}