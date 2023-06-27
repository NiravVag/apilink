using DTO.Common;
using DTO;
using DTO.HumanResource;
using DTO.Location;
using DTO.References;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using DTO.OfficeLocation;

namespace BI.Maps
{
    public  class LocationMap: ApiCommonData
    {
        public  Area GetArea(RefArea entity)
        {
            if (entity == null)
                return null;
            return new Area
            {
                Id = entity.Id,
                AreaName = entity.AreaName
            };
        }
        public  Country GetCountry(RefCountry entity)
        {
            if (entity == null)
                return null;

            return new Country
            {
                Id = entity.Id,
                countrycode = entity.CountryCode,
                CountryName = entity.CountryName,
                Area = entity.Area?.AreaName,
                AreaId = entity.AreaId == null ? 0 : entity.AreaId.Value,
                alphacode = entity.Alpha2Code
            };
        }

        public  Office GetOffice(RefLocation entity)
        {
            if (entity == null)
                return null;

            return new Office
            {
                Id = entity.Id,
                Name = entity.LocationName,
                Type = new OfficeType
                {
                    Id = entity.LocationTypeId,
                    Name = entity.LocationType?.SgtLocationType
                }
            };
        }
        public  Zone GetZone(RefZone entity)
        {
            if (entity == null)
                return null;
            return new Zone
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }

        public  Currency GetCurrency(RefCurrency entity)
        {
            if (entity == null)
                return null;

            return new Currency
            {
                Id = entity.Id,
                CurrencyCode = entity.CurrencyCodeA,
                CurrencyName = entity.CurrencyName
            };
        }

        public  State GetState(RefProvince entity)
        {
            if (entity == null)
                return null;

            return new State
            {
                Id = entity.Id,
                Name = entity.ProvinceName,
                Code = entity.ProvinceCode,
                CountryName = entity.Country.CountryName,
                CountryId = entity.Country.Id
            };
        }
        public  City GetCity(RefCity entity)
        {
            if (entity == null)
                return null;

            return new City
            {
                Id = entity.Id,
                Name = entity.CityName,
                CountryId = entity?.Province?.Country?.Id,
                CountryName = entity?.Province?.Country?.CountryName,
                ProvinceId = entity?.Province?.Id,
                ProvinceName = entity?.Province?.ProvinceName,
                PhoneCode = entity.PhCode

            };
        }
        public  City GetCityDetails(RefCity entity)
        {
            if (entity == null)
                return null;

            var citydetails = entity.RefCityDetails?.FirstOrDefault();
            return new City
            {
                Id = entity.Id,
                Name = entity.CityName,
                CountryId = entity?.Province?.Country?.Id,
                CountryName = entity?.Province?.Country?.CountryName,
                OfficeId = citydetails?.LocationId,
                OfficeName = citydetails?.Location?.LocationName,
                ProvinceId = entity?.Province?.Id,
                ProvinceName = entity?.Province?.ProvinceName,
                TravelTimeHH = citydetails?.TravelTime ?? 0,
                ZoneId = citydetails?.ZoneId,
                ZoneName = citydetails?.Zone?.Name,
                PhoneCode = entity.PhCode

            };
        }

        public  RefCountry MapCountryEntity(Country request)
        {
            if (request == null)
                return null;
            return new RefCountry
            {
                CountryName = request.CountryName?.Trim(),
                Active = true,
                AreaId = request.AreaId,
                Alpha2Code = request.alphacode,
                CountryCode = request.countrycode ?? 0,
            };
        }
        public  void MapUpdateCountryEntity(RefCountry entity, Country request)
        {
            entity.CountryName = request.CountryName;
            entity.Active = true;
            entity.AreaId = request.AreaId;
            entity.Alpha2Code = request.alphacode;
            entity.CountryCode = request.countrycode ?? 0;

        }
        public  RefProvince MapProvinseEntity(State request)
        {
            if (request == null)
                return null;
            return new RefProvince
            {
                CountryId = request.CountryId ?? 0,
                ProvinceCode = request.Code,
                Active = true,
                ProvinceName = request.Name?.Trim()
            };
        }
        public  void MapUpdateProvinseEntity(RefProvince entity, State request)
        {
            entity.CountryId = request.CountryId ?? 0;
            entity.ProvinceCode = request.Code;
            entity.Active = true;
            entity.ProvinceName = request.Name;
        }
        
        private  int? SetIntDefaultValue(int? number)
        {
            return number == 0 ? null : number;
        }
        //public  Holiday GetHoliday(HrHoliday entity, int year)
        //{
        //    if (entity == null)
        //        return null;

        //    DateTime? startDate = null;
        //    DateTime? endDate = null;

        //    switch (entity.RecurrenceType)
        //    {
        //        case (int)RecurrenceType.Manually:
        //            startDate = entity.StartDate;
        //            endDate = entity.EndDate;
        //            break; 
        //        case (int)RecurrenceType.EveryYear:
        //            startDate = entity.StartDate == null ? (DateTime?)null : new DateTime(year, entity.StartDate.Value.Month, entity.StartDate.Value.Day);
        //            endDate = entity.EndDate == null ? (DateTime?)null : new DateTime(year, entity.EndDate.Value.Month, entity.EndDate.Value.Day);
        //            break;
        //        case (int)RecurrenceType.EveryMonth:
        //            startDate = entity.StartDate == null ? (DateTime?)null : new DateTime(year, DateTime.Now.Month, entity.StartDate.Value.Day);
        //            endDate = entity.EndDate == null ? (DateTime?)null : new DateTime(year, DateTime.Now.Month, entity.EndDate.Value.Day);
        //            break;
        //        case (int)RecurrenceType.EveryWeek:
        //            var currentdate = DateTime.Now;
        //            currentdate = currentdate.AddDays((int)currentdate.DayOfWeek * (-1));

        //            for(int i  = 0; i< 7; i++)
        //            {
        //                var date = currentdate.AddDays(i);

        //                if (entity.StartDay != null && (int)date.DayOfWeek == entity.StartDay.Value)
        //                    startDate = date;

        //                if (entity.EndDay != null && (int)date.DayOfWeek == entity.EndDay.Value)
        //                    endDate = date;
        //            }
        //            break; 
        //    }


        //    return new Holiday
        //    {
        //        Id = entity.Id,
        //        RecurrenceType = (RecurrenceType)entity.RecurrenceType,
        //        Name = entity.HolidayName,
        //        StartDate = startDate.GetCustomDate(),
        //        EndDate = endDate.GetCustomDate(),
        //        CountryName = entity.Country.CountryName,
        //        OfficeName = entity.Location?.LocationName
        //    };

        //}

        
public  RefCounty MapCountyEntity(County request)
        {
            if (request == null)
                return null;
            return new RefCounty
            {
                CityId = request.CityId ?? 0,
                Active = true,
                CountyName = request.CountyName?.Trim(),
                CountyCode = request.CountyCode?.Trim(),
                CreatedOn = DateTime.Now,
                ZoneId=request.ZoneId
                
            };
        }
        public  void MapUpdateCountyEntity(RefCounty entity, County request)
        {
            entity.CityId = request.CityId ?? 0;
            entity.Active = true;
            entity.CountyName = request.CountyName?.Trim();
            entity.CountyCode = request.CountyCode?.Trim();
            entity.ModifiedOn = DateTime.Now;
            entity.ZoneId = request.ZoneId;

        }

        public  void MapDeleteCountyEntity(RefCounty entity)
        {
            entity.Active = false;
            entity.DeletedOn = DateTime.Now;

        }
        public  County GetCounty(RefCounty entity)
        {
            if (entity == null)
                return null;

            return new County
            {
                Id = entity.Id,
                CountyName = entity.CountyName,
                CountyCode = entity.CountyCode,
                ZoneId=entity.ZoneId,
                CityId = entity.CityId,
                CityName = entity.City?.CityName,
                ProvinceId = entity.City?.ProvinceId,
                ProvinceName = entity.City?.Province?.ProvinceName,
                CountryId = entity.City?.Province?.CountryId,
                CountryName = entity.City?.Province?.Country.CountryName

            };
        }
        public  Town GetTown(RefTown entity)
        {
            if (entity == null)
                return null;

            return new Town
            {
                Id = entity.Id,
                CountryId = entity.County?.City.Province.CountryId,
                CountryName = entity.County?.City.Province.Country.CountryName,
                ProvinceName = entity.County?.City.Province.ProvinceName,
                ProvinceId = entity.County?.City.ProvinceId,
                CityId = entity.County?.CityId,
                CityName = entity.County?.City.CityName,
                CountyName = entity.County?.CountyName,
                CountyId = entity.CountyId,
                TownName = entity.TownName,
                TownCode = entity.TownCode
            };
        }
        public  RefTown MapTownEntity(Town request)
        {
            if (request == null)
                return null;
            return new RefTown
            {
                CountyId = request.CountyId ?? 0,
                Active = true,
                TownName = request.TownName?.Trim(),
                TownCode = request.TownCode?.Trim(),
                CreatedOn = DateTime.Now

            };
        }
        public  void MapUpdateTownEntity(RefTown entity, Town request)
        {
            entity.CountyId = request.CountyId ?? 0;
            entity.Active = true;
            entity.TownName = request.TownName?.Trim();
            entity.TownCode = request.TownCode?.Trim();
            entity.ModifiedOn = DateTime.Now;

        }
        public  void MapDeleteTownEntity(RefTown entity)
        {
            entity.Active = false;
            entity.DeletedOn = DateTime.Now;

        }
    }
}
