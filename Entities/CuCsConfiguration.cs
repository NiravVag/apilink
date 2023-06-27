using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_CS_Configuration")]
    public partial class CuCsConfiguration
    {
        public int Id { get; set; }
        [Column("Customer_Id")]
        public int? CustomerId { get; set; }
        [Column("User_Id")]
        public int UserId { get; set; }
        public bool Active { get; set; }
        [Column("Office_Location_Id")]
        public int OfficeLocationId { get; set; }
        [Column("Service_Id")]
        public int? ServiceId { get; set; }
        [Column("Product_category_Id")]
        public int? ProductCategoryId { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("CustomerId")]
        [InverseProperty("CuCsConfigurations")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("CuCsConfigurations")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("OfficeLocationId")]
        [InverseProperty("CuCsConfigurations")]
        public virtual RefLocation OfficeLocation { get; set; }
        [ForeignKey("ProductCategoryId")]
        [InverseProperty("CuCsConfigurations")]
        public virtual RefProductCategory ProductCategory { get; set; }
        [ForeignKey("ServiceId")]
        [InverseProperty("CuCsConfigurations")]
        public virtual RefService Service { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("CuCsConfigurations")]
        public virtual HrStaff User { get; set; }
    }
}