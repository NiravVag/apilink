using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("ES_API_Default_Contacts")]
    public partial class EsApiDefaultContact
    {
        public int Id { get; set; }
        [Column("Api_Contact_Id")]
        public int ApiContactId { get; set; }
        public int OfficeId { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("ApiContactId")]
        [InverseProperty("EsApiDefaultContacts")]
        public virtual HrStaff ApiContact { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("EsApiDefaultContacts")]
        public virtual ApEntity Entity { get; set; }
    }
}