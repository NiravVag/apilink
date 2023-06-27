using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("DA_UserByBrand")]
    public partial class DaUserByBrand
    {
        public int Id { get; set; }
        [Column("DAUserCustomerId")]
        public int DauserCustomerId { get; set; }
        public int? BrandId { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("BrandId")]
        [InverseProperty("DaUserByBrands")]
        public virtual CuBrand Brand { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("DaUserByBrands")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DauserCustomerId")]
        [InverseProperty("DaUserByBrands")]
        public virtual DaUserCustomer DauserCustomer { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("DaUserByBrands")]
        public virtual ApEntity Entity { get; set; }
    }
}