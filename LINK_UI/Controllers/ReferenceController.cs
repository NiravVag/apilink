using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Managers;
using DTO.CommonClass;
using DTO.Customer;
using DTO.Quotation;
using DTO.References;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ReferenceController : ControllerBase
    {
        public readonly IReferenceManager _manager = null;
        public ReferenceController(IReferenceManager manager)
        {
            this._manager = manager;
        }

        [HttpGet("GetSeasonYear")]
        public async Task<SeasonYearResponse> GetSeasonYear()
        {
            return await _manager.GetSeasonsYear();
        }

        [HttpGet("serviceType-list/{customerId}/{serviceId}")]
        public async Task<QuotationDataSourceResponse> GetServiceTypeList(int customerId, int serviceId)
        {
            if (customerId <= 0 || serviceId <= 0)
                return new QuotationDataSourceResponse { Result = QuotationDataSourceResult.CustomerEmpty };

            return await _manager.GetServiceTypeList(customerId, serviceId);
        }

        [HttpGet("ServiceTypeList")]
        public async Task<DataSourceResponse> GetServiceTypesList()
        {
            return await _manager.GetServiceList();
        }

        [HttpGet("serviceType-list-cus-service/{customerId}/{serviceId}")]
        public async Task<DataSourceResponse> GetServiceTypeListByCusService(int customerId, int serviceId)
        {
            if (customerId <= 0 || serviceId <= 0)
                return new DataSourceResponse { Result = DataSourceResult.CannotGetList };

            return await _manager.GetServiceTypeListByCusService(customerId, serviceId);
        }

        [HttpGet("BillingMethodList")]
        public async Task<DataSourceResponse> GetBillingMethodList()
        {
            return await _manager.GetBillingMethodList();
        }

        [HttpGet("BillingToList")]
        public async Task<DataSourceResponse> GetBillingToList()
        {
            return await _manager.GetBillingToList();
        }

        [HttpGet("getCurrencyList")]
        public async Task<DataSourceResponse> getCurrencyList()
        {
            return await _manager.GetCurrencyList();
        }

        [HttpGet("CustomSampleSize")]
        public async Task<CustomSampleSizeResponse> GetCustomSampleSize()
        {
            return await _manager.GetCustomSampleSizeList();
        }

        [HttpGet("getapiservices")]
        public async Task<DataSourceResponse> GetAPIServices()
        {
            return await _manager.GetAPIServices();
        }

        [HttpGet("getbillingentityList")]
        public async Task<DataSourceResponse> GetBillingEntityList()
        {
            return await _manager.GetBillingEntityList();
        }

        [HttpGet("getInvoiceRequestTypeList")]
        public async Task<DataSourceResponse> GetinvoiceRequestList()
        {
            return await _manager.GetInvoiceRequestTypeList();
        }

        [HttpGet("getInvoiceBankList/{billingEntity}")]
        public async Task<DataSourceResponse> GetInvoiceBankList(int? billingEntity)
        {
            return await _manager.GetInvoiceBankList(billingEntity);
        }

        [HttpGet("getBankList/{billingEntity}")]
        public async Task<IEnumerable<CommonBankDataSource>> GetBankList(int? billingEntity)
        {
            return await _manager.GetBankList(billingEntity);
        }

        [HttpGet("getInvoiceFeesTypeList")]
        public async Task<DataSourceResponse> GetInvoiceFeesTypeList()
        {
            return await _manager.GetInvoiceFeesTypeList();
        }

        [HttpGet("getInvoiceOfficeList")]
        public async Task<DataSourceResponse> GetInvoiceOfficeList()
        {
            return await _manager.GetInvoiceOfficeList();
        }

        [HttpGet("getInvoicePaymentTypeList")]
        public async Task<PaymentTypeResponse> GetInvoicePaymentTypeList()
        {
            return await _manager.GetInvoicePaymentTypeList();
        }
        [HttpGet("getInvoiceExtraTypeList")]
        public async Task<DataSourceResponse> GetInvoiceExtraTypeList()
        {
            return await _manager.GetInvoiceExtraTypeList();
        }

        [HttpGet("get-service-data")]
        public async Task<DataSourceResponse> GetServices()
        {
            return await _manager.GetServiceDataList();
        }

        [HttpGet("get-full-bridge-result-data")]
        public async Task<DataSourceResponse> GetFBResultList()
        {
            return await _manager.GetFBResultList();
        }

        [HttpGet("get-office-locations")]
        public async Task<DataSourceResponse> GetOfficeLocations()
        {
            return await _manager.GetOfficeLocations();
        }

        [HttpGet("get-delimiter-data")]
        public async Task<EmailSubjectDelimiterResponse> GetDelimiterList()
        {
            return await _manager.GetDelimiterList();
        }

        [HttpPost("get-service-types")]
        public async Task<ServiceTypeResponse> GetServiceTypes(ServiceTypeRequest request)
        {
            return await _manager.GetCustomerServiceTypes(request);
        }

        [HttpGet("get-inspection-locations")]
        public async Task<DataSourceResponse> GetInspectionLocations()
        {
            return await _manager.GetInspectionLocations();
        }

        [HttpGet("get-inspection-shipmenttypes")]
        public async Task<DataSourceResponse> GetInspectionShipmentTypes()
        {
            return await _manager.GetInspectionShipmentTypes();
        }

        [HttpGet("get-business-lines")]
        public async Task<DataSourceResponse> GetBusinessLines()
        {
            return await _manager.GetBusinessLines();
        }

        [HttpGet("getEntityList")]
        public async Task<DataSourceResponse> GetEntityList()
        {
            return await _manager.GetEntityList();
        }

        [HttpGet("GetTripTypeList")]
        public async Task<DataSourceResponse> GetTripTypeList()
        {
            return await _manager.GetTripTypeList();
        }

        [HttpGet("getuserEntityList/{userType}/{id}")]
        public async Task<DataSourceResponse> GetUserEntityList(int userType, int id)
        {
            return await _manager.GetUserEntityList(userType, id);
        }

        [HttpGet("GetBillFrequencyList")]
        public async Task<DataSourceResponse> GetBillFrequencyList()
        {
            return await _manager.GetBillingFrequncyList();
        }

        [HttpGet("GetBillQuantityTypeList")]
        public async Task<DataSourceResponse> GetBillQuantityTypeList()
        {
            return await _manager.GetBillingQuantityTypeList();
        }

        [HttpGet("GetInterventionTypeList")]
        public async Task<DataSourceResponse> GetInterventionTypeList()
        {
            return await _manager.GetInterventionTypeList();
        }

        [HttpGet("GetEntityFeatureList")]
        public async Task<List<int>> GetEntityFeatureList()
        {
            return await _manager.GetEntityFeatureList();
        }

        [HttpGet("isEntityFeatureExist/{featureId}")]
        public async Task<bool> IsEntityFeatureExist(int featureId)
        {
            return await _manager.IsEntityFeatureExist(featureId);
        }

        [HttpPost("ServiceTypeList")]
        public async Task<DataSourceResponse> GetServiceTypesByServiceIds(IEnumerable<int> serviceIds)
        {
            return await _manager.GetServiceTypesByServiceIds(serviceIds);
        }

        [HttpPost("GetStaffDataSourceList")]
        public async Task<DataSourceResponse> GetStaffSourceList(CommonDataSourceRequest request)
        {
            return await _manager.GetStaffSourceList(request);
        }

        [HttpGet("CheckUserHasInvoiceAccess")]
        public async Task<bool> CheckUserHasInvoiceAccess()
        {
            return await _manager.CheckUserHasInvoiceAccess();
        }

        [HttpGet("GetExpertiseList")]
        public async Task<DataSourceResponse> GetExpertiseList()
        {
            return await _manager.GetExpertiseList();
        }

        [HttpGet("getCurrencyListWithCode")]
        public async Task<CurrencyDataSourceResponse> getCurrencyListWithCode()
        {
            return await _manager.GetCurrencyListWithCurrencyCode();
        }

        [HttpGet("get-inspection-booking-types")]
        public async Task<InspectionBookingTypeResponse> GetInspectionBookingTypeList()
        {
            return await _manager.GetInspectionBookingTypeList();
        }

        [HttpGet("get-inspection-payment-options/{customerId}")]
        public async Task<InspectionPaymentOptionsResponse> GetInspectionPaymentOptions(int customerId)
        {
            return await _manager.GetInspectionPaymentOptions(customerId);
        }

        [HttpGet("get-audit-service-list")]
        public async Task<ServiceTypeResponse> GetAuditServiceList()
        {
            return await _manager.GetAuditServiceTypes();
        }
    }
}