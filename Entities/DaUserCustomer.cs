using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("DA_UserCustomer")]
    public partial class DaUserCustomer
    {
        public DaUserCustomer()
        {
            DaUserByBrands = new HashSet<DaUserByBrand>();
            DaUserByBuyers = new HashSet<DaUserByBuyer>();
            DaUserByDepartments = new HashSet<DaUserByDepartment>();
            DaUserByFactoryCountries = new HashSet<DaUserByFactoryCountry>();
            DaUserByProductCategories = new HashSet<DaUserByProductCategory>();
            DaUserByRoles = new HashSet<DaUserByRole>();
            DaUserByServices = new HashSet<DaUserByService>();
            DaUserRoleNotificationByOffices = new HashSet<DaUserRoleNotificationByOffice>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public int? CustomerId { get; set; }
        public int UserType { get; set; }
        public bool Email { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        [Required]
        public bool? Active { get; set; }
        public int? EntityId { get; set; }
        [Column("Primary_CS")]
        public bool? PrimaryCs { get; set; }
        [Column("Primary_ReportChecker")]
        public bool? PrimaryReportChecker { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("DaUserCustomerCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("CustomerId")]
        [InverseProperty("DaUserCustomers")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("DaUserCustomers")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("DaUserCustomerUsers")]
        public virtual ItUserMaster User { get; set; }
        [ForeignKey("UserType")]
        [InverseProperty("DaUserCustomers")]
        public virtual HrProfile UserTypeNavigation { get; set; }
        [InverseProperty("DauserCustomer")]
        public virtual ICollection<DaUserByBrand> DaUserByBrands { get; set; }
        [InverseProperty("DauserCustomer")]
        public virtual ICollection<DaUserByBuyer> DaUserByBuyers { get; set; }
        [InverseProperty("DauserCustomer")]
        public virtual ICollection<DaUserByDepartment> DaUserByDepartments { get; set; }
        [InverseProperty("DaUserCustomer")]
        public virtual ICollection<DaUserByFactoryCountry> DaUserByFactoryCountries { get; set; }
        [InverseProperty("DauserCustomer")]
        public virtual ICollection<DaUserByProductCategory> DaUserByProductCategories { get; set; }
        [InverseProperty("DauserCustomer")]
        public virtual ICollection<DaUserByRole> DaUserByRoles { get; set; }
        [InverseProperty("DauserCustomer")]
        public virtual ICollection<DaUserByService> DaUserByServices { get; set; }
        [InverseProperty("DauserCustomer")]
        public virtual ICollection<DaUserRoleNotificationByOffice> DaUserRoleNotificationByOffices { get; set; }
    }
}