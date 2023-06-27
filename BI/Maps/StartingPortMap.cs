using DTO.StartingPort;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Maps
{
    public class StartingPortMap
    {
        public EcAutRefStartPort MapSaveRequest(StartingPortRequest request, int userId, int companyId)
        {
            return new EcAutRefStartPort
            {
                StartPortName = request.StartPortName,
                Active = true,
                CreatedBy = userId,
                CreatedOn = DateTime.Now,
                EntityId = companyId,
                Sort = true,
                CityId = request.CityId == 0 ? null : request.CityId
            };
        }

        public EcAutRefStartPort MapUpdateRequest(EcAutRefStartPort entity, StartingPortRequest request, int userId)
        {
            entity.StartPortName = request.StartPortName;
            entity.UpdatedBy = userId;
            entity.UpdatedOn = DateTime.Now;
            entity.CityId = request.CityId == 0 ? null : request.CityId;

            return entity;
        }

        public StartingPortSummaryData MapStartingPortData(EcAutRefStartPort entity)
        {
            return new StartingPortSummaryData
            {
                StartingPortId = entity.Id,
                CityId = entity?.CityId.GetValueOrDefault() ?? 0,
                CityName = entity?.City?.CityName,
                StartingPortName = entity?.StartPortName
            };
        }
    }
}
