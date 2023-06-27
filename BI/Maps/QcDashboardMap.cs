using System;
using System.Collections.Generic;
using System.Linq;
using DTO.Quotation;
using DTO.Common;
using DTO.File;
using DTO.Inspection;
using DTO.Report;
using DTO.Schedule;
using DTO.QcDashboard;
using Entities;
using Entities.Enums;
using DTO.HumanResource;
using DTO.User;
using DTO.DataAccess;

namespace BI.Maps
{
    public  class QcDashboardMap: ApiCommonData
    {
        public  QcDashboardCalendarScheduleItem GetQcInspectionResult(ScheduleBookingInfo entity,
            IEnumerable<FactoryCountry> factoryCountryData)
        {

            var factoryLocation = factoryCountryData.FirstOrDefault(x => x.BookingId == entity.BookingId);
            return new QcDashboardCalendarScheduleItem()
            {
                BookingIds = entity.BookingId.ToString(),
                FactoryId=entity.FactoryId,
                FactoryName = factoryLocation.FactoryCountryId== (int)CountryEnum.China? entity.FactoryName + " " + entity.FactoryRegionalName: entity.FactoryName,
                FactoryAddress = factoryLocation.FactoryCountryId == (int)CountryEnum.China ? factoryLocation.FactoryAdress+" "+ factoryLocation.FactoryRegionalAddress : factoryLocation.FactoryAdress,
                ServiceDateFrom =  entity?.ServiceDateFrom,
                ServiceDateTo = entity?.ServiceDateTo
            };
        }

        /// <summary>
        /// Map the qc calendar schedule data
        /// </summary>
        /// <param name="ScheDate"></param>
        /// <param name="data"></param>
        /// <param name="factoryCountryList"></param>
        /// <returns></returns>
        public  QcDashboardCalendar GetQcScheduleResultData(DateTime ScheDate, List<QCBookings> data, List<FactoryCountry> factoryCountryList)
        {
            var todayDate = DateTime.Now.Date;
            int _dayType = 1;
            if (ScheDate < todayDate)
                _dayType = 0;
            else if (ScheDate > todayDate)
                _dayType = 2;
            return new QcDashboardCalendar()
            {
                ServiceDateFrom = ScheDate,
                calendarDay = (int)ScheDate.DayOfWeek,
                calendarMonth = ScheDate.Month,
                calendarDate = ScheDate.Day,
                DayType = _dayType,
                QcCalendarSchedule = getQCScheduledDataByFactoryGrouping(data.Where(x => x.ScheduledDate.Date == ScheDate.Date).OrderBy(y => y.BookingId).ToList(),factoryCountryList)
            };
        }

        /// <summary>
        /// Map the grouped factory schedule calendar data
        /// </summary>
        /// <param name="QcBookingList"></param>
        /// <param name="factoryCountryList"></param>
        /// <returns></returns>
        private  List<QcDashboardCalendarScheduleItem> getQCScheduledDataByFactoryGrouping(List<QCBookings> QcBookingList, List<FactoryCountry> factoryCountryList)
        {
            List<QcDashboardCalendarScheduleItem> qcScheduleItems = new List<QcDashboardCalendarScheduleItem>();
            //group the data by factory
            var groupedQCBookingList = QcBookingList
                                .GroupBy(s => s.FactoryId)
                                .Select(grp => grp.ToList()).ToList();

            foreach (var qcBookings in groupedQCBookingList)
            {
                QcDashboardCalendarScheduleItem qcScheduleItem = new QcDashboardCalendarScheduleItem();
                var bookingId = qcBookings.Select(x => x.BookingId).FirstOrDefault();
                var factoryAddress = factoryCountryList.FirstOrDefault(x => x.BookingId == bookingId);
                qcScheduleItem.FactoryId = qcBookings.Select(x => x.FactoryId).FirstOrDefault();
                qcScheduleItem.FactoryName = factoryAddress.FactoryCountryId == (int)CountryEnum.China ? qcBookings.Select(x => x.FactoryName).FirstOrDefault() + " " 
                                            + qcBookings.Select(x => x.FactoryRegionalName).FirstOrDefault() : qcBookings.Select(x => x.FactoryName).FirstOrDefault();
                qcScheduleItem.FactoryAddress = factoryAddress.FactoryCountryId == (int)CountryEnum.China ? factoryAddress.FactoryAdress + " "+ factoryAddress.FactoryRegionalAddress : factoryAddress.FactoryAdress;
                qcScheduleItem.ServiceDateFrom = qcBookings.Select(x => x.ServiceDateFrom).FirstOrDefault();
                qcScheduleItem.ServiceDateTo = qcBookings.Select(x => x.ServiceDateTo).FirstOrDefault();
                qcScheduleItem.BookingIds = string.Join(",", qcBookings.Select(k => k.BookingId));
                qcScheduleItems.Add(qcScheduleItem);
            }

            return qcScheduleItems;
        }

        public  QcReportscount GetQcReportCountData(DateTime DateValue, IEnumerable<QcReportscount> data)
        {
            
            return new QcReportscount()
            {
               ServiceDate= DateValue,
               ReportCount= (int)data.Where(x=>x.ServiceDate== DateValue).Select(x=>x.ReportCount).FirstOrDefault()
            };
        }
    }
}
