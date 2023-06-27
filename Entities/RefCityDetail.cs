using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REF_CITY_DETAILS")]
    public partial class RefCityDetail
    {
        public int Id { get; set; }
        [Column("City_Id")]
        public int CityId { get; set; }
        [Column("Location_Id")]
        public int? LocationId { get; set; }
        [Column("Zone_Id")]
        public int? ZoneId { get; set; }
        [Column("Travel_Time")]
        public double? TravelTime { get; set; }
        public int? EntityId { get; set; }
        public bool Active { get; set; }

        [ForeignKey("CityId")]
        [InverseProperty("RefCityDetails")]
        public virtual RefCity City { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("RefCityDetails")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("LocationId")]
        [InverseProperty("RefCityDetails")]
        public virtual RefLocation Location { get; set; }
        [ForeignKey("ZoneId")]
        [InverseProperty("RefCityDetails")]
        public virtual RefZone Zone { get; set; }
    }
}