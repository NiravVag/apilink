using DTO.Common;
using DTO.CommonClass;
using DTO.Location;
using DTO.OfficeLocation;
using Entities;
using System.Collections.Generic;
using System.Linq;

namespace BI.Maps
{
    public class OfficeLocationMap: ApiCommonData
    {
        public  Office GetLocation(RefLocation entity)
        {
            if (entity == null)
                return null;
            var countryIdList = new List<int> { };
            string countriesName = "";
            int i = 1;
            foreach (var element in entity.RefLocationCountries)
            {
                if (element.LocationId == entity.Id)
                {
                    countryIdList.Add(element.CountryId);
                    if(i == entity.RefLocationCountries.Count())
                        countriesName += element.Country.CountryName;
                    else
                        countriesName += element.Country.CountryName + ", ";
                }
                i++;
            }
            var countryId = entity.City?.Province?.Country?.Id;
            var countryName = entity.City?.Province?.Country?.CountryName;
            return new Office
            {
                Id = entity.Id,
                OfficeCode = entity.OfficeCode,
                Address = entity.Address,
                Address2 = entity.Address2,
                CityId = entity.CityId,
                ZipCode = entity.ZipCode,
                Name = entity.LocationName,
                Master_Currency_Id = entity.MasterCurrencyId,
                Default_Currency_Id = entity.DefaultCurrencyId,
                Tel = entity.Tel,
                Fax = entity.Fax,
                Email = entity.Email,
                ParentId = entity.ParentId,
                Comment = entity.Comment,
                EntityId = entity.EntityId,
                HeadOffice = entity.HeadOffice,
                OperationCountries = countryIdList,
                OperationCountriesName = countriesName,
                Type = new OfficeType
                {
                    Id = entity.LocationTypeId,
                    Name = entity.LocationType?.SgtLocationType
                },
                City = new City
                {
                    Id = entity.City.Id,
                    Name = entity.City?.CityName
                },
                Country = new Country
                {
                    Id = countryId.GetValueOrDefault(0),
                    CountryName = countryName,
                }
            };
        }

        public  OfficeType GetLocationType(RefLocationType entity)
        {
            if (entity == null)
                return null;

            return new OfficeType
            {
                Id = entity.Id,
                Name = entity.SgtLocationType
            };
        }

        public  RefLocation MapOfficeEntity(Office request,int entityid)
        {

            if (request == null)
                return null;
            ICollection<RefLocationCountry> OperationCountries = new List<RefLocationCountry>();
            foreach (var element in request.OperationCountries)
            {
                RefLocationCountry newOperationCountries = new RefLocationCountry();
                newOperationCountries.LocationId = request.Id;
                newOperationCountries.CountryId = element;
                OperationCountries.Add(newOperationCountries);
            }
            return new RefLocation
            {
                LocationName = request.Name,
                OfficeCode = request.OfficeCode,
                Active = true,
                Fax = request.Fax ?? null,
                Tel = request.Tel ?? null,
                ZipCode = request.ZipCode ?? null,
                Address = request.Address,
                Address2 = request.Address2,
                Email = request.Email ?? null,
                CityId = request.CityId,
                Comment = request.Comment ?? null,
                LocationTypeId = request.LocationTypeId,
                HeadOffice = request.HeadOffice,
                RefLocationCountries = OperationCountries,
                EntityId= entityid
            };
        }

        public  CommonDataSource LocationMap(RefLocation entity)
        {
            return new CommonDataSource()
            {
                Id = entity.Id,
                Name = entity.LocationName
            };
        }

        public Office GetOfficeMap(RefLocation entity)
        {
            return new Office()
            {
                Id = entity.Id,
                Name = entity.LocationName
            };
        }

        //public  void MapUpdateOfficeEntity(RefLocation entity, Office request)
        //{
        //    entity.LocationName = request.Name;
        //    entity.OfficeCode = request.OfficeCode;
        //    entity.Active = true;
        //    entity.Fax = request.Fax ?? null;
        //    entity.Tel = request.Tel ?? null;
        //    entity.ZipCode = request.ZipCode ?? null;
        //    entity.Address = request.Address;
        //    entity.Address2 = request.Address2;
        //    entity.Email = request.Email ?? null;
        //    entity.CityId = request.CityId;
        //    entity.LocationTypeId = request.LocationTypeId;
        //    entity.HeadOffice = request.HeadOffice;
        //    entity.Comment = request.Comment ?? null;
        //    entity.EntityId = _ApplicationContext.EntityId;
        //    foreach (var element in request.OperationCountries)
        //    {
        //        RefLocationCountry newOperationCountries = new RefLocationCountry();
        //        newOperationCountries.LocationId = request.Id;
        //        newOperationCountries.CountryId = element;
        //        entity.RefLocationCountries.Add(newOperationCountries);
        //    }
        //}
    }
}
