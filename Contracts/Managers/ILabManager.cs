using DTO.Lab;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
	public interface ILabManager
	{
		/// <summary>
		/// Get All Lab Address Type
		/// </summary>
		/// <returns>Insp Lab Addres sType Response</returns>
		LabAddressTypeResponse GetLabAddressTypeSummary();
	
		/// <summary>
		/// Get All Lab Type 
		/// </summary>
		/// <returns>Insp Lab Type Response</returns>
		LabTypeResponse GetLabTypeSummary();

		/// <summary>
		/// Get All Lab Details 
		/// </summary>
		/// <returns>Insp Lab Details Response</returns>
		LabDetailsResponse GetLabDetailsSummary();



		/// <summary>
		/// Save Edit Lab Details
		/// </summary>
		/// <param name="request"></param>
		/// <returns>SaveLabResponse</returns>
		Task<SaveLabResponse> Save(LabDetails request);

		/// <summary>
		/// Delete Lab
		/// </summary>
		/// <param name="id"></param>
		/// <returns>LabDeleteResponse</returns>
		Task<LabDeleteResponse> DeleteLab(int id);

		/// <summary>
		/// Get Edit Lab
		/// </summary>
		/// <param name="id"></param>
		/// <returns>EditLabResponse</returns>
		Task<EditLabResponse> GetEditLabById(int? id);

        /// <summary>
        /// Lab Search
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        LabSearchResponse GetLabSearchData(LabSearchRequest request);

        /// <summary>
        /// Get lab details by Customer Id
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        LabDataList GetLabDetailsByCustomerId(int? customerId);
        /// <summary>
        /// Get Lab Address by Lab Id
        /// </summary>
        /// <param name="labId"></param>
        /// <returns></returns>
        Task<LabAddressDataList> GetLabAddressByLabId(int? labId);
        /// <summary>
        /// Get Lab Contacts by labId and Customer Id
        /// </summary>
        /// <param name="labId"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        Task<LabContactsDataList> GetLabContactByLabIdAndCustomerId(int? labId, int? customerId);

		Task<LabAddressListResponse> GetLabAddressByLabIdList(LabAddressRequest request);

		Task<LabContactsListResponse> GetLabContactByLabListAndCustomerId(LabContactRequest request);

		Task<SaveLabAddressResponse> SaveLabAddressList(SaveLabAddressRequestData request);


	}
}
