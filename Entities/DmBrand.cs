using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("DM_Brand")]
    public partial class DmBrand
    {
        public int Id { get; set; }
        public int BrandId { get; set; }
        [Column("DMFileId")]
        public int? DmfileId { get; set; }

        [ForeignKey("BrandId")]
        [InverseProperty("DmBrands")]
        public virtual CuBrand Brand { get; set; }
        [ForeignKey("DmfileId")]
        [InverseProperty("DmBrands")]
        public virtual DmFile Dmfile { get; set; }
    }
}