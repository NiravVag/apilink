using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("Language")]
    public partial class Language
    {
        public Language()
        {
            CuCustomers = new HashSet<CuCustomer>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [InverseProperty("LanguageNavigation")]
        public virtual ICollection<CuCustomer> CuCustomers { get; set; }
    }
}