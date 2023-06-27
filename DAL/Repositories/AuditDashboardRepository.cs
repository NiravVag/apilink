using Contracts.Repositories;
using DTO.AuditDashboard;
using DTO.Common;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class AuditDashboardRepository : Repository, IAuditDashboardRepository
    {
        public AuditDashboardRepository(API_DBContext context) : base(context)
        {
        }

        public async Task<List<AuditCountryGeoCode>> GetAuditCountryGeoCode(IEnumerable<int> auditIdlst)
        {
            return await(from audit in _context.AudTransactions
                         join fact in _context.SuSuppliers on audit.FactoryId equals fact.Id
                         join sua in _context.SuAddresses on fact.Id equals sua.SupplierId
                         join con in _context.RefCountries on sua.CountryId equals con.Id
                         join reg in _context.RefProvinces on sua.RegionId equals reg.Id
                         where auditIdlst.Contains(audit.Id) && sua.AddressTypeId == (int)Supplier_Address_Type.HeadOffice
                         && fact.Active && con.Active && reg.Active.Value
                         select new AuditCountryGeoCode
                         {
                             FactoryCountryName = con.CountryName,
                             FactoryCountryCode = con.Alpha2Code,
                             FactoryCountryId = sua.CountryId,
                             Latitude = con.Latitude,
                             Longitude = con.Longitude,
                             FactoryProvinceName = reg.ProvinceName,
                             FactoryProvinceId = reg.Id,
                             ProvinceLatitude = reg.Latitude,
                             ProvinceLongitude = reg.Longitude,
                             FactoryLatitude = sua.Latitude,
                             FactoryLongitude = sua.Longitude,
                             FactoryId = fact.Id,
                             FactoryName = fact.SupplierName
                         }).AsNoTracking().ToListAsync();
        }

        public async Task<List<AuditChartData>> GetAuditServiceTypeByQuery(IQueryable<int> auditIds)
        {
            return await _context.AudTranServiceTypes
                  .Where(x => auditIds.Contains(x.AuditId) && x.Active)
                  .Select(x => new
                  {
                      x.AuditId,
                      x.ServiceTypeId,
                      x.ServiceType.Name
                  }).GroupBy(p => new { p.ServiceTypeId, p.Name }, (key, _data) => new AuditChartData
                  {
                      Id = key.ServiceTypeId,
                      Name = key.Name,
                      Count = _data.Count(),
                  }).AsNoTracking().ToListAsync();
        }
    }
}
