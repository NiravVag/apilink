using Contracts.Repositories;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Z.EntityFramework.Plus;

namespace DAL.Repositories
{
    public class GlobalFilterRepository : Repository, IGlobalFilterRepository
    {
        public GlobalFilterRepository(API_DBContext context) : base(context)
        {
        }

        public void SetGlobalFilter(string companyId)
        {
            //_context.Set<CuCustomer>().Where(x => x.EntityId == 2);
            //var notfiltereddata = _context.CuCustomers.ToList();

            //var products = _context.CuProducts.AsQueryable()
            //           .IncludeFilter(p => p.CuProductFileAttachments.Any())
            //           .ToList();

            // var data = this.GetQueryable<CuCustomer>(x => x.EntityId == 2).ToList();

            //var products = _context.CuCustomers
            //         .IncludeFilter(p => p.EntityId.Where(t => t.Locale == "en"))
            //         .ToList();

            // _context._localeFilter = "2";

          //  _context.HrPositions.IncludeFilter(q => q.EntityId).ToList();

            var data = _context.CuCustomers.ToList();

           // _context.SetGlobalFilter(companyId);
            // _context.Filter(CuCustomer).

            //_context.Filter("Notes_CurrentUser", (CuCustomer n) =>
            //n.EntityId() =>  Int32.Parse(companyId));

 

            var data1 = _context.CuCustomers.ToList();


        }

        public void ReSetGlobalFilter()
        {
          //  _context.ResetGlobalFilter();
        }
    }
}
