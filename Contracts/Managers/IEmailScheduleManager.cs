using DTO.Schedule;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface IEmailScheduleManager
    {
        Task<List<ScheduleQCEmailTemplate>> scheduleQCEmail(bool isFromScheduler, List<int> bookingIds,string offices);
    }
}
