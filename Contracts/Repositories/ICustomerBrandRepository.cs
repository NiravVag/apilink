using DTO.CommonClass;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface ICustomerBrandRepository : IRepository
    {      
        Task<int> AddCustomerBrand(CuBrand entity);
        Task<int> EditCustomerBrand(CuBrand entity);
        Task<int> RemoveCustomerBrand();
        Task<CuCustomer> GetCustomerBrandDetails(int? id);
        Task<List<CuBrand>> GetCustomerBrands(int customerId);
        /// <summary>
        /// get active brand list by customer id
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        IQueryable<CommonDataSource> GetBrandDataSource(int customerId);

        Task<List<string>> GetBrandNameByBrandId(IEnumerable<int> brandIdList);
    }
}
