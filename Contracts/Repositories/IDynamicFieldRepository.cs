using DTO.DynamicFields;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface IDynamicFieldRepository:IRepository
    {
        /// <summary>
        /// Get all the Modules
        /// </summary>
        /// <returns></returns>
        IEnumerable<RefModule> GetModules();
        /// <summary>
        /// Get all the control types
        /// </summary>
        /// <returns></returns>
        IEnumerable<DfControlType> GetControlTypes();
        /// <summary>
        /// Get dropdown source types
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        IEnumerable<DfDdlSourceType> GetDDLSourceTypes(int customerId);
        /// <summary>
        /// Get the dropdown actual source
        /// </summary>
        /// <returns></returns>
        IEnumerable<DfDdlSource> GetDDLSource(int typeId);
        /// <summary>
        /// Save the Customer DF Configuration
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> AddDfCustomerConfiguration(DfCuConfiguration entity);
        /// <summary>
        /// Get the dfcustomerconfiguration data
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DfCuConfiguration GetDfCustomerConfiguration(int id);
        /// <summary>
        /// EditDfCuConfiguration
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> EditDfCuConfiguration(DfCuConfiguration entity);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<DfCustomerConfigSearchData>> GetDfCustomerConfigData();
        /// <summary>
        /// GetDfCustomerConfiguration
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        IQueryable<DfCuConfiguration> GetDfCustomerConfiguration(int customerId, int moduleId);
        /// <summary>
        /// GetDDLControlTypeAttributes
        /// </summary>
        /// <param name="controlTypeId"></param>
        /// <returns></returns>
        IEnumerable<DfControlTypeAttribute> GetDFControlTypeAttributes(int controlTypeId);

        Task<List<int?>> GetParentDropDownIds(int customerId);
        Task<List<DfParentDDLSource>> GetParentDropDownTypes(List<int?> dataSourceIds);

        Task<DfCustomerConfigBaseData> GetDfCustomerConfigBaseData(int id);
        Task<List<EditDfControlAttributes>> GetDfCustomerConfigAttributes(int id);
        Task<bool> RemoveDFCustomerConfiguration(int id);
        Task<bool> CheckDFCustomerConfigInBooking(int id);
        Task<List<InspectionDFCustomerConfig>> GetInspectionDFCustomerConfig();
        /// <summary>
        /// Get dynamic data by booking ids
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        Task<List<InspectionBookingDFRepo>> GetBookingDFDataByBookingIds(IEnumerable<int> bookingIds);
        /// <summary>
        /// get data source values by ids
        /// </summary>
        /// <param name="dataSourceTypeIds"></param>
        /// <returns></returns>
        Task<List<DFDataSourceRepo>> GetDropDownSourceByDataSourceTypeId(IEnumerable<int> dataSourceTypeIds);

        Task<List<InspectionBookingDFRepo>> GetBookingDFDataByBookings(IQueryable<int> bookingIds);
        Task<string> GetBookingAuditProductCategory(int bookingId, int? dataSourceTypeId);
    }
}
