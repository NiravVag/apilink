using DTO.Common;
using DTO.CommonClass;
using DTO.Customer;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BI.Maps
{
    public class CheckPointMap : ApiCommonData
    {
        public CheckPoint GetCheckPoint(CuCheckPointType entity)
        {
            if (entity == null)
                return null;
            return new CheckPoint
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }
        public CustomerCheckPoint GetCustomerCheckPoint(CustomerCheckPoint entity, List<CommonCheckPointDataSource> brand, List<CommonCheckPointDataSource> dept, List<CommonCheckPointServiceTypeDataSource> serviceType, List<CommonCheckPointDataSource> countryList)
        {
            if (entity == null)
                return null;
            return new CustomerCheckPoint
            {
                Id = entity.Id,
                CustomerName = entity.CustomerName,
                CheckPointName = entity.CheckPointName,
                ServiceName = entity.ServiceName,
                Remarks = entity?.Remarks,
                CustomerId = entity.CustomerId,
                ServiceId = entity.ServiceId,
                CheckPointId = entity.CheckPointId,
                BrandList = brand?.Select(x => x.Id).ToList(),
                DeptList = dept?.Select(x => x.Id).ToList(),
                ServiceTypeList = serviceType?.Select(x => x.ServiceTypeId).ToList(),
                BrandNames = string.Join(", ", brand?.Select(x => x.Name).ToList()),
                DeptNames = string.Join(", ", dept?.Select(x => x.Name).ToList()),
                ServiceTypeNames = string.Join(", ", serviceType?.Select(x => x.Name).ToList()),
                CountryIdList = countryList?.Select(x => x.Id).ToList(),
                CountryNames = string.Join(", ", countryList?.Select(x => x.Name).ToList())
            };
        }
        public static CuCheckPoint MapCheckPointSaveEntity(CustomerCheckPointSaveRequest request, int? userId, int _entityId)
        {
            if (request == null)
                return null;
            return new CuCheckPoint
            {
                CustomerId = request.CustomerId,
                Active = true,
                ServiceId = request.ServiceId,
                CheckpointTypeId = request.CheckPointId,
                Remarks = request.Remarks,
                CreatedBy = userId > 0 ? userId : null,
                EntityId = _entityId
            };
        }
        public CuCheckPoint UpdateCustomerCPEntity(CustomerCheckPointSaveRequest request, CuCheckPoint checkPointEntity, int? userId)
        {
            checkPointEntity.CustomerId = request.CustomerId;
            checkPointEntity.ServiceId = request.ServiceId;
            checkPointEntity.CheckpointTypeId = request.CheckPointId;
            checkPointEntity.Remarks = request.Remarks;
            checkPointEntity.Active = true;
            checkPointEntity.ModifiedBy = userId > 0 ? userId : null;
            checkPointEntity.ModifiedOn = DateTime.Now;

            return checkPointEntity;
        }
        public CuCheckPoint DeleteEntity(CuCheckPoint deleteEntity, int? userId)
        {
            deleteEntity.Active = false;
            deleteEntity.DeletedBy = userId > 0 ? userId : null;
            deleteEntity.DeletedOn = DateTime.Now;
            return deleteEntity;
        }
    }
}
