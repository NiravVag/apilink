using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("ES_CU_Contacts")]
    public partial class EsCuContact
    {
        public int Id { get; set; }
        [Column("Customer_Contact_Id")]
        public int CustomerContactId { get; set; }
        public int EsDetailsId { get; set; }

        [ForeignKey("CustomerContactId")]
        [InverseProperty("EsCuContacts")]
        public virtual CuContact CustomerContact { get; set; }
        [ForeignKey("EsDetailsId")]
        [InverseProperty("EsCuContacts")]
        public virtual EsDetail EsDetails { get; set; }
    }
}