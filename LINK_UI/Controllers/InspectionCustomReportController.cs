//using BI.Utilities;
//using Contracts.Managers;
//using DTO.CustomReport;
//using DTO.FullBridge;
//using DTO.InspectionCustomReport;
//using DTO.Report;
//using LINK_UI.App_start;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Configuration;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Claims;
//using System.Threading.Tasks;

//using System.IO;
//using FastReport.Web;
//using FastReport.Export.Pdf;
//using FastReport.Export.OoXML;

//using Entities.Enums;
//namespace LINK_UI.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    [Authorize(Policy = "ApiUserPolicy")]
//    public class InspectionCustomReportController : ControllerBase
//    {
//        private readonly IInspectionCustomReportManager _manager = null;
//        private readonly IConfiguration _configuration = null;
//        private readonly IHumanResourceManager _hrManager = null;

//        public InspectionCustomReportController(IInspectionCustomReportManager manager, IConfiguration configuration, IHumanResourceManager hrManager)
//        {
//            _manager = manager;
//            _configuration = configuration;
//            _hrManager = hrManager;

//        }

//        [HttpGet("getinspectionreport/{fbReportId}")]
//        public async Task<FBReportResponse> GetInspectionReport(int fbReportId)
//        {
//            FBReportResponse FBRetrun = new FBReportResponse();
//            WebReport InspReport = new WebReport();
//            try
//            {
//                string strFbToken = getFbToken();
//                if (fbReportId > 0)
//                {
//                    FBReportInfoResponse ReportInfo = await _manager.FetchFBReportInfo(fbReportId, strFbToken);//get report info

//                    if (ReportInfo.TemplateConfig != null)
//                    {
//                        int? FileExtensionID = ReportInfo.TemplateConfig.FileExtensionID;
//                        int? ReportToolTypeID = ReportInfo.TemplateConfig.ReportToolTypeID;

//                        if (ReportToolTypeID == (int)ReportToolType.Aspose)
//                        {

//                        }
//                        else
//                        {
//                            //loading report template
//                            InspReport.Width = "1000";
//                            InspReport.Height = "1000";
//                            string pathReportTemplate = _manager.GetReportTemplate(fbReportId, ReportInfo.lstOrderMainDetail[0], ReportInfo.TemplateConfig.TemplateName);//get report template

//                            InspReport.Report.Load(pathReportTemplate);

//                            #region RegisterData
//                            //Order Main Detail
//                            if (ReportInfo.lstOrderMainDetail != null && ReportInfo.lstOrderMainDetail.Count > 0)
//                            {
//                                InspReport.Report.RegisterData(ReportInfo.lstOrderMainDetail, "MainDetail");
//                            }
//                            //Template detail
//                            if (ReportInfo.lstTemplateDetail != null && ReportInfo.lstTemplateDetail.Count > 0)
//                            {
//                                InspReport.Report.RegisterData(ReportInfo.lstTemplateDetail, "TemplateDetail");
//                            }
//                            //Factory Analysis Chart
//                            if (ReportInfo.ChartFacAnalysisDefects != null)
//                            {
//                                InspReport.Report.RegisterData(ReportInfo.ChartFacAnalysisDefects, "ChartFacAnalysisDefects");
//                            }
//                            //Factory Analysis Chart
//                            if (ReportInfo.ChartMSOutofTolerance != null)
//                            {
//                                InspReport.Report.RegisterData(ReportInfo.ChartMSOutofTolerance, "ChartMSOutofTolerance");
//                            }

//                            //100 Charts Garment Grade
//                            if (ReportInfo.ChartsGarmentGrade != null)
//                            {
//                                InspReport.Report.RegisterData(ReportInfo.ChartsGarmentGrade, "ChartsGarmentGrade");
//                            }

//                            //Quantity Details
//                            if (ReportInfo.lstProducts != null && ReportInfo.lstProducts.Count > 0)
//                            {
//                                InspReport.Report.RegisterData(ReportInfo.lstProducts, "QuantityDetails");
//                            }
//                            //amcharts_defects_by_category
//                            if (ReportInfo.ChartDefectsCategory != null)
//                            {
//                                InspReport.Report.RegisterData(ReportInfo.ChartDefectsCategory, "ChartDefectsCategory");
//                            }

//                            //amcharts_defects_by_category2 -JAPAN ORDERS/APA  ORDERS
//                            if (ReportInfo.ChartDefectsCategory2 != null)
//                            {
//                                InspReport.Report.RegisterData(ReportInfo.ChartDefectsCategory2, "ChartDefectsCategory2");
//                            }


//                            //amcharts_defects_by_reparability
//                            if (ReportInfo.ChartDefectsReparability != null)
//                            {
//                                InspReport.Report.RegisterData(ReportInfo.ChartDefectsReparability, "ChartDefectsReparability");
//                            }
//                            //amcharts_defects_with_aql
//                            if (ReportInfo.ChartDefectsWithAql != null)
//                            {
//                                InspReport.Report.RegisterData(ReportInfo.ChartDefectsWithAql, "ChartDefectsWithAql");
//                            }
//                            //amcharts_defects_with_aql2 -JAPAN ORDERS/APA  ORDERS
//                            if (ReportInfo.ChartDefectsWithAql2 != null)
//                            {
//                                InspReport.Report.RegisterData(ReportInfo.ChartDefectsWithAql2, "ChartDefectsWithAql2");
//                            }
//                            //Defect list
//                            if (ReportInfo.dsDefect != null)
//                            {
//                                InspReport.Report.RegisterData(ReportInfo.dsDefect);
//                            }
//                            //Defect list Spec
//                            if (ReportInfo.dsDefectSpec != null)
//                            {
//                                InspReport.Report.RegisterData(ReportInfo.dsDefectSpec);
//                            }
//                            //Defect list Spec Packing
//                            if (ReportInfo.dsDefectPacking != null)
//                            {
//                                InspReport.Report.RegisterData(ReportInfo.dsDefectPacking);
//                            }
//                            //MSChart list
//                            if (ReportInfo.dsMSChart != null)
//                            {
//                                InspReport.Report.RegisterData(ReportInfo.dsMSChart);
//                            }
//                            //Order Details
//                            if (ReportInfo.lstOrderDetail != null && ReportInfo.lstOrderDetail.Count > 0)
//                            {
//                                InspReport.Report.RegisterData(ReportInfo.lstOrderDetail, "sections");
//                            }

//                            //EDEN PARK Colorway / Size Range details
//                            if (ReportInfo.dsColorway != null)
//                            {
//                                InspReport.Report.RegisterData(ReportInfo.dsColorway, "ColorwaySizes");
//                            }

//                            //EDEN PARK Products
//                            if (ReportInfo.dsProductsColor != null)
//                            {
//                                InspReport.Report.RegisterData(ReportInfo.dsProductsColor, "ProductsColor");
//                            }

//                            #region Fabric
//                            //Result for Inspected Qty in Rolls 
//                            if (ReportInfo.ChartResultQtyinYards != null)
//                            {
//                                InspReport.Report.RegisterData(ReportInfo.ChartResultQtyinYards, "ChartResultQtyinYards");
//                            }
//                            //Result for Inspected Qty in Rolls
//                            if (ReportInfo.ChartResultQtyinRolls != null)
//                            {
//                                InspReport.Report.RegisterData(ReportInfo.ChartResultQtyinRolls, "ChartResultQtyinRolls");
//                            }
//                            //Fabric Rubbing Test
//                            if (ReportInfo.dsFabricRubbingTest != null)
//                            {
//                                InspReport.Report.RegisterData(ReportInfo.dsFabricRubbingTest, "FabricRubbingTest");
//                            }
//                            //Fabric Inspection Detail
//                            if (ReportInfo.dsFabricInspectionDetail != null)
//                            {
//                                InspReport.Report.RegisterData(ReportInfo.dsFabricInspectionDetail, "FabricInspectionDetail");
//                            }
//                            #endregion
//                            #endregion

//                            #region save report
//                            string ReportPath = "";
//                            InspReport.Report.Prepare();
//                            string fileName = "";
//                            string filePath = "";
//                            string fileExtension = "pdf";
//                            fileExtension = Enum.GetName(typeof(FileExtension), FileExtensionID);

//                            fileName = $"Insp_{ReportInfo.lstOrderMainDetail[0].CustomerId}_{ReportInfo.lstOrderMainDetail[0].InspectionNo}_{DateTime.Now.ToString("yyyyMMddHHmmss")}." + fileExtension;
//                            filePath = Path.Combine("Views/Report_Templates/SGT", fileName);
//                            if (FileExtensionID == (int)FileExtension.xlsx)
//                            {
//                                Excel2007Export excel2007Export = new Excel2007Export();
//                                excel2007Export.PrintOptimized = false;
//                                using (var memory = new MemoryStream())
//                                {
//                                    InspReport.Report.Export(excel2007Export, memory);
//                                    var result = _manager.FetchCloudReportUrl(memory, fileName, fileExtension, FileContainerList.InspectionReport);//upload file to cloud
//                                    ReportPath = result.filePath;
//                                }
//                            }
//                            else if (FileExtensionID == (int)FileExtension.pdf)
//                            {
//                                PDFExport reportPDF = new PDFExport();
//                                reportPDF.PrintOptimized = true;
//                                reportPDF.JpegCompression = true;
//                                reportPDF.JpegQuality = 60;
//                                using (var memory = new MemoryStream())
//                                {
//                                    ////InspReport.Report.Export(reportPDF, filePath);
//                                    InspReport.Report.Export(reportPDF, memory);
//                                    var result = _manager.FetchCloudReportUrl(memory, fileName, fileExtension, FileContainerList.InspectionReport);//upload file to cloud
//                                    ReportPath = result.filePath;
//                                }

//                            }
//                            else if (FileExtensionID == (int)FileExtension.docx)
//                            {
//                                Word2007Export word2007Export = new Word2007Export();
//                                word2007Export.PrintOptimized = false;
//                                using (var memory = new MemoryStream())
//                                {
//                                    InspReport.Report.Export(word2007Export, memory);
//                                    var result = _manager.FetchCloudReportUrl(memory, fileName, fileExtension, FileContainerList.InspectionReport);//upload file to cloud
//                                    ReportPath = result.filePath;
//                                }

//                            }
//                            #endregion

//                            InspReport.Report.Dispose();
//                            FBRetrun.ReportId = fbReportId;
//                            FBRetrun.ReportPath = ReportPath;
//                        }

//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                FBRetrun.ReportId = fbReportId;
//                FBRetrun.ReportPath = "";
//                FBRetrun.ErrorInfo = ex?.StackTrace ?? "" + ex?.InnerException ?? "";
//                InspReport.Report.Dispose();
//            }


//            return FBRetrun;
//        }

//        /// <summary>
//        /// Get FB token based on the needs
//        /// </summary>
//        /// <returns></returns>
//        private string getFbToken()
//        {
//            var Fbclaims = new List<Claim>
//            {
//                new Claim("email",_configuration["FbAdminEmail"]),
//                new Claim("firstname", _configuration["FbAdminUserName"]),
//                new Claim("lastname", ""),
//                new Claim("role", "admin"),
//                new Claim("redirect", "")
//            };
//            return AuthentificationService.CreateFBToken(Fbclaims, _configuration["FBKey"]);
//        }


//    }
//}
