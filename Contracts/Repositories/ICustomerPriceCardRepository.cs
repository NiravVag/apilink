using DTO.CommonClass;
using DTO.Customer;
using DTO.CustomerPriceCard;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface ICustomerPriceCardRepository : IRepository
    {
        /// <summary>
        /// GetCustomerPriceCardDetail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<CustomerPriceCardRepo> GetCustomerPriceCardDetail(int? id);
        /// <summary>
        /// GetCustomerPriceCardDetails
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<CuPrDetail> GetCustomerPriceCardDetails(int? id);
        /// <summary>
        /// GetAllData
        /// </summary>
        /// <returns></returns>
        IQueryable<CuPrDetail> GetAllData();

        /// <summary>
        /// IsExists
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        IQueryable<CuPrDetail> IsExists(CustomerPriceCard request);
        /// <summary>
        /// get unit price value based on customer and service id
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        IQueryable<CuPrDetail> GetCustomerUnitPriceByCustomerIdServiceId(int customerId, int serviceId);
        /// <summary>
        /// GetCustomerPriceCardData for quotation
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<QuotationCustomerPriceCardData>> GetCustomerPriceCardData(IEnumerable<int> PriceIdList);

        /// <summary>
        /// Get the customer price holiday list
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        Task<List<CommonDataSource>> GetCustomerPriceHolidayList();


        /// <summary>
        /// Customer price card Invoice Request
        /// </summary>
        /// <param name="priceCardId"></param>
        /// <returns></returns>
        Task<IEnumerable<PriceInvoiceRequest>> GetCustomerPriceCardInvoiceRequest(int? priceCardId);

        /// <summary>
        /// Customer price card details for Export
        /// </summary>
        /// <param name="pricecardIdList"></param>
        /// <returns></returns>
        Task<List<ExportSummaryItem>> GetCustomerPriceCardDetailForExport(List<int> priceCardIsList);

        /// <summary>
        /// Customer price card supplier
        /// </summary>
        /// <param name="pricecardIdList"></param>
        /// <returns></returns>
        Task<List<CuPrCommonDataSource>> GetPrSuppliers(List<int> priceCardIdList);

        /// <summary>
        /// Customer price card product category
        /// </summary>
        /// <param name="pricecardIdList"></param>
        /// <returns></returns>
        Task<List<CuPrCommonDataSource>> GetPrProductCategories(List<int> priceCardIdList);

        /// <summary>
        /// Customer price card service type
        /// </summary>
        /// <param name="pricecardIdList"></param>
        /// <returns></returns>
        Task<List<CuPrCommonDataSource>> GetPrServiceTypes(List<int> priceCardIdList);

        /// <summary>
        /// Customer price card country
        /// </summary>
        /// <param name="pricecardIdList"></param>
        /// <returns></returns>
        Task<List<CuPrCommonDataSource>> GetPrCountries(List<int> priceCardIdList);

        /// <summary>
        /// Customer price card province
        /// </summary>
        /// <param name="pricecardIdList"></param>
        /// <returns></returns>
        Task<List<CuPrCommonDataSource>> GetPrProvince(List<int> priceCardIdList);

        /// <summary>
        /// Customer price card details for Export
        /// </summary>
        /// <param name="pricecardIdList"></param>
        /// <returns></returns>
        Task<List<CuPrCommonDataSource>> GetPrDepartment(List<int> priceCardIdList);

        /// <summary>
        /// Customer price card brand
        /// </summary>
        /// <param name="pricecardIdList"></param>
        /// <returns></returns>
        Task<List<CuPrCommonDataSource>> GetPrBrand(List<int> priceCardIdList);

        /// <summary>
        /// Customer price card details for Export
        /// </summary>
        /// <param name="pricecardIdList"></param>
        /// <returns></returns>
        Task<List<CuPrCommonDataSource>> GetPrBuyer(List<int> priceCardIdList);

        /// <summary>
        /// Customer price card price category
        /// </summary>
        /// <param name="pricecardIdList"></param>
        /// <returns></returns>
        Task<List<CuPrCommonDataSource>> GetPrPriceCategory(List<int> priceCardIdList);

        /// <summary>
        /// Customer price card holiday type
        /// </summary>
        /// <param name="pricecardIdList"></param>
        /// <returns></returns>
        Task<List<CuPrCommonDataSource>> GetPrHolidayType(List<int> priceCardIdList);

        /// <summary>
        /// get invoice request contact
        /// </summary>
        /// <param name="pricecardIdList"></param>
        /// <returns></returns>
        Task<List<CuPrCommonDataSource>> GetPrContacts(List<int> priceCardIdList);
        Task<List<CuPrContactCommonDataSource>> GetInvoiceContacts(List<int> invoiceRequestIds);
        Task<List<CuPrHolidayType>> GetHolidayTypeList(List<int> priceCardIdList);
        Task<List<CuPrCommonDataSource>> GetPrInspectionLocation(List<int> priceCardIdList);
        Task<IEnumerable<PriceSubCategory>> GetCustomerPriceCardSubCategory(int? priceCardId);
        Task<IEnumerable<PriceSpecialRule>> GetCustomerPriceRuleList(int? priceCardId);
        Task<List<CuPrCommonDataSource>> GetPrProductSubCategories(List<int> priceCardIdList);
        Task<QuotationCustomerPriceCard> GetQuotationPriceCard(int ruleId);
        Task<QuotationCustomerPriceCardData> GetQuotationCustomerPriceCardData(int ruleId);
        Task<List<CuPrCommonDataSource>> GetPrCity(List<int> priceCardIdList);
    }
}
