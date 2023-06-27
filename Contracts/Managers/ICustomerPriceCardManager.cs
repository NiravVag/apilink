using DTO.CommonClass;
using DTO.Customer;
using DTO.CustomerPriceCard;
using DTO.Inspection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface ICustomerPriceCardManager
    {
        /// <summary>
        /// save
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<SaveCustomerPriceCardResponse> Save(CustomerPriceCard request);
        /// <summary>
        /// edit the record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EditSaveCustomerPriceCardResponse> Edit(int id);
        /// <summary>
        /// get data from db
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<CustomerPriceCardSummaryResponse> GetData(CustomerPriceCardSummary request);
        /// <summary>
        /// delete the record logically
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseResult> Delete(int id);
        /// <summary>
        /// get unit price value based on customer and service id
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        Task<IEnumerable<CustomerPriceCardDetails>> GetCustomerUnitPriceByCustomerIdServiceId(int customerId, int serviceId);
     
       
        /// <summary>
        /// ExportSummary details
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<List<ExportSummary>> ExportSummary(CustomerPriceCardSummary request);

        /// <summary>
        /// Get the customer price holiday list
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        Task<DataSourceResponse> GetCustomerPriceHolidayList();

    }
}
