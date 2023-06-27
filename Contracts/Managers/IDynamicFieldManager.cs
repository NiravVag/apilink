using DTO.DynamicFields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface IDynamicFieldManager
    {
        /// <summary>
        /// Get the modules
        /// </summary>
        /// <returns></returns>
        ModuleResponse GetModules();
        /// <summary>
        /// Get the ControlTypes
        /// </summary>
        /// <returns></returns>
        ControlTypeResponse GetControlTypes();
        /// <summary>
        /// Get the DDL Souce Types
        /// </summary>
        /// <returns></returns>
        DFDDLSourceTypeResponse GetDDLSourceTypes(int customerId);
        /// <summary>
        /// Get the DDL Source
        /// </summary>
        /// <returns></returns>
        DFDDLSourceResponse GetDDLSource(int typeId);
        /// <summary>
        /// Save Customer Configuration
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<SaveDfCustomerConfigurationResponse> Save(DfCustomerConfiguration request);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EditDFCustomerConfigResponse> GetEditDfCustomerConfiguration(int id);
        /// <summary>
        /// Search Dynamic Field Customer Configurations
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<DfCustomerSearchResponse> SearchDfCustomerConfiguration(DfCustomerSearchRequest request);
        /// <summary>
        /// GetDfCustomerConfiguration
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        DfCustomerConfigurationListResponse GetDfCustomerConfiguration(int customerId, int moduleId);
        /// <summary>
        /// GetDfControlTypeAttributes
        /// </summary>
        /// <param name="controlTypeId"></param>
        /// <returns></returns>
        DfControlTypeAttributesResponse GetDfControlTypeAttributes(int controlTypeId);

        Task<DFParentDropDownResponse> GetParentDropDownTypes(int customerId);

        Task<DFCustomerConfigurationDeleteResponse> RemoveDFCustomerConfiguration(int id);

        Task<bool> CheckDFCustomerConfigInBooking(int id);

        Task<InspectionBookingDFDataResponse> GetBookingDFDataByBookingIds(IEnumerable<int> bookingIds);

        Task<InspectionBookingDFDataResponse> GetBookingDFDataByBookings(IQueryable<int> bookingIds);

        /// <summary>
        /// dynamic field gap customer configuration
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        DfCustomerConfigurationListResponse DFGapCuConfiguration(DfCustomerConfigurationRequest request);
        Task<string> GetBookingAuditProductCategory(int bookingId, int? dataSourceTypeId);

    }
}
