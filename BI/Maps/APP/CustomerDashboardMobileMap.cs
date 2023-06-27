using DTO.Common;
using DTO.Dashboard;
using DTO.MobileApp;
using DTO.User;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BI.Maps.APP
{
    public class CustomerDashboardMobileMap : ApiCommonData
    {
        public DashBoardGraph InspDashboardMobileMap(MobileDashboardResponse data)
        {
            var response = new DashBoardGraph();
            var dailyInspectionList = new List<DailyInspection>();

            int dailyInspKey = 1;
            int failReasonKey = 1;

            //var response = new DashBoardGraph();

            response.manDayOverview = new MobileManDayOverviewDashboard()
            {
                totalInspectionCount = data.manDayData.TotalInspections,
                inspectionAverageCount = data.manDayData.AveragePercentage,
                totalManDayCount = data.manDayData.TotalManDays,
                manDayAverageCount = data.manDayData.TotalManDaysPercentage
            };

            response.resultAnalytics = data.apiData.ConvertAll(x => new MobileResultAnalyticsDashboard
            {
                key = x.Id,
                svg = new SvgColor { fill = x.StatusColor },
                title = x.StatusName,
                count = x.TotalCount
            });

            response.failReasons = data.rejectData.ConvertAll(x => new CommonMobileDataSource
            {
                key = failReasonKey++,
                percent = x.TotalCount,
                svg = new SvgColor { fill = x.StatusColor },
                title = x.StatusName
            });

            response.pendingQuotationCount = data.pendingQuotationCount;

            DateTime yesterday = DateTime.Now.Date.AddDays(-1);
            DateTime dayAfter = DateTime.Now.Date.AddDays(2);
            //get individual dates from the date range
            var dateList = Enumerable.Range(0, 1 + dayAfter.Subtract(yesterday).Days)
                  .Select(offset => yesterday.AddDays(offset)).ToList();

            foreach (var date in dateList)
            {
                string day = "";
                if (date == yesterday)
                    day = DayNames.GetValueOrDefault("Yesterday");
                else if (date == DateTime.Now.Date)
                    day = DayNames.GetValueOrDefault("Today");
                else if (date == DateTime.Now.Date.AddDays(1))
                    day = DayNames.GetValueOrDefault("Tomorrow");
                else
                    day = string.Format("{0: " + StandardDateFormat3 + "}", date);

                var item = new DailyInspection()
                {
                    key = dailyInspKey++,
                    day = day,
                    count = data.currentBookingData.Count(x => x.ServiceDateTo <= date && x.ServiceDateFrom >= date),
                    date = Static_Data_Common.GetCustomDate(date)
                };
                dailyInspectionList.Add(item);
            }
            response.dailyInspection = dailyInspectionList;
            return response;
        }

        public List<MobileTaskModel> MapTask(IEnumerable<TaskModel> data)
        {
            var response = new List<MobileTaskModel>();

            data = data.Where(x => x.Type == TaskType.QuotationCustomerConfirmed).ToList();

            foreach (var task in data)
            {
                var _key = 1;
                response.Add(new MobileTaskModel
                {
                    key = _key++,
                    type = (int)TaskPageType.Quotation,
                    linkId = task.LinkId,
                    taskText = TaskText.GetValueOrDefault((int)TaskType.QuotationCustomerConfirmed) + task.LinkId
                });
            }

            return response;
        }
    }
}
