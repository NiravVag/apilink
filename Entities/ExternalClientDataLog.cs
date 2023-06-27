using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("ExternalClientDataLog")]
    public partial class ExternalClientDataLog
    {
        public int Id { get; set; }
    }
}