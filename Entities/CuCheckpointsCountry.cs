using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_CheckPoints_Country")]
    public partial class CuCheckPointsCountry
    {
        public int Id { get; set; }
        public int CheckpointId { get; set; }
        public int CountryId { get; set; }
        public bool Active { get; set; }
        public int? EntityId { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }

        [ForeignKey("CheckpointId")]
        [InverseProperty("CuCheckPointsCountries")]
        public virtual CuCheckPoint Checkpoint { get; set; }
        [ForeignKey("CountryId")]
        [InverseProperty("CuCheckPointsCountries")]
        public virtual RefCountry Country { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("CuCheckPointsCountries")]
        public virtual ApEntity Entity { get; set; }
    }
}