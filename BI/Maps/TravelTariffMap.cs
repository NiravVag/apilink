using DTO.Common;
using DTO.TravelTariff;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Maps
{
    public class TravelTariffMap
    {
        public EcAutTravelTariff TravelTariffSaveRequestMap(TravelTariffSaveRequest model, int userId, int _entityId)
        {
            return new EcAutTravelTariff()
            {
                StartDate = model.StartDate.ToDateTime(),
                EndDate = model.EndDate.ToDateTime(),
                StartPort = model.StartPort,
                TownId = model.TownId,
                TravelCurrency = model.TravelCurrency,
                TravelTariff = model.TravelTariff,
                Active = true,
                CreatedBy = userId,
                CreatedOn = DateTime.Now,
                EntityId = _entityId,
                Status = true
            };
        }

        public EcAutTravelTariff TravelTariffUpdateRequestMap(TravelTariffSaveRequest model, int userId, EcAutTravelTariff entity)
        {
            entity.Id = model.Id;
            entity.StartDate = model.StartDate.ToDateTime();
            entity.EndDate = model.EndDate.ToDateTime();
            entity.StartPort = model.StartPort;
            entity.TownId = model.TownId;
            entity.TravelCurrency = model.TravelCurrency;
            entity.TravelTariff = model.TravelTariff;
            entity.Active = true;
            entity.Status = true;
            entity.UpdatedOn = DateTime.Now;
            entity.UpdatedBy = userId;
            return entity;
        }

        public TravelTariffData GetTravelTariffMap(EcAutTravelTariff entity)
        {
            return new TravelTariffData()
            {
                Id = entity.Id,
                StartPortId = entity.StartPort,
                StartDate = Static_Data_Common.GetCustomDate(entity.StartDate),
                EndDate = Static_Data_Common.GetCustomDate(entity.EndDate),
                TownId = entity.TownId,
                TravelCurrency = entity.TravelCurrency,
                TravelTariff = entity.TravelTariff
            };
        }
    }
}
