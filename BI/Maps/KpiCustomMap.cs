using DTO.Claim;
using DTO.Common;
using DTO.CommonClass;
using DTO.Customer;
using DTO.Dashboard;
using DTO.DynamicFields;
using DTO.FinanceDashboard;
using DTO.FullBridge;
using DTO.Inspection;
using DTO.Invoice;
using DTO.InvoicePreview;
using DTO.Kpi;
using DTO.KPI;
using DTO.Quotation;
using DTO.RepoRequest.Audit;
using DTO.Report;
using DTO.Schedule;
using DTO.Supplier;
using Entities;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BI.Maps
{
    public class KpiCustomMap : ApiCommonData
    {
        public List<ExportTemplateItem> BookingquotationMapEci(List<KpiInspectionBookingItems> booking, List<KpiPoDetails> poDetails,
            List<KpiBookingProductsData> producList, List<InspectionBookingDFData> bookingDFList,
            KPIQuotDetails quotation, List<int> distinctBookingIds,
            List<BookingCustomerDepartment> customerDept, List<BookingShipment> shipmentQty, List<CustomerCustomStatus> customStatus,
            List<FactoryCountry> factoryLocation, IEnumerable<ServiceTypeList> serviceTypeList, List<QuTranStatusLog> quotStatusLogs,
            List<KPIMerchandiser> merchandiserList, List<BookingContainerItem> containerItems)
        {
            List<ExportTemplateItem> result = new List<ExportTemplateItem>();
            DateTime? confirmedDate = null;
            DateTime? reconfirmDate = null;
            DateTime date;

            foreach (var item in distinctBookingIds)
            {
                var filteredBooking = booking.Where(x => x.BookingId == item).ToList();
                var bookingData = filteredBooking.FirstOrDefault();
                var bookingPO = poDetails.Where(x => x.BookingId == item).ToList();
                var bookingProduct = producList.Where(x => x.BookingId == item).ToList();
                var quotationData = quotation?.QuotDetails.Where(x => x.Booking.Any(y => y.IdBooking == item)).FirstOrDefault();
                var quotMandayData = quotation?.MandayList?.Where(x => x.BookingId == item).FirstOrDefault();



                date = bookingData.ServiceDateTo;
                date = date.AddDays(1);
                if (date.DayOfWeek == DayOfWeek.Saturday)
                    date = date.AddDays(2);
                else if (date.DayOfWeek == DayOfWeek.Sunday)
                    date = date.AddDays(1);

                if (quotationData != null)
                {
                    //if (quotStatusLogs.Where(x => x.BookingId == item).Count() > 1)
                    //{
                    //    confirmedDate = quotStatusLogs.Where(x => x.BookingId == item).OrderByDescending(x => x.CreatedOn).Skip(1).Select(x => x.StatusChangeDate).FirstOrDefault();
                    //    reconfirmDate = quotStatusLogs.Where(x => x.BookingId == item).OrderByDescending(x => x.CreatedOn).Select(x => x.StatusChangeDate).FirstOrDefault();
                    //}
                    //else
                    //{
                    if (quotStatusLogs.Where(x => x.BookingId == item).OrderByDescending(x => x.CreatedOn).Count() > 0)
                        confirmedDate = quotStatusLogs.Where(x => x.BookingId == item).OrderByDescending(x => x.CreatedOn).Select(x => x.StatusChangeDate).FirstOrDefault();
                    //}
                }
                int servicetypeid = serviceTypeList?.Where(x => x.InspectionId == item).Select(x => x.serviceTypeId).FirstOrDefault() ?? 0;
                var res = new ExportTemplateItem
                {
                    Office = bookingData.Office,
                    BookingNo = item,
                    bookingStatus = customStatus != null && customStatus.Where(x => x.CustomerId == bookingData.CustomerId && x.StatusId == bookingData.StatusId).Select(x => x.CustomStatusName).Count() > 0 ? customStatus.Where(x => x.CustomerId == bookingData.CustomerId && x.StatusId == bookingData.StatusId).Select(x => x.CustomStatusName).FirstOrDefault() : bookingData.StatusName,
                    ServiceTypeName = serviceTypeList.Where(x => x.InspectionId == item).Select(x => x.serviceTypeName).FirstOrDefault(),
                    PONumber = string.Join(", ", bookingPO.Select(x => x.PoNumber).Distinct()), //add customerID and dept number
                    BuyerName = "01- EL CORTE INGLES",
                    DeptCode = string.Join(" ", customerDept.Where(x => x.BookingId == item).Select(x => x.Name)),
                    EciOffice = bookingDFList != null ? bookingDFList.Where(x => x.ControlConfigId == (int)DynamicFielsCuConfig.ECIOffice && x.BookingNo == item).Select(x => x.DFValue).FirstOrDefault() : "",
                    Bdm = bookingDFList != null ? bookingDFList.Where(x => x.ControlConfigId == (int)DynamicFielsCuConfig.BDM && x.BookingNo == item).Select(x => x.DFValue).FirstOrDefault() : "",
                    //Merchandise = bookingDFList != null ? bookingDFList.Where(x => x.ControlConfigId == (int)DynamicFielsCuConfig.Merchandiser && x.BookingNo == item).Select(x => x.DFValue).FirstOrDefault() : "",
                    Merchandise = merchandiserList.Any() ? string.Join(" ", merchandiserList.Where(x => x.BookingId == item).Select(x => x.Name)) : "",
                    QcmName = bookingDFList != null ? bookingDFList.Where(x => x.ControlConfigId == (int)DynamicFielsCuConfig.QCMName && x.BookingNo == item).Select(x => x.DFValue).FirstOrDefault() : "",
                    SupplierName = bookingData.SupplierName,
                    FactoryName = bookingData.FactoryName,
                    FactoryCity = factoryLocation.Where(x => x.BookingId == item).Select(x => x.CityName).FirstOrDefault(),
                    FactoryState = factoryLocation.Where(x => x.BookingId == item).Select(x => x.ProvinceName).FirstOrDefault(),
                    FactoryCountry = factoryLocation.Where(x => x.BookingId == item).Select(x => x.CountryName).FirstOrDefault(),
                    InspectionStartDate = bookingData.ServiceDateFrom.ToString(StandardDateFormat),
                    InspectionEndDate = bookingData.ServiceDateTo.ToString(StandardDateFormat),
                    ReportDate = date != null ? date.ToString(StandardDateFormat) : null,
                    ProductName = string.Join(" ", bookingProduct.Select(x => x.ProductName)),
                    ProductCategory = string.Join(" ", bookingProduct.Select(x => x.ProductCategory).Distinct()),
                    //ShipmentQty = string.Join(" ", shipmentQty.Where(x => x.BookingId == item).Select(x => x.ShipmentQty)),
                    ShipmentQty = string.Join(" ", bookingPO.Select(x => x.BookingQty)),
                    PartialShiptment = bookingDFList != null ? bookingDFList.Where(x => x.ControlConfigId == (int)DynamicFielsCuConfig.PartialShipment && x.BookingNo == item).Select(x => x.DFValue).FirstOrDefault() : "",
                    SampleSize = bookingProduct.Any(x => x.CombineProductId != 0) ? string.Join(" ", bookingProduct.Select(x => x.CombineAqlQuantity)).Replace(" 0", "") + " " + string.Join(" ", bookingProduct.Where(x => x.CombineProductId == 0).Select(x => x.AqlQty)) : string.Join(" ", bookingProduct.Select(x => x.AqlQty)),
                    IsCombined = bookingProduct.Any(x => x.CombineProductId != 0) ? true : false,
                    WMCode = bookingDFList != null ? bookingDFList.Where(x => x.ControlConfigId == (int)DynamicFielsCuConfig.WorkloadMartrixCode && x.BookingNo == item).Select(x => x.DFValue).FirstOrDefault() : "",
                    InspectionFeePerUnit = quotMandayData != null ? quotMandayData.UnitPrice : 0,
                    ManDay = quotMandayData != null ? (quotMandayData.Manday - quotMandayData.TravelManday) : 0,
                    WMDFee = quotMandayData != null ? quotMandayData.UnitPrice * (quotMandayData.Manday - quotMandayData.TravelManday) : 0,
                    InspectionFee = quotMandayData != null ? quotMandayData.InspFee : 0,
                    Quotationcomment = quotationData != null ? quotationData.QuotationAPIcomment : "",
                    TravellingCost = quotationData != null ? quotationData.TravelCostLand.GetValueOrDefault() + quotationData.HotelCost.GetValueOrDefault() + quotationData.OtherCost.GetValueOrDefault() : 0,
                    TotalInspectionFee = quotMandayData != null ? quotMandayData.TotalPrice : 0,
                    TotalReports = servicetypeid != (int)InspectionServiceTypeEnum.Container ? bookingProduct.Where(x => x.CombineProductId == 0).Count() + bookingProduct.Where(x => x.CombineProductId != 0).Select(x => x.CombineProductId).Distinct().Count() :
                                    containerItems?.Where(x => x.ReportId.HasValue).Select(x => x.ReportId).Distinct().Count() ?? 0,
                    PaidBy = quotationData != null ? quotationData.BillPaidBy == (int)QuotationPaidBy.customer ? "ECIG" : "Supplier" : "",
                    BookingRemarks = bookingData.BookingAPiRemarks,
                    ConfirmDate = confirmedDate?.ToString(StandardDateFormat) ?? null,
                    ReConfirmDate = reconfirmDate?.ToString(StandardDateFormat) ?? null,
                    IsPicking = bookingData.IsPicking,
                    FactoryRef = string.Join(" ", bookingProduct.Select(x => x.FactoryReference).Distinct()),
                    AirTicket = quotationData != null ? quotationData.TravelCostAir : 0,
                    NoOfTMD = quotMandayData != null ? quotMandayData.TravelManday : 0,
                    TMDFee = quotMandayData != null ? quotMandayData.UnitPrice * quotMandayData.TravelManday : 0,
                    TravelTime = quotMandayData != null ? quotMandayData.TravelTime : 0
                };

                result.Add(res);
            }
            return result;
        }

        public List<ExportTemplateItem> BookingquotationMapWarehouse(List<KpiInspectionBookingItems> booking, List<KpiPoDetails> poDetails,
          List<KpiBookingProductsData> productList, List<QuotationManday> quotation, List<BookingCustomerDepartment> customerDept,
          List<FactoryCountry> factoryLocation, List<CommonDataSource> qcNames, List<FbReportRemarks> reportRemarksList)
        {
            List<ExportTemplateItem> result = new List<ExportTemplateItem>();
            List<int> bookingIdList = new List<int>();

            var distinctReportList = productList.Where(x => x.ReportId.HasValue).Select(x => x.ReportId).Distinct().ToList();

            foreach (var item in distinctReportList)
            {
                if (item != null && item != 0)
                {
                    var products = productList?.Where(x => x.ReportId == item).ToList();
                    var bookingId = products?.Select(x => x.BookingId).FirstOrDefault();
                    var bookingPO = poDetails?.Where(x => x.BookingId == bookingId).ToList();

                    //logic to display quotation manday only on the first row if 1 booking has mustiple reports
                    double manday;
                    if (bookingIdList.Contains(bookingId.GetValueOrDefault()))
                    {
                        manday = 0;
                    }
                    else
                    {
                        bookingIdList.Add(bookingId.GetValueOrDefault());
                        manday = (double)quotation?.Where(x => x.BookingId == bookingId).Select(x => x.ManDay).FirstOrDefault();
                    }

                    var res = new ExportTemplateItem
                    {
                        BookingNo = bookingId.GetValueOrDefault(),
                        QcmName = string.Join(", ", qcNames.Where(x => x.Id == item).Select(x => x.Name).Distinct()),
                        ProductCategory = string.Join(", ", products.Select(x => x.ProductCategory).Distinct()),
                        CustomerName = booking?.Where(x => x.BookingId == bookingId).Select(x => x.CustomerName).FirstOrDefault(),
                        FactoryName = booking?.Where(x => x.BookingId == bookingId).Select(x => x.FactoryName).FirstOrDefault(),
                        ProductName = string.Join(", ", products.Select(x => x.ProductName).Distinct()),
                        ProductDescription = string.Join(", ", products.Select(x => x.ProductDescription).Distinct()),
                        ShipmentQty = products.Sum(x => x.BookingQuantity).ToString(),
                        ManDay = manday,
                        FactoryCountry = factoryLocation.Where(x => x.BookingId == bookingId).Select(x => x.CountryName).FirstOrDefault(),
                        FactoryCity = factoryLocation.Where(x => x.BookingId == bookingId).Select(x => x.CityName).FirstOrDefault(),
                        ReportResult = products.Select(x => x.ReportResult).FirstOrDefault(),
                        DeptCode = string.Join(", ", customerDept.Where(x => x.BookingId == bookingId).Select(x => x.Name)),
                        bookingStatus = booking?.Where(x => x.BookingId == bookingId).Select(x => x.StatusName).FirstOrDefault(),
                        ReportRemarks = string.Join("; ", reportRemarksList?.Where(x => x.ReportId == item && x.Remarks != null).Select(x => x.Remarks).Distinct()),
                        InspectionStartDate = booking?.Where(x => x.BookingId == bookingId).Select(x => x.ServiceDateFrom.ToString(StandardDateFormat)).FirstOrDefault(),
                        InspectionEndDate = booking?.Where(x => x.BookingId == bookingId).Select(x => x.ServiceDateTo.ToString(StandardDateFormat)).FirstOrDefault(),
                    };

                    result.Add(res);
                }
            }
            return result.OrderBy(x => x.BookingNo).ToList();
        }

        //GIFI - Customer KPI Template map booking, quotation, report details
        public List<ExportTemplateItem> GIFITemplateBookingQuotationMap(List<KpiInspectionBookingItems> booking, List<KpiPoDetails> poDetails,
            List<KpiBookingProductsData> productList, List<FactoryCountry> factoryLocation, List<ServiceTypeList> serviceTypeList,
            KPIQuotDetails quotation, List<QuTranStatusLog> quotStatusLogs, List<BookingCustomerBuyer> customerBuyerList, List<InspectionBookingDFData> bookingDFList,
            List<FBOtherInformation> fbOtherInformationList, List<FBSampleType> fbSampleTypeList, List<KpiInvoiceData> invoiceData)
        {
            List<ExportTemplateItem> result = new List<ExportTemplateItem>();
            int prevBookingId = 0;

            foreach (var item in productList)
            {
                if (item.ProductName != null)
                {
                    var bookingId = item.BookingId;
                    double? manday = 0;
                    double? travellingCost = 0;
                    double? totalInspectionFee = 0;
                    double? inspectionFee = 0;
                    var quotationInspData = quotation?.QuotDetails?.Where(x => x.Booking.Any(y => y.IdBooking == bookingId)).FirstOrDefault();
                    var bookingPO = poDetails?.Where(x => x.BookingId == bookingId).ToList();

                    var fbOtherInformation = fbOtherInformationList?.Where(x => x.FBReportId == item.ReportId && x.ProductId.HasValue && x.ProductId.Value == item.ProductId).ToList();
                    if (fbOtherInformation == null || !fbOtherInformation.Any())
                    {
                        fbOtherInformation = fbOtherInformationList?.Where(x => x.FBReportId == item.ReportId && !x.ProductId.HasValue).ToList();
                    }
                    var fbSampleType = fbSampleTypeList?.Where(x => x.FBReportId == item.ReportId && x.ProductId.HasValue && x.ProductId.Value == item.ProductId).ToList();
                    if (fbSampleType == null || !fbSampleType.Any())
                    {
                        fbSampleType = fbSampleTypeList?.Where(x => x.FBReportId == item.ReportId && !x.ProductId.HasValue).ToList();
                    }
                    var ColorTargetAvailable = fbSampleType?.Where(x => x.SampleType?.Trim().ToLower() == FBSampleTypeArtworkColorCard?.ToLower()).FirstOrDefault();
                    var ColorTargetnotAvailable = fbSampleType?.Where(x => x.SampleType?.Trim().ToLower() == FBSampleTypeNotProvided?.ToLower() && (x?.Description?.Split(' ')?.Any(y => FBDescriptionColorTargetCard.Contains(y.Trim().ToLower()))).HasValue &&
                                                                     (x?.Description?.Split(' ')?.Any(y => FBDescriptionColorTargetCard.Contains(y.Trim().ToLower()))).Value).FirstOrDefault();

                    var goldensampleavailable = fbSampleType?.Where(x => x.SampleType?.Trim().ToLower() == FBSampleTypeGoldenSample?.ToLower()).FirstOrDefault();
                    var goldsamplenotAvailable = fbSampleType?.Where(x => x.SampleType?.Trim().ToLower() == FBSampleTypeNotProvided?.ToLower() && (x?.Description?.Split(' ')?.Any(y => FBDescriptionGoldenSample.Contains(y.Trim().ToLower()))).HasValue &&
                                                                     (x?.Description?.Split(' ')?.Any(y => FBDescriptionGoldenSample.Contains(y.Trim().ToLower()))).Value).FirstOrDefault();


                    //logic to display quotation manday only on the first row if 1 booking has multiple reports
                    if (prevBookingId != bookingId)
                    {
                        manday = quotationInspData?.Booking?.FirstOrDefault().NoOfManDay;

                        travellingCost = quotationInspData?.Booking?.Select(x => (
                                           ((x.TravelAir > 0) ? x.TravelAir : 0) +
                                           ((x.TravelHotel > 0) ? x.TravelHotel : 0) +
                                           ((x.TravelLand > 0) ? x.TravelLand : 0))).FirstOrDefault();

                        totalInspectionFee = quotationInspData?.Booking?.Select(x => (x.TotalCost > 0) ? x.TotalCost : 0).FirstOrDefault();

                        inspectionFee = quotationInspData?.Booking?.Select(x => (x.InspFees > 0) ? x.InspFees : 0).FirstOrDefault();

                        prevBookingId = bookingId;
                    }

                    var res = new ExportTemplateItem
                    {

                        //booking details
                        BookingNo = bookingId,
                        bookingStatus = booking?.Where(x => x.BookingId == bookingId).Select(x => x.StatusName).FirstOrDefault(),
                        Office = booking?.Where(x => x.BookingId == bookingId).Select(x => x.Office).FirstOrDefault(),
                        InspectionStartDate = booking?.Where(x => x.BookingId == bookingId).Select(x => x.ServiceDateFrom.ToString(StandardDateFormat)).FirstOrDefault(),

                        FactoryName = booking?.Where(x => x.BookingId == bookingId).Select(x => x.FactoryName).FirstOrDefault(),
                        SupplierName = booking?.Where(x => x.BookingId == bookingId).Select(x => x.SupplierName).FirstOrDefault(),
                        ServiceTypeName = serviceTypeList?.Where(x => x.InspectionId == bookingId).Select(x => x.serviceTypeName).FirstOrDefault(),

                        ProductDescription = item.ProductDescription,
                        ProductName = item.ProductName,
                        PONumber = string.Join(",", poDetails?.Where(x => x.BookingId == bookingId).Select(x => x.PoNumber).Distinct()),
                        FactoryRef = item.FactoryReference,

                        FactoryCity = factoryLocation?.Where(x => x.BookingId == bookingId).Select(x => x.CityName).FirstOrDefault(),
                        FactoryState = factoryLocation?.Where(x => x.BookingId == bookingId).Select(x => x.ProvinceName).FirstOrDefault(),
                        FactoryCountry = factoryLocation?.Where(x => x.BookingId == bookingId).Select(x => x.CountryName).FirstOrDefault(),

                        BuyerName = string.Join(",", customerBuyerList?.Where(x => x.BookingId == bookingId).Select(x => x.BuyerName).FirstOrDefault()),
                        AQLLevelName = item.AQLName,
                        InspectionName = booking?.Where(x => x.BookingId == bookingId).FirstOrDefault() != null &&
                                             booking?.Where(x => x.BookingId == bookingId).FirstOrDefault().InspectionType > 0 ?
                                             "Re-Inspection" : "First Inspection",

                        //quotation
                        InvoiceNumber = invoiceData?.Where(x => x.BookingId == bookingId).Select(x => x.InvoiceNo).FirstOrDefault(),
                        InvoiceDate = invoiceData?.Where(x => x.BookingId == bookingId).Select(x => x.InvoiceDate).FirstOrDefault()?.ToString(StandardDateFormat) ?? "",

                        CurrencyName = quotationInspData?.CurrencyName,
                        PaidBy = quotationInspData?.BillPaidByName,
                        QuotationNumber = quotationInspData?.QuotationId,
                        ManDay = manday != null ? (int)manday : 0,
                        TravellingCost = travellingCost,
                        TotalInspectionFee = totalInspectionFee,
                        InspectionFee = inspectionFee,

                        QuotationDate = quotStatusLogs?.Where(x => x.BookingId == bookingId).Select(x => x.StatusChangeDate.ToString(StandardDateFormat)).FirstOrDefault(),

                        //report details
                        ReportDate = item.ServiceStartDate?.ToString(StandardDateFormat),
                        ReportResult = item.ReportResult,

                        //dynamic fields
                        GifiOfficeName = bookingDFList != null && bookingDFList.Any() ?
                                               bookingDFList?.Where(x => x.ControlConfigId == (int)DynamicFielsCuConfig.GifiOffice
                                                   && x.BookingNo == bookingId).Select(x => x.DFValue).FirstOrDefault() : "",
                        GifiQAContactName = bookingDFList != null && bookingDFList.Any() ?
                                               bookingDFList?.Where(x => x.ControlConfigId == (int)DynamicFielsCuConfig.GifiQAContact
                                                   && x.BookingNo == bookingId).Select(x => x.DFValue).FirstOrDefault() : "",


                        //fb sample type table get comments value
                        ColorTargetAvailable = ColorTargetAvailable != null ?
                                            ColorTargetAvailable?.Comments : ColorTargetnotAvailable != null ? ColorTargetnotAvailable?.Comments : "",


                        //fb other information table get remarks value
                        ColorCheckFinding = fbOtherInformation?.Where(x => x?.SubCategory?.ToLower() == FBSubCategoryArtworkColorCard?.ToLower()).Select(x => x.Remarks).FirstOrDefault() ?? "",


                        //fb sample type table get comments value
                        GoldenSampleAvailable = goldensampleavailable != null ? goldensampleavailable?.Comments : goldsamplenotAvailable != null ? goldsamplenotAvailable?.Comments : "",


                        //fb other information table get remarks value
                        GoldenSampleFinding = fbOtherInformation?.Where(x => x?.SubCategory?.ToLower() == FBSubCategoryGoldenSample?.ToLower()).Select(x => x.Remarks).FirstOrDefault() ?? "",
                    };
                    result.Add(res);
                }
            }
            return result.OrderBy(x => x.BookingNo).ToList();
        }

        //Map the details based on FB report Id
        public List<AdeoEanTemplate> BookingMapAdeoEanCode(List<KpiBookingProductsData> productList)
        {
            var result = new List<AdeoEanTemplate>();
            var distinctReportList = productList.Where(x => x.ReportId.HasValue).Select(x => x.ReportId).Distinct().ToList();

            foreach (var item in distinctReportList)
            {
                if (item > 0)
                {
                    var products = productList?.Where(x => x.ReportId == item).ToList();
                    var bookingId = products?.Select(x => x.BookingId).FirstOrDefault();

                    var res = new AdeoEanTemplate
                    {
                        BookingId = bookingId.GetValueOrDefault(),
                        ProductName = string.Join(", ", products?.Select(x => x.ProductName).Distinct()),
                        ProductDescription = string.Join(", ", products?.Select(x => x.ProductDescription).Distinct()),
                        Barcode = string.Join(", ", products?.Select(x => x.Barcode).Distinct())
                    };

                    result.Add(res);
                }
            }
            return result.OrderBy(x => x.BookingId).ToList();
        }

        //Map the details based on Product Id
        public List<ExportTemplateItem> BookingMapAdeo(KPIMapParameters parameterList)
        {

            List<ExportTemplateItem> result = new List<ExportTemplateItem>();
            var distinctBookingList = parameterList.BookingInvoice.BookingItems?.Select(x => x.BookingId).Distinct().ToList();

            foreach (var item in distinctBookingList)
            {
                if (item > 0)
                {
                    int sampleSize = 0;
                    List<int> bookingIdList = new List<int>();
                    var previousReportId = 0;
                    var combineId = 0;

                    var products = parameterList.ProductList?.Where(x => x.BookingId == item).OrderBy(x => x.CombineProductId).ThenByDescending(x => x.CombineAqlQuantity).ToList();
                    var bookingList = parameterList.BookingInvoice.BookingItems?.Where(x => x.BookingId == item).FirstOrDefault();
                    var quotMandayData = parameterList.QuotationDetails?.MandayList?.Where(x => x.BookingId == item).FirstOrDefault();
                    var quotationData = parameterList.QuotationDetails?.QuotDetails.Where(x => x.Booking.Any(y => y.IdBooking == item)).FirstOrDefault();

                    foreach (var product in products)
                    {
                        //logic to display the report remarks only in the first product if combined
                        string remarks;
                        if (previousReportId == product.ReportId || (combineId == product.CombineProductId && combineId != 0))
                        {
                            remarks = "";
                            sampleSize = 0;
                        }
                        else
                        {
                            remarks = parameterList.ReportRemarks != null ? string.Join("; ", parameterList.ReportRemarks?.Where(x => x.ReportId == product.ReportId && !string.IsNullOrEmpty(x.Remarks)).Select(x => x.Remarks).Distinct()) : "";
                            sampleSize = product.CombineProductId > 0 ? product.CombineAqlQuantity.GetValueOrDefault() : product.AqlQty;
                        }
                        previousReportId = product.ReportId.GetValueOrDefault();
                        combineId = product.CombineProductId.GetValueOrDefault();

                        //logic to display quotation manday only on the first row if 1 booking has mustiple reports
                        double manday = 0;
                        double inspFee = 0;
                        double otherCost = 0;
                        double totalCost = 0;
                        string invoiceNo = "";
                        string invoiceDate = "";
                        if (quotMandayData != null)
                        {
                            if (bookingIdList.Contains(item))
                            {
                                manday = 0;
                                inspFee = 0;
                                otherCost = 0;
                                totalCost = 0;
                                invoiceNo = "";
                                invoiceDate = "";
                            }
                            else
                            {
                                bookingIdList.Add(item);
                                manday = (double)quotMandayData?.Manday;
                                inspFee = quotMandayData?.InspFee ?? 0;
                                otherCost = quotationData.TravelCostAir.GetValueOrDefault() + quotationData.TravelCostLand.GetValueOrDefault() + quotationData.HotelCost.GetValueOrDefault() + quotationData.OtherCost.GetValueOrDefault();
                                totalCost = quotMandayData.TotalPrice;
                                invoiceNo = parameterList?.BookingInvoice?.InvoiceBookingData?.Where(x => x.BookingId == item).Select(x => x.InvoiceNo).FirstOrDefault();
                                invoiceDate = parameterList?.BookingInvoice?.InvoiceBookingData?.Where(x => x.BookingId == item).Select(x => x.InvoiceDate).FirstOrDefault();
                            }
                        }
                        else
                        {
                            //
                            if (bookingIdList.Contains(item))
                            {
                                invoiceNo = "";
                                invoiceDate = "";
                            }
                            else
                            {
                                invoiceNo = parameterList?.BookingInvoice?.InvoiceBookingData?.Where(x => x.BookingId == item).Select(x => x.InvoiceNo).FirstOrDefault();
                                invoiceDate = parameterList?.BookingInvoice?.InvoiceBookingData?.Where(x => x.BookingId == item).Select(x => x.InvoiceDate).FirstOrDefault();
                            }
                        }

                        var res = new ExportTemplateItem
                        {
                            BookingNo = item,
                            InspectionReportDate = product?.ServiceStartDate == product?.ServiceEndDate ? product?.ServiceStartDate?.ToString(StandardDateFormat) : product?.ServiceStartDate?.ToString(StandardDateFormat) + " - " + product?.ServiceEndDate?.ToString(StandardDateFormat),
                            SupplierName = bookingList.SupplierName,
                            PONumber = string.Join(", ", parameterList.PoDetails?.Where(x => x.BookingId == item && x.ProductRefId == product.Id).Select(x => x.PoNumber).Distinct()),
                            Etd = parameterList.PoDetails?.Where(x => x.BookingId == item && x.ProductRefId == product.Id).Select(x => x.Etd).FirstOrDefault()?.ToString(StandardDateFormat1) ?? "",
                            ReportStatus = product?.ReportResult,
                            FinalReportStatus = product?.ReportResultId == (int)FBReportResult.Pending ? "" : product.ReportResult,
                            Barcode = product?.Barcode,
                            DeptCode = string.Join(", ", parameterList.CustomerDept?.Where(x => x.BookingId == item).Select(x => x.Name)),
                            Month = parameterList.BookingInvoice.BookingItems != null && parameterList.BookingInvoice.BookingItems.Any() ? bookingList.ServiceDateTo.Month : 0,
                            QcmName = parameterList.QcNames != null ? string.Join(", ", parameterList.QcNames?.Where(x => x.Id == product.ReportId).Select(x => x.Name).Distinct()) : "",
                            SupplierCode = parameterList.SupplierCode != null ? parameterList.SupplierCode?.Where(x => x.SupplierId == bookingList.SupplierId).Select(x => x.Code).FirstOrDefault() : "",
                            ProductName = product?.ProductName,
                            ProductDescription = product?.ProductDescription,
                            FactoryName = bookingList?.FactoryName,
                            Office = bookingList?.Office,
                            ShipmentQty = product?.BookingQuantity.GetValueOrDefault().ToString(),
                            ServiceTypeName = parameterList.ServiceTypeList != null ? parameterList.ServiceTypeList?.Where(x => x.InspectionId == item).Select(x => x.serviceTypeName).FirstOrDefault() : "",
                            bookingStatus = bookingList?.StatusName,
                            ManDay = manday,
                            InspectionFee = inspFee,
                            TravellingCost = otherCost,
                            TotalInspectionFee = totalCost,
                            InvoiceNumber = invoiceNo,
                            MonthName = parameterList.BookingInvoice.BookingItems != null && parameterList.BookingInvoice.BookingItems.Any() ? MonthData.GetValueOrDefault(bookingList.ServiceDateTo.Month) : "",
                            ReinspectionId = parameterList.ReInspectionList != null ? parameterList.ReInspectionList?.Where(x => x.BookingId == item).Select(x => x.ReInspectionbookingId).FirstOrDefault() : null,
                            //ReportRemarks = string.IsNullOrEmpty(remarks) ? (parameterList.ReportRemarks != null  ? string.Join("; ", parameterList.ReportRemarks?.Where(x => x.ReportId == product.ReportId && x.ProductId.HasValue && x.ProductId == product.FbProductId).Select(x => x.Remarks)) : ""): (parameterList.ReportRemarks != null ? remarks + "; " + string.Join("; ", parameterList.ReportRemarks?.Where(x => x.ProductId.HasValue && x.ProductId == product.FbProductId).Select(x => x.Remarks)) : ""),
                            ReportRemarks = remarks,
                            CustomerContact = parameterList.CustomerContactData != null ? string.Join(", ", parameterList.CustomerContactData?.Where(x => x.BookingId == item).Select(x => x.StaffName).Distinct()) : "",
                            PaidBy = quotationData != null ? quotationData.BillPaidBy == (int)QuotationPaidBy.customer ? QuotationPaidBy.customer.ToString() : QuotationPaidBy.supplier.ToString() : "",
                            InspectionStartDate = product?.ServiceStartDate?.ToString(StandardDateFormat),
                            InspectionEndDate = product?.ServiceEndDate?.ToString(StandardDateFormat),
                            AQLLevelName = string.Join(", ", product.AQLName),
                            CombineId = product?.CombineProductId == 0 ? null : product?.CombineProductId,

                            Year = parameterList.BookingInvoice.BookingItems != null && parameterList.BookingInvoice.BookingItems.Any() ? bookingList.ServiceDateTo.Year : 0,
                            InvoiceDate = invoiceDate,

                            CustomerBookingNo = bookingList?.CustomerBookingNo ?? "",
                            CustomerName = bookingList?.CustomerName ?? "",
                            BuyerName = parameterList?.CustomerBuyerList != null ? string.Join(", ", parameterList?.CustomerBuyerList?.Where(x => x.BookingId == item).Select(x => x.BuyerName).Distinct()) : "",
                            ProductCategory = product?.ProductCategory ?? "",
                            ProductSubCategory = product?.ProductSubCategory ?? "",
                            ProductSubCategory2 = product?.ProductSubCategory2 ?? "",
                            ReportResult = product?.ReportResult ?? "",
                            FactoryState = parameterList?.FactoryLocation != null ? parameterList?.FactoryLocation?.Where(x => x.BookingId == item).Select(x => x.ProvinceName).FirstOrDefault() : "",
                            FactoryRef = product?.FactoryReference ?? "",
                            FactoryAddress = parameterList?.FactoryLocation != null ? parameterList?.FactoryLocation?.Where(x => x.BookingId == item).Select(x => x.FactoryAdress).FirstOrDefault() : "",
                            SampleSize = sampleSize.ToString(),
                            CombineProductQty = products.Where(x => product.CombineProductId > 0 && x.CombineProductId == product.CombineProductId).Count(),
                            PriceCategory = bookingList?.PriceCategory ?? "",
                            ProductUnitName = (product?.UnitCount == null ? "1" : product?.UnitCount.ToString()) + " " + product?.UnitName,
                            BrandName = parameterList?.CustomerBrandList != null ? string.Join(", ", parameterList?.CustomerBrandList?.Where(x => x.BookingId == item).Select(x => x.Name).Distinct()) : "",
                            BatteryType = parameterList?.ReportBatteryData != null ? parameterList?.ReportBatteryData.Where(x => product?.ReportId > 0 && x.ReportId == product?.ReportId && x.ProductId == product?.ProductId).Select(x => x.BatteryType).FirstOrDefault() : "",
                            BatteryModel = parameterList?.ReportBatteryData != null ? parameterList?.ReportBatteryData.Where(x => product?.ReportId > 0 && x.ReportId == product?.ReportId && x.ProductId == product?.ProductId).Select(x => x.BatteryModel).FirstOrDefault() : "",
                            BatteryQuantity = parameterList?.ReportBatteryData != null ? parameterList?.ReportBatteryData.Where(x => product?.ReportId > 0 && x.ReportId == product?.ReportId && x.ProductId == product?.ProductId).Select(x => x.Quantity).FirstOrDefault() : "",
                            BatteryNetWeight = parameterList?.ReportBatteryData != null ? parameterList?.ReportBatteryData.Where(x => product?.ReportId > 0 && x.ReportId == product?.ReportId && x.ProductId == product?.ProductId).Select(x => x.NetWeight).FirstOrDefault() : "",
                            PieceNo = parameterList?.ReportPackingData != null ? parameterList?.ReportPackingData.Where(x => product?.ReportId > 0 && x.ReportId == product?.ReportId && x.ProductId == product?.ProductId).Select(x => x.PieceNo).FirstOrDefault() : null,
                            MaterialGroup = parameterList?.ReportPackingData != null ? parameterList?.ReportPackingData.Where(x => product?.ReportId > 0 && x.ReportId == product?.ReportId && x.ProductId == product?.ProductId).Select(x => x.MaterialGroup).FirstOrDefault() : null,
                            MaterialCode = parameterList?.ReportPackingData != null ? parameterList?.ReportPackingData.Where(x => product?.ReportId > 0 && x.ReportId == product?.ReportId && x.ProductId == product?.ProductId).Select(x => x.MaterialCode).FirstOrDefault() : null,
                            PackingLocation = parameterList?.ReportPackingData != null ? parameterList?.ReportPackingData.Where(x => product?.ReportId > 0 && x.ReportId == product?.ReportId && x.ProductId == product?.ProductId).Select(x => x.Location).FirstOrDefault() : null,
                            PackingQuantity = parameterList?.ReportPackingData != null ? parameterList?.ReportPackingData.Where(x => product?.ReportId > 0 && x.ReportId == product?.ReportId && x.ProductId == product?.ProductId).Select(x => x.Quantity).FirstOrDefault() : null,
                            PackingNetWeight = parameterList?.ReportPackingData != null ? parameterList?.ReportPackingData.Where(x => product?.ReportId > 0 && x.ReportId == product?.ReportId && x.ProductId == product?.ProductId).Select(x => x.NetWeight).FirstOrDefault() : null,

                        };

                        result.Add(res);
                    }
                }
            }

            return result.OrderBy(x => x.BookingNo).ToList();
        }

        public List<AdeoInspSumOverallTemplate> MapAdeoInspSumOverall(KpiAdeoTemplateRequest request)
        {
            var result = new List<AdeoInspSumOverallTemplate>();
            int prevReportId = 0;
            var data = request.FactoryProductData.OrderBy(x => x.BookingId).ThenBy(x => x.ReportId).GroupBy(p => new { p.BookingId, p.ProductId }).ToList();

            foreach (var item in data)
            {
                var counter = 0;
                var remarks = "";
                var product = item.FirstOrDefault();
                var invoiceData = request.InvoiceData.Where(x => x.BookingId == product.BookingId).ToList();

                counter = prevReportId == product?.ReportId.GetValueOrDefault() ? counter++ : 0;

                if (counter == 0)
                {
                    remarks = request.RemarksData != null ? string.Join("; ", request.RemarksData?.Where(x => x.ReportId == product.ReportId && !string.IsNullOrEmpty(x.Remarks)).Select(x => x.Remarks).Distinct()) : "";
                }

                prevReportId = product?.ReportId.GetValueOrDefault() ?? 0;

                var res = new AdeoInspSumOverallTemplate
                {
                    CompanyName = Api,
                    PoNumber = string.Join(", ", item?.Select(y => y.PoNumber)),
                    ProductName = product?.ProductName,
                    ProductDescription = product?.ProductDescription,
                    DeptCode = string.Join(", ", request.DeptList.Where(x => x.BookingId == product.BookingId).Select(x => x.DepartmentName).Distinct().ToList()),
                    SupplierName = product?.SupplierName,
                    FactoryName = product?.FactoryName,
                    Office = product?.Office,
                    InspQty = product?.BookingQty ?? 0,
                    ServiceTypeName = request.ServiceTypeData.Where(x => x.InspectionId == product.BookingId).Select(x => x.serviceTypeName).FirstOrDefault(),
                    ServiceDate = product?.ServiceDateFrom == product?.ServiceDateTo ? product?.ServiceDateFrom.ToString(StandardDateFormat) : product?.ServiceDateFrom.ToString(StandardDateFormat) + " - " + product?.ServiceDateTo.ToString(StandardDateFormat),
                    MonthName = MonthData.GetValueOrDefault(product.ServiceDateTo.Month),
                    BookingStatus = product?.BookingStatus,
                    BookingId = product?.BookingId ?? 0,
                    ReportResult = product?.ReportStatus,
                    FailedRemark = remarks,
                    SupCode = request.SupCode.Where(x => x.SupplierId == product.SupplierId && x.CustomerId == product.CustomerId).Select(x => x.Code).FirstOrDefault(),
                    Manday = counter == 0 ? request.QuotationData.Where(x => x.BookingId == product.BookingId).Select(x => x.ManDay.GetValueOrDefault()).FirstOrDefault() : 0,
                    InspFees = counter == 0 ? invoiceData.Select(x => x.InspFees.GetValueOrDefault()).FirstOrDefault() : 0,
                    TravellingCost = counter == 0 ? invoiceData.Select(x => x.TravelFee.GetValueOrDefault()).FirstOrDefault() : 0,
                    TotalInspFee = counter == 0 ? invoiceData.Select(x => x.TotalFee.GetValueOrDefault()).FirstOrDefault() : 0,
                    InvoiceNo = invoiceData.Select(x => x.InvoiceNo).FirstOrDefault(),
                };
                result.Add(res);
            }
            return result;
        }

        //Map the details by Factory
        public List<ExportTemplateItem> BookingMapAdeoByFactory(KPIMapParameters parameterList)
        {

            List<ExportTemplateItem> result = new List<ExportTemplateItem>();

            var distinctFactoryList = parameterList.BookingInvoice.BookingItems.Select(x => x.FactoryId).Distinct().ToList();

            foreach (var item in distinctFactoryList)
            {
                if (item > 0)
                {
                    var bookingIdList = parameterList.BookingInvoice.BookingItems?.Where(x => x.FactoryId == item).Select(x => x.BookingId).ToList();
                    var bookings = parameterList.BookingInvoice.BookingItems?.Where(x => x.FactoryId == item).ToList();
                    var products = parameterList.ProductList?.Where(x => bookingIdList.Contains(x.BookingId)).ToList();
                    var merchandisers = parameterList.MerchandiserList.Where(x => bookingIdList.Contains(x.BookingId)).Select(x => x.Name).Distinct().ToList();

                    int servicetypeid = parameterList?.ServiceTypeList?.Where(x => bookingIdList.Contains(x.InspectionId)).Select(x => x.serviceTypeId).FirstOrDefault() ?? 0;

                    var res = new ExportTemplateItem
                    {
                        DeptCode = string.Join(", ", parameterList.CustomerDept?.Where(x => bookingIdList.Contains(x.BookingId)).Select(x => x.Name).Distinct()),
                        SupplierName = bookings?.Select(x => x.SupplierName).FirstOrDefault(),
                        SupplierCode = parameterList.SupplierCode?.Where(x => x.SupplierId == bookings.Select(y => y.SupplierId).FirstOrDefault()).Select(x => x.Code).FirstOrDefault(),
                        FactoryCode = parameterList.SupplierCode?.Where(x => x.SupplierId == item).Select(x => x.Code).FirstOrDefault(),
                        Merchandise = merchandisers != null && merchandisers.Any() ? merchandisers.FirstOrDefault() : "",
                        Merchandise2 = merchandisers != null && merchandisers.Any() ? string.Join(", ", merchandisers.Skip(1)) : "",
                        FactoryName = bookings?.Select(x => x.FactoryName).FirstOrDefault(),
                        TotalReports = servicetypeid != (int)InspectionServiceTypeEnum.Container ? products?.Where(x => x.CombineProductId == 0).Count() + products?.Where(x => x.CombineProductId != 0).Select(x => x.CombineProductId).Distinct().Count() ?? 0 :
                                parameterList?.ContainerItems?.Where(x => x.ReportId.HasValue).Select(x => x.ReportId).Distinct().Count() ?? 0,
                        TotalReportPass = servicetypeid != (int)InspectionServiceTypeEnum.Container ?
                                        (products.Any() ? products.Where(x => x.ReportResultId == (int)FBReportResult.Pass && x.CombineProductId != 0).Count() + products.Where(x => x.ReportResultId == (int)FBReportResult.Pass && x.CombineProductId == 0).Count() : 0) :
                                         parameterList?.ContainerItems?.Where(x => x.ReportResultId == (int)FBReportResult.Pass).Select(x => x.ReportId).Distinct().Count() ?? 0,
                        TotalReportFail = servicetypeid != (int)InspectionServiceTypeEnum.Container ?
                                        (products.Any() ? products.Where(x => x.ReportResultId == (int)FBReportResult.Fail && x.CombineProductId != 0).Count() + products.Where(x => x.ReportResultId == (int)FBReportResult.Fail && x.CombineProductId == 0).Count() : 0) :
                                         parameterList?.ContainerItems?.Where(x => x.ReportResultId == (int)FBReportResult.Fail).Select(x => x.ReportId).Distinct().Count() ?? 0,
                        TotalReportPending = servicetypeid != (int)InspectionServiceTypeEnum.Container ?
                                          (products.Any() ? products.Where(x => x.ReportResultId == (int)FBReportResult.Pending && x.CombineProductId != 0).Count() + products.Where(x => x.ReportResultId == (int)FBReportResult.Pending && x.CombineProductId == 0).Count() : 0) :
                                         parameterList?.ContainerItems?.Where(x => x.ReportResultId == (int)FBReportResult.Pending).Select(x => x.ReportId).Distinct().Count() ?? 0,
                        TotalReportMissing = servicetypeid != (int)InspectionServiceTypeEnum.Container ?
                                         (products.Any() ? products.Where(x => x.ReportResultId == (int)FBReportResult.Missing && x.CombineProductId != 0).Count() + products.Where(x => x.ReportResultId == (int)FBReportResult.Missing && x.CombineProductId == 0).Count() : 0) :
                                         parameterList?.ContainerItems?.Where(x => x.ReportResultId == (int)FBReportResult.Missing).Select(x => x.ReportId).Distinct().Count() ?? 0,
                        ReportPassPercentage = servicetypeid != (int)InspectionServiceTypeEnum.Container ?
                                        (products.Any() ? GetPercentage(products.Where(x => x.ReportResultId == (int)FBReportResult.Pass).Count(), products.Where(x => x.ReportId.HasValue).Count()) : 0) :
                                         parameterList.ContainerItems.Any() ? GetPercentage(parameterList.ContainerItems.Where(x => x.ReportResultId == (int)FBReportResult.Pass).Count(), parameterList.ContainerItems.Where(x => x.ReportId.HasValue).Count()) : 0,
                        Month = bookings.Any() ? bookings.Select(x => x.ServiceDateTo.Month).FirstOrDefault() : 0,
                        Year = bookings.Any() ? bookings.Select(x => x.ServiceDateTo.Year).FirstOrDefault() : 0
                    };

                    result.Add(res);
                }
            }
            return result.OrderBy(x => x.BookingNo).ToList();
        }

        //Map the details by Factory
        public List<AdeoMonthInspSumbySubconFactoTemplate> MapAdeoInspSumByFactory(KpiAdeoTemplateRequest parameterList)
        {
            List<AdeoMonthInspSumbySubconFactoTemplate> result = new List<AdeoMonthInspSumbySubconFactoTemplate>();

            var distinctFactoryList = parameterList.FactoryProductData.GroupBy(p => p.FactoryId).ToList();

            foreach (var item in distinctFactoryList)
            {
                var bookingIdList = item?.Select(x => x.BookingId).ToList();
                var merchandisers = parameterList.MerchandiserList.Where(x => bookingIdList.Contains(x.BookingId)).Select(x => x.Name).Distinct().ToList();
                var reportData = item?.Where(x => x.ReportId > 0).ToList();

                var res = new AdeoMonthInspSumbySubconFactoTemplate
                {
                    DeptCode = string.Join(", ", parameterList.DeptList?.Where(x => bookingIdList.Contains(x.BookingId)).Select(x => x.DepartmentName).Distinct().ToList()),
                    SupplierName = item?.Select(x => x.SupplierName).FirstOrDefault(),
                    SupplierCode = parameterList.SupCode?.Where(x => x.SupplierId == item.Select(y => y.SupplierId).FirstOrDefault()).Select(x => x.Code).FirstOrDefault(),
                    FactorySsmCode = parameterList.SupCode?.Where(x => x.SupplierId == item.Select(y => y.FactoryId).FirstOrDefault()).Select(x => x.Code).FirstOrDefault(),
                    Merchandise = merchandisers != null && merchandisers.Any() ? merchandisers.FirstOrDefault() : "",
                    Merchandise2 = merchandisers != null && merchandisers.Any() ? string.Join(", ", merchandisers.Skip(1)) : "",
                    FactoryName = item?.Select(x => x.FactoryName).FirstOrDefault(),
                    TotalReports = reportData.Select(x => x.ReportId).Distinct().Count(),
                    TotalReportPass = reportData.Where(x => x.ReportResultId == (int)FBReportResult.Pass).Select(x => x.ReportId).Distinct().Count(),
                    TotalReportFail = reportData.Where(x => x.ReportResultId == (int)FBReportResult.Fail).Select(x => x.ReportId).Distinct().Count(),
                    TotalReportPending = reportData.Where(x => x.ReportResultId == (int)FBReportResult.Pending).Select(x => x.ReportId).Distinct().Count(),
                    TotalReportMissing = reportData.Where(x => x.ReportResultId == (int)FBReportResult.Missing).Select(x => x.ReportId).Distinct().Count(),
                    ReportPassPercentage = GetPercentage(reportData.Where(x => x.ReportResultId == (int)FBReportResult.Pass).Select(x => x.ReportId).Distinct().Count(), reportData.Select(x => x.ReportId).Distinct().Count()).ToString() + " %",
                    Month = item?.Select(x => x.ServiceDateTo.Month).FirstOrDefault() ?? 0,
                    Year = item?.Select(x => x.ServiceDateTo.Year).FirstOrDefault() ?? 0,
                    CompanyName = Api
                };

                result.Add(res);
            }
            return result;
        }

        private double GetPercentage(int sum, int totalSum)
        {
            if (sum == 0 || totalSum == 0)
                return 0;

            var res = ((double)sum / (double)totalSum) * 100;
            return Math.Round(res, 2);
        }

        //Map the data for ADEO insp follow up
        public List<AdeoFollowUpTemplate> MapAdeoInspFollowUp(KpiAdeoTemplateRequest request)
        {
            List<AdeoFollowUpTemplate> result = new List<AdeoFollowUpTemplate>();
            var count = 0;
            var data = request.ProductData.GroupBy(p => new { p.BookingId, p.ProductName }).ToList();

            foreach (var item in data)
            {
                var product = item.FirstOrDefault();
                var bookingData = request.BookingData.FirstOrDefault(x => x.BookingId == product.BookingId);
                var poData = request.PoDetails.Where(x => x.BookingId == product.BookingId && x.ProductRefId == product.Id).ToList();
                var res = new AdeoFollowUpTemplate
                {
                    Id = ++count,
                    Etd = poData?.Select(x => x.Etd).FirstOrDefault(),
                    PoNumber = string.Join(", ", poData?.Select(x => x.PoNumber).Distinct()),
                    SupplierName = bookingData?.SupplierName,
                    SupplierCode = request?.SupCode != null ? request?.SupCode?.Where(x => x.SupplierId == bookingData.SupplierId && x.CustomerId == bookingData.CustomerId).Select(x => x.Code).FirstOrDefault() : "",
                    FactoryCountry = request.FactoryLocation != null ? request.FactoryLocation.Where(x => x.BookingId == product.BookingId).Select(x => x.CountryName).FirstOrDefault() : "",
                    BookingStatus = bookingData?.StatusId != (int)BookingStatus.Inspected ? KPICustomStatus.GetValueOrDefault(1) : BookingStatus.Inspected.ToString(),
                    InspectionEndDate = bookingData?.ServiceDateTo,
                    ReportDate = GetWorkingDate(bookingData.ServiceDateTo, bookingData.ServiceDateTo.AddDays(1), request.HolidayList, false),
                    ReportStatus = product?.ReportResultName,
                    ProductName = product?.ProductName,
                    Office = bookingData?.Office,
                    ConfirmDate = request.CusDecisionData?.Where(x => x.Id == product.ReportId).Select(x => x.Date).FirstOrDefault(), //final decision pending date in the excel 
                    ReConfirmDate = request.IcData?.Where(x => x.Id == product.ProductId).Select(x => x.Date).FirstOrDefault(), //Date of SR emitted
                    SupContactDate = GetWorkingDate(bookingData.ServiceDateFrom.AddDays(-7), bookingData.ServiceDateFrom, request.HolidayList, true),
                    SupContactDeadlineDate = GetWorkingDate(bookingData.ServiceDateFrom.AddDays(-14), bookingData.ServiceDateFrom, request.HolidayList, true),
                    InspCompanyName = Api
                };

                result.Add(res);
            }

            return result;
        }

        //Map the data for ADEO FRANCE Insp follow-up - KPI
        public List<AdeoFranceInspSummaryTemplate> MapAdeoFranceInspSummary(KpiAdeoTemplateRequest request)
        {
            List<AdeoFranceInspSummaryTemplate> result = new List<AdeoFranceInspSummaryTemplate>();
            var data = request.ProductData.GroupBy(p => new { p.BookingId, p.ProductName }).ToList();

            foreach (var item in data)
            {
                var product = item.FirstOrDefault();
                var bookingData = request.BookingData.FirstOrDefault(x => x.BookingId == product.BookingId);
                var poDetails = request.PoDetails.Where(x => x.BookingId == product.BookingId && x.ProductRefId == product.Id).ToList();
                var res = new AdeoFranceInspSummaryTemplate
                {
                    InspectionCompany = Api,
                    BookingId = product?.BookingId ?? 0,
                    ServiceDate = bookingData?.ServiceDateFrom == bookingData?.ServiceDateTo ? bookingData?.ServiceDateFrom.ToString(StandardDateFormat) : bookingData?.ServiceDateFrom.ToString(StandardDateFormat) + " - " + bookingData?.ServiceDateTo.ToString(StandardDateFormat),
                    SupplierName = bookingData?.SupplierName,
                    PoNumber = string.Join(", ", poDetails?.Select(x => x.PoNumber).Distinct()),
                    BarCode = product?.Barcode,
                    Etd = poDetails?.Select(x => x.Etd).FirstOrDefault(),
                    ReportStatus = product?.ReportResultName,
                    FinalReportStatus = product?.ReportResultId.GetValueOrDefault() == (int)FBReportResult.Pending ? "" : product?.ReportResultName,
                    DepartmentCode = string.Join(',', request.DeptList.Where(x => x.BookingId == product.BookingId).Select(y => y.DepartmentName).Distinct().ToArray()),

                };

                result.Add(res);
            }
            return result.OrderBy(x => x.BookingId).ToList();
        }

        //Map the data for ADEO FRANCE Insp follow-up - KPI
        public List<ExportTemplateItem> BookingSummaryMapAdeo(KPIMapParameters parameterList,
            List<CommonIdDate> cusDecisions, List<CommonIdDate> productIcData, List<HrHoliday> holidayList)
        {

            List<ExportTemplateItem> result = new List<ExportTemplateItem>();

            try
            {
                var distinctBookingList = parameterList.BookingInvoice.BookingItems?.Select(x => x.BookingId).Distinct().ToList();

                foreach (var item in distinctBookingList)
                {
                    if (item > 0)
                    {
                        var products = parameterList.ProductList?.Where(x => x.BookingId == item).ToList();
                        var bookingList = parameterList.BookingInvoice.BookingItems?.Where(x => x.BookingId == item).FirstOrDefault();

                        foreach (var product in products)
                        {
                            var res = new ExportTemplateItem
                            {
                                Etd = parameterList.PoDetails?.Where(x => x.BookingId == item && x.ProductRefId == product.Id).Select(x => x.Etd).FirstOrDefault()?.ToString(StandardDateFormat1) ?? "",
                                PONumber = string.Join(", ", parameterList.PoDetails?.Where(x => x.BookingId == item && x.ProductRefId == product.Id).Select(x => x.PoNumber)),
                                SupplierName = bookingList?.SupplierName,
                                SupplierCode = parameterList.SupplierCode != null ? parameterList.SupplierCode?.Where(x => x.SupplierId == bookingList.SupplierId).Select(x => x.Code).FirstOrDefault() : "",
                                FactoryCountry = parameterList.FactoryLocation != null ? parameterList.FactoryLocation.Where(x => x.BookingId == item).Select(x => x.CountryName).FirstOrDefault() : "",
                                bookingStatus = bookingList?.StatusId != (int)BookingStatus.Inspected ? KPICustomStatus.GetValueOrDefault(1) : BookingStatus.Inspected.ToString(),
                                InspectionEndDate = bookingList?.ServiceDateTo.ToString(StandardDateFormat2),
                                ReportDate = GetWorkingDate(bookingList.ServiceDateTo, bookingList.ServiceDateTo.AddDays(1), holidayList, false).ToString(StandardDateFormat2),
                                ReportStatus = product?.ReportResult,
                                ProductName = product?.ProductName,
                                Office = bookingList?.Office,
                                ConfirmDate = cusDecisions?.Where(x => x.Id == product.ReportId).Select(x => x.Date).FirstOrDefault()?.ToString(StandardDateFormat1) ?? "", //final decision pending date in the excel 
                                ReConfirmDate = productIcData?.Where(x => x.Id == product.ProductId).Select(x => x.Date).FirstOrDefault()?.ToString(StandardDateFormat1) ?? "", //Date of SR emitted
                                SupContactDate = GetWorkingDate(bookingList.ServiceDateFrom.AddDays(-7), bookingList.ServiceDateFrom, holidayList, true).ToString(StandardDateFormat2),
                                SupContactDeadlineDate = GetWorkingDate(bookingList.ServiceDateFrom.AddDays(-14), bookingList.ServiceDateFrom, holidayList, true).ToString(StandardDateFormat2)
                            };

                            result.Add(res);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var e = ex;
            }
            return result.OrderBy(x => x.BookingNo).ToList();
        }

        //Get the next or previous working date based on the condition
        private DateTime GetWorkingDate(DateTime fromDate, DateTime toDate, List<HrHolidayData> holidayList, bool isSupContactDate)
        {
            var date = new DateTime();

            if (fromDate != null && toDate != null)
            {
                var dateList = Enumerable.Range(0, 1 + toDate.Subtract(fromDate).Days)
                            .Select(offset => fromDate.AddDays(offset)).ToArray();

                //to get the next working date
                if (!isSupContactDate)
                {
                    date = toDate;
                    foreach (var item in dateList)
                    {
                        if (holidayList.Any(x => x.StartDate >= item && x.EndDate <= item))
                        {
                            date = date.AddDays(1);
                        }
                    }
                }

                //get the previous working date
                else
                {
                    date = fromDate;
                    foreach (var item in dateList.OrderBy(x => x.Date))
                    {
                        if (holidayList.Any(x => x.StartDate >= item && x.EndDate <= item))
                        {
                            date = date.AddDays(-1);
                        }
                    }

                }
            }

            return date;
        }

        //Get the next or previous working date based on the condition
        private DateTime GetWorkingDate(DateTime fromDate, DateTime toDate, List<HrHoliday> holidayList, bool isSupContactDate)
        {
            var date = new DateTime();

            if (fromDate != null && toDate != null)
            {
                var dateList = Enumerable.Range(0, 1 + toDate.Subtract(fromDate).Days)
                            .Select(offset => fromDate.AddDays(offset)).ToArray();

                //to get the next working date
                if (!isSupContactDate)
                {
                    date = toDate;
                    foreach (var item in dateList)
                    {
                        if (holidayList.Any(x => x.StartDate >= item && x.EndDate <= item))
                        {
                            date = date.AddDays(1);
                        }
                    }
                }

                //get the previous working date
                else
                {
                    date = fromDate;
                    foreach (var item in dateList.OrderBy(x => x.Date))
                    {
                        if (holidayList.Any(x => x.StartDate >= item && x.EndDate <= item))
                        {
                            date = date.AddDays(-1);
                        }
                    }

                }
            }

            return date;
        }

        //Map booking details based on Reports
        public List<ExportTemplateItem> BookingSummaryMapByReport(KPIMapParameters parameterList)
        {
            List<ExportTemplateItem> result = new List<ExportTemplateItem>();
            List<int> bookingIdList = new List<int>();

            var distinctReportList = parameterList.ProductList.Where(x => x.ReportId.HasValue).Select(x => x.ReportId).Distinct().ToList();

            foreach (var reportId in distinctReportList)
            {
                if (reportId > 0)
                {
                    var products = parameterList.ProductList?.Where(x => x.ReportId == reportId).ToList();
                    var bookingId = products?.Select(x => x.BookingId).FirstOrDefault();
                    var quotationData = parameterList.QuotationDetails?.QuotDetails.Where(x => x.Booking.Any(y => y.IdBooking == bookingId)).FirstOrDefault();
                    var quotMandayData = parameterList.QuotationDetails?.MandayList?.Where(x => x.BookingId == bookingId).FirstOrDefault();
                    var booking = parameterList?.BookingInvoice?.BookingItems?.Where(x => x.BookingId == bookingId).FirstOrDefault();
                    var customerDecision = parameterList?.CustomerDecision?.Where(x => x.ReportId == reportId).FirstOrDefault();
                    //logic to display quotation manday only on the first row if 1 booking has multiple reports
                    double manday = 0;
                    double inspFee = 0;
                    double travelCost = 0;
                    double totalCost = 0;
                    double hotel = 0;
                    if (quotMandayData != null)
                    {
                        if (bookingIdList.Contains(bookingId.GetValueOrDefault()))
                        {
                            manday = 0;
                            inspFee = 0;
                            travelCost = 0;
                            totalCost = 0;
                            hotel = 0;
                        }
                        else
                        {
                            bookingIdList.Add(bookingId.GetValueOrDefault());
                            manday = (double)quotMandayData?.Manday;
                            inspFee = quotMandayData?.InspFee ?? 0;
                            travelCost = quotationData.TravelCostAir.GetValueOrDefault() + quotationData.TravelCostLand.GetValueOrDefault() + quotationData.OtherCost.GetValueOrDefault();
                            totalCost = quotMandayData.TotalPrice;
                            hotel = quotMandayData.HotelCost;
                        }
                    }

                    var res = new ExportTemplateItem
                    {
                        InspectionStartDate = booking.ServiceDateFrom.ToString(StandardDateFormat),
                        InspectionEndDate = booking.ServiceDateTo.ToString(StandardDateFormat),
                        FactoryState = parameterList.FactoryLocation?.Where(x => x.BookingId == bookingId).Select(x => x.ProvinceName).FirstOrDefault(),
                        FactoryCity = parameterList.FactoryLocation?.Where(x => x.BookingId == bookingId).Select(x => x.CityName).FirstOrDefault(),
                        ServiceTypeName = parameterList.ServiceTypeList?.Where(x => x.InspectionId == bookingId).Select(x => x.serviceTypeName).FirstOrDefault(),
                        SupplierCode = parameterList.SupplierCode?.Where(x => x.SupplierId == parameterList.BookingInvoice.BookingItems.Where(y => y.BookingId == bookingId).Select(y => y.SupplierId).FirstOrDefault()).Select(x => x.Code).FirstOrDefault(),
                        SupplierName = booking.SupplierName,
                        ProductDescription = string.Join(", ", products?.Select(x => x.ProductDescription).Distinct()),
                        PONumber = string.Join(", ", parameterList.PoDetails?.Where(x => products.Select(y => y.Id).Contains(x.ProductRefId)).Select(x => x.PoNumber).Distinct()),
                        ProductCount = products.Count(),
                        ReportResult = products.Select(x => x.ReportResult).FirstOrDefault(),
                        BookingNo = bookingId.GetValueOrDefault(),
                        ManDay = manday,
                        TravellingCost = travelCost,
                        HotelFee = hotel,
                        InspectionFee = inspFee,
                        TotalInspectionFee = totalCost,
                        Merchandise = parameterList.MerchandiserList != null && parameterList.MerchandiserList.Any() ? string.Join(" ", parameterList.MerchandiserList.Where(x => x.BookingId == bookingId).Select(x => x.Name)) : "",
                        PaidBy = quotationData != null ? quotationData.BillPaidBy == (int)QuotationPaidBy.customer ? booking.CustomerName : QuotationPaidBy.supplier.ToString() : "",
                        QcCount = parameterList.QcNames?.Where(x => x.Id == reportId).Count(),
                        ProductCategory = string.Join(", ", products.Select(x => x.ProductCategory).Distinct()),
                        SampleSize = products.Any(x => x.CombineProductId > 0) ? products.Where(x => x.CombineAqlQuantity > 0).Select(x => x.CombineAqlQuantity.GetValueOrDefault()).FirstOrDefault().ToString() : products.Where(x => x.AqlQty > 0).Select(x => x.AqlQty).FirstOrDefault().ToString(),
                        AQLLevelName = string.Join(", ", products.Select(x => x.AQLName).Distinct()),
                        TotalQty = string.Join(", ", products.Select(x => x.BookingQuantity)),
                        CartonQty = parameterList.FbBookingQuantity != null ? string.Join(", ", parameterList.FbBookingQuantity?.Where(x => x.ReportId == reportId).Select(x => x.TotalCartons).Distinct()) : "",
                        SupContact = parameterList.ContactData != null ? string.Join(", ", parameterList.ContactData?.Where(y => y.BookingId == bookingId).Select(x => x.StaffName)) : "",
                        FactoryContact = parameterList.ContactData != null ? string.Join(", ", parameterList.ContactData?.Where(y => y.BookingId == bookingId).Select(x => x.StaffName)) : "",
                        FactoryName = booking.FactoryName,
                        Email = parameterList.ContactData != null ? string.Join(", ", parameterList.ContactData?.Where(y => y.BookingId == bookingId).Select(x => x.Email)) : "",
                        Phone = parameterList.ContactData != null ? string.Join(", ", parameterList.ContactData?.Where(y => y.BookingId == bookingId).Select(x => x.Phone)) : "",
                        InspectionName = booking?.InspectionType > 0 ?
                                       "RE-INSPECTION" : "INSPECTION",
                        InspectionReportDate = products?.FirstOrDefault()?.ServiceStartDate == products?.FirstOrDefault()?.ServiceEndDate ? products?.FirstOrDefault()?.ServiceStartDate?.ToString(StandardDateFormat)
                             : products?.FirstOrDefault()?.ServiceStartDate?.ToString(StandardDateFormat) + " - " + products?.FirstOrDefault()?.ServiceEndDate?.ToString(StandardDateFormat),
                        DeptCode = parameterList?.CustomerDept != null ? string.Join(",", parameterList?.CustomerDept?.Where(x => x.BookingId == bookingId).Select(x => x.Name).Distinct()) : "",
                        Month = booking != null && parameterList.BookingInvoice.BookingItems.Any() ? booking.ServiceDateTo.Month : 0,
                        ReinspectionId = parameterList.ReInspectionList != null ? parameterList.ReInspectionList?.Where(x => x.BookingId == bookingId).Select(x => x.ReInspectionbookingId).FirstOrDefault() : null,
                        CustomerContact = parameterList.CustomerContactData != null ? string.Join(", ", parameterList.CustomerContactData?.Where(x => x.BookingId == bookingId).Select(x => x.StaffName).Distinct()) : "",
                        ReportRemarks = parameterList.ReportRemarks != null ? string.Join("; ", parameterList.ReportRemarks?.Where(x => x.ReportId == reportId && !string.IsNullOrEmpty(x.Remarks)).Select(x => x.Remarks).Distinct()) : "",
                        ProductName = string.Join(", ", products.Select(x => x.ProductName).Distinct()),
                        CustomerDecision = customerDecision != null ? customerDecision.CustomerDecisionName : "",
                        CustomerDecisionComments = customerDecision != null ? customerDecision.CustomerDecisionComment : ""
                    };

                    result.Add(res);
                }
            }
            return result.OrderBy(x => x.BookingNo).ToList();
        }
        public ExportTemplateItem ECIRemarksTemplate(KPIMapParameters parameters, ECIRemarkParameters eciRemarkParameters)
        {
            var res = new ExportTemplateItem
            {
                FBRemarkNumber = eciRemarkParameters?.RemarkSerialNo,
                FBRemarkResult = eciRemarkParameters?.RemarkResult,
                ReportRemarks = eciRemarkParameters?.Remarks,

                //booking details
                BookingNo = (int)eciRemarkParameters?.BookingDetails?.BookingId,
                Office = eciRemarkParameters?.BookingDetails?.Office,
                InspectionStartDate = eciRemarkParameters?.BookingDetails?.ServiceDateFrom.ToString(StandardDateFormat),

                FactoryName = eciRemarkParameters?.BookingDetails?.FactoryName,
                SupplierName = eciRemarkParameters?.BookingDetails?.SupplierName,
                ServiceTypeName = parameters?.ServiceTypeList?.Where(x => x.InspectionId == eciRemarkParameters?.BookingDetails?.BookingId).Select(x => x.serviceTypeName).FirstOrDefault(),

                ProductName = eciRemarkParameters?.ProductItem.ProductName,

                FactoryCountry = parameters?.FactoryLocation?.Where(x => x.BookingId == eciRemarkParameters?.BookingDetails?.BookingId).Select(x => x.CountryName).FirstOrDefault(),

                BuyerName = string.Join(",", parameters?.CustomerBuyerList?.Where(x => x.BookingId == eciRemarkParameters?.BookingDetails?.BookingId).Select(x => x.BuyerName).FirstOrDefault()),

                //dynamic fields
                Bdm = parameters?.BookingDFDataList?.Where(x => x.ControlConfigId == (int)DynamicFielsCuConfig.BDM
                                           && x.BookingNo == eciRemarkParameters?.BookingDetails?.BookingId).Select(x => x.DFValue).FirstOrDefault(),

                ReportResult = eciRemarkParameters?.ProductItem?.ReportResult,
                TotalReports = (int)eciRemarkParameters?.TotalReports,

                CriticalMax = eciRemarkParameters?.ProductItem?.CriticalMax,
                MajorMax = eciRemarkParameters?.ProductItem?.MajorMax,
                MinorMax = eciRemarkParameters?.ProductItem?.MinorMax,

                CriticalDefect = parameters?.FBReportDefectsList?.Where(x => x.ProductId == eciRemarkParameters?.ProductItem?.ProductId &&
                                        x.FBReportDetailId == eciRemarkParameters?.ProductItem?.FBReportDetailId && x.Critical > 0).Sum(x => x.Critical),

                MajorDefect = parameters?.FBReportDefectsList?.Where(x => x.ProductId == eciRemarkParameters?.ProductItem?.ProductId &&
                                        x.FBReportDetailId == eciRemarkParameters?.ProductItem?.FBReportDetailId && x.Major > 0).Sum(x => x.Major),

                MinorDefect = parameters?.FBReportDefectsList?.Where(x => x.ProductId == eciRemarkParameters?.ProductItem?.ProductId &&
                                        x.FBReportDetailId == eciRemarkParameters?.ProductItem?.FBReportDetailId && x.Minor > 0).Sum(x => x.Minor),

                CriticalResult = parameters?.FBReportInspSubSummaryList?.Where(x => x.Name.ToLower() == FBInspSummarySubNameCritical.ToLower()
                                                && x.FBReportId == eciRemarkParameters?.ProductItem?.FBReportDetailId).
                                        Select(x => x.Result).FirstOrDefault(),

                MajorResult = parameters?.FBReportInspSubSummaryList?.Where(x => x.Name.ToLower() == FBInspSummarySubNameMajor.ToLower()
                                       && x.FBReportId == eciRemarkParameters?.ProductItem?.FBReportDetailId).
                                        Select(x => x.Result).FirstOrDefault(),

                MinorResult = parameters?.FBReportInspSubSummaryList?.Where(x => x.Name.ToLower() == FBInspSummarySubNameMinor.ToLower()
                                                       && x.FBReportId == eciRemarkParameters?.ProductItem?.FBReportDetailId).
                                        Select(x => x.Result).FirstOrDefault()

            };

            return res;
        }

        public ExportTemplateItem BookingMapByDefect(KPIMapParameters parameterList, DefectParameters defect)
        {
            var result = new ExportTemplateItem
            {
                BookingNo = defect.BookingDetails.BookingId,
                CustomerBookingNo = defect.BookingDetails.CustomerBookingNo,
                CustomerName = defect.BookingDetails.CustomerName,
                BuyerName = string.Join(",", parameterList?.CustomerBuyerList?.Where(x => x.BookingId == defect?.BookingDetails?.BookingId).Select(x => x.BuyerName).FirstOrDefault()),
                DeptCode = string.Join(",", parameterList?.CustomerDept?.Where(x => x.BookingId == defect?.BookingDetails?.BookingId).Select(x => x.Name).Distinct()),
                CustomerContact = string.Join(",", parameterList?.CustomerContactData?.Where(x => x.BookingId == defect?.BookingDetails?.BookingId).Select(x => x.StaffName).FirstOrDefault()),
                CollectionName = defect.BookingDetails.CollectionName,
                Office = defect?.BookingDetails?.Office,
                SupplierName = defect?.BookingDetails?.SupplierName,
                FactoryName = defect?.BookingDetails?.FactoryName,
                FactoryCountry = parameterList?.FactoryLocation?.Where(x => x.BookingId == defect?.BookingDetails?.BookingId).Select(x => x.CountryName).FirstOrDefault(),
                ServiceTypeName = parameterList?.ServiceTypeList?.Where(x => x.InspectionId == defect?.BookingDetails?.BookingId).Select(x => x.serviceTypeName).FirstOrDefault(),
                bookingStatus = defect?.BookingDetails?.StatusName,
                InspectionStartDate = defect?.BookingDetails?.ServiceDateFrom.ToString(StandardDateFormat1),
                InspectionEndDate = defect?.BookingDetails?.ServiceDateTo.ToString(StandardDateFormat1),
                Month = defect?.BookingDetails?.ServiceDateTo.Month ?? 0,
                Year = defect?.BookingDetails?.ServiceDateTo.Year ?? 0,
                PONumber = defect.DefectData.PoNumber,
                ProductName = defect?.ProductItem?.ProductName,
                ProductDescription = defect?.ProductItem?.ProductDescription,
                FactoryRef = defect?.ProductItem?.FactoryReference,
                ReportResult = defect?.ProductItem?.ReportResult,
                SerialNo = defect?.DefectSerialNo,
                DefectDesc = defect?.DefectData?.DefectDesc,
                CriticalDefect = defect?.DefectData.Critical,
                MajorDefect = defect?.DefectData?.Major,
                MinorDefect = defect?.DefectData.Minor,
                DefectCategory = defect?.DefectData?.DefectCategory,
                ProductCategory = defect?.ProductItem?.ProductCategory,
                ProductSubCategory = defect?.ProductItem?.ProductSubCategory,
                ProductSubCategory2 = defect?.ProductItem?.ProductSubCategory2
            };

            return result;

        }

        public ExportTemplateItem BookingMapByResult(KPIMapParameters parameterList, ResultParameters reportResult)
        {
            string CusDecisionDate = "";
            string CusDecisionName = "";
            int servicetypeid = parameterList?.ServiceTypeList?.Where(x => x.InspectionId == reportResult?.BookingDetails?.BookingId).Select(x => x.serviceTypeId).FirstOrDefault() ?? 0;
            string qcnames = servicetypeid != (int)InspectionServiceTypeEnum.Container ? parameterList.QcNames != null ? string.Join(", ", parameterList.QcNames?.Where(x => reportResult.ProductItem.Where(y => y.ReportId.HasValue).Select(y => y.ReportId.GetValueOrDefault()).Contains(x.Id)).Select(x => x.Name).Distinct()) : "" :
                                parameterList.QcNames != null ? string.Join(", ", parameterList.QcNames?.Where(x => reportResult.ContainerItems.Where(y => y.ReportId.HasValue).Select(y => y.ReportId.GetValueOrDefault()).Contains(x.Id)).Select(x => x.Name).Distinct()) : "";
            int reportcount = servicetypeid != (int)InspectionServiceTypeEnum.Container ? reportResult?.ProductItem?.Where(x => x.ReportId.HasValue).Select(x => x.ReportId).Distinct().Count() ?? 0 :
                                reportResult?.ContainerItems?.Where(x => x.ReportId.HasValue).Select(x => x.ReportId).Distinct().Count() ?? 0;
            string samplesize = servicetypeid != (int)InspectionServiceTypeEnum.Container ? (reportResult.ProductItem.Where(x => x.CombineProductId > 0 && x.CombineAqlQuantity > 0).Select(x => x.CombineAqlQuantity.Value)?.Sum() + reportResult.ProductItem.Where(x => x.CombineProductId <= 0 && x.CombineAqlQuantity <= 0).Select(x => x.AqlQty).Sum()).ToString() : "0";
            int TotalReportPass = servicetypeid != (int)InspectionServiceTypeEnum.Container ? reportResult?.ProductItem?.Where(x => x.ReportId.HasValue && x.ReportResultId == (int)FBReportResult.Pass).Select(x => x.ReportId).Distinct().Count() ?? 0 :
                                 reportResult?.ContainerItems?.Where(x => x.ReportId.HasValue && x.ReportResultId == (int)FBReportResult.Pass).Select(x => x.ReportId).Distinct().Count() ?? 0;
            int TotalReportFail = servicetypeid != (int)InspectionServiceTypeEnum.Container ? reportResult?.ProductItem?.Where(x => x.ReportResultId == (int)FBReportResult.Fail).Select(x => x.ReportId).Distinct().Count() ?? 0 :
                                reportResult?.ContainerItems?.Where(x => x.ReportResultId == (int)FBReportResult.Fail).Select(x => x.ReportId).Distinct().Count() ?? 0;
            int TotalReportMissing = servicetypeid != (int)InspectionServiceTypeEnum.Container ? reportResult?.ProductItem?.Where(x => x.ReportResultId == (int)FBReportResult.Missing).Select(x => x.ReportId).Distinct().Count() ?? 0 :
                                reportResult?.ContainerItems?.Where(x => x.ReportResultId == (int)FBReportResult.Missing).Select(x => x.ReportId).Distinct().Count() ?? 0;
            int TotalReportPending = servicetypeid != (int)InspectionServiceTypeEnum.Container ? reportResult?.ProductItem?.Where(x => x.ReportResultId == (int)FBReportResult.Pending).Select(x => x.ReportId).Distinct().Count() ?? 0 :
                                 reportResult?.ContainerItems?.Where(x => x.ReportResultId == (int)FBReportResult.Pending).Select(x => x.ReportId).Distinct().Count() ?? 0;

            if (servicetypeid != (int)InspectionServiceTypeEnum.Container && parameterList?.CustomerDecisionData != null)
            {
                int? reportid = reportResult?.ProductItem?.Where(x => x.ReportId.HasValue)?.Select(z => z.ReportId.GetValueOrDefault()).FirstOrDefault();
                CusDecisionDate = parameterList?.CustomerDecisionData?.Where(x => x.Id == reportid.GetValueOrDefault() && x.Date.HasValue)?.Select(y => y.Date).FirstOrDefault()?.ToString(StandardDateFormat) ?? "";
                CusDecisionName = parameterList?.CustomerDecisionData?.Where(x => x.Id == reportid.GetValueOrDefault()).Select(y => y.Name).FirstOrDefault() ?? "";
            }
            else if (servicetypeid == (int)InspectionServiceTypeEnum.Container && parameterList?.CustomerDecisionData != null)
            {
                int? containerreportid = reportResult?.ContainerItems?.Where(x => x.ReportId.HasValue)?.Select(z => z.ReportId.GetValueOrDefault()).FirstOrDefault();
                CusDecisionDate = parameterList?.CustomerDecisionData?.Where(x => x.Id == containerreportid.GetValueOrDefault() && x.Date.HasValue)?.Select(y => y.Date).FirstOrDefault()?.ToString(StandardDateFormat) ?? "";
                CusDecisionName = parameterList?.CustomerDecisionData?.Where(x => x.Id == containerreportid.GetValueOrDefault()).Select(y => y.Name).FirstOrDefault() ?? "";
            }

            int? invoicebilledto = parameterList?.BookingInvoice?.InvoiceBookingData?.Where(x => x.BookingId == reportResult?.BookingDetails?.BookingId && x.BilledTo.HasValue)?.Select(x => x.BilledTo.GetValueOrDefault()).FirstOrDefault();
            double invoiceextrafee = reportResult?.ExtraFeeDetails?.Where(x => x.BilledTo.HasValue && x.BilledTo == invoicebilledto.GetValueOrDefault())?.Sum(x => x.ExtraFee.GetValueOrDefault()) ?? 0;
            string buyername = string.Join(",", parameterList?.CustomerBuyerList?.Where(x => x.BookingId == reportResult?.BookingDetails?.BookingId).Select(x => x.BuyerName).FirstOrDefault());
            string departmetcode = string.Join(",", parameterList?.CustomerDept?.Where(x => x.BookingId == reportResult?.BookingDetails?.BookingId).Select(x => x.Name).FirstOrDefault());
            string customercontact = string.Join(",", parameterList?.CustomerContactData?.Where(x => x.BookingId == reportResult?.BookingDetails?.BookingId).Select(x => x.StaffName).FirstOrDefault());
            string factorycountry = parameterList?.FactoryLocation?.Where(x => x.BookingId == reportResult?.BookingDetails?.BookingId).Select(x => x.CountryName).FirstOrDefault();
            string factorystate = parameterList?.FactoryLocation?.Where(x => x.BookingId == reportResult?.BookingDetails?.BookingId).Select(x => x.ProvinceName).FirstOrDefault();
            string factorycity = parameterList?.FactoryLocation?.Where(x => x.BookingId == reportResult?.BookingDetails?.BookingId).Select(x => x.CityName).FirstOrDefault();
            string servicetype = parameterList?.ServiceTypeList?.Where(x => x.InspectionId == reportResult?.BookingDetails?.BookingId).Select(x => x.serviceTypeName).FirstOrDefault();
            string ponumber = string.Join(", ", parameterList.PoDetails?.Where(x => reportResult.ProductItem.Select(y => y.Id).Contains(x.ProductRefId)).Select(x => x.PoNumber).Distinct());
            string productname = string.Join(", ", reportResult?.ProductItem?.Select(x => x.ProductName).Distinct());
            string productdesc = string.Join(", ", reportResult?.ProductItem?.Select(x => x.ProductDescription).Distinct());
            string FactoryRef = string.Join(", ", reportResult?.ProductItem?.Select(x => x.FactoryReference).Distinct());
            string ReportResult = reportResult?.ProductItem?.Select(x => x.ReportResult).FirstOrDefault();
            string AQLLevelName = string.Join(", ", reportResult?.ProductItem?.Select(x => x.AQLName).Distinct());
            string InvoiceNumber = parameterList?.BookingInvoice?.InvoiceBookingData != null ? parameterList?.BookingInvoice?.InvoiceBookingData.Where(x => x.BookingId == reportResult?.BookingDetails?.BookingId).Select(x => x.InvoiceNo).FirstOrDefault() : "";
            string InvoiceDate = parameterList?.BookingInvoice?.InvoiceBookingData != null ? parameterList?.BookingInvoice?.InvoiceBookingData.Where(x => x.BookingId == reportResult?.BookingDetails?.BookingId).Select(x => x.InvoiceDate).FirstOrDefault() : "";
            string ProductCategory = string.Join(", ", reportResult?.ProductItem?.Select(x => x.ProductCategory).Distinct());
            string ProductSubCategory = string.Join(", ", reportResult?.ProductItem?.Select(x => x.ProductSubCategory).Distinct());
            string ProductSubCategory2 = string.Join(", ", reportResult?.ProductItem?.Select(x => x.ProductSubCategory2).Distinct());
            string FactoryAddress = parameterList?.FactoryLocation?.Where(x => x.BookingId == reportResult?.BookingDetails?.BookingId).Select(x => x.FactoryAdress).FirstOrDefault() ?? "";
            string PaidBy = reportResult?.QuotationDetails?.BillPaidBy == (int)QuotationPaidBy.customer ? QuotationPaidBy.customer.ToString() : QuotationPaidBy.supplier.ToString();
            double? TravellingCost = reportResult?.QuotationDetails?.HotelCost + reportResult?.QuotationDetails?.TravelCostLand + reportResult?.QuotationDetails?.TravelCostAir ?? 0;
            double? TotalInspectionFee = (reportResult?.QuotationMandayDetails?.TotalPrice ?? 0) + reportResult?.OtherCost + (reportResult?.ExtraFeeDetails?.Sum(x => x.ExtraFee) ?? 0) - (reportResult.Discount);
            double? Invoice_InspectionFee = parameterList?.BookingInvoice?.InvoiceBookingData != null ? parameterList?.BookingInvoice?.InvoiceBookingData.Where(x => x.BookingId == reportResult?.BookingDetails?.BookingId).Select(x => x.InspFees).FirstOrDefault() : 0;
            double? Invoice_TravelFee = parameterList?.BookingInvoice?.InvoiceBookingData != null ? parameterList?.BookingInvoice?.InvoiceBookingData.Where(x => x.BookingId == reportResult?.BookingDetails?.BookingId).Select(x => x.TravelFee).FirstOrDefault() : 0;
            double? Invoice_HotelFee = parameterList?.BookingInvoice?.InvoiceBookingData != null ? parameterList?.BookingInvoice?.InvoiceBookingData.Where(x => x.BookingId == reportResult?.BookingDetails?.BookingId).Select(x => x.HotelFee).FirstOrDefault() : 0;
            double? Invoice_OtherFee = parameterList?.BookingInvoice?.InvoiceBookingData != null ? parameterList?.BookingInvoice?.InvoiceBookingData.Where(x => x.BookingId == reportResult?.BookingDetails?.BookingId).Select(x => x.OtherExpense).FirstOrDefault() : 0;
            double? Invoice_TotalFee = parameterList?.BookingInvoice?.InvoiceBookingData != null ? parameterList?.BookingInvoice?.InvoiceBookingData.Where(x => x.BookingId == reportResult?.BookingDetails?.BookingId).Select(x => x.InvoiceTotal).FirstOrDefault() + invoiceextrafee : 0;
            double? Invoice_Discount = parameterList?.BookingInvoice?.InvoiceBookingData != null ? parameterList?.BookingInvoice?.InvoiceBookingData.Where(x => x.BookingId == reportResult?.BookingDetails?.BookingId).Select(x => x.Discount).FirstOrDefault() : 0;
            string Invoice_Currency = parameterList?.BookingInvoice?.InvoiceBookingData != null ? parameterList?.BookingInvoice?.InvoiceBookingData.Where(x => x.BookingId == reportResult?.BookingDetails?.BookingId).Select(x => x.InvoiceCurrency).FirstOrDefault() : "";
            string Merchandise = parameterList.MerchandiserList != null && parameterList.MerchandiserList.Any() ? string.Join(", ", parameterList.MerchandiserList.Where(x => x.BookingId == reportResult?.BookingDetails?.BookingId).Select(x => x.Name)) : "";
            double? ExtraFee = reportResult?.ExtraFeeDetails?.Sum(x => x.ExtraFee) ?? 0;
            var result = new ExportTemplateItem
            {
                BookingNo = reportResult?.BookingDetails?.BookingId ?? 0,
                CustomerBookingNo = reportResult?.BookingDetails?.CustomerBookingNo ?? "",
                CustomerName = reportResult?.BookingDetails?.CustomerName ?? "",
                BuyerName = buyername,
                DeptCode = departmetcode,
                CustomerContact = customercontact,
                CollectionName = reportResult.BookingDetails.CollectionName,
                Office = reportResult?.BookingDetails?.Office,
                SupplierName = reportResult?.BookingDetails?.SupplierName,
                FactoryName = reportResult?.BookingDetails?.FactoryName,
                FactoryCountry = factorycountry,
                FactoryState = factorystate,
                FactoryCity = factorycity,
                ServiceTypeName = servicetype,
                bookingStatus = reportResult?.BookingDetails?.StatusName,
                InspectionStartDate = reportResult?.BookingDetails?.ServiceDateFrom.ToString(StandardDateFormat1),
                InspectionEndDate = reportResult?.BookingDetails?.ServiceDateTo.ToString(StandardDateFormat1),
                Month = reportResult?.BookingDetails?.ServiceDateTo.Month ?? 0,
                Year = reportResult?.BookingDetails?.ServiceDateTo.Year ?? 0,
                ManDay = reportResult?.QuotationMandayDetails?.Manday ?? 0,
                PONumber = ponumber,
                ProductName = productname,
                ProductDescription = productdesc,
                FactoryRef = FactoryRef,
                ReportResult = ReportResult,
                AQLLevelName = AQLLevelName,
                FbResult = reportResult?.ReportResultData,
                InvoiceNumber = InvoiceNumber,
                InvoiceDate = InvoiceDate,
                ProductCategory = ProductCategory,
                ProductSubCategory = ProductSubCategory,
                ProductSubCategory2 = ProductSubCategory2,

                FactoryAddress = FactoryAddress,
                PaidBy = PaidBy,
                InspectionFee = reportResult?.QuotationMandayDetails?.InspFee ?? 0,
                TravellingCost = TravellingCost,
                TotalInspectionFee = TotalInspectionFee,
                Invoice_InspectionFee = Invoice_InspectionFee,
                Invoice_TravelFee = Invoice_TravelFee,
                Invoice_HotelFee = Invoice_HotelFee,
                Invoice_OtherFee = Invoice_OtherFee,
                Invoice_TotalFee = Invoice_TotalFee,
                Invoice_Discount = Invoice_Discount,
                Invoice_Currency = Invoice_Currency,
                Invoice_ExtraFee = invoiceextrafee,
                QcmName = qcnames,
                ProductCount = reportResult?.ProductItem?.Count(),
                TotalReports = reportcount,
                SampleSize = samplesize,
                TotalReportPass = TotalReportPass,
                TotalReportFail = TotalReportFail,
                TotalReportMissing = TotalReportMissing,
                TotalReportPending = TotalReportPending,
                Merchandise = Merchandise,
                CusDecisionDate = CusDecisionDate,
                CusDecisionName = CusDecisionName,
                OtherFee = reportResult?.OtherCost,
                ExtraFee = ExtraFee,
                Discount = reportResult.Discount
            };

            return result;

        }


        public ReportResultTemplateItem BookingReportResultMap(KPIReportResultMapParameters parameterList, ResultParameters reportResult, int? reportId)
        {
            DateTime? CusDecisionDate = null;
            string CusDecisionName = "";
            int servicetypeid = parameterList?.ServiceTypeList?.Where(x => x.InspectionId == reportResult?.BookingDetails?.BookingId).Select(x => x.serviceTypeId).FirstOrDefault() ?? 0;

            if (servicetypeid != (int)InspectionServiceTypeEnum.Container && parameterList?.CustomerDecisionData != null)
            {
                int? reportid = reportResult?.ProductItem?.Where(x => x.ReportId.HasValue)?.Select(z => z.ReportId.GetValueOrDefault()).FirstOrDefault();
                CusDecisionDate = parameterList?.CustomerDecisionData?.Where(x => x.Id == reportid.GetValueOrDefault() && x.Date.HasValue)?.Select(y => y.Date).FirstOrDefault();
                CusDecisionName = parameterList?.CustomerDecisionData?.Where(x => x.Id == reportid.GetValueOrDefault()).Select(y => y.Name).FirstOrDefault() ?? "";
            }
            else if (servicetypeid == (int)InspectionServiceTypeEnum.Container && parameterList?.CustomerDecisionData != null)
            {
                int? containerreportid = reportResult?.ContainerItems?.Where(x => x.ReportId.HasValue)?.Select(z => z.ReportId.GetValueOrDefault()).FirstOrDefault();
                CusDecisionDate = parameterList?.CustomerDecisionData?.Where(x => x.Id == containerreportid.GetValueOrDefault() && x.Date.HasValue)?.Select(y => y.Date).FirstOrDefault();
                CusDecisionName = parameterList?.CustomerDecisionData?.Where(x => x.Id == containerreportid.GetValueOrDefault()).Select(y => y.Name).FirstOrDefault() ?? "";
            }

            string buyername = string.Join(",", parameterList?.CustomerBuyerList?.Where(x => x.BookingId == reportResult?.BookingDetails?.BookingId).Select(x => x.BuyerName).FirstOrDefault());
            string departmetcode = string.Join(",", parameterList?.CustomerDept?.Where(x => x.BookingId == reportResult?.BookingDetails?.BookingId).Select(x => x.DepartmentName).FirstOrDefault());
            string customercontact = string.Join(",", parameterList?.CustomerContactData?.Where(x => x.BookingId == reportResult?.BookingDetails?.BookingId).Select(x => x.ContactName).FirstOrDefault());
            string factorycountry = parameterList?.FactoryLocation?.Where(x => x.BookingId == reportResult?.BookingDetails?.BookingId).Select(x => x.CountryName).FirstOrDefault();
            string factorystate = parameterList?.FactoryLocation?.Where(x => x.BookingId == reportResult?.BookingDetails?.BookingId).Select(x => x.ProvinceName).FirstOrDefault();
            string factorycity = parameterList?.FactoryLocation?.Where(x => x.BookingId == reportResult?.BookingDetails?.BookingId).Select(x => x.CityName).FirstOrDefault();
            string servicetype = parameterList?.ServiceTypeList?.Where(x => x.InspectionId == reportResult?.BookingDetails?.BookingId).Select(x => x.serviceTypeName).FirstOrDefault();
            string ponumber = string.Join(", ", parameterList?.PoDetails?.Where(x => reportResult.ProductItem.Select(y => y.Id).Contains(x.ProductRefId)).Select(x => x.PoNumber).Distinct());
            string productname = string.Join(", ", reportResult?.ProductItem?.Select(x => x.ProductName).Distinct());
            string productdesc = string.Join(", ", reportResult?.ProductItem?.Select(x => x.ProductDescription).Distinct());
            string FactoryRef = string.Join(", ", reportResult?.ProductItem?.Select(x => x.FactoryReference).Distinct());
            string ReportResult = reportResult?.ProductItem?.Select(x => x.ReportResult).FirstOrDefault();
            string AQLLevelName = string.Join(", ", reportResult?.ProductItem?.Select(x => x.AQLName).Distinct());
            string InvoiceNumber = parameterList?.InvoiceBookingData != null ? parameterList?.InvoiceBookingData.Where(x => x.BookingId == reportResult?.BookingDetails?.BookingId).Select(x => x.InvoiceNo).FirstOrDefault() : "";
            string InvoiceDate = parameterList?.InvoiceBookingData != null ? parameterList?.InvoiceBookingData.Where(x => x.BookingId == reportResult?.BookingDetails?.BookingId).Select(x => x.InvoiceDate).FirstOrDefault() : "";
            string ProductCategory = string.Join(", ", reportResult?.ProductItem?.Select(x => x.ProductCategory).Distinct());
            string ProductSubCategory = string.Join(", ", reportResult?.ProductItem?.Select(x => x.ProductSubCategory).Distinct());
            string ProductSubCategory2 = string.Join(", ", reportResult?.ProductItem?.Select(x => x.ProductSubCategory2).Distinct());

            var result = new ReportResultTemplateItem
            {
                BookingNo = reportResult?.BookingDetails?.BookingId ?? 0,
                CustomerBookingNo = reportResult?.BookingDetails?.CustomerBookingNo ?? "",
                CustomerName = reportResult?.BookingDetails?.CustomerName ?? "",
                BuyerName = buyername,
                DeptCode = departmetcode,
                CustomerContact = customercontact,
                CollectionName = reportResult.BookingDetails.CollectionName,
                Office = reportResult?.BookingDetails?.Office,
                SupplierName = reportResult?.BookingDetails?.SupplierName,
                FactoryName = reportResult?.BookingDetails?.FactoryName,
                FactoryCountry = factorycountry,
                FactoryState = factorystate,
                FactoryCity = factorycity,
                ServiceTypeName = servicetype,
                bookingStatus = reportResult?.BookingDetails?.StatusName,
                InspectionStartDate = reportResult?.BookingDetails?.ServiceDateFrom,
                InspectionEndDate = reportResult?.BookingDetails?.ServiceDateTo,
                Month = reportResult?.BookingDetails?.ServiceDateTo.Month ?? 0,
                Year = reportResult?.BookingDetails?.ServiceDateTo.Year ?? 0,
                ManDay = reportResult?.QuotationMandayDetails?.Manday ?? 0,
                PONumber = ponumber,
                ProductName = productname,
                ProductDescription = productdesc,
                FactoryRef = FactoryRef,
                ReportResult = ReportResult,
                AQLLevelName = AQLLevelName,
                InvoiceNumber = InvoiceNumber,
                InvoiceDate = InvoiceDate,
                ProductCategory = ProductCategory,
                ProductSubCategory = ProductSubCategory,
                ProductSubCategory2 = ProductSubCategory2,
                ReportId = reportId,
                CusDecisionDate = CusDecisionDate,
                CusDecisionName = CusDecisionName,
                PreviousBookingNo = reportResult?.BookingDetails?.PreviousBookingNo ?? null
            };

            return result;

        }

        public ExpenseTemplateItem BookingExpenseMapByResult(KPIExpenseMapParameters parameterList, KpiExpenseResultParameters reportResult)
        {

            string qcnames = "";

            int servicetypeid = parameterList?.ServiceTypeList?.Where(x => x.InspectionId == reportResult?.BookingDetails?.BookingId).Select(x => x.serviceTypeId).FirstOrDefault() ?? 0;

            if (servicetypeid != (int)InspectionServiceTypeEnum.Container && parameterList != null && parameterList.QcNames != null)
            {
                qcnames = string.Join(", ", parameterList.QcNames?.Where(x => reportResult.ProductItem.Where(y => y.ReportId.HasValue).Select(y => y.ReportId.GetValueOrDefault()).Contains(x.Id)).Select(x => x.Name).Distinct());
            }
            else if (servicetypeid == (int)InspectionServiceTypeEnum.Container && parameterList != null && parameterList.QcNames != null)
            {
                qcnames = string.Join(", ", parameterList.QcNames?.Where(x => reportResult.ContainerItems.Where(y => y.ReportId.HasValue).Select(y => y.ReportId.GetValueOrDefault()).Contains(x.Id)).Select(x => x.Name).Distinct());
            }

            int reportcount = servicetypeid != (int)InspectionServiceTypeEnum.Container ? reportResult?.ProductItem?.Where(x => x.ReportId.HasValue).Select(x => x.ReportId).Distinct().Count() ?? 0 :
                                reportResult?.ContainerItems?.Where(x => x.ReportId.HasValue).Select(x => x.ReportId).Distinct().Count() ?? 0;
            string samplesize = servicetypeid != (int)InspectionServiceTypeEnum.Container ? (reportResult.ProductItem.Where(x => x.CombineProductId > 0 && x.CombineAqlQuantity > 0).Select(x => x.CombineAqlQuantity.Value)?.Sum() + reportResult.ProductItem.Where(x => x.CombineProductId <= 0 && x.CombineAqlQuantity <= 0).Select(x => x.AqlQty).Sum()).ToString() : "0";
            int TotalReportPass = servicetypeid != (int)InspectionServiceTypeEnum.Container ? reportResult?.ProductItem?.Where(x => x.ReportId.HasValue && x.ReportResultId == (int)FBReportResult.Pass).Select(x => x.ReportId).Distinct().Count() ?? 0 :
                                 reportResult?.ContainerItems?.Where(x => x.ReportId.HasValue && x.ReportResultId == (int)FBReportResult.Pass).Select(x => x.ReportId).Distinct().Count() ?? 0;
            int TotalReportFail = servicetypeid != (int)InspectionServiceTypeEnum.Container ? reportResult?.ProductItem?.Where(x => x.ReportResultId == (int)FBReportResult.Fail).Select(x => x.ReportId).Distinct().Count() ?? 0 :
                                reportResult?.ContainerItems?.Where(x => x.ReportResultId == (int)FBReportResult.Fail).Select(x => x.ReportId).Distinct().Count() ?? 0;
            int TotalReportMissing = servicetypeid != (int)InspectionServiceTypeEnum.Container ? reportResult?.ProductItem?.Where(x => x.ReportResultId == (int)FBReportResult.Missing).Select(x => x.ReportId).Distinct().Count() ?? 0 :
                                reportResult?.ContainerItems?.Where(x => x.ReportResultId == (int)FBReportResult.Missing).Select(x => x.ReportId).Distinct().Count() ?? 0;
            int TotalReportPending = servicetypeid != (int)InspectionServiceTypeEnum.Container ? reportResult?.ProductItem?.Where(x => x.ReportResultId == (int)FBReportResult.Pending).Select(x => x.ReportId).Distinct().Count() ?? 0 :
                                 reportResult?.ContainerItems?.Where(x => x.ReportResultId == (int)FBReportResult.Pending).Select(x => x.ReportId).Distinct().Count() ?? 0;

            int? invoicebilledto = parameterList?.InvoiceBookingData?.
                Where(x => x.BookingId == reportResult?.BookingDetails?.BookingId && x.BilledTo.HasValue)?.
                Select(x => x.BilledTo.GetValueOrDefault()).FirstOrDefault();
            double invoiceextrafee = reportResult?.ExtraFeeDetails?.Where(x => x.BilledTo.HasValue && x.BilledTo == invoicebilledto.GetValueOrDefault())?.Sum(x => x.ExtraFee.GetValueOrDefault()) ?? 0;
            string buyername = string.Join(",", parameterList?.CustomerBuyerList?.Where(x => x.BookingId == reportResult?.BookingDetails?.BookingId).Select(x => x.BuyerName).FirstOrDefault());
            string departmetcode = string.Join(",", parameterList?.CustomerDept?.Where(x => x.BookingId == reportResult?.BookingDetails?.BookingId).Select(x => x.DepartmentName).FirstOrDefault());
            string customercontact = string.Join(",", parameterList?.CustomerContactData?.Where(x => x.BookingId == reportResult?.BookingDetails?.BookingId).Select(x => x.ContactName).FirstOrDefault());
            string factorycountry = parameterList?.FactoryLocation?.Where(x => x.BookingId == reportResult?.BookingDetails?.BookingId).Select(x => x.CountryName).FirstOrDefault();
            string factorystate = parameterList?.FactoryLocation?.Where(x => x.BookingId == reportResult?.BookingDetails?.BookingId).Select(x => x.ProvinceName).FirstOrDefault();
            string factorycity = parameterList?.FactoryLocation?.Where(x => x.BookingId == reportResult?.BookingDetails?.BookingId).Select(x => x.CityName).FirstOrDefault();
            string factorycounty = parameterList?.FactoryLocation?.Where(x => x.BookingId == reportResult?.BookingDetails?.BookingId).Select(x => x.CountyName).FirstOrDefault();
            string servicetype = parameterList?.ServiceTypeList?.Where(x => x.InspectionId == reportResult?.BookingDetails?.BookingId).Select(x => x.serviceTypeName).FirstOrDefault();
            string ponumber = string.Join(", ", parameterList.PoDetails?.Where(x => reportResult.ProductItem.Select(y => y.Id).Contains(x.ProductRefId)).Select(x => x.PoNumber).Distinct());
            string InvoiceNumber = parameterList?.InvoiceBookingData != null ? parameterList?.InvoiceBookingData.Where(x => x.BookingId == reportResult?.BookingDetails?.BookingId).Select(x => x.InvoiceNo).FirstOrDefault() : "";
            string InvoiceDate = parameterList?.InvoiceBookingData != null ? parameterList?.InvoiceBookingData.Where(x => x.BookingId == reportResult?.BookingDetails?.BookingId).Select(x => x.InvoiceDate).FirstOrDefault() : "";
            string ProductCategory = string.Join(", ", reportResult?.ProductItem?.Select(x => x.ProductCategory).Distinct());
            string ProductSubCategory = string.Join(", ", reportResult?.ProductItem?.Select(x => x.ProductSubCategory).Distinct());
            string ProductSubCategory2 = string.Join(", ", reportResult?.ProductItem?.Select(x => x.ProductSubCategory2).Distinct());
            string FactoryAddress = parameterList?.FactoryLocation?.Where(x => x.BookingId == reportResult?.BookingDetails?.BookingId).Select(x => x.FactoryAdress).FirstOrDefault() ?? "";
            string PaidBy = reportResult?.QuotationDetails?.BillPaidBy == (int)QuotationPaidBy.customer ? QuotationPaidBy.customer.ToString() : QuotationPaidBy.supplier.ToString();
            double? TravellingCost = reportResult?.QuotationDetails?.HotelCost + reportResult?.QuotationDetails?.TravelCostLand + reportResult?.QuotationDetails?.TravelCostAir ?? 0;
            double? TotalInspectionFee = (reportResult?.QuotationMandayDetails?.TotalPrice ?? 0) + reportResult?.OtherCost + (reportResult?.ExtraFeeDetails?.Sum(x => x.ExtraFee) ?? 0) - (reportResult.Discount);
            double? Invoice_InspectionFee = parameterList?.InvoiceBookingData != null ? parameterList?.InvoiceBookingData.Where(x => x.BookingId == reportResult?.BookingDetails?.BookingId).Select(x => x.InspFees).FirstOrDefault() : 0;
            double? Invoice_TravelFee = parameterList?.InvoiceBookingData != null ? parameterList?.InvoiceBookingData.Where(x => x.BookingId == reportResult?.BookingDetails?.BookingId).Select(x => x.TravelFee).FirstOrDefault() : 0;
            double? Invoice_HotelFee = parameterList?.InvoiceBookingData != null ? parameterList?.InvoiceBookingData.Where(x => x.BookingId == reportResult?.BookingDetails?.BookingId).Select(x => x.HotelFee).FirstOrDefault() : 0;
            double? Invoice_OtherFee = parameterList?.InvoiceBookingData != null ? parameterList?.InvoiceBookingData.Where(x => x.BookingId == reportResult?.BookingDetails?.BookingId).Select(x => x.OtherExpense).FirstOrDefault() : 0;
            double? Invoice_TotalFee = parameterList?.InvoiceBookingData != null ? parameterList?.InvoiceBookingData.Where(x => x.BookingId == reportResult?.BookingDetails?.BookingId).Select(x => x.InvoiceTotal).FirstOrDefault() + invoiceextrafee : 0;
            double? Invoice_Discount = parameterList?.InvoiceBookingData != null ? parameterList?.InvoiceBookingData.Where(x => x.BookingId == reportResult?.BookingDetails?.BookingId).Select(x => x.Discount).FirstOrDefault() : 0;
            string Invoice_Currency = parameterList?.InvoiceBookingData != null ? parameterList?.InvoiceBookingData.Where(x => x.BookingId == reportResult?.BookingDetails?.BookingId).Select(x => x.InvoiceCurrency).FirstOrDefault() : "";
            double? ExtraFee = reportResult?.ExtraFeeDetails?.Sum(x => x.ExtraFee) ?? 0;
            var result = new ExpenseTemplateItem
            {
                BookingNo = reportResult?.BookingDetails?.BookingId ?? 0,
                CustomerBookingNo = reportResult?.BookingDetails?.CustomerBookingNo ?? "",
                CustomerName = reportResult?.BookingDetails?.CustomerName ?? "",
                BuyerName = buyername,
                DeptCode = departmetcode,
                CustomerContact = customercontact,
                CollectionName = reportResult.BookingDetails.CollectionName,
                Office = reportResult?.BookingDetails?.Office,
                SupplierName = reportResult?.BookingDetails?.SupplierName,
                FactoryName = reportResult?.BookingDetails?.FactoryName,
                FactoryCountry = factorycountry,
                FactoryState = factorystate,
                FactoryCity = factorycity,
                FactoryCounty = factorycounty,
                ServiceTypeName = servicetype,
                bookingStatus = reportResult?.BookingDetails?.StatusName,
                InspectionStartDate = reportResult?.BookingDetails?.ServiceDateFrom,
                InspectionEndDate = reportResult?.BookingDetails?.ServiceDateTo,
                Month = reportResult?.BookingDetails?.ServiceDateTo.Month ?? 0,
                Year = reportResult?.BookingDetails?.ServiceDateTo.Year ?? 0,
                ManDay = reportResult?.QuotationMandayDetails?.Manday ?? 0,
                PONumber = ponumber,
                InvoiceNumber = InvoiceNumber,
                InvoiceDate = InvoiceDate,
                ProductCategory = ProductCategory,
                ProductSubCategory = ProductSubCategory,
                ProductSubCategory2 = ProductSubCategory2,
                FactoryAddress = FactoryAddress,
                PaidBy = PaidBy,
                InspectionFee = reportResult?.QuotationMandayDetails?.InspFee ?? 0,
                TravellingCost = TravellingCost,
                TotalInspectionFee = TotalInspectionFee,
                Invoice_InspectionFee = Invoice_InspectionFee,
                Invoice_TravelFee = Invoice_TravelFee,
                Invoice_HotelFee = Invoice_HotelFee,
                Invoice_OtherFee = Invoice_OtherFee,
                Invoice_TotalFee = Invoice_TotalFee,
                Invoice_Discount = Invoice_Discount,
                Invoice_Currency = Invoice_Currency,
                Invoice_ExtraFee = invoiceextrafee,
                QcmName = qcnames,
                ProductCount = reportResult?.ProductItem?.Count(),
                TotalReports = reportcount,
                SampleSize = samplesize,
                TotalReportPass = TotalReportPass,
                TotalReportFail = TotalReportFail,
                TotalReportMissing = TotalReportMissing,
                TotalReportPending = TotalReportPending,
                OtherFee = reportResult?.OtherCost,
                ExtraFee = ExtraFee,
                Discount = reportResult.Discount
            };

            return result;

        }

        public ExportTemplateItem BookingMapByRemark(KPIMapParameters parameters, ECIRemarkParameters eciRemarkParameters)
        {
            var res = new ExportTemplateItem
            {
                FBRemarkNumber = eciRemarkParameters?.RemarkSerialNo,
                FBRemarkResult = eciRemarkParameters?.RemarkResult,
                ReportRemarks = eciRemarkParameters?.Remarks,

                //booking details
                BookingNo = (int)eciRemarkParameters?.BookingDetails?.BookingId,
                Office = eciRemarkParameters?.BookingDetails?.Office,
                InspectionStartDate = eciRemarkParameters?.BookingDetails?.ServiceDateFrom.ToString(StandardDateFormat),

                FactoryName = eciRemarkParameters?.BookingDetails?.FactoryName,
                SupplierName = eciRemarkParameters?.BookingDetails?.SupplierName,
                ServiceTypeName = parameters?.ServiceTypeList?.Where(x => x.InspectionId == eciRemarkParameters?.BookingDetails?.BookingId).Select(x => x.serviceTypeName).FirstOrDefault(),

                ProductName = string.Join(", ", eciRemarkParameters?.ProductList?.Select(x => x.ProductName).Distinct()),
                FactoryCountry = parameters?.FactoryLocation?.Where(x => x.BookingId == eciRemarkParameters?.BookingDetails?.BookingId).Select(x => x.CountryName).FirstOrDefault(),

                BuyerName = string.Join(",", parameters?.CustomerBuyerList?.Where(x => x.BookingId == eciRemarkParameters?.BookingDetails?.BookingId).Select(x => x.BuyerName).FirstOrDefault()),

                ReportResult = eciRemarkParameters?.ProductList?.Select(x => x.ReportResult).FirstOrDefault(),
                CustomerBookingNo = eciRemarkParameters?.BookingDetails?.CustomerBookingNo ?? "",
                CustomerName = eciRemarkParameters?.BookingDetails?.CustomerName ?? "",
                DeptCode = string.Join(",", parameters?.CustomerDept?.Where(x => x.BookingId == eciRemarkParameters?.BookingDetails?.BookingId).Select(x => x.Name).Distinct()),
                CustomerContact = string.Join(",", parameters?.CustomerContactData?.Where(x => x.BookingId == eciRemarkParameters?.BookingDetails?.BookingId).Select(x => x.StaffName).Distinct()),
                CollectionName = eciRemarkParameters?.BookingDetails?.CollectionName ?? "",
                bookingStatus = eciRemarkParameters?.BookingDetails?.StatusName ?? "",
                InspectionEndDate = eciRemarkParameters?.BookingDetails?.ServiceDateTo.ToString(StandardDateFormat),
                Month = eciRemarkParameters?.BookingDetails?.ServiceDateTo.Month ?? 0,
                Year = eciRemarkParameters?.BookingDetails?.ServiceDateTo.Year ?? 0,
                PaidBy = eciRemarkParameters?.QuotationDetails?.BillPaidBy == (int)QuotationPaidBy.customer ? QuotationPaidBy.customer.ToString() : QuotationPaidBy.supplier.ToString(),
                PONumber = string.Join(", ", parameters.PoDetails?.Where(x => eciRemarkParameters.ProductList.Select(y => y.Id).Contains(x.ProductRefId)).Select(x => x.PoNumber).Distinct()),
                ProductDescription = string.Join(", ", eciRemarkParameters?.ProductList?.Select(x => x.ProductDescription).Distinct()) ?? "",
                FactoryRef = string.Join(", ", eciRemarkParameters?.ProductList?.Select(x => x.FactoryReference).Distinct()),
                ProductCategory = string.Join(", ", eciRemarkParameters?.ProductList?.Select(x => x.ProductCategory).Distinct()),
                ProductSubCategory = string.Join(", ", eciRemarkParameters?.ProductList?.Select(x => x.ProductSubCategory).Distinct()),
                ProductSubCategory2 = string.Join(", ", eciRemarkParameters?.ProductList?.Select(x => x.ProductSubCategory2).Distinct()),
                RemarkCategory = parameters?.FBReportInspSubSummaryList?.Where(x => x.FBReportId == eciRemarkParameters?.ProductList?.Select(y => y.ReportId).FirstOrDefault() && x.Id == eciRemarkParameters?.InspSummaryId).Select(x => x.Name).FirstOrDefault(),
                CombineId = eciRemarkParameters?.ProductList?.Select(x => x.CombineProductId).FirstOrDefault() == 0 ? null : eciRemarkParameters?.ProductList?.Select(x => x.CombineProductId).FirstOrDefault(),
                RemarkSubCategory = eciRemarkParameters?.RemarkSubCategory,
                RemarkSubCategory2 = eciRemarkParameters?.RemarkSubCategory2,
                CustomerRemarkCodeReference = eciRemarkParameters?.CustomerRemarkCode
            };

            return res;
        }

        //Map the details based on Product Id
        public List<ExportTemplateItem> BookingMapByProduct(KPIMapParameters parameterList)
        {
            List<ExportTemplateItem> result = new List<ExportTemplateItem>();
            var distinctBookingList = parameterList.BookingInvoice.BookingItems?.Select(x => x.BookingId).Distinct().ToList();

            foreach (var bookingId in distinctBookingList)
            {
                if (bookingId > 0)
                {
                    List<int> bookingIdList = new List<int>();
                    var previousReportId = 0;
                    var combineId = 0;

                    var products = parameterList.ProductList?.Where(x => x.BookingId == bookingId).OrderBy(x => x.CombineProductId).ThenByDescending(x => x.CombineAqlQuantity).ToList();
                    var bookingList = parameterList.BookingInvoice.BookingItems?.Where(x => x.BookingId == bookingId).FirstOrDefault();
                    var quotationData = parameterList.QuotationDetails?.QuotDetails.Where(x => x.Booking.Any(y => y.IdBooking == bookingId)).FirstOrDefault();

                    foreach (var product in products)
                    {
                        //logic to display the report remarks only in the first product if combined
                        string remarks = "";
                        string fbReportComments = "";
                        var remarkList = parameterList.ReportRemarks?.Where(x => x.ReportId == product.ReportId && !string.IsNullOrEmpty(x.Remarks)).ToList();
                        var fbCommentList = parameterList.FbReportComments?.Where(x => x.FBReportId == product.ReportId && !string.IsNullOrEmpty(x.Remarks)).ToList();

                        if (product.ReportId > 0)
                        {
                            if (previousReportId == product.ReportId || (combineId == product.CombineProductId && combineId != 0))
                            {
                                remarks = "";
                                fbReportComments = "";
                            }
                            else
                            {
                                //if products are not combined
                                if (combineId == 0)
                                {
                                    remarks = remarkList != null ? string.Join("; ", remarkList?.Where(x => !string.IsNullOrEmpty(x.Remarks)).Select(x => x.Remarks).Distinct()) : "";
                                    fbReportComments = fbCommentList != null ? string.Join("; ", fbCommentList?.Where(x => x.FBReportId == product.ReportId && !string.IsNullOrEmpty(x.Remarks)).Select(x => x.Remarks).Distinct()) : "";
                                }

                                //first product of combine products
                                else
                                {
                                    //fetch the common remarks - productId = null
                                    remarks = remarkList != null ? string.Join("; ", remarkList?.Where(x => x.ProductId == null).Select(x => x.Remarks).Distinct()) : "";
                                    fbReportComments = fbCommentList != null ? string.Join("; ", fbCommentList?.Where(x => x.ProductId == null).Select(x => x.Remarks).Distinct()) : "";

                                    //fetch the remark with product Id
                                    var productRemarksList = remarkList.Where(x => x.ProductId != null).Select(x => x.Remarks.Trim()).Distinct().ToList();

                                    //For each remark, fetch the product names and concatenate
                                    foreach (var remark in productRemarksList)
                                    {
                                        var rem = remark.Trim();
                                        var prod = remarkList?.Where(x => x.Remarks.Trim() == rem).Select(x => x.ProductId).ToList();
                                        string prodNames = string.Join(", ", products.Where(x => prod.Contains(x.ProductId)).Select(x => x.ProductName));
                                        remarks = remarks + ", " + prodNames + " => " + remark;
                                    }

                                    //fetch the fb comment with product Id
                                    var prodFbCommentList = fbCommentList.Where(x => x.ProductId != null).Select(x => x.Remarks.Trim()).Distinct().ToList();

                                    //For each fb comment, fetch the product names and concatenate
                                    foreach (var comment in prodFbCommentList)
                                    {
                                        var com = comment.Trim();
                                        var prod = fbCommentList?.Where(x => x.Remarks.Trim() == com).Select(x => x.ProductId).ToList();
                                        string prodNames = string.Join(", ", products.Where(x => prod.Contains(x.ProductId)).Select(x => x.ProductName));
                                        fbReportComments = fbReportComments + ", " + prodNames + " => " + comment;
                                    }

                                }
                            }
                            previousReportId = product.ReportId.GetValueOrDefault();
                            combineId = product.CombineProductId.GetValueOrDefault();

                        }
                        var res = new ExportTemplateItem
                        {
                            BookingNo = bookingId,
                            InspectionStartDate = product?.ServiceStartDate?.ToString(StandardDateFormat),
                            InspectionEndDate = product?.ServiceEndDate?.ToString(StandardDateFormat),
                            PONumber = string.Join(", ", parameterList.PoDetails?.Where(x => x.BookingId == bookingId && x.ProductRefId == product.Id).Select(x => x.PoNumber)),
                            bookingStatus = bookingList?.StatusName,
                            PaidBy = quotationData != null ? quotationData.BillPaidBy == (int)QuotationPaidBy.customer ? QuotationPaidBy.customer.ToString() : QuotationPaidBy.supplier.ToString() : "",
                            SupplierName = bookingList.SupplierName,
                            Office = bookingList?.Office,
                            SupplierCode = parameterList.SupplierCode != null ? parameterList.SupplierCode?.Where(x => x.SupplierId == bookingList.SupplierId).Select(x => x.Code).FirstOrDefault() : "",
                            FactoryName = bookingList?.FactoryName,
                            CustomerContact = parameterList.CustomerContactData != null ? string.Join(", ", parameterList.CustomerContactData?.Where(x => x.BookingId == bookingId).Select(x => x.StaffName).Distinct()) : "",
                            ServiceTypeName = parameterList.ServiceTypeList != null ? parameterList.ServiceTypeList?.Where(x => x.InspectionId == bookingId).Select(x => x.serviceTypeName).FirstOrDefault() : "",
                            DeptCode = string.Join(", ", parameterList.CustomerDept?.Where(x => x.BookingId == bookingId).Select(x => x.Name)),
                            AQLLevelName = string.Join(", ", product.AQLName),
                            ProductName = product?.ProductName,
                            CombineId = product?.CombineProductId == 0 ? null : product?.CombineProductId,
                            ReportStatus = product?.ReportResult,
                            ReportRemarks = remarks.Trim(','),
                            FbReportComments = fbReportComments.Trim(','),
                            FBRemarkResult = parameterList.FBReportInspSubSummaryList != null ? parameterList.FBReportInspSubSummaryList.Where(x => x.FBReportId == product.ReportId).Select(x => x.Result).FirstOrDefault() : "",
                            FBRemarkNumber = string.IsNullOrEmpty(remarks) ? 0 : 1,
                            NewProduct = product?.IsNewProduct.HasValue ?? false ? "Yes" : "No"
                        };

                        result.Add(res);
                    }
                }
            }

            return result.OrderBy(x => x.BookingNo).ToList();
        }

        //Product based mapping to export report packing and report battery details
        public List<CarreFourECOPackTemplate> BookingMapByBatteryPacking(KpiCarreFourECOPackParameters parameterList)
        {
            List<CarreFourECOPackTemplate> result = new List<CarreFourECOPackTemplate>();
            var distinctBookingList = parameterList.BookingItems?.Select(x => x.BookingId).Distinct().ToList();
            if (distinctBookingList != null)
            {
                foreach (var bookingId in distinctBookingList)
                {

                    var products = parameterList.ProductList?.Where(x => x.BookingId == bookingId).OrderBy(x => x.CombineProductId).ThenByDescending(x => x.CombineAqlQuantity).ToList();
                    var bookingList = parameterList.BookingItems?.FirstOrDefault(x => x.BookingId == bookingId);
                    if (products != null && bookingList != null)
                    {
                        foreach (var product in products)
                        {
                            var batteryData = parameterList?.ReportBatteryData.Where(x => x.ReportId == product.ReportId && x.ProductId == product.ProductId).ToList();
                            var packingData = parameterList?.ReportPackingData.Where(x => x.ReportId == product.ReportId && x.ProductId == product.ProductId).ToList();
                            var batteryDataCount = batteryData.Count;
                            var packingDataCount = packingData.Count;

                            //if multiple battery data or packing data, each result must have a row in the excel
                            var loopCount = 0;
                            if (batteryDataCount == packingDataCount)
                                loopCount = batteryDataCount;

                            //if battery data count is greater than packing data count, loop for battery count
                            else if (batteryDataCount > packingDataCount)
                                loopCount = batteryDataCount;

                            //if packing data count is greater than battery data count, loop for packing count
                            else if (batteryDataCount < packingDataCount)
                                loopCount = packingDataCount;

                            //loop the product even if there is no packing and battery data
                            loopCount = loopCount == 0 ? 1 : loopCount;

                            var qcName = parameterList.QcNames != null ? string.Join(", ", parameterList.QcNames?.Where(x => x.Id == product.ReportId).Select(x => x.Name).Distinct()) : "";

                            for (int i = 0; i < loopCount; i++)
                            {
                                var batData = batteryDataCount > i ? batteryData[i] : null;
                                var packData = packingDataCount > i ? packingData[i] : null;

                                var poNumber = string.Join(", ", parameterList.PoDetails?.Where(x => x.BookingId == bookingId && x.ProductRefId == product.Id).Select(x => x.PoNumber).Distinct());
                                var deptCode = string.Join(", ", parameterList.CustomerDept?.Where(x => x.BookingId == bookingId).Select(x => x.DepartmentName));
                                var serviceType = parameterList.ServiceTypeList != null ? parameterList.ServiceTypeList?.Where(x => x.InspectionId == bookingId).Select(x => x.serviceTypeName).FirstOrDefault() : "";
                                var res = new CarreFourECOPackTemplate
                                {
                                    BookingNo = bookingId,
                                    SupplierName = bookingList.SupplierName,
                                    PONumber = poNumber,
                                    DeptCode = deptCode,
                                    ProductName = product?.ProductName,
                                    ProductDescription = product?.ProductDescription,
                                    FactoryName = bookingList?.FactoryName,
                                    Office = bookingList?.Office,
                                    ShipmentQty = product?.BookingQuantity.GetValueOrDefault().ToString(),
                                    ServiceTypeName = serviceType,
                                    bookingStatus = bookingList?.StatusName,
                                    InspectionStartDate = product?.ServiceStartDate,
                                    InspectionEndDate = product?.ServiceEndDate,
                                    CustomerBookingNo = bookingList?.CustomerBookingNo ?? "",
                                    ProductCategory = product?.ProductCategory ?? "",
                                    ProductSubCategory = product?.ProductSubCategory ?? "",
                                    ProductSubCategory2 = product?.ProductSubCategory2 ?? "",
                                    ReportResult = product?.ReportResult ?? "",
                                    BatteryType = batData?.BatteryType ?? "",
                                    BatteryModel = batData?.BatteryModel ?? "",
                                    BatteryQuantity = batData?.Quantity,
                                    BatteryNetWeight = batData?.NetWeight,
                                    PieceNo = packData?.PieceNo,
                                    MaterialGroup = packData?.MaterialGroup,
                                    MaterialCode = packData?.MaterialCode,
                                    PackingLocation = packData?.Location,
                                    PackingQuantity = packData?.Quantity,
                                    PackingNetWeight = packData?.NetWeight,
                                    QcName = qcName
                                };

                                result.Add(res);
                            }
                        }
                    }
                }
            }
            return result.OrderBy(x => x.BookingNo).ToList();
        }

        //Product based mapping to export report packing and report battery details
        public List<ExportTemplateItem> BookingMapByBatteryPacking_old(KPIMapParameters parameterList)
        {
            List<ExportTemplateItem> result = new List<ExportTemplateItem>();
            var distinctBookingList = parameterList.BookingInvoice?.BookingItems?.Select(x => x.BookingId).Distinct().ToList();

            foreach (var bookingId in distinctBookingList)
            {
                if (bookingId > 0)
                {
                    var products = parameterList.ProductList?.Where(x => x.BookingId == bookingId).OrderBy(x => x.CombineProductId).ThenByDescending(x => x.CombineAqlQuantity).ToList();
                    var bookingList = parameterList.BookingInvoice?.BookingItems?.Where(x => x.BookingId == bookingId).FirstOrDefault();

                    foreach (var product in products)
                    {
                        var batteryData = parameterList?.ReportBatteryData.Where(x => x.ReportId == product.ReportId && x.ProductId == product.ProductId).ToList();
                        var packingData = parameterList?.ReportPackingData.Where(x => x.ReportId == product.ReportId && x.ProductId == product.ProductId).ToList();
                        var batteryDataCount = batteryData.Count();
                        var packingDataCount = packingData.Count();

                        //if multiple battery data or packing data, each result must have a row in the excel
                        var loopCount = 0;
                        if (batteryDataCount == packingDataCount)
                            loopCount = batteryDataCount;

                        //if battery data count is greater than packing data count, loop for battery count
                        else if (batteryDataCount > packingDataCount)
                            loopCount = batteryDataCount;

                        //if packing data count is greater than battery data count, loop for packing count
                        else if (batteryDataCount < packingDataCount)
                            loopCount = packingDataCount;

                        //loop the product even if there is no packing and battery data
                        loopCount = loopCount == 0 ? 1 : loopCount;

                        for (int i = 0; i < loopCount; i++)
                        {
                            var batData = batteryDataCount > i ? batteryData[i] : null;
                            var packData = packingDataCount > i ? packingData[i] : null;

                            var res = new ExportTemplateItem
                            {
                                BookingNo = bookingId,
                                SupplierName = bookingList.SupplierName,
                                PONumber = string.Join(", ", parameterList.PoDetails?.Where(x => x.BookingId == bookingId && x.ProductRefId == product.Id).Select(x => x.PoNumber).Distinct()),
                                DeptCode = string.Join(", ", parameterList.CustomerDept?.Where(x => x.BookingId == bookingId).Select(x => x.Name)),
                                ProductName = product?.ProductName,
                                ProductDescription = product?.ProductDescription,
                                FactoryName = bookingList?.FactoryName,
                                Office = bookingList?.Office,
                                ShipmentQty = product?.BookingQuantity.GetValueOrDefault().ToString(),
                                ServiceTypeName = parameterList.ServiceTypeList != null ? parameterList.ServiceTypeList?.Where(x => x.InspectionId == bookingId).Select(x => x.serviceTypeName).FirstOrDefault() : "",
                                bookingStatus = bookingList?.StatusName,
                                InspectionStartDate = product?.ServiceStartDate?.ToString(StandardDateFormat),
                                InspectionEndDate = product?.ServiceEndDate?.ToString(StandardDateFormat),
                                CustomerBookingNo = bookingList?.CustomerBookingNo ?? "",
                                CustomerName = bookingList?.CustomerName ?? "",
                                ProductCategory = product?.ProductCategory ?? "",
                                ProductSubCategory = product?.ProductSubCategory ?? "",
                                ProductSubCategory2 = product?.ProductSubCategory2 ?? "",
                                ReportResult = product?.ReportResult ?? "",
                                PriceCategory = bookingList?.PriceCategory ?? "",
                                BatteryType = batData?.BatteryType ?? "",
                                BatteryModel = batData?.BatteryModel ?? "",
                                BatteryQuantity = batData?.Quantity,
                                BatteryNetWeight = batData?.NetWeight,
                                PieceNo = packData?.PieceNo,
                                MaterialGroup = packData?.MaterialGroup,
                                MaterialCode = packData?.MaterialCode,
                                PackingLocation = packData?.Location,
                                PackingQuantity = packData?.Quantity,
                                PackingNetWeight = packData?.NetWeight
                            };

                            result.Add(res);
                        }
                    }
                }
            }

            return result.OrderBy(x => x.BookingNo).ToList();
        }

        /// <summary>
        /// Map the carrefour invoice template export
        /// </summary>
        /// <param name="parameterList"></param>
        /// <returns></returns>
        public List<CarrefourInvoiceResponse> MapCarrefourInvoiceTemplate(KpiCarrefourInvoiceParameters parameterList)
        {
            List<CarrefourInvoiceResponse> result = new List<CarrefourInvoiceResponse>();
            var bookingItem = new KpiInspectionBookingItems();
            var previousReportId = 0;
            var combineId = 0;

            // var productlist= parameterList.ProductList?.OrderBy(x => x.CustomerBookingNumber).ThenByDescending(x => x.BookingFormSerial.HasValue).ThenBy(x => x.BookingFormSerial).ToList();


            var productlist = parameterList.ProductList?.
                                OrderBy(x => x.CustomerBookingNumber).
                               ThenByDescending(x => x.BookingFormSerial.HasValue).ThenBy(x => x.BookingFormSerial).ToList();

            // var productlist = parameterList.ProductList?.OrderBy(x => x.BookingId).ThenBy(x => x.ReportId).ThenBy(x => x.CombineProductId).ThenByDescending(x => x.CombineAqlQuantity).ToList();
            //loop through the products
            foreach (var item in productlist)
            {
                if (item.ProductName != null)
                {
                    string LocationName = string.Empty;
                    string deptCode = string.Empty;
                    string deptDivision = string.Empty;
                    int bookingId = item.BookingId;
                    var invoiceData = new InvoiceBookingData();
                    string quotationbilledto = parameterList?.QuotationDetails?.Where(x => x.BookingId == item.BookingId).Select(z => z.BillPaidByName).FirstOrDefault();
                    string invoiceNumber = string.Empty;
                    double? inspectionFee = 0;
                    double? travellingCost = 0;
                    double? hotelFee = 0;
                    double? otherFee = 0;
                    double? totalInspectionFee = 0;
                    string currencyName = string.Empty;
                    int sampleSize = 0;
                    int? QCCount = 0;
                    double exFee = 0;
                    KpiExtraFeeData extraFee = new KpiExtraFeeData();


                    var bookingPO = parameterList.PoDetails?.Where(x => x.BookingId == bookingId).ToList();
                    var productPO = parameterList.PoDetails?.FirstOrDefault(x => x.ProductRefId == item.ProductId);

                    var fbDimentionDetail = parameterList.FBDimentionList?.FirstOrDefault(x => x.ProductId == item.ProductId && x.ReportId == item.ReportId);
                    var fbWeightDetail = parameterList.FBWeightList?.FirstOrDefault(x => x.ProductId == item.ProductId && x.ReportId == item.ReportId);

                    var fbOtherInformation = parameterList.FbReportComments?.Where(x => x.FBReportId == item.ReportId && x.ProductId.HasValue && x.ProductId.Value == item.ProductId).ToList();
                    if (fbOtherInformation == null || !fbOtherInformation.Any())
                    {
                        fbOtherInformation = parameterList.FbReportComments?.Where(x => x.FBReportId == item.ReportId && !x.ProductId.HasValue).ToList();
                    }

                    double.TryParse(fbDimentionDetail?.SpecClientValuesHeight, out double SpecClientValuesHeight);
                    double.TryParse(fbDimentionDetail?.SpecClientValuesLength, out double SpecClientValuesLength);
                    double.TryParse(fbWeightDetail?.SpecClientValuesWeight, out double SpecClientValuesWeight);
                    double.TryParse(fbDimentionDetail?.SpecClientValuesWidth, out double SpecClientValuesWidth);

                    double.TryParse(fbDimentionDetail?.MeasuredValuesHeight, out double MeasuredValuesHeight);
                    double.TryParse(fbDimentionDetail?.MeasuredValuesLength, out double MeasuredValuesLength);
                    double.TryParse(fbDimentionDetail?.MeasuredValuesWidth, out double MeasuredValuesWidth);
                    double.TryParse(fbWeightDetail?.MeasuredValuesWeight, out double MeasuredValuesWeight);

                    var SpecClientVolume = SpecClientValuesHeight * SpecClientValuesLength * SpecClientValuesWidth;
                    var MeasuredVolume = MeasuredValuesHeight * MeasuredValuesLength * MeasuredValuesWidth;

                    string remarks = string.Empty;

                    if (result.Any(x => x.ReportId == item.ReportId && x.AQLQuantity > 0))
                    {
                        remarks = "";
                        sampleSize = 0;
                        QCCount = 0;
                    }
                    else
                    {
                        if (item.ReportResultId == (int)FBReportResult.Fail)
                            remarks = string.Join("; ", parameterList.ReportRemarks?.Where(x => x.ReportId == item.ReportId
                            && !string.IsNullOrWhiteSpace(x.Remarks)).Select(x => x.Remarks).Distinct());

                        if (item.CombineProductId > 0)
                        {
                            sampleSize = productlist.FirstOrDefault(x => x.ReportId == item.ReportId && x.CombineAqlQuantity > 0

                                                 && x.CombineProductId == item.CombineProductId.GetValueOrDefault())?.CombineAqlQuantity.GetValueOrDefault() ?? 0;
                        }

                        else
                        {
                            sampleSize = item.AqlQty;
                        }

                        QCCount = parameterList.QcNames?.Count(x => x.Id == item.ReportId);
                    }

                    previousReportId = item.ReportId.GetValueOrDefault();
                    combineId = item.CombineProductId.GetValueOrDefault();


                    //logic to display quotation manday only on the first row if 1 booking has multiple reports
                    if (!result.Any(x => x.BookingNo == bookingId))
                    {
                        bookingItem = parameterList.BookingItems?.FirstOrDefault(x => x.BookingId == bookingId);
                        invoiceData = parameterList?.InvoiceBookingData?.FirstOrDefault(x => x.BookingId == item.BookingId);
                        extraFee = parameterList.ExtraFeeData.FirstOrDefault(x => x.InvoiceId == invoiceData?.Id && x.BilledTo == invoiceData?.BilledTo);
                        exFee = extraFee == null || extraFee.ExtraFee == null ? 0 : extraFee.ExtraFee.GetValueOrDefault();


                        if (invoiceData != null)
                        {
                            invoiceNumber = invoiceData?.InvoiceNo;
                            inspectionFee = invoiceData?.InspFees;
                            hotelFee = invoiceData?.HotelFee.GetValueOrDefault();
                            travellingCost = invoiceData?.TravelFee;
                            otherFee = invoiceData?.OtherExpense;
                            totalInspectionFee = invoiceData?.InvoiceTotal.GetValueOrDefault() + exFee;
                            currencyName = invoiceData?.InvoiceCurrency;
                        }
                    }
                    else
                    {
                        invoiceData = parameterList?.InvoiceBookingData?.FirstOrDefault(x => x.BookingId == item.BookingId);
                        if (invoiceData != null)
                        {
                            invoiceNumber = invoiceData?.InvoiceNo;
                        }
                    }

                    //office location name based on customer booking number starts
                    if (!string.IsNullOrWhiteSpace(bookingItem.CustomerBookingNo))
                    {
                        if (bookingItem.CustomerBookingNo.StartsWith('1') || bookingItem.CustomerBookingNo.StartsWith('4'))
                            LocationName = LocationOffice;
                        else if (bookingItem.CustomerBookingNo.StartsWith('2'))
                            LocationName = LocationOffice1;
                    }

                    //first two letter of po number will be a dept code
                    deptCode = bookingPO?.FirstOrDefault()?.PoNumber?.Substring(0, 2);

                    //dept code convert to int 
                    if (!string.IsNullOrWhiteSpace(deptCode) && int.TryParse(deptCode, out int deptdivi))
                    {
                        if (deptdivi < 50)
                        {
                            deptDivision = DeptDivision;
                        }
                        else if (deptdivi >= 50)
                        {
                            deptDivision = DeptDivision1;
                        }
                    }

                    var fbDefects = parameterList.FBReportDefectsList?.FirstOrDefault(x => x.FBReportDetailId == item.ReportId);

                    var totalCartons = parameterList.FbBookingQuantity?.Where(x => x.ReportId == item.ReportId).Select(x => x.TotalCartons).FirstOrDefault();

                    var factoryLocation = parameterList.FactoryLocation?.FirstOrDefault(x => x.BookingId == bookingId);

                    var supplierCode = parameterList.SupplierCode?.FirstOrDefault(x => x.SupplierId == bookingItem.SupplierId && x.CustomerId == bookingItem.CustomerId);

                    var res = new CarrefourInvoiceResponse
                    {
                        InspCompany = "API",
                        //OfficeLocationName = LocationName,
                        Office = bookingItem?.Office,
                        SupplierName = bookingItem?.SupplierName,
                        ReportId = item.ReportId.GetValueOrDefault(),

                        SupplierCode = supplierCode.Code,

                        FactoryName = bookingItem?.FactoryName,

                        FactoryCity = factoryLocation.CityName,
                        FactoryState = factoryLocation.ProvinceName,
                        FactoryCountry = factoryLocation.CountryName,

                        PONumber = string.Join(",", bookingPO?.Select(x => x.PoNumber).Distinct()),
                        DeptCode = deptCode,
                        DeptDivision = deptDivision,

                        //ProductName = item.ProductName,
                        ProductDescription = item.ProductDescription,
                        ProductCount = item.BookingQuantity,
                        DestinationCountry = productPO?.DestinationCountry,

                        InvoiceNumber = invoiceNumber,
                        InspectionFee = inspectionFee,
                        TravellingCost = travellingCost,
                        HotelFee = hotelFee,
                        OtherFee = otherFee,
                        TotalInspectionFee = totalInspectionFee,
                        //CurrencyName = currencyName,


                        InspectionStartDate = bookingItem?.ServiceDateFrom,

                        //InspMonthNumber = bookingItem?.ServiceDateFrom.Month,

                        InspMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(bookingItem.ServiceDateFrom.Month),

                        ServiceTypeName = parameterList.ServiceTypeList?.Where(x => x.InspectionId == bookingId).Select(x => x.serviceTypeName).FirstOrDefault(),

                        //report details
                        ReportResult = item.ReportResultName,

                        InspectionReportResult = item.ReportResultName,

                        CustomerBookingNumber = bookingItem?.CustomerBookingNo,

                        InspectionStartedDate = bookingItem?.ServiceDateFrom,

                        FirstServiceDateFrom = bookingItem?.FirstServiceDateFrom,

                        RescheduleReason = parameterList.RescheduleList?.Where(x => x.BookingId == item.BookingId).OrderByDescending(x => x.CreatedOn).Select(x => x.ReasonName).FirstOrDefault(),

                        InspectionEndDate = bookingItem?.ServiceDateTo,

                        InspectedQty = parameterList.FbBookingQuantity?.Where(x => x.ReportId == item.ReportId && x.BookingId == item.BookingId && x.ProductId?.Id == item.ProductId).Sum(x => x.InspectedQty.GetValueOrDefault()),

                        QcCount = QCCount,

                        AQLQuantity = sampleSize,

                        //fb other information table 
                        GoldenSampleAvailable = (bool)fbOtherInformation?.Any(x => x?.SubCategory?.ToLower() == FBSubCategoryGoldenSample?.ToLower()) ? "Yes" : string.Empty,

                        CriticalDefect = fbDefects?.Critical,
                        MajorDefect = fbDefects?.Major,
                        MinorDefect = fbDefects?.Minor,
                        ReportRemarks = remarks,
                        ProrateBookingNo = !string.IsNullOrWhiteSpace(invoiceData?.ProrateBookingNo)
                        ? "Fees for reportNo: " + invoiceData?.ProrateBookingNo : "",


                        PCB1 = totalCartons > 0 ? Math.Round((item.BookingQuantity / totalCartons).GetValueOrDefault(), 2) : 0,

                        SpecClientValuesLength = fbDimentionDetail?.SpecClientValuesLength,
                        SpecClientValuesWidth = fbDimentionDetail?.SpecClientValuesWidth,
                        SpecClientValuesHeight = fbDimentionDetail?.SpecClientValuesHeight,
                        SpecClientVolume = SpecClientVolume,
                        SpecClientValuesWeight = fbWeightDetail?.SpecClientValuesWeight,

                        PCB2 = totalCartons > 0 ? Math.Round((item.BookingQuantity / totalCartons).GetValueOrDefault(), 2) : 0,
                        MeasuredValuesHeight = fbDimentionDetail?.MeasuredValuesHeight,
                        MeasuredValuesLength = fbDimentionDetail?.MeasuredValuesLength,
                        MeasuredValuesWidth = fbDimentionDetail?.MeasuredValuesWidth,
                        MeasuredValuesWeight = fbWeightDetail?.MeasuredValuesWeight,

                        MeasuredVolume = MeasuredVolume,
                        TechnicalFile = "No",
                        SizeSpec = "N/A",

                        PercentageVolume = SpecClientVolume > 0 ? Math.Round(((MeasuredVolume - SpecClientVolume) / SpecClientVolume), 2) * 100 : 0,
                        PercentageWeight = SpecClientValuesWeight > 0 ? Math.Round(((MeasuredValuesWeight - SpecClientValuesWeight) / SpecClientValuesWeight), 2) * 100 : 0,
                        PriceCategoryId = bookingItem?.PriceCategoryId,
                        //BookingFormSerial = item?.BookingFormSerial,
                        BookingNo = bookingId,
                        InvoiceTo = invoiceData?.BilledName,
                        ExtraFee = exFee

                    };
                    result.Add(res);
                }
            }

            return result.ToList();
        }

        //Map invoice data on product level
        public List<GeneralInvoiceResponse> BookingMapInvoice(KpiGeneralInvoiceParameters parameterList)
        {
            List<GeneralInvoiceResponse> result = new List<GeneralInvoiceResponse>();
            var distinctBookingList = parameterList.BookingItems?.OrderBy(x => x.BookingId).Select(x => x.BookingId).Distinct().ToList();
            if (distinctBookingList != null)
            {
                var quotMandayList = parameterList.QuotationDetails?.MandayList?.OrderBy(x => x.QuotationId).ThenBy(x => x.BookingId);

                int prevQuotationId = 0;

                foreach (int bookingId in distinctBookingList)
                {

                    int sampleSize = 0;
                    List<int> bookingIdList = new List<int>();
                    var previousReportId = 0;
                    var combineId = 0;
                    double OtherCost = 0;

                    var products = parameterList.ProductList?.Where(x => x.BookingId == bookingId).OrderBy(x => x.CombineProductId).ThenByDescending(x => x.CombineAqlQuantity).ToList();
                    if (products != null)
                    {
                        var bookingData = parameterList.BookingItems?.FirstOrDefault(x => x.BookingId == bookingId);
                        var quotMandayData = quotMandayList?.FirstOrDefault(x => x.BookingId == bookingId);
                        var invoiceData = parameterList?.InvoiceBookingData?.FirstOrDefault(x => x.BookingId == bookingId);
                        var extraFee = parameterList.ExtraFeeData?.FirstOrDefault(x => x.InvoiceId == invoiceData?.Id && x.BilledTo == invoiceData?.BilledTo);

                        //fetch quotation id from manday list using booking id
                        int quotationId = quotMandayData?.QuotationId ?? 0;

                        if (prevQuotationId != quotationId)
                        {
                            //get other cost based on quotation id
                            OtherCost = parameterList?.QuotationDetails?.QuotDetails?.FirstOrDefault(x => x.QuotationId == quotationId)?.OtherCost ?? 0;
                            prevQuotationId = quotationId;
                        }
                        else
                        {
                            OtherCost = 0;
                        }

                        foreach (var product in products)
                        {
                            //logic to display the sample size only in the first product if combined
                            if (previousReportId == product.ReportId || (combineId == product.CombineProductId && combineId != 0))
                            {
                                sampleSize = 0;
                            }
                            else
                            {
                                sampleSize = product.CombineProductId > 0 ? product.CombineAqlQuantity.GetValueOrDefault() : product.AqlQty;
                            }
                            previousReportId = product.ReportId.GetValueOrDefault();
                            combineId = product.CombineProductId.GetValueOrDefault();

                            //logic to display quotation manday and invoice cost only on the first row if 1 booking has mustiple reports
                            double manday = 0;
                            double invoiceInspFee = 0;
                            double invoiceTotalCost = 0;
                            double invoiceTravelCost = 0;
                            double invoiceHotelCost = 0;
                            string invoiceNo = "";
                            double exFee = 0;

                            if (bookingIdList.Contains(bookingId))
                            {
                                manday = 0;
                                invoiceInspFee = 0;
                                invoiceTotalCost = 0;
                                invoiceTravelCost = 0;
                                invoiceNo = "";
                                exFee = 0;
                            }
                            else
                            {
                                bookingIdList.Add(bookingId);
                                manday = quotMandayData != null ? (double)quotMandayData?.Manday : manday;
                                exFee = extraFee == null || extraFee.ExtraFee == null ? 0 : extraFee.ExtraFee.GetValueOrDefault();
                                invoiceInspFee = invoiceData?.InspFees ?? 0;
                                invoiceTravelCost = invoiceData?.TravelFee ?? 0;
                                invoiceTotalCost = ((invoiceData?.InvoiceTotal ?? 0) + exFee + OtherCost);
                                invoiceHotelCost = invoiceData?.HotelFee ?? 0;
                                invoiceNo = invoiceData?.InvoiceNo ?? "";
                            }

                            var customerContact = parameterList.CustomerContactData != null ? string.Join(", ", parameterList.CustomerContactData?.Where(x => x.BookingId == bookingId).Select(x => x.ContactName).Distinct()) : "";
                            var poNumber = string.Join(", ", parameterList.PoDetails?.Where(x => x.BookingId == bookingId && x.ProductRefId == product.Id).Select(x => x.PoNumber));
                            var deptCode = string.Join(", ", parameterList.CustomerDept?.Where(x => x.BookingId == bookingId).Select(x => x.DepartmentName));
                            var supplierCode = parameterList.SupplierCode != null ? parameterList.SupplierCode?.Where(x => x.SupplierId == bookingData.SupplierId && x.CustomerId == bookingData.CustomerId).Select(x => x.Code).FirstOrDefault() : "";
                            var serviceTypeName = parameterList.ServiceTypeList != null ? parameterList.ServiceTypeList?.Where(x => x.InspectionId == bookingId).Select(x => x.serviceTypeName).FirstOrDefault() : "";
                            var buyerName = parameterList?.CustomerBuyerList != null ? string.Join(", ", parameterList?.CustomerBuyerList?.Where(x => x.BookingId == bookingId).Select(x => x.BuyerName).Distinct()) : "";
                            var brandName = parameterList?.CustomerBrandList != null ? string.Join(", ", parameterList?.CustomerBrandList?.Where(x => x.BookingId == bookingId).Select(x => x.Name).Distinct()) : "";
                            var merchandiser = parameterList?.CustomerMerchandiserList != null ? string.Join(",", parameterList?.CustomerMerchandiserList?.Where(x => x.BookingId == bookingId).Select(x => x.Name).Distinct()) : "";
                            var res = new GeneralInvoiceResponse
                            {
                                BookingNo = bookingId,
                                SupplierName = bookingData.SupplierName,
                                PONumber = poNumber,
                                DeptCode = deptCode,
                                SupplierCode = supplierCode,
                                ProductName = product?.ProductName,
                                ProductDescription = product?.ProductDescription,
                                FactoryName = bookingData?.FactoryName,
                                FactoryCountry = bookingData?.FactoryCountry,
                                FactoryProvince = bookingData?.FactoryProvince,
                                ServiceTypeName = serviceTypeName,
                                ManDay = manday,
                                InspectionFee = invoiceInspFee,
                                TravellingCost = invoiceTravelCost,
                                TotalInspectionFee = invoiceTotalCost,
                                HotelFee = invoiceHotelCost,
                                InvoiceNumber = invoiceNo,
                                CustomerContact = customerContact,
                                InspectionStartDate = bookingData?.ServiceDateFrom,
                                InspectionEndDate = bookingData?.ServiceDateTo,
                                AQLLevelName = string.Join(", ", product.AQLName),
                                CustomerName = bookingData?.CustomerName ?? "",
                                BuyerName = buyerName,
                                ReportResult = product?.ReportResultName ?? "",
                                FactoryRef = product?.FactoryReference ?? "",
                                SampleSize = sampleSize.ToString(),
                                BrandName = brandName,
                                CollectionName = bookingData.CollectionName,
                                QuotationId = quotMandayData?.QuotationId ?? 0,
                                ReportId = product.ReportId ?? 0,
                                FirstServiceDateFrom = bookingData.FirstServiceDateFrom,
                                OrderQty = product.BookingQuantity.ToString(),
                                InvoiceRemarks = invoiceData?.InvoieRemarks ?? "",
                                ExtraFee = exFee,
                                OtherFee = OtherCost,
                                Merchandiser = merchandiser,

                            };

                            result.Add(res);
                        }
                    }

                }
            }

            return result.OrderBy(x => x.BookingNo).ToList();
        }

        //Product based mapping to export report packing and report battery details
        public List<ExportTemplateItem> MdmDefectSummaryMap(List<MdmDefectData> data)
        {
            return data.ConvertAll(x => new ExportTemplateItem
            {
                BookingNo = x.BookingNo.GetValueOrDefault(),
                ServiceDate = x.ServiceDate,
                CustomerName = x.CustomerName,
                SupplierName = x.SupplierName,
                FactoryName = x.FactoryName,
                ServiceTypeName = x.ServiceType,
                bookingStatus = x.InspectionStatus,
                ProductName = x.ProductRef,
                ProductDescription = x.ProductDesc,
                PONumber = x.PoNumber,
                PoQuantity = x.PoQty.GetValueOrDefault(),
                InspectedQty = x.InspectedQty.GetValueOrDefault(),
                TotalDefects = x.TotalDefects.GetValueOrDefault(),
                CriticalDefect = x.TotalCritical,
                MajorDefect = x.TotalMajor,
                MinorDefect = x.TotalMinor,
                TotalQtyReworked = x.TotalQtyReworked.GetValueOrDefault(),
                TotalQtyRejected = x.TotalQtyRejected.GetValueOrDefault(),
                TotalQtyReplaced = x.TotalQtyReplaced.GetValueOrDefault(),
                FinalReportStatus = x.FinalResult,
                ProductId = x.Productid.GetValueOrDefault(),
                ReportId = x.Reportid.GetValueOrDefault(),
                DeptCode = x.Department,
                FactoryCountry = x.FactoryCountry
            }).ToList();
        }

        /// <summary>
        /// Map the inspection picking data 
        /// </summary>
        /// <param name="parameterList"></param>
        /// <returns></returns>
        public List<ExportTemplateItem> InspectionPickingMap(KPIMapParameters parameterList)
        {
            List<ExportTemplateItem> result = new List<ExportTemplateItem>();
            if (parameterList != null && parameterList.InspectionPickingList != null && parameterList.InspectionPickingList.Any())
            {
                foreach (var pickinginfo in parameterList.InspectionPickingList)
                {
                    var exportItem = new ExportTemplateItem()
                    {

                        PickingProductName = pickinginfo.ProductName,
                        PickingProductCategory = pickinginfo.ProductCategory,
                        PickingProductSubCategory = pickinginfo.ProductSubCategory,
                        PickingCustomerName = pickinginfo.CustomerName,
                        PickingSupplierName = pickinginfo.SupplierName,
                        PickingFactoryName = pickinginfo.FactoryName,
                        PickingInspectionId = pickinginfo.InspectionId,
                        PickingServiceDate = pickinginfo.ServiceDate,
                        PickingLabName = pickinginfo.LabName,
                        PickingQuantity = pickinginfo.PickingQuantity,
                        PickingPoNumber = pickinginfo.PoNumber
                    };
                    result.Add(exportItem);
                }

            }
            return result;
        }

        public List<AdeoFailedPoTemplate> MapAdeoFailedData(KpiAdeoTemplateRequest request)
        {
            List<AdeoFailedPoTemplate> result = new List<AdeoFailedPoTemplate>();
            var reportData = request.FactoryProductData.GroupBy(p => p.ReportId).ToList();

            foreach (var item in reportData)
            {
                var product = item.FirstOrDefault();
                var reInspectionId = request.ReinspectionData != null && request.ReinspectionData.Any() ? request.ReinspectionData?.Where(x => x.BookingId == product.BookingId).Select(x => x.ReInspectionbookingId).FirstOrDefault() : 0;
                var bookingData = request.BookingData.Where(x => x.BookingId == product.BookingId).FirstOrDefault();
                var res = new AdeoFailedPoTemplate
                {
                    BookingId = product?.BookingId ?? 0,
                    PoNumber = string.Join(", ", request.PoDetails.Where(x => x.BookingId == product.BookingId)?.Select(x => x.PoNumber).Distinct()),
                    ProductName = string.Join(", ", item?.Select(x => x.ProductName).Distinct()),
                    ServiceDate = bookingData?.ServiceDateFrom == bookingData?.ServiceDateTo ? bookingData?.ServiceDateFrom.ToString(StandardDateFormat) : bookingData?.ServiceDateFrom.ToString(StandardDateFormat) + " - " + bookingData?.ServiceDateTo.ToString(StandardDateFormat),
                    ReportResult = product?.ReportStatus,
                    DeptCode = string.Join(',', request.DeptList.Where(x => x.BookingId == product.BookingId).Select(y => y.DepartmentName).Distinct().ToArray()),
                    SupplierName = bookingData?.SupplierName,
                    SupplierCode = request.SupCode.Where(x => x.SupplierId == product.SupplierId && x.CustomerId == product.CustomerId).Select(x => x.Code).FirstOrDefault(),
                    ProductDescription = string.Join(", ", item?.Select(x => x.ProductDescription).Distinct()),
                    ReportRemarks = request.RemarksData != null ? string.Join("; ", request.RemarksData?.Where(x => x.ReportId == item.Key && !string.IsNullOrEmpty(x.Remarks)).Select(x => x.Remarks).Distinct()) : "",
                    ReinspectionId = reInspectionId > 0 ? reInspectionId.ToString() : "",
                };

                result.Add(res);
            }
            return result;
        }

        public List<KpiReportCommentsTemplate> MapInspCommentSummary(InspCommentSummaryMapRequest request)
        {
            var result = new List<KpiReportCommentsTemplate>();
            var groupedReportRemarks = request.CommentData.GroupBy(x => x.ReportId).ToList();

            foreach (var reportComments in groupedReportRemarks)
            {
                int commentsNumber = 0;

                foreach (var x in reportComments)
                {

                    if (!string.IsNullOrEmpty(x.ReportComments))
                    {
                        commentsNumber++;

                        // Add product specific filters

                        var reportProducts = request.ProductData.Where(z => z.ReportId == x.ReportId).ToList();
                        var billpaidBy = request.BillPaidByData.Where(y => y.BookingId == x.BookingNo).Select(y => y.BillPaidById).FirstOrDefault();

                        result.Add(new KpiReportCommentsTemplate()
                        {
                            BookingNo = x.BookingNo,
                            CustomerBookingNo = x.CustomerBookingNo,
                            CustomerName = x.CustomerName,
                            BuyerName = string.Join(',', request.BuyerData.Where(y => y.BookingId == x.BookingNo).Select(y => y.BuyerName).Distinct().ToArray()),
                            CustomerContact = request.ContactData.Where(y => y.BookingId == x.BookingNo).Select(y => y.ContactName).FirstOrDefault(),
                            DeptCode = string.Join(',', request.DeptData.Where(y => y.BookingId == x.BookingNo).Select(y => y.DepartmentName).Distinct().ToArray()),
                            CollectionName = x.CollectionName,
                            Office = x.Office,
                            SupplierName = x.SupplierName,
                            FactoryName = x.FactoryName,
                            FactoryCountry = request.FactoryLocation.Where(y => y.BookingId == x.BookingNo).Select(y => y.CountryName).FirstOrDefault(),
                            ServiceTypeName = request.ServiceTypeData.Where(y => y.InspectionId == x.BookingNo).Select(y => y.serviceTypeName).FirstOrDefault(),
                            BookingStatus = x.BookingStatus,
                            InspectionStartDate = x.InspectionStartDate,
                            InspectionEndDate = x.InspectionEndDate,
                            Month = x.Month,
                            Year = x.Year,
                            PONumber = string.Join(',', request.PoData.Where(y => reportProducts.Select(z => z.Id).Contains(y.ProductRefId)).Select(z => z.PoNumber).Distinct().ToArray()),
                            ProductName = string.Join(',', reportProducts.Select(z => z.ProductName).Distinct().ToArray()),
                            ProductDescription = string.Join(',', reportProducts.Select(z => z.ProductDescription).Distinct().ToArray()),
                            FactoryRef = string.Join(',', reportProducts.Select(z => z.FactoryReference).Distinct().ToArray()),
                            CombinedWith = reportProducts.Select(z => z.CombineProductId).FirstOrDefault() == 0 ? null :
                                    reportProducts.Select(z => z.CombineProductId).FirstOrDefault(),
                            ReportResult = x.ReportResult,
                            ReportComments = x.ReportComments,
                            FBCommentNumber = commentsNumber,
                            RemarkCategory = x.RemarkCategory,
                            RemarkSubCategory = x.RemarkSubCategory,
                            RemarkSubCategory2 = x.RemarkSubCategory2,
                            BillPaidBy = billpaidBy == (int)QuotationPaidBy.customer ? QuotationPaidBy.customer.ToString() : QuotationPaidBy.supplier.ToString(),
                            CustomerRemarkCodeReference = x.CustomerRemarkCodeReference,
                            ProductCategory = string.Join(',', reportProducts.Select(z => z.ProductCategory).Distinct().ToArray()),
                            ProductSubCategory = string.Join(',', reportProducts.Select(z => z.ProductSubCategory).Distinct().ToArray()),
                            ProductSubCategory2 = string.Join(',', reportProducts.Select(z => z.ProductSubCategory2).Distinct().ToArray()),
                            Merchandise = string.Join(", ", request.MerchandiserData?.Where(y => y.BookingId == x.BookingNo).Select(x => x.Name).Distinct().ToArray()),
                            SupplierCode = request.SupplierCode?.FirstOrDefault(y => y.SupplierId == x.SupplierId)?.Code,
                            FactoryCode = request.SupplierCode?.FirstOrDefault(y => y.SupplierId == x.FactoryId)?.Code
                        });
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Map Inspection Data Summary
        /// </summary>
        /// <param name="parameterList"></param>
        /// <returns></returns>
        public List<ExportInspectionData> MapInspectionDataSummary(KPIMapParameters parameterList)
        {
            var result = new List<ExportInspectionData>();
            var disctinctBookingIdList = parameterList.BookingInvoice.BookingItems.Select(x => x.BookingId).Distinct();

            foreach (var bookingId in disctinctBookingIdList)
            {
                var booking = parameterList.BookingInvoice.BookingItems.FirstOrDefault(x => x.BookingId == bookingId);
                var products = parameterList.ProductList.Where(x => x.BookingId == bookingId).OrderBy(x => x.CombineProductId);
                var factory = parameterList.FactoryLocation?.FirstOrDefault(x => x.BookingId == bookingId);
                var departments = string.Join(",", parameterList.CustomerDept?.Where(x => x.BookingId == bookingId).Select(y => y.Name));
                var brands = string.Join(",", parameterList.CustomerBrandList?.Where(x => x.BookingId == bookingId).Select(y => y.Name));
                var serviceType = parameterList.ServiceTypeList?.FirstOrDefault(x => x.InspectionId == bookingId)?.serviceTypeName;
                var merchandise = string.Join(", ", parameterList.MerchandiserList?.Where(x => x.BookingId == bookingId).Select(x => x.Name).Distinct());
                var customerContacts = string.Join(", ", parameterList.CustomerContactData?.Where(x => x.BookingId == bookingId).Select(x => x.StaffName).Distinct());
                int combineId = 0;
                foreach (var product in products)
                {
                    if (combineId == product.CombineProductId && combineId != 0)
                    {
                        continue;
                    }
                    IEnumerable<int> productIds;
                    if (product.CombineProductId.HasValue && product.CombineProductId != 0)
                    {
                        productIds = products.Where(x => x.CombineProductId == combineId).Select(y => y.ProductId);
                    }
                    else
                    {
                        productIds = new List<int>() { product.ProductId };
                    }
                    combineId = product.CombineProductId.GetValueOrDefault();
                    var isCombineProduct = product.CombineProductId.HasValue && product.CombineProductId != 0;

                    var defectList = parameterList.FBReportDefectsList.Where(x => x.FBReportDetailId == product.ReportId && productIds.Contains(x.ProductId));
                    var data = new ExportInspectionData()
                    {
                        BookingNo = booking.BookingId,
                        ProductName = isCombineProduct ? string.Join(",", products?.Where(x => productIds.Contains(x.ProductId)).Select(y => y.ProductName)) : product.ProductName,
                        ProductDescription = isCombineProduct ? string.Join(",", products?.Where(x => productIds.Contains(x.ProductId)).Select(y => y.ProductDescription)) : product.ProductDescription,
                        CustomerBookingNo = booking.CustomerBookingNo,
                        FactoryName = booking.FactoryName,
                        SupplierName = booking.SupplierName,
                        ServiceTypeName = serviceType,
                        ServiceFromDate = booking.ServiceDateFrom.ToString(StandardDateFormat),
                        ServiceToDate = booking.ServiceDateTo.ToString(StandardDateFormat),
                        FbResult = parameterList?.FBReportInspSubSummaryList?.Where(x => x.FBReportId == product.ReportId)
                               .Select(y => new FbReportInspSummaryResult
                               {
                                   Name = y.Name,
                                   Result = y.Result
                               }).ToList(),
                        ReportResult = product.ReportResult,
                        CustomerResult = parameterList.CustomerDecisionData?.FirstOrDefault(x => x.Id == product.ReportId)?.Name,
                        ProducedQty = product.PresentedQty.GetValueOrDefault(),
                        OrderQty = product.OrderQty.GetValueOrDefault(),
                        InspectedQty = product.InspectedQty.GetValueOrDefault(),
                        MajorDefect = defectList.Sum(x => x.Major),
                        MajorDefects = string.Join(";\n", defectList?.Where(x => x.Major > 0).GroupBy(x => new { x.DefectDesc, x.FBReportDetailId }).Select(x => x.Key.DefectDesc + "-" + x.Sum(y => y.Major))),
                        MinorDefect = defectList.Sum(x => x.Minor),
                        MinorDefects = string.Join(";\n", defectList?.Where(x => x.Minor > 0).GroupBy(x => new { x.DefectDesc, x.FBReportDetailId }).Select(x => x.Key.DefectDesc + "-" + x.Sum(y => y.Minor))),
                        CriticalDefect = defectList.Sum(x => x.Critical),
                        CriticalDefects = string.Join(";\n", defectList?.Where(x => x.Critical > 0).GroupBy(x => new { x.DefectDesc, x.FBReportDetailId }).Select(x => x.Key.DefectDesc + "-" + x.Sum(y => y.Critical))),
                        FactoryCountry = factory?.CountryName,
                        OfficeCountry = parameterList.OfficeCountryList?.FirstOrDefault(x => x.ParentId == booking.OfficeId)?.Name,
                        Office = booking.Office,
                        Season = booking.SeasonYear > 0 ? $"{booking.Season} - {booking.SeasonYear}" : booking.Season,
                        DeptCode = departments,
                        BrandName = brands,
                        ProductCategory = product.ProductCategory,
                        ProductSubCategory = product.ProductSubCategory,
                        ShipmentDate = booking.ShipmentDate?.ToString(StandardDateFormat),
                        bookingStatus = booking.StatusName,
                        ProductSubCategory2 = product.ProductSubCategory2,
                        Merchandise = merchandise,
                        CustomerContact = customerContacts,
                        SupplierCode = parameterList.SupplierCode?.FirstOrDefault(x => x.SupplierId == booking.SupplierId)?.Code,
                        FactoryCode = parameterList.SupplierCode?.FirstOrDefault(x => x.SupplierId == booking.FactoryId)?.Code
                    };

                    result.Add(data);
                }
            }
            return result;
        }

        public List<ExportTemplateItem> MapOrderStatusLogSummary(KPIMapParameters parameterList)
        {

            List<ExportTemplateItem> result = new List<ExportTemplateItem>();
            var distinctBookingListId = parameterList.BookingInvoice.BookingItems?.Select(x => x.BookingId).Distinct().ToList();

            foreach (var bookingId in distinctBookingListId)
            {
                if (bookingId > 0)
                {
                    var combineId = 0;

                    var products = parameterList.ProductList?.Where(x => x.BookingId == bookingId).OrderBy(x => x.BookingId).ThenBy(x => x.CombineProductId).ToList();
                    var bookingList = parameterList.BookingInvoice.BookingItems?.Where(x => x.BookingId == bookingId).FirstOrDefault();
                    var invoice = parameterList.BookingInvoice.InvoiceBookingData.FirstOrDefault(x => x.BookingId == bookingId);
                    string invoiceContact = "";
                    if (invoice != null)
                    {
                        var billedContact = parameterList.BilledContactList.FirstOrDefault(x => x.InvoiceId == invoice.Id);
                        if (billedContact != null)
                        {
                            switch (invoice.BilledTo)
                            {
                                case (int)InvoiceTo.Customer:
                                    invoiceContact = billedContact.CustomerContactName;
                                    break;
                                case (int)InvoiceTo.Factory:
                                    invoiceContact = billedContact.FactoryContactName;
                                    break;

                                case (int)InvoiceTo.Supplier:
                                    invoiceContact = billedContact.SupplierContactName;
                                    break;

                            }
                        }

                    }
                    int perBookingFirstRowIndex = 1;
                    foreach (var product in products)
                    {
                        var _exportTemplateItem = new ExportTemplateItem();

                        //logic to display the report remarks only in the first product if combined                        
                        if (combineId == product.CombineProductId && combineId != 0)
                        {
                            continue;
                        }
                        combineId = product.CombineProductId.GetValueOrDefault();

                        if (perBookingFirstRowIndex > 1)
                        {

                            _exportTemplateItem.AirTicket = null;
                            _exportTemplateItem.Invoice_HotelFee = null;
                            _exportTemplateItem.Invoice_TravelFee = null;
                            _exportTemplateItem.UnitPrice = null;
                            _exportTemplateItem.BillingManDays = null;
                            _exportTemplateItem.Invoice_InspectionFee = null;
                            _exportTemplateItem.Invoice_TravelLandFee = null;
                        }
                        else
                        {
                            _exportTemplateItem.AirTicket = invoice?.TravelAirFee;
                            _exportTemplateItem.Invoice_HotelFee = invoice?.HotelFee;
                            _exportTemplateItem.Invoice_TravelFee = invoice?.TravelFee;
                            _exportTemplateItem.UnitPrice = invoice?.UnitPrice;
                            _exportTemplateItem.BillingManDays = invoice?.BillingManDays.GetValueOrDefault();
                            _exportTemplateItem.Invoice_InspectionFee = invoice?.InspFees;
                            _exportTemplateItem.Invoice_TravelLandFee = invoice?.TravelLandFee;
                        }

                        _exportTemplateItem.BookingNo = bookingList.BookingId;
                        _exportTemplateItem.Office = bookingList.Office;
                        _exportTemplateItem.CustomerName = bookingList.CustomerName;
                        _exportTemplateItem.BrandName = string.Join(",", parameterList.CustomerBrandList.Where(x => x.BookingId == bookingList.BookingId).Select(x => x.Name));
                        _exportTemplateItem.DeptCode = string.Join(",", parameterList.CustomerDept.Where(x => x.BookingId == bookingList.BookingId).Select(y => y.Name));
                        _exportTemplateItem.ProductSubCategory = product.ProductSubCategory;
                        _exportTemplateItem.CustomerProductCategory = parameterList.CustomerProductCategoryList.FirstOrDefault(x => x.ParentId == bookingId)?.Name;
                        _exportTemplateItem.CustomerBookingNo = bookingList.CustomerBookingNo;
                        _exportTemplateItem.ProductName = combineId > 0 ? string.Join(",", products.Where(x => x.CombineProductId == combineId).Select(y => y.ProductName)) : product.ProductName;
                        _exportTemplateItem.ReportId = product.ReportId.GetValueOrDefault();
                        _exportTemplateItem.ReportNo = product.ReportNo;
                        _exportTemplateItem.ServiceFromDate = bookingList.ServiceDateFrom.ToString(StandardDateFormat);
                        _exportTemplateItem.ServiceToDate = bookingList.ServiceDateTo.ToString(StandardDateFormat);
                        _exportTemplateItem.Season = bookingList.Season;
                        _exportTemplateItem.SeasonYear = bookingList.SeasonYear;
                        _exportTemplateItem.ProducedQty = product.PresentedQty.GetValueOrDefault();
                        _exportTemplateItem.OrderQty = product.OrderQty.GetValueOrDefault().ToString();
                        _exportTemplateItem.InspectedQty = product.InspectedQty.GetValueOrDefault();
                        _exportTemplateItem.ServiceTypeName = string.Join(",", parameterList.ServiceTypeList.Where(x => x.InspectionId == bookingId).Select(y => y.serviceTypeName));
                        _exportTemplateItem.ProductCategory = product.ProductCategory;
                        _exportTemplateItem.ServiceName = Service_Inspection;
                        _exportTemplateItem.Color = string.Join(",", parameterList.POColorList.Where(x => x.ProductRefId == product.Id).Select(y => y.ColorName));
                        _exportTemplateItem.SupplierName = bookingList.SupplierName;
                        _exportTemplateItem.FactoryName = bookingList.FactoryName;
                        _exportTemplateItem.FactoryCity = parameterList.FactoryLocation.FirstOrDefault(x => x.BookingId == bookingId)?.CityName;
                        _exportTemplateItem.FactoryCountry = parameterList.FactoryLocation.FirstOrDefault(x => x.BookingId == bookingId)?.CountryName;
                        _exportTemplateItem.BookingCreationDate = bookingList.ApplyDate.ToString(StandardDateFormat);
                        _exportTemplateItem.BookingCreatedBy = bookingList.CreatedBy;
                        _exportTemplateItem.InvoiceNumber = invoice?.InvoiceNo;
                        _exportTemplateItem.Invoice_Currency = invoice?.InvoiceCurrency;
                        _exportTemplateItem.BillingMode = invoice?.InvoiceMethod;
                        _exportTemplateItem.InvoiceTo = invoice?.InvoiceTo;
                        _exportTemplateItem.InvoiceContact = invoiceContact;
                        _exportTemplateItem.InvoiceDate = invoice?.InvoiceDate;
                        _exportTemplateItem.ReportStatus = product.ReportStatus;
                        _exportTemplateItem.TotalStaff = "";
                        _exportTemplateItem.bookingStatus = bookingList.StatusName;
                        _exportTemplateItem.InspectionLocation = bookingList.InspectionLocation;
                        _exportTemplateItem.OfficeCountry = parameterList.OfficeCountryList.FirstOrDefault(x => x.ParentId == bookingList.OfficeId)?.Name;
                        _exportTemplateItem.Inspectors = string.Join(",", parameterList.QcNames.Where(x => x.Id == product.ReportId).Select(y => y.Name));

                        result.Add(_exportTemplateItem);
                        perBookingFirstRowIndex++;
                    }
                }
            }

            return result.OrderBy(x => x.BookingNo).ToList();
        }

        public List<CustomerCulturaTemplate> MapCustomerCultura(KPIMapParameters parameterList)
        {
            List<CustomerCulturaTemplate> result = new List<CustomerCulturaTemplate>();
            var bookingIds = parameterList.BookingInvoice.BookingItems?.Select(x => x.BookingId).Distinct().ToList();

            if (bookingIds.Any() && bookingIds != null)
            {
                foreach (var bookingId in bookingIds)
                {
                    if (bookingId > 0)
                    {
                        var booking = parameterList.BookingInvoice.BookingItems?.FirstOrDefault(x => x.BookingId == bookingId);
                        var products = parameterList.ProductList?.Where(x => x.BookingId == bookingId).OrderBy(x => x.CombineProductId).ThenByDescending(x => x.CombineAqlQuantity).ToList();
                        var quotMandayData = parameterList.QuotationDetails?.MandayList?.FirstOrDefault(x => x.BookingId == bookingId);
                        var quotationData = parameterList.QuotationDetails?.QuotDetails?.FirstOrDefault(x => x.Booking.Any(y => y.IdBooking == bookingId));
                        var bookingStatusLog = parameterList.BookingStatusLogs?.FirstOrDefault(x => x.BookingId == bookingId && x.StatusId == booking.StatusId);

                        if (products.Any() && products != null)
                        {
                            foreach (var product in products)
                            {
                                var res = new CustomerCulturaTemplate
                                {
                                    BookingCreationDate = booking?.ApplyDate.ToString(StandardDateFormat),
                                    SupplierName = booking?.SupplierName,
                                    FactoryName = booking?.FactoryName,
                                    PONumber = string.Join(", ", parameterList.PoDetails?.Where(x => x.BookingId == bookingId && x.ProductRefId == product.Id).Select(x => x.PoNumber).Distinct()),
                                    ProductName = product.ProductName,
                                    ServiceTypeName = parameterList.ServiceTypeList != null ? parameterList.ServiceTypeList?.FirstOrDefault(x => x.InspectionId == bookingId)?.serviceTypeName : "",
                                    InspectionStartDate = product?.ServiceStartDate?.ToString(StandardDateFormat),
                                    InspectionEndDate = product?.ServiceEndDate?.ToString(StandardDateFormat),
                                    Party = parameterList.EntityName,
                                    ReportDate = bookingStatusLog?.StatusChangeDate?.ToString(StandardDateFormat),
                                    ReportResult = product?.ReportResult,
                                    QuotationId = quotMandayData?.QuotationId,
                                    ReportTitle = product.ReportNo
                                };
                                if (products.First().ProductId == product.ProductId)
                                {
                                    if (quotationData != null)
                                    {
                                        res.MandayRate = quotationData.UnitPrice;
                                        res.MandayUsed = quotMandayData.Manday;
                                        res.TotalInspectionFee = quotMandayData.TotalPrice;
                                    }

                                    if (quotationData != null)
                                    {
                                        var otherCost = quotationData.TravelCostAir.GetValueOrDefault() + quotationData.TravelCostLand.GetValueOrDefault() + quotationData.HotelCost.GetValueOrDefault() + quotationData.OtherCost.GetValueOrDefault();
                                        res.Fee = otherCost;
                                    }
                                }
                                result.Add(res);
                            }
                        }
                    }
                }

            }

            return result.OrderBy(x => x.InspectionEndDate).ToList();
        }

        public List<ECITemplateResponse> MapECITemplate(ECITemplateParameters parameterList)
        {
            List<ECITemplateResponse> result = new List<ECITemplateResponse>();

            var productlist = parameterList.ProductList?.
                                OrderBy(x => x.CustomerBookingNumber).
                               ThenByDescending(x => x.BookingFormSerial.HasValue).ThenBy(x => x.BookingFormSerial).ToList();

            if (productlist != null && productlist.Any())
            {
                foreach (var item in productlist)
                {
                    if (item.ProductName != null)
                    {
                        int bookingId = item.BookingId;

                        var bookingItem = parameterList.BookingItems?.FirstOrDefault(x => x.BookingId == bookingId);
                        var factoryLocation = parameterList.FactoryLocation?.FirstOrDefault(x => x.BookingId == bookingId);
                        var bookingPO = parameterList.PoDetails?.FirstOrDefault(x => x.BookingId == bookingId && x.ProductRefId == item.Id);
                        var fbDefects = parameterList.FBReportDefectsList?.Where(x => x.FBReportDetailId == item.ReportId && x.ProductId == item.ProductId).ToList();
                        var fbBookingQuantity = parameterList.FbBookingQuantity?.FirstOrDefault(x => x.BookingId == bookingId && x.ProductId.Id == item.ProductId);
                        var customerDept = parameterList.CustomerDept?.FirstOrDefault(x => x.BookingId == bookingId);
                        var customerBuyer = parameterList.CustomerBuyerList?.FirstOrDefault(x => x.BookingId == bookingId);

                        var res = new ECITemplateResponse
                        {
                            InspectionEndDate = bookingItem?.ServiceDateTo,
                            ThirdParty = ThirdParty,
                            BookingNo = bookingId,
                            ProductId = item.ProductId,
                            ReportId = item.ReportId.GetValueOrDefault(),
                            ReportResult = item.ReportResultName,
                            BuyerName = customerDept?.DepartmentCode + " " + customerBuyer?.BuyerName,
                            Division = string.Empty,
                            Brand = string.Empty,
                            Bdm = parameterList.BookingDFDataList?.FirstOrDefault(x => x.ControlConfigId == (int)DynamicFielsCuConfig.BDM && x.BookingNo == bookingId)?.DFValue,
                            Section = string.Empty,
                            Merchandiser = parameterList.CustomerMerchandiserList != null ? string.Join(",", parameterList.CustomerMerchandiserList?.Where(x => x.BookingId == bookingId).Select(x => x.Name).Distinct()) : string.Empty,
                            QcmName = parameterList.BookingDFDataList?.FirstOrDefault(x => x.ControlConfigId == (int)DynamicFielsCuConfig.QCMName && x.BookingNo == bookingId)?.DFValue,
                            SupplierCode = string.Empty,
                            SupplierName = bookingItem?.SupplierName,
                            FactoryName = bookingItem?.FactoryName,
                            FactoryCountry = factoryLocation?.CountryName,
                            EciOffice = parameterList.BookingDFDataList?.FirstOrDefault(x => x.ControlConfigId == (int)DynamicFielsCuConfig.ECIOffice && x.BookingNo == bookingId)?.DFValue,
                            PONumber = bookingPO?.PoNumber,
                            ProductName = item.ProductName,
                            ProductDescription = item.ProductDescription,
                            HardLine = HardLine,
                            ServiceTypeName = parameterList.ServiceTypeList?.FirstOrDefault(x => x.InspectionId == bookingId)?.serviceTypeName,
                            OrderQty = item?.BookingQuantity,
                            ShipmentQty = fbBookingQuantity?.ShipmentQty,
                            OverOrShortQty = string.Empty,
                            OverQty = string.Empty,
                            ShortQty = string.Empty,
                            CriticalDefect = fbDefects?.GroupBy(x => x.DefectDesc).Sum(x => x.FirstOrDefault().Critical),
                            MajorDefect = fbDefects?.GroupBy(x => x.DefectDesc).Sum(x => x.FirstOrDefault().Major),
                            MinorDefect = fbDefects?.GroupBy(x => x.DefectDesc).Sum(x => x.FirstOrDefault().Minor),
                            OrdnanceDeFabrication = string.Empty,
                            ShortDescription = string.Empty,
                            Season = string.Empty,
                            IRCode = string.Empty
                        };
                        result.Add(res);
                    }
                }

                return result;
            }
            else
            {
                return result;
            }
        }


        public List<ScheduleAnalysisTemplate> MapScheduleAnalysis(KPIMapParameters parameterList)
        {
            List<ScheduleAnalysisTemplate> result = new List<ScheduleAnalysisTemplate>();
            List<int> bookingIds = parameterList.BookingInvoice.BookingItems?.Select(x => x.BookingId).Distinct().ToList();

            if (bookingIds != null)
            {
                foreach (var bookingId in bookingIds)
                {
                    if (bookingId > 0)
                    {
                        var booking = parameterList.BookingInvoice.BookingItems?.FirstOrDefault(x => x.BookingId == bookingId);
                        var products = parameterList.ProductList?.Where(x => x.BookingId == bookingId).OrderBy(x => x.BookingId).ThenBy(x => x.CombineProductId).ToList();
                        var invoice = parameterList.BookingInvoice?.InvoiceBookingData?.FirstOrDefault(x => x.BookingId == bookingId);
                        var extraFeeData = parameterList.ExtraFeeData?.FirstOrDefault(x => x.BookingId == bookingId);
                        var factoryLocation = parameterList.FactoryLocation?.FirstOrDefault(x => x.BookingId == bookingId);
                        var quotDetails = parameterList.QuotationDetails?.QuotDetails?.FirstOrDefault(x => x.Booking.Any(y => y.IdBooking == bookingId));
                        var expenseData = parameterList.ExpenseData?.Where(x => x.InspectionId == bookingId).ToList();
                        var brands = parameterList?.CustomerBrandList != null ? string.Join(", ", parameterList?.CustomerBrandList?.Where(x => x.BookingId == booking.BookingId).Select(x => x.Name).Distinct()) : string.Empty;
                        var customerDept = parameterList.CustomerDept != null ? string.Join(", ", parameterList.CustomerDept?.Where(x => x.BookingId == booking.BookingId).Select(y => y.Name).Distinct()) : string.Empty;
                        var serviceType = parameterList.ServiceTypeList?.FirstOrDefault(x => x.InspectionId == bookingId);
                        var scheduleQc = parameterList.ScheduleQcList?.Where(x => x.BookingId == booking.BookingId).ToList();
                        var mandayList = parameterList.QuotationDetails?.MandayList?.FirstOrDefault(x => x.BookingId == bookingId);

                        foreach (var product in products)
                        {
                            var color = parameterList.POColorList != null ? string.Join(", ", parameterList.POColorList?.Where(x => x.ProductRefId == product.Id)?.Select(y => y.ColorName).Distinct()) : string.Empty;
                            var customerResult = parameterList.CustomerDecisionData?.FirstOrDefault(x => x.Id == product.ReportId)?.Name;

                            var res = new ScheduleAnalysisTemplate
                            {
                                BookingNo = booking.BookingId,
                                CustomerBookingNo = booking.CustomerBookingNo,
                                CustomerName = booking.CustomerName,
                                BrandName = brands,
                                DeptCode = customerDept,
                                SupplierName = booking.SupplierName,
                                FactoryName = booking.FactoryName,
                                FactoryCity = factoryLocation?.CityName,
                                FactoryProvince = factoryLocation?.ProvinceName,
                                FactoryCountry = factoryLocation?.CountryName,
                                Office = booking.Office,
                                BookingCreationDate = booking.ApplyDate.ToString(StandardDateFormat),
                                Season = booking.SeasonYear != null ? booking.Season + " / " + booking.SeasonYear : booking.Season,
                                ServiceTypeName = serviceType?.serviceTypeName,
                                ProductRef = product.ProductName,
                                ProductCategory = product.ProductCategory,
                                ProductName = product.ProductSubCategory2,
                                Color = color,
                                OrderQty = product.BookingQuantity.GetValueOrDefault(),
                                InspectedQty = product.InspectedQty.GetValueOrDefault(),
                                ProducedQty = product.PresentedQty.GetValueOrDefault(),
                                ReportResult = product.ReportResult,
                                ClientResult = customerResult,
                                Inspectors = scheduleQc != null ? string.Join(", ", scheduleQc.Where(x => x.QCType == (int)QCType.QC).Select(x => x.Name).Distinct()) : string.Empty,
                                InspectorCount = scheduleQc != null ? scheduleQc.Where(x => x.QCType == (int)QCType.QC).Select(x => x.Name).Distinct().Count() : 0,
                                AdditionalInspectors = scheduleQc != null ? string.Join(", ", scheduleQc.Where(x => x.QCType == (int)QCType.AdditionalQC)
                                .Select(x => x.Name).Distinct()) : string.Empty,
                                ExpenseClaimNo = expenseData != null ? string.Join(", ", expenseData.Select(x => x.ClaimNo).Distinct()) : string.Empty,
                                CurrencyName = quotDetails?.CurrencyName,
                                InvoiceDate = invoice?.InvoiceDate,
                                BookingStatus = booking.StatusName,
                                InspectionLocation = booking.InspectionLocation,
                                InvoiceTo = invoice?.InvoiceTo,
                                InvoiceNumber = invoice?.InvoiceNo,
                                //Invoice_TotalFee = invoice?.InvoiceTotal,
                                Invoice_Currency = invoice?.InvoiceCurrency,
                                Invoice_PaymentStatus = invoice != null ? Enum.GetName(typeof(InvoicePaymentStatus), invoice.InvoicePaymentStatus) : string.Empty,
                                ExtraFeeBilledName = extraFeeData?.BilledName,
                                ExtraFeeBilledTo = extraFeeData?.BilledToName,
                                ExtraFeeInvoiceNumber = extraFeeData?.ExtraFeeInvoiceNo,
                                ExtraFeeInvoice_Currency = extraFeeData?.CurrencyName,
                                ExtraFeeInvoice_PaymentStatus = extraFeeData != null ? Enum.GetName(typeof(InvoicePaymentStatus), extraFeeData.PaymentStatus) : string.Empty,
                                QuotationNumber = quotDetails?.QuotationId,
                                //QuotationPrice = quotDetails?.QuotationPrice,
                                EstimatedManDay = mandayList?.Manday,
                                ActualManDay = scheduleQc?.Select(x => x.ActualManDay).FirstOrDefault(),
                                BookingType = booking?.BookingType,
                                SupplierGrade = parameterList?.SupplierGrades.Where(x => x.CustomerId == booking.CustomerId && x.SupplierId == booking.SupplierId && x.PeriodFrom.Date <= booking.ServiceDateTo.Date && booking.ServiceDateTo.Date <= x.PeriodTo.Date).Select(x => !string.IsNullOrEmpty(x.CustomName) ? x.CustomName : x.Level).FirstOrDefault(),
                                FactoryGrade = parameterList?.FactoryGrades.Where(x => x.CustomerId == booking.CustomerId && x.SupplierId == booking.FactoryId && x.PeriodFrom.Date <= booking.ServiceDateTo.Date && booking.ServiceDateTo.Date <= x.PeriodTo.Date).Select(x => !string.IsNullOrEmpty(x.CustomName) ? x.CustomName : x.Level).FirstOrDefault(),
                                GapExternalReportNo = product?.ExternalReportNo,
                                PONumber = string.Join(", ", parameterList.PoDetails?.Where(x => x.BookingId == bookingId && x.ProductRefId == product.Id).Select(x => x.PoNumber).Distinct().ToList()),
                                InvoiceBilledTo = invoice?.BilledToName,
                                GapInspectionPlatform = parameterList?.BookingDFDataList?.FirstOrDefault(x => x.ControlConfigId == (int)DynamicFielsCuConfig.InspectionPlatform && x.BookingNo == bookingId)?.DFValue,
                                GapPaymentOptions = booking?.GapPaymentOption ?? "",
                                SampleSize = product.CombineProductId > 0 ? product.CombineAqlQuantity.Value : product.AqlQty,
                                ShIpDateFrom = booking.ServiceDateFrom,
                                ShIpDateTo = booking.ServiceDateTo
                            };

                            if (product.ServiceStartDate?.ToString(StandardDateFormat) != product.ServiceEndDate?.ToString(StandardDateFormat))
                            {
                                res.ActualShIpDateFromAndTo = product.ServiceStartDate?.ToString(StandardDateFormat) + " / " + product.ServiceEndDate?.ToString(StandardDateFormat);
                            }
                            else
                            {
                                res.ActualShIpDateFromAndTo = product.ServiceStartDate?.ToString(StandardDateFormat);
                            }
                            if (!result.Any(x => x.BookingNo == product.BookingId))
                            {
                                res.Invoice_TotalFee = invoice?.InvoiceTotal;
                                res.QuotationPrice = parameterList.QuotationDetails?.QuotDetails?.SelectMany(x => x.Booking)?.FirstOrDefault(x => x.IdBooking == bookingId)?.TotalCost;
                                res.ExtraFeeInvoice_TotalFee = extraFeeData?.ExtraFee;
                                res.ExpenseClaimAmount = expenseData?.Sum(x => x.AmmountHk);
                            }

                            result.Add(res);
                        }
                    }
                }
            }

            return result;
        }
        public List<CustomerMandayData> MapCustomerManday(List<KpiInspectionBookingItems> bookings, List<BookingShipment> quantityDetailFromReports, List<InspectionMandayDashboard> actualManday,
            List<InspectionMandayDashboard> estimatedManday, List<InspectionProductCountDto> productCount, List<SupplierAddressData> addresses,
            List<BookingServiceType> bookingServiceTypes)
        {
            var result = new List<CustomerMandayData>();
            if (bookings != null && bookings.Any())
            {
                foreach (var booking in bookings)
                {
                    var factoryAddress = addresses.FirstOrDefault(x => x.SupplierId == booking.FactoryId);
                    var reportQuantity = quantityDetailFromReports.Where(x => x.BookingId == booking.BookingId).ToList();
                    var serviceType = bookingServiceTypes.FirstOrDefault(x => x.BookingNo == booking.BookingId);
                    var customerMandayData = new CustomerMandayData()
                    {
                        CustomerId = booking.CustomerId.GetValueOrDefault(),
                        CustomerName = booking.CustomerName,
                        FactoryCity = factoryAddress?.CityName,
                        FactoryCountry = factoryAddress?.CountryName,
                        FactoryProvince = factoryAddress.ProvinceName,
                        ServiceType = serviceType?.ServiceTypeName,
                        ServiceTypeId = serviceType?.ServiceTypeId ?? 0,
                        ActualManday = actualManday.Where(x => x.InspectionId == booking.BookingId).Sum(y => y.ActualMandayCount),
                        EstimatedManday = estimatedManday.Where(x => x.InspectionId == booking.BookingId).Sum(y => y.MandayCount),
                        InspectedQty = reportQuantity.Sum(x => x.InspectedQty),
                        OrderQty = reportQuantity.Sum(x => x.OrderQty),
                        PresentedQty = reportQuantity.Sum(x => x.PresentedQty),
                        ProductCount = productCount.FirstOrDefault(x => x.InspectionId == booking.BookingId)?.ProductCount ?? 0,
                        InspectionId = booking.BookingId,
                        ReportCount = reportQuantity.Select(y => y.ReportId).Distinct().Count(),
                    };

                    result.Add(customerMandayData);
                }
            }
            return result;
        }

        public List<InspectionSummaryQCTemplate> MapInspectionSummaryQC(List<ScheduleStaffItem> qcList,
            List<KpiInspectionBookingItems> bookingItems,
            List<ServiceTypeList> serviceTypeList,
            List<FactoryCountry> factoryLocationData,
            List<InspectionQcKpiExpenseDetails> expenseList,
            List<InspectionQcKpiInvoiceDetails> invoiceList,
            List<KpiExtraFeeData> extraFeeDataList)
        {
            var result = new List<InspectionSummaryQCTemplate>();
            var landExpenseTypes = new List<int>() {
                    (int)ClaimExpenseType.TravellingByFerry ,
                    (int)ClaimExpenseType.TravellingByBus ,
                    (int)ClaimExpenseType.TravellingByTrain,
                    (int)ClaimExpenseType.TravellingByTaxi,
                    (int)ClaimExpenseType.TravellingOtherModes };

            var allExpenseTypes = new List<int>() {
                    (int)ClaimExpenseType.MandayCost,
                    (int)ClaimExpenseType.TravellingByPlane,
                    (int)ClaimExpenseType.TravellingByFerry ,
                    (int)ClaimExpenseType.TravellingByBus ,
                    (int)ClaimExpenseType.TravellingByTrain,
                    (int)ClaimExpenseType.TravellingByTaxi,
                    (int)ClaimExpenseType.TravellingOtherModes,
                    (int)ClaimExpenseType.FoodAllowance,
                    (int)ClaimExpenseType.HotelExpenses};
            foreach (var qc in qcList)
            {
                var booking = bookingItems?.FirstOrDefault(x => x.BookingId == qc.BookingId);
                var factoryLocation = factoryLocationData?.FirstOrDefault(x => x.BookingId == qc.BookingId);
                var serviceType = serviceTypeList?.FirstOrDefault(x => x.InspectionId == qc.BookingId);

                var expenseData = expenseList.FirstOrDefault(x => x.QcId == qc.Id && x.InspectionId == qc.BookingId);
                var invoiceData = invoiceList.FirstOrDefault(x => x.InspectionId == qc.BookingId);

                var expenseListByQc = expenseList.Where(x => x.QcId == qc.Id && x.InspectionId == qc.BookingId).ToList();

                var inspectionFees = (expenseListByQc.Where(x => x.ExpenseType == (int)ClaimExpenseType.MandayCost).Select(x => x.ClaimAmount).Sum()
                                     - expenseListByQc.Where(x => x.ExpenseType == (int)ClaimExpenseType.MandayCost).Select(x => x.ServiceTax.GetValueOrDefault()).Sum());
                var air = expenseListByQc.Where(x => x.ExpenseType == (int)ClaimExpenseType.TravellingByPlane).Select(x => x.ClaimAmount).Sum() -
                    expenseListByQc.Where(x => x.ExpenseType == (int)ClaimExpenseType.TravellingByPlane).Select(x => x.ServiceTax.GetValueOrDefault()).Sum();
                var land = expenseListByQc.Where(x => landExpenseTypes.Contains(x.ExpenseType)).Select(x => x.ClaimAmount).Sum() -
                    expenseListByQc.Where(x => landExpenseTypes.Contains(x.ExpenseType)).Select(x => x.ServiceTax.GetValueOrDefault()).Sum();
                var food = expenseListByQc.Where(x => x.ExpenseType == (int)ClaimExpenseType.FoodAllowance).Select(x => x.ClaimAmount).Sum() -
                    expenseListByQc.Where(x => x.ExpenseType == (int)ClaimExpenseType.FoodAllowance).Select(x => x.ServiceTax.GetValueOrDefault()).Sum();
                var hotel = expenseListByQc.Where(x => x.ExpenseType == (int)ClaimExpenseType.HotelExpenses).Select(x => x.ClaimAmount).Sum()
                    - expenseListByQc.Where(x => x.ExpenseType == (int)ClaimExpenseType.HotelExpenses).Select(x => x.ServiceTax.GetValueOrDefault()).Sum();
                var other = expenseListByQc.Where(x => !allExpenseTypes.Contains(x.ExpenseType)).Select(x => x.ClaimAmount).Sum() -
                    expenseListByQc.Where(x => !allExpenseTypes.Contains(x.ExpenseType)).Select(x => x.ServiceTax.GetValueOrDefault()).Sum();
                var serviceTax = expenseListByQc.Select(x => x.ServiceTax.GetValueOrDefault()).Sum();
                var claimTotal = inspectionFees + air + land + food + hotel + other + serviceTax;

                var inspectionQcSummaryTemplate = new InspectionSummaryQCTemplate
                {
                    Office = booking?.Office,
                    BookingNo = booking?.BookingId,
                    ServiceDateFrom = booking?.ServiceDateFrom,
                    ServiceDateTo = booking?.ServiceDateTo,
                    ServiceTypeName = serviceType?.serviceTypeName,
                    ScheduleDate = qc.ServiceDate,
                    QCName = qc.Name,
                    CustomerName = booking?.CustomerName,
                    SupplierName = booking?.SupplierName,
                    FactoryName = booking?.FactoryName,
                    FactoryCountry = factoryLocation?.CountryName,
                    EmployeeTypeName = qc.EmployeeTypeName,
                    OrderStatus = booking?.StatusName,
                    OutsourceCompany = expenseData?.OutsourceCompany,
                    PayrollCurrency = qc.PayrollCurrency,
                    FactoryCity = factoryLocation?.CityName,
                    FactoryTown = factoryLocation?.TownName,
                    ClaimStatus = string.Join(", ", expenseListByQc.GroupBy(x => new { x.ClaimNumber, x.ClaimStatus }).Select(x => x.Key.ClaimStatus).ToList()),
                    ClaimRemarks = string.Join(", ", expenseListByQc.Where(x => !string.IsNullOrWhiteSpace(x.ClaimRemarks)).Select(x => x.ClaimRemarks).Distinct().ToList()),
                    ClaimNumber = string.Join(", ", expenseListByQc.Select(x => x.ClaimNumber).Distinct().ToList()),
                    ClaimDate = expenseData?.ClaimDate,
                    StartPort = qc.StartPortName,
                    //StartCity = expenseData?.StartCity,
                    //EndCity = expenseData?.EndCity
                };

                //for only showing one time claim amounts in excel other time we are showing empty row, based on booking id and qc name
                if (!result.Any(x => x.BookingNo == qc.BookingId && x.QCName == qc.Name))
                {

                    inspectionQcSummaryTemplate.NumberOfManDay = expenseListByQc.Sum(x => x.NumberOfManDay);
                    inspectionQcSummaryTemplate.InspectionFees = inspectionFees;
                    inspectionQcSummaryTemplate.Air = air;
                    inspectionQcSummaryTemplate.Land = land;
                    inspectionQcSummaryTemplate.Food = food;
                    inspectionQcSummaryTemplate.Hotel = hotel;
                    inspectionQcSummaryTemplate.ServiceTax = serviceTax;
                    inspectionQcSummaryTemplate.ClaimTotal = claimTotal;
                    inspectionQcSummaryTemplate.Other = other;
                }

                if (invoiceData != null)
                    inspectionQcSummaryTemplate.InvoiceNo = invoiceData.InvoiceNo;

                var extraFeeData = extraFeeDataList.Where(x => x.BookingId == qc.BookingId).ToList();

                if (extraFeeData.Any())
                {
                    inspectionQcSummaryTemplate.ExtraFeeInvoiceNo = string.Join(", ", extraFeeData.Where(x => !string.IsNullOrWhiteSpace(x.ExtraFeeInvoiceNo)).Select(x => x.ExtraFeeInvoiceNo).ToList());
                    inspectionQcSummaryTemplate.ExtraFeeInvoiceStatus = string.Join(", ", extraFeeData.Where(x => !string.IsNullOrWhiteSpace(x.ExtraFeeStatus)).Select(x => x.ExtraFeeStatus).ToList());
                }

                // Invoice details - add only for first row.
                if (result.Count(x => x.BookingNo == qc.BookingId) == 0)
                {
                    if (invoiceData != null)
                    {
                        inspectionQcSummaryTemplate.InvoiceCurrency = invoiceData.InvoiceCurrency;
                        inspectionQcSummaryTemplate.InvoiceInspectionFees = invoiceData.InvoiceInspectionFees;
                        inspectionQcSummaryTemplate.InvoiceTravellingFees = invoiceData.InvoiceTravellingFees;
                        inspectionQcSummaryTemplate.InvoiceOtherFees = invoiceData.InvoiceOtherFees + invoiceData.InvoiceHotelFees;
                        inspectionQcSummaryTemplate.InvoiceTotalTax = invoiceData.InvoiceTotalTax;
                        inspectionQcSummaryTemplate.InvoiceTotalFees = invoiceData.InvoiceTotalFees;
                        inspectionQcSummaryTemplate.InvoiceManDay = invoiceData.InvoiceManDay;
                    }
                    if (extraFeeData.Any())
                    {
                        inspectionQcSummaryTemplate.ExtraFeeCurrency = string.Join(", ", extraFeeData.Where(x => !string.IsNullOrWhiteSpace(x.CurrencyName)).Select(x => x.CurrencyName).ToList());
                        inspectionQcSummaryTemplate.TotalExtraFees = extraFeeData.Select(x => x.ExtraFee).Sum();
                    }
                }





                result.Add(inspectionQcSummaryTemplate);
            }
            return result.OrderBy(x => x.ScheduleDate).ThenBy(x => x.QCName).ToList();
        }

        public List<ExportGapCustomerProductRef> MapGapCustomerProductRefLevel(List<KpiInspectionBookingItems> inspectionBookingItems,
            IEnumerable<BookingQuantityData> inspProducts, List<BookingShipment> reportQuantities, List<GapCustomerKpiReportData> reportData, List<FBReportDefects> fBReportDefects,
            List<ScheduleStaffItem> scheduleStaffItems, List<BookingBrandAccess> brands, List<BookingServiceType> bookingServiceTypes, List<FbReportQualityPlan> fbReportQualityPlans,
            List<SupplierCode> supplierCodes, List<SupplierCode> factoryCodes, List<SupplierAddressData> factoryAddresses, List<ParentDataSource> officeCountries, List<InspectionPoNumberList> inspectionPoNumberLists,
            List<GapKpiFbReportPackingPackagingLabellingProduct> reportPackingPackagingLabellingProducts)

        {
            var result = new List<ExportGapCustomerProductRef>();
            foreach (var item in inspProducts)
            {
                var booking = inspectionBookingItems.FirstOrDefault(x => x.BookingId == item.BookingId);
                var report = reportData.FirstOrDefault(x => x.InspectionId == item.BookingId && x.ReportId == item.FbReportId);
                var reportQty = reportQuantities.Where(x => x.ReportId == item.FbReportId).ToList();
                var defects = fBReportDefects.Where(x => x.FBReportDetailId == item.FbReportId).ToList();
                var criticalDefectVisual = defects.Where(x => x.DefectCheckpoint == (int)FbReportPackageType.Workmanship && x.Critical.HasValue).Sum(x => x.Critical);
                var majorDefectVisual = defects.Where(x => x.DefectCheckpoint == (int)FbReportPackageType.Workmanship && x.Major.HasValue).Sum(x => x.Major);
                var minorDefectVisual = defects.Where(x => x.DefectCheckpoint == (int)FbReportPackageType.Workmanship && x.Major.HasValue).Sum(x => x.Minor);
                var reportQualityPlan = fbReportQualityPlans.Where(x => x.FbReportDetailsId == item.FbReportId).ToList();
                var data = new ExportGapCustomerProductRef()
                {
                    InspectionId = item.BookingId,
                    ProductRef = item.ProductName,
                    PoNumber = string.Join(", ", inspectionPoNumberLists.Where(x => x.InspectionId == item.BookingId && x.ProductRefId == item.ProductRefId).Select(x => x.PoNumber).Distinct().ToList()),
                    Supplier = booking?.SupplierName,
                    SupplierCode = supplierCodes?.FirstOrDefault(x => x.SupplierId == booking.SupplierId && x.CustomerId == booking.CustomerId)?.Code ?? "",
                    Factory = booking?.FactoryName,
                    FactoryCode = factoryCodes?.FirstOrDefault(x => x.SupplierId == booking.FactoryId && x.CustomerId == booking.CustomerId)?.Code ?? "",
                    ServiceType = bookingServiceTypes?.FirstOrDefault(x => x.BookingNo == item.BookingId)?.ServiceTypeName ?? "",
                    InspectionStartedDateTime = report != null && report.InspectionStartedDate.HasValue && !string.IsNullOrWhiteSpace(report.InspectionStartTime) ? report?.InspectionStartedDate?.ToString(StandardDateFormat) + ", " + report.InspectionStartTime : "",
                    InspectionSubmittedDateTime = report != null && report.InspectionSubmittedDate.HasValue && !string.IsNullOrWhiteSpace(report.InspectionEndTime) ? report?.InspectionSubmittedDate?.ToString(StandardDateFormat) + ", " + report.InspectionEndTime : "",
                    ReportResult = report?.ReportResult,
                    OrderQty = reportQty.Where(x => x.OrderQty.HasValue).Sum(x => x.OrderQty),
                    PresentedQty = reportQty.Where(x => x.PresentedQty.HasValue).Sum(x => x.PresentedQty),
                    InspectedQty = reportQty.Where(x => x.InspectedQty.HasValue).Sum(x => x.InspectedQty),
                    CriticalVisualDefects = criticalDefectVisual,
                    MajorVisualDefects = majorDefectVisual,
                    MinorVisualDefects = minorDefectVisual,
                    TotalVisualDefects = criticalDefectVisual + majorDefectVisual + minorDefectVisual,
                    TotalDectiveUnits = reportQualityPlan.Sum(x => x.TotalDefectiveUnits),
                    ActualMeasuredSamplesize = reportQualityPlan.Where(x => !string.IsNullOrWhiteSpace(x.ActualMeasuredSampleSize)).Select(y => int.TryParse(y.ActualMeasuredSampleSize, out int sampleSize) ? sampleSize : 0).Sum(),
                    VisualSampleSize = reportPackingPackagingLabellingProducts.Where(x => x.FbReportDetailId == item.FbReportId && x.PackingType == (int)FbReportPackageType.Workmanship).Sum(x => x.SampleSize),
                    MeasurementDefectDetails = string.Join(", ", defects.Where(x => x.DefectCheckpoint == (int)FbReportPackageType.Measurement && x.Major > 0).Select(x => x.DefectCode + "-" + x.DefectDesc + "-" + x.Major).ToList()),//when we save the report data defect checkpoint 3 save with only major
                    CriticalDefectsVisual = string.Join(", ", defects.Where(x => x.Critical > 0 && x.DefectCheckpoint == (int)FbReportPackageType.Workmanship).Select(x => x.DefectCode + "-" + x.DefectDesc + "-" + x.Critical).ToList()),
                    MajorDefectsVisual = string.Join(", ", defects.Where(x => x.Major > 0 && x.DefectCheckpoint == (int)FbReportPackageType.Workmanship).Select(x => x.DefectCode + "-" + x.DefectDesc + "-" + x.Major).ToList()),
                    MinorDefectsVisual = string.Join(", ", defects.Where(x => x.Minor > 0 && x.DefectCheckpoint == (int)FbReportPackageType.Workmanship).Select(x => x.DefectCode + "-" + x.DefectDesc + "-" + x.Minor).ToList()),
                    FactoryCountry = factoryAddresses.FirstOrDefault(x => x.SupplierId == booking.FactoryId)?.CountryName,
                    Region = report?.Region,
                    OfficeCountry = officeCountries.FirstOrDefault(x => x.ParentId == booking.OfficeId)?.Name ?? "",
                    Office = booking.Office,
                    Season = !string.IsNullOrWhiteSpace(booking.Season) && booking.SeasonYear.HasValue ? booking.Season + "-" + booking.SeasonYear : "",
                    ProductFamily = booking?.CustomerProductCategory, //Fb Report Details table Product Category
                    ProductCategory = item.ProductCategory, // Product table REF_ProductCategory
                    ProductName = item.ProductSub2Category,
                    InspectionStatus = booking.StatusName,
                    KeyStyleHighRisk = report?.KeyStyleHighRisk,
                    Brand = string.Join(", ", brands.Where(x => x.BookingId == item.BookingId).Select(x => x.BrandName).ToList()),
                    DACorrelationDone = report?.DACorrelationDone,
                    FactoryTourDone = report?.FactoryTourDone,
                    DACCorrelactionRate = report?.DACorrelationRate,
                    DAEmail = report?.DACorrelationEmail,
                    QcNames = string.Join(", ", scheduleStaffItems.Where(x => x.BookingId == item.BookingId).Select(y => y.Name).ToList()),
                    ReportId = item.FbReportId,
                    ExternalReportNumber = report?.ExternalReportNumber
                };
                result.Add(data);
            }

            return result;
        }

        /// <summary>
        /// Map the AR Follow up report
        /// </summary>
        /// <param name="bookingInspections"></param>
        /// <param name="quotationDetails"></param>
        /// <param name="poDetails"></param>
        /// <param name="factoryAddressList"></param>
        /// <param name="bookingBrands"></param>
        /// <param name="bookingDepartments"></param>
        /// <param name="bookingServiceTypes"></param>
        /// <param name="invoiceDataList"></param>
        /// <param name="invoiceCreditData"></param>
        /// <param name="extraFeeDataList"></param>
        /// <param name="usdConversionData"></param>
        /// <param name="extraFeeUsdConversionData"></param>
        /// <param name="entityName"></param>
        /// <param name="invoiceCommunications"></param>
        /// <returns></returns>
        public List<ExportARFollowUpReport> MapARFollowUpReport(List<KpiInspectionBookingItems> bookingInspections, List<CarrefourQuoationDetails> quotationDetails,
            List<KpiPoDetails> poDetails, List<SupplierAddressData> factoryAddressList, List<BookingBrandAccess> bookingBrands, List<BookingCustomerDepartment> bookingDepartments,
            List<BookingServiceType> bookingServiceTypes, List<KpiInvoiceData> invoiceDataList, List<InvoiceCreditDetails> invoiceCreditData,
            List<KpiExtraFeeData> extraFeeDataList, List<ExchangeCurrencyItem> usdConversionData, List<ExchangeCurrencyItem> extraFeeUsdConversionData, string entityName,
            List<InvoiceCommunication> invoiceCommunications, List<KpiInspectionBookingItems> extraFeeBookingInspections, List<CarrefourQuoationDetails> extraFeeQuotationDetails,
            List<KpiPoDetails> extraFeePoDetails, List<SupplierAddressData> extraFeeFactoryAddressList, List<BookingBrandAccess> extraFeeBookingBrands, List<BookingCustomerDepartment> extraFeeBookingDepartments,
            List<BookingServiceType> extraFeeBookingServiceTypes)
        {
            List<ExportARFollowUpReport> reportList = new List<ExportARFollowUpReport>();

            foreach (var invoiceData in invoiceDataList)
            {
                var bookingData = bookingInspections.Where(x => x.BookingId == invoiceData.BookingId).FirstOrDefault();

                var factoryAddressData = factoryAddressList.Where(x => x.SupplierId == bookingData.FactoryId).FirstOrDefault();

                var exchangeCurrencyItem = usdConversionData.Where(x => x.Id == bookingData.BookingId).FirstOrDefault();

                var quotationData = quotationDetails.Where(x => x.BookingId == bookingData.BookingId).FirstOrDefault();

                ExportARFollowUpReport exportARFollowUpReport = new ExportARFollowUpReport();

                //map the booking data for ar follow up report
                MapARFollowUpBookingData(exportARFollowUpReport, bookingData, poDetails, factoryAddressData, bookingServiceTypes, bookingBrands, bookingDepartments, quotationData, entityName);

                //map the ar follow up invoice data
                MapARFollowUpInvoiceData(exportARFollowUpReport, invoiceData, invoiceCommunications, exchangeCurrencyItem, invoiceCreditData);

                reportList.Add(exportARFollowUpReport);
            }

            //loop through the extra fee data
            foreach (var extraFeeData in extraFeeDataList)
            {
                var bookingItem = extraFeeBookingInspections.Where(x => x.BookingId == extraFeeData.BookingId).FirstOrDefault();

                var quoationData = extraFeeQuotationDetails.Where(x => x.BookingId == bookingItem.BookingId).FirstOrDefault();

                var factoryAddressData = extraFeeFactoryAddressList.Where(x => x.SupplierId == bookingItem.FactoryId).FirstOrDefault();

                var exchangeCurrencyItem = extraFeeUsdConversionData.Where(x => x.Id == bookingItem.BookingId).FirstOrDefault();

                ExportARFollowUpReport exportARFollowUpReport = new ExportARFollowUpReport();

                //map the booking data for ar follow up report
                MapARFollowUpBookingData(exportARFollowUpReport, bookingItem, extraFeePoDetails, factoryAddressData, extraFeeBookingServiceTypes, extraFeeBookingBrands, extraFeeBookingDepartments, quoationData, entityName);

                //map the extra fee data
                MapExtraFeeData(exportARFollowUpReport, extraFeeData, exchangeCurrencyItem);

                reportList.Add(exportARFollowUpReport);

            }
            return reportList;
        }

        /// <summary>
        /// Map the AR Follow up booking data
        /// </summary>
        /// <param name="exportARFollowUpReport"></param>
        /// <param name="bookingItem"></param>
        /// <param name="poDetails"></param>
        /// <param name="factoryAddressData"></param>
        /// <param name="bookingServiceTypes"></param>
        /// <param name="bookingBrands"></param>
        /// <param name="bookingDepartments"></param>
        /// <param name="quoationData"></param>
        /// <param name="entityName"></param>
        private void MapARFollowUpBookingData(ExportARFollowUpReport exportARFollowUpReport,
            KpiInspectionBookingItems bookingItem, List<KpiPoDetails> poDetails,
            SupplierAddressData factoryAddressData, List<BookingServiceType> bookingServiceTypes,
            List<BookingBrandAccess> bookingBrands, List<BookingCustomerDepartment> bookingDepartments,
            CarrefourQuoationDetails quoationData, string entityName)
        {
            exportARFollowUpReport.BookingNo = bookingItem.BookingId;
            exportARFollowUpReport.CustomerName = bookingItem.CustomerName;
            exportARFollowUpReport.Supplier = bookingItem.SupplierName;
            exportARFollowUpReport.Factory = bookingItem.FactoryName;
            exportARFollowUpReport.ServiceFromDate = bookingItem.ServiceDateFrom.ToString(StandardDateFormat);
            exportARFollowUpReport.ServiceToDate = bookingItem.ServiceDateTo.ToString(StandardDateFormat);
            exportARFollowUpReport.Office = bookingItem.Office;
            //exportARFollowUpReport.Company = entityName;

            exportARFollowUpReport.PoNo = string.Join(",", poDetails.Where(x => x.BookingId == bookingItem.BookingId).Select(x => x.PoNumber).ToList());

            exportARFollowUpReport.City = factoryAddressData?.CityName;
            exportARFollowUpReport.FactoryCountry = factoryAddressData?.CountryName;
            exportARFollowUpReport.ServiceName = KpiARFollowUpReportConstantList.GetValueOrDefault((int)KpiARFollowUpReportConstant.Inspection, "");

            exportARFollowUpReport.ServiceType = bookingServiceTypes.Where(x => x.BookingNo == bookingItem.BookingId).FirstOrDefault()?.ServiceTypeName;

            exportARFollowUpReport.Department = string.Join(",", bookingDepartments.Where(x => x.BookingId == bookingItem.BookingId).Select(x => x.Name).ToList());
            exportARFollowUpReport.Brand = string.Join(",", bookingBrands.Where(x => x.BookingId == bookingItem.BookingId).Select(x => x.BrandName).ToList());
            exportARFollowUpReport.QuotationNo = quoationData?.QuotationId;
        }

        /// <summary>
        /// Map the ar follow up invoice data
        /// </summary>
        /// <param name="exportARFollowUpReport"></param>
        /// <param name="invoiceData"></param>
        /// <param name="invoiceCommunications"></param>
        /// <param name="exchangeCurrencyItem"></param>
        private void MapARFollowUpInvoiceData(ExportARFollowUpReport exportARFollowUpReport,
            KpiInvoiceData invoiceData, List<InvoiceCommunication> invoiceCommunications, ExchangeCurrencyItem exchangeCurrencyItem,
            List<InvoiceCreditDetails> invoiceCreditData)
        {
            if (invoiceData != null)
            {
                exportARFollowUpReport.InvoiceType = invoiceData.InvoiceType;
                exportARFollowUpReport.InvoiceDate = invoiceData.InvoiceDate?.ToString(StandardDateFormat);
                exportARFollowUpReport.InvoiceNo = invoiceData.InvoiceNo;
                exportARFollowUpReport.InvoiceAmount = invoiceData.TotalFee;
                exportARFollowUpReport.Currency = invoiceData.CurrencyName;
                exportARFollowUpReport.Company = invoiceData.InvocieBillingEntity;
                exportARFollowUpReport.InvoiceBilledTo = invoiceData.BilledName;
                exportARFollowUpReport.PaymentTerms = invoiceData.PaymentTerms;
                exportARFollowUpReport.InvoiceStatus = invoiceData.InvoiceStatus;
                exportARFollowUpReport.PaymentStatus = invoiceData.PaymentStatusName;
                exportARFollowUpReport.InvoiceBilledAddress = invoiceData.BilledAddress;
                exportARFollowUpReport.InvoiceBilledToName = invoiceData.BilledToName;
                exportARFollowUpReport.CreditNo = string.Join(",", invoiceCreditData.Where(x => x.BookingId == invoiceData.BookingId).Select(x => x.CreditNumber).ToList());

                exportARFollowUpReport.PaymentDate = invoiceData.PaymentDate?.ToString(StandardDateFormat);

                CalculateDueDateAndUpdateDSO(exportARFollowUpReport, invoiceData.PaymentDuration, invoiceData.InvoiceDate);

                if (invoiceCommunications != null && invoiceCommunications.Any())
                    exportARFollowUpReport.Communication = invoiceCommunications.Where(x => x.InvoiceNo == invoiceData.InvoiceNo).FirstOrDefault()?.Comment;

            }

            exportARFollowUpReport.InUSD = Math.Round((Double)exchangeCurrencyItem?.Fee, 2);
        }

        /// <summary>
        /// Map the ar follow up extrafee data
        /// </summary>
        /// <param name="exportARFollowUpReport"></param>
        /// <param name="extraFeeData"></param>
        /// <param name="exchangeCurrencyItem"></param>
        private void MapExtraFeeData(ExportARFollowUpReport exportARFollowUpReport, KpiExtraFeeData extraFeeData, ExchangeCurrencyItem exchangeCurrencyItem)
        {
            exportARFollowUpReport.InvoiceType = KpiARFollowUpReportConstantList.GetValueOrDefault((int)KpiARFollowUpReportConstant.Penalty, "");
            exportARFollowUpReport.InvoiceDate = extraFeeData.InvoiceDate?.ToString(StandardDateFormat);
            exportARFollowUpReport.InvoiceNo = extraFeeData.ExtraFeeInvoiceNo;
            exportARFollowUpReport.InvoiceAmount = extraFeeData.ExtraFee;
            exportARFollowUpReport.Currency = extraFeeData.CurrencyName;
            exportARFollowUpReport.Company = extraFeeData.BillingEntity;
            exportARFollowUpReport.InvoiceBilledAddress = extraFeeData.BilledToAddress;
            exportARFollowUpReport.InvoiceBilledToName = extraFeeData.BilledToName;
            exportARFollowUpReport.InvoiceBilledTo = extraFeeData.BilledName;
            exportARFollowUpReport.PaymentTerms = extraFeeData.PaymentTerms;
            exportARFollowUpReport.InvoiceStatus = extraFeeData.ExtraFeeStatus;
            exportARFollowUpReport.PaymentStatus = extraFeeData.PaymentStatusName;

            exportARFollowUpReport.PaymentDate = extraFeeData.PaymentDate?.ToString(StandardDateFormat);

            CalculateDueDateAndUpdateDSO(exportARFollowUpReport, extraFeeData.PaymentDuration?.ToString(), extraFeeData.InvoiceDate);

            exportARFollowUpReport.InUSD = Math.Round((Double)exchangeCurrencyItem?.ExtraFee, 2);
        }

        /// <summary>
        /// Calculate the due date and update DSO
        /// </summary>
        /// <param name="exportARFollowUpReport"></param>
        /// <param name="invoiceDate"></param>
        /// <param name="paymentDurationValue"></param>
        private void CalculateDueDateAndUpdateDSO(ExportARFollowUpReport exportARFollowUpReport,
            string paymentDurationValue, DateTime? invoiceDate)
        {
            int paymentDuration;
            if (invoiceDate != null && !string.IsNullOrEmpty(paymentDurationValue) && int.TryParse(paymentDurationValue, out paymentDuration))
            {
                DateTime? dueDate;
                //calcualte due data by adding payment duration with invoice date
                dueDate = invoiceDate?.AddDays(Convert.ToInt32(paymentDuration));
                exportARFollowUpReport.DueDate = dueDate?.ToString(StandardDateFormat);

                //calculate the DUO based on difference bw Due Date and current date
                if (dueDate != null)
                {
                    TimeSpan DUOValue = DateTime.Now - dueDate.GetValueOrDefault();
                    if (DUOValue.Days < 0)
                        exportARFollowUpReport.DSO = KpiARFollowUpReportConstantList.GetValueOrDefault((int)KpiARFollowUpReportConstant.DSONotDue, "");
                    else if (DUOValue.Days < 30)
                        exportARFollowUpReport.DSO = KpiARFollowUpReportConstantList.GetValueOrDefault((int)KpiARFollowUpReportConstant.DSO30, "");
                    else if (DUOValue.Days < 60)
                        exportARFollowUpReport.DSO = KpiARFollowUpReportConstantList.GetValueOrDefault((int)KpiARFollowUpReportConstant.DSO60, "");
                    else if (DUOValue.Days < 180)
                        exportARFollowUpReport.DSO = KpiARFollowUpReportConstantList.GetValueOrDefault((int)KpiARFollowUpReportConstant.DSO180, "");
                    else if (DUOValue.Days > 180)
                        exportARFollowUpReport.DSO = KpiARFollowUpReportConstantList.GetValueOrDefault((int)KpiARFollowUpReportConstant.DSOOLDER, "");

                }
            }
        }



        public List<ExportGapCustomerFlashProcessAudit> MapGapFlashProcessAudit(List<KpiInspectionBookingItems> inspectionBookingItems, List<GapCustomerKpiReportData> reportData,
            List<InspectionBookingDFData> bookingDFDataList, List<BookingServiceType> bookingServiceTypes, List<ScheduleStaffItem> scheduleStaffItems,
            List<SupplierCode> supplierCodes, List<SupplierCode> factoryCodes, List<SupplierAddressData> factoryAddresses)
        {
            var result = new List<ExportGapCustomerFlashProcessAudit>();
            foreach (var item in reportData)
            {
                var booking = inspectionBookingItems.FirstOrDefault(x => x.BookingId == item.InspectionId);
                var data = new ExportGapCustomerFlashProcessAudit()
                {
                    InspectionId = item.InspectionId,
                    ReportNo = item.ReportNo,
                    ServiceType = bookingServiceTypes?.FirstOrDefault(x => x.BookingNo == item.InspectionId)?.ServiceTypeName ?? "",
                    PivotAudit = item.ExternalReportNumber,
                    InspectionStartedTime = item.InspectionStartedDate.HasValue && !string.IsNullOrWhiteSpace(item.InspectionStartTime) ? item?.InspectionStartedDate?.ToString(StandardDateFormat) + ", " + item.InspectionStartTime : "",
                    InspectionSubmittedTime = item.InspectionSubmittedDate.HasValue && !string.IsNullOrWhiteSpace(item.InspectionEndTime) ? item?.InspectionSubmittedDate?.ToString(StandardDateFormat) + ", " + item.InspectionEndTime : "",
                    LastAuditScore = item.LastAuditScore,
                    Auditor = string.Join(", ", scheduleStaffItems.Where(x => x.BookingId == item.InspectionId).Select(y => y.Name).Distinct().ToList()),
                    Supplier = booking?.SupplierName,
                    SupplierCode = supplierCodes?.FirstOrDefault(x => x.SupplierId == booking.SupplierId && x.CustomerId == booking.CustomerId)?.Code ?? "",
                    Factory = booking?.FactoryName,
                    FactoryCode = factoryCodes?.FirstOrDefault(x => x.SupplierId == booking.FactoryId && x.CustomerId == booking.CustomerId)?.Code ?? "",
                    FactoryCountry = factoryAddresses?.FirstOrDefault(x => x.SupplierId == booking.FactoryId)?.CountryName,
                    MainCategory = item.ProductCategory,
                    OtherCategory = item.OtherCategory,
                    Market = item.Market,
                    TotalScore = item.TotalScore,
                    Grade = item.Grade,
                    ReportId = item.ReportId,
                    AuditProductCategory = bookingDFDataList?.FirstOrDefault(x => x.BookingNo == item.InspectionId && x.ControlConfigId == (int)DynamicFielsCuConfig.TemplateProductCategory)?.DFValue
                };
                result.Add(data);
            }

            return result;
        }

        public List<ExportGapCustomerProcessAudit> MapGapProcessAudit(List<KpiAuditBookingItems> auditBookingItems, List<AuditServiceTypeRepoResponse> auditServiceTypes,
            List<AuditAuditorRepoResponse> auditors,
           List<SupplierCode> supplierCodes, List<SupplierCode> factoryCodes, List<SupplierAddressData> factoryAddresses)
        {
            var result = new List<ExportGapCustomerProcessAudit>();
            foreach (var item in auditBookingItems)
            {
                var data = new ExportGapCustomerProcessAudit()
                {
                    AuditId = item.AuditId,
                    ReportNo = item.ReportNo,
                    ServiceType = auditServiceTypes?.FirstOrDefault(x => x.AuditId == item.AuditId)?.ServiceTypeName ?? "",
                    Auditor = string.Join(", ", auditors.Where(x => x.AuditId == item.AuditId).Select(y => y.AuditorName).Distinct().ToList()),
                    AuditStartedTime = item.AuditStartTime,
                    AuditSubmittedTime = item.AuditSubmittedTime,
                    Supplier = item?.SupplierName,
                    SupplierCode = supplierCodes?.FirstOrDefault(x => x.SupplierId == item.SupplierId && x.CustomerId == item.CustomerId)?.Code ?? "",
                    Factory = item?.FactoryName,
                    FactoryCode = factoryCodes?.FirstOrDefault(x => x.SupplierId == item.FactoryId && x.CustomerId == item.CustomerId)?.Code ?? "",
                    FactoryCountry = factoryAddresses?.FirstOrDefault(x => x.SupplierId == item.FactoryId)?.CountryName,
                    Grade = item.Grade,
                    LastAuditScore = item.LastAuditScore,
                    MainCategory = item.MainCategory,
                    OtherCategory = item.OtherCategory,
                    Market = item.Market,
                    TotalScore = item.ScoreValues,
                    AuditProductCategory = item.AuditProdutCategory,
                    PivotAudit = item.PivotAudit
                };
                result.Add(data);
            }

            return result;
        }

        public List<InspectionSummaryExpenseTemplate> MapInspectionSummaryExpenses(List<ScheduleStaffItem> qcList,
          List<KpiInspectionBookingItems> bookingItems,
          List<ServiceTypeList> serviceTypeList,
          List<FactoryCountry> factoryLocationData,
          List<InspectionQcKpiExpenseDetails> expenseList,
          List<InspectionQcKpiInvoiceDetails> invoiceList,
          List<KpiExtraFeeData> extraFeeDataList)
        {
            var result = new List<InspectionSummaryExpenseTemplate>();
            var qcData = qcList.OrderBy(x => x.Id).ThenBy(x => x.ServiceDate).ToList();
            foreach (var qc in qcData)
            {
                var booking = bookingItems?.FirstOrDefault(x => x.BookingId == qc.BookingId);
                var expenseListByQC = expenseList.Where(x => x.InspectionId == qc.BookingId && x.QcId == qc.Id).ToList();
                var factoryLocation = factoryLocationData?.FirstOrDefault(x => x.BookingId == qc.BookingId);
                var serviceType = serviceTypeList?.FirstOrDefault(x => x.InspectionId == qc.BookingId);

                var invoiceData = invoiceList.FirstOrDefault(x => x.InspectionId == qc.BookingId);
                var extraFeeData = extraFeeDataList.Where(x => x.BookingId == qc.BookingId).ToList();

                //for this condition for add data by expense level, if the same booking and qc data is not available
                if (!result.Any(x => x.BookingNo == qc.BookingId && x.QcId == qc.Id))
                {
                    if (expenseListByQC.Any())
                    {
                        foreach (var expense in expenseListByQC)
                        {
                            var inspectionQcSummaryTemplate = MapInspectionSummaryExpenseTemplateDate(result, qc, booking, factoryLocation, serviceType, invoiceData, extraFeeData);
                            inspectionQcSummaryTemplate.OutsourceCompany = expense?.OutsourceCompany;
                            inspectionQcSummaryTemplate.StartCity = expense?.StartCity;
                            inspectionQcSummaryTemplate.EndCity = expense?.EndCity;
                            inspectionQcSummaryTemplate.ExpenseType = expense.ExpenseTypeName;
                            inspectionQcSummaryTemplate.ExpenseDate = expense.ExpenseDate.ToString(StandardDateFormat);
                            inspectionQcSummaryTemplate.TripType = expense.TripTypeName;
                            inspectionQcSummaryTemplate.NumberOfManDay = expense.NumberOfManDay;
                            inspectionQcSummaryTemplate.ClaimAmount = expense.ClaimAmount;
                            inspectionQcSummaryTemplate.ClaimStatus = expense.ClaimStatus;
                            inspectionQcSummaryTemplate.ClaimRemarks = expense.ClaimRemarks;
                            inspectionQcSummaryTemplate.ClaimNumber = expense.ClaimNumber;
                            inspectionQcSummaryTemplate.ClaimDate = expense?.ClaimDate;
                            result.Add(inspectionQcSummaryTemplate);
                        }
                    }
                    else
                    {
                        var inspectionQcSummaryTemplate = MapInspectionSummaryExpenseTemplateDate(result, qc, booking, factoryLocation, serviceType, invoiceData, extraFeeData);
                        result.Add(inspectionQcSummaryTemplate);
                    }
                }
                else
                {
                    var expense = expenseListByQC.FirstOrDefault();
                    var inspectionQcSummaryTemplate = MapInspectionSummaryExpenseTemplateDate(result, qc, booking, factoryLocation, serviceType, invoiceData, extraFeeData);
                    if (expense != null)
                    {
                        inspectionQcSummaryTemplate.OutsourceCompany = expense?.OutsourceCompany;
                        inspectionQcSummaryTemplate.ClaimStatus = expense?.ClaimStatus;
                        inspectionQcSummaryTemplate.ClaimRemarks = expense?.ClaimRemarks;
                        inspectionQcSummaryTemplate.ClaimNumber = expense?.ClaimNumber;
                        inspectionQcSummaryTemplate.ClaimDate = expense?.ClaimDate;
                    }

                    result.Add(inspectionQcSummaryTemplate);
                }
            }

            return result.OrderBy(x => x.BookingNo).ThenBy(x => x.QcId).ThenBy(x => x.ScheduleDate).ToList();
        }

        private InspectionSummaryExpenseTemplate MapInspectionSummaryExpenseTemplateDate(List<InspectionSummaryExpenseTemplate> inspectionSummaryExpenses, ScheduleStaffItem qc, KpiInspectionBookingItems booking, FactoryCountry factoryLocation, ServiceTypeList serviceType, InspectionQcKpiInvoiceDetails invoiceData, List<KpiExtraFeeData> extraFeeData)
        {
            var inspectionQcSummaryTemplate = new InspectionSummaryExpenseTemplate
            {
                Office = booking?.Office,
                BookingNo = booking?.BookingId,
                ServiceDateFrom = booking?.ServiceDateFrom,
                ServiceDateTo = booking?.ServiceDateTo,
                ServiceTypeName = serviceType?.serviceTypeName,
                ScheduleDate = qc.ServiceDate,
                QcId = qc.Id,
                QCName = qc.Name,
                CustomerName = booking?.CustomerName,
                SupplierName = booking?.SupplierName,
                FactoryName = booking?.FactoryName,
                FactoryCountry = factoryLocation?.CountryName,
                EmployeeTypeName = qc.EmployeeTypeName,
                OrderStatus = booking?.StatusName,
                PayrollCurrency = qc.PayrollCurrency,
                FactoryCity = factoryLocation?.CityName,
                FactoryTown = factoryLocation?.TownName,
                StartPort = qc.StartPortName,

            };

            if (invoiceData != null)
                inspectionQcSummaryTemplate.InvoiceNo = invoiceData.InvoiceNo;


            if (extraFeeData.Any())
            {
                inspectionQcSummaryTemplate.ExtraFeeInvoiceNo = string.Join(", ", extraFeeData.Where(x => !string.IsNullOrWhiteSpace(x.ExtraFeeInvoiceNo)).Select(x => x.ExtraFeeInvoiceNo).ToList());
                inspectionQcSummaryTemplate.ExtraFeeInvoiceStatus = string.Join(", ", extraFeeData.Where(x => !string.IsNullOrWhiteSpace(x.ExtraFeeStatus)).Select(x => x.ExtraFeeStatus).ToList());
            }

            // Invoice details - add only for first row of the booking
            if (!inspectionSummaryExpenses.Any(x => x.BookingNo == qc.BookingId))
            {
                if (invoiceData != null)
                {
                    inspectionQcSummaryTemplate.InvoiceCurrency = invoiceData.InvoiceCurrency;
                    inspectionQcSummaryTemplate.InvoiceInspectionFees = invoiceData.InvoiceInspectionFees;
                    inspectionQcSummaryTemplate.InvoiceTravellingFees = invoiceData.InvoiceTravellingFees;
                    inspectionQcSummaryTemplate.InvoiceOtherFees = invoiceData.InvoiceOtherFees + invoiceData.InvoiceHotelFees;
                    inspectionQcSummaryTemplate.InvoiceTotalTax = invoiceData.InvoiceTotalTax;
                    inspectionQcSummaryTemplate.InvoiceTotalFees = invoiceData.InvoiceTotalFees;
                    inspectionQcSummaryTemplate.InvoiceManDay = invoiceData.InvoiceManDay;
                }
                if (extraFeeData.Any())
                {
                    inspectionQcSummaryTemplate.ExtraFeeCurrency = string.Join(", ", extraFeeData.Where(x => !string.IsNullOrWhiteSpace(x.CurrencyName)).Select(x => x.CurrencyName).ToList());
                    inspectionQcSummaryTemplate.TotalExtraFees = extraFeeData.Select(x => x.ExtraFee).Sum();
                }
            }

            return inspectionQcSummaryTemplate;
        }
    }
}
