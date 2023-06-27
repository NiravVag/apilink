using Contracts.Repositories;
using DTO.CommonClass;
using DTO.Customer;
using DTO.Inspection;
using DTO.Quotation;
using DTO.RepoRequest.Audit;
using DTO.Report;
using DTO.Schedule;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class CustomerComplaintRepository : Repository, ICustomerComplaintRepository
    {
        public CustomerComplaintRepository(API_DBContext context) : base(context)
        {
        }
        //Get All Complaint Type
        public Task<List<CommonDataSource>> GetComplaintType()
        {
           return _context.CompRefTypes.Where(x => x.Active).OrderBy(x => x.Sort)
                                  .Select(x => new CommonDataSource { Id = x.Id, Name = x.Name }).AsNoTracking().ToListAsync();
        }
        //Get All category  
        public Task<List<CommonDataSource>> GetComplaintCategory()
        {
            return _context.CompRefCategories.Where(x => x.Active).OrderBy(x => x.Sort)
                                  .Select(x => new CommonDataSource { Id = x.Id, Name = x.Name }).AsNoTracking().ToListAsync();
        }
        //Get All Recipient Type
        public Task<List<CommonDataSource>> GetComplaintRecipientType()
        {
            return _context.CompRefRecipientTypes.Where(x => x.Active).OrderBy(x => x.Sort)
                                  .Select(x => new CommonDataSource { Id = x.Id, Name = x.Name }).AsNoTracking().ToListAsync();
        }
        //Get All Department
        public Task<List<CommonDataSource>> GetComplaintDepartment()
        {
            return _context.CompRefDepartments.Where(x => x.Active).OrderBy(x => x.Sort)
                                  .Select(x => new CommonDataSource { Id = x.Id, Name = x.Name }).AsNoTracking().ToListAsync();
        }
         
        
        /// Get the complaintData Data By ID
        public async Task<ComplaintDetailedRepo> GetComplaintById(int? Id)  
        {
            return await _context.CompComplaints.Where(x => x.Id == Id && x.Active.Value).
                 Select(x => new ComplaintDetailedRepo
                 {
                     Id = x.Id,
                     ComplaintTypeId = x.Type,
                     ServiceId = x.Service,
                     BookingNo =  x.InspectionId ,
                     AuditId=   x.AuditId,
                     ComplaintDate = x.ComplaintDate ,
                     RecipientTypeId = x.RecipientType,
                     DepartmentId = x.Department,
                     CustomerId = x.CustomerId,
                     CountryId = x.Country,
                     OfficeId = x.Office,
                     Remarks=x.Remarks
                 }).AsNoTracking().FirstOrDefaultAsync();
                 
        }
       // Get Complaints Items By Id
        public async Task<CompComplaint> GetComplaintsItemsById(int? id)
        {

            return await _context.CompComplaints.Where(x => x.Id == id && x.Active.Value)
                .Include(x => x.CompTranComplaintsDetails)
               .Include(x => x.CompTranPersonInCharges)
               .FirstOrDefaultAsync();
        }
            /// Get the complaintDetailsData Data By ID
        public async Task<IEnumerable<ComplaintDetailRepo>> GetComplaintDetailsById(int? Id)
        {
            return await _context.CompTranComplaintsDetails.Where(x => x.ComplaintId == Id && x.Active.Value).
                 Select(x => new ComplaintDetailRepo
                 {
                     Id = x.Id,
                     ProductId = x.ProductId,
                     CategoryId = x.ComplaintCategory,
                     Title = x.Title,
                     Description = x.ComplaintDescription,
                     CorrectiveAction = x.CorrectiveAction,
                     Remarks = x.Remarks,
                     AnswerDate = x.AnswerDate 
                 }).AsNoTracking().ToListAsync();
        }
        /// Get the complaintDetailsData Data By ID
        public IEnumerable<CompTranComplaintsDetail> GetComplaintDetailItemById(int? Id)
        {
            return   _context.CompTranComplaintsDetails.Where(x => x.Id == Id && x.Active.Value);
        }
        /// Get the complaintPersonIncharge Data By ID
        public async Task<IEnumerable<int>> GetComplaintPersonInchargeById(int? Id)
        {
            return await _context.CompTranPersonInCharges.Where(x => x.ComplaintId == Id && x.Active)
                .Select(y=>y.PsersonInCharge).ToListAsync();
        }

        /// <summary>
        /// Add Complaints details
        /// </summary>
        /// <param name="entity">comp complaints</param>
        /// <returns></returns>
        public Task<int> AddComplaints(CompComplaint entity)
        {
            _context.CompComplaints.Add(entity);
             return _context.SaveChangesAsync();
        }
        /// <summary>
        /// Add Complaints details
        /// </summary>
        /// <param name="entity">comp complaints</param>
        /// <returns></returns>
        public Task<int> EditComplaints(CompComplaint entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return _context.SaveChangesAsync();
        }

        public IQueryable<CompComplaint> GetComplaintData()
        {
            return _context.CompComplaints.Where(x => x.Active.Value);
        }
        /// <summary>
        /// Get Booking Products Po Data By ProductRefIds by  booking id
        /// </summary>
        /// <param name="productRefIds"></param>
        /// <returns></returns>
        public async Task<CustomerComplaintEmailRepo> GetComplaintEmailData(int complaintId)
        {
            return await _context.CompComplaints.Where(x => x.Id == complaintId && x.Active.Value).
                            Select(x => new CustomerComplaintEmailRepo
                            {
                                Customer = x.Customer.CustomerName,
                                serviceId=x.Service.GetValueOrDefault(),
                                serviceName=x.ServiceNavigation.Name,
                                BookingNo = x.InspectionId.GetValueOrDefault(),
                                AuditNo = x.AuditId.GetValueOrDefault(),
                                ComplaintDate=x.ComplaintDate,
                                Department=x.DepartmentNavigation.Name
                            }).AsNoTracking().FirstOrDefaultAsync();
        }
        public async Task<List<string>> GetComplaintCategoryData(int complaintId)
        {
            return await _context.CompTranComplaintsDetails.Where(x => x.ComplaintId == complaintId && x.Active.HasValue&&x.Active.Value).
                            Select(y=>y.ComplaintCategoryNavigation.Name).AsNoTracking().ToListAsync();
        }
        public async Task<List<ComplaintEmailUserRepo>> GetUserData(List<int> userIds)
        {
            return await _context.HrStaffs.Where(x => x.Active.HasValue&& userIds.Contains(x.Id)).OrderBy(x => x.PersonName)
                                  .Select(y => new ComplaintEmailUserRepo {
                                      userId = y.Id,
                                      Name = y.PersonName,
                                      EmailAddress=y.CompanyEmail
                                  }
                                  ).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<ExportComplaintDetailRepo>> GetComplaintDetailItemByComplaintIds(List<int> complaintIds)
        {
            return await _context.CompTranComplaintsDetails.Where(x => complaintIds.Contains(x.ComplaintId) && x.Active.Value).
                 Select(x => new ExportComplaintDetailRepo
                 {
                     Id = x.Id,
                     ComplaintId = x.ComplaintId,
                     ProductId = x.ProductId,
                     CategoryId = x.ComplaintCategory,
                     ProductDescription = x.Product.ProductDescription,
                     Category = x.ComplaintCategoryNavigation.Name,
                     Title = x.Title,
                     Description = x.ComplaintDescription,
                     CorrectiveAction = x.CorrectiveAction,
                     Remarks = x.Remarks,
                     AnswerDate = x.AnswerDate
                 }).AsNoTracking().ToListAsync();
        }

        /// Get the complaintPersonIncharge Data By ID
        public async Task<IEnumerable<CompTranPersonInCharge>> GetComplaintPersonInchargeByComplaintIds(List<int> complaintIds)
        {
            return await _context.CompTranPersonInCharges.Where(x => complaintIds.Contains(x.ComplaintId) && x.Active).ToListAsync();
        }
    }
}
