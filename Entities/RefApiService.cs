using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REF_API_Services")]
    public partial class RefApiService
    {
        public int Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
    }
}