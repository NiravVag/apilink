using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_Address")]
    public partial class CuAddress
    {
        public CuAddress()
        {
            CuContacts = new HashSet<CuContact>();
            InspTranPickings = new HashSet<InspTranPicking>();
        }

        public int Id { get; set; }
        [Column("Address_Type")]
        public int AddressType { get; set; }
        public string Address { get; set; }
        [Column("Box_Post")]
        [StringLength(100)]
        public string BoxPost { get; set; }
        [Column("Zip_Code")]
        [StringLength(20)]
        public string ZipCode { get; set; }
        [Column("Country_Id")]
        public int? CountryId { get; set; }
        [Column("City_Id")]
        public int? CityId { get; set; }
        [Column("Customer_Id")]
        public int? CustomerId { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public bool? Active { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("AddressType")]
        [InverseProperty("CuAddresses")]
        public virtual RefAddressType AddressTypeNavigation { get; set; }
        [ForeignKey("CityId")]
        [InverseProperty("CuAddresses")]
        public virtual RefCity City { get; set; }
        [ForeignKey("CountryId")]
        [InverseProperty("CuAddresses")]
        public virtual RefCountry Country { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("CuAddressCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("CustomerId")]
        [InverseProperty("CuAddresses")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("CuAddressDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("CuAddresses")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("CuAddressUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
        [InverseProperty("OfficeNavigation")]
        public virtual ICollection<CuContact> CuContacts { get; set; }
        [InverseProperty("CusAddress")]
        public virtual ICollection<InspTranPicking> InspTranPickings { get; set; }
    }
}