using DTO.Common;
using DTO.OtherManday;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Maps
{
    public class OtherMandayMap
    {
        public OtherMandayData MapOtherMandayData(OtherMandayDataRepo item)
        {
            return new OtherMandayData
            {
                Id = item.Id,
                CustomerId = item.CustomerId.GetValueOrDefault(),
                CustomerName = item.CustomerName,
                QcId = item.QcId.GetValueOrDefault(),
                QcName = item.QcName,
                OperationalCountryId = item.OperationalCountryId.GetValueOrDefault(),
                OperationalCountryName = item.OperationalCountryName,
                OfficeCountryId = item.OfficeCountryId.GetValueOrDefault(),
                OfficeCountryName = item.OfficeCountryName,
                PurposeId = item.PurposeId.GetValueOrDefault(),
                Purpose = item.Purpose,
                ServiceDate = item.ServiceDate?.ToString(ApiCommonData.StandardDateFormat),
                Manday = item.Manday.GetValueOrDefault(),
                Remarks = item.Remarks,
                ServiceDateObject = Static_Data_Common.GetCustomDate(item.ServiceDate)
            };
        }

        public ExportOtherMandayData MapExportOtherMandayData(OtherMandayDataRepo item)
        {
            return new ExportOtherMandayData
            {
                CustomerName = item.CustomerName,
                QcName = item.QcName,
                OperationalCountryName = item.OperationalCountryName,
                OfficeCountryName = item.OfficeCountryName,
                Purpose = item.Purpose,
                ServiceDate = item.ServiceDate,
                Manday = item.Manday.GetValueOrDefault(),
                Remarks = item.Remarks,
                OfficeName = item.OfficeName,
                CreatedOn = item.CreatedOn,
                CreatedBy = item.CreatedBy
            };
        }
    }
}
