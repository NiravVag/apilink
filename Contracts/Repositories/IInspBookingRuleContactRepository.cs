using Contracts.Repositories;
using Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
	public interface IInspBookingRuleContactRepository : IRepository
	{

		/// <summary>
		/// Get Booking Contacts
		/// </summary>
		/// <param name="officeId"></param>
		/// <returns>list</returns>
		//Task<List<InspBookingContact>> GetInspBookingContacts(int officeId);
		/// <summary>
		/// Get Insp Booking Rule
		/// </summary>
		/// <param name=""></param>
		/// <returns>int </returns>
		Task<List<InspBookingRule>> GetInspBookingRule();
	}
}
