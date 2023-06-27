using DTO.CommonClass;
using DTO.TravelTariff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface ITravelTariffManager
    {
        Task<TravelTariffSaveResponse> SaveTravelTariff(TravelTariffSaveRequest request);
        Task<TravelTariffGetResponse> GetTravelTariffDetails(int Id);
        Task<TravelTariffGetAllResponse> GetAllTravelTariffDetails(TravelTariffSearchRequest request);
        Task<TravelTariffSaveResponse> UpdateTravelTariff(TravelTariffSaveRequest request);
        Task<TravelTariffDeleteResponse> DeleteTravelTariff(int Id);
        Task<TravelTariffGetAllResponse> GetExportTravelTariffDetails(TravelTariffSearchRequest request);
        Task<DataSourceResponse> GetStartPotList();
    }
}
