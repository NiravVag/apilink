using DTO.CommonClass;
using DTO.Customer;
using DTO.Inspection;
using DTO.Quotation;
using DTO.RepoRequest.Audit;
using DTO.Report;
using DTO.Schedule;
using Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface ICustomerComplaintRepository : IRepository
    {
        /// <summary>
        /// get all Complaint types from master table
        /// </summary>
        /// <returns>List of Complaint types</returns>
        Task<List<CommonDataSource>> GetComplaintType();
        /// <summary>
        /// get all Complaint Category from master table
        /// </summary>
        /// <returns>List of Complaint Category</returns>
        Task<List<CommonDataSource>> GetComplaintCategory();
        /// <summary>
        /// get all Complaint RecipientType from master table
        /// </summary>
        /// <returns>List of Complaint RecipientType</returns>
        Task<List<CommonDataSource>> GetComplaintRecipientType();
        /// <summary>
        /// get all Complaint Department from master table
        /// </summary>
        /// <returns>List of Complaint Department</returns>
        Task<List<CommonDataSource>> GetComplaintDepartment();
        
         
        /// <summary>
        ///  Complaint by Id
        /// </summary>
        /// <param name="Id"> Complaint by Id</param>
        /// <returns></returns>
        Task<ComplaintDetailedRepo> GetComplaintById(int? Id);
        /// <summary>
        ///  complaintDetailsData   By ID
        /// </summary>
        /// <param name="Id"> complaintDetailsData   By ID</param>
        /// <returns></returns>
       
        Task<IEnumerable<ComplaintDetailRepo>> GetComplaintDetailsById(int? Id);
        /// <summary>
        ///  ComplaintPersonIncharge By ID
        /// </summary>
        /// <param name="Id"> ComplaintPersonIncharge By ID</param>
        /// <returns></returns>
        Task<IEnumerable<int>> GetComplaintPersonInchargeById(int? Id);
        /// <summary>
        /// Add Complaints details
        /// </summary>
        /// <param name="entity">comp complaints</param>
        /// <returns></returns>
        Task<int> AddComplaints(CompComplaint entity);
        /// <summary>
        /// Edit Complaints details
        /// </summary>
        /// <param name="entity">comp complaints</param>
        /// <returns></returns>
        Task<int> EditComplaints(CompComplaint entity);
        Task<CompComplaint> GetComplaintsItemsById(int? id);
        IEnumerable<CompTranComplaintsDetail> GetComplaintDetailItemById(int? Id);

        /// <summary>
        /// get the complaint data
        /// </summary>
        /// <returns></returns>
        IQueryable<CompComplaint> GetComplaintData();
        Task<CustomerComplaintEmailRepo> GetComplaintEmailData(int complaintId);
        Task<List<string>> GetComplaintCategoryData(int complaintId);
        Task<List<ComplaintEmailUserRepo>> GetUserData(List<int> userIds);

        Task<IEnumerable<ExportComplaintDetailRepo>> GetComplaintDetailItemByComplaintIds(List<int> complaintIds);
        Task<IEnumerable<CompTranPersonInCharge>> GetComplaintPersonInchargeByComplaintIds(List<int> complaintIds);
    }
}
