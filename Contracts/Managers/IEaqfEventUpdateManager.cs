using DTO.CancelBooking;
using DTO.Eaqf;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface IEaqfEventUpdateManager
    {
        Task<bool> UpdateRescheduleStatusToEAQF(EAQFEventUpdate request, EAQFBookingEventRequestType eAQFBookingEvent);
    }
}
