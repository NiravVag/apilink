using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("ES_FA_Country_Config")]
    public partial class EsFaCountryConfig
    {
        public int Id { get; set; }
        [Column("Factory_CountryId")]
        public int FactoryCountryId { get; set; }
        public int EsDetailsId { get; set; }

        [ForeignKey("EsDetailsId")]
        [InverseProperty("EsFaCountryConfigs")]
        public virtual EsDetail EsDetails { get; set; }
        [ForeignKey("FactoryCountryId")]
        [InverseProperty("EsFaCountryConfigs")]
        public virtual RefCountry FactoryCountry { get; set; }
    }
}