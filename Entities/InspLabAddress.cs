using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INSP_LAB_Address")]
    public partial class InspLabAddress
    {
        public InspLabAddress()
        {
            InspTranPickings = new HashSet<InspTranPicking>();
        }

        public int Id { get; set; }
        public int CountryId { get; set; }
        public int ProvinceId { get; set; }
        public int CityId { get; set; }
        [StringLength(20)]
        public string ZipCode { get; set; }
        [StringLength(2000)]
        public string Address { get; set; }
        [StringLength(2000)]
        public string RegionalLanguage { get; set; }
        [Column("Lab_Id")]
        public int? LabId { get; set; }
        public int? AddressTypeId { get; set; }

        [ForeignKey("AddressTypeId")]
        [InverseProperty("InspLabAddresses")]
        public virtual InspLabAddressType AddressType { get; set; }
        [ForeignKey("CityId")]
        [InverseProperty("InspLabAddresses")]
        public virtual RefCity City { get; set; }
        [ForeignKey("CountryId")]
        [InverseProperty("InspLabAddresses")]
        public virtual RefCountry Country { get; set; }
        [ForeignKey("LabId")]
        [InverseProperty("InspLabAddresses")]
        public virtual InspLabDetail Lab { get; set; }
        [ForeignKey("ProvinceId")]
        [InverseProperty("InspLabAddresses")]
        public virtual RefProvince Province { get; set; }
        [InverseProperty("LabAddress")]
        public virtual ICollection<InspTranPicking> InspTranPickings { get; set; }
    }
}