using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("AUD_TRAN_FA_Profile")]
    public partial class AudTranFaProfile
    {
        public int Id { get; set; }
        [Column("Audit_Id")]
        public int AuditId { get; set; }
        [Column("Created_Date", TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        [Column("No_of_Customer")]
        [StringLength(1000)]
        public string NoOfCustomer { get; set; }
        [Column("No_Of_Suppliers_Component")]
        [StringLength(1000)]
        public string NoOfSuppliersComponent { get; set; }
        [Column("Annual_production")]
        [StringLength(1000)]
        public string AnnualProduction { get; set; }
        [Column("Maximum_Capacity")]
        [StringLength(1000)]
        public string MaximumCapacity { get; set; }
        [Column("Company_Surface_Area")]
        [StringLength(1000)]
        public string CompanySurfaceArea { get; set; }
        [Column("Percentage_Cus_Total_Capacity")]
        [StringLength(1000)]
        public string PercentageCusTotalCapacity { get; set; }
        [Column("Possibility_Of_Extension")]
        [StringLength(1000)]
        public string PossibilityOfExtension { get; set; }
        [Column("Type_Of_product_Manufactured")]
        [StringLength(3000)]
        public string TypeOfProductManufactured { get; set; }
        [Column("Types_Of_Brands")]
        [StringLength(3000)]
        public string TypesOfBrands { get; set; }
        [Column("Number_Of_Sites")]
        [StringLength(1000)]
        public string NumberOfSites { get; set; }
        [Column("Company_Open_Time")]
        [StringLength(1000)]
        public string CompanyOpenTime { get; set; }
        [Column("Annual_Holidays")]
        [StringLength(1000)]
        public string AnnualHolidays { get; set; }
        [Column("Total_Staff")]
        public int? TotalStaff { get; set; }
        [Column("Production_Staff")]
        public int? ProductionStaff { get; set; }
        [Column("Administrative_Staff")]
        public int? AdministrativeStaff { get; set; }
        [Column("Quality_Staff")]
        public int? QualityStaff { get; set; }
        [Column("Sales_Staff")]
        public int? SalesStaff { get; set; }
        [Column("Investment_Background")]
        [StringLength(3000)]
        public string InvestmentBackground { get; set; }
        [Column("Public_Liability_Insurance")]
        [StringLength(3500)]
        public string PublicLiabilityInsurance { get; set; }
        [Column("Industry_TradeAssociation")]
        [StringLength(3500)]
        public string IndustryTradeAssociation { get; set; }
        [StringLength(3500)]
        public string Accrediations { get; set; }
        public bool Active { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public int? DeletedBy { get; set; }

        [ForeignKey("AuditId")]
        [InverseProperty("AudTranFaProfiles")]
        public virtual AudTransaction Audit { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("AudTranFaProfileCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("AudTranFaProfileDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
    }
}