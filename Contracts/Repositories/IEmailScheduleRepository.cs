using DTO.Common;
using DTO.CommonClass;
using DTO.Schedule;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface IEmailScheduleRepository
    {
        Task<IEnumerable<ScheduleStaffItem>> GetScheduledDetailsForQCsByServiceDate(DateTime startDate, DateTime endDate, List<int> officeList);
        Task<IEnumerable<ScheduleQCEntityData>> GetScheduledInspectionByInspectionList(List<int> bookingIds);

        Task<List<CommonDataSource>> GetCSName(List<int> bookingIds);

        Task<IEnumerable<ScheduleStaffItem>> GetAuditorDetailsByServiceDate(DateTime startDate, DateTime endDate, List<int> office);

        Task<IEnumerable<ScheduleQCEntityData>> GetAuditDataByAuditIds(List<int> auditIds);
    }


}
