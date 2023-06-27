using DTO.Common;
using DTO.Inspection;
using DTO.InspectionCustomerDecision;
using DTO.Kpi;
using DTO.Report;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using static DTO.Common.Static_Data_Common;

namespace BI.Maps
{
    public class CustomerDecisionMap : ApiCommonData
    {
        public CustomerDecisionSummaryResult CustomerDecisionDataMap(CustomerDecisionSummaryResult cdItem, List<CustomerDecisionCount> cdCount,
            IEnumerable<ServiceTypeList> serviceTypeList, List<CustomerDecisionProductList> productList)
        {
            var serviceType = serviceTypeList.Where(x => x.InspectionId == cdItem.BookingId).FirstOrDefault();
            if (ContainerServiceList.Contains(serviceType.serviceTypeId))
            {
                foreach (var item in productList.Where(x => x.BookingId == cdItem.BookingId))
                {
                    item.ProductIdList = item.ProductIdList.Select(x => "Container - " + x).ToList();
                }
            }

            return new CustomerDecisionSummaryResult
            {
                BookingId = cdItem.BookingId,
                CustomerId = cdItem.CustomerId,
                SupplierId = cdItem.SupplierId,
                FactoryId = cdItem.FactoryId,
                CustomerName = cdItem.CustomerName,
                SupplierName = cdItem.SupplierName,
                FactoryName = cdItem.FactoryName,
                BookingNoCustomerNo = cdItem.BookingId.ToString() + (string.IsNullOrEmpty(cdItem.CustomerBookingNo) ? "" : " /" + cdItem.CustomerBookingNo),
                ServiceDate = cdItem.ServiceDateFrom == cdItem.ServiceDateTo ? cdItem.ServiceDateFrom.ToString(StandardDateFormat) : cdItem.ServiceDateFrom.ToString(StandardDateFormat) + " - " + cdItem.ServiceDateTo.ToString(StandardDateFormat),
                HasCustomerDecisionRole = cdItem.HasCustomerDecisionRole,
                TotalReportCount = cdItem.TotalReportCount,
                ServiceTypeName = serviceType.serviceTypeName,
                DecisionStatusCount = cdCount.Where(x => x.BookingId == cdItem.BookingId).Sum(x => x.Count),
                StatusId = cdItem.StatusId,
                ProductResultList = productList.Where(x => x.BookingId == cdItem.BookingId).ToList()
            };
        }

        public static CustomerDecisionSummaryExport ExportComplaintSummaryDataMap(CustomerDecisionProductList item, List<InspectionBookingExportData> bookingList,
            IEnumerable<ServiceTypeList> serviceTypeList, List<BookingDeptAccess> bookingDeptAccess, List<BookingBrandAccess> bookingBrandAccess, List<BookingBuyerAccess> bookingBuyerAccess,
            List<InspectionPOExportData> poList, List<CustomerDecisionProductList> cusDecisionData)
        {
            List<InspectionPOExportData> poData = null;
            var bookingData = bookingList.FirstOrDefault(x => x.BookingNo == item.BookingId);

            if (serviceTypeList.Any(x => x.InspectionId == item.BookingId && x.serviceTypeId == (int)InspectionServiceTypeEnum.Container))
            {
                poData = poList.Where(x => x.ProductRefId == item.ProductRefId && x.ContainerId == item.ContainerId).ToList();
            }
            else
            {
                //if booking service type is other than container then filter po details only by productid
                poData = poList.Where(x => x.ProductRefId == item.ProductRefId).ToList();
            }

            var cusDecision = cusDecisionData.Where(x => x.ReportId == item.ReportId).FirstOrDefault();

            return new CustomerDecisionSummaryExport
            {
                BookingId = item?.BookingId ?? 0,
                CustomerBookingNo = bookingData?.CustomerBookingNo,
                Customer = bookingData?.Customer,
                Supplier = bookingData?.Supplier,
                Factory = bookingData?.Factory,
                ServiceType = serviceTypeList?.Where(x => x.InspectionId == item.BookingId).Select(x => x.serviceTypeName).FirstOrDefault() ?? "",
                ServiceFromDate = bookingData?.ServiceDateFrom,
                ServiceToDate = bookingData?.ServiceDateTo,
                ProductName = item?.ProductId,
                ProductDesc = item?.ProdDesc,
                PoNumber = string.Join(" ,", poData?.Select(x => x.PONumber).Distinct()),
                ReportResult = item?.ReportResultName,
                CustomerDecision = cusDecision?.CustomerDecisionName,
                CustomerComment = cusDecision?.CustomerDecisionComment,
                Department = string.Join(",", bookingDeptAccess?.Where(x => x.BookingId == item.BookingId).Select(x => x.DeptName).Distinct().ToArray()),
                Brand = string.Join(",", bookingBrandAccess?.Where(x => x.BookingId == item.BookingId).Select(x => x.BrandName).Distinct().ToArray()),
                Buyer = string.Join(",", bookingBuyerAccess?.Where(x => x.BookingId == item.BookingId).Select(x => x.BuyerName).Distinct().ToArray()),
                Collection = bookingData?.Collection,
                ReportId = item?.ReportId ?? 0
            };
        }


        /// <summary>
        /// Map dynamic values to columns headers
        /// </summary>
        /// <param name="dtBookingTable"></param>
        /// <param name="columns"></param>
        /// <param name="dataList"></param>
        /// <param name="removedColumn"></param>
        /// <returns></returns>
        public static DataTable MapDynamicColumns(DataTable dtBookingTable, List<string> columns, List<FbReportInspectionResult> dataList, string removedColumn)
        {
            if (columns != null && columns.Any())
            {
                foreach (DataRow row in dtBookingTable.Rows)
                {

                    var reportResList = dataList.Where
                            (x => x.ReportId == Convert.ToInt32(row["ReportId"].ToString()));

                    if (reportResList.Any())
                    {
                        //map the dynamic column values to column name
                        foreach (var efHeader in columns)
                        {
                            row[efHeader] = reportResList.FirstOrDefault
                               (x => x.Name == efHeader)?.Result;
                        }
                    }
                }
            }

            //remove unwanted columns in the excel
            if (dtBookingTable != null && dtBookingTable.Columns.Count > 0 && !string.IsNullOrEmpty(removedColumn))
            {
                dtBookingTable.Columns.Remove(removedColumn);
            }
            return dtBookingTable;
        }
    }
}
