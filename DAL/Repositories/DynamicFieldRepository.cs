using Contracts.Repositories;
using DTO.DynamicFields;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class DynamicFieldRepository : Repository, IDynamicFieldRepository
    {
        public DynamicFieldRepository(API_DBContext context) : base(context)
        {

        }
        /// <summary>
        /// Get the Modules
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RefModule> GetModules()
        {
            return _context.RefModules.Where(x => x.Active)
                    .OrderBy(x => x.Name);
        }
        /// <summary>
        /// Get the controltypes
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DfControlType> GetControlTypes()
        {
            return _context.DfControlTypes.Where(x => x.Active)
                    .OrderBy(x => x.Name);
        }
        /// <summary>
        /// Get the DDL Source Types
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public IEnumerable<DfDdlSourceType> GetDDLSourceTypes(int customerId)
        {
            return _context.DfDdlSourceTypes.Where(x => x.Active && x.DfCuDdlSourceTypes
                    .Any(y => y.Active && y.CustomerId == customerId))
                    .OrderBy(x => x.Name);
        }
        /// <summary>
        /// Get the DDL Source
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public IEnumerable<DfDdlSource> GetDDLSource(int typeId)
        {
            return _context.DfDdlSources.Where(x => x.Active && x.Type == typeId)
                    .OrderBy(x => x.Name);
        }
        /// <summary>
        /// AddDfCustomerConfiguration
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task<int> AddDfCustomerConfiguration(DfCuConfiguration entity)
        {
            _context.DfCuConfigurations.Add(entity);
            return _context.SaveChangesAsync();
        }
        /// <summary>
        /// Get the df customer configuration
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DfCuConfiguration GetDfCustomerConfiguration(int id)
        {
            return _context.DfCuConfigurations.
                    Where(x => x.Active && x.Id == id).
                    Include(x => x.DfControlAttributes).
                    FirstOrDefault();

        }
        /// <summary>
        /// EditDfCuConfiguration
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task<int> EditDfCuConfiguration(DfCuConfiguration entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return _context.SaveChangesAsync();
        }
        /// <summary>
        /// Get Dynamic Field Customer Configuration List
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<DfCustomerConfigSearchData>> GetDfCustomerConfigData()
        {
            return await _context.DfCuConfigurations.
                    Where(x => x.Active).
                    Select(x =>
                    new DfCustomerConfigSearchData
                    {
                        id = x.Id,
                        ModuleId = x.ModuleId,
                        ModuleName = x.Module.Name,
                        ControlTypeId = x.ControlTypeId,
                        ControlTypeName = x.ControlType.Name,
                        CustomerId = x.CustomerId,
                        CustomerName = x.Customer.CustomerName,
                        Label = x.Label,
                        DisplayOrder = x.DisplayOrder
                    }).ToListAsync();

        }
        /// <summary>
        /// GetDfCustomerConfiguration
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public IQueryable<DfCuConfiguration> GetDfCustomerConfiguration(int customerId, int moduleId)
        {
            return _context.DfCuConfigurations.
                    Where(x => x.Active && x.CustomerId == customerId && x.ModuleId == moduleId)
                    .Include(x => x.DfControlAttributes)
                    .Include(x => x.DataSourceTypeNavigation)
                    .ThenInclude(x => x.DfDdlSources)
                    .Include(x => x.DfControlAttributes)
                    .ThenInclude(x => x.ControlAttribute);
            //.ThenInclude(x=>x.;

        }
        /// <summary>
        /// GetDDLControlTypeAttributes
        /// </summary>
        /// <param name="controlTypeId"></param>
        /// <returns></returns>
        public IEnumerable<DfControlTypeAttribute> GetDFControlTypeAttributes(int controlTypeId)
        {
            return _context.DfControlTypeAttributes.
                    Where(x => x.Active && x.ControlTypeId == controlTypeId)
                    .Include(x => x.Attribute)
                    .OrderBy(x => x.Id);
        }

        public async Task<List<DfParentDDLSource>> GetParentDropDownTypes(List<int?> dataSourceIds)
        {
            return await _context.DfDdlSourceTypes.Where(x => dataSourceIds.Contains(x.Id) && x.Active).
                            Select(x => new DfParentDDLSource
                            {
                                Name = x.Name,
                                Id = x.Id
                            }).ToListAsync();
        }

        public async Task<List<int?>> GetParentDropDownIds(int customerId)
        {
            return await _context.DfCuConfigurations.Where(x => x.CustomerId == customerId && x.Active).
                            Select(x => x.DataSourceType).ToListAsync();
        }


        public async Task<DfCustomerConfigBaseData> GetDfCustomerConfigBaseData(int id)
        {
            return await _context.DfCuConfigurations.
                    Where(x => x.Active && x.Id == id).
                    Select(x =>
                    new DfCustomerConfigBaseData
                    {
                        Id = x.Id,
                        CustomerId = x.CustomerId,
                        ModuleId = x.ModuleId,
                        ControlTypeId = x.ControlTypeId,
                        Label = x.Label,
                        Type = x.Type,
                        FbReference = x.Fbreference,
                        DataSourceType = x.DataSourceType,
                        DisplayOrder = x.DisplayOrder
                    }).FirstOrDefaultAsync();

        }

        public async Task<List<EditDfControlAttributes>> GetDfCustomerConfigAttributes(int id)
        {
            return await _context.DfControlAttributes.
                    Where(x => x.Active && x.ControlConfigurationId == id).
                    Select(x =>
                    new EditDfControlAttributes
                    {
                        Id = x.Id,
                        Name = x.ControlAttribute.Attribute.Name,
                        DataType = x.ControlAttribute.Attribute.DataType,
                        Value = x.Value,
                        AttributeId = x.ControlAttribute.AttributeId,
                        ControlAttributeId = x.ControlAttributeId,
                        ControlTypeId = x.ControlAttribute.ControlTypeId
                    }).ToListAsync();

        }


        public async Task<bool> RemoveDFCustomerConfiguration(int id)
        {
            var entity = await _context.DfCuConfigurations.FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
                return false;

            entity.Active = false;

            _context.Entry(entity).State = EntityState.Modified;
            int numReturn = await _context.SaveChangesAsync();
            return numReturn > 0;
        }

        public async Task<bool> CheckDFCustomerConfigInBooking(int id)
        {
            var entity = await _context.InspDfTransactions.FirstOrDefaultAsync(x => x.ControlConfigurationId == id);
            if (entity != null)
                return true;
            else
                return false;

        }

        public async Task<List<InspectionDFCustomerConfig>> GetInspectionDFCustomerConfig()
        {
            return await _context.InspDfTransactions.Where(x => x.Active).
                                        Select(x => new InspectionDFCustomerConfig
                                        {
                                            Id = x.Id,
                                            BookingId = x.BookingId,
                                            ControlConfigId = x.ControlConfigurationId,
                                            CustomerId = x.Booking.CustomerId
                                        }).ToListAsync();
        }

        public async Task<List<InspectionBookingDFRepo>> GetBookingDFDataByBookingIds(IEnumerable<int> bookingIds)
        {
            return await _context.InspDfTransactions.Where(x => bookingIds.Contains(x.BookingId) && x.Active && x.ControlConfiguration.Active).
                            Select(x => new InspectionBookingDFRepo
                            {
                                DFName = x.ControlConfiguration.Label,
                                BookingNo = x.BookingId,
                                DFValue = x.Value,
                                FbReference = x.ControlConfiguration.Fbreference,
                                ControlType = x.ControlConfiguration.ControlTypeId,
                                DFSourceType = x.ControlConfiguration.DataSourceType,
                                ControlConfigurationId = x.ControlConfigurationId
                            }).ToListAsync();
        }

        public async Task<List<DFDataSourceRepo>> GetDropDownSourceByDataSourceTypeId(IEnumerable<int> dataSourceTypeIds)
        {
            return await _context.DfDdlSources.Where(x => x.Active && dataSourceTypeIds.Contains(x.Type)).
                        Select(x => new DFDataSourceRepo
                        {
                            Id = x.Id,
                            Name = x.Name,
                            DataSourceTypeId = x.Type
                        }).ToListAsync();
        }
        /// <summary>
        /// Get the booking dynamic data by booking query
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<InspectionBookingDFRepo>> GetBookingDFDataByBookings(IQueryable<int> bookingIds)
        {
            return await _context.InspDfTransactions.Where(x => bookingIds.Contains(x.BookingId) && x.Active && x.ControlConfiguration.Active).
                            Select(x => new InspectionBookingDFRepo
                            {
                                DFName = x.ControlConfiguration.Label,
                                BookingNo = x.BookingId,
                                DFValue = x.Value,
                                FbReference = x.ControlConfiguration.Fbreference,
                                ControlType = x.ControlConfiguration.ControlTypeId,
                                DFSourceType = x.ControlConfiguration.DataSourceType,
                                ControlConfigurationId = x.ControlConfigurationId
                            }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get audit product category for the inspection
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="dataSourceTypeId"></param>
        /// <returns></returns>
        public async Task<string> GetBookingAuditProductCategory(int bookingId, int? dataSourceTypeId)
        {
            return await _context.InspDfTransactions.Where(x => x.BookingId == bookingId 
            && x.Active && x.ControlConfiguration.DataSourceType == dataSourceTypeId).
            Select(x => x.Value).FirstOrDefaultAsync();
        }

    }
}
