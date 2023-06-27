using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("DA_UserByProductCategory")]
    public partial class DaUserByProductCategory
    {
        public int Id { get; set; }
        [Column("DAUserCustomerId")]
        public int DauserCustomerId { get; set; }
        public int? ProductCategoryId { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("DaUserByProductCategories")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DauserCustomerId")]
        [InverseProperty("DaUserByProductCategories")]
        public virtual DaUserCustomer DauserCustomer { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("DaUserByProductCategories")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("ProductCategoryId")]
        [InverseProperty("DaUserByProductCategories")]
        public virtual RefProductCategory ProductCategory { get; set; }
    }
}