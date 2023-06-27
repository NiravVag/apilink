using DTO.Customer;
using DTO.HumanResource;
using DTO.Location;
using DTO.OfficeLocation;
using DTO.References;
using DTO.Supplier;

using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Audit
{
    public class EditAuditResponse
    {
        public IEnumerable<SupplierItem> SupplierList { get; set; }

        public IEnumerable<SupplierItem> FactoryList { get; set; }

        public  Entities.Enums.UserTypeEnum UserType { get; set; }

        public AuditDetails AuditDetails { get; set; }

        public EditAuditResult Result { get; set; }
    }
    public class EditAuidtCusDetails
    {
        public IEnumerable<CustomerContact> CustomerContactList { get; set; }

        public IEnumerable<CustomerBrand> CustomerBrandList { get; set; }

        public IEnumerable<CustomerDepartment> CustomerDepartmentList { get; set; }

        public IEnumerable<ServiceTypeData> CustomerServiceList { get; set; }

        public IEnumerable<Season> SeasonList { get; set; }

        public IEnumerable<SupplierItem> SupplierList { get; set; }

        public EditAuditResult Result { get; set; }

        public string SupplierCode { get; set; }

        public IEnumerable<suppliercontact> SupplierContactList { get; set; }

        public string FactoryCode { get; set; }

        public IEnumerable<suppliercontact> FactoryContactList { get; set; }

        public int OfficeId { get; set; }
    }
    public class EditAuditSupDetails
    {
        public IEnumerable<SupplierItem> FactoryList { get; set; }

        public IEnumerable<suppliercontact> SupplierContactList { get; set; }

        public string SupplierCode { get; set; }

        public EditAuditResult Result { get; set; }
    }
    public class EditAuditFactDetails
    {
        public IEnumerable<suppliercontact> FactoryContactList { get; set; }

        public IEnumerable<SupplierItem> SupplierList { get; set; }

        public string FactoryAddress { get; set; }

        public string FactoryRegionalAddress { get; set; }

        public string FactoryCode { get; set; }

        public int OfficeId { get; set; }

        public EditAuditResult Result { get; set; }

        public IEnumerable<CustomerCS> AuditCS { get; set; }
    }
    public class AuditBookingContactResponse
    {
        public AuditBookingContact ContactDetails { get; set; }

        public EditAuditResult Result { get; set; }
    }
    public enum EditAuditResult
    {
        Success = 1,
        CannotGetCustomerList = 2,
        CannotGetCustomerBrandList = 3,
        CannotGetCustomerDepartmentList = 4,
        CannotGetCustomerContactList = 5,
        CannotGetSeasonList = 6,
        CannotGetServiceTypeList = 7,
        CannotGetSelectSeasonList = 9,
        CannotGetOfficeList = 10,
        CannotGetUnitList = 11,
        CannotGetEvalutionRoundList = 12,
        CannotGetSupplierList = 13,
        CannotGetfactoryList = 14,
        CannotGetFabricCategoryList = 15,
        CannotGetProcessTypeList = 16,
        CannotGetProductionStatusList = 17,
        CannotGetProductionQuantityList = 18,
        GetAuditDetailsByCustomerIdSuccess = 19,
        GetSupplierDetailsBySupplierCUstomerIdSuccess = 20,
        CannotGetSupplierContactList = 21,
        CannotGetSupplierDetails = 22,
        GetFactoryDetailsByIdSuccess = 23,
        CannotGetFactoryContactList = 24,
        CannotGetFactoryDetails = 25,
        CanotGetCustomerDetails = 26,
        CannotGetAuditDetails = 27,
        CannotGetBookingRule = 28,
        CannotGetContactDetails=29,
        GetContactDetailsSuccess=30
    }
}
