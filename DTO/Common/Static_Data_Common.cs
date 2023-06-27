using DTO.Dashboard;
using DTO.ExtraFees;
using DTO.Invoice;
using DTO.UtilizationDashboard;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace DTO.Common
{
    public static class Static_Data_Common
    {

        /// <summary>
        /// Returns TimeZone adjusted time for a given from a Utc or local time.
        /// Date is first converted to UTC then adjusted.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="timeZoneId"></param>
        /// <returns></returns>
        public static DateTime ToTimeZoneTime(this DateTime time, string timeZoneId = "Canada Central Standard Time")
        {
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return time.ToTimeZoneTime(tzi);
        }

        /// <summary>
        /// Returns TimeZone adjusted time for a given from a Utc or local time.
        /// Date is first converted to UTC then adjusted.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="timeZoneId"></param>
        /// <returns></returns>
        public static DateTime ToTimeZoneTime(this DateTime time, TimeZoneInfo tzi)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(time, tzi);
        }

        public static DateObject GetCustomDate(this DateTime? date)
        {
            if (date == null)
                return null;

            return new DateObject
            {
                Day = date.Value.Day,
                Month = date.Value.Month,
                Year = date.Value.Year
            };
        }

        public static DateObject GetCustomDate(this DateTime date)
        {
            return new DateObject
            {
                Day = date.Day,
                Month = date.Month,
                Year = date.Year
            };
        }

        public static string RemoveExtraSpace(this string lines)
        {
            if (string.IsNullOrEmpty(lines))
                return string.Empty;
            return lines.Replace("\r\n", string.Empty)
                        ?.Replace("\r", string.Empty)
                        ?.Replace("\n", string.Empty)
                        ?.Replace("\t", string.Empty)
                        ?.Replace(Environment.NewLine, string.Empty)?.Trim();
        }

        public static List<int> ContainerServiceList { get; set; } = new List<int>()
        {
            {(int)InspectionServiceTypeEnum.Container},
            //{(int)InspectionServiceTypeEnum.SgtContainer}
        };

        public enum InternalUserCombineRoleAccess
        {
            InspectionRequest = 23,
            InspectionConfirmed = 24,
            InspectionVerified = 25
        }

        public static List<int> InternalUserCombineRoleAccessList { get; set; } = new List<int>()
        {
            {(int)InternalUserCombineRoleAccess.InspectionRequest},
            {(int)InternalUserCombineRoleAccess.InspectionVerified},
            {(int)InternalUserCombineRoleAccess.InspectionConfirmed}
        };

        public static List<int> InternalUserCombineStatusAccessList { get; set; } = new List<int>()
        {
            {(int)BookingStatus.Received},
            {(int)BookingStatus.Verified},
            {(int)BookingStatus.Confirmed},
            {(int)BookingStatus.AllocateQC},
            {(int)BookingStatus.Rescheduled}
        };

        public static List<int> ExternalUserCombineStatusAccessList { get; set; } = new List<int>()
        {
            {(int)BookingStatus.Received}
        };


        public static void SaveStreamAsFile(string filePath, Stream inputStream, string fileName)
        {
            DirectoryInfo info = new DirectoryInfo(filePath);
            if (!info.Exists)
            {
                info.Create();
            }

            string path = Path.Combine(filePath, fileName);
            using (FileStream outputFileStream = new FileStream(path, FileMode.Create))
            {
                inputStream.CopyTo(outputFileStream);
            }
        }
    }
}

