using DTO.AuditReport;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
   public interface IAuditCusReportManager
    {
        Task<AuditCusReportBookingDetailsResponse> SearchAuditCusReport(AuditCusReportBookingDetailsRequest request);
    }
}
