using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REF_Country_Location")]
    public partial class RefCountryLocation
    {
        public int CountryId { get; set; }
        public int LocationId { get; set; }

        [ForeignKey("CountryId")]
        [InverseProperty("RefCountryLocations")]
        public virtual RefCountry Country { get; set; }
        [ForeignKey("LocationId")]
        [InverseProperty("RefCountryLocations")]
        public virtual RefLocation Location { get; set; }
    }
}