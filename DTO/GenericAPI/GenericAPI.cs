using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.GenericAPI
{
    public class APIGatewayGETRequest
    {
        public string RequestUrl { get; set; }
        public int Client { get; set; }
        public bool IsGenericToken { get; set; }
        public string Token { get; set; }
    }

    public class APIGatewayPOSTRequest
    {
        public string RequestUrl { get; set; }
        public dynamic RequestData { get; set; }
        public int Client { get; set; }
        public bool IsGenericToken { get; set; }
        public string Token { get; set; }
    }

    public class APIGatewayPUTRequest
    {
        public string RequestUrl { get; set; }
        public int Client { get; set; }
        public bool IsGenericToken { get; set; }
        public string Token { get; set; }
    }

    public class GenericAPIDELETERequest
    {
        public string RequestUrl { get; set; }
        public string RequestBase { get; set; }
        public string Token { get; set; }
        public string Client { get; set; }
    }

    public class GenericFileUploadRequest
    {
        public int Id { get; set; }
        public string RequestUrl { get; set; }
        public string Token { get; set; }
        public List<IFormFile> Files { get; set; }
        public int Client { get; set; }
    }

    public enum ClientEnum
    {
        TCF = 1,
        FullBridge = 2
    }
}
