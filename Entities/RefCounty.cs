using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REF_County")]
    public partial class RefCounty
    {
        public RefCounty()
        {
            HrStaffs = new HashSet<HrStaff>();
            InvTmDetailCounties = new HashSet<InvTmDetail>();
            InvTmDetailInspPortCounties = new HashSet<InvTmDetail>();
            RefTowns = new HashSet<RefTown>();
            SuAddresses = new HashSet<SuAddress>();
        }

        public int Id { get; set; }
        [Column("City_Id")]
        public int CityId { get; set; }
        [Required]
        [Column("County_Name")]
        [StringLength(500)]
        public string CountyName { get; set; }
        [Column("County_Code")]
        [StringLength(500)]
        public string CountyCode { get; set; }
        public bool Active { get; set; }
        [Column("Created_By")]
        public int? CreatedBy { get; set; }
        [Column("Created_On", TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        [Column("Modified_By")]
        public int? ModifiedBy { get; set; }
        [Column("Modified_On", TypeName = "datetime")]
        public DateTime? ModifiedOn { get; set; }
        [Column("Deleted_By")]
        public int? DeletedBy { get; set; }
        [Column("Deleted_On", TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public int? ZoneId { get; set; }

        [ForeignKey("CityId")]
        [InverseProperty("RefCounties")]
        public virtual RefCity City { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("RefCountyCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("RefCountyDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("ModifiedBy")]
        [InverseProperty("RefCountyModifiedByNavigations")]
        public virtual ItUserMaster ModifiedByNavigation { get; set; }
        [ForeignKey("ZoneId")]
        [InverseProperty("RefCounties")]
        public virtual RefZone Zone { get; set; }
        [InverseProperty("CurrentCounty")]
        public virtual ICollection<HrStaff> HrStaffs { get; set; }
        [InverseProperty("County")]
        public virtual ICollection<InvTmDetail> InvTmDetailCounties { get; set; }
        [InverseProperty("InspPortCounty")]
        public virtual ICollection<InvTmDetail> InvTmDetailInspPortCounties { get; set; }
        [InverseProperty("County")]
        public virtual ICollection<RefTown> RefTowns { get; set; }
        [InverseProperty("County")]
        public virtual ICollection<SuAddress> SuAddresses { get; set; }
    }
}