using Contracts.Repositories;
using DTO.AuditReport;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Enums;

namespace DAL.Repositories
{
    public class AuditCusReportRepository : Repository, IAuditCusReportRepository
    {
        public AuditCusReportRepository(API_DBContext context) : base(context)
        {
        }

        public async Task<List<AuditServiceType>> GetAuditserviceType(List<int> lstbookId)
        {
            return await _context.AudTranServiceTypes.
                 Join(_context.RefServiceTypes,
                 audserv => audserv.ServiceTypeId,
                 refservice => refservice.Id,
                 (audserv, refservice) => new AuditServiceType
                 {
                     bookingid = audserv.AuditId,
                     ServiceType = refservice.Name,
                     ServiceTypeId = audserv.ServiceTypeId,
                     Active = audserv.Active
                 }).Where(x => x.Active && lstbookId.Contains(x.bookingid)).AsNoTracking().ToListAsync();
        }

        public async Task<List<AuditcusReport>> IsAuditReportExists(List<int> lstbookId)
        {
            var auditreportlst = new List<AuditcusReport>();
            var auditreportfromdb = await (from trans in _context.AudTransactions
                                           join report in _context.AudTranReports1 on trans.Id equals report.AuditId
                                           where (report.Active && lstbookId.Contains(report.AuditId))
                                           select new AuditcusReport
                                           { AuditId = report.AuditId, ReportId = report.Id, filename = report.FileName }).AsNoTracking().ToListAsync();

            auditreportlst.AddRange(auditreportfromdb);

            var auditfromcloud = await _context.AudTranReports.Where(x => x.Active && lstbookId.Contains(x.AuditId))
                .Select(x => new AuditcusReport
                {
                    AuditId = x.AuditId,
                    filename = x.FileName,
                    ReportId = x.Id,
                    fileUrl = x.FileUrl,
                    ReportFileUniqueId = x.UniqueId,
                    FbReportid = x.Audit.FbreportId
                }).AsNoTracking().ToListAsync();

            auditreportlst.AddRange(auditfromcloud);

            if(!auditreportlst.Any())
            {
                await _context.AudTransactions.Where(x => lstbookId.Contains(x.Id))
                     .Select(x => new AuditcusReport
                     {
                         AuditId = x.Id,
                         FbReportid=x.FbreportId
                     }).AsNoTracking().ToListAsync();
            }
            return auditreportlst;
        }

        public IQueryable<AuditRepoCusReportBookingDetails> SearchAuditCusReport()
        {
            return _context.AudTransactions
                  .Select(x => new AuditRepoCusReportBookingDetails
                  {
                      AuditId = x.Id,
                      Customer = x.Customer.CustomerName,
                      Factory = x.Factory.SupplierName,
                      officeName = x.Office.LocationName,
                      ReportNo = x.ReportNo,
                      CustomerBookingNo = x.CustomerBookingNo,
                      ServiceFromDate = x.ServiceDateFrom,
                      ServiceToDate = x.ServiceDateTo,
                      Supplier = x.Supplier.SupplierName,
                      CustomerId = x.CustomerId,
                      FactoryId = x.FactoryId,
                      SupplierId = x.SupplierId,
                      officeId = x.OfficeId,
                      StatusId = x.StatusId,
                      StatusName = x.Status.Status,
                      CreatedOn = x.CreatedOn,
                      ReportId = x.FbreportId,
                      ReportUrl = x.FinalReportPath
                  }).OrderByDescending(x => x.ServiceToDate);
        }

        /// <summary>
        /// get factory country details by audit ids
        /// </summary>
        /// <param name="auditIds"></param>
        /// <returns></returns>
        public async Task<List<AuditFactoryCountry>> GetFactorycountryByAuditIds(IEnumerable<int> auditIds)
        {
            return await (from fact in _context.SuAddresses
                          join aud in _context.AudTransactions on fact.SupplierId equals aud.FactoryId
                          where auditIds.Contains(aud.Id) && fact.AddressTypeId == (int)Supplier_Address_Type.HeadOffice
                          select new AuditFactoryCountry
                          {
                              FactoryCountryId = fact.CountryId,
                              AuditId = aud.Id,
                              CountryName = fact.Country.CountryName
                          }).AsNoTracking().ToListAsync();
        }
    }
}
