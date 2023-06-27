using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Components.Web;
using Contracts.Managers;
using DTO.KPI;
using LINK_UI.FileModels;
using LINK_UI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class KPIController : ControllerBase
    {

        private readonly IKpiManager _manager = null;

        public KPIController(IKpiManager manager)
        {
            _manager = manager; 
        }


        [HttpGet("modules")]
        [Right("edit-supplier")]
        public async Task<DTO.KPI.ModuleListResponse> GetModuleList()
        {
            return await _manager.GetModuleList();
        }

        [HttpGet("modules/{id}/submodules")]
        [Right("edit-supplier")]
        public async Task<ModuleListResponse> GetSubModuleList(int id)
        {
            return await _manager.GetSubModuleList(id);
        }

        [HttpGet("submodule/{id}/columns")]
        [Right("edit-supplier")]
        public async Task<KpiColumnListResponse> GetColumnList(int id)
        {
            return await _manager.GetColumnList(id);
        }

        [HttpGet("submodule/{id}/filters")]
        [Right("edit-supplier")]
        public async Task<KpiFilterListResponse> GetFilterList(int id)
        {
            return await _manager.GetFilterList(id);
        }

        [HttpGet("module/{id}/columns")]
        [Right("edit-supplier")]
        public async Task<KpiColumnListResponse> GetColumnListByModule(int id)
        {
            return await _manager.GetColumnListByModule(id);
        }

        [HttpGet("module/{id}/filters")]
        [Right("edit-supplier")]
        public async Task<KpiFilterListResponse> GetFilterListByModule(int id)
        {
            return await _manager.GetFilterListByModule(id);
        }


        [HttpGet("submodule/{id}/templates")]
        [Right("edit-supplier")]
        public async Task<KpiTemplateListResponse> GetTemplateList(int id)
        {
            return await _manager.GetTemplateList(id);
        }

        [HttpPost("templates")]
        [Right("edit-supplier")]
        public async Task<KpiTemplateListResponse> SearchTemplates(KpiTemplateListRequest request)
        {
            return await _manager.SearchTemplates(request);
        }


        [HttpGet("templates")]
        [Right("edit-supplier")]
        public async Task<KpiTemplateListResponse> GetTemplateList()
        {
            return await _manager.GetTemplateList(null);
        }

        [HttpGet("template/{id}")]
        [Right("edit-supplier")]
        public async Task<KpiTemplateItemResponse> GetTemplate(int id)
        {
            return await _manager.GetTemplate(id);
        }

        [HttpGet("template/{id}/columns")]
        [Right("edit-supplier")]
        public async Task<KpiTemplateColumnListResponse> GetTemplateColumnList(int id)
        {
            return await _manager.GetTemplateColumnList(id);
        }


        [HttpGet("template/{id}/filters")]
        [Right("edit-supplier")]
        public async Task<KpiTemplateFilterListResponse> GetTemplateFilterList(int id)
        {
            return await _manager.GetTemplateFilterList(id);
        }


        [HttpGet("filter/{id}/data-source")]
        [Right("edit-supplier")]
        public async Task<KpiDataSourceResponse> GetDataSource(int id)
        {
            return await _manager.GetDataSource(id);
        }

        [HttpGet("filter/{id}/data-lazy/{fieldName}/{term}")]
        [Right("edit-supplier")]
        public async Task<IEnumerable<object>> GetDataLazy(int id, string fieldName, string term)
        {
            return await _manager.GetDataLazy(id, fieldName, term);
        }



        [HttpPost("template/save")]
        [Right("edit-supplier")]
        public async Task<KpiSavetemplateResponse> SaveTemplate(KpiTemplateRequest request)
        {
            return await _manager.SaveTemplate(request);
        }

        [HttpGet("template/view/{id}")]
        [Right("edit-supplier")]
        public async Task<KpiTemplateViewResponse> GetViewTemplate(int id)
        {
            return await _manager.GetViewTemplate(id);
        }

        [HttpPost("template/view")]
        [Right("edit-supplier")]
        public async Task<ViewDataResponse> ViewResult(KpiTemplateViewRequest request)
        {
            return await _manager.ViewResult(request);
        }

        [HttpPost("template/export")]
        [Right("edit-supplier")]
        public async Task<IActionResult> ExportResult([FromBody]KpiTemplateViewRequest request)
        {
            request.ForExport = true; 

            var response  = await _manager.ViewResult(request);

            if (response.Result  != ViewDataResult.Success)
                return NotFound();

            var model = new KpiModel
            {
                ColumnList = response.ColumnList,
                Rows = response.Rows,
                UseFormulas = response.UseXls
            };
 
            return await this.FileAsync("Kpi", model, Components.Core.entities.FileType.Excel);
        }


        [HttpDelete("template/{id}/delete")]
        [Right("edit-supplier")]
        public async Task<DeleteTemplateResponse> DeleteTemplate(int id)
        {
            return await _manager.DeleteTemplate(id);
        }


    }
}