using DTO.Common;
using DTO.Inspection;
using DTO.MobileApp;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace BI.Maps.APP
{
    public class InspReportMobileMap : ApiCommonData
    {
        public List<MobileInspectionReportProducData> InspReportMap(List<InternalReportProducts> productList, int? pageindex = null, int? pagesize = null, int? skip = null, int? take = null)
        {
            var response = new List<MobileInspectionReportProducData>();

            int _key = pageindex != null && pagesize != null ? ((pageindex.GetValueOrDefault() - 1) * pagesize.GetValueOrDefault()) + 1 : 1;

            var reportIdList = productList.Where(x => x.FbReportId > 0).Select(x => x.FbReportId).Distinct().ToList();

            if (skip > 0 && take > 0)
            {
                reportIdList = reportIdList.Skip(skip.Value).Take(take.Value).ToList();
            }

            foreach (var reportId in reportIdList)
            {
                var productData = productList.FirstOrDefault(x => x.FbReportId == reportId);

                response.Add(new MobileInspectionReportProducData()
                {
                    key = _key++,
                    ProductName = productData.ProductName,
                    PoNumber = string.Join(",", productList.Where(x => x.ProductName == productData.ProductName).Select(x => x.PONumber).ToList()),
                    ReportId = productData.FbReportId,
                    ReportNo = productData.ReportTitle,
                    ProductDescription = productData.ProductDescription,
                    CombineProductCount = productList.Where(x => x.FbReportId == productData.FbReportId).Select(x => x.ProductId).Distinct().Count() > 1 ? productList.Where(x => x.FbReportId == productData.FbReportId).Select(x => x.ProductId).Distinct().Count() : 0,
                    InspectionDate = (productData.ServiceDateFrom != null && productData.ServiceDateTo != null) ? productData.ServiceDateFrom == productData.ServiceDateTo ? string.Format("{0: " + StandardDateFormat3 + "}", productData.ServiceDateFrom.GetValueOrDefault())
                        : string.Format("{0: " + StandardDateFormat3 + "}", productData.ServiceDateFrom.GetValueOrDefault()) + " to " + string.Format("{0: " + StandardDateFormat3 + "}", productData.ServiceDateTo.GetValueOrDefault()) : "",
                    ProductImageUrl = productData.ProductImageUrl,
                    ReportUrl = !string.IsNullOrEmpty(productData.FinalReportManualPath) ? productData.FinalReportManualPath : productData.ReportPath,
                    ReportSummaryUrl = productData.ReportSummaryLink,
                    ReportResultId = productData.ReportStatusId,
                    BookingStatus = productData.BookingStatus,
                    CombineAqlQuantity = productData.CombineAqlQuantity,
                    CombineProductId = productData.CombineProductId,
                    CreatedDate = productData.CreatedDate,
                    ServiceStartDate = productData.ServiceDateFrom,
                    ServiceEndDate = productData.ServiceDateTo
                });
            }

            return response;
        }
    }
}
