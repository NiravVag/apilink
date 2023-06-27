using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REF_Service")]
    public partial class RefService
    {
        public RefService()
        {
            CompComplaints = new HashSet<CompComplaint>();
            CuApiServices = new HashSet<CuApiService>();
            CuBuyerApiServices = new HashSet<CuBuyerApiService>();
            CuCheckPoints = new HashSet<CuCheckPoint>();
            CuContactEntityServiceMaps = new HashSet<CuContactEntityServiceMap>();
            CuContactServices = new HashSet<CuContactService>();
            CuCsConfigurations = new HashSet<CuCsConfiguration>();
            CuPrDetails = new HashSet<CuPrDetail>();
            CuProductApiServices = new HashSet<CuProductApiService>();
            CuServiceTypes = new HashSet<CuServiceType>();
            DaUserByServices = new HashSet<DaUserByService>();
            EntPages = new HashSet<EntPage>();
            EsDetails = new HashSet<EsDetail>();
            HrStaffEntityServiceMaps = new HashSet<HrStaffEntityServiceMap>();
            HrStaffServices = new HashSet<HrStaffService>();
            InvAutTranDetails = new HashSet<InvAutTranDetail>();
            InvExfTransactions = new HashSet<InvExfTransaction>();
            InvManTransactions = new HashSet<InvManTransaction>();
            ItRightTypes = new HashSet<ItRightType>();
            QuQuotations = new HashSet<QuQuotation>();
            RefProductCategoryApiServices = new HashSet<RefProductCategoryApiService>();
            RefServiceTypes = new HashSet<RefServiceType>();
            SuApiServices = new HashSet<SuApiService>();
            SuContactApiServices = new HashSet<SuContactApiService>();
            SuContactEntityServiceMaps = new HashSet<SuContactEntityServiceMap>();
        }

        public int Id { get; set; }
        [StringLength(200)]
        public string Name { get; set; }
        public bool Active { get; set; }
        public int? EntityId { get; set; }
        [Column("Fb_Service_Id")]
        public int? FbServiceId { get; set; }

        [ForeignKey("EntityId")]
        [InverseProperty("RefServices")]
        public virtual ApEntity Entity { get; set; }
        [InverseProperty("ServiceNavigation")]
        public virtual ICollection<CompComplaint> CompComplaints { get; set; }
        [InverseProperty("Service")]
        public virtual ICollection<CuApiService> CuApiServices { get; set; }
        [InverseProperty("Service")]
        public virtual ICollection<CuBuyerApiService> CuBuyerApiServices { get; set; }
        [InverseProperty("Service")]
        public virtual ICollection<CuCheckPoint> CuCheckPoints { get; set; }
        [InverseProperty("Service")]
        public virtual ICollection<CuContactEntityServiceMap> CuContactEntityServiceMaps { get; set; }
        [InverseProperty("Service")]
        public virtual ICollection<CuContactService> CuContactServices { get; set; }
        [InverseProperty("Service")]
        public virtual ICollection<CuCsConfiguration> CuCsConfigurations { get; set; }
        [InverseProperty("Service")]
        public virtual ICollection<CuPrDetail> CuPrDetails { get; set; }
        [InverseProperty("Service")]
        public virtual ICollection<CuProductApiService> CuProductApiServices { get; set; }
        [InverseProperty("Service")]
        public virtual ICollection<CuServiceType> CuServiceTypes { get; set; }
        [InverseProperty("Service")]
        public virtual ICollection<DaUserByService> DaUserByServices { get; set; }
        [InverseProperty("Service")]
        public virtual ICollection<EntPage> EntPages { get; set; }
        [InverseProperty("Service")]
        public virtual ICollection<EsDetail> EsDetails { get; set; }
        [InverseProperty("Service")]
        public virtual ICollection<HrStaffEntityServiceMap> HrStaffEntityServiceMaps { get; set; }
        [InverseProperty("Service")]
        public virtual ICollection<HrStaffService> HrStaffServices { get; set; }
        [InverseProperty("Service")]
        public virtual ICollection<InvAutTranDetail> InvAutTranDetails { get; set; }
        [InverseProperty("Service")]
        public virtual ICollection<InvExfTransaction> InvExfTransactions { get; set; }
        [InverseProperty("Service")]
        public virtual ICollection<InvManTransaction> InvManTransactions { get; set; }
        [InverseProperty("ServiceNavigation")]
        public virtual ICollection<ItRightType> ItRightTypes { get; set; }
        [InverseProperty("Service")]
        public virtual ICollection<QuQuotation> QuQuotations { get; set; }
        [InverseProperty("Service")]
        public virtual ICollection<RefProductCategoryApiService> RefProductCategoryApiServices { get; set; }
        [InverseProperty("Service")]
        public virtual ICollection<RefServiceType> RefServiceTypes { get; set; }
        [InverseProperty("Service")]
        public virtual ICollection<SuApiService> SuApiServices { get; set; }
        [InverseProperty("Service")]
        public virtual ICollection<SuContactApiService> SuContactApiServices { get; set; }
        [InverseProperty("Service")]
        public virtual ICollection<SuContactEntityServiceMap> SuContactEntityServiceMaps { get; set; }
    }
}