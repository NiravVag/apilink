using DTO.Common;
using DTO.Inspection;
using DTO.InspectionCustomerDecision;
using DTO.Kpi;
using DTO.MobileApp;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BI.Maps.APP
{
    public class InspSummaryMobileMap : ApiCommonData
    {
        public List<MobileInspectionData> InspSummaryMap(List<BookingItem> data, int pageIndex, List<BookingReportData> reportData)
        {
            //page index - 1 * 10 (pagesize) + 1
            var _key = ((pageIndex - 1) * PageSize) + 1;

            var response = data.ConvertAll(x => new MobileInspectionData
            {
                key = _key++,
                inspectionId = x.BookingId,
                supplierId = x.SupplierId.GetValueOrDefault(),
                supplierName = x.SupplierName,
                factoryId = x.FactoryId.GetValueOrDefault(),
                factoryName = x.FactoryName,
                serviceTypeId = x.ServiceTypeId,
                serviceTypeName = x.ServiceType,
                serviceDate = x.ServiceDateFrom == x.ServiceDateTo ? string.Format("{0: " + StandardDateFormat3 + "}", DateTime.ParseExact(x.ServiceDateFrom, StandardDateFormat, CultureInfo.InvariantCulture)) : string.Format("{0: " + StandardDateFormat3 + "}", DateTime.ParseExact(x.ServiceDateFrom, StandardDateFormat, CultureInfo.InvariantCulture)) + " to " + string.Format("{0: " + StandardDateFormat3 + "}", DateTime.ParseExact(x.ServiceDateTo, StandardDateFormat, CultureInfo.InvariantCulture)),
                bookingStatusId = x.StatusId,
                isInspected = x.StatusId == (int)BookingStatus.Inspected || x.StatusId == (int)BookingStatus.ReportSent,
                reportCount = reportData.Where(y => y.BookingId == x.BookingId && y.ReportId.HasValue).Select(y => y.ReportId).Count(),
                bookingStatusName = x.StatusName,
                statusColor = new SvgColor { fill = InspectionStatusColor.GetValueOrDefault(x.StatusId) }
            });

            return response;
        }

        public BookingProductData BookingProductsMap(MobileBookingProducts data)
        {
            var response = new BookingProductData();

            response.bookingProductsList = data.BookingProductsList;
            response.bookingStatusList = data.BookingStatusList;
            response.productUpdatedDate = data.ProductUpdatedDate;
            return response;
        }

        public MobileInspectionReportDetail InspDetailSummaryMap(ReportData paramList)
        {
            var response = new MobileInspectionReportDetail();
            var defectList = new List<MobileDefectData>();

            var reportIdList = paramList.productDataList.Where(x => x.ReportId.HasValue).Select(x => x.ReportId).Distinct().ToList();

            foreach (var reportId in reportIdList)
            {
                var productData = paramList.productDataList.Where(x => x.ReportId == reportId).ToList();
                var productImageList = new List<string>();
                productImageList.Add(productData.Select(x => x.ProductImage).FirstOrDefault());
                productImageList.AddRange(productData.Select(x => x.AdditionalPhotos).FirstOrDefault());

                var defectData = paramList.defects.Where(y => y.FbReportDetailId == reportId).ToList();
                var reportSummary = paramList.inspectionReportSummaries.Where(y => y.FbReportDetailId == reportId).ToList();
                var reportSubSummary = paramList.fbReportInspSubSummaries.Where(y => y.FBReportId == reportId).ToList();

                var productKey = 1;
                var defectKey = 1;
                var cusDecisionKey = 1;
                var fbResultKey = 1;
                var innerFbResultKey = 1;

                var distinctDefectNames = defectData.Select(x => x.Description).Distinct().ToList();

                var reportLink = productData.Select(x => x.FinalManualReportPath).FirstOrDefault() != null ?
                            productData.Select(x => x.FinalManualReportPath).FirstOrDefault() :
                            productData.Select(x => x.ReportPath).FirstOrDefault();

                foreach (var item in distinctDefectNames)
                {
                    var data = defectData.Where(x => x.Description == item);
                    if (data.Any(x => x.Minor > 0))
                    {
                        var list = new MobileDefectData()
                        {
                            key = defectKey++,
                            title = item,
                            count = data.Sum(x => x.Minor.GetValueOrDefault()),
                            type = (int)DefectType.Minor,
                            defectImagedata = paramList.defectImageData.Where(z => data.Select(x => x.Id).Contains(z.DefectId)).Select(p => new DefectImageData
                            {
                                defectImageDesc = p.Desc,
                                defectImageUrl = p.Image
                            }).ToList()
                        };
                        defectList.Add(list);
                    }
                    if (data.Any(x => x.Major > 0))
                    {
                        var list = new MobileDefectData()
                        {
                            key = defectKey++,
                            title = item,
                            count = data.Sum(x => x.Major.GetValueOrDefault()),
                            type = (int)DefectType.Major,
                            defectImagedata = paramList.defectImageData.Where(z => data.Select(x => x.Id).Contains(z.DefectId)).Select(p => new DefectImageData
                            {
                                defectImageDesc = p.Desc,
                                defectImageUrl = p.Image
                            }).ToList()
                        };
                        defectList.Add(list);
                    }
                    if (data.Any(x => x.Critical > 0))
                    {
                        var list = new MobileDefectData()
                        {
                            key = defectKey++,
                            title = item,
                            count = data.Sum(x => x.Critical.GetValueOrDefault()),
                            type = (int)DefectType.Critical,
                            defectImagedata = paramList.defectImageData.Where(z => data.Select(x => x.Id).Contains(z.DefectId)).Select(p => new DefectImageData
                            {
                                defectImageDesc = p.Desc,
                                defectImageUrl = p.Image
                            }).ToList()
                        };
                        defectList.Add(list);
                    }
                }

                response = new MobileInspectionReportDetail()
                {
                    inspectionId = paramList.bookingData?.BookingId,
                    reportNo = reportId.GetValueOrDefault(),
                    reportTitle = productData.Select(x => x.ReportNo).FirstOrDefault(),
                    supplierId = paramList.bookingData?.supplierId,
                    supplierName = paramList.bookingData?.SupplierName,
                    factoryId = paramList.bookingData?.factoryId,
                    factoryName = paramList.bookingData?.FactoryName,
                    inspectionResult = productData?.Select(x => x.ReportResult).FirstOrDefault(),
                    inspectionResultId = productData?.Select(x => x.ReportResultId.GetValueOrDefault()).FirstOrDefault(),
                    customerDecisionResult = paramList.cusDecision?.Where(x => x.ReportId == reportId).Select(x => x.CustomerDecisionStatus).FirstOrDefault(),
                    customerDecisionResultId = paramList.cusDecision?.Where(x => x.ReportId == reportId).Select(x => x.CustomerResultId).FirstOrDefault(),
                    serviceType = paramList.bookingData?.InspectionType,
                    serviceDate = productData?.Select(x => x.ServiceStartDate).FirstOrDefault() == productData?.Select(x => x.ServiceEndDate).FirstOrDefault() ? string.Format("{0: " + StandardDateFormat3 + "}", productData.Select(x => x.ServiceStartDate).FirstOrDefault()) : string.Format("{0: " + StandardDateFormat3 + "}", productData.Select(x => x.ServiceStartDate).FirstOrDefault()) + " to " + string.Format("{0: " + StandardDateFormat3 + "}", productData.Select(x => x.ServiceEndDate).FirstOrDefault()),
                    bookingStatus = productData?.Select(x => x.BookingStatusName).FirstOrDefault(),
                    poNumber = productData?.Select(x => x.PoNumber).FirstOrDefault(),
                    destinationCountry = productData?.Select(x => x.DestinationCountry).FirstOrDefault(),
                    productImageUrl = productImageList,
                    combinedProductsCount = productData?.Count() > 1 ? productData.Count : 0,
                    isCombined = productData?.Count() > 1,
                    reportLink = reportLink,
                    criticalCount = defectData?.Select(x => x.Critical).Sum(),
                    majorCount = defectData?.Select(x => x.Major).Sum(),
                    minorCount = defectData?.Select(x => x.Minor).Sum(),
                    combineProductList = productData?.Count() > 1 ? productData?.Select(x => new ProductData
                    {
                        key = productKey++,
                        productRef = x.ProductName,
                        productDesc = x.ProductDescription,
                        criticalDefectsCount = defectData.Where(z => z.ProductRefId == x.Id).Select(z => z.Critical.GetValueOrDefault()).Sum(),
                        majorDefectsCount = defectData.Where(z => z.ProductRefId == x.Id).Select(z => z.Major.GetValueOrDefault()).Sum(),
                        minorDefectsCount = defectData.Where(z => z.ProductRefId == x.Id).Select(z => z.Minor.GetValueOrDefault()).Sum(),
                        orderQty = x.BookingQuantity,
                        inspectedQty = x.InspectedQuantity,
                        presentedQty = x.PresentedQuantity,
                        destinationCountry = x.DestinationCountry,
                        poNumber = x.PoNumber,
                        defectList = CalculateDefectCount(defectData, reportId.GetValueOrDefault(), x.Id, paramList.defectImageData),
                        //defectData?.Where(y => y.FbReportDetailId == x.ReportId && y.ProductRefId == x.Id).Select(y => new MobileDefectData
                        //{
                        //    key = productDefectKey++,
                        //    title = y.Description,
                        //    count = //y.Minor > 0 ? y.Minor.GetValueOrDefault() : (y.Major > 0 ? y.Major.GetValueOrDefault() : y.Critical.GetValueOrDefault()),
                        //    y.Minor > 0 ? defectData.Where(z => z.Description == y.Description).Select(z => z.Minor.GetValueOrDefault()).FirstOrDefault() : (y.Major > 0 ? defectData.Where(z => z.Description == y.Description).Select(z => z.Major.GetValueOrDefault()).FirstOrDefault() : defectData.Where(z => z.Description == y.Description).Select(z => z.Critical.GetValueOrDefault()).FirstOrDefault()),
                        //    type = y.Minor > 0 ? (int)DefectType.Minor : y.Major > 0 ? (int)DefectType.Major : (int)DefectType.Critical
                        //}).ToList()
                    }).ToList() : null,
                    inspectionResultList = reportSummary?.Select(y => new FbResult
                    {
                        key = fbResultKey++,
                        resultId = y.ResultId,
                        fbResultName = y.Name,
                        result = y.Result,
                        imageCount = y.PhotoCount,
                        resultImage = y.Photos,
                        remarks = y.Remarks,
                        subResultList = reportSubSummary?.Where(z => z.Id == y.Id).Select(z => new FbResult
                        {
                            key = innerFbResultKey++,
                            resultId = z.Id,
                            fbResultName = z.Name,
                            result = z.Result,
                            remarks = z.Remarks
                        }).ToList()
                    }).ToList(),
                    defectList = defectList,

                    customerDecision = paramList.customerDecisionModes?.Select(y => new CusDecision
                    {
                        key = cusDecisionKey++,
                        title = y.Name,
                        id = y.Id,
                        svg = new SvgColor { fill = CustomerResultDashboardColor.GetValueOrDefault(y.Name) }
                    }).ToList()
                };
            }

            return response;
        }

        public List<MobileDefectData> CalculateDefectCount(List<InspectionReportDefects> defectData, int reportId, int productId, List<ReportDefectsImage> imageList)
        {
            int defectKey = 1;
            var defectList = new List<MobileDefectData>();

            var filteredData = defectData.Where(x => x.FbReportDetailId == reportId && x.ProductRefId == productId).ToList();

            var defectNames = filteredData.Select(x => x.Description).Distinct().ToList();

            foreach (var item in defectNames)
            {
                var data = filteredData.Where(x => x.Description == item);
                if (data.Any(x => x.Minor > 0))
                {
                    var list = new MobileDefectData()
                    {
                        key = defectKey++,
                        title = item,
                        count = data.Sum(x => x.Minor.GetValueOrDefault()),
                        type = (int)DefectType.Minor,
                        defectImagedata = imageList.Where(z => data.Select(x => x.Id).Contains(z.DefectId)).Select(p => new DefectImageData
                        {
                            defectImageDesc = p.Desc,
                            defectImageUrl = p.Image
                        }).ToList()
                    };
                    defectList.Add(list);
                }
                if (data.Any(x => x.Major > 0))
                {
                    var list = new MobileDefectData()
                    {
                        key = defectKey++,
                        title = item,
                        count = data.Sum(x => x.Major.GetValueOrDefault()),
                        type = (int)DefectType.Major,
                        defectImagedata = imageList.Where(z => data.Select(x => x.Id).Contains(z.DefectId)).Select(p => new DefectImageData
                        {
                            defectImageDesc = p.Desc,
                            defectImageUrl = p.Image
                        }).ToList()
                    };
                    defectList.Add(list);
                }
                if (data.Any(x => x.Critical > 0))
                {
                    var list = new MobileDefectData()
                    {
                        key = defectKey++,
                        title = item,
                        count = data.Sum(x => x.Critical.GetValueOrDefault()),
                        type = (int)DefectType.Critical,
                        defectImagedata = imageList.Where(z => data.Select(x => x.Id).Contains(z.DefectId)).Select(p => new DefectImageData
                        {
                            defectImageDesc = p.Desc,
                            defectImageUrl = p.Image
                        }).ToList()
                    };
                    defectList.Add(list);
                }
            }

            return defectList;
        }
    }
}
