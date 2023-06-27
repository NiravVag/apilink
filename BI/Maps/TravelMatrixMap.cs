using DTO.Common;
using DTO.CommonClass;
using DTO.Customer;
using DTO.Invoice;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BI.Maps
{
    public  class TravelMatrixMap: ApiCommonData
    {
        //save map
        public  InvTmDetail GetSaveMap(TravelMatrix model, int userId,int _entityId)
        {
            return new InvTmDetail()
            {
                CreatedOn = DateTime.Now,
                CreatedBy = userId,
                Active = true,
                AirCost = model.AirCost,
                BusCost = model.BusCost,
                HotelCost = model.HotelCost,
                TaxiCost = model.TaxiCost,
                TrainCost = model.TrainCost,
                CityId = model.CityId,
                CountryId = model.CountryId,
                CountyId = model.CountyId,
                ProvinceId = model.ProvinceId,
                CustomerId = model.CustomerId,
                DistanceKm = model.DistanceKM,
                FixedExchangeRate = model.FixExchangeRate,

                InspPortCountyId = model.InspPortId,
                InspPortCityId = model.InspPortCityId,
                MarkUpAirCost = model.MarkUpCostAir,
                SourceCurrencyId = model.SourceCurrencyId,
                TravelCurrencyId = model.TravelCurrencyId,

                Remarks = model.Remarks,
                OtherCost = model.OtherCost,
                TravelMatrixTypeId = model.TariffTypeId,
                MarkUpCost = model.MarkUpCost,
                TravelTime = model.TravelTime,
                EntityId=_entityId
            };
        }

        //update map
        public  InvTmDetail GetUpdateMap(InvTmDetail entity, TravelMatrix model, int userId)
        {
            entity.UpdatedOn = DateTime.Now;
            entity.UpdatedBy = userId;
            entity.Active = true;
            entity.AirCost = model.AirCost;
            entity.BusCost = model.BusCost;
            entity.HotelCost = model.HotelCost;
            entity.TaxiCost = model.TaxiCost;
            entity.TrainCost = model.TrainCost;
            entity.CityId = model.CityId;
            entity.CountryId = model.CountryId;
            entity.CountyId = model.CountyId;
            entity.ProvinceId = model.ProvinceId;
            entity.CustomerId = model.CustomerId;
            entity.DistanceKm = model.DistanceKM;
            entity.FixedExchangeRate = model.FixExchangeRate;
            entity.InspPortCountyId = model.InspPortId;
            entity.InspPortCityId = model.InspPortCityId;
            entity.MarkUpAirCost = model.MarkUpCostAir;
            entity.SourceCurrencyId = model.SourceCurrencyId;
            entity.TravelCurrencyId = model.TravelCurrencyId;

            entity.Remarks = model.Remarks;
            entity.OtherCost = model.OtherCost;
            entity.TravelMatrixTypeId = model.TariffTypeId;
            entity.MarkUpCost = model.MarkUpCost;
            entity.TravelTime = model.TravelTime;

            return entity;
        }

        // search map
        public  TravelMatrixSearch GetSearchMap(InvTmDetail entity)
        {
            return new TravelMatrixSearch()
            {
                Id = entity.Id,
                CityId = entity.CityId,
                CountryId = entity.CountryId,
                CountyId = entity.CountyId,
                ProvinceId = entity.ProvinceId,
                CustomerId = entity.CustomerId,
                AirCost = entity.AirCost,
                BusCost = entity.BusCost,
                HotelCost = entity.HotelCost,
                TaxiCost = entity.TaxiCost,
                TrainCost = entity.TrainCost,
                DistanceKM = entity.DistanceKm,
                FixExchangeRate = entity.FixedExchangeRate,
                MarkUpCostAir = entity.MarkUpAirCost,
                SourceCurrencyId = entity.SourceCurrencyId,
                TravelCurrencyId = entity.TravelCurrencyId,
                MarkUpCost = entity.MarkUpCost,
                TravelTime = entity.TravelTime,
                OtherCost = entity.OtherCost,
                InspPortId = entity.InspPortCountyId,
                Remarks = entity.Remarks,
                TariffTypeId = entity.TravelMatrixTypeId,
            };
        }

        public  CommonDataSource GetAreaMap(ParentDataSource dataSource)
        {
            return new CommonDataSource()
            {
                Id = dataSource.Id,
                Name = dataSource.Name
            };
        }

        public  TravelMatrixSearch GetDefaultMap(AreaDetails entity)
        {
            return new TravelMatrixSearch()
            {
                CityId = entity.CityId,
                CountryId = entity.CountryId,
                CountyId = entity.CountyId,
                ProvinceId = entity.ProvinceId,
            };
        }

        public  TravelMatrixExists ExistsMap(TravelMatrixData data)
        {
            return new TravelMatrixExists()
            {
                CustomerName = data.CustomerName,
                CountryName = data.CountryName,
                CityName = data.CityName,
                CountyName = data.CountyName,
                ProvinceName = data.ProvinceName,
                SourceCurrencyName = data.SourceCurrencyName,
                TariffTypeName = data.TariffTypeName,
                TravelCurrencyName = data.TravelCurrencyName
            };
        }

        //export summary map
        public  TravelMatrixExportSummary MapExportSummary(TravelMatrixSearch item)
        {

            return new TravelMatrixExportSummary()
            {
                CountryName = item.CountryName,
                CountryId = item.CountryId,
                ProvinceName = item.ProvinceName,
                ProvinceId = item.ProvinceId,
                CityName = item.CityName,
                CityId = item.CityId,
                CountyName = item.CountyName,
                CountyId = item.CountyId,
                InspPortNameCounty = item.InspPortName,
                InspPortCountyId = item.InspPortId,
                InspPortNameCity=item.InspPortCityName,
                InspPortCityId = item.InspPortCityId,
                CustomerName = item.CustomerName,
                TariffTypeName = item.TariffTypeName,
                DistanceKM = item.DistanceKM,
                TravelTime = item.TravelTime,
                Remarks = item.Remarks,
                BusCost = item.BusCost,
                TrainCost = item.TrainCost,
                TaxiCost = item.TaxiCost,
                HotelCost = item.HotelCost,
                OtherCost = item.OtherCost,
                MarkUpCost = item.MarkUpCost,
                MarkUpCostAir = item.MarkUpCostAir,
                AirCost = item.AirCost,
                TravelCurrencyName = item.TravelCurrencyName,
                SourceCurrencyName = item.SourceCurrencyName,
                FixExchangeRate = item.FixExchangeRate,
            };
        }
        public  ExistsTravelMatrixRequest ExistsRequestMap(IEnumerable<TravelMatrix> model)
        {
            return new ExistsTravelMatrixRequest()
            {
                CityList = model.Select(x => x.CityId),
                CountryList = model.Select(x => x.CountryId),
                ProvinceList = model.Select(x => x.ProvinceId),
                CountyList = model.Select(x => x.CountyId),
                SourceCurrencyList = model.Select(x => x.SourceCurrencyId),
                TariffTypeList = model.Select(x => x.TariffTypeId)
            };
        }
        public  CustomerPriceCardRequest PriceCardRequestMap(TravelMatrixRequest request)
        {
            return new CustomerPriceCardRequest()
            {
                BillMethodId = request.BillMethodId,
                BillPaidById = request.BillPaidById,
                CurrencyId = request.CurrencyId,
                BookingId = request.BookingId
            };
        }

        //request frame to get travel matrix details
        public  QuotationMatrixRequest MatrixMapRequest(TravelMatrixRequest request, QuotationCustomerPriceCard priceCardDetails)
        {
            return new QuotationMatrixRequest()
            {
                CountyId = request.CountyId,
                CityId=request.CityId,
                ProvinceId=request.ProvinceId,
                CurrencyId = priceCardDetails.CurrencyId,
                MatrixTypeId = priceCardDetails.TravelMatrixTypeId,
                customerId= request.customerId
            };
        }
    }
}
