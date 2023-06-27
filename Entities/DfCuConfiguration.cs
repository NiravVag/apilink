using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("DF_CU_Configuration")]
    public partial class DfCuConfiguration
    {
        public DfCuConfiguration()
        {
            DfControlAttributes = new HashSet<DfControlAttribute>();
            InspDfTransactions = new HashSet<InspDfTransaction>();
        }

        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int ModuleId { get; set; }
        public int ControlTypeId { get; set; }
        [Required]
        [StringLength(100)]
        public string Label { get; set; }
        [StringLength(100)]
        public string Type { get; set; }
        public int? DataSourceType { get; set; }
        public int DisplayOrder { get; set; }
        public bool Active { get; set; }
        public int CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        [Column("FBReference")]
        [StringLength(200)]
        public string Fbreference { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("ControlTypeId")]
        [InverseProperty("DfCuConfigurations")]
        public virtual DfControlType ControlType { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("DfCuConfigurationCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("CustomerId")]
        [InverseProperty("DfCuConfigurations")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("DataSourceType")]
        [InverseProperty("DfCuConfigurations")]
        public virtual DfDdlSourceType DataSourceTypeNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("DfCuConfigurationDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("DfCuConfigurations")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("ModuleId")]
        [InverseProperty("DfCuConfigurations")]
        public virtual RefModule Module { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("DfCuConfigurationUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
        [InverseProperty("ControlConfiguration")]
        public virtual ICollection<DfControlAttribute> DfControlAttributes { get; set; }
        [InverseProperty("ControlConfiguration")]
        public virtual ICollection<InspDfTransaction> InspDfTransactions { get; set; }
    }
}