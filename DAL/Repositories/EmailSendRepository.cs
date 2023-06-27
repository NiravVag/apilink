using Contracts.Repositories;
using DTO.CommonClass;
using DTO.EmailSend;
using Entities;
using Microsoft.EntityFrameworkCore;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO.Common;
using DTO.EmailLog;
using DTO.Inspection;
using DTO.Invoice;

namespace DAL.Repositories
{
    public class EmailSendRepository : Repository, IEmailSendRepository
    {

        public EmailSendRepository(API_DBContext context) : base(context)
        {
        }

        /// <summary>
        /// get report details
        /// </summary>
        /// <param name="reportIdList"></param>
        /// <returns></returns>
        public async Task<List<ReportDetailsRepo>> GetReportDetails(IEnumerable<int> reportIdList)
        {
            return await _context.FbReportDetails.Where(x => x.Active.Value && reportIdList.Contains(x.Id)).
                Select(x => new ReportDetailsRepo()
                {
                    BookingId = x.InspProductTransactions.Where(y => y.Active.Value).Select(y => y.InspectionId).FirstOrDefault(),
                    ReportId = x.Id,
                    FbReportId = x.FbReportMapId,
                    RequestedReportRevision = x.RequestedReportRevision,
                    ReportVersion = x.ReportVersion,
                    ReportRevision = x.ReportRevision,
                    ReportSendRevisison = x.LogBookingReportEmailQueues.
                    Where(x => x.EmailLog.Status == (int)EmailStatus.Success).
                    OrderByDescending(x => x.Id).Select(y => y.ReportRevision).FirstOrDefault(),
                    ReportSendCount = x.LogBookingReportEmailQueues.
                    Where(x => x.EmailLog.Status == (int)EmailStatus.Success).
                    OrderByDescending(x => x.Id).Select(y => y.ReportRevision).Count(),
                    ReportName = x.ReportTitle,
                    ReportLink = x.FinalReportPath,
                    FinalManualReportPath = x.FinalManualReportPath,
                    ReportResult = x.Result.ResultName,
                    ReportSummaryLink = x.ReportSummaryLink,
                    ReportImagePath = x.ReportPicturePath,
                    ReportStatus = x.FbReportStatusNavigation.FbstatusName,
                    ReportStatusId = x.FbReportStatus,
                    CustomerDecisionResultId = x.InspRepCusDecisions.Where(y => y.Active.Value).Select(y => y.CustomerResultId).FirstOrDefault(),
                    CustomerDecisionDate = x.InspRepCusDecisions.Where(y => y.Active.Value).Select(y => y.CreatedOn).FirstOrDefault(),
                    CustomerDecisionComments = x.InspRepCusDecisions.Where(y => y.Active.Value).Select(y => y.Comments).FirstOrDefault(),
                    Observations = x.MainObservations
                }).ToListAsync();
        }



        public async Task<List<ReportDetailsRepo>> GetInvoiceReportDetails(List<string> InvoiceNoList)
        {
            return await _context.InvTranFiles.Where(x => x.Active.Value && !x.FileTypeNavigation.IsUpload == null && InvoiceNoList.Contains(x.InvoiceNo)).
                Select(x => new ReportDetailsRepo()
                {
                    ReportLink = x.FilePath,
                    FinalManualReportPath = x.FilePath,
                    ReportSummaryLink = x.FilePath,
                    InvoiceNo = x.InvoiceNo,
                    BookingId = x.Invoice.InspectionId.GetValueOrDefault(),
                    TotalInvoiceFees = x.Invoice.TotalInvoiceFees.GetValueOrDefault(),
                    InvoiceCurrencyCode = x.Invoice.InvoiceCurrencyNavigation.CurrencyCodeA
                }).ToListAsync();
        }


        /// <summary>
        /// Get Container fb report details
        /// </summary>
        /// <param name="reportIdList"></param>
        /// <returns></returns>
        public async Task<List<ReportDetailsRepo>> GetContainerFbReportDetails(IEnumerable<int> reportIdList)
        {
            return await _context.FbReportDetails.Where(x => x.Active.Value && reportIdList.Contains(x.Id)).
                Select(x => new ReportDetailsRepo()
                {
                    BookingId = x.InspContainerTransactions.Select(y => y.InspectionId).FirstOrDefault(),
                    ReportId = x.Id,
                    ReportRevision = x.ReportRevision,
                    ReportVersion = x.ReportVersion,
                    ReportName = x.ReportTitle,
                    ReportLink = x.FinalReportPath,
                    FinalManualReportPath = x.FinalManualReportPath,
                    ReportImagePath = x.ReportPicturePath,
                    ReportResult = x.Result.ResultName,
                    ReportSummaryLink = x.ReportSummaryLink,
                    ReportStatus = x.FbReportStatusNavigation.FbstatusName,
                    CustomerDecisionResultId = x.InspRepCusDecisions.Where(y => y.Active.Value).Select(y => y.CustomerResultId).FirstOrDefault(),
                }).ToListAsync();
        }

        /// <summary>
        /// get product details
        /// </summary>
        /// <param name="bookingIdList"></param>
        /// <returns></returns>
        public async Task<List<InspectionProductRepo>> GetProductDetails(IEnumerable<int> bookingIdList)
        {
            return await _context.InspPurchaseOrderTransactions.Where(x => x.Active.Value && x.ProductRef.Active.Value && bookingIdList.Contains(x.InspectionId)).
                OrderBy(x => x.InspectionId)
                .Select(x => new InspectionProductRepo()
                {
                    BookingId = x.InspectionId,
                    PoId = x.PoId,
                    PoNumber = x.Po.Pono,
                    Etd = x.Etd.ToString(),
                    DestinationCountry = x.DestinationCountry.CountryName,
                    ProductName = x.ProductRef.Product.ProductId,
                    ProductId = x.ProductRef.ProductId,
                    ProductDesc = x.ProductRef.Product.ProductDescription,
                    CombineProductId = x.ProductRef.CombineProductId,
                    TotalBookingQty = x.BookingQuantity,
                    ReportId = x.ProductRef.FbReportId,
                    CombineAqlQuantity = x.ProductRef.CombineAqlQuantity,
                }).ToListAsync();
        }


        /// <summary>
        /// Get Non container active products
        /// </summary>
        /// <param name="bookingIdList"></param>
        /// <returns></returns>
        public async Task<List<InspectionProductRepo>> GetNonContainerProductDetails(IEnumerable<int> bookingIdList)
        {
            return await _context.InspProductTransactions.
                Where(x => x.Active.Value && bookingIdList.Contains(x.InspectionId) &&
                x.Inspection.InspTranServiceTypes.Any(y => y.ServiceTypeId != (int)InspectionServiceTypeEnum.Container)).
                OrderBy(x => x.InspectionId)
                .Select(x => new InspectionProductRepo()
                {
                    BookingId = x.InspectionId,
                    ProductRefId = x.Id,
                    ProductName = x.Product.ProductId,
                    ProductId = x.ProductId,
                    ProductDesc = x.Product.ProductDescription,
                    CombineProductId = x.CombineProductId,
                    ReportId = x.FbReportId,
                    CombineAqlQuantity = x.CombineAqlQuantity,
                    InspectedQty = x.FbReport.InspectedQty,
                    OrderQty = x.FbReport.OrderQty,
                    PresentedQty = x.FbReport.PresentedQty
                }).ToListAsync();
        }


        /// <summary>
        /// get container details
        /// </summary>
        /// <param name="bookingIdList"></param>
        /// <returns></returns>
        public async Task<List<InspectionContainerRepo>> GetContainerDetails(IEnumerable<int> bookingIdList)
        {
            return await _context.InspContainerTransactions.Where(x => x.Active.Value && bookingIdList.Contains(x.InspectionId)).
                OrderBy(x => x.InspectionId).
                Select(x => new InspectionContainerRepo()
                {
                    ContainerRefId = x.Id,
                    BookingId = x.InspectionId,
                    ContainerNumber = x.ContainerSizeNavigation.Name,
                    TotalBookingQty = x.TotalBookingQuantity,
                    ReportId = x.FbReportId,
                    ContainerId = x.ContainerId,
                }).ToListAsync();
        }

        /// <summary>
        /// get email send data
        /// </summary>
        /// <param name="emailSendFileId"></param>
        /// <returns></returns>
        public async Task<EsTranFile> GetEmailSendData(int emailSendFileId)
        {
            return await _context.EsTranFiles.Where(x => x.Active.Value && x.Id == emailSendFileId)
                .FirstOrDefaultAsync();
        }

        public async Task<InvTranFile> GetInvoiceSendData(int emailSendFileId)
        {
            return await _context.InvTranFiles.Where(x => x.Active.Value && x.Id == emailSendFileId)
                .FirstOrDefaultAsync();
        }

        public async Task<List<EmailSendInvoiceRepo>> GetEmailSendInvoiceList(List<string> invoiceList)
        {
            return await _context.InvAutTranDetails.Where(x => invoiceList.Contains(x.InvoiceNo))
                .Select(x => new EmailSendInvoiceRepo()
                {
                    InvoiceId = x.Id,
                    InvoiceNo = x.InvoiceNo,
                    InvoiceDate = x.InvoiceDate,
                    InvoiceTotal = x.TotalInvoiceFees,
                    BilledName = x.Rule.InvoiceRequestBilledName,
                    BillTo = x.Rule.BillingTo.Label,
                    InvoiceType = x.InvoiceTypeNavigation.Name,
                    BookingId = x.InspectionId.GetValueOrDefault(),
                    CurrencyCode = x.InvoiceCurrencyNavigation.CurrencyCodeA,
                    InvoiceFileUrl = x.InvTranFiles.Where(x => x.Active.Value).
                                     Select(x => x.FilePath).FirstOrDefault()
                }).AsNoTracking().ToListAsync();
        }

        public async Task<FbReportDetail> GetFbReportInfo(int apiReportId)
        {
            return await _context.FbReportDetails.Where(x => x.Active.Value && x.Id == apiReportId)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// get file type list
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<CommonDataSource>> GetFileTypeList()
        {
            return await _context.EsRefFileTypes.Where(x => x.Active.Value)
                      .Select(x => new CommonDataSource { Id = x.Id, Name = x.Name }).ToListAsync();
        }

        public async Task<IEnumerable<CommonDataSource>> GetInvoiceFileTypeList()
        {
            return await _context.InvRefFileTypes.Where(x => x.Active && x.IsUpload.Value)
                      .Select(x => new CommonDataSource { Id = x.Id, Name = x.Name }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get email send transaction file details
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EmailSendFileDetailsRepo>> GetEmailFileList(BookingReportRequest request)
        {
            return await _context.EsTranFiles.Where(x => x.Active.Value && request.BookingIdList.Contains(x.InspectionId.GetValueOrDefault()))
                .Select(x => new EmailSendFileDetailsRepo()
                {
                    BookingId = x.InspectionId,
                    ReportId = x.ReportId,
                    FileTypeId = x.FileTypeId,
                    FileLink = x.FileLink,
                    FileName = x.FileName,
                    EmailSendFileId = x.Id,
                    FileTypeName = x.FileType.Name,
                    ReportName = x.Report.ReportTitle
                }).ToListAsync();
        }


        public async Task<IEnumerable<EmailSendFileDetailsRepo>> GetInvoiceSendFileList(InvoiceSendFilesRequest request)
        {
            return await _context.InvTranFiles.Where(x => x.Active.Value
            && (x.FileTypeNavigation.IsUpload.Value)
            && request.InvoiceNoList.Contains(x.InvoiceNo))
                .Select(x => new EmailSendFileDetailsRepo()
                {
                    InvoiceId = x.InvoiceId,
                    InvoiceNo = x.InvoiceNo,
                    FileTypeId = x.FileType,
                    FileLink = x.FilePath,
                    FileName = x.FileName,
                    EmailSendFileId = x.Id,
                    FileTypeName = x.FileTypeNavigation.Name
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the email rule by customer and service 
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        public async Task<List<ReportEmailSendType>> GetEmailDataByCustomer(int customerId, int serviceId)
        {
            return await _context.EsDetails.Where(x => x.CustomerId == customerId && x.ServiceId == serviceId &&
             x.Active == true)
             .Select(y => new ReportEmailSendType
             {
                 CustomerId = y.CustomerId.GetValueOrDefault(),
                 ServiceId = y.ServiceId,
                 ReportSendType = y.ReportSendType
             }).ToListAsync();
        }

        public async Task<IEnumerable<int>> GetBookingNumbersbyInvoiceList(List<string> InvoiceList)
        {
            return await _context.InvAutTranDetails.Where(x => InvoiceList.Contains(x.InvoiceNo))
             .Select(y => y.InspectionId.GetValueOrDefault()).Distinct().ToListAsync();
        }

        public async Task<IEnumerable<InvoiceBookingEmailSend>> GetBookingDataByInvoiceList(List<string> InvoiceList)
        {
            return await _context.InvAutTranDetails.Where(x => InvoiceList.Contains(x.InvoiceNo))
             .Select(y => new InvoiceBookingEmailSend()
             {
                 bookingId = y.InspectionId,
                 InvoiceNo = y.InvoiceNo
             }).AsNoTracking().ToListAsync();
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<BookingDetails>> GetBookingDetails(IEnumerable<int> bookingIds)
        {
            return await _context.InspTransactions.Where(x => bookingIds.Contains(x.Id)).Select(x => new BookingDetails
            {
                BookingId = x.Id,
                FactoryId = x.FactoryId,
                SupplierId = x.SupplierId,
                CustomerId = x.CustomerId,
                OfficeId = x.OfficeId,
                FactoryCountryId = x.Factory.SuAddresses.Where(y => y.AddressTypeId == (int)Supplier_Address_Type.HeadOffice).
                                              Select(y => y.CountryId).FirstOrDefault(),
                CollectionId = x.CollectionId,
                Customer = x.Customer.CustomerName,
                Supplier = x.Supplier.SupplierName,
                Factory = x.Factory.SupplierName
            }).ToListAsync();
        }


        /// <summary>
        /// Get the Email Send Configuration Base Data
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="customerId"></param>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        public async Task<List<EmailSendConfigBaseDetails>> GetEmailConfigurationBaseDetails(int typeId, List<int> customerList, int serviceId, int? invoiceType = null, bool isAnyCustomerPreInvoiceRuleConfigured = false)
        {
            var query = _context.EsDetails.Where(x => x.TypeId == typeId && x.ServiceId == serviceId && x.Active.Value);

            //if type is invoice status
            if (typeId == (int)EmailSendingType.InvoiceStatus)
            {
                query = query.Where(x => x.InvoiceTypeId == invoiceType);
            }

            //1. if type is invoice status and invoice type is pre invocie and any customerrule configured then filter with customer list
            //2. if type is invoice status and invoice type is monthly then filter with customer list
            //3. if type is not invoice status then filter with customer list
            if ((typeId == (int)EmailSendingType.InvoiceStatus && invoiceType == (int)INVInvoiceType.PreInvoice && isAnyCustomerPreInvoiceRuleConfigured) || (typeId == (int)EmailSendingType.InvoiceStatus && invoiceType == (int)INVInvoiceType.Monthly) || typeId != (int)EmailSendingType.InvoiceStatus)
            {
                query = query.Where(x => customerList.Contains(x.CustomerId.GetValueOrDefault()));
            }
            //this condition for pick the default data when type is invoice status and invoice type is preinvoice
            else if ((typeId == (int)EmailSendingType.InvoiceStatus && invoiceType == (int)INVInvoiceType.PreInvoice && !isAnyCustomerPreInvoiceRuleConfigured))
            {
                query = query.Where(x => x.CustomerId == null);
            }

            return await query.
                Select(x => new EmailSendConfigBaseDetails
                {
                    Id = x.Id,
                    CustomerId = x.CustomerId.GetValueOrDefault(),
                    ServiceId = x.ServiceId,
                    Type = x.TypeId,
                    ReportInEmail = x.ReportInEmailNavigation.Name,
                    ReportSendTypeId = x.ReportSendTypeNavigation.Id,
                    ReportSendType = x.ReportSendTypeNavigation.Name
                }).AsNoTracking().ToListAsync();
        }

        public async Task<bool> IsAnyCustomerPreInvoiceRuleConfigured(int typeId, List<int> customerList, int serviceId, int? invoiceType = null)
        {
            return await _context.EsDetails.AsNoTracking().AnyAsync(x => customerList.Contains(x.CustomerId.GetValueOrDefault()) && x.Active.Value && x.TypeId == typeId && x.InvoiceTypeId == invoiceType && x.ServiceId == serviceId);
        }

        /// <summary>
        /// Get the Email Send Customer Configuration Data
        /// </summary>
        /// <param name="esDetailsId"></param>
        /// <returns></returns>
        public async Task<List<EmailSendCustomerConfigDetails>> GetESCustomerConfigDetails(List<int> esDetailsId)
        {
            return await _context.EsCuConfigs.
                Where(x => esDetailsId.Contains(x.EsDetailsId)).
                Select(x => new EmailSendCustomerConfigDetails
                {
                    BrandId = x.BrandId,
                    EsDetailsId = x.EsDetailsId,
                    DepartmentId = x.DepartmentId,
                    BuyerId = x.BuyerId,
                    CollectionId = x.CollectionId,
                    BrandName = x.Brand.Name,
                    DepartmentName = x.Department.Name,
                    BuyerName = x.Buyer.Name,
                    CollectionName = x.Collection.Name
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the Email Send Customer contact data
        /// </summary>
        /// <param name="esDetailsId"></param>
        /// <returns></returns>
        public async Task<List<EmailSendCustomerContactDetails>> GetESCustomerContactDetails(List<int> esDetailsId)
        {
            return await _context.EsCuContacts.
                Where(x => esDetailsId.Contains(x.EsDetailsId) && x.CustomerContact.Active.Value).
                Select(x => new EmailSendCustomerContactDetails()
                {
                    CustomerContactId = x.CustomerContactId,
                    CustomerContactEmail = x.CustomerContact.Email,
                    ESDetailId = x.EsDetailsId
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the Email Send Service Type data
        /// </summary>
        /// <param name="esDetailsId"></param>
        /// <returns></returns>
        public async Task<List<EmailSendServiceTypeDetails>> GetESServiceTypeDetails(List<int> esDetailsId)
        {
            return await _context.EsServiceTypeConfigs.
                Where(x => esDetailsId.Contains(x.EsDetailsId)).
                Select(x => new EmailSendServiceTypeDetails()
                {
                    ServiceTypeId = x.ServiceTypeId,
                    ServiceTypeName = x.ServiceType.Name,
                    ESDetailId = x.EsDetailsId
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the Email Config Factory Country Data
        /// </summary>
        /// <param name="esDetailsId"></param>
        /// <returns></returns>
        public async Task<List<EmailSendFactoryCountryDetails>> GetESFactoryCountryDetails(List<int> esDetailsId)
        {
            return await _context.EsFaCountryConfigs.
                Where(x => esDetailsId.Contains(x.EsDetailsId)).
                Select(x => new EmailSendFactoryCountryDetails()
                {
                    FactoryCountryId = x.FactoryCountryId,
                    FactoryCountryName = x.FactoryCountry.CountryName,
                    ESDetailId = x.EsDetailsId
                }).AsNoTracking().ToListAsync();
        }
        /// <summary>
        /// Get the Email Configured Supplier or Factory Id
        /// </summary>
        /// <param name="esDetailsId"></param>
        /// <returns></returns>
        public async Task<List<EmailSendSupplierFactoryDetails>> GetESSupplierOrFactoryDetails(List<int> esDetailsId)
        {
            return await _context.EsSupFactConfigs.
                Where(x => esDetailsId.Contains(x.EsDetailsId)).
                Select(x => new EmailSendSupplierFactoryDetails()
                {
                    SupplierId = x.SupplierOrFactoryId,
                    SupplierName = x.SupplierOrFactory.SupplierName,
                    SupplierType = x.SupplierOrFactory.Type.Id,
                    ESDetailId = x.EsDetailsId
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the Email Configured office data
        /// </summary>
        /// <param name="esDetailsId"></param>
        /// <returns></returns>
        public async Task<List<EmailSendOfficeDetails>> GetESOfficeDetails(List<int> esDetailsId)
        {
            return await _context.EsOfficeConfigs.
                Where(x => esDetailsId.Contains(x.EsDetailsId)).
                Select(x => new EmailSendOfficeDetails()
                {
                    OfficeId = x.OfficeId,
                    ESDetailId = x.EsDetailsId
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the Email Configured Api Contact Ids
        /// </summary>
        /// <param name="esDetailsId"></param>
        /// <returns></returns>
        public async Task<List<ESApiContacts>> GetESApiContactDetails(List<int> esDetailsId)
        {
            return await _context.EsApiContacts.
                Where(x => esDetailsId.Contains(x.EsDetailsId) && x.ApiContact.Active.Value).
                Select(x => new ESApiContacts()
                {
                    ApiContactId = x.ApiContactId,
                    CompanyEmail = x.ApiContact.CompanyEmail,
                    EsDetailId = x.EsDetailsId
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the Email Configured Special Rule Data
        /// </summary>
        /// <param name="esDetailsId"></param>
        /// <returns></returns>
        public async Task<List<EmailSendResultDetails>> GetESReportResultDetails(List<int> esDetailsId)
        {
            return await _context.EsResultConfigs.
                Where(x => esDetailsId.Contains(x.EsDetailsId)).
                Select(x => new EmailSendResultDetails()
                {
                    ApiResultId = x.ApiResultId,
                    ApiResultName = x.ApiResult.ResultName,
                    CustomerResultId = x.CustomerResultId,
                    ESDetailId = x.EsDetailsId
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the Email Configured Product Category Data
        /// </summary>
        /// <param name="esDetailsId"></param>
        /// <returns></returns>
        public async Task<List<ESProductCategoryDetails>> GetESProductCategoryDetails(List<int> esDetailsId)
        {
            return await _context.EsProductCategoryConfigs.
                Where(x => esDetailsId.Contains(x.EsDetailsId)).
                Select(x => new ESProductCategoryDetails()
                {
                    Id = x.ProductCategoryId,
                    Name = x.ProductCategory.Name,
                    ESDetailId = x.EsDetailsId
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the Email Configured Special Rule Data
        /// </summary>
        /// <param name="esDetailsId"></param>
        /// <returns></returns>
        public async Task<List<ESSpecialRuleDetails>> GetESSpecialRuleDetails(List<int> esDetailsId)
        {
            return await _context.EsSpecialRules.
                Where(x => esDetailsId.Contains(x.EsDetailsId.Value)).
                Select(x => new ESSpecialRuleDetails()
                {
                    Id = x.SpecialRule.Id,
                    Name = x.SpecialRule.Name,
                    ESDetailId = x.EsDetailsId
                }).AsNoTracking().ToListAsync();
        }

        public async Task<List<ESToCCDetails>> GetESRecipientDetails(List<int> esDetailsId)
        {
            return await _context.EsRecipientTypes.
                Where(x => esDetailsId.Contains(x.EsDetailsId.Value)).
                Select(x => new ESToCCDetails()
                {
                    RecipientId = x.RecipientTypeId,
                    IsToValue = x.IsTo,
                    IsCCValue = x.IsCc,
                    ESDetailId = x.EsDetailsId
                }).AsNoTracking().ToListAsync();
        }



        public IQueryable<EmailRuleDataRepo> GetEmailRuleData(int emailRuleId)
        {
            return _context.EsDetails.Where(x => x.Id == emailRuleId && x.Active.Value).Select(x => new EmailRuleDataRepo()
            {
                RuleId = x.Id,
                EmailSendType = x.TypeId,
                ReportSendType = x.ReportSendType,
                RecipientName = x.RecipientName,
                SubjectDelimeterName = x.EmailSubjectNavigation.Delimiter.Name,
                FileDelimeterName = x.FileName.Delimiter.Name,
                EmailSize = x.EmailSizeNavigation.Value,
                ReportInEmail = x.ReportInEmail,
                NoOfReports = x.NoOfReports,
                EmailSubjectId = x.EmailSubject,
                EmailFileId = x.FileNameId,
                IsPictureFileInEmail = x.IsPictureFileInEmail,
                CustomerId = x.CustomerId
            });
        }

        public async Task<List<EmailRuleTemplateDetailsRepo>> GetEmailRuleSubjectTemplateData(int? EmailSubjectId)
        {
            return await _context.EsSuTemplateDetails.Where(x => x.TemplateId == EmailSubjectId).
                OrderBy(x => x.Sort).Select(x => new EmailRuleTemplateDetailsRepo()
                {
                    RuleId = x.Id,
                    FieldId = x.Field.Id,
                    FieldName = x.Field.FieldName,
                    FieldIsText = x.Field.IsText,
                    IsTitle = x.IsTitle,
                    TitleCustomName = x.TitleCustomName,
                    DateFormatId = x.DateFormat,
                    IsDateSeparator = x.IsDateSeperator,
                    DateFormat = x.DateFormatNavigation.DateFormat,
                    MaxChar = x.MaxChar,
                    MaxItem = x.MaxItems
                }).AsNoTracking().ToListAsync();
        }

        public async Task<List<EmailRuleTemplateDetailsRepo>> GetEmailRuleFileTemplateData(int? EmailFileId)
        {
            return await _context.EsSuTemplateDetails.Where(x => x.TemplateId == EmailFileId).
                OrderBy(x => x.Sort).Select(x => new EmailRuleTemplateDetailsRepo()
                {
                    RuleId = x.Id,
                    FieldId = x.Field.Id,
                    FieldName = x.Field.FieldName,
                    FieldIsText = x.Field.IsText,
                    IsTitle = x.IsTitle,
                    TitleCustomName = x.TitleCustomName,
                    DateFormat = x.DateFormatNavigation.DateFormat,
                    DateFormatId = x.DateFormat,
                    MaxChar = x.MaxChar,
                    MaxItem = x.MaxItems
                }).AsNoTracking().ToListAsync();
        }

        public async Task<List<EsRecipientType>> GetEmailRecipientDetails(int emailRuleId)
        {
            return await _context.EsRecipientTypes.
               Include(x => x.RecipientType)
              .Where(x => x.EsDetailsId == emailRuleId && x.Active.HasValue && x.Active.Value)
              .ToListAsync();
        }

        /// <summary>
        /// Get Api Default contacts based on office list
        /// </summary>
        /// <param name="officeIds"></param>
        /// <returns></returns>
        public async Task<List<string>> GetAPIDefaultContacts(List<int?> officeIds)
        {
            return await _context.EsApiDefaultContacts.
                Where(x => officeIds.Contains(x.OfficeId)).
                Select(x => x.ApiContact.CompanyEmail)
                .ToListAsync();
        }

        public async Task<List<EmailInspectionDetail>> GetEmailInspectionDetails(IEnumerable<int> bookingIdList)
        {
            return await _context.InspTransactions.Where(x => bookingIdList.Contains(x.Id)).
                OrderBy(x => x.Id)
                .Select(x => new EmailInspectionDetail()
                {
                    InspectionID = x.Id,
                    CustomerBookingNo = x.CustomerBookingNo,
                    CustomerId = x.CustomerId,
                    Customer = x.Customer.CustomerName,
                    Supplier = x.Supplier.SupplierName,
                    SupplierCode = x.Customer.SuSupplierCustomers.
                                   Where(y => y.SupplierId == x.SupplierId).Select(y => y.Code).FirstOrDefault(),
                    Factory = x.Factory.SupplierName,
                    Collection = x.Collection.Name,
                    Office = x.Office.LocationName,
                    ServiceType = x.InspTranServiceTypes.Where(x => x.Active).Select(y => y.ServiceType.Name).FirstOrDefault(),
                    ServiceTypeCode = x.InspTranServiceTypes.Select(y => y.ServiceType.Abbreviation).FirstOrDefault(),
                    ServiceDate = x.ServiceDateTo.ToString(StandardDateFormat),
                    FactoryCountry = x.Factory.SuAddresses.Select(z => z.Country.CountryName).FirstOrDefault()
                }).ToListAsync();
        }

        /// <summary>
        /// Get the log booking report email queues
        /// </summary>
        /// <returns></returns>
        public IQueryable<LogBookingReportEmailQueueData> GetLogBookingReportEmailQueues()
        {

            return _context.LogBookingReportEmailQueues.Where(x => x.EsTypeId == (int)EmailSendingType.ReportSend)
                                            .Select(y => new LogBookingReportEmailQueueData()
                                            {
                                                InspectionId = y.InspectionId,
                                                ReportId = y.ReportId,
                                                EmailLogId = y.EmailLogId
                                            });

        }

        public async Task<List<LogBookingReportEmailQueueData>> GetLogBookingReportEmailQueues(List<int> bookingIds)
        {

            return await _context.LogBookingReportEmailQueues.Where(x => x.EsTypeId == (int)EmailSendingType.ReportSend && bookingIds.Contains(x.InspectionId.Value))
                                            .Select(y => new LogBookingReportEmailQueueData()
                                            {
                                                InspectionId = y.InspectionId,
                                                ReportId = y.ReportId,
                                                EmailLogId = y.EmailLogId
                                            }).ToListAsync();

        }

        /// <summary>
        /// Get the LogEmailQueues
        /// </summary>
        /// <returns></returns>
        public async Task<List<LogEmailQueues>> GetLogEmailQueues(List<int?> emailLogIds)
        {
            return await _context.LogEmailQueues.Where(x => emailLogIds.Contains(x.Id) && x.Status == (int)EmailStatus.Success && x.Active)
                                            .Select(y => new LogEmailQueues()
                                            {
                                                EmailLogId = y.Id,
                                                StatusId = y.Status
                                            }).ToListAsync();
        }

        public async Task<List<BookingReportMap>> GetEmailBookingReportMaps(List<int> bookingIds)
        {
            return await _context.LogBookingReportEmailQueues.
                Where(x => x.EsTypeId == (int)EmailSendingType.ReportSend
                && x.EmailLog.Status == (int)EmailStatus.Success
                && bookingIds.Contains(x.InspectionId.GetValueOrDefault()))
                .Select(x => new BookingReportMap()
                {
                    InspectionId = x.InspectionId.GetValueOrDefault(),
                    ReportId = x.ReportId.GetValueOrDefault(),
                    ReportRevision = x.Report.ReportRevision,
                    ReportVersion = x.Report.ReportVersion
                }).AsNoTracking().ToListAsync();
        }

        public async Task<List<BookingReportMap>> GetContainerBookingReportMaps(List<int> bookingIds, int entityId)
        {
            return await _context.InspContainerTransactions.
                Where(x => x.Active.Value && x.EntityId == entityId && x.FbReportId > 0 &&
                 bookingIds.Contains(x.InspectionId))
                .Select(x => new BookingReportMap()
                {
                    InspectionId = x.InspectionId,
                    ReportId = x.FbReportId
                }).AsNoTracking().IgnoreQueryFilters().ToListAsync();
        }


        public async Task<List<BookingReportMap>> GetNonContainerBookingReportMaps(List<int> bookingIds, int entityId)
        {
            return await _context.InspProductTransactions.
                Where(x => x.Active.Value && x.EntityId == entityId && x.FbReportId > 0 &&
                 bookingIds.Contains(x.InspectionId))
                .Select(x => new BookingReportMap()
                {
                    InspectionId = x.InspectionId,
                    ReportId = x.FbReportId
                }).AsNoTracking().IgnoreQueryFilters().ToListAsync();
        }

        /// <summary>
        /// Get the email sent history
        /// </summary>
        /// <param name="inspectionId"></param>
        /// <param name="reportId"></param>
        /// <returns></returns>
        public async Task<List<EmailSendHistoryRepo>> GetEmailSendHistory(int inspectionId, int reportId, int EmailTypeId)
        {
            return await _context.LogBookingReportEmailQueues.Where(x => x.InspectionId == inspectionId && x.ReportId == reportId
                                        && x.EmailLog.Status != (int)EmailStatus.NotStarted && x.EsTypeId == EmailTypeId).
                                        Select(x => new EmailSendHistoryRepo()
                                        {
                                            EmailSentBy = x.EmailLog.CreatedByNavigation.FullName,
                                            EmailSentOn = x.EmailLog.SendOn,
                                            EmailStatus = x.EmailLog.Status.GetValueOrDefault()
                                        }).OrderByDescending(x => x.EmailSentOn).AsNoTracking().ToListAsync();

        }

        public async Task<List<DefectData>> GetDefectData(int bookingId)
        {
            return await _context.FbReportInspDefects.
                Where(x => x.Active.Value && x.FbReportDetail.InspectionId.GetValueOrDefault() == bookingId)
                .Select(x => new DefectData()
                {
                    BookingId = x.FbReportDetail.InspectionId.GetValueOrDefault(),
                    ReportId = x.FbReportDetailId,
                    DefectDesc = x.Description,
                    CriticalDefect = x.Critical.GetValueOrDefault(),
                    MajorDefect = x.Major.GetValueOrDefault(),
                    MinorDefect = x.Minor.GetValueOrDefault(),
                }).ToListAsync();
        }

        public async Task<List<InspPurchaseOrderTransaction>> GetPoTransactionbyBooking(int bookingId)
        {
            return await _context.InspPurchaseOrderTransactions.Include(x => x.InspPurchaseOrderColorTransactions).Where(y => y.Active.HasValue && y.Active.Value && y.InspectionId == bookingId).AsNoTracking().ToListAsync();
        }

        public async Task<List<InspectionReportSummary>> GetInspectionSummaryData(int bookingId)
        {
            return await _context.FbReportInspSummaries.Where(y => y.Active.HasValue && y.Active.Value
            && y.FbReportDetail.InspectionId.Value == bookingId).OrderBy(c => c.Sort).Select(z => new InspectionReportSummary()
            {
                Id = z.Id,
                Name = z.Name,
                Remarks = z.Remarks,
                FbReportDetailId = z.FbReportDetailId,
                Result = z.ResultNavigation.ResultName,
                ResultId = z.ResultId
            }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Add invoice email Attachment
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> AddInvoiceEmailAttachment(InvTranFile entity)
        {
            _context.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<List<InvTranFile>> GetInvoiceEmailAttachment(int invoiceId)
        {
            return await _context.InvTranFiles.Where(x => x.Active.Value && x.InvoiceId == invoiceId).ToListAsync();
        }
    }
}