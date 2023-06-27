using Contracts.Repositories;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Repositories
{
    /// <summary>
    /// Shared inspection repo 
    /// </summary>
    public class SharedInspectionRepo : Repository, ISharedInspectionRepo
    {
        public SharedInspectionRepo(API_DBContext context) : base(context)
        {
        }

        /// <summary>
        /// Get all the inspection as Query
        /// </summary>
        /// <returns></returns>
        public IQueryable<InspTransaction> GetAllInspectionsQuery()
        {
            return _context.InspTransactions;
        }

        public IQueryable<FbReportDetail> GetAllFbReportDetailsQuery()
        {
            return _context.FbReportDetails.Where(x => x.Active == true && x.InspProductTransactions.Any(x => x.Active == true));
        }

        public IQueryable<InspTransaction> GetInspectionsQuery(int bookingId)
        {
            return _context.InspTransactions.Where(x => x.Id == bookingId);
        }
    }
}
