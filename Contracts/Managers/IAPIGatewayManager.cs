using DTO.GenericAPI;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface IAPIGatewayManager
    {
        /// <summary>
        /// Generic Get Request
        /// </summary>
        /// <param name="baseRequest"></param>
        /// <param name="request"></param>
        /// <param name="token"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        Task<dynamic> GetRequest(APIGatewayGETRequest request);

        /// <summary>
        /// Generic Post Request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<dynamic> PostRequest(APIGatewayPOSTRequest request);

        /// <summary>
        /// Generic Put Request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<dynamic> PutRequest(APIGatewayPUTRequest request);

        /// <summary>
        /// Generic Delete Request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<dynamic> DeleteRequest(GenericAPIDELETERequest request);

        /// <summary>
        /// Upload the tcf document
        /// </summary>
        /// <param name="fileToUpload"></param>
        /// <param name="requestFormValues"></param>
        /// <returns></returns>
        Task<dynamic> UploadDocument(IFormFile fileToUpload, IDictionary<string, string> requestFormValues);

        Task<dynamic> UploadFiles(List<IFormFile> files, IDictionary<string, string> requestFormValues);
    }
}
