using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("ES_CU_Config")]
    public partial class EsCuConfig
    {
        public int Id { get; set; }
        public int? DepartmentId { get; set; }
        public int? BrandId { get; set; }
        public int? CollectionId { get; set; }
        public int? BuyerId { get; set; }
        public int EsDetailsId { get; set; }

        [ForeignKey("BrandId")]
        [InverseProperty("EsCuConfigs")]
        public virtual CuBrand Brand { get; set; }
        [ForeignKey("BuyerId")]
        [InverseProperty("EsCuConfigs")]
        public virtual CuBuyer Buyer { get; set; }
        [ForeignKey("CollectionId")]
        [InverseProperty("EsCuConfigs")]
        public virtual CuCollection Collection { get; set; }
        [ForeignKey("DepartmentId")]
        [InverseProperty("EsCuConfigs")]
        public virtual CuDepartment Department { get; set; }
        [ForeignKey("EsDetailsId")]
        [InverseProperty("EsCuConfigs")]
        public virtual EsDetail EsDetails { get; set; }
    }
}