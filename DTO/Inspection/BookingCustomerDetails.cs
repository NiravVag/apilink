using DTO.CommonClass;
using DTO.Customer;
using DTO.References;
using DTO.Supplier;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Inspection
{
    public class BookingCustomerDetails
    {
        public IEnumerable<CustomerBrand> CustomerBrandList { get; set; }

        public IEnumerable<CustomerDepartment> CustomerDepartmentList { get; set; }

        public IEnumerable<CustomerBuyers> CustomerBuyerList { get; set; }

        public IEnumerable<CommonDataSource> CustomerMerchandiserList { get; set; }

        public IEnumerable<ServiceType> CustomerServiceList { get; set; }

        public IEnumerable<Season> SeasonList { get; set; }

        public IEnumerable<SupplierItem> SupplierList { get; set; }

        public EditBookingResult Result { get; set; }

        public string SupplierCode { get; set; }

        public IEnumerable<suppliercontact> SupplierContactList { get; set; }

        public string FactoryCode { get; set; }

        public IEnumerable<suppliercontact> FactoryContactList { get; set; }

        public int? OfficeId { get; set; }

        public string BookingDefaultComments { get; set; }

        public IEnumerable<CommonDataSource> Collection { get; set; }

        public IEnumerable<CommonDataSource> PriceCategory { get; set; }
    }

    public enum EditBookingResult
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
        GetBookingDetailsByCustomerIdSuccess = 19,
        GetSupplierDetailsBySupplierCUstomerIdSuccess = 20,
        CannotGetSupplierContactList = 21,
        CannotGetSupplierDetails = 22,
        GetFactoryDetailsByIdSuccess = 23,
        CannotGetFactoryContactList = 24,
        CannotGetFactoryDetails = 25,
        CanotGetCustomerDetails = 26,
        CannotGetBookingRule = 28,
        CannotGetContactDetails = 29,
        GetContactDetailsSuccess = 30
    }
    public class CSConfigResponse
    {
        public string CsName { get; set; }
        public CSConfigResult Result { get; set; }
    }
    public enum CSConfigResult
    {
        Success = 1,
        NotFound = 2,
        RequestNotValid = 3
    }

    public class CSConfigListResponse
    {
        public List<CommonDataSource> CsList { get; set; }
        public CSConfigListResult Result { get; set; }
    }

    public enum CSConfigListResult
    {
        Success = 1,
        NotFound = 2,
        RequestNotValid = 3
    }

    public class EditBookingCustomerDetails
    {
        public List<CustomerContact> CustomerContactList { get; set; }

        public List<CommonDataSource> CustomerBrandList { get; set; }

        public List<CommonDataSource> CustomerDepartmentList { get; set; }

        public List<CommonDataSource> CustomerBuyerList { get; set; }

        public List<CommonDataSource> CustomerMerchandiserList { get; set; }

        public List<CommonDataSource> Collection { get; set; }

        public List<CommonDataSource> PriceCategory { get; set; }

        public List<CommonDataSource> CustomerServiceList { get; set; }

        public IEnumerable<Season> SeasonList { get; set; }

        public IEnumerable<CommonDataSource> SupplierList { get; set; }

        public EditBookingCustomerResult Result { get; set; }
    }

    public enum EditBookingCustomerResult
    {
        Success = 1,
        CustomerContactNotFound = 2,
        CustomerBrandNotFound = 3,
        CustomerDeptNotFound = 4,
        CustomerBuyerNotFound = 5,
        MerchandiserNotFound = 6,
        CollectionNotFound = 7,
        PriceCategoryNotFound = 8,
        CustomerServiceTypeNotFound = 9,
        SupplierNotFound = 10,
        CannotGetDetails = 11

    }
}
