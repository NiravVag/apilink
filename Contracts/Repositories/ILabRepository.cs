using DTO.Lab;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface ILabRepository : IRepository
    {
        /// <summary>
        /// Get All Lab Details
        /// </summary>
        /// <returns></returns>
        IEnumerable<InspLabDetail> GetAllLabDetails();

        /// <summary>
        /// Get Lab Details
        /// </summary>
        /// <param name="labId"></param>
        /// <returns>Insp Lab Detail</returns>
        Task<InspLabDetail> GetLabDetailsById(int? labId);

        /// <summary>
        /// Add New Lab Details
        /// </summary>
        /// <param name="labDetail"></param>
        /// <returns></returns>
        Task<int> AddLabDetails(InspLabDetail entity);

        /// <summary>
        /// Save Edit Lab Details
        /// </summary>
        /// <param name="labDetail"></param>
        /// <returns></returns>
        Task<int> EditLabDetails(InspLabDetail entity);

        /// <summary>
        /// Get All Lab Address Type
        /// </summary>
        /// <returns></returns>
        IEnumerable<InspLabAddressType> GetAllLabAddressType();

        /// <summary>
        /// Get All Lab Type
        /// </summary>
        /// <returns></returns>
        IEnumerable<InspLabType> GetAllLabType();

        /// <summary>
        ///  Get Lab types
        /// </summary>
        /// <returns></returns>
        Task<List<InspLabType>> GetLabTypes();
        
        /// <summary>
        /// Get Lab Address by Lab Id
        /// </summary>
        /// <param name="labId"></param>
        /// <returns></returns>
        IEnumerable<InspLabAddress> GetLabAddressByLabId(int? labId);
        /// <summary>
        /// Get Lab Contacts by labId and Customer Id
        /// </summary>
        /// <param name="labId"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        IEnumerable<InspLabContact> GetLabContactByLabIdAndCustomerId(int? labId, int? customerId);
        /// <summary>
        /// Get all lab
        /// </summary>
        /// <returns>list lab</returns>
        IEnumerable<InspLabDetail> GetAllLab();

        IQueryable<InspLabAddress> GetLabAddressByLabIdList(List<int?> labIds);

        IQueryable<InspLabContact> GetLabContactByLabIdListAndCustomerId(LabContactRequest request);
        Task<InspLabDetail> GetLabDetailById(int? labId);
    }
}
