using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Components.Web;
using Contracts.Managers;
using DTO.CommonClass;
using DTO.Schedule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static DTO.Common.Static_Data_Common;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class QCBlockController : ControllerBase
    {
        readonly IQCBlockManager _qcBlockmanager = null;

        public QCBlockController(IQCBlockManager qcBlockmanager)
        {
            _qcBlockmanager = qcBlockmanager;
        }

        [HttpPost]
        public async Task<SaveQCBlockResponse> Save(QCBlockRequest request)
        {
            //any other fields should have atleast one value along with QC value
            if(!((request.CustomerIds != null && request.CustomerIds.Any()) || (request.SupplierIds != null && request.SupplierIds.Any()) || 
                (request.FactoryIds != null && request.FactoryIds.Any()) || (request.ProductCategoryIds != null && request.ProductCategoryIds.Any()) ||
                (request.ProductCategorySubIds != null && request.ProductCategorySubIds.Any()) || 
                (request.ProductCategorySub2Ids != null && request.ProductCategorySub2Ids.Any())))
            {
                return new SaveQCBlockResponse() { Result = QCBlockResponseResult.SelectAnyOtherField };
            }
            return await _qcBlockmanager.Save(request);
        }

        [HttpGet("{id}")]
        public async Task<EditQCBlockResponse> Edit(int id)
        {
            return await _qcBlockmanager.Edit(id);
        }

        [HttpPost("search")]
        public async Task<QCBlockSummaryResponse> Search(QCBlockSummaryRequest request)
        {
            return await _qcBlockmanager.Search(request);
        }

        [HttpPost("export-summary")]
        public async Task<IActionResult> ExportSearchSummary(QCBlockSummaryRequest request)
        {
            int pageindex = 1;
            int PageSize = 100000;
            if (request == null)
                return NotFound();
            request.Index = pageindex;
            request.PageSize = PageSize;
            var response = await _qcBlockmanager.ExportQCBlockSummary(request);
            if (response == null)
                return NotFound();
            return await this.FileAsync("QCBlockSearchSummary", response, Components.Core.entities.FileType.Excel);
        }
        [HttpPost("delete")]
        public async Task<DeleteQCBlockResponse> DeleteQCBlock(IEnumerable<int> qcBlockIds)
        {
            return await _qcBlockmanager.DeleteQCBlock(qcBlockIds);
        }
    }
}