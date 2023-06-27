using Contracts.Repositories;
using DTO.CommonClass;
using DTO.TravelTariff;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DTO.ScheduleJob;
using Entities.Enums;
using DTO.Common;

namespace DAL.Repositories
{
    public class ScheduleJobRepository : Repository, IScheduleJobRepository
    {
        public ScheduleJobRepository(API_DBContext context) : base(context)
        {
        }

        public IQueryable<InspProductTransaction> GetCulturaScheduleJobData(int customerid)
        {
            return _context.InspProductTransactions.Where(x => x.Active.Value && x.FbReportId > 0 && x.Inspection.CustomerId == customerid);
        }
        public IQueryable<InspProductTransaction> GetScheduleJobData()
        {
            return _context.InspProductTransactions.Where(x => x.Active.Value && x.FbReportId > 0);
        }

        public async Task<List<StartPortCity>> GetCityIdByStartPortList(List<int?> startPortIds)
        {
            return await _context.EcAutRefStartPorts
                .Where(x => startPortIds.Contains(x.Id))
                .Select(x => new StartPortCity()
                {
                    StartPortId = x.Id,
                    CityId = x.CityId
                }).ToListAsync();
        }

        public async Task<List<FactoryTownCity>> GetCityIdByTownIdList(List<int?> townIds)
        {
            return await _context.RefTowns
                .Where(x => townIds.Contains(x.Id))
                .Select(x => new FactoryTownCity()
                {
                    TownId = x.Id,
                    CityId = x.County.CityId
                }).ToListAsync();
        }

        public async Task<List<EcAutQcTravelExpense>> GetAutoQcTravelExpenseData(List<int> travelExpenseIds)
        {
            if (travelExpenseIds.Any())
            {

                return await _context.EcAutQcTravelExpenses.
                            Include(x => x.Qc).
                       Where(x => travelExpenseIds.Contains(x.Id) && x.Active.Value
                       && !x.IsExpenseCreated.Value
                       )
                       .ToListAsync();

            }

            // get last month data 
            var today = DateTime.Today;
            var monthStart = new DateTime(today.Year, today.Month, 1);
            var lastMonthStart = monthStart.AddMonths(-1);
            var lastMonthEnd = monthStart.AddDays(-1);

            return await _context.EcAutQcTravelExpenses.
                   Include(x => x.Qc).
                   Where(x => x.Active.Value
                   && !x.IsExpenseCreated.Value && x.ServiceDate.Value.Date >= lastMonthStart.Date &&
                   x.ServiceDate.Value.Date <= lastMonthEnd.Date
                   )
                   .ToListAsync();
        }

        public async Task<List<EcAutQcFoodExpense>> GetAutoQcFoodExpenseData(List<int> foodExpenseIds)
        {
            if (foodExpenseIds.Any())
            {
                return await _context.EcAutQcFoodExpenses.
                       Include(x => x.Qc).
                       Where(x => foodExpenseIds.Contains(x.Id) && x.Active.Value
                       && !x.IsExpenseCreated.Value
                       ).ToListAsync();
            }

            // get last month data 
            var today = DateTime.Today;
            var monthStart = new DateTime(today.Year, today.Month, 1);
            var lastMonthStart = monthStart.AddMonths(-1);
            var lastMonthEnd = monthStart.AddDays(-1);

            return await _context.EcAutQcFoodExpenses.
                  Include(x => x.Qc).
                   Where(x => x.Active.Value &&
                   x.ServiceDate.Value.Date >= lastMonthStart.Date &&
                    x.ServiceDate.Value.Date <= lastMonthEnd.Date
                   && !x.IsExpenseCreated.Value).ToListAsync();
        }

        public IQueryable<EcAutQcTravelExpense> GetQcTravelExpenseData()
        {
            return _context.EcAutQcTravelExpenses.Where(x => x.Active.Value);
        }

        public IQueryable<EcAutQcFoodExpense> GetQcFoodExpenseData()
        {
            return _context.EcAutQcFoodExpenses.Where(x => x.Active.Value);
        }

        public async Task<List<ScheduleTravelTariffEmail>> GetInActivatTravelTariffList()
        {
            return await _context.EcAutTravelTariffs.
                          Where(x => x.Active.Value && !x.Status.Value)
                          .Select(x => new ScheduleTravelTariffEmail()
                          {
                              Id = x.Id,
                              StartPortName = x.StartPortNavigation.StartPortName,
                              FactoryTown = x.Town.TownName
                          })
                          .ToListAsync();
        }

        public async Task<List<JobConfiguration>> GetScheduleJobConfigurationList()
        {
            return await _context.JobScheduleConfigurations.
                          Where(x => x.Active.Value).
                          Select(x => new JobConfiguration()
                          {
                              Id = x.Id,
                              Name = x.Name,
                              FileName = x.FileName,
                              StartDate = x.StartDate,
                              ScheduleInterval = x.ScheduleInterval,
                              Cc = x.Cc,
                              To = x.To,
                              FolderPath = x.FolderPath,
                              Type = x.Type,
                              CustomerIds = x.CustomerId
                          }).AsNoTracking().ToListAsync();
        }

        public async Task<List<JobConfiguration>> GetScheduleJobConfigurations(List<int> typeIds)
        {
            return await _context.JobScheduleConfigurations.
                          Where(x => x.Active.Value && typeIds.Contains(x.Type.GetValueOrDefault())).
                          Select(x => new JobConfiguration()
                          {
                              Id = x.Id,
                              Name = x.Name,
                              FileName = x.FileName,
                              StartDate = x.StartDate,
                              ScheduleInterval = x.ScheduleInterval,
                              Cc = x.Cc,
                              To = x.To,
                              FolderPath = x.FolderPath,
                              Type = x.Type
                          }).AsNoTracking().ToListAsync();
        }

        public async Task<List<ScheduleClaimReminderEmail>> GetClaimReminderList(IQueryable<ClmTransaction> claimQuery)
        {
            return await claimQuery.Select(x => new ScheduleClaimReminderEmail
            {
                ClaimId = x.Id,
                ClaimNo = x.ClaimNo,
                ClaimDate = x.ClaimDate,
                BookingId = x.InspectionNo,
                CustomerId = x.InspectionNoNavigation.CustomerId,
                SupplierId = x.InspectionNoNavigation.SupplierId,
                CustomerName = x.InspectionNoNavigation.Customer.CustomerName,
                SupplierName = x.InspectionNoNavigation.Supplier.SupplierName,
                FactoryName = x.InspectionNoNavigation.Factory.SupplierName,
                ServiceDateFrom = x.InspectionNoNavigation.ServiceDateFrom,
                ServiceDateTo = x.InspectionNoNavigation.ServiceDateTo,
                StatusName = x.Status.Name,
                Office = x.InspectionNoNavigation.Office.LocationName,
                OfficeId = x.InspectionNoNavigation.OfficeId,
                StatusId = x.StatusId,
                CreatedBy = x.CreatedBy
            }).AsNoTracking().ToListAsync();
        }

        public async Task<List<ProductDetail>> GetProductTransactionList(List<int> bookingIds)
        {
            return await _context.InspProductTransactions.Where(x => x.Active.Value &&
                            bookingIds.Contains(x.InspectionId) && !x.Product.CuProductFileAttachments.Any(x => x.FileTypeId != 1)).
                Select(x => new ProductDetail()
                {
                    InspectionId = x.InspectionId,
                    ProductId = x.Id,
                    ProductRef = x.Product.ProductId,
                    ProductName = x.Product.ProductCategorySub2Navigation.Name
                }).AsNoTracking().ToListAsync();
        }
        public async Task<List<CustomerDetail>> GetCustomerBookingDetails(List<int> customerIds)
        {
            return await _context.CuCustomers.Where(x => x.Active.Value == true && customerIds.Contains(x.Id)).
               Select(x => new CustomerDetail()
               {
                   CustomerId = x.Id,
                   CustomerName = x.CustomerName,
                   CustomerEmail = x.Email
               }).ToListAsync();
        }

        public async Task<JobConfiguration> GetScheduleJobConfiguration(int configureId)
        {
            return await _context.JobScheduleConfigurations.
              Where(x => x.Active.Value && x.Id == configureId).
              Select(x => new JobConfiguration()
              {
                  Id = x.Id,
                  Name = x.Name,
                  FileName = x.FileName,
                  StartDate = x.StartDate,
                  ScheduleInterval = x.ScheduleInterval,
                  Cc = x.Cc,
                  To = x.To,
                  FolderPath = x.FolderPath,
                  Type = x.Type
              }).AsNoTracking().FirstOrDefaultAsync();
        }
    }
}
