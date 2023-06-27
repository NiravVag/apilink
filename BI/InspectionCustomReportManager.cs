using BI.Maps;
using BI.Utilities;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.CustomReport;
using DTO.FullBridge;
using DTO.InspectionCustomReport;
using DTO.Report;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using Newtonsoft.Json;
using System.Data;
using System.Net.Http.Headers;
using DTO.File;
using Microsoft.Extensions.Configuration;
using Components.Core.contracts;
namespace BI
{
    public class InspectionCustomReportManager : ApiCommonData, IInspectionCustomReportManager
    {
        private readonly ISharedInspectionManager _sharedInspection = null;
        private readonly IInspectionBookingRepository _inspeRepo = null;
        private readonly IInspectionCustomReportRepository _inspeCusReport = null;
        private readonly InspectionCustomReportMap _inspectionCustomReportmap = null;
        private readonly IHelper _helper = null;
        private readonly FBSettings _fbSettings = null;
        private readonly IConfiguration _configuration = null;
        private ITenantProvider _filterService = null;
        private readonly IFileManager _fileManager = null;
        public InspectionCustomReportManager
            (
            IAPIUserContext applicationContext,
            ISharedInspectionManager sharedInspection, IInspectionBookingRepository inspeRepo, IOptions<FBSettings> fbSettings, IHelper helper, IConfiguration configuration, ITenantProvider filterService, IInspectionCustomReportRepository inspeCusReport, IFileManager fileManager
            )
        {
            _sharedInspection = sharedInspection;
            _inspeRepo = inspeRepo;
            _inspectionCustomReportmap = new InspectionCustomReportMap();
            _helper = helper;
            _fbSettings = fbSettings.Value;
            _configuration = configuration;
            _filterService = filterService;
            _inspeCusReport = inspeCusReport;
            _fileManager = fileManager;
        }
        /// <summary>
        /// get booking details
        /// </summary>
        /// <param name="fbReportId"></param>
        /// <returns></returns>
        public async Task<InspectionCustomReportSummaryResponse> GetReportDetails(int fbreportid)
        {
            try
            {
                string color = "";
                string colorCode = "";
                int? fbReportMapId = 0;
                var FbReportDetails = await _inspeRepo.GetFbReportsDetail(fbreportid);
                fbReportMapId = FbReportDetails.FBReportMapId;
                //CS
                var FBCsUserIds = await _inspeCusReport.GetFBReportCSIds(fbreportid);
                var CSStaffList = await _inspeCusReport.GetFBReportStaffList(FBCsUserIds);
                var CSIDs = CSStaffList != null && CSStaffList.Any() ? string.Join(",", CSStaffList.Select(x => x.StaffId).Distinct().ToList()) : "";
                var CSNames = CSStaffList != null && CSStaffList.Any() ? string.Join(",", CSStaffList.Select(x => x.PersonName).Distinct().ToList()) : "";
                //QC
                var FBQcIds = await _inspeCusReport.GetFbReportQcIDs(fbreportid);
                var QCStaffList = await _inspeCusReport.GetFBReportStaffList(FBQcIds);
                var QCIds = QCStaffList != null && QCStaffList.Any() ? string.Join(",", QCStaffList.Select(x => x.StaffId).Distinct().ToList()) : "";
                var QCNames = QCStaffList != null && QCStaffList.Any() ? string.Join(",", QCStaffList.Select(x => x.PersonName).Distinct().ToList()) : "";

                var inspectionQuery = _sharedInspection.GetAllInspectionQuery();
                var bookingIds = await inspectionQuery.SelectMany(x => x.FbReportDetails)
                    .Where(x => x.Active.HasValue && x.Active.Value && x.FbReportMapId == fbReportMapId && x.InspectionId.HasValue)
                    .Select(x => x.InspectionId.Value).Distinct().ToListAsync();

                var bookingDeptAccessList = await _inspeRepo.GetDeptBookingIdsByBookingIds(bookingIds);

                var bookingBrandAccessList = await _inspeRepo.GetBrandBookingIdsByBookingIds(bookingIds);

                var bookingBuyerAccessList = await _inspeRepo.GetBuyerBookingIdsByBookingIds(bookingIds);

                var servicetype = await _inspeRepo.GetBookingServiceTypes(bookingIds);

                var ProductIds = await _inspeCusReport.GetInspProductIds(fbreportid);
                var colorinfo = await _inspeCusReport.GetInspPurchaseOrderColorTransactionList(fbreportid, ProductIds, bookingIds);
                color = string.Join(",", colorinfo.Select(x => x.ColorName).ToList().Distinct());
                colorCode = string.Join(",", colorinfo.Select(x => x.ColorCode).ToList().Distinct());

                var productpoinfo = await _inspeRepo.GetProductPoListByBooking(bookingIds);
                var ponumber = string.Join(",", productpoinfo.Where(x => x.FbReportId == fbreportid).Select(x => x.PoNumber).Distinct());
                var prodctref = string.Join(",", string.Join(",", productpoinfo.Where(x => x.FbReportId == fbreportid).Select(x => x.ProductId).Distinct()));
                var productdesc = string.Join(",", string.Join(",", productpoinfo.Where(x => x.FbReportId == fbreportid).Select(x => x.ProductDescription).Distinct()));
                var ETD = string.Join(",", string.Join(",", productpoinfo.Where(x => x.Etd.HasValue && x.FbReportId == fbreportid).Select(x => (x.Etd.Value).ToString("dd/MM/yyyy")).Distinct()));
                var customercontact = await _inspeRepo.GetBookingCustomerContact(bookingIds.FirstOrDefault());
                var result = await inspectionQuery.Where(x => bookingIds.Contains(x.Id)).Select(x => new InspectionCustomReportItem()
                {
                    InspectionNo = x.Id,
                    CustomerId = x.CustomerId,
                    CustomerName = x.Customer.CustomerName,
                    Collection = x.Collection == null ? "" : x.Collection.Name,
                    SupplierName = x.Supplier.SupplierName,
                    FactoryName = x.Factory.SupplierName,
                    FactoryCountry = x.Factory.SuAddresses.Where(x => x.AddressTypeId.Value == (int)SuAddressTypeEnum.Headoffice).Select(x => x.Country.CountryName).FirstOrDefault(),
                    FactoryAddress = x.Factory.SuAddresses.Where(x => x.AddressTypeId.Value == (int)SuAddressTypeEnum.Headoffice).Select(x => x.Address).FirstOrDefault(),
                    Inspection_From_Date = x.ServiceDateFrom,
                    Inspection_To_Date = x.ServiceDateTo,
                    OfficeId = x.OfficeId,
                    Office = x.Office.LocationName,
                    ProductCategory = x.ProductCategory == null ? "" : x.ProductCategory.Name,
                    ProductCategoryId = x.ProductCategoryId,
                    ProductSubCategory = x.ProductSubCategory == null ? "" : x.ProductSubCategory.Name,
                    Season = (x.Season == null ? "" : x.Season.Season.Name) + " " + (x.SeasonYear == null ? "" : x.SeasonYear.Year.ToString()),
                    CustomerProductCategory = x.CuProductCategoryNavigation == null ? "" : x.CuProductCategoryNavigation.Name,
                    ShipmentDate = x.ShipmentDate,
                    ReInspectionTypeId = x.ReInspectionType


                }).FirstOrDefaultAsync();
                if (result == null)
                {
                    result = new InspectionCustomReportItem();
                }
                result.Brand = bookingBrandAccessList != null && bookingBrandAccessList.Any() ? string.Join(",", bookingBrandAccessList.Select(x => x.BrandName).Distinct()) : "";
                result.BrandId = bookingBrandAccessList != null && bookingBrandAccessList.Any() ? bookingBrandAccessList[0].BrandId : 0;
                result.Department = bookingDeptAccessList != null && bookingDeptAccessList.Any() ? string.Join(",", bookingDeptAccessList.Select(x => x.DeptName).Distinct()) : "";
                result.DepartId = bookingDeptAccessList != null && bookingDeptAccessList.Any() ? bookingDeptAccessList[0].DeptId : 0;
                var ReInspectionType = result.ReInspectionTypeId != null && !string.IsNullOrEmpty(result.ReInspectionTypeId.ToString()) ? (" #" + result.ReInspectionTypeId.ToString()) : "";
                result.ServiceType = servicetype != null && servicetype.Any() ? string.Join(",", servicetype.Select(x => x.ServiceTypeName).Distinct()) + ReInspectionType : "";

                result.ServiceTypeId = servicetype != null && servicetype.Any() ? servicetype[0].ServiceTypeId : 0;
                result.Buyer = bookingBuyerAccessList != null && bookingBuyerAccessList.Any() ? string.Join(",", bookingBuyerAccessList.Select(x => x.BuyerName).Distinct()) : "";
                result.PONumber = ponumber;
                result.Color = color;
                result.ColorCode = colorCode;
                result.ProductRef = prodctref;
                result.ProductDesc = productdesc;
                result.ETD = ETD;
                result.CustomerContacts = customercontact != null && customercontact.Any() ? string.Join(",", customercontact) : "";
                result.Inspection_Date = result.Inspection_From_Date == result.Inspection_To_Date ? result.Inspection_From_Date.Date.ToString(StandardDateFormat) : string.Join(" - ", result.Inspection_From_Date.Date.ToString(StandardDateFormat), result.Inspection_To_Date.Date.ToString(StandardDateFormat));
                result.ReportTitle = FbReportDetails.ReportTitle;
                result.MissionTitle = FbReportDetails.MissionTitle;
                result.OverAllResult = FbReportDetails.OverAllResult;
                result.FBReportMapId = FbReportDetails.FBReportMapId;
                result.CsIds = CSIDs;
                result.CsNames = CSNames;
                result.QCIds = QCIds;
                result.QCNames = QCNames;
                return new InspectionCustomReportSummaryResponse()
                {
                    Result = InspectionCustomReportSummaryResponseResult.Success,
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// get Full bridge info based on the Fb report Id
        /// </summary>
        /// <param name="fbReportId"></param>
        /// <param name="fbToken"></param>
        /// <returns></returns>
        public async Task<FBReportResponse> FetchFBReport(int fbReportId, string fbToken)
        {
            try
            {
                var response = new FBReportResponse { ReportId = fbReportId };
                var bookingdetails = await GetReportDetails(fbReportId);
                var reportjson = string.Empty;

                if (fbReportId > 0)
                    reportjson = await GetFullBridgeReportInfo(fbReportId, fbToken);

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<string> GetFullBridgeReportInfo(int? fbReportId, string fbToken)
        {
            try
            {
                var fbBase = _fbSettings.BaseUrl;

                var fbRequest = string.Format(_fbSettings.FbReportDataRequestUrl, fbReportId);

                // backend api from FB.
                //fbRequest = fbRequest + "?token=" + fbToken;

                HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(Method.Get, fbRequest, null, fbBase, fbToken, true);

                var StatusCode = httpResponse.StatusCode;

                if (StatusCode == HttpStatusCode.OK)
                {
                    var reportData = httpResponse.Content.ReadAsStringAsync();
                    return reportData.Result;
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// get Full bridge Info based on the FB Report Map Id
        /// </summary>
        /// <param name="fbReportMapId"></param>
        /// <param name="fbToken"></param>
        /// <returns></returns>
        public async Task<FBReportInfoResponse> FetchFBReportInfo(int fbReportId, string fbToken)
        {

            var response = new FBReportInfoResponse { ReportMapId = fbReportId };
            if (fbReportId > 0)
            {

                var MainOrderInfo = await GetReportDetails(fbReportId);//Get Order Main Info from DB
                List<InspectionCustomReportItem> OrderMainDetail = new List<InspectionCustomReportItem>();
                OrderMainDetail.Add(MainOrderInfo.Data);
                response.lstOrderMainDetail = OrderMainDetail;


                #region get Template Config
                int EntityId = _filterService.GetCompanyId();
                TemplateConfigResponse TemplateConfig = new TemplateConfigResponse();
                List<RepFastTemplateConfig> lstRepFastTemplateConfig = new List<RepFastTemplateConfig>();

                //per customer id to get the report template list
                var lstTemplateConfig = await _inspeCusReport.GetTemplateConfigList(EntityId, MainOrderInfo.Data.Inspection_From_Date.Date, MainOrderInfo.Data.CustomerId);
                lstRepFastTemplateConfig = GetTemplateConfig(lstTemplateConfig, MainOrderInfo);
                if (lstRepFastTemplateConfig == null || lstRepFastTemplateConfig.Count == 0)
                {
                    // get the Standard report template list
                    lstTemplateConfig = await _inspeCusReport.GetStandardTemplateConfigList(EntityId, MainOrderInfo.Data.Inspection_From_Date.Date);
                    lstRepFastTemplateConfig = GetTemplateConfig(lstTemplateConfig, MainOrderInfo);
                }

                if (lstRepFastTemplateConfig != null && lstRepFastTemplateConfig.Count >= 1)
                {
                    TemplateConfig = new TemplateConfigResponse()
                    {
                        TemplateName = lstRepFastTemplateConfig[0].Template.Name,
                        FileExtensionID = lstRepFastTemplateConfig[0].FileExtensionId,
                        ReportToolTypeID = lstRepFastTemplateConfig[0].ReportToolTypeId
                    };
                }
                response.TemplateConfig = TemplateConfig;
                #endregion


                var fbReportMapId = MainOrderInfo.Data.FBReportMapId;
                string jsonOrderDetail = await GetFullBridgeReportInfo(fbReportMapId, fbToken);  //Get reportjson from FB
                //// jsonOrderDetail = System.IO.File.ReadAllText(Path.Combine("Views/Report_Templates/SGT", "report-massimo-dutti-10001423.json"));
                var fbOrderDetail = JsonConvert.DeserializeObject<OrderDetailRoot>(jsonOrderDetail);
                List<Sections> OrderDetail = new List<Sections>();
                if (fbOrderDetail != null && fbOrderDetail.sections != null)
                {
                    OrderDetail.Add(fbOrderDetail.sections);
                }
                response.lstOrderDetail = OrderDetail;

                #region get report info
                if (fbOrderDetail != null && fbOrderDetail.sections != null && fbOrderDetail.template != null)
                {
                    //Template detail
                    List<TemplateDetail> TemplateDetail = new List<TemplateDetail>();
                    TemplateDetail.Add(new TemplateDetail() { id = fbOrderDetail.template.id, ISONo = fbOrderDetail.template.reference });
                    response.lstTemplateDetail = TemplateDetail;

                    #region Quantity Details / products/Sizes
                    DataTable tblProductsColor = new DataTable("ProductsColor");
                    DataColumn dcProductsColor = null;
                    dcProductsColor = tblProductsColor.Columns.Add("ProColorID", Type.GetType("System.Int32"));
                    dcProductsColor.AutoIncrement = true;
                    dcProductsColor.AutoIncrementSeed = 1;
                    dcProductsColor.AutoIncrementStep = 1;
                    dcProductsColor.AllowDBNull = false;
                    dcProductsColor = tblProductsColor.Columns.Add("report_product_id", Type.GetType("System.Int32"));
                    dcProductsColor = tblProductsColor.Columns.Add("productId", Type.GetType("System.Int32"));
                    dcProductsColor = tblProductsColor.Columns.Add("quantity", Type.GetType("System.Int32"));
                    dcProductsColor = tblProductsColor.Columns.Add("purchaseOrderNumber", Type.GetType("System.String"));
                    dcProductsColor = tblProductsColor.Columns.Add("productReference", Type.GetType("System.String"));
                    dcProductsColor = tblProductsColor.Columns.Add("productDescription", Type.GetType("System.String"));
                    dcProductsColor = tblProductsColor.Columns.Add("color", Type.GetType("System.String"));
                    dcProductsColor = tblProductsColor.Columns.Add("destinationCountry", Type.GetType("System.String"));

                    DataTable tblProColorSizes = new DataTable("ProColorSizes");
                    DataColumn dcProColorSizes = null;
                    dcProColorSizes = tblProColorSizes.Columns.Add("ProSizesID", Type.GetType("System.Int32"));
                    dcProColorSizes.AutoIncrement = true;
                    dcProColorSizes.AutoIncrementSeed = 1;
                    dcProColorSizes.AutoIncrementStep = 1;
                    dcProColorSizes.AllowDBNull = false;
                    dcProColorSizes = tblProColorSizes.Columns.Add("report_product_id", Type.GetType("System.Int32"));
                    dcProColorSizes = tblProColorSizes.Columns.Add("sizename", Type.GetType("System.String"));


                    DataRow newprocolorRow;
                    DataRow newprosizesRow;
                    if (fbOrderDetail.products != null && fbOrderDetail.products.Count > 0)
                    {
                        List<Products> lstProducts = new List<Products>();
                        lstProducts = fbOrderDetail.products;
                        response.lstProducts = lstProducts;

                        #region table ProductsColor
                        foreach (var procolor in fbOrderDetail.products)
                        {
                            newprocolorRow = tblProductsColor.NewRow();
                            newprocolorRow["report_product_id"] = procolor.report_product_id;
                            newprocolorRow["productId"] = procolor.productId;
                            newprocolorRow["quantity"] = procolor.quantity;
                            newprocolorRow["purchaseOrderNumber"] = procolor.purchaseOrderNumber;
                            newprocolorRow["productReference"] = procolor.productReference;
                            newprocolorRow["productDescription"] = procolor.productDescription;
                            newprocolorRow["color"] = procolor.color;
                            newprocolorRow["destinationCountry"] = procolor.destinationCountry;
                            tblProductsColor.Rows.Add(newprocolorRow);

                            #region table sizes
                            if (procolor.sizes != null && procolor.sizes.Count > 0)
                            {
                                foreach (var prosizes in procolor.sizes)
                                {
                                    newprosizesRow = tblProColorSizes.NewRow();
                                    newprosizesRow["report_product_id"] = procolor.report_product_id;
                                    newprosizesRow["sizename"] = prosizes.sizename;
                                    tblProColorSizes.Rows.Add(newprosizesRow);
                                }
                            }
                            #endregion
                        }
                        #endregion

                    }
                    DataSet dsProductsColor = new DataSet();
                    dsProductsColor.Tables.Add(tblProductsColor);
                    dsProductsColor.Tables.Add(tblProColorSizes);
                    dsProductsColor.Relations.Add("PCSizesLink", dsProductsColor.Tables[0].Columns["report_product_id"], dsProductsColor.Tables[1].Columns["report_product_id"]);
                    response.dsProductsColor = dsProductsColor;
                    #endregion

                    #region Factory Analysis Chart
                    DataTable ChartFacAnalysisDefects = new DataTable();
                    DataColumn dcChartFAD = null;
                    dcChartFAD = ChartFacAnalysisDefects.Columns.Add("type", Type.GetType("System.String"));
                    dcChartFAD = ChartFacAnalysisDefects.Columns.Add("allowed", Type.GetType("System.Int32"));
                    dcChartFAD = ChartFacAnalysisDefects.Columns.Add("found", Type.GetType("System.Int32"));
                    if (OrderDetail[0].result_and_conclusion != null && OrderDetail[0].result_and_conclusion.result_and_conclusion_fields!=null && OrderDetail[0].result_and_conclusion.result_and_conclusion_fields.result_and_conclusion_amcharts_defects_with_aql != null && OrderDetail[0].result_and_conclusion.result_and_conclusion_fields.result_and_conclusion_amcharts_defects_with_aql.result_and_conclusion_amcharts_defects_with_aql_value != null && OrderDetail[0].result_and_conclusion.result_and_conclusion_fields.result_and_conclusion_amcharts_defects_with_aql.result_and_conclusion_amcharts_defects_with_aql_value.Count > 0)
                    {
                        ChartFacAnalysisDefects = new DataTable();
                        ChartFacAnalysisDefects = _helper.ConvertToDataTable(OrderDetail[0].result_and_conclusion.result_and_conclusion_fields.result_and_conclusion_amcharts_defects_with_aql.result_and_conclusion_amcharts_defects_with_aql_value);
                    }
                    response.ChartFacAnalysisDefects = ChartFacAnalysisDefects;
                    #endregion

                    #region Factory Analysis Chart-MeasurementsOutofTolerance
                    DataTable ChartMSOutofTolerance = new DataTable();
                    DataColumn dcMSOT = null;
                    dcMSOT = ChartMSOutofTolerance.Columns.Add("category", Type.GetType("System.String"));
                    dcMSOT = ChartMSOutofTolerance.Columns.Add("value", Type.GetType("System.Int32"));
                    if (OrderDetail[0].result_and_conclusion != null && OrderDetail[0].result_and_conclusion.result_and_conclusion_fields!=null && OrderDetail[0].result_and_conclusion.result_and_conclusion_fields.result_and_conclusion_amcharts_measurements_spec != null && OrderDetail[0].result_and_conclusion.result_and_conclusion_fields.result_and_conclusion_amcharts_measurements_spec.result_and_conclusion_amcharts_measurements_spec_value != null && OrderDetail[0].result_and_conclusion.result_and_conclusion_fields.result_and_conclusion_amcharts_measurements_spec.result_and_conclusion_amcharts_measurements_spec_value.Count > 0)
                    {
                        ChartMSOutofTolerance = new DataTable();
                        ChartMSOutofTolerance = _helper.ConvertToDataTable(OrderDetail[0].result_and_conclusion.result_and_conclusion_fields.result_and_conclusion_amcharts_measurements_spec.result_and_conclusion_amcharts_measurements_spec_value);
                    }
                    response.ChartMSOutofTolerance = ChartMSOutofTolerance;
                    #endregion

                    #region 100 Charts Garment Grade
                    DataTable ChartsGarmentGrade = new DataTable();
                    DataColumn dcGarmentGrade = null;
                    dcGarmentGrade = ChartsGarmentGrade.Columns.Add("type", Type.GetType("System.String"));
                    dcGarmentGrade = ChartsGarmentGrade.Columns.Add("value", Type.GetType("System.Int32"));
                    if (OrderDetail[0].result_and_conclusion != null && OrderDetail[0].result_and_conclusion.result_and_conclusion_fields!=null && OrderDetail[0].result_and_conclusion.result_and_conclusion_fields.result_and_conclusion_amcharts_garment_grade != null && OrderDetail[0].result_and_conclusion.result_and_conclusion_fields.result_and_conclusion_amcharts_garment_grade.result_and_conclusion_amcharts_garment_grade_value != null && OrderDetail[0].result_and_conclusion.result_and_conclusion_fields.result_and_conclusion_amcharts_garment_grade.result_and_conclusion_amcharts_garment_grade_value.Count > 0)
                    {
                        ChartsGarmentGrade = new DataTable();
                        ChartsGarmentGrade = _helper.ConvertToDataTable(OrderDetail[0].result_and_conclusion.result_and_conclusion_fields.result_and_conclusion_amcharts_garment_grade.result_and_conclusion_amcharts_garment_grade_value);
                    }
                    response.ChartsGarmentGrade = ChartsGarmentGrade;
                    #endregion

                    #region amcharts_defects_by_category
                    DataTable ChartDefectsCategory = new DataTable();
                    DataColumn dcDefectsCategory = null;
                    dcDefectsCategory = ChartDefectsCategory.Columns.Add("category", Type.GetType("System.String"));
                    dcDefectsCategory = ChartDefectsCategory.Columns.Add("value", Type.GetType("System.Int32"));
                    if (OrderDetail[0].workmanship != null && OrderDetail[0].workmanship.workmanship_subSections!=null && OrderDetail[0].workmanship.workmanship_subSections.workmanship_results_analysis!=null && OrderDetail[0].workmanship.workmanship_subSections.workmanship_results_analysis.workmanship_results_analysis_fields!=null && OrderDetail[0].workmanship.workmanship_subSections.workmanship_results_analysis.workmanship_results_analysis_fields.workmanship_results_analysis_amcharts_defects_by_category != null && OrderDetail[0].workmanship.workmanship_subSections.workmanship_results_analysis.workmanship_results_analysis_fields.workmanship_results_analysis_amcharts_defects_by_category.workmanship_results_analysis_amcharts_defects_by_category_value != null && OrderDetail[0].workmanship.workmanship_subSections.workmanship_results_analysis.workmanship_results_analysis_fields.workmanship_results_analysis_amcharts_defects_by_category.workmanship_results_analysis_amcharts_defects_by_category_value.Count > 0)
                    {
                        ChartDefectsCategory = new DataTable();
                        ChartDefectsCategory = _helper.ConvertToDataTable(OrderDetail[0].workmanship.workmanship_subSections.workmanship_results_analysis.workmanship_results_analysis_fields.workmanship_results_analysis_amcharts_defects_by_category.workmanship_results_analysis_amcharts_defects_by_category_value);
                    }
                    response.ChartDefectsCategory = ChartDefectsCategory;
                    #endregion

                    #region amcharts_defects_by_category -JAPAN ORDERS/APA  ORDERS
                    DataTable ChartDefectsCategory2 = new DataTable();
                    DataColumn dcDefectsCategory2 = null;
                    dcDefectsCategory2 = ChartDefectsCategory2.Columns.Add("category", Type.GetType("System.String"));
                    dcDefectsCategory2 = ChartDefectsCategory2.Columns.Add("value", Type.GetType("System.Int32"));
                    if (OrderDetail[0].workmanship2 != null && OrderDetail[0].workmanship2.workmanship_subSections.workmanship_results_analysis.workmanship_results_analysis_fields.workmanship_results_analysis_amcharts_defects_by_category != null && OrderDetail[0].workmanship2.workmanship_subSections.workmanship_results_analysis.workmanship_results_analysis_fields.workmanship_results_analysis_amcharts_defects_by_category.workmanship_results_analysis_amcharts_defects_by_category_value != null && OrderDetail[0].workmanship2.workmanship_subSections.workmanship_results_analysis.workmanship_results_analysis_fields.workmanship_results_analysis_amcharts_defects_by_category.workmanship_results_analysis_amcharts_defects_by_category_value.Count > 0)
                    {
                        ChartDefectsCategory2 = new DataTable();
                        ChartDefectsCategory2 = _helper.ConvertToDataTable(OrderDetail[0].workmanship2.workmanship_subSections.workmanship_results_analysis.workmanship_results_analysis_fields.workmanship_results_analysis_amcharts_defects_by_category.workmanship_results_analysis_amcharts_defects_by_category_value);
                    }
                    response.ChartDefectsCategory2 = ChartDefectsCategory2;
                    #endregion

                    #region amcharts_defects_by_reparability
                    DataTable ChartDefectsReparability = new DataTable();
                    DataColumn dcDefectsReparability = null;
                    dcDefectsReparability = ChartDefectsReparability.Columns.Add("category", Type.GetType("System.String"));
                    dcDefectsReparability = ChartDefectsReparability.Columns.Add("value", Type.GetType("System.Int32"));
                    if (OrderDetail[0].workmanship != null && OrderDetail[0].workmanship.workmanship_subSections!=null &&  OrderDetail[0].workmanship.workmanship_subSections.workmanship_results_analysis!=null && OrderDetail[0].workmanship.workmanship_subSections.workmanship_results_analysis.workmanship_results_analysis_fields != null && OrderDetail[0].workmanship.workmanship_subSections.workmanship_results_analysis.workmanship_results_analysis_fields.workmanship_results_analysis_amcharts_defects_by_reparability != null && OrderDetail[0].workmanship.workmanship_subSections.workmanship_results_analysis.workmanship_results_analysis_fields.workmanship_results_analysis_amcharts_defects_by_reparability.workmanship_results_analysis_amcharts_defects_by_reparability_value != null && OrderDetail[0].workmanship.workmanship_subSections.workmanship_results_analysis.workmanship_results_analysis_fields.workmanship_results_analysis_amcharts_defects_by_reparability.workmanship_results_analysis_amcharts_defects_by_reparability_value.Count > 0)
                    {
                        ChartDefectsReparability = new DataTable();
                        ChartDefectsReparability = _helper.ConvertToDataTable(OrderDetail[0].workmanship.workmanship_subSections.workmanship_results_analysis.workmanship_results_analysis_fields.workmanship_results_analysis_amcharts_defects_by_reparability.workmanship_results_analysis_amcharts_defects_by_reparability_value);
                    }
                    response.ChartDefectsReparability = ChartDefectsReparability;
                    #endregion

                    #region amcharts_defects_with_aql
                    DataTable ChartDefectsWithAql = new DataTable();
                    DataColumn dcDWA = null;
                    dcDWA = ChartDefectsWithAql.Columns.Add("type", Type.GetType("System.String"));
                    dcDWA = ChartDefectsWithAql.Columns.Add("allowed", Type.GetType("System.Int32"));
                    dcDWA = ChartDefectsWithAql.Columns.Add("found", Type.GetType("System.Int32"));
                    if (OrderDetail[0].workmanship != null && OrderDetail[0].workmanship.workmanship_subSections!=null && OrderDetail[0].workmanship.workmanship_subSections.workmanship_results_analysis!=null && OrderDetail[0].workmanship.workmanship_subSections.workmanship_results_analysis.workmanship_results_analysis_fields!=null && OrderDetail[0].workmanship.workmanship_subSections.workmanship_results_analysis.workmanship_results_analysis_fields.workmanship_results_analysis_amcharts_defects_with_aql != null && OrderDetail[0].workmanship.workmanship_subSections.workmanship_results_analysis.workmanship_results_analysis_fields.workmanship_results_analysis_amcharts_defects_with_aql.workmanship_results_analysis_amcharts_defects_with_aql_value != null && OrderDetail[0].workmanship.workmanship_subSections.workmanship_results_analysis.workmanship_results_analysis_fields.workmanship_results_analysis_amcharts_defects_with_aql.workmanship_results_analysis_amcharts_defects_with_aql_value.Count > 0)
                    {
                        ChartDefectsWithAql = new DataTable();
                        ChartDefectsWithAql = _helper.ConvertToDataTable(OrderDetail[0].workmanship.workmanship_subSections.workmanship_results_analysis.workmanship_results_analysis_fields.workmanship_results_analysis_amcharts_defects_with_aql.workmanship_results_analysis_amcharts_defects_with_aql_value);
                    }
                    response.ChartDefectsWithAql = ChartDefectsWithAql;
                    #endregion

                    #region amcharts_defects_with_aql -JAPAN ORDERS/APA  ORDERS
                    DataTable ChartDefectsWithAql2 = new DataTable();
                    DataColumn dcDWA2 = null;
                    dcDWA2 = ChartDefectsWithAql2.Columns.Add("type", Type.GetType("System.String"));
                    dcDWA2 = ChartDefectsWithAql2.Columns.Add("allowed", Type.GetType("System.Int32"));
                    dcDWA2 = ChartDefectsWithAql2.Columns.Add("found", Type.GetType("System.Int32"));
                    if (OrderDetail[0].workmanship2 != null && OrderDetail[0].workmanship2.workmanship_subSections.workmanship_results_analysis.workmanship_results_analysis_fields.workmanship_results_analysis_amcharts_defects_with_aql != null && OrderDetail[0].workmanship2.workmanship_subSections.workmanship_results_analysis.workmanship_results_analysis_fields.workmanship_results_analysis_amcharts_defects_with_aql.workmanship_results_analysis_amcharts_defects_with_aql_value != null && OrderDetail[0].workmanship2.workmanship_subSections.workmanship_results_analysis.workmanship_results_analysis_fields.workmanship_results_analysis_amcharts_defects_with_aql.workmanship_results_analysis_amcharts_defects_with_aql_value.Count > 0)
                    {
                        ChartDefectsWithAql2 = new DataTable();
                        ChartDefectsWithAql2 = _helper.ConvertToDataTable(OrderDetail[0].workmanship2.workmanship_subSections.workmanship_results_analysis.workmanship_results_analysis_fields.workmanship_results_analysis_amcharts_defects_with_aql.workmanship_results_analysis_amcharts_defects_with_aql_value);
                    }
                    response.ChartDefectsWithAql2 = ChartDefectsWithAql2;
                    #endregion

                    #region defects list
                    DataTable tblDefectList = new DataTable("DefectList");
                    DataColumn dcDefect = null;
                    dcDefect = tblDefectList.Columns.Add("ID", Type.GetType("System.Int32"));
                    dcDefect.AutoIncrement = true;
                    dcDefect.AutoIncrementSeed = 1;
                    dcDefect.AutoIncrementStep = 1;
                    dcDefect.AllowDBNull = false;
                    dcDefect = tblDefectList.Columns.Add("DefectID", Type.GetType("System.Int32"));
                    dcDefect = tblDefectList.Columns.Add("report_product_id", Type.GetType("System.String"));
                    dcDefect = tblDefectList.Columns.Add("defect_correction_actions", Type.GetType("System.String"));
                    dcDefect = tblDefectList.Columns.Add("defect_critical", Type.GetType("System.String"));
                    dcDefect = tblDefectList.Columns.Add("defect_description", Type.GetType("System.String"));
                    dcDefect = tblDefectList.Columns.Add("defect_immediate_actions", Type.GetType("System.String"));
                    dcDefect = tblDefectList.Columns.Add("defect_immediate_actions_comment", Type.GetType("System.String"));
                    dcDefect = tblDefectList.Columns.Add("defect_major", Type.GetType("System.String"));
                    dcDefect = tblDefectList.Columns.Add("defect_minor", Type.GetType("System.String"));
                    dcDefect = tblDefectList.Columns.Add("defect_root_cause_analysis", Type.GetType("System.String"));
                    dcDefect = tblDefectList.Columns.Add("position", Type.GetType("System.String"));
                    dcDefect = tblDefectList.Columns.Add("product", Type.GetType("System.String"));
                    dcDefect = tblDefectList.Columns.Add("reparability", Type.GetType("System.String"));
                    dcDefect = tblDefectList.Columns.Add("size", Type.GetType("System.String"));
                    dcDefect = tblDefectList.Columns.Add("zone", Type.GetType("System.String"));
                    dcDefect = tblDefectList.Columns.Add("defect_code", Type.GetType("System.String"));
                    dcDefect = tblDefectList.Columns.Add("defect_status", Type.GetType("System.String"));
                    dcDefect = tblDefectList.Columns.Add("percent", Type.GetType("System.String"));
                    dcDefect = tblDefectList.Columns.Add("garment_grade", Type.GetType("System.String"));
                    dcDefect = tblDefectList.Columns.Add("defectvalue", Type.GetType("System.String"));
                    dcDefect = tblDefectList.Columns.Add("garment_size", Type.GetType("System.String"));
                    dcDefect = tblDefectList.Columns.Add("frequency_same_piece", Type.GetType("System.String"));
                    dcDefect = tblDefectList.Columns.Add("inspected_final_inspection", Type.GetType("System.String"));


                    DataTable tblDefectListPic = new DataTable("DefectListPic");
                    DataColumn dcPic = null;
                    dcPic = tblDefectListPic.Columns.Add("PicID", Type.GetType("System.Int32"));
                    dcPic.AutoIncrement = true;
                    dcPic.AutoIncrementSeed = 1;
                    dcPic.AutoIncrementStep = 1;
                    dcPic.AllowDBNull = false;

                    dcPic = tblDefectListPic.Columns.Add("DefectID", Type.GetType("System.Int32"));
                    dcPic = tblDefectListPic.Columns.Add("report_product_id", Type.GetType("System.String"));
                    dcPic = tblDefectListPic.Columns.Add("url", Type.GetType("System.String"));
                    dcPic = tblDefectListPic.Columns.Add("mediaType", Type.GetType("System.String"));
                    dcPic = tblDefectListPic.Columns.Add("ordering", Type.GetType("System.Int32"));
                    dcPic = tblDefectListPic.Columns.Add("caption", Type.GetType("System.String"));



                    //add rows to tables
                    DataRow newDefectRow;
                    DataRow newPicRow;
                    int orderingDefect = 1;
                    if (fbOrderDetail.sections.workmanship != null && fbOrderDetail.sections.workmanship.workmanship_fields.workmanship_dynamic_lines.workmanship_dynamic_lines_lines != null && fbOrderDetail.sections.workmanship.workmanship_fields.workmanship_dynamic_lines.workmanship_dynamic_lines_lines.Count > 0)
                    {
                        foreach (var defectlist in fbOrderDetail.sections.workmanship.workmanship_fields.workmanship_dynamic_lines.workmanship_dynamic_lines_lines)
                        {
                            orderingDefect = orderingDefect + 1;
                            newDefectRow = tblDefectList.NewRow();
                            newDefectRow["DefectID"] = orderingDefect;
                            newDefectRow["defectvalue"] = defectlist.workmanship_dynamic_lines_defect != null ? defectlist.workmanship_dynamic_lines_defect.value : "0";
                            newDefectRow["report_product_id"] = defectlist.workmanship_dynamic_lines_defect != null ? defectlist.workmanship_dynamic_lines_defect.report_product_id : "0";
                            newDefectRow["defect_correction_actions"] = defectlist.workmanship_dynamic_lines_defect_correction_actions != null ? defectlist.workmanship_dynamic_lines_defect_correction_actions.value : "";
                            newDefectRow["defect_critical"] = defectlist.workmanship_dynamic_lines_defect_critical != null ? defectlist.workmanship_dynamic_lines_defect_critical.value : "";
                            newDefectRow["defect_description"] = defectlist.workmanship_dynamic_lines_defect_description != null ? defectlist.workmanship_dynamic_lines_defect_description.value : "";
                            newDefectRow["defect_immediate_actions"] = defectlist.workmanship_dynamic_lines_defect_immediate_actions != null ? defectlist.workmanship_dynamic_lines_defect_immediate_actions.value : "";
                            newDefectRow["defect_immediate_actions_comment"] = defectlist.workmanship_dynamic_lines_defect_immediate_actions_comment != null ? defectlist.workmanship_dynamic_lines_defect_immediate_actions_comment.value : "";
                            newDefectRow["defect_major"] = defectlist.workmanship_dynamic_lines_defect_major != null ? defectlist.workmanship_dynamic_lines_defect_major.value : "";
                            newDefectRow["defect_minor"] = defectlist.workmanship_dynamic_lines_defect_minor != null ? defectlist.workmanship_dynamic_lines_defect_minor.value : "";
                            newDefectRow["defect_root_cause_analysis"] = defectlist.workmanship_dynamic_lines_defect_root_cause_analysis != null ? defectlist.workmanship_dynamic_lines_defect_root_cause_analysis.value : "";
                            newDefectRow["position"] = defectlist.workmanship_dynamic_lines_position != null ? defectlist.workmanship_dynamic_lines_position.value : "";
                            newDefectRow["product"] = defectlist.workmanship_dynamic_lines_product != null ? defectlist.workmanship_dynamic_lines_product.value : "";
                            newDefectRow["reparability"] = defectlist.workmanship_dynamic_lines_reparability != null ? defectlist.workmanship_dynamic_lines_reparability.value : "";
                            newDefectRow["size"] = defectlist.workmanship_dynamic_lines_size != null ? defectlist.workmanship_dynamic_lines_size.value : "";
                            newDefectRow["zone"] = defectlist.workmanship_dynamic_lines_zone != null ? defectlist.workmanship_dynamic_lines_zone.value : "";
                            newDefectRow["defect_code"] = defectlist.workmanship_dynamic_lines_defect_code != null ? defectlist.workmanship_dynamic_lines_defect_code.value : "";
                            newDefectRow["defect_status"] = defectlist.workmanship_dynamic_lines_defect_status != null ? defectlist.workmanship_dynamic_lines_defect_status.value : "";
                            newDefectRow["percent"] = defectlist.workmanship_dynamic_lines_percent != null ? defectlist.workmanship_dynamic_lines_percent.value : "";
                            newDefectRow["garment_grade"] = defectlist.workmanship_dynamic_lines_garment_grade != null ? defectlist.workmanship_dynamic_lines_garment_grade.value : "";
                            newDefectRow["garment_size"] = defectlist.workmanship_dynamic_lines_garment_size != null ? defectlist.workmanship_dynamic_lines_garment_size.value : "";
                            newDefectRow["frequency_same_piece"] = defectlist.workmanship_dynamic_lines_frequency_same_piece != null ? defectlist.workmanship_dynamic_lines_frequency_same_piece.value : "";
                            newDefectRow["inspected_final_inspection"] = defectlist.workmanship_dynamic_lines_inspected_final_inspection != null ? defectlist.workmanship_dynamic_lines_inspected_final_inspection.value : "";
                            tblDefectList.Rows.Add(newDefectRow);
                            if (defectlist.workmanship_dynamic_lines_photos != null && defectlist.workmanship_dynamic_lines_photos.files != null && defectlist.workmanship_dynamic_lines_photos.files.Count > 0)
                            {
                                foreach (var defectpic in defectlist.workmanship_dynamic_lines_photos.files)
                                {
                                    newPicRow = tblDefectListPic.NewRow();
                                    newPicRow["DefectID"] = orderingDefect;
                                    newPicRow["report_product_id"] = defectlist.workmanship_dynamic_lines_photos.report_product_id;
                                    newPicRow["url"] = defectpic.url;
                                    newPicRow["mediaType"] = defectpic.mediaType;
                                    newPicRow["ordering"] = defectpic.ordering;
                                    newPicRow["caption"] = defectpic.caption;
                                    tblDefectListPic.Rows.Add(newPicRow);
                                }
                            }
                        }
                    }

                    DataSet dsDefect = new DataSet();
                    dsDefect.Tables.Add(tblDefectList);
                    dsDefect.Tables.Add(tblDefectListPic);
                    dsDefect.Relations.Add("DPLink", dsDefect.Tables[0].Columns["DefectID"], dsDefect.Tables[1].Columns["DefectID"]);
                    response.dsDefect = dsDefect;
                    #endregion

                    #region defects list -JAPAN ORDERS/APA  ORDERS
                    DataTable tblDefectList2 = new DataTable("DefectList2");
                    DataColumn dcDefect2 = null;
                    dcDefect2 = tblDefectList2.Columns.Add("ID", Type.GetType("System.Int32"));
                    dcDefect2.AutoIncrement = true;
                    dcDefect2.AutoIncrementSeed = 1;
                    dcDefect2.AutoIncrementStep = 1;
                    dcDefect2.AllowDBNull = false;
                    dcDefect2 = tblDefectList2.Columns.Add("DefectID", Type.GetType("System.Int32"));
                    dcDefect2 = tblDefectList2.Columns.Add("report_product_id", Type.GetType("System.String"));
                    dcDefect2 = tblDefectList2.Columns.Add("defect_correction_actions", Type.GetType("System.String"));
                    dcDefect2 = tblDefectList2.Columns.Add("defect_critical", Type.GetType("System.String"));
                    dcDefect2 = tblDefectList2.Columns.Add("defect_description", Type.GetType("System.String"));
                    dcDefect2 = tblDefectList2.Columns.Add("defect_immediate_actions", Type.GetType("System.String"));
                    dcDefect2 = tblDefectList2.Columns.Add("defect_immediate_actions_comment", Type.GetType("System.String"));
                    dcDefect2 = tblDefectList2.Columns.Add("defect_major", Type.GetType("System.String"));
                    dcDefect2 = tblDefectList2.Columns.Add("defect_minor", Type.GetType("System.String"));
                    dcDefect2 = tblDefectList2.Columns.Add("defect_root_cause_analysis", Type.GetType("System.String"));
                    dcDefect2 = tblDefectList2.Columns.Add("position", Type.GetType("System.String"));
                    dcDefect2 = tblDefectList2.Columns.Add("product", Type.GetType("System.String"));
                    dcDefect2 = tblDefectList2.Columns.Add("reparability", Type.GetType("System.String"));
                    dcDefect2 = tblDefectList2.Columns.Add("size", Type.GetType("System.String"));
                    dcDefect2 = tblDefectList2.Columns.Add("zone", Type.GetType("System.String"));
                    dcDefect2 = tblDefectList2.Columns.Add("defect_code", Type.GetType("System.String"));
                    dcDefect2 = tblDefectList2.Columns.Add("defect_status", Type.GetType("System.String"));
                    dcDefect2 = tblDefectList2.Columns.Add("percent", Type.GetType("System.String"));
                    dcDefect2 = tblDefectList2.Columns.Add("garment_grade", Type.GetType("System.String"));
                    dcDefect2 = tblDefectList2.Columns.Add("defectvalue", Type.GetType("System.String"));
                    dcDefect2 = tblDefectList2.Columns.Add("garment_size", Type.GetType("System.String"));
                    dcDefect2 = tblDefectList2.Columns.Add("frequency_same_piece", Type.GetType("System.String"));
                    dcDefect2 = tblDefectList2.Columns.Add("inspected_final_inspection", Type.GetType("System.String"));


                    DataTable tblDefectList2Pic = new DataTable("DefectList2Pic");
                    DataColumn dcPic2 = null;
                    dcPic2 = tblDefectList2Pic.Columns.Add("PicID", Type.GetType("System.Int32"));
                    dcPic2.AutoIncrement = true;
                    dcPic2.AutoIncrementSeed = 1;
                    dcPic2.AutoIncrementStep = 1;
                    dcPic2.AllowDBNull = false;

                    dcPic2 = tblDefectList2Pic.Columns.Add("DefectID", Type.GetType("System.Int32"));
                    dcPic2 = tblDefectList2Pic.Columns.Add("report_product_id", Type.GetType("System.String"));
                    dcPic2 = tblDefectList2Pic.Columns.Add("url", Type.GetType("System.String"));
                    dcPic2 = tblDefectList2Pic.Columns.Add("mediaType", Type.GetType("System.String"));
                    dcPic2 = tblDefectList2Pic.Columns.Add("ordering", Type.GetType("System.Int32"));
                    dcPic2 = tblDefectList2Pic.Columns.Add("caption", Type.GetType("System.String"));



                    //add rows to tables
                    DataRow newDefectRow2;
                    DataRow newPicRow2;
                    int orderingDefect2 = 1;
                    if (fbOrderDetail.sections.workmanship2 != null && fbOrderDetail.sections.workmanship2.workmanship_fields.workmanship_dynamic_lines.workmanship_dynamic_lines_lines != null && fbOrderDetail.sections.workmanship2.workmanship_fields.workmanship_dynamic_lines.workmanship_dynamic_lines_lines.Count > 0)
                    {
                        foreach (var defectlist in fbOrderDetail.sections.workmanship2.workmanship_fields.workmanship_dynamic_lines.workmanship_dynamic_lines_lines)
                        {
                            orderingDefect2 = orderingDefect2 + 1;
                            newDefectRow2 = tblDefectList2.NewRow();
                            newDefectRow2["DefectID"] = orderingDefect2;
                            newDefectRow2["defectvalue"] = defectlist.workmanship_dynamic_lines_defect != null ? defectlist.workmanship_dynamic_lines_defect.value : "0";
                            newDefectRow2["report_product_id"] = defectlist.workmanship_dynamic_lines_defect != null ? defectlist.workmanship_dynamic_lines_defect.report_product_id : "0";
                            newDefectRow2["defect_correction_actions"] = defectlist.workmanship_dynamic_lines_defect_correction_actions != null ? defectlist.workmanship_dynamic_lines_defect_correction_actions.value : "";
                            newDefectRow2["defect_critical"] = defectlist.workmanship_dynamic_lines_defect_critical != null ? defectlist.workmanship_dynamic_lines_defect_critical.value : "";
                            newDefectRow2["defect_description"] = defectlist.workmanship_dynamic_lines_defect_description != null ? defectlist.workmanship_dynamic_lines_defect_description.value : "";
                            newDefectRow2["defect_immediate_actions"] = defectlist.workmanship_dynamic_lines_defect_immediate_actions != null ? defectlist.workmanship_dynamic_lines_defect_immediate_actions.value : "";
                            newDefectRow2["defect_immediate_actions_comment"] = defectlist.workmanship_dynamic_lines_defect_immediate_actions_comment != null ? defectlist.workmanship_dynamic_lines_defect_immediate_actions_comment.value : "";
                            newDefectRow2["defect_major"] = defectlist.workmanship_dynamic_lines_defect_major != null ? defectlist.workmanship_dynamic_lines_defect_major.value : "";
                            newDefectRow2["defect_minor"] = defectlist.workmanship_dynamic_lines_defect_minor != null ? defectlist.workmanship_dynamic_lines_defect_minor.value : "";
                            newDefectRow2["defect_root_cause_analysis"] = defectlist.workmanship_dynamic_lines_defect_root_cause_analysis != null ? defectlist.workmanship_dynamic_lines_defect_root_cause_analysis.value : "";
                            newDefectRow2["position"] = defectlist.workmanship_dynamic_lines_position != null ? defectlist.workmanship_dynamic_lines_position.value : "";
                            newDefectRow2["product"] = defectlist.workmanship_dynamic_lines_product != null ? defectlist.workmanship_dynamic_lines_product.value : "";
                            newDefectRow2["reparability"] = defectlist.workmanship_dynamic_lines_reparability != null ? defectlist.workmanship_dynamic_lines_reparability.value : "";
                            newDefectRow2["size"] = defectlist.workmanship_dynamic_lines_size != null ? defectlist.workmanship_dynamic_lines_size.value : "";
                            newDefectRow2["zone"] = defectlist.workmanship_dynamic_lines_zone != null ? defectlist.workmanship_dynamic_lines_zone.value : "";
                            newDefectRow2["defect_code"] = defectlist.workmanship_dynamic_lines_defect_code != null ? defectlist.workmanship_dynamic_lines_defect_code.value : "";
                            newDefectRow2["defect_status"] = defectlist.workmanship_dynamic_lines_defect_status != null ? defectlist.workmanship_dynamic_lines_defect_status.value : "";
                            newDefectRow2["percent"] = defectlist.workmanship_dynamic_lines_percent != null ? defectlist.workmanship_dynamic_lines_percent.value : "";
                            newDefectRow2["garment_grade"] = defectlist.workmanship_dynamic_lines_garment_grade != null ? defectlist.workmanship_dynamic_lines_garment_grade.value : "";
                            newDefectRow2["garment_size"] = defectlist.workmanship_dynamic_lines_garment_size != null ? defectlist.workmanship_dynamic_lines_garment_size.value : "";
                            newDefectRow2["frequency_same_piece"] = defectlist.workmanship_dynamic_lines_frequency_same_piece != null ? defectlist.workmanship_dynamic_lines_frequency_same_piece.value : "";
                            newDefectRow2["inspected_final_inspection"] = defectlist.workmanship_dynamic_lines_inspected_final_inspection != null ? defectlist.workmanship_dynamic_lines_inspected_final_inspection.value : "";
                            tblDefectList2.Rows.Add(newDefectRow2);
                            if (defectlist.workmanship_dynamic_lines_photos != null && defectlist.workmanship_dynamic_lines_photos.files != null && defectlist.workmanship_dynamic_lines_photos.files.Count > 0)
                            {
                                foreach (var defectpic in defectlist.workmanship_dynamic_lines_photos.files)
                                {
                                    newPicRow2 = tblDefectList2Pic.NewRow();
                                    newPicRow2["DefectID"] = orderingDefect2;
                                    newPicRow2["report_product_id"] = defectlist.workmanship_dynamic_lines_photos.report_product_id;
                                    newPicRow2["url"] = defectpic.url;
                                    newPicRow2["mediaType"] = defectpic.mediaType;
                                    newPicRow2["ordering"] = defectpic.ordering;
                                    newPicRow2["caption"] = defectpic.caption;
                                    tblDefectList2Pic.Rows.Add(newPicRow2);
                                }
                            }
                        }
                    }

                    DataSet dsDefectSpec = new DataSet();
                    dsDefectSpec.Tables.Add(tblDefectList2);
                    dsDefectSpec.Tables.Add(tblDefectList2Pic);
                    dsDefectSpec.Relations.Add("DPSpecLink", dsDefectSpec.Tables[0].Columns["DefectID"], dsDefectSpec.Tables[1].Columns["DefectID"]);
                    response.dsDefectSpec = dsDefectSpec;
                    #endregion

                    #region defects list -studio_asia_workmanship_packing
                    DataTable tblDefectList3 = new DataTable("DefectList3");
                    DataColumn dcDefect3 = null;
                    dcDefect3 = tblDefectList3.Columns.Add("ID", Type.GetType("System.Int32"));
                    dcDefect3.AutoIncrement = true;
                    dcDefect3.AutoIncrementSeed = 1;
                    dcDefect3.AutoIncrementStep = 1;
                    dcDefect3.AllowDBNull = false;
                    dcDefect3 = tblDefectList3.Columns.Add("DefectID", Type.GetType("System.Int32"));
                    dcDefect3 = tblDefectList3.Columns.Add("report_product_id", Type.GetType("System.String"));
                    dcDefect3 = tblDefectList3.Columns.Add("defect_correction_actions", Type.GetType("System.String"));
                    dcDefect3 = tblDefectList3.Columns.Add("defect_critical", Type.GetType("System.String"));
                    dcDefect3 = tblDefectList3.Columns.Add("defect_description", Type.GetType("System.String"));
                    dcDefect3 = tblDefectList3.Columns.Add("defect_immediate_actions", Type.GetType("System.String"));
                    dcDefect3 = tblDefectList3.Columns.Add("defect_immediate_actions_comment", Type.GetType("System.String"));
                    dcDefect3 = tblDefectList3.Columns.Add("defect_comment", Type.GetType("System.String"));
                    dcDefect3 = tblDefectList3.Columns.Add("defect_major", Type.GetType("System.String"));
                    dcDefect3 = tblDefectList3.Columns.Add("defect_minor", Type.GetType("System.String"));
                    dcDefect3 = tblDefectList3.Columns.Add("defect_root_cause_analysis", Type.GetType("System.String"));
                    dcDefect3 = tblDefectList3.Columns.Add("position", Type.GetType("System.String"));
                    dcDefect3 = tblDefectList3.Columns.Add("product", Type.GetType("System.String"));
                    dcDefect3 = tblDefectList3.Columns.Add("reparability", Type.GetType("System.String"));
                    dcDefect3 = tblDefectList3.Columns.Add("size", Type.GetType("System.String"));
                    dcDefect3 = tblDefectList3.Columns.Add("zone", Type.GetType("System.String"));
                    dcDefect3 = tblDefectList3.Columns.Add("defect_code", Type.GetType("System.String"));
                    dcDefect3 = tblDefectList3.Columns.Add("defect_status", Type.GetType("System.String"));
                    dcDefect3 = tblDefectList3.Columns.Add("percent", Type.GetType("System.String"));
                    dcDefect3 = tblDefectList3.Columns.Add("garment_grade", Type.GetType("System.String"));
                    dcDefect3 = tblDefectList3.Columns.Add("defectvalue", Type.GetType("System.String"));
                    dcDefect3 = tblDefectList3.Columns.Add("garment_size", Type.GetType("System.String"));
                    dcDefect3 = tblDefectList3.Columns.Add("frequency_same_piece", Type.GetType("System.String"));
                    dcDefect3 = tblDefectList3.Columns.Add("inspected_final_inspection", Type.GetType("System.String"));


                    DataTable tblDefectList3Pic = new DataTable("DefectList3Pic");
                    DataColumn dcPic3 = null;
                    dcPic3 = tblDefectList3Pic.Columns.Add("PicID", Type.GetType("System.Int32"));
                    dcPic3.AutoIncrement = true;
                    dcPic3.AutoIncrementSeed = 1;
                    dcPic3.AutoIncrementStep = 1;
                    dcPic3.AllowDBNull = false;

                    dcPic3 = tblDefectList3Pic.Columns.Add("DefectID", Type.GetType("System.Int32"));
                    dcPic3 = tblDefectList3Pic.Columns.Add("report_product_id", Type.GetType("System.String"));
                    dcPic3 = tblDefectList3Pic.Columns.Add("url", Type.GetType("System.String"));
                    dcPic3 = tblDefectList3Pic.Columns.Add("mediaType", Type.GetType("System.String"));
                    dcPic3 = tblDefectList3Pic.Columns.Add("ordering", Type.GetType("System.Int32"));
                    dcPic3 = tblDefectList3Pic.Columns.Add("caption", Type.GetType("System.String"));



                    //add rows to tables
                    DataRow newDefectRow3;
                    DataRow newPicRow3;
                    int orderingDefect3 = 1;
                    if (fbOrderDetail.sections.workmanship != null && fbOrderDetail.sections.workmanship.workmanship_subSections != null && fbOrderDetail.sections.workmanship.workmanship_subSections.studio_asia_workmanship_packing != null && fbOrderDetail.sections.workmanship.workmanship_subSections.studio_asia_workmanship_packing.studio_asia_workmanship_packing_fields != null && fbOrderDetail.sections.workmanship.workmanship_subSections.studio_asia_workmanship_packing.studio_asia_workmanship_packing_fields.studio_asia_workmanship_packing_dynamic_lines != null && fbOrderDetail.sections.workmanship.workmanship_subSections.studio_asia_workmanship_packing.studio_asia_workmanship_packing_fields.studio_asia_workmanship_packing_dynamic_lines.studio_asia_workmanship_packing_dynamic_lines_lines != null && fbOrderDetail.sections.workmanship.workmanship_subSections.studio_asia_workmanship_packing.studio_asia_workmanship_packing_fields.studio_asia_workmanship_packing_dynamic_lines.studio_asia_workmanship_packing_dynamic_lines_lines.Count > 0)
                    {
                        foreach (var defectlist in fbOrderDetail.sections.workmanship.workmanship_subSections.studio_asia_workmanship_packing.studio_asia_workmanship_packing_fields.studio_asia_workmanship_packing_dynamic_lines.studio_asia_workmanship_packing_dynamic_lines_lines)
                        {
                            orderingDefect3 = orderingDefect3 + 1;
                            newDefectRow3 = tblDefectList3.NewRow();
                            newDefectRow3["DefectID"] = orderingDefect3;
                            newDefectRow3["defectvalue"] = "0";
                            newDefectRow3["report_product_id"] = "0";
                            newDefectRow3["defect_correction_actions"] = "";
                            newDefectRow3["defect_critical"] = "";
                            newDefectRow3["defect_description"] = defectlist.studio_asia_workmanship_packing_dynamic_lines_packing_defect_name != null ? defectlist.studio_asia_workmanship_packing_dynamic_lines_packing_defect_name.value : "";
                            newDefectRow3["defect_immediate_actions"] = "";
                            newDefectRow3["defect_immediate_actions_comment"] = "";
                            newDefectRow3["defect_comment"] = defectlist.studio_asia_workmanship_packing_dynamic_lines_comment != null ? defectlist.studio_asia_workmanship_packing_dynamic_lines_comment.value : "";
                            newDefectRow3["defect_major"] = defectlist.studio_asia_workmanship_packing_dynamic_lines_nb_defects != null ? defectlist.studio_asia_workmanship_packing_dynamic_lines_nb_defects.value : "";
                            newDefectRow3["defect_minor"] = "";
                            newDefectRow3["defect_root_cause_analysis"] = "";
                            newDefectRow3["position"] = "";
                            newDefectRow3["product"] = "";
                            newDefectRow3["reparability"] = "";
                            newDefectRow3["size"] = "";
                            newDefectRow3["zone"] = "";
                            newDefectRow3["defect_code"] = defectlist.studio_asia_workmanship_packing_dynamic_lines_packing_defect_code != null ? defectlist.studio_asia_workmanship_packing_dynamic_lines_packing_defect_code.value : "";
                            newDefectRow3["defect_status"] = "";
                            newDefectRow3["percent"] = "";
                            newDefectRow3["garment_grade"] = "";
                            newDefectRow3["garment_size"] = "";
                            newDefectRow3["frequency_same_piece"] = "";
                            newDefectRow3["inspected_final_inspection"] = "";
                            tblDefectList3.Rows.Add(newDefectRow3);
                            if (defectlist.studio_asia_workmanship_packing_dynamic_lines_packing_defect_photos != null && defectlist.studio_asia_workmanship_packing_dynamic_lines_packing_defect_photos.files != null && defectlist.studio_asia_workmanship_packing_dynamic_lines_packing_defect_photos.files.Count > 0)
                            {
                                foreach (var defectpic in defectlist.studio_asia_workmanship_packing_dynamic_lines_packing_defect_photos.files)
                                {
                                    newPicRow3 = tblDefectList3Pic.NewRow();
                                    newPicRow3["DefectID"] = orderingDefect3;
                                    newPicRow3["report_product_id"] = "0";
                                    newPicRow3["url"] = defectpic.url;
                                    newPicRow3["mediaType"] = defectpic.mediaType;
                                    newPicRow3["ordering"] = defectpic.ordering;
                                    newPicRow3["caption"] = defectpic.caption;
                                    tblDefectList3Pic.Rows.Add(newPicRow3);
                                }
                            }
                        }
                    }

                    DataSet dsDefectPacking = new DataSet();
                    dsDefectPacking.Tables.Add(tblDefectList3);
                    dsDefectPacking.Tables.Add(tblDefectList3Pic);
                    dsDefectPacking.Relations.Add("DPPackingLink", dsDefectPacking.Tables[0].Columns["DefectID"], dsDefectPacking.Tables[1].Columns["DefectID"]);
                    response.dsDefectPacking = dsDefectPacking;
                    #endregion

                    #region MSChart list
                    DataTable tblMSchartProduct = new DataTable("MSchartProduct");
                    DataColumn dcMSchartProduct = null;
                    dcMSchartProduct = tblMSchartProduct.Columns.Add("ProID", Type.GetType("System.Int32"));
                    dcMSchartProduct.AutoIncrement = true;
                    dcMSchartProduct.AutoIncrementSeed = 1;
                    dcMSchartProduct.AutoIncrementStep = 1;
                    dcMSchartProduct.AllowDBNull = false;
                    dcMSchartProduct = tblMSchartProduct.Columns.Add("report_product_id", Type.GetType("System.Int32"));
                    dcMSchartProduct = tblMSchartProduct.Columns.Add("unit", Type.GetType("System.String"));
                    dcMSchartProduct = tblMSchartProduct.Columns.Add("grandtotal_pieces_measured", Type.GetType("System.Int32"));
                    dcMSchartProduct = tblMSchartProduct.Columns.Add("grandtotal_out_of_tolerance", Type.GetType("System.Int32"));




                    DataTable tblMSchartColor = new DataTable("MSchartColor");
                    DataColumn dcMSchartColor = null;
                    dcMSchartColor = tblMSchartColor.Columns.Add("ColorID", Type.GetType("System.Int32"));
                    dcMSchartColor.AutoIncrement = true;
                    dcMSchartColor.AutoIncrementSeed = 1;
                    dcMSchartColor.AutoIncrementStep = 1;
                    dcMSchartColor.AllowDBNull = false;

                    dcMSchartColor = tblMSchartColor.Columns.Add("report_product_id", Type.GetType("System.Int32"));
                    dcMSchartColor = tblMSchartColor.Columns.Add("total_pieces_measured", Type.GetType("System.Int32"));
                    dcMSchartColor = tblMSchartColor.Columns.Add("total_out_of_tolerance", Type.GetType("System.Int32"));
                    dcMSchartColor = tblMSchartColor.Columns.Add("size", Type.GetType("System.String"));
                    dcMSchartColor = tblMSchartColor.Columns.Add("color", Type.GetType("System.String"));
                    dcMSchartColor = tblMSchartColor.Columns.Add("samples", Type.GetType("System.Int32"));
                    dcMSchartColor = tblMSchartColor.Columns.Add("ordering", Type.GetType("System.Int32"));

                    DataTable tblMSchartPoints = new DataTable("MSchartPoints");
                    DataColumn dcMSchartPoints = null;
                    dcMSchartPoints = tblMSchartPoints.Columns.Add("PointsID", Type.GetType("System.Int32"));
                    dcMSchartPoints.AutoIncrement = true;
                    dcMSchartPoints.AutoIncrementSeed = 1;
                    dcMSchartPoints.AutoIncrementStep = 1;
                    dcMSchartPoints.AllowDBNull = false;

                    dcMSchartPoints = tblMSchartPoints.Columns.Add("ColorID", Type.GetType("System.Int32"));
                    dcMSchartPoints = tblMSchartPoints.Columns.Add("title", Type.GetType("System.String"));
                    dcMSchartPoints = tblMSchartPoints.Columns.Add("required", Type.GetType("System.String"));
                    dcMSchartPoints = tblMSchartPoints.Columns.Add("tolerance_minus", Type.GetType("System.String"));
                    dcMSchartPoints = tblMSchartPoints.Columns.Add("tolerance_plus", Type.GetType("System.String"));
                    dcMSchartPoints = tblMSchartPoints.Columns.Add("tolerance_plus_percent", Type.GetType("System.Boolean"));
                    dcMSchartPoints = tblMSchartPoints.Columns.Add("tolerance_minus_percent", Type.GetType("System.Boolean"));
                    dcMSchartPoints = tblMSchartPoints.Columns.Add("subtotal_pieces_measured", Type.GetType("System.Int32"));
                    dcMSchartPoints = tblMSchartPoints.Columns.Add("subtotal_out_of_tolerance", Type.GetType("System.Int32"));
                    dcMSchartPoints = tblMSchartPoints.Columns.Add("ordering", Type.GetType("System.Int32"));

                    DataTable tblMSchartSamplesColumnName = new DataTable("MSchartSamplesColumnName");
                    DataColumn dcMSchartSamplesColumnName = null;
                    dcMSchartSamplesColumnName = tblMSchartSamplesColumnName.Columns.Add("SamplesColumnNameID", Type.GetType("System.Int32"));
                    dcMSchartSamplesColumnName.AutoIncrement = true;
                    dcMSchartSamplesColumnName.AutoIncrementSeed = 1;
                    dcMSchartSamplesColumnName.AutoIncrementStep = 1;
                    dcMSchartSamplesColumnName.AllowDBNull = false;

                    dcMSchartSamplesColumnName = tblMSchartSamplesColumnName.Columns.Add("ColorID", Type.GetType("System.Int32"));
                    dcMSchartSamplesColumnName = tblMSchartSamplesColumnName.Columns.Add("title", Type.GetType("System.String"));
                    dcMSchartSamplesColumnName = tblMSchartSamplesColumnName.Columns.Add("ordering", Type.GetType("System.Int32"));

                    DataTable tblMSchartSamples = new DataTable("MSchartSamples");
                    DataColumn dcMSchartSamples = null;
                    dcMSchartSamples = tblMSchartSamples.Columns.Add("SamplesID", Type.GetType("System.Int32"));
                    dcMSchartSamples.AutoIncrement = true;
                    dcMSchartSamples.AutoIncrementSeed = 1;
                    dcMSchartSamples.AutoIncrementStep = 1;
                    dcMSchartSamples.AllowDBNull = false;

                    dcMSchartSamples = tblMSchartSamples.Columns.Add("PointsID", Type.GetType("System.Int32"));
                    dcMSchartSamples = tblMSchartSamples.Columns.Add("value", Type.GetType("System.String"));
                    dcMSchartSamples = tblMSchartSamples.Columns.Add("is_out_of_tolerance", Type.GetType("System.Boolean"));


                    //add rows to tables
                    DataRow newMSProductRow;
                    DataRow newMSColorRow;
                    DataRow newMSPointsRow;
                    DataRow newMSSamplesRow, newMSSamplesColumnNameRow;
                    int orderingColor = 1;
                    int orderingPoint = 1;
                    int orderingSamplesColumnName = 1;
                    if (fbOrderDetail.sections.measurement_and_fitting != null && fbOrderDetail.sections.measurement_and_fitting.measurement_and_fitting_subSections.measurement_and_fitting_results.measurement_and_fitting_results_fields.measurement_and_fitting_results_measurement_fitting != null)
                    {
                        foreach (var MSProductList in fbOrderDetail.sections.measurement_and_fitting.measurement_and_fitting_subSections.measurement_and_fitting_results.measurement_and_fitting_results_fields.measurement_and_fitting_results_measurement_fitting)
                        {
                            if (MSProductList.measurement != null)
                            {
                                newMSProductRow = tblMSchartProduct.NewRow();
                                newMSProductRow["report_product_id"] = MSProductList.report_product_id != null ? MSProductList.report_product_id : 0;
                                newMSProductRow["unit"] = MSProductList.measurement.unit != null ? MSProductList.measurement.unit : "";
                                newMSProductRow["grandtotal_pieces_measured"] = MSProductList.measurement.grandtotal_pieces_measured != null ? MSProductList.measurement.grandtotal_pieces_measured : 0;
                                newMSProductRow["grandtotal_out_of_tolerance"] = MSProductList.measurement.grandtotal_out_of_tolerance != null ? MSProductList.measurement.grandtotal_out_of_tolerance : 0;
                                tblMSchartProduct.Rows.Add(newMSProductRow);

                                if (MSProductList.measurement.tables != null && MSProductList.measurement.tables.Count > 0)
                                {
                                    foreach (var MSchartColorList in MSProductList.measurement.tables)
                                    {
                                        orderingColor = orderingColor + 1;
                                        newMSColorRow = tblMSchartColor.NewRow();
                                        newMSColorRow["report_product_id"] = MSProductList.report_product_id != null ? MSProductList.report_product_id : 0;
                                        newMSColorRow["total_pieces_measured"] = MSchartColorList.total_pieces_measured != null ? MSchartColorList.total_pieces_measured : 0;
                                        newMSColorRow["total_out_of_tolerance"] = MSchartColorList.total_out_of_tolerance != null ? MSchartColorList.total_out_of_tolerance : 0;
                                        newMSColorRow["size"] = MSchartColorList.size != null ? MSchartColorList.size : "";
                                        newMSColorRow["color"] = MSchartColorList.color != null ? MSchartColorList.color : "";
                                        newMSColorRow["samples"] = MSchartColorList.samples != null ? MSchartColorList.samples : 0;
                                        newMSColorRow["ordering"] = orderingColor;
                                        tblMSchartColor.Rows.Add(newMSColorRow);

                                        //samples name
                                        if (MSchartColorList.samples_names != null && MSchartColorList.samples_names.Count > 0)
                                        {
                                            foreach (var MSchartSamplesColumnName in MSchartColorList.samples_names)
                                            {
                                                orderingSamplesColumnName = orderingSamplesColumnName + 1;
                                                newMSSamplesColumnNameRow = tblMSchartSamplesColumnName.NewRow();
                                                newMSSamplesColumnNameRow["ColorID"] = orderingColor;
                                                newMSSamplesColumnNameRow["title"] = !string.IsNullOrEmpty(MSchartSamplesColumnName) ? MSchartSamplesColumnName : "Sample" + orderingSamplesColumnName.ToString();
                                                newMSSamplesColumnNameRow["ordering"] = orderingSamplesColumnName;
                                                tblMSchartSamplesColumnName.Rows.Add(newMSSamplesColumnNameRow);
                                            }
                                        }

                                        if (MSchartColorList.points != null && MSchartColorList.points.Count > 0)
                                        {
                                            foreach (var MSchartPointsList in MSchartColorList.points)
                                            {
                                                orderingPoint = orderingPoint + 1;
                                                newMSPointsRow = tblMSchartPoints.NewRow();
                                                newMSPointsRow["ColorID"] = orderingColor;
                                                newMSPointsRow["title"] = MSchartPointsList.title != null ? MSchartPointsList.title : "";
                                                newMSPointsRow["required"] = MSchartPointsList.required != null ? MSchartPointsList.required : "";
                                                newMSPointsRow["tolerance_minus"] = MSchartPointsList.tolerance_minus != null ? MSchartPointsList.tolerance_minus : "";
                                                newMSPointsRow["tolerance_plus"] = MSchartPointsList.tolerance_plus != null ? MSchartPointsList.tolerance_plus : "";
                                                newMSPointsRow["tolerance_plus_percent"] = MSchartPointsList.tolerance_plus_percent;
                                                newMSPointsRow["tolerance_minus_percent"] = MSchartPointsList.tolerance_minus_percent;
                                                newMSPointsRow["subtotal_pieces_measured"] = MSchartPointsList.subtotal_pieces_measured != null ? MSchartPointsList.subtotal_pieces_measured : 0;
                                                newMSPointsRow["subtotal_out_of_tolerance"] = MSchartPointsList.subtotal_out_of_tolerance != null ? MSchartPointsList.subtotal_out_of_tolerance : 0;
                                                newMSPointsRow["ordering"] = orderingPoint;
                                                tblMSchartPoints.Rows.Add(newMSPointsRow);

                                                if (MSchartPointsList.samples != null && MSchartPointsList.samples.Count > 0)
                                                {
                                                    foreach (var MSchartSamplesList in MSchartPointsList.samples)
                                                    {
                                                        newMSSamplesRow = tblMSchartSamples.NewRow();
                                                        newMSSamplesRow["PointsID"] = orderingPoint;
                                                        newMSSamplesRow["value"] = MSchartSamplesList.value != null ? MSchartSamplesList.value : "";
                                                        newMSSamplesRow["is_out_of_tolerance"] = MSchartSamplesList.is_out_of_tolerance;
                                                        tblMSchartSamples.Rows.Add(newMSSamplesRow);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    DataSet dsMSChart = new DataSet();
                    dsMSChart.Tables.Add(tblMSchartProduct);
                    dsMSChart.Tables.Add(tblMSchartColor);
                    dsMSChart.Tables.Add(tblMSchartPoints);
                    dsMSChart.Tables.Add(tblMSchartSamples);
                    dsMSChart.Tables.Add(tblMSchartSamplesColumnName);
                    dsMSChart.Relations.Add("MSPCLink", dsMSChart.Tables[0].Columns["report_product_id"], dsMSChart.Tables[1].Columns["report_product_id"]);
                    dsMSChart.Relations.Add("MSCPLink", dsMSChart.Tables[1].Columns["ordering"], dsMSChart.Tables[2].Columns["ColorID"]);
                    dsMSChart.Relations.Add("MSPSLink", dsMSChart.Tables[2].Columns["ordering"], dsMSChart.Tables[3].Columns["PointsID"]);
                    dsMSChart.Relations.Add("MSSMColLink", dsMSChart.Tables[1].Columns["ordering"], dsMSChart.Tables[4].Columns["ColorID"]);
                    response.dsMSChart = dsMSChart;
                    #endregion

                    #region EDEN PARK - Colorway / Size Range details
                    #region create  tables
                    //Fit
                    DataTable tblColorwayFit = new DataTable("ColorwayFit");
                    DataColumn dcColorwayFit = null;
                    dcColorwayFit = tblColorwayFit.Columns.Add("FitID", Type.GetType("System.Int32"));
                    dcColorwayFit.AutoIncrement = true;
                    dcColorwayFit.AutoIncrementSeed = 1;
                    dcColorwayFit.AutoIncrementStep = 1;
                    dcColorwayFit.AllowDBNull = false;
                    dcColorwayFit = tblColorwayFit.Columns.Add("colorway_fit", Type.GetType("System.String"));
                    dcColorwayFit = tblColorwayFit.Columns.Add("grandtotal", Type.GetType("System.String"));
                    dcColorwayFit = tblColorwayFit.Columns.Add("ordering", Type.GetType("System.Int32"));

                    //Size Range details
                    DataTable tblColorwayFitSize = new DataTable("ColorwayFitSize");
                    DataColumn dcColorwayFitSize = null;
                    dcColorwayFitSize = tblColorwayFitSize.Columns.Add("CFSizeID", Type.GetType("System.Int32"));
                    dcColorwayFitSize.AutoIncrement = true;
                    dcColorwayFitSize.AutoIncrementSeed = 1;
                    dcColorwayFitSize.AutoIncrementStep = 1;
                    dcColorwayFitSize.AllowDBNull = false;
                    dcColorwayFitSize = tblColorwayFitSize.Columns.Add("FitID", Type.GetType("System.Int32"));
                    dcColorwayFitSize = tblColorwayFitSize.Columns.Add("value", Type.GetType("System.String"));
                    dcColorwayFitSize = tblColorwayFitSize.Columns.Add("size", Type.GetType("System.String"));
                    dcColorwayFitSize = tblColorwayFitSize.Columns.Add("report_product_id", Type.GetType("System.Int32"));

                    DataRow newColorwayFitRow, newColorwayFitSizeRow;
                    int orderingColorwayFit = 0;
                    if (OrderDetail[0].result_and_conclusion != null && OrderDetail[0].result_and_conclusion.result_and_conclusion_fields != null && OrderDetail[0].result_and_conclusion.result_and_conclusion_fields.result_and_conclusion_colorway_size_range != null)
                    {
                        var colorway_size_range = OrderDetail[0].result_and_conclusion.result_and_conclusion_fields.result_and_conclusion_colorway_size_range;
                        if (colorway_size_range.result_and_conclusion_colorway_size_range_lines != null && colorway_size_range.result_and_conclusion_colorway_size_range_lines.Count > 0)
                        {

                            foreach (var CWSize in colorway_size_range.result_and_conclusion_colorway_size_range_lines)
                            {
                                #region  Fit 
                                orderingColorwayFit = orderingColorwayFit + 1;
                                newColorwayFitRow = tblColorwayFit.NewRow();
                                newColorwayFitRow["colorway_fit"] = CWSize.result_and_conclusion_colorway_size_range_colorway_fit != null && CWSize.result_and_conclusion_colorway_size_range_colorway_fit.value != null ? CWSize.result_and_conclusion_colorway_size_range_colorway_fit.value : "";
                                newColorwayFitRow["grandtotal"] = CWSize.result_and_conclusion_colorway_size_range_colorway_grandtotal != null && CWSize.result_and_conclusion_colorway_size_range_colorway_grandtotal.value != null ? CWSize.result_and_conclusion_colorway_size_range_colorway_grandtotal.value : "";
                                newColorwayFitRow["ordering"] = orderingColorwayFit;
                                tblColorwayFit.Rows.Add(newColorwayFitRow);
                                #endregion

                                #region Size Range
                                if (CWSize.result_and_conclusion_colorway_size_range_colorway_product_size_total != null && CWSize.result_and_conclusion_colorway_size_range_colorway_product_size_total.Count > 0)
                                {
                                    foreach (var sizeRange in CWSize.result_and_conclusion_colorway_size_range_colorway_product_size_total)
                                    {
                                        newColorwayFitSizeRow = tblColorwayFitSize.NewRow();
                                        newColorwayFitSizeRow["value"] = sizeRange.value != null ? sizeRange.value : "";
                                        newColorwayFitSizeRow["size"] = sizeRange.size != null ? sizeRange.size : "";
                                        int sizeRange_rpid = 0;
                                        if (sizeRange.report_product_id != null)
                                        {
                                            int.TryParse(sizeRange.report_product_id, out sizeRange_rpid);
                                        }
                                        else
                                        {
                                            sizeRange_rpid = 0;
                                        }
                                        newColorwayFitSizeRow["report_product_id"] = sizeRange_rpid;
                                        newColorwayFitSizeRow["FitID"] = orderingColorwayFit;
                                        tblColorwayFitSize.Rows.Add(newColorwayFitSizeRow);
                                    }

                                }
                                #endregion

                            }
                        }
                    }

                    DataSet dsColorway = new DataSet();
                    dsColorway.Tables.Add(tblColorwayFit);
                    dsColorway.Tables.Add(tblColorwayFitSize);
                    dsColorway.Relations.Add("CFSLink", dsColorway.Tables[0].Columns["ordering"], dsColorway.Tables[1].Columns["FitID"]);
                    response.dsColorway = dsColorway;
                    #endregion
                    #endregion 

                    #region Fabric

                    #region  Fabric Inspection Analysis-> Result for Inspected Qty in Rolls 
                    DataTable ChartResultQtyinYards = new DataTable();
                    DataColumn dcChartResultQtyinYards = null;
                    dcChartResultQtyinYards = ChartResultQtyinYards.Columns.Add("category", Type.GetType("System.String"));
                    dcChartResultQtyinYards = ChartResultQtyinYards.Columns.Add("value", Type.GetType("System.Int32"));
                    if (OrderDetail[0].result_and_conclusion!=null && OrderDetail[0].result_and_conclusion.result_and_conclusion_fields.result_and_conclusion_amcharts_fabric_inspection_summary != null && OrderDetail[0].result_and_conclusion.result_and_conclusion_fields.result_and_conclusion_amcharts_fabric_inspection_summary.result_and_conclusion_amcharts_fabric_inspection_summary_value != null && OrderDetail[0].result_and_conclusion.result_and_conclusion_fields.result_and_conclusion_amcharts_fabric_inspection_summary.result_and_conclusion_amcharts_fabric_inspection_summary_value.Count > 0)
                    {
                        ChartResultQtyinYards = new DataTable();
                        ChartResultQtyinYards = _helper.ConvertToDataTable(OrderDetail[0].result_and_conclusion.result_and_conclusion_fields.result_and_conclusion_amcharts_fabric_inspection_summary.result_and_conclusion_amcharts_fabric_inspection_summary_value);
                    }
                    response.ChartResultQtyinYards = ChartResultQtyinYards;
                    #endregion

                    #region  Fabric Inspection Analysis-> Result for Inspected Qty in Rolls
                    DataTable ChartResultQtyinRolls = new DataTable();
                    DataColumn dcChartResultQtyinRolls = null;
                    dcChartResultQtyinRolls = ChartResultQtyinRolls.Columns.Add("category", Type.GetType("System.String"));
                    dcChartResultQtyinRolls = ChartResultQtyinRolls.Columns.Add("value", Type.GetType("System.Int32"));
                    if (OrderDetail[0].result_and_conclusion != null && OrderDetail[0].result_and_conclusion.result_and_conclusion_fields.result_and_conclusion_amcharts_fabric_inspection_detail != null && OrderDetail[0].result_and_conclusion.result_and_conclusion_fields.result_and_conclusion_amcharts_fabric_inspection_detail.result_and_conclusion_amcharts_fabric_inspection_detail_value != null && OrderDetail[0].result_and_conclusion.result_and_conclusion_fields.result_and_conclusion_amcharts_fabric_inspection_detail.result_and_conclusion_amcharts_fabric_inspection_detail_value.Count > 0)
                    {
                        ChartResultQtyinRolls = new DataTable();
                        ChartResultQtyinRolls = _helper.ConvertToDataTable(OrderDetail[0].result_and_conclusion.result_and_conclusion_fields.result_and_conclusion_amcharts_fabric_inspection_detail.result_and_conclusion_amcharts_fabric_inspection_detail_value);
                    }
                    response.ChartResultQtyinRolls = ChartResultQtyinRolls;
                    #endregion

                    #region Fabric Inspection Detail

                    #region  create table FabricDefectColor
                    DataTable tblFabricDefectColor = new DataTable("FabricDefectColor");
                    DataColumn dcFabricDefectColor = null;
                    dcFabricDefectColor = tblFabricDefectColor.Columns.Add("ID", Type.GetType("System.Int32"));
                    dcFabricDefectColor.AutoIncrement = true;
                    dcFabricDefectColor.AutoIncrementSeed = 1;
                    dcFabricDefectColor.AutoIncrementStep = 1;
                    dcFabricDefectColor.AllowDBNull = false;
                    dcFabricDefectColor = tblFabricDefectColor.Columns.Add("report_product_id", Type.GetType("System.Int32"));
                    dcFabricDefectColor = tblFabricDefectColor.Columns.Add("color", Type.GetType("System.String"));
                    dcFabricDefectColor = tblFabricDefectColor.Columns.Add("Result", Type.GetType("System.String"));
                    dcFabricDefectColor = tblFabricDefectColor.Columns.Add("acceptance_criteria", Type.GetType("System.String"));
                    dcFabricDefectColor = tblFabricDefectColor.Columns.Add("no_of_points_100_sqy", Type.GetType("System.String"));
                    dcFabricDefectColor = tblFabricDefectColor.Columns.Add("classification_copy_original_to_actual", Type.GetType("System.String"));
                    dcFabricDefectColor = tblFabricDefectColor.Columns.Add("classification_width", Type.GetType("System.String"));

                    //table FabricDefectPhotos
                    DataTable tblFabricDefectPhotos = new DataTable("FabricDefectPhotos");
                    DataColumn dcFabricDefectPhotos = null;
                    dcFabricDefectPhotos = tblFabricDefectPhotos.Columns.Add("DefPicID", Type.GetType("System.Int32"));
                    dcFabricDefectPhotos.AutoIncrement = true;
                    dcFabricDefectPhotos.AutoIncrementSeed = 1;
                    dcFabricDefectPhotos.AutoIncrementStep = 1;
                    dcFabricDefectPhotos.AllowDBNull = false;
                    dcFabricDefectPhotos = tblFabricDefectPhotos.Columns.Add("report_product_id", Type.GetType("System.Int32"));
                    dcFabricDefectPhotos = tblFabricDefectPhotos.Columns.Add("url", Type.GetType("System.String"));
                    dcFabricDefectPhotos = tblFabricDefectPhotos.Columns.Add("mediaType", Type.GetType("System.String"));
                    dcFabricDefectPhotos = tblFabricDefectPhotos.Columns.Add("ordering", Type.GetType("System.Int32"));
                    dcFabricDefectPhotos = tblFabricDefectPhotos.Columns.Add("caption", Type.GetType("System.String"));


                    //table FabricInspectionDetail
                    DataTable tblFabricInspectionDetail = new DataTable("FabricInspectionDetail");
                    DataColumn dcFabricInspectionDetail = null;
                    dcFabricInspectionDetail = tblFabricInspectionDetail.Columns.Add("TestID", Type.GetType("System.Int32"));
                    dcFabricInspectionDetail.AutoIncrement = true;
                    dcFabricInspectionDetail.AutoIncrementSeed = 1;
                    dcFabricInspectionDetail.AutoIncrementStep = 1;
                    dcFabricInspectionDetail.AllowDBNull = false;

                    dcFabricInspectionDetail = tblFabricInspectionDetail.Columns.Add("report_product_id", Type.GetType("System.Int32"));
                    dcFabricInspectionDetail = tblFabricInspectionDetail.Columns.Add("color", Type.GetType("System.String"));
                    dcFabricInspectionDetail = tblFabricInspectionDetail.Columns.Add("acceptance_criteria", Type.GetType("System.String"));
                    dcFabricInspectionDetail = tblFabricInspectionDetail.Columns.Add("length_original", Type.GetType("System.String"));
                    dcFabricInspectionDetail = tblFabricInspectionDetail.Columns.Add("no_of_defect", Type.GetType("System.String"));
                    dcFabricInspectionDetail = tblFabricInspectionDetail.Columns.Add("no_of_points", Type.GetType("System.String"));
                    dcFabricInspectionDetail = tblFabricInspectionDetail.Columns.Add("roll_number", Type.GetType("System.String"));
                    dcFabricInspectionDetail = tblFabricInspectionDetail.Columns.Add("weight_actual", Type.GetType("System.String"));
                    dcFabricInspectionDetail = tblFabricInspectionDetail.Columns.Add("weight_original", Type.GetType("System.String"));
                    dcFabricInspectionDetail = tblFabricInspectionDetail.Columns.Add("width_original", Type.GetType("System.String"));
                    dcFabricInspectionDetail = tblFabricInspectionDetail.Columns.Add("comment", Type.GetType("System.String"));
                    dcFabricInspectionDetail = tblFabricInspectionDetail.Columns.Add("dye_lot", Type.GetType("System.String"));
                    dcFabricInspectionDetail = tblFabricInspectionDetail.Columns.Add("dye_lot_default", Type.GetType("System.String"));
                    dcFabricInspectionDetail = tblFabricInspectionDetail.Columns.Add("length_actual", Type.GetType("System.String"));
                    dcFabricInspectionDetail = tblFabricInspectionDetail.Columns.Add("no_of_points_100_sqy", Type.GetType("System.String"));
                    dcFabricInspectionDetail = tblFabricInspectionDetail.Columns.Add("result", Type.GetType("System.String"));
                    dcFabricInspectionDetail = tblFabricInspectionDetail.Columns.Add("production_sample_cutted", Type.GetType("System.String"));
                    dcFabricInspectionDetail = tblFabricInspectionDetail.Columns.Add("production_sample_cutted_Image", Type.GetType("System.String"));
                    dcFabricInspectionDetail = tblFabricInspectionDetail.Columns.Add("width_actual", Type.GetType("System.String"));


                    //table FabricInspectionPhotos
                    DataTable tblFabricInspectionPhotos = new DataTable("FabricInspectionPhotos");
                    DataColumn dcFabricInspectionPhotos = null;
                    dcFabricInspectionPhotos = tblFabricInspectionPhotos.Columns.Add("InspPicID", Type.GetType("System.Int32"));
                    dcFabricInspectionPhotos.AutoIncrement = true;
                    dcFabricInspectionPhotos.AutoIncrementSeed = 1;
                    dcFabricInspectionPhotos.AutoIncrementStep = 1;
                    dcFabricInspectionPhotos.AllowDBNull = false;
                    dcFabricInspectionPhotos = tblFabricInspectionPhotos.Columns.Add("report_product_id", Type.GetType("System.Int32"));
                    dcFabricInspectionPhotos = tblFabricInspectionPhotos.Columns.Add("TestID", Type.GetType("System.Int32"));
                    dcFabricInspectionPhotos = tblFabricInspectionPhotos.Columns.Add("url", Type.GetType("System.String"));
                    dcFabricInspectionPhotos = tblFabricInspectionPhotos.Columns.Add("mediaType", Type.GetType("System.String"));
                    dcFabricInspectionPhotos = tblFabricInspectionPhotos.Columns.Add("ordering", Type.GetType("System.Int32"));
                    dcFabricInspectionPhotos = tblFabricInspectionPhotos.Columns.Add("caption", Type.GetType("System.String"));
                    dcFabricInspectionPhotos = tblFabricInspectionPhotos.Columns.Add("marknumber", Type.GetType("System.Int32"));

                    DataTable tblFInspPhotos = new DataTable("FabInspPhotos");
                    DataColumn dcFInspPhotos = null;
                    dcFInspPhotos = tblFInspPhotos.Columns.Add("PicID", Type.GetType("System.Int32"));
                    dcFInspPhotos.AutoIncrement = true;
                    dcFInspPhotos.AutoIncrementSeed = 1;
                    dcFInspPhotos.AutoIncrementStep = 1;
                    dcFInspPhotos.AllowDBNull = false;
                    dcFInspPhotos = tblFInspPhotos.Columns.Add("report_product_id", Type.GetType("System.Int32"));
                    dcFInspPhotos = tblFInspPhotos.Columns.Add("TestID", Type.GetType("System.Int32"));
                    dcFInspPhotos = tblFInspPhotos.Columns.Add("url", Type.GetType("System.String"));
                    dcFInspPhotos = tblFInspPhotos.Columns.Add("mediaType", Type.GetType("System.String"));
                    dcFInspPhotos = tblFInspPhotos.Columns.Add("ordering", Type.GetType("System.Int32"));
                    dcFInspPhotos = tblFInspPhotos.Columns.Add("caption", Type.GetType("System.String"));
                    dcFInspPhotos = tblFInspPhotos.Columns.Add("url2", Type.GetType("System.String"));
                    dcFInspPhotos = tblFInspPhotos.Columns.Add("mediaType2", Type.GetType("System.String"));
                    dcFInspPhotos = tblFInspPhotos.Columns.Add("ordering2", Type.GetType("System.Int32"));
                    dcFInspPhotos = tblFInspPhotos.Columns.Add("caption2", Type.GetType("System.String"));

                    //table FabricDefects
                    DataTable tblFabricDefects = new DataTable("FabricDefects");
                    DataColumn dcFabricDefects = null;
                    dcFabricDefects = tblFabricDefects.Columns.Add("DefID", Type.GetType("System.Int32"));
                    dcFabricDefects.AutoIncrement = true;
                    dcFabricDefects.AutoIncrementSeed = 1;
                    dcFabricDefects.AutoIncrementStep = 1;
                    dcFabricDefects.AllowDBNull = false;
                    dcFabricDefects = tblFabricDefects.Columns.Add("report_product_id", Type.GetType("System.Int32"));
                    dcFabricDefects = tblFabricDefects.Columns.Add("TestID", Type.GetType("System.Int32"));
                    dcFabricDefects = tblFabricDefects.Columns.Add("location", Type.GetType("System.String"));
                    dcFabricDefects = tblFabricDefects.Columns.Add("point", Type.GetType("System.String"));
                    dcFabricDefects = tblFabricDefects.Columns.Add("defect", Type.GetType("System.String"));
                    dcFabricDefects = tblFabricDefects.Columns.Add("defectDescription", Type.GetType("System.String"));


                    #endregion

                    #region add rows to tables Fabric Inspection Detail
                    DataRow newFabricDefectColorRow, newFabricDefectPhotosRow;
                    DataRow newFabInspDetaillRow, newFInspPhotosRow, newFDefRow, newFIPhotosRow;
                    int orderingFabricInspectionDetail = 0;
                    if (fbOrderDetail.sections.fabric_defects_classification != null && fbOrderDetail.sections.fabric_defects_classification.fabric_defects_classification_fields != null)
                    {
                        if (fbOrderDetail.products != null && fbOrderDetail.products.Count > 0)
                        {
                            var defclassfields = fbOrderDetail.sections.fabric_defects_classification.fabric_defects_classification_fields;
                            foreach (var productslist in fbOrderDetail.products)
                            {
                                newFabricDefectColorRow = tblFabricDefectColor.NewRow();
                                newFabricDefectColorRow["report_product_id"] = productslist.report_product_id;
                                newFabricDefectColorRow["color"] = productslist.color;
                                #region  get result/100_sqy/acceptance_criteria

                                if (defclassfields.fabric_defects_classification_color_result != null && defclassfields.fabric_defects_classification_color_result.Count > 0)
                                {
                                    var color_result = (defclassfields.fabric_defects_classification_color_result).Find(t => t.report_product_id == productslist.report_product_id);
                                    newFabricDefectColorRow["Result"] = color_result != null ? color_result.value : "";
                                }

                                if (defclassfields.fabric_defects_classification_color_acceptance_criteria != null && defclassfields.fabric_defects_classification_color_acceptance_criteria.Count > 0)
                                {
                                    var acceptance_criteria = (defclassfields.fabric_defects_classification_color_acceptance_criteria).Find(t => t.report_product_id == productslist.report_product_id);
                                    newFabricDefectColorRow["acceptance_criteria"] = acceptance_criteria != null ? acceptance_criteria.value : "";
                                }
                                if (defclassfields.fabric_defects_classification_color_no_of_points_100_sqy != null && defclassfields.fabric_defects_classification_color_no_of_points_100_sqy.Count > 0)
                                {
                                    var no_of_points_100_sqy = (defclassfields.fabric_defects_classification_color_no_of_points_100_sqy).Find(t => t.report_product_id == productslist.report_product_id);
                                    newFabricDefectColorRow["no_of_points_100_sqy"] = no_of_points_100_sqy != null ? no_of_points_100_sqy.value : "";
                                }
                                if (defclassfields.fabric_defects_classification_copy_original_to_actual != null && defclassfields.fabric_defects_classification_copy_original_to_actual.Count > 0)
                                {
                                    var original_to_actual = (defclassfields.fabric_defects_classification_copy_original_to_actual).Find(t => t.report_product_id == productslist.report_product_id);
                                    newFabricDefectColorRow["classification_copy_original_to_actual"] = original_to_actual != null ? original_to_actual.value : "";
                                }
                                if (defclassfields.fabric_defects_classification_width != null && defclassfields.fabric_defects_classification_width.Count > 0)
                                {
                                    var classification_width = (defclassfields.fabric_defects_classification_width).Find(t => t.report_product_id == productslist.report_product_id);
                                    newFabricDefectColorRow["classification_width"] = classification_width != null ? classification_width.value : "";
                                }
                                #endregion
                                tblFabricDefectColor.Rows.Add(newFabricDefectColorRow);

                                #region  get FabricDefectPhotos
                                if (defclassfields.fabric_defects_classification_defect_photos != null && defclassfields.fabric_defects_classification_defect_photos.Count > 0)
                                {
                                    var FDefectPhotos = defclassfields.fabric_defects_classification_defect_photos;
                                    foreach (var lstFDefectPhotos in FDefectPhotos)
                                    {
                                        if (productslist.report_product_id == lstFDefectPhotos.report_product_id)
                                        {

                                            if (lstFDefectPhotos.files != null && lstFDefectPhotos.files.Count > 0)
                                            {

                                                foreach (var lstFDefPhotos in lstFDefectPhotos.files)
                                                {
                                                    newFabricDefectPhotosRow = tblFabricDefectPhotos.NewRow();
                                                    newFabricDefectPhotosRow["report_product_id"] = lstFDefectPhotos.report_product_id;
                                                    newFabricDefectPhotosRow["url"] = lstFDefPhotos.url;
                                                    newFabricDefectPhotosRow["mediaType"] = lstFDefPhotos.mediaType;
                                                    newFabricDefectPhotosRow["ordering"] = lstFDefPhotos.ordering;
                                                    newFabricDefectPhotosRow["caption"] = lstFDefPhotos.caption;
                                                    tblFabricDefectPhotos.Rows.Add(newFabricDefectPhotosRow);
                                                }
                                            }
                                        }
                                    }
                                }
                                #endregion

                                #region  get FabricInspectionDetail
                                if (defclassfields.fabric_defects_classification_detail_fabric_inspection != null && defclassfields.fabric_defects_classification_detail_fabric_inspection.fabric_defects_classification_detail_fabric_inspection_lines_split_product != null && defclassfields.fabric_defects_classification_detail_fabric_inspection.fabric_defects_classification_detail_fabric_inspection_lines_split_product.Count > 0)
                                {
                                    var FabInspDetail = defclassfields.fabric_defects_classification_detail_fabric_inspection.fabric_defects_classification_detail_fabric_inspection_lines_split_product;
                                    foreach (var lstFabInspDetail in FabInspDetail)
                                    {
                                        if (productslist.report_product_id == lstFabInspDetail.report_product_id)
                                        {
                                            if (lstFabInspDetail.fabric_defects_classification_detail_fabric_inspection_lines_product != null && lstFabInspDetail.fabric_defects_classification_detail_fabric_inspection_lines_product.Count > 0)
                                            {
                                                #region  get FabricInspectionDetail
                                                foreach (var lstDetail in lstFabInspDetail.fabric_defects_classification_detail_fabric_inspection_lines_product)
                                                {
                                                    orderingFabricInspectionDetail = orderingFabricInspectionDetail + 1;
                                                    newFabInspDetaillRow = tblFabricInspectionDetail.NewRow();
                                                    newFabInspDetaillRow["report_product_id"] = lstFabInspDetail.report_product_id;
                                                    newFabInspDetaillRow["color"] = productslist.productReference + "-" + productslist.color;
                                                    newFabInspDetaillRow["acceptance_criteria"] = lstDetail.fabric_defects_classification_detail_fabric_inspection_acceptance_criteria!=null? lstDetail.fabric_defects_classification_detail_fabric_inspection_acceptance_criteria.value:"";
                                                    newFabInspDetaillRow["length_original"] = lstDetail.fabric_defects_classification_detail_fabric_inspection_length_original!=null? lstDetail.fabric_defects_classification_detail_fabric_inspection_length_original.value:"";
                                                    newFabInspDetaillRow["no_of_defect"] = lstDetail.fabric_defects_classification_detail_fabric_inspection_no_of_defect!=null? lstDetail.fabric_defects_classification_detail_fabric_inspection_no_of_defect.value:"";
                                                    newFabInspDetaillRow["no_of_points"] = lstDetail.fabric_defects_classification_detail_fabric_inspection_no_of_points!=null? lstDetail.fabric_defects_classification_detail_fabric_inspection_no_of_points.value:"";
                                                    newFabInspDetaillRow["roll_number"] = lstDetail.fabric_defects_classification_detail_fabric_inspection_roll_number!=null? lstDetail.fabric_defects_classification_detail_fabric_inspection_roll_number.value:"";
                                                    newFabInspDetaillRow["weight_actual"] = lstDetail.fabric_defects_classification_detail_fabric_inspection_weight_actual!=null? lstDetail.fabric_defects_classification_detail_fabric_inspection_weight_actual.value:"";
                                                    newFabInspDetaillRow["weight_original"] = lstDetail.fabric_defects_classification_detail_fabric_inspection_weight_original!=null? lstDetail.fabric_defects_classification_detail_fabric_inspection_weight_original.value:"";
                                                    newFabInspDetaillRow["width_original"] = lstDetail.fabric_defects_classification_detail_fabric_inspection_width_original!=null? lstDetail.fabric_defects_classification_detail_fabric_inspection_width_original.value:"";
                                                    newFabInspDetaillRow["comment"] = lstDetail.fabric_defects_classification_detail_fabric_inspection_comment != null ? lstDetail.fabric_defects_classification_detail_fabric_inspection_comment.value : "";
                                                    newFabInspDetaillRow["dye_lot"] = lstDetail.fabric_defects_classification_detail_fabric_inspection_dye_lot!=null? lstDetail.fabric_defects_classification_detail_fabric_inspection_dye_lot.value:"";
                                                    newFabInspDetaillRow["dye_lot_default"] = lstDetail.fabric_defects_classification_detail_fabric_inspection_dye_lot_default!=null? lstDetail.fabric_defects_classification_detail_fabric_inspection_dye_lot_default.value:"";
                                                    newFabInspDetaillRow["length_actual"] = lstDetail.fabric_defects_classification_detail_fabric_inspection_length_actual!=null? lstDetail.fabric_defects_classification_detail_fabric_inspection_length_actual.value:"";
                                                    newFabInspDetaillRow["no_of_points_100_sqy"] = lstDetail.fabric_defects_classification_detail_fabric_inspection_no_of_points_100_sqy!=null? lstDetail.fabric_defects_classification_detail_fabric_inspection_no_of_points_100_sqy.value:"";
                                                    newFabInspDetaillRow["result"] = lstDetail.fabric_defects_classification_detail_fabric_inspection_result!=null? lstDetail.fabric_defects_classification_detail_fabric_inspection_result.value:"";
                                                    newFabInspDetaillRow["production_sample_cutted"] = lstDetail.fabric_defects_classification_detail_fabric_inspection_production_sample_cutted != null ? lstDetail.fabric_defects_classification_detail_fabric_inspection_production_sample_cutted.value : "";
                                                    newFabInspDetaillRow["production_sample_cutted_Image"] = lstDetail.fabric_defects_classification_detail_fabric_inspection_production_sample_cutted != null && lstDetail.fabric_defects_classification_detail_fabric_inspection_production_sample_cutted.value == "1" ? "https://www.linkqms.io/Documents/InspReportImg/fabric-sample-cutted.jpg" : "";
                                                    newFabInspDetaillRow["width_actual"] = lstDetail.fabric_defects_classification_detail_fabric_inspection_width_actual!=null? lstDetail.fabric_defects_classification_detail_fabric_inspection_width_actual.value:"";
                                                    tblFabricInspectionDetail.Rows.Add(newFabInspDetaillRow);

                                                    #region FabricInspectionPhotos
                                                    if (lstDetail.fabric_defects_classification_detail_fabric_inspection_photos != null && lstDetail.fabric_defects_classification_detail_fabric_inspection_photos.files != null && lstDetail.fabric_defects_classification_detail_fabric_inspection_photos.files.Count > 0)
                                                    {
                                                        int marknumber = 0;
                                                        foreach (var lstInpPic in lstDetail.fabric_defects_classification_detail_fabric_inspection_photos.files)
                                                        {
                                                            marknumber = marknumber + 1;
                                                            newFInspPhotosRow = tblFabricInspectionPhotos.NewRow();
                                                            newFInspPhotosRow["report_product_id"] = lstDetail.fabric_defects_classification_detail_fabric_inspection_photos.report_product_id;
                                                            newFInspPhotosRow["TestID"] = orderingFabricInspectionDetail;
                                                            newFInspPhotosRow["url"] = lstInpPic.url;
                                                            newFInspPhotosRow["mediaType"] = lstInpPic.mediaType;
                                                            newFInspPhotosRow["ordering"] = lstInpPic.ordering;
                                                            newFInspPhotosRow["caption"] = lstInpPic.caption;
                                                            newFInspPhotosRow["marknumber"] = marknumber;
                                                            tblFabricInspectionPhotos.Rows.Add(newFInspPhotosRow);
                                                        }

                                                    }

                                                    #endregion

                                                    #region FabricDefects
                                                    if (lstDetail.fabric_defects_classification_detail_fabric_inspection_nested_sublines != null && lstDetail.fabric_defects_classification_detail_fabric_inspection_nested_sublines.fabric_defects_classification_detail_fabric_inspection_nested_sublines_sublines != null && lstDetail.fabric_defects_classification_detail_fabric_inspection_nested_sublines.fabric_defects_classification_detail_fabric_inspection_nested_sublines_sublines.Count > 0)
                                                    {
                                                        foreach (var lstFDef in lstDetail.fabric_defects_classification_detail_fabric_inspection_nested_sublines.fabric_defects_classification_detail_fabric_inspection_nested_sublines_sublines)
                                                        {
                                                            newFDefRow = tblFabricDefects.NewRow();
                                                            newFDefRow["report_product_id"] = lstFDef.fabric_defects_classification_detail_fabric_inspection_nested_sublines_defect.report_product_id;
                                                            newFDefRow["TestID"] = orderingFabricInspectionDetail;
                                                            newFDefRow["location"] = lstFDef.fabric_defects_classification_detail_fabric_inspection_nested_sublines_location.value;
                                                            newFDefRow["point"] = lstFDef.fabric_defects_classification_detail_fabric_inspection_nested_sublines_point.value;
                                                            newFDefRow["defect"] = lstFDef.fabric_defects_classification_detail_fabric_inspection_nested_sublines_defect!=null? lstFDef.fabric_defects_classification_detail_fabric_inspection_nested_sublines_defect.value:"";
                                                            newFDefRow["defectDescription"] = lstFDef.fabric_defects_classification_detail_fabric_inspection_nested_sublines_defect_description!=null? lstFDef.fabric_defects_classification_detail_fabric_inspection_nested_sublines_defect_description.value:"";
                                                            tblFabricDefects.Rows.Add(newFDefRow);
                                                        }

                                                    }

                                                    #endregion
                                                }
                                                #endregion
                                            }
                                        }
                                    }
                                }
                                #endregion


                            }

                            #region add data to tblFInspPhotos
                            if (tblFabricInspectionDetail.Rows.Count > 0)
                            {
                                for (int i = 0; i < tblFabricInspectionDetail.Rows.Count; i++)
                                {
                                    int TestID = int.Parse(tblFabricInspectionDetail.Rows[i]["TestID"].ToString());
                                    int report_product_id = int.Parse(tblFabricInspectionDetail.Rows[i]["report_product_id"].ToString());
                                    if (tblFabricInspectionPhotos.Rows.Count > 0)
                                    {
                                        DataRow[] drs = null, drs2 = null; ;
                                        drs = tblFabricInspectionPhotos.Select(string.Format(" report_product_id={0} and TestID={1}", report_product_id, TestID));
                                        if (drs.Length > 0)
                                        {
                                            for (int h = 0; h <= drs.Length - 1; h++)
                                            {
                                                if (h % 2 == 0)
                                                {
                                                    newFIPhotosRow = tblFInspPhotos.NewRow();
                                                    newFIPhotosRow["report_product_id"] = report_product_id;
                                                    newFIPhotosRow["TestID"] = TestID;

                                                    newFIPhotosRow["url"] = drs[h]["url"].ToString();
                                                    newFIPhotosRow["mediaType"] = drs[h]["mediaType"].ToString();
                                                    newFIPhotosRow["ordering"] = drs[h]["ordering"].ToString();
                                                    newFIPhotosRow["caption"] = drs[h]["caption"].ToString();
                                                    int marknumber1 = int.Parse(drs[h]["marknumber"].ToString());
                                                    marknumber1 = marknumber1 + 1;//get the next row  marknumber  
                                                    drs2 = tblFabricInspectionPhotos.Select(string.Format(" report_product_id={0} and TestID={1} and  marknumber={2}", report_product_id, TestID, marknumber1));
                                                    if (drs2.Length > 0)
                                                    {
                                                        for (int k2 = 0; k2 < drs2.Length; k2++)
                                                        {
                                                            newFIPhotosRow["url2"] = drs2[k2]["url"].ToString();
                                                            newFIPhotosRow["mediaType2"] = drs2[k2]["mediaType"].ToString();
                                                            newFIPhotosRow["ordering2"] = drs2[k2]["ordering"].ToString();
                                                            newFIPhotosRow["caption2"] = drs2[k2]["caption"].ToString();
                                                        }
                                                    }
                                                    tblFInspPhotos.Rows.Add(newFIPhotosRow);
                                                }

                                            }
                                        }

                                    }
                                }
                            }
                            #endregion
                        }

                    }
                    #endregion

                    #region add table to dataset
                    DataSet dsFabricInspectionDetail = new DataSet();
                    dsFabricInspectionDetail.Tables.Add(tblFabricDefectColor);
                    dsFabricInspectionDetail.Tables.Add(tblFabricDefectPhotos);
                    dsFabricInspectionDetail.Tables.Add(tblFabricInspectionDetail);
                    dsFabricInspectionDetail.Tables.Add(tblFabricInspectionPhotos);
                    dsFabricInspectionDetail.Tables.Add(tblFabricDefects);
                    dsFabricInspectionDetail.Tables.Add(tblFInspPhotos);
                    dsFabricInspectionDetail.Relations.Add("FDPicLink", dsFabricInspectionDetail.Tables[0].Columns["report_product_id"], dsFabricInspectionDetail.Tables[1].Columns["report_product_id"]);
                    dsFabricInspectionDetail.Relations.Add("FIDLink", dsFabricInspectionDetail.Tables[0].Columns["report_product_id"], dsFabricInspectionDetail.Tables[2].Columns["report_product_id"]);
                    dsFabricInspectionDetail.Relations.Add("FIDPicLink", dsFabricInspectionDetail.Tables[2].Columns["TestID"], dsFabricInspectionDetail.Tables[3].Columns["TestID"]);
                    dsFabricInspectionDetail.Relations.Add("FIDDefLink", dsFabricInspectionDetail.Tables[2].Columns["TestID"], dsFabricInspectionDetail.Tables[4].Columns["TestID"]);

                    dsFabricInspectionDetail.Relations.Add("FIDPictureLink", dsFabricInspectionDetail.Tables[2].Columns["TestID"], dsFabricInspectionDetail.Tables[5].Columns["TestID"]);

                    response.dsFabricInspectionDetail = dsFabricInspectionDetail;
                    #endregion
                    #endregion

                    #region fabric rubbing test
                    #region  create table
                    DataTable tblRubbingTestColor = new DataTable("RubbingTestColor");
                    DataColumn dcRubbingTestColor = null;
                    dcRubbingTestColor = tblRubbingTestColor.Columns.Add("ID", Type.GetType("System.Int32"));
                    dcRubbingTestColor.AutoIncrement = true;
                    dcRubbingTestColor.AutoIncrementSeed = 1;
                    dcRubbingTestColor.AutoIncrementStep = 1;
                    dcRubbingTestColor.AllowDBNull = false;
                    dcRubbingTestColor = tblRubbingTestColor.Columns.Add("report_product_id", Type.GetType("System.Int32"));
                    dcRubbingTestColor = tblRubbingTestColor.Columns.Add("color", Type.GetType("System.String"));
                    dcRubbingTestColor = tblRubbingTestColor.Columns.Add("wet_rubbing_test_customer_requirement", Type.GetType("System.String"));
                    dcRubbingTestColor = tblRubbingTestColor.Columns.Add("dry_rubbing_test_customer_requirement", Type.GetType("System.String"));
                    dcRubbingTestColor = tblRubbingTestColor.Columns.Add("fabric_rubbing_test_comments", Type.GetType("System.String"));

                    DataTable tblFabricRubbingTest = new DataTable("FabricRubbingTest");
                    DataColumn dcFabricRubbingTest = null;
                    dcFabricRubbingTest = tblFabricRubbingTest.Columns.Add("RubbingTestID", Type.GetType("System.Int32"));
                    dcFabricRubbingTest.AutoIncrement = true;
                    dcFabricRubbingTest.AutoIncrementSeed = 1;
                    dcFabricRubbingTest.AutoIncrementStep = 1;
                    dcFabricRubbingTest.AllowDBNull = false;

                    dcFabricRubbingTest = tblFabricRubbingTest.Columns.Add("report_product_id", Type.GetType("System.Int32"));
                    dcFabricRubbingTest = tblFabricRubbingTest.Columns.Add("color", Type.GetType("System.String"));
                    dcFabricRubbingTest = tblFabricRubbingTest.Columns.Add("dye_lot", Type.GetType("System.String"));
                    dcFabricRubbingTest = tblFabricRubbingTest.Columns.Add("roll", Type.GetType("System.String"));
                    dcFabricRubbingTest = tblFabricRubbingTest.Columns.Add("dry_rub", Type.GetType("System.String"));
                    dcFabricRubbingTest = tblFabricRubbingTest.Columns.Add("wet_rub", Type.GetType("System.String"));
                    dcFabricRubbingTest = tblFabricRubbingTest.Columns.Add("rub_test_result", Type.GetType("System.String"));
                    dcFabricRubbingTest = tblFabricRubbingTest.Columns.Add("photos_wet_rub", Type.GetType("System.String"));
                    dcFabricRubbingTest = tblFabricRubbingTest.Columns.Add("photos_dry_rub", Type.GetType("System.String"));
                    dcFabricRubbingTest = tblFabricRubbingTest.Columns.Add("dry_rub_over", Type.GetType("System.String"));
                    dcFabricRubbingTest = tblFabricRubbingTest.Columns.Add("wet_rub_over", Type.GetType("System.String"));

                    #endregion

                    #region add rows to tables fabric rubbing test
                    DataRow newRubbingTestColorRow;
                    DataRow newFabricRubbingTestRow;
                    int orderingRubbingTestColor = 1;
                    if (fbOrderDetail.sections.fabric_rubbing_test != null && fbOrderDetail.sections.fabric_rubbing_test.fabric_rubbing_test_fields != null && fbOrderDetail.sections.fabric_rubbing_test.fabric_rubbing_test_fields.fabric_rubbing_test_wet_rubbing_test_customer_requirement != null && fbOrderDetail.sections.fabric_rubbing_test.fabric_rubbing_test_fields.fabric_rubbing_test_wet_rubbing_test_customer_requirement.Count > 0)
                    {
                        if (fbOrderDetail.products != null && fbOrderDetail.products.Count > 0)
                        {
                            foreach (var productslist in fbOrderDetail.products)
                            {
                                orderingRubbingTestColor = orderingRubbingTestColor + 1;
                                newRubbingTestColorRow = tblRubbingTestColor.NewRow();
                                newRubbingTestColorRow["report_product_id"] = productslist.report_product_id;
                                newRubbingTestColorRow["color"] = productslist.color;
                                #region  get wet_rubbing_test_customer_requirement and dry_rubbing_test_customer_requirement

                                var wet_rubbing = (fbOrderDetail.sections.fabric_rubbing_test.fabric_rubbing_test_fields.fabric_rubbing_test_wet_rubbing_test_customer_requirement).Find(t => t.report_product_id == productslist.report_product_id);
                                newRubbingTestColorRow["wet_rubbing_test_customer_requirement"] = wet_rubbing != null ? wet_rubbing.value : "";

                                if (fbOrderDetail.sections.fabric_rubbing_test.fabric_rubbing_test_fields.fabric_rubbing_test_dry_rubbing_test_customer_requirement != null && fbOrderDetail.sections.fabric_rubbing_test.fabric_rubbing_test_fields.fabric_rubbing_test_dry_rubbing_test_customer_requirement.Count > 0)
                                {
                                    var dry_rubbing = (fbOrderDetail.sections.fabric_rubbing_test.fabric_rubbing_test_fields.fabric_rubbing_test_dry_rubbing_test_customer_requirement).Find(t => t.report_product_id == productslist.report_product_id);
                                    newRubbingTestColorRow["dry_rubbing_test_customer_requirement"] = dry_rubbing != null ? dry_rubbing.value : "";
                                }

                                if (fbOrderDetail.sections.fabric_rubbing_test.fabric_rubbing_test_fields.fabric_rubbing_test_comments != null && fbOrderDetail.sections.fabric_rubbing_test.fabric_rubbing_test_fields.fabric_rubbing_test_comments.Count > 0)
                                {
                                    var test_comments = (fbOrderDetail.sections.fabric_rubbing_test.fabric_rubbing_test_fields.fabric_rubbing_test_comments).Find(t => t.report_product_id == productslist.report_product_id);
                                    newRubbingTestColorRow["fabric_rubbing_test_comments"] = test_comments != null ? test_comments.value : "";
                                }
                                #endregion
                                tblRubbingTestColor.Rows.Add(newRubbingTestColorRow);


                                #region  get RubbingTest data
                                if (fbOrderDetail.sections.fabric_rubbing_test != null && fbOrderDetail.sections.fabric_rubbing_test.fabric_rubbing_test_fields != null && fbOrderDetail.sections.fabric_rubbing_test.fabric_rubbing_test_fields.fabric_rubbing_test_rubbing_test_lines != null && fbOrderDetail.sections.fabric_rubbing_test.fabric_rubbing_test_fields.fabric_rubbing_test_rubbing_test_lines.fabric_rubbing_test_rubbing_test_lines_lines_split_product != null && fbOrderDetail.sections.fabric_rubbing_test.fabric_rubbing_test_fields.fabric_rubbing_test_rubbing_test_lines.fabric_rubbing_test_rubbing_test_lines_lines_split_product.Count > 0)
                                {
                                    foreach (var RubbingTestlist1 in fbOrderDetail.sections.fabric_rubbing_test.fabric_rubbing_test_fields.fabric_rubbing_test_rubbing_test_lines.fabric_rubbing_test_rubbing_test_lines_lines_split_product)
                                    {
                                        if (RubbingTestlist1.fabric_rubbing_test_rubbing_test_lines_lines_product != null && RubbingTestlist1.fabric_rubbing_test_rubbing_test_lines_lines_product.Count > 0)
                                        {
                                            foreach (var RubbingTestlist2 in RubbingTestlist1.fabric_rubbing_test_rubbing_test_lines_lines_product)
                                            {
                                                if (productslist.report_product_id == RubbingTestlist1.report_product_id)
                                                {
                                                    newFabricRubbingTestRow = tblFabricRubbingTest.NewRow();
                                                    newFabricRubbingTestRow["report_product_id"] = productslist.report_product_id;
                                                    newFabricRubbingTestRow["color"] = productslist.color;
                                                    if (RubbingTestlist2.fabric_rubbing_test_rubbing_test_lines_dye_lot != null)
                                                    {
                                                        newFabricRubbingTestRow["dye_lot"] = RubbingTestlist2.fabric_rubbing_test_rubbing_test_lines_dye_lot.value;
                                                    }
                                                    if (RubbingTestlist2.fabric_rubbing_test_rubbing_test_lines_roll != null)
                                                    {
                                                        newFabricRubbingTestRow["roll"] = RubbingTestlist2.fabric_rubbing_test_rubbing_test_lines_roll.value;
                                                    }
                                                    if (RubbingTestlist2.fabric_rubbing_test_rubbing_test_lines_dry_rub != null)
                                                    {
                                                        newFabricRubbingTestRow["dry_rub"] = RubbingTestlist2.fabric_rubbing_test_rubbing_test_lines_dry_rub.value;
                                                    }
                                                    if (RubbingTestlist2.fabric_rubbing_test_rubbing_test_lines_wet_rub != null)
                                                    {
                                                        newFabricRubbingTestRow["wet_rub"] = RubbingTestlist2.fabric_rubbing_test_rubbing_test_lines_wet_rub.value;
                                                    }
                                                    if (RubbingTestlist2.fabric_rubbing_test_rubbing_test_lines_rub_test_result != null)
                                                    {
                                                        newFabricRubbingTestRow["rub_test_result"] = RubbingTestlist2.fabric_rubbing_test_rubbing_test_lines_rub_test_result.value;
                                                    }
                                                    if (RubbingTestlist2.fabric_rubbing_test_rubbing_test_lines_photos_wet_rub != null && RubbingTestlist2.fabric_rubbing_test_rubbing_test_lines_photos_wet_rub.files != null && RubbingTestlist2.fabric_rubbing_test_rubbing_test_lines_photos_wet_rub.files.Count > 0)
                                                    {
                                                        newFabricRubbingTestRow["photos_wet_rub"] = RubbingTestlist2.fabric_rubbing_test_rubbing_test_lines_photos_wet_rub.files[0].url;
                                                    }
                                                    if (RubbingTestlist2.fabric_rubbing_test_rubbing_test_lines_photos_dry_rub != null && RubbingTestlist2.fabric_rubbing_test_rubbing_test_lines_photos_dry_rub.files != null && RubbingTestlist2.fabric_rubbing_test_rubbing_test_lines_photos_dry_rub.files.Count > 0)
                                                    {
                                                        newFabricRubbingTestRow["photos_dry_rub"] = RubbingTestlist2.fabric_rubbing_test_rubbing_test_lines_photos_dry_rub.files[0].url;
                                                    }

                                                    if (RubbingTestlist2.fabric_rubbing_test_rubbing_test_lines_dry_rub_over != null)
                                                    {
                                                        newFabricRubbingTestRow["dry_rub_over"] = RubbingTestlist2.fabric_rubbing_test_rubbing_test_lines_dry_rub_over.value;
                                                    }

                                                    if (RubbingTestlist2.fabric_rubbing_test_rubbing_test_lines_wet_rub_over != null)
                                                    {
                                                        newFabricRubbingTestRow["wet_rub_over"] = RubbingTestlist2.fabric_rubbing_test_rubbing_test_lines_wet_rub_over.value;
                                                    }

                                                    tblFabricRubbingTest.Rows.Add(newFabricRubbingTestRow);
                                                }
                                            }
                                        }
                                    }

                                }

                                #endregion

                            }
                        }

                    }
                    #endregion

                    #region add table to dataset
                    DataSet dsFabricRubbingTest = new DataSet();
                    dsFabricRubbingTest.Tables.Add(tblRubbingTestColor);
                    dsFabricRubbingTest.Tables.Add(tblFabricRubbingTest);
                    dsFabricRubbingTest.Relations.Add("FRTLink", dsFabricRubbingTest.Tables[0].Columns["report_product_id"], dsFabricRubbingTest.Tables[1].Columns["report_product_id"]);
                    response.dsFabricRubbingTest = dsFabricRubbingTest;
                    #endregion

                    #endregion



                    #endregion

                }
                #endregion
            }
            return response;

        }

        #region GetTemplateConfig
        private List<RepFastTemplateConfig> GetTemplateConfig(List<RepFastTemplateConfig> lstTemplateConfig, InspectionCustomReportSummaryResponse MainOrderInfo)
        {
            List<RepFastTemplateConfig> lstRepFastTemplateConfig = new List<RepFastTemplateConfig>();
            var ServiceTypeId = MainOrderInfo.Data.ServiceTypeId;
            var ProductCategoryId = MainOrderInfo.Data.ProductCategoryId;
            var brandid = MainOrderInfo.Data.BrandId == null ? 0 : MainOrderInfo.Data.BrandId;
            var departid = MainOrderInfo.Data.DepartId == null ? 0 : MainOrderInfo.Data.DepartId;
            if (lstTemplateConfig != null && lstTemplateConfig.Count >= 1)
            {
                #region 
                lstRepFastTemplateConfig = lstTemplateConfig.Where(x => x.ServiceTypeId == ServiceTypeId && x.ProductCategoryId == ProductCategoryId && x.BrandId == brandid && x.DepartId == departid).ToList();//1 departid +brandid
                if (lstRepFastTemplateConfig == null || lstRepFastTemplateConfig.Count == 0)
                {
                    lstRepFastTemplateConfig = lstTemplateConfig.Where(x => x.ServiceTypeId == ServiceTypeId && x.ProductCategoryId == ProductCategoryId && x.DepartId == departid).ToList();//2 departid
                }
                if (lstRepFastTemplateConfig == null || lstRepFastTemplateConfig.Count == 0)
                {
                    lstRepFastTemplateConfig = lstTemplateConfig.Where(x => x.ProductCategoryId == ProductCategoryId && x.DepartId == departid).ToList();//3 departid
                }
                if (lstRepFastTemplateConfig == null || lstRepFastTemplateConfig.Count == 0)
                {
                    lstRepFastTemplateConfig = lstTemplateConfig.Where(x => x.ServiceTypeId == ServiceTypeId && x.DepartId == departid).ToList();//4 departid
                }
                if (lstRepFastTemplateConfig == null || lstRepFastTemplateConfig.Count == 0)
                {
                    lstRepFastTemplateConfig = lstTemplateConfig.Where(x => x.ServiceTypeId == ServiceTypeId && x.ProductCategoryId == ProductCategoryId && x.BrandId == brandid).ToList();//5 brandid
                }

                if (lstRepFastTemplateConfig == null || lstRepFastTemplateConfig.Count == 0)
                {
                    lstRepFastTemplateConfig = lstTemplateConfig.Where(x => x.ProductCategoryId == ProductCategoryId && x.BrandId == brandid).ToList();//6 brandid
                }
                if (lstRepFastTemplateConfig == null || lstRepFastTemplateConfig.Count == 0)
                {
                    lstRepFastTemplateConfig = lstTemplateConfig.Where(x => x.ServiceTypeId == ServiceTypeId && x.BrandId == brandid).ToList();//7 brandid
                }

                if (lstRepFastTemplateConfig == null || lstRepFastTemplateConfig.Count == 0)
                {
                    lstRepFastTemplateConfig = lstTemplateConfig.Where(x => x.ServiceTypeId == ServiceTypeId && x.ProductCategoryId == ProductCategoryId).ToList();//8
                }

                if (lstRepFastTemplateConfig == null || lstRepFastTemplateConfig.Count == 0)
                {
                    lstRepFastTemplateConfig = lstTemplateConfig.Where(x => x.ProductCategoryId == ProductCategoryId).ToList();//9
                }

                if (lstRepFastTemplateConfig == null || lstRepFastTemplateConfig.Count == 0)
                {
                    lstRepFastTemplateConfig = lstTemplateConfig.Where(x => x.ServiceTypeId == ServiceTypeId).ToList();//10
                }
                #endregion
            }

            return lstRepFastTemplateConfig;

        }
        #endregion

        /// <summary>
        /// get Report Template
        /// </summary>
        /// <param name="fbReportMapId"></param>
        /// <returns></returns>
        public string GetReportTemplate(int fbReportId, InspectionCustomReportItem OrderMainDetail, string ReportTemplateName)
        {
            try
            {
                string ReportTemplatePath = string.Empty;
                if (string.IsNullOrEmpty(ReportTemplateName))
                {
                    ReportTemplateName = "StandardReport_Final.frx";
                    if (OrderMainDetail != null)
                    {
                        if (OrderMainDetail.ServiceType != null)
                        {
                            if (OrderMainDetail.ProductCategory.ToLower() != "fabric")
                            {
                                if (OrderMainDetail.ServiceType.Contains("Inline"))
                                {
                                    ReportTemplateName = "StandardReport_Inline.frx";
                                }
                                else if (OrderMainDetail.ServiceType.Contains("100"))
                                {
                                    ReportTemplateName = "StandardReport_100.frx";
                                }
                            }
                            else
                            {
                                ReportTemplateName = "StandardReport_Fabric.frx";
                            }

                        }
                    }
                }
                ReportTemplatePath = Path.Combine("Views/Report_Templates/SGT", ReportTemplateName);
                return ReportTemplatePath;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        /// <summary>
        /// upload file to cloud
        /// </summary>
        /// <param name="reportmemory"></param>
        /// /// <param name="fileName"></param>
        /// <returns></returns>
        public (string filePath, string uniqueId) FetchCloudReportUrl(MemoryStream reportmemory, string fileName, string fileExtension, FileContainerList container)
        {
            try
            {
                string ReportPath = string.Empty;
                string uniqueId = string.Empty;
                string fileMimeType = _fileManager.GetMimeType(fileExtension);
                using (var memory = reportmemory)
                {
                    FileResponse document = new FileResponse()
                    {
                        Content = memory.ToArray(),
                        MimeType = fileMimeType, //"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",//"application/pdf"
                        Result = DTO.File.FileResult.Success,
                        Name = fileName
                    };

                    // upload file to cloud
                    var multipartContent = new MultipartFormDataContent();
                    var byteContent = new ByteArrayContent(document.Content);
                    byteContent.Headers.ContentType = MediaTypeHeaderValue.Parse(fileMimeType); //MediaTypeHeaderValue.Parse("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"); 
                    multipartContent.Add(byteContent, "files", Guid.NewGuid().ToString());
                    using (var httpClient = new HttpClient())
                    {
                        httpClient.Timeout = TimeSpan.FromMinutes(4);
                        int ContainerId = (int)FileContainerList.DevContainer;
                        if (!Convert.ToBoolean(_configuration["IsDevelopment_Enviornment"]))
                        {
                            ContainerId = (int)container;
                        }

                        int EntityId = _filterService.GetCompanyId();
                        var cloudFileUrl = _configuration["FileServer"] + "savefile/" + ContainerId.ToString() + "/" + EntityId.ToString();

                        HttpResponseMessage dataResponse = httpClient.PostAsync(cloudFileUrl, multipartContent).Result;
                        var StatusCode = dataResponse.StatusCode;
                        if (StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            string result = dataResponse.Content.ReadAsStringAsync().Result;

                            if (!string.IsNullOrEmpty(result))
                            {
                                var fileResultData = JsonConvert.DeserializeObject<FileUploadResponse>(result);
                                var fileUploadData = fileResultData?.FileUploadDataList?.FirstOrDefault();
                                if (fileResultData != null && fileResultData.FileUploadDataList != null
                                    && fileUploadData != null
                                    && fileUploadData.Result == FileUploadResponseResult.Sucess)
                                {
                                    ReportPath = fileUploadData.FileCloudUri;
                                    uniqueId = fileUploadData.FileName;
                                }
                            }
                        }
                    }
                }
                return (filePath: ReportPath, uniqueId: uniqueId);
            }
            catch (Exception ex)
            {
                return (null, null);
            }
        }

        /// <summary>
        /// get Template Config Info
        /// </summary>
        /// <param name="fbReportId"></param>
        /// <returns></returns>
        public async Task<TemplateConfigResponse> GetTemplateConfigInfo(int fbReportId)
        {
            var response = new TemplateConfigResponse();




            return response;
        }

        /// <summary>
        /// get fb report staff list
        /// </summary>
        /// <param name="fbFbUserIds"></param>
        /// <returns></returns>
        public async Task<List<InspectionCustomReportStaff>> GetFBReportStaffList(List<int> fbUserIds)
        {
            return await _inspeCusReport.GetFBReportStaffList(fbUserIds);
        }

    }
}
