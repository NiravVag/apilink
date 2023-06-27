using BI.Utilities;
using Contracts.Managers;
using DTO.GenericAPI;
using Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class APIGatewayController : ControllerBase
    {
        private readonly IAPIGatewayManager _manager = null;
        private readonly IHelper _helper = null;
        public APIGatewayController(IAPIGatewayManager manager, IHelper helper)
        {
            _manager = manager;
            _helper = helper;
        }

        [HttpPost("GetData")]
        public async Task<dynamic> Get(APIGatewayGETRequest request)
        {
            return await _manager.GetRequest(request);
        }

        [HttpPost]
        public async Task<dynamic> Post(APIGatewayPOSTRequest request)
        {
            return await _manager.PostRequest(request);
        }

        [HttpPut]
        public async Task<dynamic> Put(APIGatewayPUTRequest request)
        {
            return await _manager.PutRequest(request);
        }

        [HttpDelete]
        public async Task<dynamic> Delete(GenericAPIDELETERequest request)
        {
            return await _manager.DeleteRequest(request);
        }

        [HttpPost("attachdocument")]
        [DisableRequestSizeLimit]
        public async Task<dynamic> UploadAttachedFiles(List<IFormFile> files)
        {
            if (files != null && files.Any())
            {

                var requestFormValues = Request.Form.ToDictionary(x => x.Key, x => x.Value.ToString());

                foreach (var file in files)
                {
                    if (file != null && file.Length > 0)
                    {
                        return await _manager.UploadDocument(file, requestFormValues);
                    }
                }

            }
            return default(dynamic);
        }

        [HttpPost("uploadfiles")]
        [DisableRequestSizeLimit]
        public async Task<dynamic> UploadFiles(List<IFormFile> files)
        {
            var requestFormValues = Request.Form.ToDictionary(x => x.Key, x => x.Value.ToString());

            //var response=dynamic
            if (files != null && files.Any())
            {
                return await _manager.UploadFiles(files, requestFormValues);
            }
            return default(dynamic);
        }

    }
}