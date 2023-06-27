using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Contracts.Repositories
{
    public interface ISharedInspectionRepo
    {
        IQueryable<InspTransaction> GetAllInspectionsQuery();

        IQueryable<InspTransaction> GetInspectionsQuery(int bookingId);

        IQueryable<FbReportDetail> GetAllFbReportDetailsQuery();
    }
}
