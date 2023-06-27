using DTO.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface IQCBlockManager 
    {
        Task<SaveQCBlockResponse> Save(QCBlockRequest request);

        Task<EditQCBlockResponse> Edit(int qcBlockId);

        Task<QCBlockSummaryResponse> Search(QCBlockSummaryRequest request);

        Task<List<QCBlockSummaryItem>> ExportQCBlockSummary(QCBlockSummaryRequest requestDto);

        Task<DeleteQCBlockResponse> DeleteQCBlock(IEnumerable<int> qcBlockIds);

        Task<List<int>> GetQCBlockIdList(int bookingId);
    }
}
