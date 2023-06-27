
using Contracts.Repositories;
using DTO.CommonClass;
using DTO.OtherManday;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class OtherMandayRepository: Repository, IOtherMandayRepository
    {
        public OtherMandayRepository(API_DBContext context) : base(context)
        {
        }
        public async Task<bool> CheckIfOtherMandayAlreadyExists(SaveOtherMandayRequest request)
        {
            return await _context.OmDetails.AnyAsync(x => x.Active.Value && x.OfficeCountryId == request.OfficeCountryId && x.QcId == request.QcId &&
                x.OperationalCountryId == request.OperationalCountryId && x.PurposeId == request.PurposeId && x.ServiceDate == request.ServiceDate.ToDateTime());
        }
        public async Task<OmDetail> GetOtherMandayDataById(int id)
        {
            return await _context.OmDetails.Where(x => x.Active.Value && x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<OtherMandayDataRepo> GetOtherMandayEditDataById(int id)
        {
            return await _context.OmDetails.Where(x => x.Active.Value && x.Id == id)
                .Select(x => new OtherMandayDataRepo
                {
                    Id = x.Id,
                    CustomerId = x.CustomerId,
                    OfficeCountryId = x.OfficeCountryId,
                    PurposeId = x.PurposeId,
                    QcId = x.QcId,
                    OperationalCountryId = x.OperationalCountryId,
                    ServiceDate = x.ServiceDate,
                    Remarks = x.Remarks,
                    Manday = x.ManDay
                }).AsNoTracking().FirstOrDefaultAsync();
        }
        public IQueryable<OtherMandayDataRepo> GetOtherMandayByEfCore()
        {
            return _context.OmDetails.Where(x => x.Active.Value)
                .Select(x => new OtherMandayDataRepo
                {
                    Id = x.Id,
                    CustomerId = x.CustomerId,
                    CustomerName = x.Customer.CustomerName,
                    OfficeCountryId = x.OfficeCountryId,
                    OfficeCountryName = x.OfficeCountry.CountryName,
                    PurposeId = x.PurposeId,
                    Purpose = x.Purpose.Name,
                    QcId = x.QcId,
                    QcName = x.Qc.PersonName,
                    OperationalCountryId = x.OperationalCountryId,
                    OperationalCountryName = x.OperationalCountry.CountryName,
                    ServiceDate = x.ServiceDate,
                    Manday = x.ManDay,
                    Remarks = x.Remarks,
                    OfficeName = x.Qc.Location.LocationName,
                    CreatedBy = x.CreatedByNavigation.FullName,
                    CreatedOn = x.CreatedOn
                });
        }

        public async Task<List<CommonDataSource>> GetPurposeList()
        {
            return await _context.OmRefPurposes.Where(x => x.Active)
                .Select(x => new CommonDataSource
                {
                    Id = x.Id,
                    Name = x.Name
                }).AsNoTracking().ToListAsync();
        }
    }
}
