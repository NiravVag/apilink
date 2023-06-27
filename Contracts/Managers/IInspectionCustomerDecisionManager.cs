using DTO.CommonClass;
using DTO.EmailSendingDetails;
using DTO.Inspection;
using DTO.InspectionCertificate;
using DTO.InspectionCustomerDecision;
using DTO.MobileApp;
using Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface IInspectionCustomerDecisionManager
    {
        Task<CustomerDecisionListResponse> GetCustomerDecisionList(int customerId);
        Task<CustomerDecisionSaveResponse> AddCustomerDecision(CustomerDecisionSaveRequest modelData);
        Task<CustomerDecisionResponse> GetCustomerDecisionData(int reportId);
        Task<ReportCustomerDecisionResponse> GetCustomerDecisionReportsData(int reportId);
        Task<List<FBReportCustomerDecision>> GetCustomerDescistionWithReportId(List<int> fbReportIds);
        Task<CustomerDecisionSaveMobileResponse> SaveMobileCustomerDecision(CustomerDecisionMobileSaveRequest request);
        Task<CustomerDecisionReponse> CustomerDecisionSummary(CustomerDecisionSummaryRequest request);
        Task<EditCustomerDecisionResponse> GetCustomerDecisionBookingAndProducts(int bookingId);
        Task<CustomerDecisionSaveResponse> SaveCustomerDecisionList(CustomerDecisionListSaveRequest request);
        Task<CusDecisionProblematicRemarksResponse> GetProblematicRemarksByReport(int id, int fbReportId);
        Task<int> GetBookingIdByReportId(int reportId);
        Task<DataTable> ExportCustomerDecisionSummary(CustomerDecisionSummaryRequest request);
        Task<IEnumerable<InspRepCusDecisionTemplate>> GetCusDecisionTemplate();
    }
}
