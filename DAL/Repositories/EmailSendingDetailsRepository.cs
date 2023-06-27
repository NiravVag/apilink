using Contracts.Repositories;
using DTO.Common;
using DTO.EmailSend;
using DTO.EmailSendingDetails;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    /// <summary>
    /// email sending details repo 
    /// </summary>
    public class EmailSendingDetailsRepository : Repository, IEmailSendingDetailsRepository
    {

        public EmailSendingDetailsRepository(API_DBContext context) : base(context)
        {
        }

        /// <summary>
        /// get customer decision email repo
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        public async Task<List<CustomerDecisionEmailData>> GetCustomerDecisionEmailData(List<int> reportIdList)
        {
            return await _context.InspProductTransactions.
                 Where(x => x.Active.Value && reportIdList.Contains(x.FbReportId.GetValueOrDefault()))
                 .Select(x => new CustomerDecisionEmailData()
                 {
                     BookingId = x.InspectionId,
                     PoNumberList = x.InspPurchaseOrderTransactions.Where(y => y.Active.Value).Select(y => y.Po.Pono).ToList(),
                     ProductName = x.Product.ProductId,
                     ProductId = x.ProductId,
                     ProductDescription = x.Product.ProductDescription,
                     CombineProductId = x.CombineProductId,
                     ReportId = x.FbReportId,
                     CombineAqlQuantity = x.CombineAqlQuantity,
                     CustomerDecisionResult = x.FbReport.InspRepCusDecisions.FirstOrDefault(z => z.Active.Value).CustomerResult.CustomDecisionName,
                     CustomerDecisionResultId = x.FbReport.InspRepCusDecisions.FirstOrDefault(z => z.Active.Value).CustomerResultId
                 }).ToListAsync();



        }

        /// <summary>
        /// Get factory email contacts by booking
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<string>> GetBookingFactoryEmailContacts(int bookingId)
        {
            return await _context.InspTranFaContacts.Where(x => x.Active && x.InspectionId == bookingId && x.Contact.Mail != null).
                 Select(x => x.Contact.Mail).ToListAsync();
        }
        /// <summary>
        /// Get supplier email contacts by booking
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<string>> GetBookingSupplierEmailContacts(int bookingId)
        {
            return await _context.InspTranSuContacts.Where(x => x.Active && x.InspectionId == bookingId && x.Contact.Mail != null).
               Select(x => x.Contact.Mail).ToListAsync();
        }

        /// <summary>
        /// Get customer email contacts by booking
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<string>> GetBookingCustomerEmailContacts(int bookingId)
        {
            return await _context.InspTranCuContacts.Where(x => x.Active && x.InspectionId == bookingId && x.Contact.Email != null).
             Select(x => x.Contact.Email).ToListAsync();
        }

        /// <summary>
        /// get email configuration data rule
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public async Task<List<EmailSendingDetails>> GetEmailConfiguration(int typeId, int customerId, int serviceId)
        {
            return await _context.EsDetails.
                Where(x => x.TypeId == typeId && x.CustomerId == customerId && x.ServiceId == serviceId && x.Active.HasValue && x.Active.Value).
                Select(x => new EmailSendingDetails
                {
                    Id = x.Id,
                    CustomerId = x.CustomerId,
                    ServiceId = x.ServiceId,
                    ServiceTypeIds = x.EsServiceTypeConfigs.Select(y => y.ServiceTypeId).ToList(),
                    CustomerContactIds = x.EsCuContacts.Select(y => y.CustomerContactId).ToList(),
                    BrandIds = x.EsCuConfigs.Select(y => y.BrandId.GetValueOrDefault()),
                    DepartmentIds = x.EsCuConfigs.Select(y => y.DepartmentId.GetValueOrDefault()),
                    BuyerIds = x.EsCuConfigs.Select(y => y.BuyerId.GetValueOrDefault()),
                    CollectionIds = x.EsCuConfigs.Select(y => y.CollectionId.GetValueOrDefault()),
                    FactoryCountryIds = x.EsFaCountryConfigs.Select(y => y.FactoryCountryId).ToList(),
                    SupplierOrFactoryIds = x.EsSupFactConfigs.Select(y => y.SupplierOrFactoryId).ToList(),
                    Type = x.TypeId,
                    OfficeIds = x.EsOfficeConfigs.Select(y => y.OfficeId).ToList(),
                    ApiContactIds = x.EsApiContacts.Select(y => y.ApiContactId).ToList(),
                    CustomerResultIds = x.EsResultConfigs.Select(y => y.CustomerResultId).ToList(),
                    ProductCategoryIds = x.EsProductCategoryConfigs.Select(y => y.ProductCategoryId).ToList()
                }).ToListAsync();
        }

        /// <summary>
        /// Get API default contacts based on the office
        /// </summary>
        /// <param name="officeId"></param>
        /// <returns></returns>
        public async Task<List<string>> GetAPIDefaultContacts(int officeId)
        {
            return await _context.EsApiDefaultContacts.
                Where(x => x.OfficeId == officeId).
                Select(x => x.ApiContact.CompanyEmail)
                .ToListAsync();
        }

        /// <summary>
        /// get customer decision email repo
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        public async Task<List<CustomerDecisionEmailContainerData>> GetContainerCustomerDecisionEmailData(List<int> reportIdList)
        {
            return await _context.InspContainerTransactions.Where(x => x.Active.Value && reportIdList.Contains(x.FbReportId.GetValueOrDefault())).
                OrderBy(x => x.InspectionId).
                Select(x => new CustomerDecisionEmailContainerData()
                {
                    BookingId = x.InspectionId,
                    ContainerNumber = x.ContainerSizeNavigation.Name,
                    TotalBookingQty = x.TotalBookingQuantity,
                    ReportId = x.FbReportId,
                    ContainerId = x.ContainerId,
                    PoNumberList = x.InspPurchaseOrderTransactions.Where(y => y.Active.Value).Select(y => y.Po.Pono).ToList(),
                    CustomerDecisionResult = x.FbReport.InspRepCusDecisions.FirstOrDefault(z => z.Active.Value).CustomerResult.CustomDecisionName,
                    CustomerDecisionResultId = x.FbReport.InspRepCusDecisions.FirstOrDefault(z => z.Active.Value).CustomerResultId,
                }).ToListAsync();


        }

        /// <summary>
        /// get report details
        /// </summary>
        /// <param name="reportIdList"></param>
        /// <returns></returns>
        public async Task<List<ReportDetailsRepo>> GetCusDecisionReportDetails(IEnumerable<int> reportIdList)
        {
            return await _context.FbReportDetails.Where(x => x.Active.Value && reportIdList.Contains(x.Id)).
                Select(x => new ReportDetailsRepo()
                {
                    BookingId = x.Inspection.Id,
                    ReportId = x.Id,
                    ReportName = x.ReportTitle,
                    ReportRevision = x.ReportRevision,
                    ReportVersion = x.ReportVersion,
                    ReportLink = x.FinalReportPath,
                    ReportResult = x.Result.ResultName,
                    ReportSummaryLink = x.ReportSummaryLink,
                    ReportStatus = x.FbReportStatusNavigation.FbstatusName,
                    ReportStatusId = x.FbReportStatus,
                    SupplierName = x.Inspection.Supplier.SupplierName,
                    ServiceType = x.Inspection.InspTranServiceTypes.Where(y => y.Active).Select(z => z.ServiceType.Abbreviation).FirstOrDefault(),
                    ServiceDateFrom = x.Inspection.ServiceDateFrom,
                    CustomerDecisionResult = x.InspRepCusDecisions.Where(y => y.Active.Value).Select(y => y.CustomerResult.CustomDecisionName).FirstOrDefault(),
                    CustomerDecisionResultId = x.InspRepCusDecisions.Where(y => y.Active.Value).Select(y => y.CustomerResultId).FirstOrDefault()
                }).ToListAsync();
        }

    }
}
