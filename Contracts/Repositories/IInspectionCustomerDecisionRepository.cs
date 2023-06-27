using DTO.EmailSendingDetails;
using DTO.Inspection;
using DTO.InspectionCertificate;
using DTO.InspectionCustomerDecision;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface IInspectionCustomerDecisionRepository : IRepository
    {
        Task<List<CustomerDecisionRepo>> GetCustomerDefaultDecisionList();
        Task<List<CustomerDecisionRepo>> GetCustomerDecisionListByCustomer(int customerId);
        Task<int> AddCustomerDecision(InspRepCusDecision entity);
        Task<int> UpdateCustomerDecision(InspRepCusDecision entity);
        Task<InspRepCusDecision> GetCustomerDecisionData(int reportId);
        Task<ReportCustomerDecision> GetReportCustomerDecision(int reportId);
        Task<List<FBReportCustomerDecision>> GetCustomerDescistionWithReportId(List<int> fbReportIds);
        Task<List<CustomerDecisionCount>> GetCustomerDescistionWithBookingId(List<int> bookingIds);
        Task<List<CustomerDecisionProductList>> GetCustomerDecisionProductList(List<int> bookingId);
        Task<List<CustomerDecisionProductList>> GetCustomerDecisionContainerList(List<int> bookingId);
        Task<List<InspRepCusDecision>> GetCustomerDecisionListData(List<int> reportIdList);
        Task<List<CusDecisionProblematicRemarks>> GetProblematicRemarksCd(int id, int reportId);
        Task<int> GetBookingIdByReportId(int reportId);
        IQueryable<CustomerDecisionRepo> GetCustomerDecisionListByEfCore();
        Task<List<CustomerDecisionProductList>> GetCustomerDecisionContainerListByEfCore(IQueryable<int> bookingIds);
        Task<List<CustomerDecisionProductList>> GetCustomerDecisionProductListByEfCore(IQueryable<int> bookingIds);
        Task<List<CustomerDecisionProductList>> GetCustomerDecisionByEfCore(IQueryable<int> bookingIds);
        Task<IEnumerable<InspRepCusDecisionTemplate>> GetCusDecisionTemplate();
        Task<List<CustomerDecisionProductList>> GetCustomerDecisionByEfCoreReportIds(IQueryable<int> reportIds);
    }
}
