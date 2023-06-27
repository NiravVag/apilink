using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("ES_API_Contacts")]
    public partial class EsApiContact
    {
        public int Id { get; set; }
        [Column("Api_Contact_Id")]
        public int ApiContactId { get; set; }
        public int EsDetailsId { get; set; }

        [ForeignKey("ApiContactId")]
        [InverseProperty("EsApiContacts")]
        public virtual HrStaff ApiContact { get; set; }
        [ForeignKey("EsDetailsId")]
        [InverseProperty("EsApiContacts")]
        public virtual EsDetail EsDetails { get; set; }
    }
}