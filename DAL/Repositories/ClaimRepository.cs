using Contracts.Repositories;
using DTO.Claim;
using DTO.CommonClass;
using DTO.Customer;
using DTO.InvoicePreview;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class ClaimRepository : Repository, IClaimRepository
    {
        public ClaimRepository(API_DBContext context) : base(context)
        {
        }

        public async Task<BookingClaimRepoData> GetBookingData(int bookingId)
        {
            return await _context.InspTransactions.Where(x => x.Id == bookingId)
                .Select(x => new BookingClaimRepoData
                {
                    CustomerId = x.CustomerId,
                    CustomerName = x.Customer.CustomerName,
                    SupplierName = x.Supplier.SupplierName,
                    FactoryName = x.Factory.SupplierName,
                    ServiceFromDate = x.ServiceDateFrom,
                    ServiceToDate = x.ServiceDateTo,
                    FactoryAddress = x.Supplier.SuAddresses.Select(x => x.Address).FirstOrDefault(),
                    FactoryRegionalAddress = x.Supplier.SuAddresses.Select(x => x.LocalLanguage).FirstOrDefault(),
                }).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<ClaimBookingData> GetBookingResponse(int inspectionId)
        {
            return await _context.InspTransactions.Where(x => x.Id == inspectionId).Select(x => new ClaimBookingData
            {
                OfficeId = x.OfficeId,
                CustomerBookingNo = x.CustomerBookingNo,
                CustomerId = x.CustomerId,
                ServiceFromDate = x.ServiceDateFrom,
                ServiceToDate = x.ServiceDateTo,
                CustomerName = x.Customer.CustomerName,
                SupplierName = x.Supplier.SupplierName,
                FactoryName = x.Factory.SupplierName,
                SupplierAddress = x.Supplier.SuAddresses.Select(x => x.Address).FirstOrDefault(),
                FactoryAddress = x.Factory.SuAddresses.Select(x => x.Address).FirstOrDefault(),
                FactoryRegionalAddress = x.Supplier.SuAddresses.Select(x => x.LocalLanguage).FirstOrDefault(),
                ServiceTypeId = x.InspTranServiceTypes.Select(x => x.ServiceTypeId).FirstOrDefault()
            }).FirstOrDefaultAsync();
        }

        public async Task<ClaimData> GetClaimDetailsByClaimId(int claimId)
        {
            return await _context.ClmTransactions.Where(x => x.Id == claimId).Select(x => new ClaimData
            {
                Id = x.Id,
                ClaimNo = x.ClaimNo,
                BookingId = x.InspectionNo.GetValueOrDefault(),
                ClaimDate = x.ClaimDate,
                RequestContactName = x.RequestedContactName,
                ReceivedFromId = x.ReceivedFrom,
                ClaimSourceId = x.ClaimSource,
                ClaimDescription = x.ClaimDescription,
                PriorityId = x.CustomerPriority,
                Priority = x.CustomerPriorityNavigation.Name,
                Amount = x.CustomerReqRefundAmount,
                CurrencyId = x.CustomerReqRefundCurrency,
                Customercomment = x.CustomerComments,
                QcControlId = x.Qccontrol100goods,
                DefectPercentage = x.DefectPercentage,
                NoOfPieces = x.NoOfPieces,
                CompareToAQL = x.CompareToAql,
                DefectDistributionId = x.DefectDistribution,
                Color = x.Color,
                DefectCartonInspected = x.DefectCartonInspected,
                FobPrice = x.FobPrice,
                FobCurrencyId = x.FobCurrency,
                RetailPrice = x.RetailPrice,
                RetailCurrencyId = x.RetailCurrency,
                StatusId = x.StatusId,
                StatusName = x.Status.Name,
                ClaimResultId = x.ClaimValidateResult,
                ClaimRecommendation = x.ClaimRecommendation,
                FinalAmount = x.ClaimRefundAmount,
                FinalCurrencyId = x.ClaimRefundCurrency,
                FinalCurrency = x.ClaimRefundCurrencyNavigation,
                ClaimRefundRemarks = x.ClaimRefundRemarks,
                ClaimRemarks = x.ClaimRemarks,
                RealInspectionFees = x.RealInspectionFees,
                RealInspectionCurrencyId = x.RealInspectionFeesCurrency,
                ClmTranReports = x.ClmTranReports,
                ClmTranCustomerRequests = x.ClmTranCustomerRequests,
                ClmTranCustomerRequestRefunds = x.ClmTranCustomerRequestRefunds,
                ClmTranDefectFamilies = x.ClmTranDefectFamilies,
                ClmTranDepartments = x.ClmTranDepartments,
                ClmTranFinalDecisions = x.ClmTranFinalDecisions,
                ClmTranClaimRefunds = x.ClmTranClaimRefunds,
                ClmTranAttachments = x.ClmTranAttachments,
                ClaimFromId = x.ClaimForm,
                AnalyzerFeedback = x.AnalyzerFeedback
            }).FirstOrDefaultAsync();
        }

        public async Task<List<FbReportQuantityData>> GetFbQuantityData(int bookingId)
        {
            return await _context.FbReportQuantityDetails.Where(x => x.FbReportDetail.InspectionId.GetValueOrDefault() == bookingId && x.Active.HasValue && x.Active.Value)
              .Select(x => new FbReportQuantityData
              {
                  ReportId = x.FbReportDetail.Id,
                  PresentedQty = x.PresentedQuantity.HasValue ? x.PresentedQuantity.Value : 0,
                  InspectedQty = x.InspectedQuantity.HasValue ? x.InspectedQuantity.Value : 0
              }).AsNoTracking().ToListAsync();
        }

        public IQueryable<InspTransaction> GetBookingIdDataSource()
        {
            return _context.InspTransactions.OrderBy(x => x.Id);
        }

        public async Task<List<CommonDataSource>> GetClaimFromList()
        {
            return await _context.ClmRefFroms.Where(x => x.Active)
            .Select(x => new CommonDataSource
            {
                Id = x.Id,
                Name = x.Name
            }).AsNoTracking().ToListAsync();
        }
        public async Task<List<CommonDataSource>> GetReceivedFromList()
        {
            return await _context.ClmRefReceivedFroms.Where(x => x.Active)
            .Select(x => new CommonDataSource
            {
                Id = x.Id,
                Name = x.Name
            }).AsNoTracking().ToListAsync();
        }
        public async Task<List<CommonDataSource>> GetClaimSourceList()
        {
            return await _context.ClmRefSources.Where(x => x.Active)
            .Select(x => new CommonDataSource
            {
                Id = x.Id,
                Name = x.Name
            }).AsNoTracking().ToListAsync();
        }
        public async Task<List<CommonDataSource>> GetFbDefectList()
        {
            return await _context.ClmRefDefectFamilies.Where(x => x.Active && x.Name != null)
            .Select(x => new CommonDataSource
            {
                Id = x.Id,
                Name = x.Name
            }).AsNoTracking().Distinct().ToListAsync();
        }

        public async Task<List<CommonDataSource>> GetClaimDepartmentList()
        {
            return await _context.ClmRefDepartments.Where(x => x.Active)
            .Select(x => new CommonDataSource
            {
                Id = x.Id,
                Name = x.Name
            }).AsNoTracking().Distinct().ToListAsync();
        }

        public async Task<List<CommonDataSource>> GetClaimCustomerRequestList()
        {
            return await _context.ClmRefCustomerRequests.Where(x => x.Active)
            .Select(x => new CommonDataSource
            {
                Id = x.Id,
                Name = x.Name
            }).AsNoTracking().Distinct().ToListAsync();
        }

        public async Task<List<CommonDataSource>> GetClaimPriorityList()
        {
            return await _context.ClmRefPriorities.Where(x => x.Active)
            .Select(x => new CommonDataSource
            {
                Id = x.Id,
                Name = x.Name
            }).AsNoTracking().Distinct().ToListAsync();
        }

        public async Task<List<CommonDataSource>> GetClaimRefundTypeList()
        {
            return await _context.ClmRefRefundTypes.Where(x => x.Active)
            .Select(x => new CommonDataSource
            {
                Id = x.Id,
                Name = x.Name
            }).AsNoTracking().Distinct().ToListAsync();
        }

        public async Task<List<CommonDataSource>> GetClaimDefectDistributionList()
        {
            return await _context.ClmRefDefectDistributions.Where(x => x.Active)
            .Select(x => new CommonDataSource
            {
                Id = x.Id,
                Name = x.Name
            }).AsNoTracking().Distinct().ToListAsync();
        }

        public async Task<List<CommonDataSource>> GetClaimResultList()
        {
            return await _context.ClmRefResults.Where(x => x.Active && x.IsValidate == true)
            .Select(x => new CommonDataSource
            {
                Id = x.Id,
                Name = x.Name
            }).AsNoTracking().Distinct().ToListAsync();
        }

        public async Task<List<CommonDataSource>> GetClaimFinalResultList()
        {
            return await _context.ClmRefResults.Where(x => x.Active && x.IsFinal == true)
            .Select(x => new CommonDataSource
            {
                Id = x.Id,
                Name = x.Name
            }).AsNoTracking().Distinct().ToListAsync();
        }

        public async Task<List<string>> GetClaimFinalResultListByIds(List<int> claimFinalDecisionids)
        {
            return await _context.ClmRefResults.Where(x => claimFinalDecisionids.Contains(x.Id) && x.Active && x.IsFinal == true)
            .Select(x => x.Name).AsNoTracking().ToListAsync();
        }

        public async Task<List<CommonDataSource>> GetCurrencies()
        {
            return await _context.RefCurrencies.Where(x => x.Active)
            .Select(x => new CommonDataSource
            {
                Id = x.Id,
                Name = x.CurrencyName
            }).OrderBy(x => x.Name).AsNoTracking().Distinct().ToListAsync();
        }

        public async Task<List<CommonDataSource>> GetClaimFileTypeList()
        {
            return await _context.ClmRefFileTypes.Where(x => x.Active)
            .Select(x => new CommonDataSource
            {
                Id = x.Id,
                Name = x.Name
            }).AsNoTracking().Distinct().ToListAsync();
        }

        public int AddClaim(ClmTransaction entity)
        {
            _context.ClmTransactions.Add(entity);
            _context.SaveChanges();

            return entity.Id;
        }

        public Task<int> EditClaim(ClmTransaction entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return _context.SaveChangesAsync();
        }

        public void RemoveEntities<T>(IEnumerable<T> entities) where T : class, new()
        {
            if (entities != null && entities.Any())
            {
                foreach (var entity in entities)
                    _context.Entry<T>(entity).State = EntityState.Deleted;
            }
        }

        public async Task<ClmTransaction> GetClaimDetails(int claimId)
        {
            return await _context.ClmTransactions.Where(x => x.Id == claimId)
                .Include(x => x.ClmTranReports)
               .Include(x => x.ClmTranCustomerRequests)
               .Include(x => x.ClmTranCustomerRequestRefunds)
               .Include(x => x.ClmTranDefectFamilies)
               .Include(x => x.ClmTranDepartments)
               .Include(x => x.ClmTranFinalDecisions)
               .Include(x => x.ClmTranClaimRefunds)
               .Include(x => x.ClmTranAttachments).FirstOrDefaultAsync();
        }
        public Task<ItUserMaster> GetUserName(int userId)
        {
            return _context.ItUserMasters.Where(x => x.Id == userId && x.Active).FirstOrDefaultAsync();
        }
        public async Task<List<BookingClaims>> GetClaimByBookingId(int inspectionNo)
        {
            return await _context.ClmTransactions.Where(x => x.InspectionNo == inspectionNo)
                .Select(x => new BookingClaims
                {
                    Id = x.Id,
                    ClaimNo = x.ClaimNo
                }).AsNoTracking().ToListAsync();
        }

        public async Task<CustomerKamDetail> GetCustomerKAMDetails(int customerId)
        {
            return await _context.CuKams.Where(x => x.Active != null && x.Active.Value == 1 && x.Kam.Active.HasValue && x.Kam.Active.Value && x.CustomerId == customerId).
                        Select(x => new CustomerKamDetail()
                        {
                            Name = x.Kam.PersonName,
                            Email = x.Kam.CompanyEmail,
                            PhoneNumber = x.Kam.CompanyMobileNo
                        }).FirstOrDefaultAsync();
        }

        public async Task<InvoiceDetail> GetClaimInvoiceDetails(int inspectionNo)
        {
            return await _context.InvAutTranDetails.Where(x => x.InspectionId == inspectionNo).
                        Select(x => new InvoiceDetail()
                        {
                            InvoiceNo = x.InvoiceNo,
                            BilledDate = x.InvoiceDate,
                            TotalInvoiceFees = x.TotalInvoiceFees,
                            InvoiceCurrency = x.InvoiceCurrency,
                            InvoiceCurrencyName = x.InvoiceCurrencyNavigation.CurrencyName
                        }).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ClmRefStatus>> GetClaimStatusList()
        {
            return await _context.ClmRefStatuses.AsNoTracking().ToListAsync();
        }

        public async Task<IQueryable<ClmTransaction>> GetAllClaimsQuery()
        {
            return _context.ClmTransactions;
        }

        public async Task<List<InspectionQcDetail>> GetQcDetails(IEnumerable<int> claimIds)
        {
            return await (from itUser in _context.ItUserMasters
                          join fbReportQC in _context.FbReportQcdetails on itUser.FbUserId equals fbReportQC.QcId
                          join fbReport in _context.FbReportDetails on fbReportQC.FbReportDetailId equals fbReport.Id
                          join inspTrans in _context.InspTransactions on fbReport.InspectionId equals inspTrans.Id
                          join claim in _context.ClmTransactions on inspTrans.Id equals claim.InspectionNo
                          where claimIds.Contains(claim.Id)
                          select new InspectionQcDetail
                          {
                              ClaimId = claim.Id,
                              BookingId = fbReport.InspectionId.GetValueOrDefault(),
                              QcId = fbReportQC.Id,
                              Name = itUser.Staff.PersonName
                          }).AsNoTracking().ToListAsync();
        }

        public async Task<List<InvoiceDetail>> GetClaimInvoiceDetailByBookingIds(List<int> bookingIds)
        {
            return await _context.InvAutTranDetails.Where(x => bookingIds.Contains(x.InspectionId.GetValueOrDefault())).
                        Select(x => new InvoiceDetail()
                        {
                            InspectionId = x.InspectionId,
                            InvoiceNo = x.InvoiceNo,
                            BilledDate = x.InvoiceDate,
                            TotalInvoiceFees = x.TotalInvoiceFees,
                            InvoiceCurrency = x.InvoiceCurrency,
                            InvoiceCurrencyName = x.InvoiceCurrencyNavigation.CurrencyCodeA
                        }).ToListAsync();
        }

        public async Task<List<FbReportQuantityData>> GetFbQuantityDataByBookingIds(List<int> bookingIds)
        {
            return await _context.FbReportQuantityDetails.Where(x => bookingIds.Contains(x.FbReportDetail.InspectionId.GetValueOrDefault()) && x.Active.HasValue && x.Active.Value)
              .Select(x => new FbReportQuantityData
              {
                  InspectionId = x.FbReportDetail.InspectionId,
                  ReportId = x.FbReportDetail.Id,
                  PresentedQty = x.PresentedQuantity.HasValue ? x.PresentedQuantity.Value : 0,
                  InspectedQty = x.InspectedQuantity.HasValue ? x.InspectedQuantity.Value : 0,
                  OrderQty = x.OrderQuantity.HasValue ? x.OrderQuantity.Value : 0
              }).AsNoTracking().ToListAsync();
        }

        public IQueryable<PendingClaimRepoItem> GetPendingClaims()
        {
            return _context.ClmTransactions.Where(x => x.StatusId == (int)ClaimStatus.Closed
            && x.InspectionNoNavigation.InvAutTranDetails.Any(x => x.InvoiceStatus != (int)DTO.Invoice.InvoiceStatus.Cancelled) && x.ClmTranFinalDecisions.Any(y => y.FinalDecision == 1)
            && (!x.InvCreTranClaimDetails.Any() || !x.InvCreTranClaimDetails.Any(y => y.Active == true)))
                .Select(z => new PendingClaimRepoItem()
                {
                    ClaimId = z.Id,
                    CustomerId = z.InspectionNoNavigation.CustomerId,
                    InspectionId = z.InspectionNo,
                    Customer = z.InspectionNoNavigation.Customer.CustomerName,
                    ClaimDate = z.ClaimDate,
                    ServiceDate = z.InspectionNoNavigation.ServiceDateFrom,
                    Office = z.InspectionNoNavigation.Office.LocationName,
                    ProductCategory = z.InspectionNoNavigation.ProductCategory.Name,
                    ProductSubCategory = z.InspectionNoNavigation.ProductSubCategory.Name,
                    OfficeId = z.InspectionNoNavigation.OfficeId,
                    ClaimNo = z.ClaimNo
                });
        }

        public IQueryable<CreditNoteSummaryRepoItem> GetCreditNotes()
        {
            return _context.InvCreTransactions.Where(x => x.Active == true).Select(x => new CreditNoteSummaryRepoItem()
            {
                BillTo = x.BillTo,
                PostDate = x.PostDate,
                CreatedBy = x.CreatedByNavigation.FullName,
                CreatedOn = x.CreatedOn,
                CreditDate = x.CreditDate,
                CreditNo = x.CreditNo,
                CreditType = x.CreditType.Name,
                CreditTypeId = x.CreditTypeId,
                Currency = x.CurrencyNavigation.CurrencyName,
                Id = x.Id
            });
        }

        public IQueryable<ExportCreditNoteSummaryRepoItem> GetExportCreditNotes()
        {
            return _context.InvCreTranClaimDetails.Where(x => x.Active == true).Select(x => new ExportCreditNoteSummaryRepoItem()
            {
                BillTo = x.Credit.BillTo,
                ClaimNo = x.Claim.ClaimNo,
                InspectionNo = x.Inspection.CustomerBookingNo,
                InvoiceNo = x.Invoice.InvoiceNo,
                RefundAmount = x.RefundAmount,
                SortAmount = x.SortAmount,
                Remark = x.Remarks,
                InspectionFee = x.Claim.RealInspectionFees,
                InspectionFeeCurrency = x.Claim.RealInspectionFeesCurrencyNavigation.CurrencyName,
                CreatedBy = x.Credit.CreatedByNavigation.FullName,
                CreatedOn = x.Credit.CreatedOn,
                CreditDate = x.Credit.CreditDate,
                CreditNo = x.Credit.CreditNo,
                CreditType = x.Credit.CreditType.Name,
                CreditTypeId = x.Credit.CreditTypeId,
                Currency = x.Credit.CurrencyNavigation.CurrencyName,
                Id = x.Id,
                ServiceFromDate = x.Inspection.ServiceDateFrom,
                Bank = x.Credit.Bank.BankName,
                Category = x.Inspection.ProductCategory.Name,
                SubCategory = x.Inspection.ProductSubCategory.Name,
                Office = x.Credit.OfficeNavigation.LocationName,
                PaymentDuration = x.Credit.PaymentDuration,
                PaymentTerms = x.Credit.PaymentTerms,
                PostDate = x.Credit.PostDate,
                ServiceToDate = x.Inspection.ServiceDateTo
            });
        }



        public async Task<List<CommonDataSource>> GetCreditTypeList()
        {
            return await _context.InvCreRefCreditTypes.Where(x => x.Active.HasValue && x.Active.Value).OrderBy(y => y.Sort).Select(z => new CommonDataSource()
            {
                Id = z.Id,
                Name = z.Name
            }).AsNoTracking().ToListAsync();
        }

        public async Task<bool> IsCreditNoExist(string creditNo)
        {
            var lowerCreditNo = creditNo.ToLower();
            return await _context.InvCreTransactions.AnyAsync(x => x.CreditNo.ToLower().Contains(lowerCreditNo));
        }

        public async Task<List<CreditNoteClaimRepoItem>> GetCreditClaimDetailsByCreditIds(IEnumerable<int?> creditIds)
        {
            return await _context.InvCreTranClaimDetails.Where(x => x.Active.HasValue && x.Active.Value && creditIds.Contains(x.CreditId))
                .Select(y => new CreditNoteClaimRepoItem()
                {
                    Id = y.Id,
                    InspectionId = y.InspectionId.Value,
                    ClaimId = y.ClaimId.Value,
                    ClaimNo = y.Claim.ClaimNo,
                    InspectionDate = y.Inspection.ServiceDateFrom,
                    InspectionFee = y.Invoice.InspectionFees,
                    InvoiceNo = y.Invoice.InvoiceNo,
                    InvoiceId = y.InvoiceId.Value,
                    ProductCategory = y.Inspection.ProductCategory.Name,
                    ProductSubCategory = y.Inspection.ProductSubCategory.Name,
                    Office = y.Inspection.Office.LocationName,
                    RefundAmount = y.RefundAmount,
                    Remarks = y.Remarks,
                    SortAmount = y.SortAmount
                }).AsNoTracking().ToListAsync();
        }

        public async Task<List<InvCreTranClaimDetail>> GetCreditTranClaimDetails(IEnumerable<int> creditIds)
        {
            return await _context.InvCreTranClaimDetails.Where(x => x.Active.HasValue && x.Active.Value && creditIds.Contains(x.CreditId.Value)).AsNoTracking().ToListAsync();
        }

        public async Task<List<CreditNoteDetailsRepoItem>> GetCreditNoteTransactionDetails(IEnumerable<int> creditIds)
        {
            return await _context.InvCreTranClaimDetails.Where(x => x.Active.HasValue && x.Active.Value && creditIds.Contains(x.CreditId.Value))
                .Select(y => new CreditNoteDetailsRepoItem()
                {
                    InspectionId = y.InspectionId,
                    CreditId = y.CreditId,
                    CustomerId = y.Inspection.CustomerId,
                    RefundAmount = y.RefundAmount,
                    SortAmount = y.SortAmount
                }).AsNoTracking().ToListAsync();
        }
        public async Task<List<InvCreTranContact>> GetCreditContacts(IEnumerable<int?> creditIds)
        {
            return await _context.InvCreTranContacts.Where(x => x.Active.HasValue && x.Active.Value && creditIds.Contains(x.Credited)).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<ClmTranReport>> GetClaimReportList(List<int> claimIds)
        {
            return await _context.ClmTranReports.Where(x => claimIds.Contains(x.ClaimId.GetValueOrDefault())).AsNoTracking().ToListAsync();
        }
        /// <summary>
        /// get credit note details for preview
        /// </summary>
        /// <param name="creditNo"></param>
        /// <returns></returns>
        public async Task<List<InvoiceDetailsRepo>> GetCreditNoteDetails(string creditNo)
        {
            return await _context.InvCreTranClaimDetails.Where(x => x.Active.HasValue && x.Active.Value && x.Credit.CreditNo == creditNo)
                .Select(y => new InvoiceDetailsRepo()
                {
                    Id = y.Invoice.Id,
                    CreditDate = y.Credit.CreditDate,
                    CreditNumber = y.Credit.CreditNo,
                    InvoiceDate = y.Invoice.InvoiceDate,
                    InvoiceNumber = y.Invoice.InvoiceNo,
                    PostDate = y.Credit.PostDate,
                    Currency = y.Credit.CurrencyNavigation.CurrencyName,
                    BilledName = y.Credit.BillTo,
                    BilledAddress = y.Credit.BilledAddress,
                    PaymentTerm = y.Credit.PaymentTerms,
                    PaymentDuration = y.Credit.PaymentDuration.GetValueOrDefault().ToString(),
                    OfficeName = y.Credit.OfficeNavigation.LocationName,
                    OfficeAddress = y.Credit.OfficeNavigation.Address,
                    OfficeFax = y.Credit.OfficeNavigation.Fax,
                    OfficeMail = y.Credit.OfficeNavigation.Email,
                    InspectionId = y.InspectionId,
                    InspFee = y.Invoice.InspectionFees,
                    AccountId = y.Credit.Bank.Id,
                    AccountName = y.Credit.Bank.AccountName,
                    AccountNumber = y.Credit.Bank.AccountNumber,
                    BankName = y.Credit.Bank.BankName,
                    BankAddress = y.Credit.Bank.BankAddress,
                    BankSwiftCode = y.Credit.Bank.SwiftCode,
                    CreditRefundAmount = y.RefundAmount,
                    CreditSortAmount = y.SortAmount,
                    CreditRemarks = y.Remarks,
                    Subject = y.Credit.Subject,
                    ServiceId = y.Invoice.ServiceId,
                    CreatedOn = y.CreatedOn
                }).AsNoTracking().ToListAsync();
        }

        public async Task<List<InvoiceCreditDetails>> GetInvoiceCreditDetailsByBookingIds(IQueryable<int> bookingIds)
        {
            return await _context.InvCreTranClaimDetails.Where(x => bookingIds.Contains(x.InspectionId.GetValueOrDefault())
                                && x.Active.Value).Select(x => new InvoiceCreditDetails()
                                {
                                    CreditNumber = x.Credit.CreditNo,
                                    BookingId = x.InspectionId
                                }).AsNoTracking().ToListAsync();
        }
    }
}
