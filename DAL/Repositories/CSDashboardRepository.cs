using Contracts.Repositories;
using DAL.Helper;
using DTO.Common;
using DTO.Dashboard;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DTO.Common.Static_Data_Common;

namespace DAL.Repositories
{
    public class CSDashboardRepository : Repository, ICSDashboardRepository
    {
        private readonly IConfiguration _configuration = null;

        public CSDashboardRepository(API_DBContext context, IConfiguration configuration) : base(context)
        {
            _configuration = configuration;
        }

        /// <summary>
        ///  get booking related count - product, customer, supplier, factory, PO, booking
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<CSDashboardCountItem>> GetCountNewDetails(CSDashboardFilterModel request)
        {
            var dataSet = await ADOHelper.GetLinkDataSource(_configuration, DbConnectionName, SP_CSDashboard_GetNewDetails, request);

            var list = await ADOHelper.ConvertDataTable<CSDashboardCountItem>(dataSet.Tables[0]);

            return list;
        }

        /// <summary>
        /// get service type list
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<CSDashboardItem>> GetServiceTypeList(CSDashboardDBRequest request)
        {
            var dataSet = await ADOHelper.GetLinkDataSource(_configuration, DbConnectionName, SP_CSDashboard_GetServiceType, request);

            var list = await ADOHelper.ConvertDataTable<CSDashboardItem>(dataSet.Tables[0]);

            return list;
        }

        /// <summary>
        /// get man day count by office list
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<CSDashboardItem>> GetMandayByOfficeList(CSDashboardDBRequest request)
        {
            var dataSet = await ADOHelper.GetLinkDataSource(_configuration, DbConnectionName, SP_CSDashboard_GetMandayByOffice, request);

            var list = await ADOHelper.ConvertDataTable<CSDashboardItem>(dataSet.Tables[0]);

            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<DayReportCountRepo>> GetDayFBReportCountList(CSDashboardDBRequest request)
        {
            var dataSet = await ADOHelper.GetLinkDataSource(_configuration, DbConnectionName, SP_CSDashboard_GetReports, request);

            var list = await ADOHelper.ConvertDataTable<DayReportCountRepo>(dataSet.Tables[0]);

            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<StatusTaskCountItemRepo>> GetStatusTaskCountList(CSDashboardStatusDBRequest request)
        {
            var dataSet = await ADOHelper.GetLinkDataSource(_configuration, DbConnectionName, SP_CSDashboard_GetModuleStatus, request);

            var response = await ADOHelper.ConvertDataTable<StatusTaskCountItemRepo>(dataSet.Tables[0]);

            return response;
        }

        /// <summary>
        /// Get the booking detail by customerdashboard filter request
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Base booking detail</returns>
        public async Task<List<BookingDetail>> GetBookingDetail(CSDashboardModelRequest request, IAPIUserContext applicationContext)
        {
            IQueryable<InspTransaction> inspTransactions = _context.InspTransactions;

            if (request != null)
            {
                //filter based on request status id list
                if (request.StatusIdList != null && request.StatusIdList.Any())
                {
                    inspTransactions = inspTransactions.Where(x => request.StatusIdList.Contains(x.StatusId));
                }
                // filter based on status - inspected and validated
                else
                {
                    inspTransactions = inspTransactions.Where(x => x.StatusId != (int)BookingStatus.Cancel);
                    inspTransactions = inspTransactions.Where(x => InspectedStatusList.Contains(x.StatusId));
                }

                if (request.CustomerId != null && request.CustomerId > 0)
                {
                    inspTransactions = inspTransactions.Where(x => x.CustomerId == request.CustomerId);
                }

                if (request.SupplierId != null && request.SupplierId > 0)
                {
                    inspTransactions = inspTransactions.Where(x => x.SupplierId == request.SupplierId);
                }

                if (request.FactoryIdList != null && request.FactoryIdList.Any())
                {
                    inspTransactions = inspTransactions.Where(x => x.FactoryId>0 && request.FactoryIdList.Contains(x.FactoryId.GetValueOrDefault()));
                }

                if (request.ServiceDateFrom != null && request.ServiceDateTo != null)
                {
                    inspTransactions = inspTransactions.Where(x => x.ServiceDateTo <= request.ServiceDateTo.ToDateTime() && x.ServiceDateTo >= request.ServiceDateFrom.ToDateTime());
                }

                if (request.BrandIdList != null && request.BrandIdList.Any())
                {
                    inspTransactions = inspTransactions.Where(x => x.InspTranCuBrands.Any(y => request.BrandIdList.Contains(y.BrandId)));
                }

                if (request.BuyerIdList != null && request.BuyerIdList.Any())
                {
                    inspTransactions = inspTransactions.Where(x => x.InspTranCuBuyers.Any(y => request.BuyerIdList.Contains(y.BuyerId)));
                }

                if (request.DeptIdList != null && request.DeptIdList.Any())
                {
                    inspTransactions = inspTransactions.Where(x => x.InspTranCuDepartments.Any(y => request.DeptIdList.Contains(y.DepartmentId)));
                }

                if (request.OfficeIdList != null && request.OfficeIdList.Any())
                {
                    inspTransactions = inspTransactions.Where(x => request.OfficeIdList.Contains(x.OfficeId.GetValueOrDefault()));
                }

                if (request.ServiceTypeList != null && request.ServiceTypeList.Any())
                {
                    inspTransactions = inspTransactions.Where(x => x.InspTranServiceTypes.Any(y => request.ServiceTypeList.Contains(y.ServiceTypeId)));
                }

                //in the initial run or if office not selected, data is filtered based on logged in user office access
                if (applicationContext.LocationList != null && applicationContext.LocationList.Any())
                {
                    inspTransactions = inspTransactions.Where(x => applicationContext.LocationList.ToList().Contains(x.OfficeId.GetValueOrDefault()));
                }
            }
            return await inspTransactions.
                                    Select(x =>
                                    new BookingDetail
                                    {
                                        InspectionId = x.Id,
                                        CustomerId = x.CustomerId,
                                        SupplierId = x.SupplierId,
                                        FactoryId = x.FactoryId,
                                        CreationDate = x.CreatedOn.Value,
                                        ServiceDateFrom = x.ServiceDateFrom,
                                        ServiceDateTo = x.ServiceDateTo,
                                        StatusId = x.StatusId
                                    }).ToListAsync();
        }
    }
}
