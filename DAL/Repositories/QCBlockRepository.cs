using Contracts.Repositories;
using DTO.CommonClass;
using DTO.Schedule;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class QCBlockRepository : Repository, IQCBlockRepository
    {
        public QCBlockRepository(API_DBContext context) : base(context)
        {

        }
       
        /// <summary>
        /// Get QC block details
        /// </summary>
        /// <param name="qcBlockId"></param>
        /// <returns></returns>
        public async Task<QcBlockList> GetQCDetails(int qcBlockId)
        {
            return await _context.QcBlockLists.Where(x => x.Active.Value && x.Id == qcBlockId)
                .Include(x => x.QcBlCustomers)
                .Include(x => x.QcBlProductCatgeories)
                .Include(x => x.QcBlProductSubCategories)
                .Include(x => x.QcBlProductSubCategory2S)
                .Include(x => x.QcBlSupplierFactories)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// get qc block details
        /// </summary>
        /// <returns></returns>
        public IQueryable<QCBlockSummaryRepo> GetQCBlockSummaryDetails()
        {
            return _context.QcBlockLists.Where(x => x.Active.Value)
                .Select(x => new QCBlockSummaryRepo
                {
                    QCName = x.Qc.PersonName,
                    QCId = x.Qcid,
                    QCBlockId = x.Id,
                });
        }
        public Task<List<CommonDataSource>> GetQcBlockCustomers(IQueryable<int> Qcblockid)
        {
            return _context.QcBlCustomers.Where(y=> Qcblockid.Contains( y.Qcblid))
                .Select(x =>new CommonDataSource {Name= x.Customer.CustomerName,Id=x.Qcblid }).AsNoTracking().ToListAsync();
        }
        public Task<List<CommonDataSource>> GetQcBlockFactories(IQueryable<int> Qcblockid)
        {
            return _context.QcBlSupplierFactories.Where(y => Qcblockid.Contains(y.Qcblid) && y.TypeId == (int)Supplier_Type.Factory)
                .Select(x => new CommonDataSource { Name = x.SupplierFactory.SupplierName,Id=x.Qcblid }).AsNoTracking().ToListAsync();
        }
        public Task<List<CommonDataSource>> GetQcBlockProductCategory(IQueryable<int> Qcblockid)
        {
            return _context.QcBlProductCatgeories.Where(y => Qcblockid.Contains(y.Qcblid))
                .Select(y => new CommonDataSource { Name = y.ProductCategory.Name, Id = y.Qcblid }).AsNoTracking().ToListAsync();
        }
        public Task<List<CommonDataSource>> GetQcBlockProductSubCategory(IQueryable<int> Qcblockid)
        {
            return _context.QcBlProductSubCategories.Where(y => Qcblockid.Contains(y.Qcblid))
                .Select(y => new CommonDataSource { Name = y.ProductSubCategory.Name, Id = y.Qcblid }).AsNoTracking().ToListAsync();
        }
        public Task<List<CommonDataSource>> GetQcBlockProductSub2Category(IQueryable<int> Qcblockid)
        {
            return _context.QcBlProductSubCategory2S.Where(y => Qcblockid.Contains(y.Qcblid))
                .Select(y =>new CommonDataSource {Name= y.ProductSubCategory2.Name,Id=y.Qcblid }).AsNoTracking().ToListAsync();
        }
        public Task<List<CommonDataSource>> GetQcBlockSuppliers(IQueryable<int> Qcblockid)
        {
            return _context.QcBlSupplierFactories.Where(y => Qcblockid.Contains(y.Qcblid) && y.TypeId == (int)Supplier_Type.Supplier_Agent)
                .Select(y =>new CommonDataSource { Name = y.SupplierFactory.SupplierName, Id = y.Qcblid }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get QC block details
        /// </summary>
        /// <param name="qcBlockIds"></param>
        /// <returns></returns>
        public async Task<List<QcBlockList>> GetQCBlockData(IEnumerable<int> qcBlockIds)
        {
            return await _context.QcBlockLists.Where(x => qcBlockIds.Contains(x.Id))
                .Include(x => x.QcBlCustomers)
                .Include(x => x.QcBlProductCatgeories)
                .Include(x => x.QcBlProductSubCategories)
                .Include(x => x.QcBlProductSubCategory2S)
                .Include(x => x.QcBlSupplierFactories)
                .ToListAsync();
        }

        /// <summary>
        /// get qc block Ids with following ids supplier, factory product category, sub, sub2
        /// </summary>
        /// <returns></returns>
        public IQueryable<QcBlockList> GetQCBlockRelatedIdList()
        {
            return _context.QcBlockLists.Where(x => x.Active.Value);
                
        }

     /// <summary>
     /// get QC Details 
     /// </summary>
     /// <param name="request"></param>
     /// <returns></returns>
        public async Task<IEnumerable<QcBlockList>> GetAllQCDetails(QCBlockRequest request)
        {
            return await _context.QcBlockLists.Where(x => x.Active.Value && x.Id != request.Id 
            && x.Qcid == request.QCId)
                .Include(x => x.QcBlCustomers)
                .Include(x => x.QcBlProductCatgeories)
                .Include(x => x.QcBlProductSubCategories)
                .Include(x => x.QcBlProductSubCategory2S)
                .Include(x => x.QcBlSupplierFactories)
                .ToListAsync();
        }
    }
}
