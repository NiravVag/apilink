using Contracts.Repositories;
using DTO.Dashboard;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class SupFactDashboardRepository : Repository, ISupFactDashboardRepository
    {
        public SupFactDashboardRepository(API_DBContext context) : base(context)
        {
        }
        /// <summary>
        /// Get the product count for the inspectionids
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <returns></returns>
        public async Task<List<FactoryGeoCode>> GetInspFactoryGeoCode(IEnumerable<int> InspIdList)
        {
            return await (from insp in _context.InspTransactions join
                           fact in _context.SuSuppliers on insp.FactoryId equals fact.Id
                          join sua in _context.SuAddresses on fact.Id equals sua.SupplierId
                          where (InspIdList.Contains(insp.Id) && insp.StatusId != (int)BookingStatus.Cancel && fact.TypeId == (int)Supplier_Type.Factory &&
                          sua.AddressTypeId == (int)Supplier_Address_Type.HeadOffice)
                          select new FactoryGeoCode
                          {
                              FactoryLatitude = sua.Latitude,
                              FactoryLongitude = sua.Longitude,
                              FactoryId = fact.Id,
                              FactoryName = fact.SupplierName
                          }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get customer details by booking ids
        /// </summary>
        /// <param name="InspIdList"></param>
        /// <returns></returns>
        public async Task<List<CustomerBookingModel>> GetCusBookingDetails(IEnumerable<int> InspIdList)
        {
            return await _context.InspTransactions.Where(x => x.StatusId != (int)BookingStatus.Cancel && InspIdList.Contains(x.Id)).Select(x => 
                    new CustomerBookingModel
            {
                BookingId = x.Id,
                CustomerName = x.Customer.CustomerName
            }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get booking details
        /// </summary>
        /// <param name="InspIdList"></param>
        /// <returns></returns>
        public async Task<List<BookingDetailsRepo>> GetBookingDetails(IEnumerable<int> InspIdList)
        {
            var cancelStatus = (int)BookingStatus.Cancel;
            return await _context.InspTransactions.Where(x => x.StatusId != cancelStatus && InspIdList.Contains(x.Id))
                .OrderByDescending(x => x.CreatedOn)
                .Select(x => new BookingDetailsRepo
                {
                    BookingId = x.Id,
                    CustomerName = x.Customer.CustomerName,
                    SupplierName = x.Supplier.SupplierName,
                    FactoryName = x.Factory.SupplierName,
                    CreatedBy = x.CreatedBy,
                    ServiceFromDate = x.ServiceDateFrom,
                    ServiceToDate = x.ServiceDateTo,
                    CountryName = x.Factory.SuAddresses.Select(z => z.Country.CountryName).FirstOrDefault()
                }).AsNoTracking().ToListAsync();
        }
    }
}
