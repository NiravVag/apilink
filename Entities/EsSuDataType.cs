using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("ES_SU_DataType")]
    public partial class EsSuDataType
    {
        public EsSuDataType()
        {
            EsSuPreDefinedFields = new HashSet<EsSuPreDefinedField>();
        }

        public int Id { get; set; }
        [StringLength(20)]
        public string Data { get; set; }

        [InverseProperty("DataTypeNavigation")]
        public virtual ICollection<EsSuPreDefinedField> EsSuPreDefinedFields { get; set; }
    }
}