using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_CheckPoints")]
    public partial class CuCheckPoint
    {
        public CuCheckPoint()
        {
            CuCheckPointsBrands = new HashSet<CuCheckPointsBrand>();
            CuCheckPointsCountries = new HashSet<CuCheckPointsCountry>();
            CuCheckPointsDepartments = new HashSet<CuCheckPointsDepartment>();
            CuCheckPointsServiceTypes = new HashSet<CuCheckPointsServiceType>();
        }

        public int Id { get; set; }
        public int CheckpointTypeId { get; set; }
        public int ServiceId { get; set; }
        public string Remarks { get; set; }
        public bool Active { get; set; }
        public int CustomerId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public int? DeletedBy { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("CheckpointTypeId")]
        [InverseProperty("CuCheckPoints")]
        public virtual CuCheckPointType CheckpointType { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("CuCheckPointCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("CustomerId")]
        [InverseProperty("CuCheckPoints")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("CuCheckPointDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("CuCheckPoints")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("ModifiedBy")]
        [InverseProperty("CuCheckPointModifiedByNavigations")]
        public virtual ItUserMaster ModifiedByNavigation { get; set; }
        [ForeignKey("ServiceId")]
        [InverseProperty("CuCheckPoints")]
        public virtual RefService Service { get; set; }
        [InverseProperty("Checkpoint")]
        public virtual ICollection<CuCheckPointsBrand> CuCheckPointsBrands { get; set; }
        [InverseProperty("Checkpoint")]
        public virtual ICollection<CuCheckPointsCountry> CuCheckPointsCountries { get; set; }
        [InverseProperty("Checkpoint")]
        public virtual ICollection<CuCheckPointsDepartment> CuCheckPointsDepartments { get; set; }
        [InverseProperty("Checkpoint")]
        public virtual ICollection<CuCheckPointsServiceType> CuCheckPointsServiceTypes { get; set; }
    }
}