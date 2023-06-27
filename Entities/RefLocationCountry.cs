using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REF_Location_Country")]
    public partial class RefLocationCountry
    {
        [Column("LocationID")]
        public int LocationId { get; set; }
        [Column("CountryID")]
        public int CountryId { get; set; }

        [ForeignKey("CountryId")]
        [InverseProperty("RefLocationCountries")]
        public virtual RefCountry Country { get; set; }
        [ForeignKey("LocationId")]
        [InverseProperty("RefLocationCountries")]
        public virtual RefLocation Location { get; set; }
    }
}