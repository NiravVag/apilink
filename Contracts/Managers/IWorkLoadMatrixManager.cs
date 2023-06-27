using DTO.WorkLoadMatrix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface IWorkLoadMatrixManager
    {
        Task<SaveWorkLoadMatrixResponse> SaveWorkLoadMatrix(SaveWorkLoadMatrixRequest request);
        Task<SaveWorkLoadMatrixResponse> UpdateWorkLoadMatrix(SaveWorkLoadMatrixRequest request);
        Task<DeleteWorkLoadMatrixResponse> DeleteWorkLoadMatrix(int id);
        Task<EditWorkLoadMatrixResponse> GetEditWorkLoadMatrix(int id, bool workLoadMatrixNotConfigured);
        Task<WorkLoadMatrixSummaryResponse> GetWorkLoadMatrixSummary(WorkLoadMatrixSummaryRequest request);
        Task<EditWorkLoadMatrixResponse> GettWorkLoadMatrixDataByProdCatSub3Id(int prodCatSub3Id);
        Task<List<ExportWorkLoadMatrixData>> ExportWorkLoadMatrixSummary(WorkLoadMatrixSummaryRequest request);
    }
}
