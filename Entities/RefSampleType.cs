using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REF_SampleType")]
    public partial class RefSampleType
    {
        public RefSampleType()
        {
            InspProductTransactions = new HashSet<InspProductTransaction>();
        }

        public int Id { get; set; }
        [StringLength(100)]
        public string SampleType { get; set; }
        [StringLength(100)]
        public string SampleSize { get; set; }
        public bool? Active { get; set; }

        [InverseProperty("SampleTypeNavigation")]
        public virtual ICollection<InspProductTransaction> InspProductTransactions { get; set; }
    }
}