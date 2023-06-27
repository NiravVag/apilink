using DTO.Common;
using DTO.CommonClass;
using DTO.Customer;
using DTO.References;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BI.Maps
{
    public class ReferenceMap : ApiCommonData
    {

        public Unit GetUnit(RefUnit entity)
        {
            if (entity == null)
                return null;
            return new Unit
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }
        public Season GetSeason(RefSeason entity)
        {
            if (entity == null)
                return null;
            return new Season
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }
        public SeasonYear GetSeasonYear(RefSeasonYear entity)
        {
            if (entity == null)
                return null;
            return new SeasonYear
            {
                Id = entity.Id,
                Year = entity.Year
            };
        }

        public Service GetService(RefService entity)
        {
            if (entity == null)
                return null;
            return new Service
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }

        public ServiceType GetServiceType(RefServiceType entity)
        {
            if (entity == null)
                return null;
            return new ServiceType
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }

        public ProductCategory GetProductCategory(RefProductCategory entity)
        {
            if (entity == null)
                return null;
            return new ProductCategory
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }

        public PickType GetPickType(RefPickType entity)
        {
            if (entity == null)
                return null;
            return new PickType
            {
                Id = entity.Id,
                Value = entity.Value
            };
        }

        public LevelPick1 GetLevelPick1(RefLevelPick1 entity)
        {
            if (entity == null)
                return null;
            return new LevelPick1
            {
                Id = entity.Id,
                Value = entity.Value
            };
        }

        public LevelPick2 GetLevelPick2(RefLevelPick2 entity)
        {
            if (entity == null)
                return null;
            return new LevelPick2
            {
                Id = entity.Id,
                Value = entity.Value
            };
        }

        public Pick1 GetPick1(RefPick1 entity)
        {
            if (entity == null)
                return null;
            return new Pick1
            {
                Id = entity.Id,
                Value = entity.Value
            };
        }

        public Pick2 GetPick2(RefPick2 entity)
        {
            if (entity == null)
                return null;
            return new Pick2
            {
                Id = entity.Id,
                Value = entity.Value
            };
        }

        public DefectClassification GetDefectClassification(RefDefectClassification entity)
        {
            if (entity == null)
                return null;
            return new DefectClassification
            {
                Id = entity.Id,
                Value = entity.Value
            };
        }

        public ReportUnit GetReportUnit(RefReportUnit entity)
        {
            if (entity == null)
                return null;
            return new ReportUnit
            {
                Id = entity.Id,
                Value = entity.Value
            };
        }

        //map service type from table(RefServiceType)
        public CommonDataSource GetServiceTypeMap(RefServiceType entity)
        {
            if (entity == null)
                return null;
            return new CommonDataSource
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }

        //map currency from table
        public CommonDataSource GetCurrencyMap(RefCurrency entity)
        {
            if (entity == null)
                return null;
            return new CommonDataSource
            {
                Id = entity.Id,
                Name = entity.CurrencyName
            };
        }

        //map currency from table
        public CurrencyDataSource GetCurrencyCodeMap(RefCurrency entity)
        {
            if (entity == null)
                return null;
            return new CurrencyDataSource
            {
                Id = entity.Id,
                Name = entity.CurrencyName,
                CurrencyCode = entity.CurrencyCodeA
            };
        }

        public CommonDataSource GetServiceData(Service entity)
        {
            if (entity == null)
                return null;
            return new CommonDataSource
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }

        public DpPoint GetDpPoint(InspRefDpPoint entity)
        {
            if (entity == null)
                return null;
            return new DpPoint
            {
                Id = entity.Id,
                Value = entity.Name
            };
        }
    }
}
