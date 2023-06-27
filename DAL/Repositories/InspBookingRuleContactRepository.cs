using Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Entities.Enums;
using DTO.Common;
using Contracts.Repositories;

namespace DAL.Repositories
{
	public class InspBookingRuleContactRepository : Repository, IInspBookingRuleContactRepository
	{
        private readonly IAPIUserContext _ApplicationContext = null;

        public InspBookingRuleContactRepository(API_DBContext context, IAPIUserContext applicationContext) : base(context)
        {  _ApplicationContext = applicationContext; }

		//public Task<List<InspBookingContact>> GetInspBookingContacts(int officeId)
		//{
		//	return _context.InspBookingContacts
		//		.Include(x => x.Office)
		//		.Include(x => x.FactoryCountry)
		//		.Include(x => x.User)
		//		.ThenInclude(x => x.Staff).Where(x => x.Active)
		//		.Where(x => x.Active && x.OfficeId == officeId).ToListAsync();
		//}
		public Task<List<InspBookingRule>> GetInspBookingRule()
		{
			return _context.InspBookingRules
				.Where(x => x.Active).ToListAsync();
		}
	}
}
