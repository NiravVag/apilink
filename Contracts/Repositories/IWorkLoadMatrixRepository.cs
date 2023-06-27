using DTO.WorkLoadMatrix;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface IWorkLoadMatrixRepository: IRepository
    {
        Task<bool> CheckIfWorkLoadMatrixAlreadyExists(int subCategory3Id,int workloadmatrixid=0);
        Task<QuWorkLoadMatrix> GetWorkLoadMatrixById(int id);
        Task<WorkLoadMatrixData> GetWorkLoadMatrixEditDataById(int id);
        IQueryable<WorkLoadMatrixData> GetWorkLoadMatrixByEfCore();
        Task<WorkLoadMatrixData> GetProductCategorySub3ById(int id);
        Task<WorkLoadMatrixData> GetWorkLoadMatrixByProductCatSub3Id(int prodCatSub3Id);
        Task<List<WorkLoadMatrixData>> GetWorkLoadMatrixByProductCatSub3List(List<int> prodCatSub3List);
    }
}
