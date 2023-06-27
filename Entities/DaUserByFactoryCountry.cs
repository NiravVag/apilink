using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("DA_UserByFactoryCountry")]
    public partial class DaUserByFactoryCountry
    {
        public int Id { get; set; }
        public int DaUserCustomerId { get; set; }
        public int FactoryCountryId { get; set; }
        public int CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        public int EntityId { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("DaUserByFactoryCountries")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DaUserCustomerId")]
        [InverseProperty("DaUserByFactoryCountries")]
        public virtual DaUserCustomer DaUserCustomer { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("DaUserByFactoryCountries")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("FactoryCountryId")]
        [InverseProperty("DaUserByFactoryCountries")]
        public virtual RefCountry FactoryCountry { get; set; }
    }
}