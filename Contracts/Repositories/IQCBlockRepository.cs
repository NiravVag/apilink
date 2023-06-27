using DTO.CommonClass;
using DTO.Schedule;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface IQCBlockRepository : IRepository
    {
        Task<QcBlockList> GetQCDetails(int qcBlockId);

        IQueryable<QCBlockSummaryRepo> GetQCBlockSummaryDetails();

        Task<List<QcBlockList>> GetQCBlockData(IEnumerable<int> qcBlockIds);

        IQueryable<QcBlockList> GetQCBlockRelatedIdList();

        Task<IEnumerable<QcBlockList>> GetAllQCDetails(QCBlockRequest request);

        Task<List<CommonDataSource>> GetQcBlockFactories(IQueryable<int> Qcblockid);
        Task<List<CommonDataSource>> GetQcBlockProductCategory(IQueryable<int> Qcblockid);
        Task<List<CommonDataSource>> GetQcBlockCustomers(IQueryable<int> Qcblockid);
        Task<List<CommonDataSource>> GetQcBlockProductSubCategory(IQueryable<int> Qcblockid);
        Task<List<CommonDataSource>> GetQcBlockProductSub2Category(IQueryable<int> Qcblockid);
        Task<List<CommonDataSource>> GetQcBlockSuppliers(IQueryable<int> Qcblockid);
    }
}
