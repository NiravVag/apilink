using DTO.Customer;
using DTO.Inspection;
using Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface ICustomerComplaintManager
    {
        /// <summary>
        /// get all Complaint Type  
        /// </summary>
        /// <returns>List of Complaint Type</returns>
        Task<ComplaintDataResponse> GetComplaintType();
        /// <summary>
        /// get all Complaint Category  
        /// </summary>
        /// <returns>List of Complaint Category</returns>
        Task<ComplaintDataResponse> GetComplaintCategory();
        /// <summary>
        /// get all Complaint RecipientType  
        /// </summary>
        /// <returns>List of Complaint RecipientType</returns>
        Task<ComplaintDataResponse> GetComplaintRecipientType();
        /// <summary>
        /// get all Complaint Department  
        /// </summary>
        /// <returns>List of Complaint Department</returns>
        Task<ComplaintDataResponse> GetComplaintDepartment();
        /// <summary>
        /// get all Booking or audit no data source by serviceid 
        /// </summary>
        /// <returns>List of Complaint Type</returns>
        Task<BookingNoDataSourceResponse> GetBookingNoDataSource(BookingNoDataSourceRequest request);
       
        /// <summary>
        /// get Audit Data
        /// </summary>
        /// <returns>Audit Data</returns>
        Task<ComplaintBookingDataResponse> GetAuditDetails(int AuditId);
        //Get product details by booking
        /// <summary>
        /// Get product details by booking
        /// </summary>
        /// <returns>product details by booking</returns>
        Task<ComplaintBookingProductDataResponse> GetProductItemByBooking(int bookingId);
        //Get Complaint Details By Id
        /// <summary>
        /// Get Complaint Details By Id
        /// </summary>
        /// <returns>Complaint Details By Id</returns>
        Task<ComplaintDetailedInfoResponse> GetComplaintDetailsById(int? Id);
        //Save Complaints
        /// <summary>
        /// Save Complaints
        /// </summary>
        /// <returns>Save Complaints result</returns>
        Task<SaveComplaintResponse> SaveComplaints(ComplaintDetailedInfo request);
        Task<RemoveComplaintDetailResponse> RemoveComplaintDetail(int? id);
        Task<CustomerComplaintEmailTemplate> GetComplaintEmailData(ComplaintDetailedInfo request, int complaitId);

        /// <summary>
        /// get the complaint summary data
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ComplaintSummaryResponse> GetComplaintSummary(ComplaintSummaryRequest request);

        /// <summary>
        /// delete the complaints
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<DeleteComplaintResponse> DeleteComplaint(int id);

        Task<ExportComplaintSummaryResponse> GetComplaintDetails(ComplaintSummaryRequest request);
        Task<List<ExportComplaintSummaryResult>> ExportCompalintSummary(IEnumerable<ExportComplaintSummaryRepoResult> data);
    }
}
